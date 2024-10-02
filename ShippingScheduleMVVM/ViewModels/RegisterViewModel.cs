using ShippingScheduleMVVM.Commands;
using ShippingScheduleMVVM.Models;
using ShippingScheduleMVVM.Services;
using ShippingScheduleMVVM.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ShippingScheduleMVVM.ViewModels
{
    public class RegisterViewModel : ViewModelBase
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
        private string? _email { get; set; }
        public string? Email
        {
            get
            { return _email; }
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged("Password");
                }
            }
        }
        private string? _role { get; set; }
        public string? Role
        {
            get
            { return _role; }
            set
            {
                if (_role != value)
                {
                    _role = value;
                    OnPropertyChanged("Role");
                }
            }
        }
        private ObservableCollection<string>? _roles { get; set; }
        public ObservableCollection<string>? Roles
        {
            get
            { return _roles; }
            set
            {
                if (_roles != value)
                {
                    _roles = value;
                    OnPropertyChanged("Roles");
                }
            }
        }
        public ICommand CloseWindow { get; }
        public ICommand NavigateToLogin { get; }
        public ICommand PasswordChanged { get; }
        public ICommand RegisterCommand { get; }
        public RegisterViewModel()
        {
            DatabaseOperations database = new();
            Roles = database.GetRoles() ?? new ObservableCollection<string>();

            WindowSettings = new("ThisRegisterWindow");
            WindowSettings.Initialize(1.8, 230, 280, true, "ThisRegisterWindow");

            // Initialize Commands
            CloseWindow = new CloseWindowCommand(true);
            NavigateToLogin = new ChangeWindowCommand(typeof(LoginWindow), typeof(LoginViewModel), true);
            PasswordChanged = new PasswordChangedCommand(value => Password = value);
            RegisterCommand = new RegisterCommand(this);
        }
    }
}
