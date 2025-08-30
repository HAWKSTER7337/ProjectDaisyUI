#!/bin/bash

# iOS Build Script for Physical Device
# Usage: ./buildForIOSDevice.sh [device_name]

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

# Function to list available devices
list_devices() {
    print_status "Available iOS devices:"
    echo ""
    xcrun xctrace list devices | grep -E "(iPhone|iPad)" | while read -r line; do
        if [[ $line =~ ([0-9A-F]{40}) ]]; then
            device_id="${BASH_REMATCH[1]}"
            device_name=$(echo "$line" | sed 's/.*"\([^"]*\)".*/\1/')
            echo "  $device_name ($device_id)"
        fi
    done
    echo ""
}

# Function to validate device name
validate_device() {
    local device_name="$1"
    
    if [[ -z "$device_name" ]]; then
        return 1
    fi
    
    # Check if device exists in the list
    if xcrun xctrace list devices | grep -q "$device_name"; then
        return 0
    else
        return 1
    fi
}

# Main script logic
main() {
    print_status "iOS Build Script for Physical Device"
    echo ""
    
    # Check if we're on macOS
    if [[ "$OSTYPE" != "darwin"* ]]; then
        print_error "This script must be run on macOS"
        exit 1
    fi
    
    # Check if Xcode command line tools are available
    if ! command -v xcrun &> /dev/null; then
        print_error "Xcode command line tools not found. Please install Xcode first."
        exit 1
    fi
    
    # Check if we're in the correct directory
    if [[ ! -f "Daily3-UI/Daily3-UI.csproj" ]]; then
        print_error "Please run this script from the project root directory (where Daily3-UI.csproj is located)"
        exit 1
    fi
    
    # Get device name from command line argument
    DEVICE_NAME="$1"
    
    # If no device name provided, show available devices and exit
    if [[ -z "$DEVICE_NAME" ]]; then
        print_warning "No device name provided"
        echo ""
        print_status "To find available devices, run:"
        echo "  xcrun xctrace list devices"
        echo ""
        print_status "Available devices:"
        list_devices
        print_status "Usage: $0 <device_name>"
        print_status "Example: $0 \"iPhone 15 Pro\""
        exit 1
    fi
    
    # Validate device name
    print_status "Validating device: $DEVICE_NAME"
    if ! validate_device "$DEVICE_NAME"; then
        print_error "Device '$DEVICE_NAME' not found"
        echo ""
        print_status "Available devices:"
        list_devices
        print_status "Please use one of the device names from the list above"
        exit 1
    fi
    
    print_success "Device '$DEVICE_NAME' found"
    echo ""
    
    # Build the project
    print_status "Building project for iOS device..."
    print_status "Target Framework: net8.0-ios"
    print_status "Runtime: ios-arm64"
    print_status "Device: $DEVICE_NAME"
    echo ""
    
    # Execute the build command
    dotnet build -t:Run -f net8.0-ios -p:_DeviceName="$DEVICE_NAME" -p:RuntimeIdentifier=ios-arm64
    
    # Check build result
    if [[ $? -eq 0 ]]; then
        print_success "Build completed successfully!"
        print_status "The app should now be running on your device: $DEVICE_NAME"
    else
        print_error "Build failed!"
        print_status "Please check the error messages above and try again"
        exit 1
    fi
}

# Run the main function with all arguments
main "$@"
