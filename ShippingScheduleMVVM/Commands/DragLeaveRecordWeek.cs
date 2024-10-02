using ShippingScheduleMVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingScheduleMVVM.Commands
{
    public class DragLeaveRecordWeek : CommandBase
    {
        private readonly ScheduleManagementViewModel _scheduleManagementViewModel;
        public DragLeaveRecordWeek(ScheduleManagementViewModel scheduleManagementViewModel)
        {
            _scheduleManagementViewModel = scheduleManagementViewModel;
        }
        public override void Execute(object? parameter)
        {
            _scheduleManagementViewModel.IsDraggingWeekLeft = false;
            _scheduleManagementViewModel.IsDraggingWeekRight = false;
            _scheduleManagementViewModel.ChangeWeekCounter = 0;
        }
    }
}
