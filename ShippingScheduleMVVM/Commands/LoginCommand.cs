using Microsoft.IdentityModel.Tokens;
using ShippingScheduleMVVM.Models;
using ShippingScheduleMVVM.ViewModels;
using ShippingScheduleMVVM.Views.Modals;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace ShippingScheduleMVVM.Commands
{
    public class LoginCommand : CommandBase
    {
        private readonly LoginViewModel _loginViewModel;
        public LoginCommand(LoginViewModel loginViewModel)
        {
            _loginViewModel = loginViewModel;
            _loginViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(LoginViewModel.Username) || e.PropertyName == nameof(LoginViewModel.Password))
            {
                _loginViewModel.UserBorderColor = new SolidColorBrush(Colors.LightGray);
                _loginViewModel.PasswordBorderColor = new SolidColorBrush(Colors.LightGray);

                RaiseCanExecuteChanged();
            }
        }
        public override bool CanExecute(object? parameter)
        {
            if (_loginViewModel.Username.IsNullOrEmpty())
            {
                _loginViewModel.UserBorderColor = new SolidColorBrush(Color.FromRgb(255, 77, 79)); // LightRed
            }
            if (_loginViewModel.Password.IsNullOrEmpty())
            {
                _loginViewModel.PasswordBorderColor = new SolidColorBrush(Color.FromRgb(255, 77, 79)); // LightRed
            }

            return !_loginViewModel.Username.IsNullOrEmpty()
                && !_loginViewModel.Password.IsNullOrEmpty()
                && base.CanExecute(parameter);
        }
        public override void Execute(object? parameter)
        {
            User user = new(_loginViewModel.Username ?? "", _loginViewModel.Password ?? "");
            // <SucessStatus,StatusDescription>
            Tuple<bool, string, User?> response = user.Login();

            if (response.Item1 == true)
            {
                if (response.Item3 != null) { App.loggedUser = response.Item3; }
                user.SetUserCredentials(_loginViewModel.RememberUserChecked);

                _loginViewModel.NavigateToMainWindow.Execute(parameter);
            }

            if (response.Item1 == false)
            {
                user.SetUserCredentials(_loginViewModel.RememberUserChecked);
                CustomAlertWindow alert = new CustomAlertWindow("Erro Login", response.Item2, _loginViewModel);
                alert.Owner = Application.Current.MainWindow;
                alert.ShowDialog();
            }
        }
    }
}
