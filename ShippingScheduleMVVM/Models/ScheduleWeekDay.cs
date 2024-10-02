using ShippingScheduleMVVM.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

namespace ShippingScheduleMVVM.Models
{
    public class ScheduleWeekDay : ViewModelBase
    {
        private int? _day;
        public int? Day
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
        private string? _weekDay;
        public string WeekDay
        {
            get { return _weekDay ?? ""; }
            set
            {
                if (_weekDay != value)
                {
                    _weekDay = value;
                    OnPropertyChanged("WeekDay");
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
        private SolidColorBrush _foregroundColor = new SolidColorBrush(Colors.Black);
        public SolidColorBrush ForegroundColor
        {
            get { return _foregroundColor; }
            set
            {
                if (_foregroundColor != value)
                {
                    _foregroundColor = value;
                    OnPropertyChanged("ForegroundColor");
                }
            }
        }
        private SolidColorBrush _backgroundColor = new SolidColorBrush(Color.FromRgb(249, 249, 249));
        public SolidColorBrush BackgroundColor
        {
            get { return _backgroundColor; }
            set
            {
                if (_backgroundColor != value)
                {
                    _backgroundColor = value;
                    OnPropertyChanged("BackgroundColor");
                }
            }
        }

        private SolidColorBrush _borderColor = new SolidColorBrush(Colors.LightGray);
        public SolidColorBrush BorderColor
        {
            get { return _borderColor; }
            set
            {
                if (_borderColor != value)
                {
                    _borderColor = value;
                    OnPropertyChanged("BorderColor");
                }
            }
        }
        private Visibility _visibility = Visibility.Visible;
        public Visibility Visibility
        {
            get { return _visibility; }
            set
            {
                if (_visibility != value)
                {
                    _visibility = value;
                    OnPropertyChanged("Visibility");
                }
            }
        }
        public ScheduleWeekDay()
        {

        }
        public static void ShowOnlySelectedDay(ObservableCollection<ScheduleWeekDay> weekDays, int dayOfTheWeek)
        {
            foreach (ScheduleWeekDay day in weekDays)
            {
                day.Visibility = Visibility.Collapsed;
                if (day.Column == dayOfTheWeek + 1)
                {
                    day.Visibility = Visibility.Visible;
                }
            }
        }
        public static void InitializeDaysOfWeek(ObservableCollection<ScheduleWeekDay> weekDays, DateTime selectedDate)
        {
            int count = 0;
            DateTime sunday = selectedDate.AddDays(-(int)selectedDate.DayOfWeek);

            while (count < 7)
            {
                ScheduleWeekDay day = new();

                DateTime newDate = sunday.AddDays(count);
                day.Day = newDate.Day;
                day.WeekDay = GetDayOfWeek((int)newDate.DayOfWeek);
                day.Column = count + 1;

                if (newDate.Day == DateTime.Now.Day && newDate.Month == DateTime.Now.Month && newDate.Year == DateTime.Now.Year)
                {
                    day.BorderColor = (SolidColorBrush)Application.Current.Resources["PrimaryThemeColor"];
                    day.BackgroundColor = (SolidColorBrush)Application.Current.Resources["PrimaryThemeColor"];
                    day.ForegroundColor = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                }
                else
                {
                    day.BorderColor = new SolidColorBrush(Colors.LightGray);
                    day.BackgroundColor = new SolidColorBrush(Color.FromRgb(247, 247, 247));
                    day.ForegroundColor = new SolidColorBrush(Colors.Black);
                }

                weekDays.Add(day);
                count++;
            }
        }
        public static string GetDayOfWeek(int number)
        {
            string[] weekDays = new string[] { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
            return weekDays[number];
        }
    }
}
