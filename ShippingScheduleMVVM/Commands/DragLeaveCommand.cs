using ShippingScheduleMVVM.Converters;
using ShippingScheduleMVVM.Models;
using System;
using System.Windows;
using System.Windows.Controls;

namespace ShippingScheduleMVVM.Commands
{
    public class DragLeaveCommand : CommandBase
    {
        public DragLeaveCommand()
        {

        }
        public override void Execute(object? parameter)
        {
            EventParemeters? parameters = (EventParemeters?)parameter;
            if (parameters == null) { return; }

            DragEventArgs e = (DragEventArgs)parameters.E;
            Border border = (Border)parameters.Sender;
            var record = (ScheduleRecord?)e.Data.GetData(typeof(ScheduleRecord)); ;
            var row = (ScheduleRow)border.DataContext;
            var day = (int?)border.Tag;

            if (e == null || border == null || row == null || day == null || record == null) { return; }

            DateTime droppedDate = record.Date;
            DateTime[] receivingDates = { row.DateStart, row.DateStart.AddDays(1), row.DateStart.AddDays(2), row.DateStart.AddDays(3), row.DateStart.AddDays(4), row.DateStart.AddDays(5), row.DateStart.AddDays(6) };
            DateTime receivingDate = receivingDates[(int)day];

            if (droppedDate.Date == receivingDate.Date && row.Time == record.Time) { return; }

            switch (day)
            {
                case 0:
                    row.SundayRecords.Remove(record);
                    break;
                case 1:
                    row.MondayRecords.Remove(record);
                    break;
                case 2:
                    row.TuesdayRecords.Remove(record);
                    break;
                case 3:
                    row.WednesdayRecords.Remove(record);
                    break;
                case 4:
                    row.ThursdayRecords.Remove(record);
                    break;
                case 5:
                    row.FridayRecords.Remove(record);
                    break;
                case 6:
                    row.SaturdayRecords.Remove(record);
                    break;
            }
        }
    }
}
