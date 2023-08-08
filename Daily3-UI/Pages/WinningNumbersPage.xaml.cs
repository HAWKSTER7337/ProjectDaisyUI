using Daily3_UI.Classes;

namespace Daily3_UI.Pages;

public partial class WinningNumbersPage : ContentPage
{
    public WinningNumbersPage()
    {
        InitializeComponent();
    }

    private WinningNumber YesterdayMidday { get; set; }
    private WinningNumber YesterdayEvening { get; set; }
    private WinningNumber TodayMidday { get; set; }
    private WinningNumber TodayEvening { get; set; }

    protected override async void OnAppearing()
    {
        await UpdatePage();
    }

    /// <summary>
    ///     Updates all of the values to what they are stored as
    /// </summary>
    private async Task UpdatePage()
    {
        var winningNumbers = await WinningNumbersClient.GetWinningNumbers();

        switch (winningNumbers.Count)
        {
            case 4:
                TodayEvening = winningNumbers[3];
                TodayEvening1.Text = TodayEvening.Number1.ToString();
                TodayEvening2.Text = TodayEvening.Number2.ToString();
                TodayEvening3.Text = TodayEvening.Number3.ToString();
                goto case 3;
            case 3:
                TodayMidday = winningNumbers[2];
                TodayMidday1.Text = TodayMidday.Number1.ToString();
                TodayMidday2.Text = TodayMidday.Number2.ToString();
                TodayMidday3.Text = TodayMidday.Number3.ToString();
                goto case 2;
            case 2:
                YesterdayEvening = winningNumbers[1];
                YesterdayEvening1.Text = YesterdayEvening.Number1.ToString();
                YesterdayEvening2.Text = YesterdayEvening.Number2.ToString();
                YesterdayEvening3.Text = YesterdayEvening.Number3.ToString();
                goto case 1;
            case 1:
                YesterdayMidday = winningNumbers[0];
                YesterdayMidday1.Text = YesterdayMidday.Number1.ToString();
                YesterdayMidday2.Text = YesterdayMidday.Number2.ToString();
                YesterdayMidday3.Text = YesterdayMidday.Number3.ToString();
                break;
        }
    }
}