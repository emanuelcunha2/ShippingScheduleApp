using System;

namespace ShippingScheduleMVVM.Commands
{
    public class RecalculateWindowCommand : CommandBase
    {
        private Action _updateWindow;
        public RecalculateWindowCommand(Action updateWindow)
        {
            _updateWindow = updateWindow;
        }
        public override void Execute(object? parameter)
        {
            _updateWindow();
        }
    }
}
