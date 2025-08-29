#!/bin/bash

# Quick iOS Build and Run Script
# Simple script to build and run Daily3-UI on iPhone simulator
# Updated for iOS 18.5/18.6 compatibility

echo "🚀 Building Daily3-UI for iOS..."

# Check if simulator is already running
if xcrun simctl list devices | grep -q "Booted"; then
    echo "✅ Using existing running simulator"
else
    echo "📱 Booting iPhone simulator..."
    # Get first available iPhone simulator and boot it
    SIM_ID=$(xcrun simctl list devices | grep "iPhone" | grep "Shutdown" | head -1 | sed -E 's/.*\(([A-F0-9-]+)\).*/\1/')
    if [ -n "$SIM_ID" ]; then
        xcrun simctl boot "$SIM_ID"
        sleep 5
        open -a Simulator
    else
        echo "❌ No iPhone simulators found. Please create one in Xcode."
        exit 1
    fi
fi

# Navigate to project directory
cd "$(dirname "$0")/../Daily3-UI"

# Show available runtimes for debugging
echo "📋 Available iOS runtimes:"
xcrun simctl list runtimes | grep "iOS" | head -5

# Check iOS SDK version
echo "🔧 Checking iOS SDK version..."
IOS_SDK=$(xcodebuild -showsdks | grep "iOS" | grep -E "[0-9]+\.[0-9]+" | head -1 | sed -E 's/.*iOS ([0-9]+\.[0-9]+).*/\1/')
if [ -n "$IOS_SDK" ]; then
    echo "✅ iOS SDK version: $IOS_SDK"
else
    echo "⚠️  Could not determine iOS SDK version"
fi

# Build for iOS with specific runtime identifier
echo "🔨 Building for iOS (iossimulator-arm64)..."
dotnet build -f net8.0-ios -p:RuntimeIdentifier=iossimulator-arm64

if [ $? -eq 0 ]; then
    echo "✅ Build completed successfully!"
else
    echo "❌ Build failed! Check the error messages above."
    echo "💡 Try running './BuildScripts/buildForIOS.sh --runtimes' to see available iOS versions"
    exit 1
fi

# Check if app bundle exists
APP_BUNDLE="bin/Debug/net8.0-ios/iossimulator-arm64/Daily3-UI.app"
if [ ! -d "$APP_BUNDLE" ]; then
    echo "❌ App bundle not found: $APP_BUNDLE"
    exit 1
fi

# Get the booted simulator ID
SIM_ID=$(xcrun simctl list devices | grep "Booted" | head -1 | sed -E 's/.*\(([A-F0-9-]+)\).*/\1/')
if [ -z "$SIM_ID" ]; then
    echo "❌ No booted simulator found"
    exit 1
fi

# Install the app
echo "📲 Installing app on simulator..."
xcrun simctl install "$SIM_ID" "$APP_BUNDLE"

if [ $? -eq 0 ]; then
    echo "✅ App installed successfully!"
else
    echo "❌ Failed to install app on simulator!"
    exit 1
fi

# Launch the app
echo "🚀 Launching app..."
xcrun simctl launch "$SIM_ID" com.alexhawkins.daily3

if [ $? -eq 0 ]; then
    echo "✅ App launched successfully on iOS Simulator!"
    echo "📱 The app should now be visible on your simulator."
else
    echo "❌ Failed to launch app on simulator!"
    exit 1
fi
