using DocumentFormat.OpenXml.Spreadsheet;
using ShippingScheduleMVVM.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingScheduleMVVM.Models
{
    public class RecordsStatus
    {
        public int Created { get; set; }
        public int Released { get; set; }
        public int Handled { get; set;}
        public int Prepared { get; set;}
        public int Shipped { get; set;}
        public int Notified { get; set;}
        public int Canceled { get; set;}
        public int Rescheduled { get; set;}
        
        public static RecordsStatus GetTotalStatus()
        {
            RecordsStatus rStatus = new();
            DatabaseOperations database = new();
            rStatus= database.CountStatus();

            return rStatus;
        }
        public static RecordsStatus GetDailyStatusDB(DateTime date)
        {
            RecordsStatus rStatus = new();
            DateTime previousDay;
            DatabaseOperations database = new();

            // If Monday
            if ((int)date.DayOfWeek == 1)
            {
                previousDay = date.AddDays(-3);
            }
            else { previousDay = date.AddDays(-1); }

            rStatus = database.SelectDailyStatus(previousDay);

            return rStatus;
        }
        public static RecordsStatus GetWeeklyStatus(ObservableCollection<ScheduleRow> scheduleRows) 
        {
            RecordsStatus rStatus = new();
            foreach (ScheduleRow row in scheduleRows)
            {
                AddRecordsDay(row.SundayRecords, rStatus);
                AddRecordsDay(row.MondayRecords, rStatus);
                AddRecordsDay(row.TuesdayRecords, rStatus);
                AddRecordsDay(row.WednesdayRecords, rStatus);
                AddRecordsDay(row.ThursdayRecords, rStatus);
                AddRecordsDay(row.FridayRecords, rStatus);
                AddRecordsDay(row.SaturdayRecords, rStatus);
            }
            return rStatus;
        }
        public static void AddRecordsDay(ObservableCollection<ScheduleRecord> day, RecordsStatus rStatus) 
        {
            foreach (ScheduleRecord record in day)
            {
                if (record.WasCancelled) rStatus.Canceled++;
                if (!record.WasNotified && record.ShipmentType != "Especial") rStatus.Notified++;
                if (record.WasRescheduled) rStatus.Rescheduled++;

                switch (record.Category)
                {
                    case "Created":
                        rStatus.Created++;
                        break;
                    case "Released":
                        rStatus.Released++;
                        break;
                    case "Process":
                        rStatus.Handled++;
                        break;
                    case "Prepared":
                        rStatus.Prepared++;
                        break;
                    case "Shipped":
                        rStatus.Shipped++;
                        break;
                }
            }
        }
        public static RecordsStatus GetDailyStatus(ObservableCollection<ScheduleRow> scheduleRows, DateTime date)
        {
            RecordsStatus rStatus = new();

            // Define a dictionary mapping each day of the week to its corresponding ScheduleRecord property
            Dictionary<DayOfWeek, Func<ScheduleRow, IEnumerable<ScheduleRecord>>> recordSelector = new()
            {
                { DayOfWeek.Sunday, row => row.SundayRecords },
                { DayOfWeek.Monday, row => row.MondayRecords },
                { DayOfWeek.Tuesday, row => row.TuesdayRecords },
                { DayOfWeek.Wednesday, row => row.WednesdayRecords },
                { DayOfWeek.Thursday, row => row.ThursdayRecords },
                { DayOfWeek.Friday, row => row.FridayRecords },
                { DayOfWeek.Saturday, row => row.SaturdayRecords }
            };

            // Use the dictionary to dynamically select the correct ScheduleRecord property based on the input date
            Func<ScheduleRow, IEnumerable<ScheduleRecord>> selector = recordSelector[date.DayOfWeek];
            foreach (ScheduleRow row in scheduleRows)
            {
                foreach (ScheduleRecord record in selector(row))
                {
                    if (record.WasCancelled) rStatus.Canceled++;
                    if (!record.WasNotified && record.ShipmentType != "Especial") rStatus.Notified++;
                    if (record.WasRescheduled) rStatus.Rescheduled++;

                    switch (record.Category)
                    {
                        case "Created":
                            rStatus.Created++;
                            break;
                        case "Released":
                            rStatus.Released++;
                            break;
                        case "Process":
                            rStatus.Handled++;
                            break;
                        case "Prepared":
                            rStatus.Prepared++;
                            break;
                        case "Shipped":
                            rStatus.Shipped++;
                            break;
                    }
                }
            }

            return rStatus;
        }

    }
}
