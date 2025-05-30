using System.Diagnostics;
using Foundation;
using UIKit;

namespace Daily3_UI;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
    protected override MauiApp CreateMauiApp()
    {
        return MauiProgram.CreateMauiApp();
    }

    public override bool FinishedLaunching(UIApplication app, NSDictionary options)
    {
        // Set the font name - this must be the internal font name, not the filename
        var fontSize = 32f;
        var fontName = "Bebas"; // Replace with your actual font name

        var customFont = UIFont.FromName(fontName, fontSize);
        
        var attributes = new UIStringAttributes
        {
            Font = customFont
        };
        
        UITabBarItem.Appearance.SetTitleTextAttributes(attributes, UIControlState.Normal);
        UITabBarItem.Appearance.SetTitleTextAttributes(attributes, UIControlState.Selected);
        
        return base.FinishedLaunching(app, options);
    }
}