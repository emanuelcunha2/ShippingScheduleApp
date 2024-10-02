using ShippingScheduleMVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingScheduleMVVM.Commands
{
    public class DragRecordWeek : CommandBase
    {
        private ScheduleManagementViewModel _scheduleManagementViewModel;
        public DragRecordWeek(ScheduleManagementViewModel scheduleManagementViewModel)
        {
            _scheduleManagementViewModel = scheduleManagementViewModel;
        }
        public override void Execute(object? parameter)
        {
            var parameterString = parameter as string;

            if (parameterString != null)
            {
                if(parameterString == "DragRight")
                {
                    _scheduleManagementViewModel.IsDraggingWeekRight= true;
                }
                if (parameterString == "DragLeft")
                {
                    _scheduleManagementViewModel.IsDraggingWeekLeft = true;
                }
            }
        }
    }
}
