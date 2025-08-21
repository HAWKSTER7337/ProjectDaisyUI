using Daily3_UI.Classes;
using Daily3_UI.Clients;

namespace Daily3_UI.Pages;

public partial class WinningNumbersPageDaily4 : ContentPage
{
    public WinningNumbersPageDaily4()
    {
        InitializeComponent();
    }

    private WinningNumberDaily4 YesterdayMidday { get; set; }
    private WinningNumberDaily4 YesterdayEvening { get; set; }
    private WinningNumberDaily4 TodayMidday { get; set; }
    private WinningNumberDaily4 TodayEvening { get; set; }

    private bool _isLoading = true;

    public bool IsLoading
    {
        get => _isLoading;
        private set
        {
            _isLoading = value;

            if (_isLoading)
            {
                LoadingIndicator.IsVisible = true;
                ErrorContainer.IsVisible = false;
                TodayLabel.IsVisible = false;
                TodayMiddayBorder.IsVisible = false;
                TodayEveningBorder.IsVisible = false;
                YesterdayLabel.IsVisible = false;
                YesterdayMiddayBorder.IsVisible = false;
                YesterdayEveningBorder.IsVisible = false;
            }
            else
            {
                LoadingIndicator.IsVisible = false;
                ErrorContainer.IsVisible = false;
                TodayLabel.IsVisible = true;
                TodayMiddayBorder.IsVisible = true;
                TodayEveningBorder.IsVisible = true;
                YesterdayLabel.IsVisible = true;
                YesterdayMiddayBorder.IsVisible = true;
                YesterdayEveningBorder.IsVisible = true;
            }
        }
    }

    private void ShowError()
    {
        LoadingIndicator.IsVisible = false;
        ErrorContainer.IsVisible = true;
        TodayLabel.IsVisible = false;
        TodayMiddayBorder.IsVisible = false;
        TodayEveningBorder.IsVisible = false;
        YesterdayLabel.IsVisible = false;
        YesterdayMiddayBorder.IsVisible = false;
        YesterdayEveningBorder.IsVisible = false;
    }

    protected override async void OnAppearing()
    {
        await UpdatePage();
    }

    private async void FlipToOtherRaffle(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("WinningNumbers3");
    }

    /// <summary>
    ///     Updates all the values to what they are stored as
    /// </summary>
    private async Task UpdatePage()
    {
        IsLoading = true;
        ClearWinningNumbers();
        
        try
        {
            var winningNumbers = await WinningNumbersClient.GetWinningNumbersDaily4();

            if (winningNumbers == null || winningNumbers.Count == 0)
            {
                ShowError();
                return;
            }

            switch (winningNumbers.Count)
            {
                case 4:
                    TodayEvening = winningNumbers[3];
                    TodayEvening1.Text = TodayEvening.Number1.ToString();
                    TodayEvening2.Text = TodayEvening.Number2.ToString();
                    TodayEvening3.Text = TodayEvening.Number3.ToString();
                    TodayEvening4.Text = TodayEvening.Number4.ToString();
                    goto case 3;
                case 3:
                    TodayMidday = winningNumbers[2];
                    TodayMidday1.Text = TodayMidday.Number1.ToString();
                    TodayMidday2.Text = TodayMidday.Number2.ToString();
                    TodayMidday3.Text = TodayMidday.Number3.ToString();
                    TodayMidday4.Text = TodayMidday.Number4.ToString();
                    goto case 2;
                case 2:
                    YesterdayEvening = winningNumbers[1];
                    YesterdayEvening1.Text = YesterdayEvening.Number1.ToString();
                    YesterdayEvening2.Text = YesterdayEvening.Number2.ToString();
                    YesterdayEvening3.Text = YesterdayEvening.Number3.ToString();
                    YesterdayEvening4.Text = YesterdayEvening.Number4.ToString();
                    goto case 1;
                case 1:
                    YesterdayMidday = winningNumbers[0];
                    YesterdayMidday1.Text = YesterdayMidday.Number1.ToString();
                    YesterdayMidday2.Text = YesterdayMidday.Number2.ToString();
                    YesterdayMidday3.Text = YesterdayMidday.Number3.ToString();
                    YesterdayMidday4.Text = YesterdayMidday.Number4.ToString();
                    break;
            }

            IsLoading = false;
        }
        catch (Exception ex)
        {
            // Log the exception if you have logging configured
            System.Diagnostics.Debug.WriteLine($"Error loading winning numbers: {ex.Message}");
            ShowError();
        }
    }

    private async void ToBuyTicketPage(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private async void RetryButton_Clicked(object sender, EventArgs e)
    {
        await UpdatePage();
    }

    private void ClearWinningNumbers()
    {
        YesterdayMidday1.Text = "-";
        YesterdayMidday2.Text = "-";
        YesterdayMidday3.Text = "-";
        YesterdayMidday4.Text = "-";

        YesterdayEvening1.Text = "-";
        YesterdayEvening2.Text = "-";
        YesterdayEvening3.Text = "-";
        YesterdayEvening4.Text = "-";

        TodayMidday1.Text = "-";
        TodayMidday2.Text = "-";
        TodayMidday3.Text = "-";
        TodayMidday4.Text = "-";

        TodayEvening1.Text = "-";
        TodayEvening2.Text = "-";
        TodayEvening3.Text = "-";
        TodayEvening4.Text = "-";
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
        YesterdayMidday4.FontSize = numberFontSize;
        YesterdayEvening1.FontSize = numberFontSize;
        YesterdayEvening2.FontSize = numberFontSize;
        YesterdayEvening3.FontSize = numberFontSize;
        YesterdayEvening4.FontSize = numberFontSize;
        TodayMidday1.FontSize = numberFontSize;
        TodayMidday2.FontSize = numberFontSize;
        TodayMidday3.FontSize = numberFontSize;
        TodayMidday4.FontSize = numberFontSize;
        TodayEvening1.FontSize = numberFontSize;
        TodayEvening2.FontSize = numberFontSize;
        TodayEvening3.FontSize = numberFontSize;
        TodayEvening4.FontSize = numberFontSize;
    }
}