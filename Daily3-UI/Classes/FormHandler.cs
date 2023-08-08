using Microsoft.Maui.Handlers;
#if IOS
using UIKit;
#endif

namespace Daily3_UI;

public static class FormHandler
{
    public static void RemoveBorders()
    {
        EntryHandler.Mapper.AppendToMapping("Borderless", (handler, view) =>
        {
#if ANDROID
            handler.PlatformView.Background = null;
            handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
#elif IOS
            handler.PlatformView.BackgroundColor = UIColor.Clear;
            handler.PlatformView.Layer.BorderWidth = 0;
            handler.PlatformView.BorderStyle = UITextBorderStyle.None;
#endif
        });

        PickerHandler.Mapper.AppendToMapping("Borderless", (handler, view) =>
        {
#if ANDROID
            handler.PlatformView.Background = null;
            handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
#elif IOS
            handler.PlatformView.BackgroundColor = UIColor.Clear;
            handler.PlatformView.Layer.BorderWidth = 0;
            handler.PlatformView.BorderStyle = UITextBorderStyle.None;
#endif
        });
    }
}