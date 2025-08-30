using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Platform;
#if IOS
using UIKit;
using System.Runtime.Versioning;
#endif
#if ANDROID
using Android.Views.InputMethods;
using Android.Content;
#endif

namespace Daily3_UI.Classes
{
    public static class KeyboardHelper
    {
        public static void HideKeyboard()
        {
#if ANDROID
            var context = Platform.CurrentActivity;
            var inputMethodManager = context.GetSystemService(Context.InputMethodService) as InputMethodManager;
            var currentFocus = context.CurrentFocus ?? context.Window.DecorView;
            inputMethodManager?.HideSoftInputFromWindow(currentFocus?.WindowToken, HideSoftInputFlags.None);
#elif IOS
            if (OperatingSystem.IsIOSVersionAtLeast(13))
            {
                // Use modern Scene-based API for iOS 13+
                var window = UIApplication.SharedApplication
                   .ConnectedScenes
                   .OfType<UIWindowScene>()
                   .SelectMany(scene => scene.Windows)
                   .FirstOrDefault(window => window.IsKeyWindow);

                window?.EndEditing(true);
            }
            else
            {
                // Fallback for older iOS versions
                UIApplication.SharedApplication.SendAction(new ObjCRuntime.Selector("resignFirstResponder"), null, null, null);
            }
#endif
        }
    }
}