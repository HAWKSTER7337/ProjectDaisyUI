using Daily3_UI.Classes;

namespace Daily3_UI.Pages;

public abstract class ChangeRaffle : ContentPage
{
    public List<KeyValuePair<string, ContentPage>> NewTaskBar;

    protected virtual void FlipToOtherRaffle(object sender, EventArgs e)
    {
        var tabBar = Shell.Current.Items.FirstOrDefault(item => item is TabBar) as TabBar;
        if (tabBar == null) return;

        tabBar.Items.Clear();

        for (var i = 0; i < NewTaskBar.Count; i++)
        {
            if (i == 3 && Globals.Status == 0) return;
            var i1 = i;
            tabBar.Items.Add(
                new ShellContent
                {
                    Title = NewTaskBar[i].Key,
                    ContentTemplate = new DataTemplate(() => NewTaskBar[i1].Value)
                }
            );
        }
    }
}