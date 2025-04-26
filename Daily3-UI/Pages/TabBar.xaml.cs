using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace Daily3_UI.Pages;

public partial class DailyTabBar : ContentView
{
    
    public DailyTabBar(List<KeyValuePair<string, string>> tabs)
    {
        InitializeComponent();
        if (tabs.Count > 4) throw new ArgumentOutOfRangeException(nameof(tabs.Count),  "Tab count is greater than 4");
        List<Button> buttons = new()
        {
            Button1,
            Button2,
            Button3,
            Button4
        };
        
        switch (tabs.Count)
        {
            case 4:
                buttons[3].Text = tabs[3].Key;
                buttons[3].Command = new Command(() => LoadPage(tabs[3].Value));
                goto case 3;
            case 3:
                buttons[2].Text = tabs[2].Key;
                buttons[2].Command = new Command(() => LoadPage(tabs[2].Value));
                goto case 2;
            case 2:
                buttons[1].Text = tabs[1].Key;
                buttons[1].Command = new Command(() => LoadPage(tabs[1].Value));
                goto case 1;
            case 1:
                buttons[0].Text = tabs[0].Key;
                buttons[0].Command = new Command(() => LoadPage(tabs[0].Value));
                break;
        }
    }

    private void LoadPage(string pageRoute)
    {
        Shell.Current.GoToAsync($"{pageRoute}");
    }
    
}