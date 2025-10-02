using System.Transactions;
using Daily3_UI.Classes;
using Daily3_UI.Clients;
using Microsoft.Maui.Graphics;

namespace Daily3_UI.Pages.PagesDaily3;

public partial class WinningNumbersPage : ContentPage
{
    public WinningNumbersPage()
    {
        InitializeComponent();
    }

    private WinningNumberDaily3 YesterdayMidday { get; set; }
    private WinningNumberDaily3 YesterdayEvening { get; set; }
    private WinningNumberDaily3 TodayMidday { get; set; }
    private WinningNumberDaily3 TodayEvening { get; set; }

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

    /// <summary>
    ///     Updates all the values to what they are stored as
    /// </summary>
    private async Task UpdatePage()
    {
        IsLoading = true;
        ClearWinningNumbers();
        
        try
        {
            var winningNumbers = await WinningNumbersClient.GetWinningNumbersDaily3();

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

        YesterdayEvening1.Text = "-";
        YesterdayEvening2.Text = "-";
        YesterdayEvening3.Text = "-";

        TodayMidday1.Text = "-";
        TodayMidday2.Text = "-";
        TodayMidday3.Text = "-";

        TodayEvening1.Text = "-";
        TodayEvening2.Text = "-";
        TodayEvening3.Text = "-";
    }

    private void OnSizeChanged(object sender, EventArgs e)
    {
        try
        {
            if (Width <= 0 || Height <= 0 || double.IsNaN(Width) || double.IsNaN(Height))
            {
                return;
            }

            var screenWidth = Width;
            var screenHeight = Height;

            // Detect if we're on a tablet (larger screen)
            var isTablet = screenWidth >= 768 || screenHeight >= 1024;
            
            // Scale values for elements
            var borderPadding = screenWidth * 0.05;
            var labelFontSize = screenWidth * 0.05;
            var numberFontSize = screenWidth * 0.07;
            var imageHeight = screenHeight * 0.1;
            var borderBallHeight = screenHeight * (3.0 / 7);
            var groupingNumberBorderHeight = screenHeight * 0.19;

            // Ball border scaling for tablets
            var ballSize = isTablet ? Math.Max(80, screenWidth * 0.12) : 60;
            var ballRadius = isTablet ? Math.Max(120, ballSize * 1.5) : 100;
            var ballMargin = isTablet ? Math.Max(8, screenWidth * 0.015) : 5;

            // Adjust elements accordingly
            if (Daily3TitleImage != null)
                Daily3TitleImage.HeightRequest = imageHeight;

            if (YesterdayLabel != null)
                YesterdayLabel.FontSize = labelFontSize;
            if (TodayLabel != null)
                TodayLabel.FontSize = labelFontSize;

            if (YesterdayMiddayLabel != null)
                YesterdayMiddayLabel.FontSize = labelFontSize;
            if (YesterdayEveningLabel != null)
                YesterdayEveningLabel.FontSize = labelFontSize;
            if (TodayMiddayLabel != null)
                TodayMiddayLabel.FontSize = labelFontSize;
            if (TodayEveningLabel != null)
                TodayEveningLabel.FontSize = labelFontSize;

            if (YesterdayMiddayBorder != null)
                YesterdayMiddayBorder.HeightRequest = groupingNumberBorderHeight;
            if (YesterdayEveningBorder != null)
                YesterdayEveningBorder.HeightRequest = groupingNumberBorderHeight;
            if (TodayMiddayBorder != null)
                TodayMiddayBorder.HeightRequest = groupingNumberBorderHeight;
            if (TodayEveningBorder != null)
                TodayEveningBorder.HeightRequest = groupingNumberBorderHeight;

            UpdateBallBorder(YesterdayMidday1, ballSize, ballRadius, ballMargin);
            UpdateBallBorder(YesterdayMidday2, ballSize, ballRadius, ballMargin);
            UpdateBallBorder(YesterdayMidday3, ballSize, ballRadius, ballMargin);
            UpdateBallBorder(YesterdayEvening1, ballSize, ballRadius, ballMargin);
            UpdateBallBorder(YesterdayEvening2, ballSize, ballRadius, ballMargin);
            UpdateBallBorder(YesterdayEvening3, ballSize, ballRadius, ballMargin);
            UpdateBallBorder(TodayMidday1, ballSize, ballRadius, ballMargin);
            UpdateBallBorder(TodayMidday2, ballSize, ballRadius, ballMargin);
            UpdateBallBorder(TodayMidday3, ballSize, ballRadius, ballMargin);
            UpdateBallBorder(TodayEvening1, ballSize, ballRadius, ballMargin);
            UpdateBallBorder(TodayEvening2, ballSize, ballRadius, ballMargin);
            UpdateBallBorder(TodayEvening3, ballSize, ballRadius, ballMargin);

            // Keep number font sizes consistent
            if (YesterdayMidday1 != null)
                YesterdayMidday1.FontSize = numberFontSize;
            if (YesterdayMidday2 != null)
                YesterdayMidday2.FontSize = numberFontSize;
            if (YesterdayMidday3 != null)
                YesterdayMidday3.FontSize = numberFontSize;
            if (YesterdayEvening1 != null)
                YesterdayEvening1.FontSize = numberFontSize;
            if (YesterdayEvening2 != null)
                YesterdayEvening2.FontSize = numberFontSize;
            if (YesterdayEvening3 != null)
                YesterdayEvening3.FontSize = numberFontSize;
            if (TodayMidday1 != null)
                TodayMidday1.FontSize = numberFontSize;
            if (TodayMidday2 != null)
                TodayMidday2.FontSize = numberFontSize;
            if (TodayMidday3 != null)
                TodayMidday3.FontSize = numberFontSize;
            if (TodayEvening1 != null)
                TodayEvening1.FontSize = numberFontSize;
            if (TodayEvening2 != null)
                TodayEvening2.FontSize = numberFontSize;
            if (TodayEvening3 != null)
                TodayEvening3.FontSize = numberFontSize;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error in OnSizeChanged: {ex.Message}");
        }
    }

    private void UpdateBallBorder(Label numberLabel, double ballSize, double ballRadius, double ballMargin)
    {
        try
        {
            if (numberLabel?.Parent is Border ballBorder)
            {
                ballBorder.WidthRequest = ballSize;
                ballBorder.HeightRequest = ballSize;
                ballBorder.Margin = new Thickness(ballMargin, ballMargin, ballMargin, ballMargin);
                
                // Update the stroke shape for better tablet appearance
                // Use platform-safe approach for Android compatibility
                if (DeviceInfo.Platform == DevicePlatform.Android)
                {
                    // On Android, use a simpler approach or skip stroke shape updates
                    // The original Ball style will handle the appearance
                    return;
                }
                else
                {
                    // On iOS and other platforms, use the enhanced stroke shape
                    try
                    {
                        var strokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle
                        {
                            CornerRadius = new CornerRadius(ballRadius)
                        };
                        ballBorder.StrokeShape = strokeShape;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error updating stroke shape: {ex.Message}");
                        // Fallback: don't update stroke shape if it fails
                    }
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error in UpdateBallBorder: {ex.Message}");
        }
    }
}