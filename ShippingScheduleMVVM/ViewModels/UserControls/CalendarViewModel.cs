using ShippingScheduleMVVM.Commands;
using ShippingScheduleMVVM.Models;
using ShippingScheduleMVVM.SharedServices;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Input;
using static ShippingScheduleMVVM.Models.Calendar;

namespace ShippingScheduleMVVM.ViewModels
{
    class CalendarViewModel : ViewModelBase
    {
        private readonly SharedDateDataService _sharedDateDataService;
        public ObservableCollection<CalendarDay> DaysOfMonth { get; set; }
        private DateTime _currentlySelectedDate { get; set; } = DateTime.MinValue;
        public DateTime CurrentlySelectedDate
        {
            get { return _currentlySelectedDate; }
            set
            {
                if (_currentlySelectedDate != value)
                {
                    _currentlySelectedDate = value;
                    _sharedDateDataService.CurrentlySelectedDate = value;

                    OnPropertyChanged("CurrentlySelectedDate");
                    OnPropertyChanged("CurrentlySelectedMonth");
                }
            }
        }
        public string CurrentlySelectedMonth
        {
            get { return CultureInfo.InvariantCulture.TextInfo.ToTitleCase(CurrentlySelectedDate.ToString("MMMM")) + " " + CurrentlySelectedDate.Year; }
        }
        public ICommand AddMonth { get; }
        public ICommand RemoveMonth { get; }
        public ICommand SelectedDay { get; }
        public CalendarViewModel(SharedDateDataService sharedDateDataService)
        {
            _sharedDateDataService = sharedDateDataService;
            _sharedDateDataService.SelectedDateChanged += OnSelectedDateChanged;

            DaysOfMonth = new ObservableCollection<CalendarDay>();

            // Populate Calendar
            CurrentlySelectedDate = DateTime.Now;
            CalendarDay.SetDaysOfMonth(DaysOfMonth, CurrentlySelectedDate);
            CalendarDay.SelectDayByDate(DaysOfMonth, CurrentlySelectedDate);

            // Initialize Commands
            AddMonth = new ChangeMonthCalendarCommand(this, CalendarAction.AddMonth);
            RemoveMonth = new ChangeMonthCalendarCommand(this, CalendarAction.RemoveMonth);
            SelectedDay = new SelectCalendarDayCommand(this);
        }
        private void OnSelectedDateChanged(object? sender, EventArgs e)
        {
            // Cast the sender argument to a DateService instance to access its properties
            var dateService = sender as SharedDateDataService ?? new SharedDateDataService();

            // Get the new date value from the service
            CurrentlySelectedDate = dateService.CurrentlySelectedDate;

            CalendarDay.SetDaysOfMonth(DaysOfMonth, CurrentlySelectedDate);
            CalendarDay.SelectDayByDate(DaysOfMonth, CurrentlySelectedDate);
        }
        private void DisposeOfEvents()
        {
            _sharedDateDataService.SelectedDateChanged -= OnSelectedDateChanged;
        }
    }
}
