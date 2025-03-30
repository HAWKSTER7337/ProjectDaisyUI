using Daily3_UI.Classes;
using Daily3_UI.Clients;

namespace Daily3_UI.Pages;

public partial class WinningNumbersPage : ChangeRaffle
{
    public WinningNumbersPage()
    {
        NewTaskBar = Globals.Daily4ContentPages;
        InitializeComponent();
    }

    private WinningNumberDaily3 YesterdayMidday { get; set; }
    private WinningNumberDaily3 YesterdayEvening { get; set; }
    private WinningNumberDaily3 TodayMidday { get; set; }
    private WinningNumberDaily3 TodayEvening { get; set; }

    protected override async void OnAppearing()
    {
        NewTaskBar = Globals.Daily4ContentPages;
        await UpdatePage();
    }

    /// <summary>
    ///     Updates all of the values to what they are stored as
    /// </summary>
    private async Task UpdatePage()
    {
        var winningNumbers = await WinningNumbersClient.GetWinningNumbersDaily3();

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

    private void OnSizeChanged(object sender, EventArgs e)
    {
        var screenWidth = Width;
        var screenHeight = Height;

        // Scale values for elements
        var borderPadding = screenWidth * 0.05;
        var labelFontSize = screenWidth * 0.05;
        var numberFontSize = screenWidth * 0.07;
        var imageHeight = screenHeight * 0.1;
        var borderBallHeight = screenHeight * (3.0 / 7);
        var groupingNumberBorderHeight = screenHeight * 0.19;

        // Adjust elements accordingly
        Daily3TitleImage.HeightRequest = imageHeight;

        YesterdayLabel.FontSize = labelFontSize;
        TodayLabel.FontSize = labelFontSize;

        YesterdayMiddayLabel.FontSize = labelFontSize;
        YesterdayEveningLabel.FontSize = labelFontSize;
        TodayMiddayLabel.FontSize = labelFontSize;
        TodayEveningLabel.FontSize = labelFontSize;

        YesterdayMiddayBorder.HeightRequest = groupingNumberBorderHeight;
        YesterdayEveningBorder.HeightRequest = groupingNumberBorderHeight;
        TodayMiddayBorder.HeightRequest = groupingNumberBorderHeight;
        TodayEveningBorder.HeightRequest = groupingNumberBorderHeight;

        YesterdayMidday1.FontSize = numberFontSize;
        YesterdayMidday2.FontSize = numberFontSize;
        YesterdayMidday3.FontSize = numberFontSize;
        YesterdayEvening1.FontSize = numberFontSize;
        YesterdayEvening2.FontSize = numberFontSize;
        YesterdayEvening3.FontSize = numberFontSize;
        TodayMidday1.FontSize = numberFontSize;
        TodayMidday2.FontSize = numberFontSize;
        TodayMidday3.FontSize = numberFontSize;
        TodayEvening1.FontSize = numberFontSize;
        TodayEvening2.FontSize = numberFontSize;
        TodayEvening3.FontSize = numberFontSize;
    }
}