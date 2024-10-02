using System;
using System.Windows;

namespace ShippingScheduleMVVM.Commands
{
    class MaximizeWindowCommand : CommandBase
    {
        private readonly Func<WindowState> _getWindowState;
        private readonly Action _maximizeWindow;
        private readonly Action<double> _normalizeWindow;
        private readonly Action<WindowState> _adjustUI;
        private readonly double _scale;
        public MaximizeWindowCommand(Func<WindowState> getWindowState, Action<double> normalizeWindow, Action maximizeWindow, double scale, Action<WindowState> adjustUI)
        {
            _getWindowState = getWindowState;
            _maximizeWindow = maximizeWindow;
            _normalizeWindow = normalizeWindow;
            _scale = scale;
            _adjustUI = adjustUI;
        }
        public override void Execute(object? parameter)
        {
            if (_getWindowState() == WindowState.Normal)
            {
                _adjustUI(WindowState.Maximized);
                _maximizeWindow();
                return;
            }
            _adjustUI(WindowState.Normal);
            _normalizeWindow(_scale);
        }
    }
}
