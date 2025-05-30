using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Views;
using Daily3_UI.Classes;

namespace Daily3_UI.Pages;

public partial class DatePopUpPage : Popup
{
    public DatePopUpPage(MonthAndDay monthAndDay)
    {
        MonthAndDay = monthAndDay;
        InitializeComponent();
        InitMonths();
        GetDaysInMonth();
        MonthAndDay = monthAndDay;
        BindingContext = this;
    }
    
    private MonthAndDay MonthAndDay { get; set; }

    private List<int> _months = new();
    private List<int> _daysInMonth = new();
    
    public List<string> Months => _months.Select(month => month.ToString()).ToList();
    public List<string> DaysInMonth => _daysInMonth.Select(day => day.ToString()).ToList();

    public int Month
    {
        get => MonthAndDay.Month;
        set
        {
            if (MonthAndDay.Month == value) return;
            MonthAndDay.Month = value;
            OnPropertyChanged(nameof(MonthString));
            OnPropertyChanged();
            GetDaysInMonth();
            MakeSureDayIsInMonth();
        }
    }

    public int Day
    {
        get => MonthAndDay.Day;
        set
        {
            if (MonthAndDay.Day == value) return;
            MonthAndDay.Day = value;
            OnPropertyChanged(nameof(DayString));
            OnPropertyChanged();
        }
    }

    public string MonthString
    {
        get => Month.ToString();

        set => Month = Convert.ToInt32(value);
    }

    public string DayString
    {
        get => Day.ToString();
        
        set => Day = Convert.ToInt32(value);
    }

    private void DayPickerFocused(object sender, FocusEventArgs e)
    {
        GetDaysInMonth();
        OnPropertyChanged(nameof(DaysInMonth));
    }

    private void MakeSureDayIsInMonth()
    {
        if (_daysInMonth.Contains(Day)) return;
        Day = Day > _daysInMonth.Last() ? _daysInMonth.Last() : _daysInMonth.First();
    }

    private void GetDaysInMonth()
    {
        int days = DateTime.DaysInMonth(Globals.CurrentYear, Month);
        var firstDay = 1;

        if (Month == DateTime.Today.Month)
        {
            firstDay = DateTime.Today.Day;
        }

        _daysInMonth.Clear();
        for (int i = firstDay; i <= days; i++) _daysInMonth.Add(i);
        if (Day > _daysInMonth.Last()) Day = _daysInMonth.Last();
    }

    private void InitMonths()
    {
        var today = DateTime.Today.Month;
        for (int i = today; i <= 12; i++) _months.Add(i);
    }

    private async void OnExitClicked(object sender, EventArgs e)
    {
        await CloseAsync();
    }
}

public class MonthAndDay
{
    public int Day;
    public int Month;
}