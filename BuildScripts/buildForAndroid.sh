#!/bin/bash

# Check if an argument was provided
if [ -z "$1" ]; then
    echo "Usage: $0 <AVD_NAME>"
    echo "To get a device name run 'adb devices'"
    exit 1
fi

AVD_NAME="$1"

# Function to check if emulator is running
is_emulator_running() {
    pgrep -fl "emulator.*$AVD_NAME" > /dev/null
}

# Check for running emulator
if is_emulator_running; then
    echo "Emulator '$AVD_NAME' is already running."
else
    # Check for connected physical device
    if adb devices | grep -q "device$"; then
        echo "Physical Android device detected. Using it instead of emulator."
    else
        echo "Starting emulator '$AVD_NAME'..."
        ~/Library/Android/sdk/emulator/emulator -avd "$AVD_NAME" &
        echo "Waiting for emulator to boot..."
        adb wait-for-device
    fi
fi

# Change to your project directory
pushd ../Daily3-UI || { echo "Failed to change directory"; exit 1; }

# Build and run the .NET Android project
dotnet build -t:Run -f net8.0-android

# Return to old directory
popd

