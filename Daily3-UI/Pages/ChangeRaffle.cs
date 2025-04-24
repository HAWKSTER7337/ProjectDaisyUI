using Daily3_UI.Classes;
using Daily3_UI.Pages;

namespace Daily3_UI.Pages;

public abstract class ChangeRaffle : ContentPage
{
    public List<KeyValuePair<string, ContentPage>> NewTaskBar;

    protected virtual void FlipToOtherRaffle(object sender, EventArgs e)
    {
        // Ensure NewTaskBar is correctly populated (debugging step)
        if (NewTaskBar == null || NewTaskBar.Count == 0)
        {
            Console.WriteLine("NewTaskBar is null or empty.");
            return;
        }

        // Get the current TabBar from the Shell
        var currentTabBar = Shell.Current.Items.FirstOrDefault(item => item is TabBar) as TabBar;
    
        // If no TabBar is found, exit early
        if (currentTabBar == null) return;

        // Remove the existing TabBar
        Shell.Current.Items.Remove(currentTabBar);

        // Create a new TabBar to replace the old one
        var newTabBar = new TabBar();

        // Loop through your NewTaskBar and add ShellContent dynamically
        for (var i = 0; i < NewTaskBar.Count; i++)
        {
            // Check if the status is 0 and skip index 3 (or add any other condition you need)
            if (i == 3 && Globals.Status == 0) break;

            KeyValuePair<string, ContentPage> kvp = NewTaskBar[i];

            // Ensure kvp.Value (ContentPage) is not null
            if (kvp.Value == null)
            {
                Console.WriteLine($"Skipping null page at index {i}.");
                continue; // Skip null pages
            }

            // Create a new ShellContent
            ShellContent shellContent = new ShellContent
            {
                Title = kvp.Key,
                Content = kvp.Value, // Set the page content
                Route = kvp.Key.Replace(" ", "") // Optional: clean up route
            };

            // Add the new ShellContent to the new TabBar
            newTabBar.Items.Add(shellContent);
        }

        // Add the new TabBar to the Shell
        Shell.Current.Items.Add(newTabBar);

        // Optionally refresh or close flyout if needed
        Shell.Current.FlyoutIsPresented = false; // Close flyout if it was opened
    }


}