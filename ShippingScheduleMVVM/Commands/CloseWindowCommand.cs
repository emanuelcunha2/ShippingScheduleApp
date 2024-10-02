using System;
using System.Windows;

namespace ShippingScheduleMVVM.Commands
{
    class CloseWindowCommand : CommandBase
    {
        private bool _closeApplication { get; set; }
        public CloseWindowCommand(bool closeApplication)
        {
            _closeApplication = closeApplication;
        }
        public override void Execute(object? parameter)
        {
            if (parameter is Window)
            {
                Window originWindow = (Window)parameter;
                originWindow.Close();
                if (_closeApplication) { Application.Current.Shutdown(); }
            }
        }
    }
}
