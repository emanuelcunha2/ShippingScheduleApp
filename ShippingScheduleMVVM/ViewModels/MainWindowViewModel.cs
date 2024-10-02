using ShippingScheduleMVVM.Commands;
using ShippingScheduleMVVM.Models;
using ShippingScheduleMVVM.Views;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace ShippingScheduleMVVM.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private SolidColorBrush _backgroundCalendar { get; set; } = (SolidColorBrush)Application.Current.Resources["DarkPrimaryThemeColor"];
        public SolidColorBrush BackgroundCalendar
        {
            get { return _backgroundCalendar; }
            set 
            {
                _backgroundCalendar = value;
                OnPropertyChanged("BackgroundCalendar");
            }
        }
        private SolidColorBrush _shippedScheduleBackground { get; set; } = new SolidColorBrush(Colors.Transparent);
        public SolidColorBrush ShippedScheduleBackground
        {
            get { return _shippedScheduleBackground; }
            set
            {
                _shippedScheduleBackground = value;
                OnPropertyChanged("ShippedScheduleBackground");
            }
        }
        private SolidColorBrush _backgroundSettings { get; set; } = new SolidColorBrush(Colors.Transparent);
        public SolidColorBrush BackgroundSettings
        {
            get { return _backgroundSettings; }
            set
            {
                if(_backgroundSettings != value)
                {
                    _backgroundSettings = value;
                    OnPropertyChanged(nameof(BackgroundSettings));
                }
            }
        }
        private Visibility _visibilityOverlay = Visibility.Collapsed;
        public Visibility VisibilityOverlay
        {
            get { return _visibilityOverlay; }
            set
            {
                if (_visibilityOverlay != value)
                {
                    _visibilityOverlay = value;
                    OnPropertyChanged("VisibilityOverlay");
                }
            }
        } 
        private WindowSettings? _windowSettings { get; set; }
        public WindowSettings WindowSettings
        {
            get
            { return _windowSettings ?? new WindowSettings("ThisMainWindow"); }
            set
            {
                if (_windowSettings != value)
                {
                    _windowSettings = value;

                    OnPropertyChanged("WindowSettings");
                }
            }
        }
        private ViewModelBase? _currentViewModel { get; set; }
        public ViewModelBase? CurrentViewModel
        {
            get
            { return _currentViewModel; }
            set
            {
                if (_currentViewModel != value)
                {
                    _currentViewModel = value;
                    OnPropertyChanged("CurrentViewModel");
                }
            }
        } 
        private Uri? _maximizeMinimizeImage { get; set; }
        public Uri MaximizeMinimizeImage
        {
            get
            { return _maximizeMinimizeImage ?? new Uri(""); }
            set
            {
                if (_maximizeMinimizeImage != value)
                {
                    _maximizeMinimizeImage = value;
                    OnPropertyChanged("MaximizeMinimizeImage");
                }
            }
        }
        private Visibility _visibilitySlider = Visibility.Visible;
        public Visibility VisibilitySlider
        {
            get { return _visibilitySlider; }
            set
            {
                if (_visibilitySlider != value)
                {
                    _visibilitySlider = value;
                    OnPropertyChanged("VisibilitySlider");
                }
            }
        }
        public ViewModelBase? CalendarViewModel { get; set; } = null;
        public ViewModelBase? ShippedScheduleViewModel { get; set; }  = null; 
        public ViewModelBase? ExpeditionScheduleViewModel { get; set; } = null;
        public ViewModelBase? ShipToSettingsViewModel { get; set; } = null;
        public ViewModelBase? UnloadingPointsSettingsViewModel { get; set; } = null;
        public ICommand CloseWindow { get; }
        public ICommand MinimizeWindow { get; }
        public ICommand MaximizeMinimizeWindow { get; }
        public ICommand RecalculateWindowSize { get; }
        public ICommand LogOff { get; }
        public ICommand ClickedCalendar { get; }
        public ICommand ClickedShippedSchedule { get; }
        public ICommand ClickedExpedition { get; }
        public ICommand ClickedSettings { get; }
        public MainWindowViewModel()
        {
            Application.Current.Dispatcher.InvokeAsync( async() =>
            {
                await Task.Delay(100);
                CalendarViewModel = new ScheduleManagementViewModel();
                CurrentViewModel = CalendarViewModel;
            });

            Application.Current.Dispatcher.InvokeAsync(async () =>
            {
                await Task.Delay(100);
                ShippedScheduleViewModel = new ScheduleVisualManagementViewModel(this);
            });

            WindowSettings = new("ThisMainWindow");
            WindowSettings.Initialize(1.8, 970, 550, true, "ThisMainWindow");
            App.InitialDPI = WindowSettings.OriginalDpi;

            MaximizeMinimizeImage = new Uri("pack://application:,,,/ShippingScheduleMVVM;component/Resources/Images/Maximize.png");

            // Intialize Commands
            CloseWindow = new CloseWindowCommand(true);
            MinimizeWindow = new MinimizeWindowCommand(() => WindowSettings.State, value => WindowSettings.State = value);
            MaximizeMinimizeWindow = new MaximizeWindowCommand(() => WindowSettings.State, normalizeWindow: WindowSettings.Normalize, WindowSettings.Maximize, 1.8, MaximizeMinimize);
            RecalculateWindowSize = new RecalculateWindowCommand(WindowSettings.RecalculateWindow);

            LogOff = new RelayCommand(() => {

                System.Windows.Window? window = Application.Current.Windows.OfType<System.Windows.Window>().FirstOrDefault(w => w.Title == "ThisMainWindow");
                if (window == null) { return; }

                LoginWindow destinationWindow = new LoginWindow() { DataContext = new LoginViewModel() };
                destinationWindow.Show();

                window.Close();

            });

            ClickedCalendar = new RelayCommand(() =>
            {
                BackgroundCalendar = (SolidColorBrush)Application.Current.Resources["DarkPrimaryThemeColor"];
                ShippedScheduleBackground = new SolidColorBrush(Colors.Transparent);
                BackgroundSettings = new SolidColorBrush(Colors.Transparent);

                Application.Current.Dispatcher.InvokeAsync(async() =>
                {
                    await Task.Delay(150);
                    if (CurrentViewModel != CalendarViewModel)
                    {
                        CurrentViewModel = CalendarViewModel;
                    }
                });
            });

            ClickedShippedSchedule = new RelayCommand(() =>
            {
                ShippedScheduleBackground = (SolidColorBrush)Application.Current.Resources["DarkPrimaryThemeColor"];
                BackgroundCalendar = new SolidColorBrush(Colors.Transparent);
                BackgroundSettings = new SolidColorBrush(Colors.Transparent);

                Application.Current.Dispatcher.InvokeAsync(async() =>
                {
                    await Task.Delay(50);
                    if (CurrentViewModel != ShippedScheduleViewModel)
                    {
                        CurrentViewModel = ShippedScheduleViewModel;
                    }
                });
            });

            ClickedSettings = new RelayCommand(() =>
            {

            });

            ClickedExpedition = new RelayCommand(() =>
            {
                if(CurrentViewModel != ExpeditionScheduleViewModel)
                {
                    CurrentViewModel = ExpeditionScheduleViewModel;
                }
            });
 
        }
        public void SetVisibilityOnState(WindowState state)
        {
            if (state == WindowState.Normal || state == WindowState.Maximized)
            {
                WindowSettings.Visibility = Visibility.Visible;
            } 
        }
        public void MaximizeMinimize(WindowState windowState)
        {
            if (windowState == WindowState.Maximized)
            { 
                MaximizeMinimizeImage = new Uri("pack://application:,,,/ShippingScheduleMVVM;component/Resources/Images/Normalize.png"); 
                return;
            }
            MaximizeMinimizeImage = new Uri("pack://application:,,,/ShippingScheduleMVVM;component/Resources/Images/Maximize.png"); 
        }
     
    }
}
