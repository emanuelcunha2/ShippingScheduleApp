using System;
using System.Windows.Controls;

namespace ShippingScheduleMVVM.Commands
{
    class PasswordChangedCommand : CommandBase
    {
        private readonly Action<string?> _updatePassword;
        public PasswordChangedCommand(Action<string?> updatePassword)
        {
            _updatePassword = updatePassword;
        }

        public override void Execute(object? parameter)
        {
            if (parameter is PasswordBox)
            {
                string password = ((PasswordBox)parameter).Password;
                _updatePassword(password);
            }
        }
    }
}
