using System;

namespace Daily3_UI;

public partial class MainPage : ContentPage
{
	public WinningNumber YesterdayMidday { get; private set; }
    public WinningNumber YesterdayEvening { get; private set; }
    public WinningNumber TodayMidday { get; private set; }
    public WinningNumber TodayEvening { get; private set; }

    public MainPage()
	{
        InitializeComponent();
        UpdatePage();
    }

    /// <summary>
    /// Updates all of the values to what they are stored as
    /// </summary>
    private async void UpdatePage()
    {
        List<WinningNumber> winningNumbers = await WebScraper.GetWinningNumbers();

        YesterdayMidday = winningNumbers[0];
        yesterdayMidday1.Text = YesterdayMidday.Number1.ToString();
        yesterdayMidday2.Text = YesterdayMidday.Number2.ToString();
        yesterdayMidday3.Text = YesterdayMidday.Number3.ToString();

        YesterdayEvening = winningNumbers[1];
        yesterdayEvening1.Text = YesterdayEvening.Number1.ToString();
        yesterdayEvening2.Text = YesterdayEvening.Number2.ToString();
        yesterdayEvening3.Text = YesterdayEvening.Number3.ToString();

        if (winningNumbers.Count > 2)
        {
            TodayMidday = winningNumbers[2];
            todayMidday1.Text = TodayMidday.Number1.ToString();
            todayMidday2.Text = TodayMidday.Number2.ToString();
            todayMidday3.Text = TodayMidday.Number3.ToString();
        }
        if (winningNumbers.Count > 3)
        {
            TodayEvening = winningNumbers[3];
            todayEvening1.Text = TodayEvening.Number1.ToString();
            todayEvening2.Text = TodayEvening.Number2.ToString();
            todayEvening3.Text = TodayEvening.Number3.ToString();
        }
    }
}

