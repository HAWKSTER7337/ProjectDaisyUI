using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daily3_UI.Pages;

public partial class HouseAddUsersPage : ContentPage
{
    public HouseAddUsersPage()
    {
        InitializeComponent();
    }
    
    private async void OnHousePageButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}