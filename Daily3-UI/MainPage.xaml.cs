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
        YesterdayMidday1.Text = YesterdayMidday.Number1.ToString();
        YesterdayMidday2.Text = YesterdayMidday.Number2.ToString();
        YesterdayMidday3.Text = YesterdayMidday.Number3.ToString();

        YesterdayEvening = winningNumbers[1];
        YesterdayEvening1.Text = YesterdayEvening.Number1.ToString();
        YesterdayEvening2.Text = YesterdayEvening.Number2.ToString();
        YesterdayEvening3.Text = YesterdayEvening.Number3.ToString();

        if (winningNumbers.Count > 2)
        {
            TodayMidday = winningNumbers[2];
            TodayMidday1.Text = TodayMidday.Number1.ToString();
            TodayMidday2.Text = TodayMidday.Number2.ToString();
            TodayMidday3.Text = TodayMidday.Number3.ToString();
        }
        if (winningNumbers.Count > 3)
        {
            TodayEvening = winningNumbers[3];
            TodayEvening1.Text = TodayEvening.Number1.ToString();
            TodayEvening2.Text = TodayEvening.Number2.ToString();
            TodayEvening3.Text = TodayEvening.Number3.ToString();
        }
    }
}

