using ShippingScheduleMVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingScheduleMVVM.Commands
{
    public class RefreshRecordsCommand : CommandBase
    {
        private ScheduleManagementViewModel _scheduleManagementViewModel;
        public RefreshRecordsCommand(ScheduleManagementViewModel scheduleManagementViewModel)
        {
            _scheduleManagementViewModel = scheduleManagementViewModel;
        }
        public override void Execute(object? parameter)
        {
            _scheduleManagementViewModel.UpdateTimerManualy();
        }
    }
}
