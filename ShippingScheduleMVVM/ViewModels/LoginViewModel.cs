using ShippingScheduleMVVM.Commands;
using ShippingScheduleMVVM.Models;
using ShippingScheduleMVVM.Views;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace ShippingScheduleMVVM.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly double _screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
        private readonly double _screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;

        private WindowSettings? _windowSettings { get; set; }
        public WindowSettings? WindowSettings
        {
            get => _windowSettings;
            set
            {
                if (_windowSettings != value)
                {
                    _windowSettings = value;
                    OnPropertyChanged("WindowSettings");
                }
            }
        }

        private string? _username { get; set; }
        public string? Username
        {
            get
            { return _username; }
            set
            {
                if (_username != value)
                {
                    _username = value;
                    OnPropertyChanged("Username");
                }
            }
        }
        private string? _password { get; set; }
        public string? Password
        {
            get
            { return _password; }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged("Password");
                }
            }
        }
        private bool _rememberUserChecked { get; set; }
        public bool RememberUserChecked
        {
            get
            { return _rememberUserChecked; }
            set
            {
                if (_rememberUserChecked != value)
                {
                    _rememberUserChecked = value;
                    OnPropertyChanged("RememberUserChecked");
                }
            }
        }
        private Visibility _checkboxVisibility { get; set; }
        public Visibility CheckboxVisibility
        {
            get
            { return (Visibility)_checkboxVisibility; }
            set
            {
                if (_checkboxVisibility != value)
                {
                    _checkboxVisibility = value;
                    OnPropertyChanged("CheckboxVisibility");
                }
            }
        }
        private SolidColorBrush? _checkboxBorderBackground { get; set; }
        public SolidColorBrush? CheckboxBorderBackground
        {
            get
            { return _checkboxBorderBackground; }
            set
            {
                if (_checkboxBorderBackground != value)
                {
                    _checkboxBorderBackground = value;
                    OnPropertyChanged("CheckboxBorderBackground");
                }
            }
        }
        private SolidColorBrush? _userBorderColor { get; set; }
        public SolidColorBrush? UserBorderColor
        {
            get
            { return _userBorderColor; }
            set
            {
                if (_userBorderColor != value)
                {
                    _userBorderColor = value;
                    OnPropertyChanged("UserBorderColor");
                }
            }
        }
        private SolidColorBrush? _passwordBorderColor { get; set; }
        public SolidColorBrush? PasswordBorderColor
        {
            get
            { return _passwordBorderColor; }
            set
            {
                if (_passwordBorderColor != value)
                {
                    _passwordBorderColor = value;
                    OnPropertyChanged("PasswordBorderColor");
                }
            }
        }

        public ICommand LoginCommand { get; } 
        public ICommand CloseWindow { get; }
        public ICommand PasswordChanged { get; }
        public ICommand NavigateToRegister { get; }
        public ICommand NavigateToMainWindow { get; }
        public ICommand RememberUserCheckboxClickedCommand { get; }

        public LoginViewModel()
        {
            // Remember me checkbox
            RememberUserChecked = false;
            CheckboxVisibility = Visibility.Hidden;
            CheckboxBorderBackground = new SolidColorBrush(Color.FromRgb(189, 189, 189));

            // Login Password Inputs
            UserBorderColor = new SolidColorBrush(Colors.LightGray);
            PasswordBorderColor = new SolidColorBrush(Colors.LightGray);

            WindowSettings = new("ThisLoginWindow");
            WindowSettings.Initialize(1.8, 230, 260, true, "ThisLoginWindow");

            // Initialize Commands
            RememberUserCheckboxClickedCommand = new RelayCommand(() =>
            {
                RememberUserChecked = !RememberUserChecked;
            });

            NavigateToRegister = new ChangeWindowCommand(typeof(RegisterWindow), typeof(RegisterViewModel), true);
            NavigateToMainWindow = new ChangeWindowCommand(typeof(MainWindow), typeof(MainWindowViewModel), true);
            LoginCommand = new LoginCommand(this);
              
            PasswordChanged = new PasswordChangedCommand(value => Password = value);
            CloseWindow = new CloseWindowCommand(true);
        }
    }
}
