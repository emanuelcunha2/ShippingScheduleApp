
using Microsoft.IdentityModel.Tokens;
using ShippingScheduleMVVM.Commands;
using ShippingScheduleMVVM.Converters;
using ShippingScheduleMVVM.Helpers;
using ShippingScheduleMVVM.Models;
using ShippingScheduleMVVM.Services;
using ShippingScheduleMVVM.SharedServices;
using ShippingScheduleMVVM.ViewModels.Modals;
using ShippingScheduleMVVM.Views.Modals;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
 
using Windows.UI.Notifications;

namespace ShippingScheduleMVVM.ViewModels
{
    public class ScheduleManagementViewModel : ViewModelBase
    {
        public string Username { get; set; } = App.loggedUser.Username;
        public string Role { get; set; } = (App.loggedUser?.Roles?[0] == null) ? "" : App.loggedUser.Roles[0];
        public bool IsRecordOpen = false;
        private string _forumMessage = string.Empty;
        public string ForumMessage
        {
            get => _forumMessage;
            set
            {
                if (_forumMessage != value)
                {
                    _forumMessage = value;
                    OnPropertyChanged(nameof(ForumMessage));
                }
            }
        }

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
        public bool IsDraggingRecord = false;
        private int _SecondsTilUpdate = 0;
        public int SecondsTilUpdate
        {
            get { return _SecondsTilUpdate; }
            set { _SecondsTilUpdate = value; OnPropertyChanged(nameof(SecondsTilUpdate)); }
        }
        private int _testValue = 0;
        public int TestValue
        {
            get { return _testValue; }
            set { _testValue = value; OnPropertyChanged(nameof(TestValue)); }
        }
        private RecordsStatus _scheduleRecordStatus = new RecordsStatus();
        public RecordsStatus ScheduleRecordsStatus
        {
            get { return _scheduleRecordStatus; }
            set { _scheduleRecordStatus = value; OnPropertyChanged(nameof(ScheduleRecordsStatus));}
        }
        private Thickness _dailyStatusBorderThickness = new Thickness(1);
        public Thickness DailyStatusBorderThickness
        {
            get { return _dailyStatusBorderThickness; }
            set
            {
                if (_dailyStatusBorderThickness != value)
                {
                    _dailyStatusBorderThickness = value;
                    OnPropertyChanged("DailyStatusBorderThickness");
                }
            }
        }
        private Thickness _totalStatusBorderThickness = new Thickness(1);
        public Thickness TotalStatusBorderThickness
        {
            get { return _totalStatusBorderThickness; }
            set
            {
                if (_totalStatusBorderThickness != value)
                {
                    _totalStatusBorderThickness = value;
                    OnPropertyChanged("TotalStatusBorderThickness");
                }
            }
        }
        private SolidColorBrush _dailyStatusBorderBrush = Brushes.Black;
        public SolidColorBrush DailyStatusBorderBrush
        {
            get { return _dailyStatusBorderBrush; }
            set
            {
                if (_dailyStatusBorderBrush != value)
                {
                    _dailyStatusBorderBrush = value;
                    OnPropertyChanged("DailyStatusBorderBrush");
                }
            }
        }
        private SolidColorBrush _totalStatusBorderBrush = Brushes.Black;
        public SolidColorBrush TotalStatusBorderBrush
        {
            get { return _totalStatusBorderBrush; }
            set
            {
                if (_totalStatusBorderBrush != value)
                {
                    _totalStatusBorderBrush = value;
                    OnPropertyChanged("TotalStatusBorderBrush");
                }
            }
        }
        private SolidColorBrush _dailyStatusBackground = Brushes.White;
        public SolidColorBrush DailyStatusBackground
        {
            get { return _dailyStatusBackground; }
            set
            {
                if (_dailyStatusBackground != value)
                {
                    _dailyStatusBackground = value;
                    OnPropertyChanged("DailyStatusBackground");
                }
            }
        }
        private SolidColorBrush _totalStatusBackground = Brushes.White;
        public SolidColorBrush TotalStatusBackground
        {
            get { return _totalStatusBackground; }
            set
            {
                if (_totalStatusBackground != value)
                {
                    _totalStatusBackground = value;
                    OnPropertyChanged("TotalStatusBackground");
                }
            }
        }
        private SolidColorBrush _dailyStatusForeground = Brushes.White;
        public SolidColorBrush DailyStatusForeground
        {
            get { return _dailyStatusForeground; }
            set
            {
                if (_dailyStatusForeground != value)
                {
                    _dailyStatusForeground = value;
                    OnPropertyChanged(nameof(DailyStatusForeground));
                }
            }
        }
        private SolidColorBrush _totalStatusForeground = Brushes.White;
        public SolidColorBrush TotalStatusForeground
        {
            get { return _totalStatusForeground; }
            set
            {
                if (_totalStatusForeground != value)
                {
                    _totalStatusForeground = value;
                    OnPropertyChanged(nameof(TotalStatusForeground));
                }
            }
        }

        private bool _isDailyStatus { get; set; } = true;
        public bool IsDailyStatus
        {
            get { return _isDailyStatus; }
            set
            {
                if (_isDailyStatus != value)
                {
                    _isDailyStatus = value;

                    OnPropertyChanged("IsDailyStatus");
                    if (value == true)
                    {
                        DailyStatusBorderBrush = (SolidColorBrush)Application.Current.Resources["PrimaryThemeColor"];
                        DailyStatusBorderThickness = new Thickness(0);
                        DailyStatusBackground = (SolidColorBrush)Application.Current.Resources["PrimaryThemeColor"];
                        DailyStatusForeground = new SolidColorBrush(Colors.White);

                        TotalStatusBorderBrush = new SolidColorBrush(Colors.LightGray);
                        TotalStatusBorderThickness = new Thickness(0, 1, 1, 1);
                        TotalStatusBackground = new SolidColorBrush(Colors.White);
                        TotalStatusForeground = new SolidColorBrush(Colors.Black);
                        ScheduleRecordsStatus = RecordsStatus.GetDailyStatus(ScheduleRows, CurrentlySelectedDate);
                    }
                    else
                    {
                        DailyStatusBorderBrush = new SolidColorBrush(Colors.LightGray);
                        DailyStatusBorderThickness = new Thickness(1, 1, 0, 1);
                        DailyStatusBackground = new SolidColorBrush(Colors.White);
                        DailyStatusForeground = new SolidColorBrush(Colors.Black);

                        TotalStatusBorderBrush = (SolidColorBrush)Application.Current.Resources["PrimaryThemeColor"];
                        TotalStatusBorderThickness = new Thickness(0);
                        TotalStatusBackground = (SolidColorBrush)Application.Current.Resources["PrimaryThemeColor"];
                        TotalStatusForeground = new SolidColorBrush(Colors.White);
                        ScheduleRecordsStatus = RecordsStatus.GetWeeklyStatus(ScheduleRows);
                    }
                }
            }
        }
        private Visibility _visualDragVisibility = Visibility.Hidden;
        public Visibility VisualDragVisibility
        {
            get { return _visualDragVisibility; }
            set
            {
                if (_visualDragVisibility != value)
                {
                    _visualDragVisibility = value;
                    OnPropertyChanged("VisualDragVisibility");
                }
            }
        }
        public MouseEventArgsConverter EventArgsConverter { get; set; } = new();
        private GridLength _columnDefinitionWidthDaysTitle = new(0.55, GridUnitType.Star);
        public GridLength ColumnDefinitionWidthDaysTitle
        {
            get { return _columnDefinitionWidthDaysTitle; }
            set
            {
                if( _columnDefinitionWidthDaysTitle != value)
                {
                    _columnDefinitionWidthDaysTitle = value;
                    OnPropertyChanged("ColumnDefinitionWidthDaysTitle");
                }
 
            }
        }
        private double _scrollPosition;
        public double ScrollPosition
        {
            get { return _scrollPosition; }
            set
            {
                if(_scrollPosition != value)
                {
                    _scrollPosition = value;
                    OnPropertyChanged("ScrollPosition");
                }
 
            }
        }
        private readonly SharedDateDataService _sharedDateDataService;
        public string? Information { get; set; }
        private ViewModelBase? _scheduleCalendarViewModel { get; set; } = null;
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


                    // Get Week Number
                    System.Globalization.Calendar calendar = new GregorianCalendar();
                    DateTime monday = value.AddDays(-(int)value.DayOfWeek + 1);
                    int weekNumber = calendar.GetWeekOfYear(monday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
                    CurrentlySelectedWeek = weekNumber.ToString();


                    OnPropertyChanged("CurrentlySelectedDate");
                    RefreshWeekTitles(ShowDailyChecked);
                }
                else
                {
                    ScrollToSelectedRecordHour(value);
                }
            }
        }

        private string _currentlySelectedWeek = string.Empty;
        public  string CurrentlySelectedWeek
        {
            get => _currentlySelectedWeek;
            set
            {
                if (_currentlySelectedWeek != value)
                {
                    _currentlySelectedWeek = "Week " + value;
                    OnPropertyChanged("CurrentlySelectedWeek");
                }
                 
            }
        }
        public ObservableCollection<ScheduleWeekDay> WeekDays { get; set; } = new();
        public ObservableCollection<ScheduleRow> ScheduleRows { get; set; } = new();
        public ObservableCollection<Models.ChatMessage> ChatMessages { get; set; } = new();
        public ObservableCollection<RecordDayStatistics> WeeklyRecordsStatistics { get; set; } = new();
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
        private bool _showWhitesChecked { get; set; } = false;
        public bool ShowWhitesChecked
        {
            get
            { return _showWhitesChecked; }
            set
            {
                if (_showWhitesChecked != value)
                {
                    SecondsTilUpdate = 20;
                    _showWhitesChecked = value;
                    OnPropertyChanged("ShowWhitesChecked");

                    Application.Current.Dispatcher.InvokeAsync(async () =>
                    {
                        await Task.Delay(50);

                        ScheduleWeekDay.InitializeDaysOfWeek(WeekDays, CurrentlySelectedDate);

                        ScheduleRows.Clear();
                        ScheduleRows = ScheduleRow.InitializeScheduleRows(CurrentlySelectedDate, ShowWhitesChecked, ShowDailyChecked);
                        OnPropertyChanged("ScheduleRows");
                        RefreshWeekTitles(ShowDailyChecked);
                    });
 
                }
            }
        }
        private bool _showDailyChecked { get; set; } = false;
        public bool ShowDailyChecked
        {
            get
            { return _showDailyChecked; }
            set
            {
                if (_showDailyChecked != value)
                {
                    SecondsTilUpdate = 20;
                    _showDailyChecked = value;
                    OnPropertyChanged("ShowDailyChecked");


                    Application.Current.Dispatcher.InvokeAsync(async () =>
                    {
                        await Task.Delay(50);

                        ScheduleRows.Clear();

                        ScheduleRows = ScheduleRow.InitializeScheduleRows(CurrentlySelectedDate, ShowWhitesChecked, ShowDailyChecked);
                        OnPropertyChanged("ScheduleRows");
                        RefreshWeekTitles(ShowDailyChecked);

                        WeeklyRecordsStatistics.Clear();
                        WeeklyRecordsStatistics = RecordDayStatistics.CreateStatisticsWeek(ScheduleRows);
                        OnPropertyChanged("WeeklyRecordsStatistics");

                    });
                }
            }
        }
        private string _searchRecordIdText  = string.Empty;
        public string SearchRecordIdText
        {
            get => _searchRecordIdText;
            set
            {
                if (_searchRecordIdText != value)
                {
                    _searchRecordIdText = value;
                    OnPropertyChanged("SearchRecordIdText");
                }
            }
        }
        private bool _IsRecordsDropDownOpen { get; set; } = false;
        public bool IsRecordsDropDownOpen
        {
            get
            { return _IsRecordsDropDownOpen; }
            set
            {
                if (_IsRecordsDropDownOpen != value)
                {
                    _IsRecordsDropDownOpen = value;
                    OnPropertyChanged("IsRecordsDropDownOpen");
                }
            } 
        }
        private bool _wasRecordSelectedViaId = false;
        public ObservableCollection<ScheduleRecord> SearchingRecordsList { get; set; } = new();

        private ScheduleRecord _searchingRecord = new();
        public ScheduleRecord SearchingRecord
        {
            get => _searchingRecord;
            set
            {
                if(value != null)
                {
                    _searchingRecord = value;
                    if (value.Date > DateTime.MinValue)
                    {
                        if (!value.ShipTos.IsNullOrEmpty()) { SearchRecordIdText = value.Id  + " " + value.ShipTos; }
                        else { SearchRecordIdText = value.Id.ToString(); }

                        _wasRecordSelectedViaId = true;
                        CurrentlySelectedDate = value.Date; 
                    }
                }
                OnPropertyChanged("SearchingRecord");
            }
        }
        public bool IsDraggingWeekRight = false;
        public bool IsDraggingWeekLeft = false;
        public ICommand OpenUpdateRecordData { get; }
        public ICommand DragRecord { get; }
        public ICommand DropRecord { get; }
        public ICommand DragOverRecord { get; }
        public ICommand DragLeaveRecord { get; }
        public ICommand ChangeIsDailyStatus { get; }
        public ICommand RefreshRecords { get; }
        public ICommand DragRecordWeek { get; }
        public ICommand DragLeaveWeek { get; }
        public ICommand RightClickRecord { get; }
        public ICommand ExportRecordsToExcel { get; }
        public ICommand RescheduleEnter { get; }
        public ICommand RescheduleLeave { get; }
        public ICommand SearchIdTextChanged { get; } 
        public ICommand SendMessageForum { get; }
        public ICommand ScrollToBottom { get; }
        public ICommand SetChatList { get; }

        private DispatcherTimer _timer;
        private ListView? _chatListView = null;
        private DispatcherTimer _smalltimer;
        public ScheduleManagementViewModel()
        {
            _sharedDateDataService = new SharedDateDataService();
            _sharedDateDataService.SelectedDateChanged += OnSelectedDateChanged;

 
            Application.Current.Dispatcher.InvokeAsync(async () =>
            {
                await Task.Delay(50);

                ScheduleWeekDay.InitializeDaysOfWeek(WeekDays, CurrentlySelectedDate);
                ScheduleRows = ScheduleRow.InitializeScheduleRows(CurrentlySelectedDate, ShowWhitesChecked, ShowDailyChecked);
                RefreshWeekTitles(ShowDailyChecked);
                WeeklyRecordsStatistics = RecordDayStatistics.CreateStatisticsWeek(ScheduleRows);

                ScrollPosition = 1;
            });

            Application.Current.Dispatcher.InvokeAsync(async () =>
            {
                await Task.Delay(50);
                Information = "ThisInformation";
                ScheduleCalendarViewModel = new CalendarViewModel(_sharedDateDataService);
                
                await Task.Delay(50);
                GetChatMessages();
                if (_chatListView != null)
                {
                    _chatListView.ScrollIntoView(ChatMessages[ChatMessages.Count() - 1]);
                }
            });

            // Initialize commands
            OpenUpdateRecordData = new OpenRecordDataWindow(this);
            DragRecord = new MouseMoveDragRecordCommand(this);
            DropRecord = new DropRecordCommand(this);
            DragOverRecord = new DragOverRecordCommand();
            DragLeaveRecord = new DragLeaveCommand();
            ChangeIsDailyStatus = new ChangeStatusRangeCommand(this);
            RefreshRecords = new RefreshRecordsCommand(this);
            DragRecordWeek = new DragRecordWeek(this);
            DragLeaveWeek = new DragLeaveRecordWeek(this);

            SetChatList = new RelayCommandParemeters(parameter =>
            {
                if(parameter is ListView lview)
                {
                    _chatListView = lview;
                }
            });

            RightClickRecord = new RelayCommandParemeters(parameter =>
            {
                CopyPastePopup(parameter);
            });

            ExportRecordsToExcel = new RelayCommand(() =>
            {
                System.Windows.Window? window = System.Windows.Application.Current.Windows.OfType<System.Windows.Window>().FirstOrDefault(w => w.Title == "ThisMainWindow");
                if (window == null) { return; }

                ExportRecordsExcelWindow popup = new() { DataContext = new ExportRecordsExcelViewModel() };
                popup.Owner = window;

                popup.ShowDialog();
            });

            SearchIdTextChanged = new RelayCommand(() =>
            {
                IsRecordsDropDownOpen = false;
                if (!string.IsNullOrEmpty(SearchRecordIdText.Replace(" ", "")))
                {
                    DatabaseOperations database = new();
                    ObservableCollection<ScheduleRecord> records = database.SelectScheduleRecordsId(SearchRecordIdText);
                    SearchingRecordsList.Clear();

                    foreach (ScheduleRecord rec in records)
                    {
                        SearchingRecordsList.Add(rec); 
                    } 
                    if(SearchingRecordsList.Count > 0) { IsRecordsDropDownOpen = true; }               
                }
            });

            RescheduleEnter = new RelayCommandParemeters((parameter) =>
            {
                if(parameter is Image image)
                {
                    System.Windows.Window? owner = Application.Current.Windows.OfType<System.Windows.Window>().FirstOrDefault(w => w.Title == "ThisMainWindow");
                    if (owner == null) { return; } 

                    if(image.DataContext is ScheduleRecord record)
                    {
                        // Get the position of the element relative to the window
                        Point relativePosition = image.TranslatePoint(new Point(0, 0), owner);

                        // Convert the relative position to screen coordinates
                        Point screenPosition = owner.PointToScreen(relativePosition);

                        var window = new RescheduleHoverWindow() { DataContext = new RescheduleHoverViewModel(screenPosition.Y, screenPosition.X, record) };
                        window.Owner = owner;

                        window.Show();
                    }
                }
            });

            RescheduleLeave = new RelayCommand(() =>
            {
                System.Windows.Window? window = Application.Current.Windows.OfType<System.Windows.Window>().FirstOrDefault(w => w.Title == "ThisRescheduleHoverWindow");
                window?.Close();
            });

            SendMessageForum = new RelayCommandParemeters((parameter) =>
            {
                if (ForumMessage.IsNullOrEmpty()) { return; }
                if(parameter is ListView listView)
                {
                    InsertMessageIntoForum(listView);
                }
            });

            ScrollToBottom = new RelayCommandParemeters((parameter) =>
            {
                if (ForumMessage.IsNullOrEmpty()) { return; }
                if (parameter is ListView listView)
                {
                    listView.ScrollIntoView(ChatMessages[ChatMessages.Count() - 1]);
                    _chatListView = listView;
                }
            }); 

            // Create a new Dispatcher_timer object
            _timer = new DispatcherTimer();
            _smalltimer = new DispatcherTimer();

            // Set the interval to 5 seconds
            _timer.Interval = TimeSpan.FromMilliseconds(1000);
            _smalltimer.Interval= TimeSpan.FromMilliseconds(500);

            // Set the tick event handler
            _timer.Tick += Update_timer;
            _smalltimer.Tick += UpdateWeek;

            // Start the _timer
            _timer.Start();
            _smalltimer.Start();
        }

        private void GetChatMessages()
        {
            DatabaseOperations db = new();
            var list = db.GetTodaysChat();
             
            foreach (Models.ChatMessage item in list )
            {
                if(ChatMessages.Where(x => x.Id == item.Id).Count() == 0) 
                {
                    Visibility visibility = Visibility.Visible;
 

                    if(ChatMessages.Count > 0)
                    {
                        var user = ChatMessages[ChatMessages.Count() - 1].User;

                        if (user.Substring(1) == item.User)
                        {
                            visibility = Visibility.Collapsed;
                        }
                    }

                    var message = new Models.ChatMessage() { Id = item.Id, MessageContent = item.MessageContent, Date = item.Date.Substring(item.Date.Length - 8, 5), User = "@" + item.User, UserVisibility = visibility };
                    if(message.User.Substring(1) == Username) { message.HorizontalAlign = HorizontalAlignment.Right; message.ColorBrush = new SolidColorBrush(Color.FromRgb(199, 255, 208)); }
                    ChatMessages.Add(message);
                }
            }
        }

        private void InsertMessageIntoForum(ListView lView)
        {
            DatabaseOperations database = new();
            database.InsertMessageForum(ForumMessage, Username);
            ForumMessage = "";

            GetChatMessages();

            // ShowToastNotification_Click();
            if (ChatMessages.Count > 0)
                lView.ScrollIntoView(ChatMessages[ChatMessages.Count() - 1]);
        }

        private bool IsWindows10OrNewer()
        {
            // Use Environment.OSVersion.Version to check the Windows version
            // For example, Windows 10 has a major version of 10
            return Environment.OSVersion.Version.Major == 10;
        }

        public ScheduleRecord? RecordPastingCache { get; set; } = null;
        private void CopyPastePopup(object? parameter)
        {
            System.Windows.Window? window = System.Windows.Application.Current.Windows.OfType<System.Windows.Window>().FirstOrDefault(w => w.Title == "ThisMainWindow");
            var position = WindowScreen.GetLocationMouse("ThisMainWindow"); 
            if (position == null) { return; }
            var point = (Point)position;

            ScheduleRow? rowPasting = null;
            ScheduleRecord? recordPasting = null;
            string? pastingTag = "";

            // If is paste
            if (parameter is Border border)
            {
                if (border.DataContext is ScheduleRow row)
                {
                    rowPasting = row;
                    pastingTag = border.Tag.ToString() ?? "";
                }
            }

            // If is copy
            if (parameter is ScheduleRecord)
            {
                recordPasting = (ScheduleRecord?)parameter;
            }

            var copyPasteWindow = new CopyPasteWindow(parameter)
            {
                DataContext = new CopyPasteViewModel(point.Y, point.X, rowPasting, recordPasting, this, pastingTag),
                Owner = window
            };

            copyPasteWindow.Show();
        }
        public bool CanUpdateTimer = true;
        private void Update_timer(object? sender, EventArgs e)
        {
            GetChatMessages();

            if (SecondsTilUpdate > 0 && CanUpdateTimer)
            {
                SecondsTilUpdate--;
            }
            if (SecondsTilUpdate <= 0)
            {
                SecondsTilUpdate = 20;
                ScheduleRows.Clear();
                ScheduleRows = ScheduleRow.InitializeScheduleRows(CurrentlySelectedDate, ShowWhitesChecked, ShowDailyChecked);
                OnPropertyChanged("ScheduleRows");

                if (ShowDailyChecked)
                {
                    IsDailyStatus = false;
                    IsDailyStatus = true;
                }
                else
                {
                    IsDailyStatus = true;
                    IsDailyStatus = false;
                } 
                WeeklyRecordsStatistics.Clear(); 
                WeeklyRecordsStatistics = RecordDayStatistics.CreateStatisticsWeek(ScheduleRows);
                OnPropertyChanged("WeeklyRecordsStatistics");
            }
        }
        public int ChangeWeekCounter = 0;
        private void UpdateWeek(object? sender, EventArgs e)
        {
            if(IsDraggingWeekLeft || IsDraggingWeekRight)
            {
                if (ChangeWeekCounter < 1)
                {
                    ChangeWeekCounter++;
                    return;
                }
            }
            else { return; }

            if (IsDraggingWeekRight)
            {
                DateTime sunday = CurrentlySelectedDate.AddDays(-(int)CurrentlySelectedDate.DayOfWeek);
                CurrentlySelectedDate = sunday.AddDays(7);
                IsDraggingWeekRight = false;
                IsDraggingWeekLeft = false;
                ChangeWeekCounter = 0;
                return;
            }
            if (IsDraggingWeekLeft)
            {
                DateTime sunday = CurrentlySelectedDate.AddDays(-(int)CurrentlySelectedDate.DayOfWeek);
                CurrentlySelectedDate = sunday.AddDays(-7);
                IsDraggingWeekRight = false;
                IsDraggingWeekLeft = false;
                ChangeWeekCounter = 0;
                return;
            }
        }

        public void UpdateTimerManualy()
        {
            ScheduleRows.Clear();
            ScheduleRows = ScheduleRow.InitializeScheduleRows(CurrentlySelectedDate, ShowWhitesChecked, ShowDailyChecked);
            OnPropertyChanged("ScheduleRows");

            if (ShowDailyChecked)
            {
                IsDailyStatus = false;
                IsDailyStatus = true;
            }
            else 
            {
                IsDailyStatus = true;
                IsDailyStatus = false;
            }
            SecondsTilUpdate = 20;
        }

        private void RefreshWeekTitles(bool showDaily)
        {
            WeekDays.Clear();
            ScheduleWeekDay.InitializeDaysOfWeek(WeekDays, CurrentlySelectedDate);

            ColumnDefinitionWidthDaysTitle = new GridLength(0.55, GridUnitType.Star);
            if (showDaily)
            {
                ColumnDefinitionWidthDaysTitle = new GridLength(0, GridUnitType.Star);
                ScheduleWeekDay.ShowOnlySelectedDay(WeekDays, (int)CurrentlySelectedDate.DayOfWeek);
            }
            OnPropertyChanged("WeekDays");
        }
        private void OnSelectedDateChanged(object? sender, EventArgs e)
        { 
            SecondsTilUpdate = 20;
            // Cast the sender argument to a DateService instance to access its properties
            var dateService = sender as SharedDateDataService ?? new SharedDateDataService();

            // Get the new date value from the service
            CurrentlySelectedDate = dateService.CurrentlySelectedDate;

            ScheduleRows.Clear();

            Application.Current.Dispatcher.InvokeAsync(async () =>
            {
                await Task.Delay(50);

                ScheduleRows = ScheduleRow.InitializeScheduleRows(_currentlySelectedDate, ShowWhitesChecked, ShowDailyChecked);
                OnPropertyChanged("ScheduleRows");

                WeeklyRecordsStatistics.Clear();
                WeeklyRecordsStatistics = RecordDayStatistics.CreateStatisticsWeek(ScheduleRows);
                OnPropertyChanged("WeeklyRecordsStatistics");

                if (ShowDailyChecked)
                {
                    IsDailyStatus = true;
                }
                else { IsDailyStatus = false; }

                if (_wasRecordSelectedViaId)
                {
                    ScrollToSelectedRecordHour(CurrentlySelectedDate);

                    _wasRecordSelectedViaId = false;
                    if (_searchingRecord == null) { return; }
                    foreach (ScheduleRow row in ScheduleRows)
                    {
                        if (row.Time + ":00" == CurrentlySelectedDate.TimeOfDay.ToString() || row.Time + ":30" == CurrentlySelectedDate.TimeOfDay.ToString())
                        {
                            Func<ScheduleRow, ObservableCollection<ScheduleRecord>> getProperty = dayToPropertyMap[CurrentlySelectedDate.DayOfWeek.ToString()];
                            ObservableCollection<ScheduleRecord> records = getProperty(row);
                            foreach (ScheduleRecord record in records)
                            {
                                if (record.Id == _searchingRecord.Id)
                                {
                                    record.BackgroundColor = new(System.Windows.Media.Color.FromRgb(255, 74, 74));
                                    break;
                                }
                            }
                        }
                    }
                }

            });

        }

        Dictionary<string, Func<ScheduleRow, ObservableCollection<ScheduleRecord>>> dayToPropertyMap = new Dictionary<string, Func<ScheduleRow, ObservableCollection<ScheduleRecord>>>
        {
                { "Monday", row => row.MondayRecords },
                { "Tuesday", row => row.TuesdayRecords },
                { "Wednesday", row => row.WednesdayRecords },
                { "Thursday", row => row.ThursdayRecords },
                { "Friday", row => row.FridayRecords },
                { "Saturday", row => row.SaturdayRecords },
                { "Sunday", row => row.SundayRecords }
        };

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

        public void ScrollToSelectedRecordHour(DateTime date)
        {
            int halfAnHour = date.Minute >= 30 ? 1 : 0;
            int indexDiference = 0;

            if (ShowWhitesChecked)
            { 
                if (ScrollPosition == date.Hour * 2 - indexDiference + halfAnHour)
                {
                    ScrollPosition = date.Hour * 2 - indexDiference + 0.1 + halfAnHour;
                    return;
                }
                if (ScrollPosition == date.Hour * 2 - indexDiference + 0.1 + halfAnHour)
                {
                    ScrollPosition = date.Hour * 2 - indexDiference + halfAnHour;
                    return;
                } 

                ScrollPosition = date.Hour * 2 - indexDiference + halfAnHour;

                return;
            }

            int countLines = 0;
            foreach(ScheduleRow row in ScheduleRows)
            {
                if(row.Time + ":00" == date.TimeOfDay.ToString() || row.Time + ":30" == date.TimeOfDay.ToString())
                {  
                    break;
                }
                countLines++;
            }

            ScrollPosition = countLines ;
        }
    }
}
