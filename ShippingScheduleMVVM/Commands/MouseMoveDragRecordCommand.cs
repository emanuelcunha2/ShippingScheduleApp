using ShippingScheduleMVVM.Converters;
using ShippingScheduleMVVM.Models;
using ShippingScheduleMVVM.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace ShippingScheduleMVVM.Commands
{
    public class MouseMoveDragRecordCommand : CommandBase
    {
        private ScheduleManagementViewModel _scheduleManagementViewModel;
        public MouseMoveDragRecordCommand(ScheduleManagementViewModel scheduleManagementViewModel)
        {
            _scheduleManagementViewModel = scheduleManagementViewModel;
        }
        public override void Execute(object? parameter)
        {
            EventParemeters? parameters = (EventParemeters?)parameter;
            if (parameters == null) { return; }

            MouseEventArgs e = (MouseEventArgs)parameters.E;
            Border border = (Border)parameters.Sender;

            if (e.LeftButton == MouseButtonState.Pressed && !_scheduleManagementViewModel.IsRecordOpen)
            {
                if (!border.IsLoaded) 
                {
                    return;
                }
                // Create a copy of the border
                ScheduleRecord newScheduleRecord = ((ScheduleRecord)border.DataContext).Clone();

                // Set Permissions
                if (!RolePermissions.CanDragRecord(newScheduleRecord.Category)) { return; }

                _scheduleManagementViewModel.IsDraggingRecord = true;
                _scheduleManagementViewModel.VisualDragVisibility = Visibility.Visible;
                _scheduleManagementViewModel.Brush = new System.Windows.Media.SolidColorBrush(Color.FromRgb(220, 230, 247));
                DragDrop.DoDragDrop(border, newScheduleRecord, DragDropEffects.Move);
                _scheduleManagementViewModel.VisualDragVisibility = Visibility.Hidden;
                _scheduleManagementViewModel.IsDraggingRecord = false;
                _scheduleManagementViewModel.Brush = new System.Windows.Media.SolidColorBrush(Color.FromRgb(250, 250, 250));

                _scheduleManagementViewModel.IsDraggingWeekRight = false;
                _scheduleManagementViewModel.IsDraggingWeekLeft = false;

                ScheduleRow.SetScheduleRowsHitTest(_scheduleManagementViewModel.ScheduleRows, true);
            }
        }
    }
}