using System;

namespace Daily3_UI;

public partial class MainPage : ContentPage
{
	public WinningNumber YesterdayMidday { get; } = new WinningNumber(1,8,5);
    public WinningNumber YesterdayEvening { get; } = new WinningNumber(8,2,5);
    public WinningNumber TodayMidday { get; } = new WinningNumber(8, 2, 7);
    public WinningNumber TodayEvening { get; } = new WinningNumber(7, 2, 6);

    public MainPage()
	{
		InitializeComponent();
        UpdatePage();
    }

    /// <summary>
    /// Updates all of the values to what they are stored as
    /// </summary>
    async private void UpdatePage()
    {
        Task InnerFunction()
        {
            yesterdayMidday1.Text = YesterdayMidday.Number1.ToString();
            yesterdayMidday2.Text = YesterdayMidday.Number2.ToString();
            yesterdayMidday3.Text = YesterdayMidday.Number3.ToString();

            yesterdayEvening1.Text = YesterdayEvening.Number1.ToString();
            yesterdayEvening2.Text = YesterdayEvening.Number2.ToString();
            yesterdayEvening3.Text = YesterdayEvening.Number3.ToString();

            todayMidday1.Text = TodayMidday.Number1.ToString();
            todayMidday2.Text = TodayMidday.Number2.ToString();
            todayMidday3.Text = TodayMidday.Number3.ToString();

            todayEvening1.Text = TodayEvening.Number1.ToString();
            todayEvening2.Text = TodayEvening.Number2.ToString();
            todayEvening3.Text = TodayEvening.Number3.ToString();
            return Task.CompletedTask;
        }
        await InnerFunction();
    }
}

