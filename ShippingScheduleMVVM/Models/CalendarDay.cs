using ShippingScheduleMVVM.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

namespace ShippingScheduleMVVM.Models
{
    public class CalendarDay : ViewModelBase
    {
        private int _day;
        public int Day
        {
            get { return _day; }
            set
            {
                if (_day != value)
                {
                    _day = value;
                    OnPropertyChanged("Day");
                }
            }
        }
        private int _month;
        public int Month
        {
            get { return _month; }
            set
            {
                if (_month != value)
                {
                    _month = value;
                    OnPropertyChanged("Month");
                }
            }
        }
        private int _year;
        public int Year
        {
            get { return _year; }
            set
            {
                if (_year != value)
                {
                    _year = value;
                    OnPropertyChanged("Year");
                }
            }
        }
        private int _row;
        public int Row
        {
            get { return _row; }
            set
            {
                if (_row != value)
                {
                    _row = value;
                    OnPropertyChanged("Row");
                }
            }
        }

        private int _column;
        public int Column
        {
            get { return _column; }
            set
            {
                if (_column != value)
                {
                    _column = value;
                    OnPropertyChanged("Column");
                }
            }
        }
        private SolidColorBrush _borderColor { get; set; } = new SolidColorBrush(Colors.Transparent);
        public SolidColorBrush BorderColor
        {
            get
            { return _borderColor; }
            set
            {
                if (_borderColor != value)
                {
                    _borderColor = value;
                    OnPropertyChanged("borderColor");
                }
            }
        }
        private SolidColorBrush _dayColor { get; set; } = new SolidColorBrush(Colors.Black);
        public SolidColorBrush DayColor

        {
            get
            { return _dayColor; }
            set
            {
                if (_dayColor != value)
                {
                    _dayColor = value;
                    OnPropertyChanged("borderColor");
                }
            }
        }

        private SolidColorBrush _backgroundDayColor { get; set; } = new SolidColorBrush(Colors.Transparent);
        public SolidColorBrush BackgroundDayColor
        {
            get
            { return _backgroundDayColor; }
            set
            {
                if (_backgroundDayColor != value)
                {
                    _backgroundDayColor = value;
                    OnPropertyChanged("BackgroundDayColor");
                }
            }
        }
        private Thickness _borderThickness { get; set; } = new Thickness(1);
        public Thickness BorderThickness
        {
            get
            { return _borderThickness; }
            set
            {
                if (_borderThickness != value)
                {
                    _borderThickness = value;
                    OnPropertyChanged("BorderThickness");
                }
            }
        }
        public CalendarDay()
        {

        }
        public CalendarDay(int day, int row, int column, int month, int year, bool isCurrentMonth)
        {
            Day = day;
            Row = row;
            Column = column;
            Month = month;
            Year = year;

            BackgroundDayColor = new SolidColorBrush(Colors.Transparent);

            if (!isCurrentMonth)
            {
                DayColor = new SolidColorBrush(Color.FromRgb(186, 186, 186));
            }
            else
            {
                DayColor = new SolidColorBrush(Colors.Black);

                if (DateTime.Now.Date == new DateTime(year, month, day).Date)
                {
                    DayColor = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                    BackgroundDayColor = (SolidColorBrush)Application.Current.Resources["PrimaryThemeColor"];
                }
            }
        }
        public DateTime SelectDay()
        {
            DateTime selectedDate = new DateTime(Year, Month, Day);
            return selectedDate;
        }
        public static void SelectDayByDate(ObservableCollection<CalendarDay> CalendarDays, DateTime selectedDate)
        {
            foreach (CalendarDay calendarDay in CalendarDays)
            {
                calendarDay.BorderThickness = new Thickness(1);
                if (calendarDay.Day == selectedDate.Day && calendarDay.Month == selectedDate.Month)
                {
                    calendarDay.BorderColor = (SolidColorBrush)Application.Current.Resources["PrimaryThemeColor"];

                    if (DateTime.Now.Date == new DateTime(calendarDay.Year, calendarDay.Month, calendarDay.Day).Date)
                    {
                        calendarDay.BorderThickness = new Thickness(0);
                    }

                    return;
                }
            }
        }
        public static void SetDaysOfMonth(ObservableCollection<CalendarDay> calendarDays, DateTime selectedDate)
        {
            calendarDays.Clear();
            int month = selectedDate.Month;
            int year = selectedDate.Year;
            int row = 0;
            int count = 0;

            // Get the first day of the month and the last day of the previous month
            DateTime firstDayOfMonth = new(year, month, 1);
            DateTime lastDayOfPreviousMonth = firstDayOfMonth.AddDays(-1);

            // Calculate the number of days to add from the previous month
            int daysFromPreviousMonth = (int)lastDayOfPreviousMonth.DayOfWeek;

            if (lastDayOfPreviousMonth.DayOfWeek == DayOfWeek.Saturday)
            {
                daysFromPreviousMonth = -1;
            }

            // Get the number of days in the current month
            int daysInMonth = DateTime.DaysInMonth(year, month);

            // Calculate the number of days to add from the next month
            int daysFromNextMonth = 42 - (daysFromPreviousMonth + daysInMonth);

            // Add the days from the previous month
            for (int i = daysFromPreviousMonth; i >= 0; i--)
            {
                DateTime date = lastDayOfPreviousMonth.AddDays(-i);
                calendarDays.Add(new CalendarDay(date.Day, row, (int)date.DayOfWeek, date.Month, date.Year, false));
                count++;
                if (count == 7)
                {
                    count = 0;
                    row++;
                }
            }

            // Add the days from the current month
            for (int i = 1; i <= daysInMonth; i++)
            {
                DateTime date = lastDayOfPreviousMonth.AddDays(i);
                calendarDays.Add(new CalendarDay(i, row, (int)date.DayOfWeek, date.Month, date.Year, true));
                count++;
                if (count == 7)
                {
                    count = 0;
                    row++;
                }
            }

            // Add the days from the next month
            for (int i = 1; i < daysFromNextMonth; i++)
            {
                DateTime date = firstDayOfMonth.AddDays(daysInMonth + i - 1);
                calendarDays.Add(new CalendarDay(date.Day, row, (int)date.DayOfWeek, date.Month, date.Year, false));
                count++;
                if (count == 7)
                {
                    count = 0;
                    row++;
                }
            }
        }
    }
}
