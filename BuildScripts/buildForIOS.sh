#!/bin/bash

# Build and Run .NET MAUI App on iPhone Simulator
# This script builds the Daily3-UI app and launches it on iOS Simulator
# Updated for iOS 18.5/18.6 compatibility

set -e  # Exit on any error

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Function to print colored output
print_status() {
    echo -e "${BLUE}[INFO]${NC} $1"
}

print_success() {
    echo -e "${GREEN}[SUCCESS]${NC} $1"
}

print_warning() {
    echo -e "${YELLOW}[WARNING]${NC} $1"
}

print_error() {
    echo -e "${RED}[ERROR]${NC} $1"
}

# Function to check if command exists
command_exists() {
    command -v "$1" >/dev/null 2>&1
}

# Function to check iOS SDK version
check_ios_sdk() {
    print_status "Checking iOS SDK version..."
    
    if ! command_exists "xcodebuild"; then
        print_error "Xcode command line tools not found. Please install Xcode from the App Store."
        exit 1
    fi
    
    # Get iOS SDK version
    local ios_sdk=$(xcodebuild -showsdks | grep "iOS" | grep -E "[0-9]+\.[0-9]+" | head -1 | sed -E 's/.*iOS ([0-9]+\.[0-9]+).*/\1/')
    
    if [ -n "$ios_sdk" ]; then
        print_success "iOS SDK version: $ios_sdk"
        
        # Check if SDK is compatible with our project
        if [[ "$ios_sdk" == "18."* ]]; then
            print_success "iOS SDK is compatible with project configuration (iOS 18.0+)"
        else
            print_warning "iOS SDK version $ios_sdk detected. Project supports iOS 18.0+"
        fi
    else
        print_warning "Could not determine iOS SDK version"
    fi
}

# Function to check if iOS Simulator is available
check_ios_simulator() {
    if ! command_exists "xcrun"; then
        print_error "Xcode command line tools not found. Please install Xcode from the App Store."
        exit 1
    fi
    
    if ! xcrun simctl list devices >/dev/null 2>&1; then
        print_error "iOS Simulator not available. Please ensure Xcode is properly installed."
        exit 1
    fi
}

# Function to get available iOS simulator runtimes
get_available_runtimes() {
    print_status "Available iOS Simulator Runtimes:"
    xcrun simctl list runtimes | grep "iOS" | while read -r line; do
        if [[ $line =~ iOS-([0-9]+)-([0-9]+) ]]; then
            version="${BASH_REMATCH[1]}.${BASH_REMATCH[2]}"
            print_status "  iOS $version"
        fi
    done
    echo ""
}

# Function to get available iOS simulators
get_ios_simulators() {
    print_status "Available iOS Simulators:"
    xcrun simctl list devices | grep "iPhone\|iPad" | grep "Booted\|Shutdown" | head -15
    echo ""
}

# Function to check if any simulator is already running
check_running_simulator() {
    local running_simulator=$(xcrun simctl list devices | grep "iPhone\|iPad" | grep "Booted" | head -1)
    
    if [ -n "$running_simulator" ]; then
        local simulator_name=$(echo "$running_simulator" | sed -E 's/.*"([^"]+)".*/\1/')
        local simulator_id=$(echo "$running_simulator" | sed -E 's/.*\(([A-F0-9-]+)\).*/\1/')
        print_success "Found running simulator: $simulator_name (ID: $simulator_id)"
        return 0
    else
        return 1
    fi
}

# Function to boot iOS simulator if not running
boot_simulator() {
    print_status "Checking iOS Simulator status..."
    
    # Check if a simulator is already running
    if check_running_simulator; then
        print_status "Using existing running simulator"
        return 0
    fi
    
    # Get available iPhone simulators with their runtime versions
    print_status "Looking for available iPhone simulators..."
    
    # Get the first available iPhone simulator (prefer newer iOS versions)
    local simulator_info=$(xcrun simctl list devices | grep "iPhone" | grep "Shutdown" | head -5)
    
    if [ -z "$simulator_info" ]; then
        print_error "No available iPhone simulators found."
        print_status "Available simulators:"
        xcrun simctl list devices | grep "iPhone\|iPad" | grep "Shutdown" | head -10
        exit 1
    fi
    
    # Parse simulator info to get ID and name
    local simulator_id=$(echo "$simulator_info" | sed -E 's/.*\(([A-F0-9-]+)\).*/\1/')
    local simulator_name=$(echo "$simulator_info" | sed -E 's/.*"([^"]+)".*/\1/')
    
    if [ -z "$simulator_id" ]; then
        print_error "Could not parse simulator ID from: $simulator_info"
        exit 1
    fi
    
    print_status "Booting iPhone simulator: $simulator_name (ID: $simulator_id)"
    xcrun simctl boot "$simulator_id"
    
    # Wait for simulator to boot
    print_status "Waiting for simulator to boot..."
    sleep 8
    
    # Verify simulator is running
    if xcrun simctl list devices | grep "$simulator_id" | grep -q "Booted"; then
        print_success "Simulator booted successfully!"
    else
        print_error "Failed to boot simulator"
        exit 1
    fi
    
    # Open Simulator app
    print_status "Opening iOS Simulator..."
    open -a Simulator
}

# Function to build the app
build_app() {
    print_status "Building Daily3-UI app for iOS..."
    
    # Navigate to project directory (we're already in the project root)
    cd "Daily3-UI"
    
    # Clean previous builds if requested
    if [ "$CLEAN_BUILD" = true ]; then
        print_status "Cleaning previous builds..."
        dotnet clean
    fi
    
    # Restore packages
    print_status "Restoring NuGet packages..."
    dotnet restore
    
    # Build for iOS with specific runtime identifier
    print_status "Building for iOS (iossimulator-arm64)..."
    dotnet build -f net8.0-ios -p:RuntimeIdentifier=iossimulator-arm64
    
    if [ $? -eq 0 ]; then
        print_success "Build completed successfully!"
    else
        print_error "Build failed!"
        exit 1
    fi
}

# Function to install and run the app on simulator
install_and_run_app() {
    print_status "Installing and running app on iOS Simulator..."
    
    # Check if app bundle exists
    local app_bundle="bin/Debug/net8.0-ios/iossimulator-arm64/Daily3-UI.app"
    if [ ! -d "$app_bundle" ]; then
        print_error "App bundle not found: $app_bundle"
        print_status "Please run the build first."
        exit 1
    fi
    
    # Get the booted simulator ID
    local simulator_id=$(xcrun simctl list devices | grep "Booted" | head -1 | sed -E 's/.*\(([A-F0-9-]+)\).*/\1/')
    
    if [ -z "$simulator_id" ]; then
        print_error "No booted simulator found"
        exit 1
    fi
    
    # Install the app
    print_status "Installing app on simulator..."
    xcrun simctl install "$simulator_id" "$app_bundle"
    
    if [ $? -eq 0 ]; then
        print_success "App installed successfully!"
    else
        print_error "Failed to install app on simulator!"
        exit 1
    fi
    
    # Launch the app
    print_status "Launching app..."
    xcrun simctl launch "$simulator_id" com.alexhawkins.daily3
    
    if [ $? -eq 0 ]; then
        print_success "App launched successfully on iOS Simulator!"
        print_status "The app should now be visible on your simulator."
    else
        print_error "Failed to launch app on simulator!"
        exit 1
    fi
}

# Function to show usage
show_usage() {
    echo "Usage: $0 [OPTIONS]"
    echo ""
    echo "Options:"
    echo "  -h, --help          Show this help message"
    echo "  -b, --build-only    Only build the app, don't run"
    echo "  -s, --simulator     Show available simulators"
    echo "  -r, --runtimes      Show available iOS runtimes"
    echo "  -c, --clean         Clean build artifacts before building"
    echo "  -i, --install       Install and run the built app"
    echo ""
    echo "Examples:"
    echo "  $0                  Build and run on iPhone simulator"
    echo "  $0 --build-only     Only build the app"
    echo "  $0 --simulator      Show available simulators"
    echo "  $0 --runtimes       Show available iOS runtimes"
    echo "  $0 --clean          Clean and rebuild"
    echo "  $0 --install        Install and run the built app"
}

# Main script
main() {
    print_status "Daily3-UI iOS Build Script"
    print_status "=========================="
    
    # Parse command line arguments
    BUILD_ONLY=false
    SHOW_SIMULATORS=false
    SHOW_RUNTIMES=false
    CLEAN_BUILD=false
    INSTALL_ONLY=false
    
    while [[ $# -gt 0 ]]; do
        case $1 in
            -h|--help)
                show_usage
                exit 0
                ;;
            -b|--build-only)
                BUILD_ONLY=true
                shift
                ;;
            -s|--simulator)
                SHOW_SIMULATORS=true
                shift
                ;;
            -r|--runtimes)
                SHOW_RUNTIMES=true
                shift
                ;;
            -c|--clean)
                CLEAN_BUILD=true
                shift
                ;;
            -i|--install)
                INSTALL_ONLY=true
                shift
                ;;
            *)
                print_error "Unknown option: $1"
                show_usage
                exit 1
                ;;
        esac
    done
    
    # Check if we're on macOS
    if [[ "$OSTYPE" != "darwin"* ]]; then
        print_error "This script is designed for macOS only."
        exit 1
    fi
    
    # Check prerequisites
    if ! command_exists "dotnet"; then
        print_error ".NET SDK not found. Please install .NET 8.0 or later."
        exit 1
    fi
    
    # Check .NET version
    DOTNET_VERSION=$(dotnet --version)
    print_status "Using .NET version: $DOTNET_VERSION"
    
    # Check iOS SDK version
    check_ios_sdk
    
    # Check if we're in the right directory
    if [ ! -f "Daily3-UI/Daily3-UI.csproj" ]; then
        print_error "Daily3-UI.csproj not found. Please run this script from the project root directory."
        exit 1
    fi
    
    # Show runtimes if requested
    if [ "$SHOW_RUNTIMES" = true ]; then
        check_ios_simulator
        get_available_runtimes
        exit 0
    fi
    
    # Show simulators if requested
    if [ "$SHOW_SIMULATORS" = true ]; then
        check_ios_simulator
        get_ios_simulators
        exit 0
    fi
    
    # Check iOS simulator availability
    check_ios_simulator
    
    # Show available runtimes for debugging
    get_available_runtimes
    
    # If install only, skip building
    if [ "$INSTALL_ONLY" = true ]; then
        install_and_run_app
        exit 0
    fi
    
    # Boot simulator if needed
    boot_simulator
    
    # Build the app
    build_app
    
    # Install and run the app if not build-only
    if [ "$BUILD_ONLY" = false ]; then
        install_and_run_app
    else
        print_success "Build completed. Use '$0 --install' to install and run the app on simulator."
    fi
}

# Run main function
main "$@"
