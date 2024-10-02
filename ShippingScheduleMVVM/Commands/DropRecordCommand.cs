using ShippingScheduleMVVM.Models;
using ShippingScheduleMVVM.Services;
using ShippingScheduleMVVM.ViewModels;
using ShippingScheduleMVVM.ViewModels.Modals;
using ShippingScheduleMVVM.Views.Modals;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ShippingScheduleMVVM.Commands
{
    public class DropRecordCommand : CommandBase
    {
        private readonly ScheduleManagementViewModel _scheduleManagementViewModel;
        public DropRecordCommand(ScheduleManagementViewModel scheduleManagementViewModel)
        {
            _scheduleManagementViewModel = scheduleManagementViewModel;
        }
        public override void Execute(object? parameter)
        { 
            var e = (DragEventArgs?)parameter;
            if (e == null) { return; }

            if (e.Data.GetData(typeof(ScheduleRecord)) is not ScheduleRecord record) { return; }

            System.Windows.Window? owner = Application.Current.Windows.OfType<System.Windows.Window>().FirstOrDefault(w => w.Title == "ThisMainWindow");
            if (owner == null) { return; }

            if(e.OriginalSource is Border borderDrop)
            {
                if (borderDrop.DataContext is not ScheduleRow row) { return; }

                var tag = borderDrop.Tag.ToString();
                if (tag == null) { return; }

                var time = row.Time;
                var date = ScheduleRow.GetDateFromTag(row,tag);
                var dropDate = Record.GetDateFromTag(date);
                var originDate = DateTime.Parse(record.Date.ToString("dd/MM/yyyy") + " " + record.Time + ":00");
                // If its trying to drop on the same date
                if (dropDate == originDate) { return; }

                ViewModelBase parentViewModel = null;
                // Since the parentViewModel isn't the direct parent we have to find the real parentViewModel
                foreach( Window xWindow in Application.Current.Windows)
                {
                    if(xWindow.DataContext is MainWindowViewModel xViewModel)
                    {
                       if(xViewModel.CalendarViewModel == _scheduleManagementViewModel) { parentViewModel = xViewModel; break; }
                    }
                }
                var window = new CustomAlertWindow("Change Schedule?", "Are you sure you want to change this record's schedule to: [" + dropDate.ToString("dd/MM/yyyy") + " " + time + "]?", parentViewModel ?? new ViewModelBase());

                window.Owner = owner;

                _scheduleManagementViewModel.CanUpdateTimer = false;
                window.ShowDialog();
                _scheduleManagementViewModel.CanUpdateTimer = true;

                if (window.DataContext is CustomAlertWindowViewModel vm)
                {
                    if (vm.WasConfirmed)
                    {
                        DatabaseOperations database = new();
                        database.UpdateRecordDate(time, dropDate.Date, record.Id.ToString(), record.Category);
                        _scheduleManagementViewModel.UpdateTimerManualy();

                        var changedRecord = database.SelectRecordById(record.Id);
                        var changedRecordParts = database.SelectRecordParts(record.Id);
                        Mail.SendMailOnReschedule(changedRecord, changedRecordParts, record.OriginalSchedule);
                        Mail.SendMailIfCreatedRecordToday(changedRecord, changedRecordParts, record.OriginalSchedule);
                        return;
                    }
                    _scheduleManagementViewModel.UpdateTimerManualy();
                    return;
                }
            }
        }
    }
}
