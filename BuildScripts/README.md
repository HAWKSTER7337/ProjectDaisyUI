# iOS Build Scripts for Daily3-UI

This directory contains scripts to build and run the Daily3-UI .NET MAUI app on iOS Simulator.

## ⚠️ Important: iOS 18.5/18.6 Compatibility

The project has been updated to support **iOS 18.0+** and is fully compatible with:
- ✅ **iOS 18.5 SDK** (latest available)
- ✅ **iOS 18.6 Simulator** (latest simulator version)
- ✅ **Backward compatibility** with older iOS versions

The build scripts now include automatic iOS SDK version checking and compatibility validation.

## Prerequisites

Before using these scripts, ensure you have:

1. **macOS** - These scripts only work on Mac
2. **Xcode** - Install from the Mac App Store (latest version recommended)
3. **.NET 8.0 SDK** - Install from [dotnet.microsoft.com](https://dotnet.microsoft.com)
4. **iOS Simulator** - Comes with Xcode

## Scripts

### 1. `buildForIOS.sh` - Full-Featured Build Script

A comprehensive script with error handling, simulator management, and multiple options.

**Usage:**
```bash
# Build and run on iPhone simulator
./BuildScripts/buildForIOS.sh

# Only build (don't run)
./BuildScripts/buildForIOS.sh --build-only

# Install and run the built app (skip building)
./BuildScripts/buildForIOS.sh --install

# Show available simulators
./BuildScripts/buildForIOS.sh --simulator

# Show available iOS runtimes
./BuildScripts/buildForIOS.sh --runtimes

# Clean and rebuild
./BuildScripts/buildForIOS.sh --clean

# Show help
./BuildScripts/buildForIOS.sh --help
```

**Features:**
- ✅ **iOS 18.5/18.6 compatibility** - Automatic SDK version checking
- ✅ **Automatic simulator detection** and booting
- ✅ **Smart simulator reuse** - Uses running simulators when available
- ✅ **Runtime compatibility checking** - Shows available iOS versions
- ✅ **Proper app installation** - Uses simctl for reliable app deployment
- ✅ **Colored output** with status messages
- ✅ **Error handling** and validation
- ✅ **Multiple build options**
- ✅ **Prerequisites checking**

### 2. `quickIOS.sh` - Simple Quick Build

A minimal script for quick builds when you just want to run the app.

**Usage:**
```bash
./BuildScripts/quickIOS.sh
```

**Features:**
- ✅ **iOS 18.5/18.6 compatibility** - Automatic SDK version checking
- ✅ **Simple and fast**
- ✅ **Simulator detection** - Reuses running simulators
- ✅ **Runtime information** - Shows available iOS versions
- ✅ **Proper app installation** - Uses simctl for reliable app deployment
- ✅ **Minimal output**
- ✅ **Direct build and run**

## How to Use

### First Time Setup

1. **Make scripts executable** (if not already done):
   ```bash
   chmod +x BuildScripts/*.sh
   ```

2. **Run from project root**:
   ```bash
   cd /path/to/ProjectDaisyUI
   ./BuildScripts/buildForIOS.sh
   ```

### Daily Development

- **Quick testing**: Use `quickIOS.sh`
- **Full build with options**: Use `buildForIOS.sh`
- **Install only**: Use `buildForIOS.sh --install` (if already built)
- **Troubleshooting**: Use `buildForIOS.sh --simulator` to check simulators
- **Runtime issues**: Use `buildForIOS.sh --runtimes` to check iOS versions

## Troubleshooting

### Common Issues

1. **"Xcode command line tools not found"**
   - Install Xcode from the Mac App Store
   - Run: `xcode-select --install`

2. **"No available iPhone simulators found"**
   - Open Xcode → Window → Devices and Simulators
   - Add a new simulator if none exist

3. **".NET SDK not found"**
   - Install .NET 8.0 SDK from [dotnet.microsoft.com](https://dotnet.microsoft.com)

4. **Build fails with runtime errors**
   - Check available runtimes: `./BuildScripts/buildForIOS.sh --runtimes`
   - Ensure you have compatible iOS simulator versions
   - The script now shows available runtimes automatically

5. **"Could not find simulator runtime" errors**
   - This usually means the iOS version in your project doesn't match available simulators
   - Check runtimes: `./BuildScripts/buildForIOS.sh --runtimes`
   - Create a simulator with a compatible iOS version in Xcode

6. **iOS SDK compatibility warnings**
   - The script now automatically checks iOS SDK version
   - Ensure your Xcode is up to date (supports iOS 18.5+)
   - Project supports iOS 18.0+ (compatible with iOS 18.5/18.6)

### iOS Version Compatibility

The project has been updated with the following improvements:
- ✅ **iOS 18.0+ support** - Target framework updated
- ✅ **Automatic SDK detection** - Scripts check iOS SDK version
- ✅ **Runtime identifier specification** - Uses `iossimulator-arm64`
- ✅ **API compatibility fixes** - Updated for latest iOS features
- ✅ **Backward compatibility** - Works with older iOS versions

The scripts now automatically:
- ✅ **Detect iOS SDK version** and show compatibility status
- ✅ **Detect running simulators** and reuse them
- ✅ **Show available iOS runtimes** for debugging
- ✅ **Use proper build commands** with runtime identifiers
- ✅ **Install apps correctly** using simctl

### Simulator Management

- **List simulators**: `./BuildScripts/buildForIOS.sh --simulator`
- **Check runtimes**: `./BuildScripts/buildForIOS.sh --runtimes`
- **Boot specific simulator**: Use Xcode → Window → Devices and Simulators
- **Reset simulator**: Simulator → Device → Erase All Content and Settings

If you get runtime compatibility errors:
1. Run `./BuildScripts/buildForIOS.sh --runtimes` to see available versions
2. Create a simulator with a compatible iOS version in Xcode
3. The script will automatically use the best available simulator

## Build Process

The updated build process includes:

1. **iOS SDK Version Check** - Validates SDK compatibility
2. **Simulator Detection** - Finds and boots appropriate simulator
3. **Clean Build** - Ensures fresh build artifacts
4. **Runtime-Specific Build** - Uses `iossimulator-arm64` runtime
5. **App Installation** - Installs app bundle on simulator
6. **App Launch** - Launches app with proper bundle identifier

## Script Output

The full build script provides colored output:
- 🔵 **Blue**: Information messages
- 🟢 **Green**: Success messages  
- 🟡 **Yellow**: Warning messages
- 🔴 **Red**: Error messages

## File Structure

```
ProjectDaisyUI/
├── BuildScripts/
│   ├── buildForIOS.sh      # Full-featured build script
│   ├── quickIOS.sh         # Quick build script
│   └── README.md           # This file
└── Daily3-UI/              # Your .NET MAUI project
    └── Daily3-UI.csproj
```

## Notes

- Scripts automatically navigate to the correct project directory
- **iOS Simulator will be reused if already running** (no need to boot new ones)
- The app will launch on the first available iPhone simulator
- **iOS 18.5/18.6 compatibility** is now fully supported
- **Runtime compatibility is automatically checked** and displayed
- **iPhone simulators are preferred** over iPad simulators for better compatibility
- **Proper app installation** using simctl ensures reliable deployment
