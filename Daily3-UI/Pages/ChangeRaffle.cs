using Daily3_UI.Classes;

namespace Daily3_UI.Pages;

public abstract class ChangeRaffle : ContentPage
{
    public List<KeyValuePair<string, Type>> NewTaskBar;

    protected virtual void FlipToOtherRaffle(object sender, EventArgs e)
{
    // Always make changes to UI in the main thread
    MainThread.BeginInvokeOnMainThread(() =>
    {
        var tabBar = Shell.Current.Items.FirstOrDefault(item => item is TabBar) as TabBar;
        if (tabBar == null) return;

        tabBar.Items.Clear();

        for (var i = 0; i < NewTaskBar.Count; i++)
        {
            // Skip tab at index 3 if Status is 0
            if (i == 3 && Globals.Status == 0)
                continue;

            var i1 = i; // capture index for closure

            tabBar.Items.Add(new ShellContent
            {
                Title = NewTaskBar[i].Key,
                ContentTemplate = new DataTemplate(() =>
                {
                    var pageType = NewTaskBar[i1].Value;
                    return Activator.CreateInstance(pageType) as Page;
                })
            });
        }
    });
}

}