using ShippingScheduleMVVM.ViewModels;
using System;
using System.Windows;

namespace ShippingScheduleMVVM.Commands
{
    public class ChangeWindowCommand : CommandBase
    {
        private Type _destinationWindow { get; set; }
        private Type _viewModel { get; set; }
        private bool _closeWindow { get; set; }
        public ChangeWindowCommand(Type destinationWindow, Type viewModel, bool closeWindow)
        {
            _destinationWindow = destinationWindow;
            _viewModel = viewModel;
            _closeWindow = closeWindow;
        }

        public override void Execute(object? parameter)
        {
            if (parameter is Window originWindow)
            {
                // Get the elements using the strings
                Window? destinationWindow = (Window?)Activator.CreateInstance(_destinationWindow);
                ViewModelBase? viewModel = (ViewModelBase?)Activator.CreateInstance(_viewModel);

                if (destinationWindow == null || viewModel == null) { return; }
                destinationWindow.DataContext = viewModel;
                destinationWindow.Show();

                if (_closeWindow)
                    originWindow.Close();
            }
        }
    }
}
