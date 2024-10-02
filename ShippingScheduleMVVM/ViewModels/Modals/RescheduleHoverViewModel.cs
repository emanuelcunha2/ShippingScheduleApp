using ShippingScheduleMVVM.Helpers;
using ShippingScheduleMVVM.Models;
using System;
using System.Windows.Forms;

namespace ShippingScheduleMVVM.ViewModels.Modals
{
    public class RescheduleHoverViewModel : ViewModelBase
    {
        private WindowSettings? _windowSettings { get; set; }
        public WindowSettings? WindowSettings
        {
            get
            { return _windowSettings; }
            set
            {
                if (_windowSettings != value)
                {
                    _windowSettings = value;
                    OnPropertyChanged("WindowSettings");
                }
            }
        }

        private string? _user { get; set; }
        public string? User
        {
            get
            { return _user; }
            set
            {
                if (_user != value)
                {
                    _user = value;
                    OnPropertyChanged("User");
                }
            }
        }

        private string? _date { get; set; }
        public string? Date
        {
            get
            { return _date; }
            set
            {
                if (_date != value)
                {
                    _date = value;
                    OnPropertyChanged("Date");
                }
            }
        }
        public RescheduleHoverViewModel(double top, double left, ScheduleRecord record)
        {
            WindowSettings = new("ThisRescheduleHoverWindow");
            WindowSettings.Initialize(1.6, 110, 55, true, "ThisMainWindow");

            Screen? screen = WindowScreen.FindScreen("ThisRescheduleHoverWindow");
            if (screen == null) { return; }
            Tuple<double, double> dpi = WindowScreen.GetDPI("ThisRescheduleHoverWindow");

            double dpiX = dpi.Item1;
            double dpiY = dpi.Item2;

            WindowSettings.Top = top / dpiY;
            WindowSettings.Left = left / dpiX;
            WindowSettings.Left -= (WindowSettings.Width * 1);

            Date = record.OriginalSchedule.Replace("+", "    ");
        }
    }
}
