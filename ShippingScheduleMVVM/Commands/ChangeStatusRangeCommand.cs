using ShippingScheduleMVVM.ViewModels;

namespace ShippingScheduleMVVM.Commands
{
    public class ChangeStatusRangeCommand : CommandBase
    {
        private readonly ScheduleManagementViewModel _scheduleManagementViewModel;
        public ChangeStatusRangeCommand(ScheduleManagementViewModel scheduleManagementViewModel)
        {
            _scheduleManagementViewModel = scheduleManagementViewModel;
        }
        public override void Execute(object? parameter)
        {
            if (_scheduleManagementViewModel.IsDailyStatus)
            {
                _scheduleManagementViewModel.IsDailyStatus = false;
                return;
            }
            _scheduleManagementViewModel.IsDailyStatus = true;
        }
    }
}
