using Microsoft.IdentityModel.Tokens;
using ShippingScheduleMVVM.Models;
using ShippingScheduleMVVM.ViewModels;
using System;
using System.Net.Mail;
using System.Windows;

namespace ShippingScheduleMVVM.Commands
{
    public class RegisterCommand : CommandBase
    {
        private readonly RegisterViewModel _registerViewModel;
        public RegisterCommand(RegisterViewModel registerViewModel)
        {
            _registerViewModel = registerViewModel;
        }
        static bool IsValidEmail(string email)
        {
            try
            {
                if (email.IsNullOrEmpty()) { return false; }
                MailAddress mailAddress = new MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
        public override void Execute(object? parameter)
        {
            if (!IsValidEmail(_registerViewModel.Email ?? ""))
            {
                MessageBox.Show("This email isn't valid:" + _registerViewModel.Email ?? "");
                return;
            }
            User user = new(_registerViewModel.Username ?? "", _registerViewModel.Password ?? "", _registerViewModel.Email ?? "");

            user.Register();
        }
    }
}
