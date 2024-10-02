using ShippingScheduleMVVM.Commands;
using ShippingScheduleMVVM.Converters;

using ShippingScheduleMVVM.Models;
using ShippingScheduleMVVM.SharedServices;

using System;

using System.Collections.ObjectModel;

using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;


namespace ShippingScheduleMVVM.ViewModels
{
    public class ScheduleVisualManagementViewModel : ViewModelBase
    {
        public string Username { get; set; } = App.loggedUser.Username;
        public string Role { get; set; } = (App.loggedUser?.Roles?[0] == null) ? "" : App.loggedUser.Roles[0];
        private SolidColorBrush _brush = new(Color.FromRgb(250, 250, 250));
        public SolidColorBrush Brush
        {
            get { return _brush; }
            set
            {
                if (_brush != value)
                {
                    _brush = value;
                    OnPropertyChanged("Brush");
                }
            }
        }
        public string TodayString { get; set; } = "Today, " + DateTime.Now.Day.ToString() + " " + DateTime.Now.ToString("MMMM") + " " + DateTime.Now.Year.ToString();
        private int _SecondsTilUpdate = 0;
        public int SecondsTilUpdate
        {
            get { return _SecondsTilUpdate; }
            set { _SecondsTilUpdate = value; OnPropertyChanged(nameof(SecondsTilUpdate)); }
        }
        private RecordsStatus _scheduleRecordStatus = new RecordsStatus();
        public RecordsStatus ScheduleRecordsStatus
        {
            get { return _scheduleRecordStatus; }
            set { _scheduleRecordStatus = value; OnPropertyChanged(nameof(ScheduleRecordsStatus)); }
        }   
        public MouseEventArgsConverter EventArgsConverter { get; set; } = new();
        private double _scrollPosition;
        public double ScrollPosition
        {
            get { return _scrollPosition; }
            set
            {
                _scrollPosition = value;
                OnPropertyChanged("ScrollPosition");
            }
        }
        private readonly SharedDateDataService _sharedDateDataService;
        public string Information { get; set; }
        private ViewModelBase? _scheduleCalendarViewModel { get; set; }
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
                }
            }
        }  
        public ViewModelBase? ScheduleCalendarViewModel
        {
            get
            { return _scheduleCalendarViewModel; }
            set
            {
                if (_scheduleCalendarViewModel != value)
                {
                    _scheduleCalendarViewModel = value;
                    OnPropertyChanged("ScheduleCalendarViewModel");
                }
            }
        }
        public ObservableCollection<ScheduleRecord> DayRecords { get; set; } = new();
        private DispatcherTimer _timer; 
        public ICommand RefreshRecords { get; }
        private readonly MainWindowViewModel _parent;
        public ScheduleVisualManagementViewModel(MainWindowViewModel parent)
        {
            _parent = parent;
            _sharedDateDataService = new SharedDateDataService();
            _sharedDateDataService.SelectedDateChanged += OnSelectedDateChanged;

            Information = "ThisInformation";
            ScheduleCalendarViewModel = new CalendarViewModel(_sharedDateDataService);

            DayRecords = ScheduleRecord.SetScheduleRecordsDayMargins(CurrentlySelectedDate);
            // Initialize Commands
            RefreshRecords = new RelayCommand(() =>
            {
                DayRecords.Clear();
                DayRecords = ScheduleRecord.SetScheduleRecordsDayMargins(CurrentlySelectedDate);
                OnPropertyChanged("DayRecords");
            });
            // Create a new Dispatcher_timer object
            _timer = new DispatcherTimer(); 

            // Set the interval to 5 seconds
            _timer.Interval = TimeSpan.FromMilliseconds(1000); 

            // Set the tick event handler
            _timer.Tick += Update_timer; 

            // Start the _timer
            _timer.Start(); 
        } 
        public bool CanUpdateTimer = true;
        private void Update_timer(object? sender, EventArgs e)
        {
            if (_parent.CurrentViewModel != this)
            {
                return;
            }

            if (SecondsTilUpdate > 0 && CanUpdateTimer)
            {
                SecondsTilUpdate--;
            }
            if (SecondsTilUpdate <= 0)
            {
                SecondsTilUpdate = 20;
                UpdateTimerManualy();
            }
        }
        public int ChangeWeekCounter = 0; 
        public void UpdateTimerManualy()
        {
            DayRecords.Clear();
            DayRecords = ScheduleRecord.SetScheduleRecordsDayMargins(CurrentlySelectedDate);
            OnPropertyChanged("DayRecords");
        } 
        private void OnSelectedDateChanged(object? sender, EventArgs e)
        {
            SecondsTilUpdate = 20;
            // Cast the sender argument to a DateService instance to access its properties
            var dateService = sender as SharedDateDataService ?? new SharedDateDataService();

            // Get the new date value from the service
            CurrentlySelectedDate = dateService.CurrentlySelectedDate;
            UpdateTimerManualy();
        }
        public void ScrollToCurrentHour()
        {
            int halfAnHour = DateTime.Now.Minute > 30 ? 1 : 0;
            int indexDiference = 2;

            if (ScrollPosition == DateTime.Now.Hour * 2 - indexDiference + halfAnHour)
            {
                ScrollPosition = DateTime.Now.Hour * 2 - indexDiference + 0.1 + halfAnHour;
                return;
            }
            if (ScrollPosition == DateTime.Now.Hour * 2 - indexDiference + 0.1 + halfAnHour)
            {
                ScrollPosition = DateTime.Now.Hour * 2 - indexDiference + halfAnHour;
                return;
            }
            ScrollPosition = DateTime.Now.Hour * 2 - indexDiference + halfAnHour;
        }
    }
}