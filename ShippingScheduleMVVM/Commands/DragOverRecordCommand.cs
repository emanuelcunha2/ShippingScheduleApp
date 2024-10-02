using ShippingScheduleMVVM.Converters;
using ShippingScheduleMVVM.Models;
using System;
using System.Windows;
using System.Windows.Controls;

namespace ShippingScheduleMVVM.Commands
{
    public class DragOverRecordCommand : CommandBase
    {
        public DragOverRecordCommand()
        {

        }
        public override void Execute(object? parameter)
        {
            EventParemeters? parameters = (EventParemeters?)parameter;
            if (parameters == null) { return; }

            DragEventArgs e = (DragEventArgs)parameters.E;
            Border border = (Border)parameters.Sender;
            var record = (ScheduleRecord?)e.Data.GetData(typeof(ScheduleRecord));
            var row = (ScheduleRow)border.DataContext;
            var day = (int?)border.Tag;

            if (e == null || border == null || row == null || day == null || record == null) { return; }

            record.HitTestVisibility = false;
            record.Corner = new CornerRadius(3);
            record.AddRecordVisibility = Visibility.Collapsed;
            record.OpacityMask.Opacity = 0.5;

            DateTime droppedDate = record.Date;
            DateTime[] receivingDates = { row.DateStart, row.DateStart.AddDays(1), row.DateStart.AddDays(2), row.DateStart.AddDays(3), row.DateStart.AddDays(4), row.DateStart.AddDays(5), row.DateStart.AddDays(6) };
            DateTime receivingDate = receivingDates[(int)day];

            if (droppedDate.Date == receivingDate.Date && row.Time == record.Time) { return; }

            ScheduleRow.SetScheduleRowsHitTestRow(row, false);
            switch (day)
            {
                case 0:
                    row.SundayRecords.Add(record);
                    break;
                case 1:
                    row.MondayRecords.Add(record);
                    break;
                case 2:
                    row.TuesdayRecords.Add(record);
                    break;
                case 3:
                    row.WednesdayRecords.Add(record);
                    break;
                case 4:
                    row.ThursdayRecords.Add(record);
                    break;
                case 5:
                    row.FridayRecords.Add(record);
                    break;
                case 6:
                    row.SaturdayRecords.Add(record);
                    break;
            }
        }
    }
}
