using System;
using System.Windows;

namespace ShippingScheduleMVVM.Commands
{
    class MinimizeWindowCommand : CommandBase
    {
        private Func<WindowState> _getWindowState;
        private Action<WindowState> _updateWindowState;
        public MinimizeWindowCommand(Func<WindowState> getWindowState, Action<WindowState> updateWindowState)
        {
            _getWindowState = getWindowState;
            _updateWindowState = updateWindowState;
        }
        public override void Execute(object? parameter)
        {
            if (_getWindowState() == WindowState.Normal || _getWindowState() == WindowState.Maximized)
            {
                _updateWindowState(WindowState.Minimized);
            }
        }
    }
}
