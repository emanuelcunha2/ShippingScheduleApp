using ShippingScheduleMVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ShippingScheduleMVVM.Models
{
    public class RecordDayStatistics : ViewModelBase
    {
        private int _paletsNumber { get; set; } = 0;
        public int PaletsNumber
        {
            get { return _paletsNumber; }
            set
            {
                _paletsNumber = value;
                OnPropertyChanged(nameof(PaletsNumber));
            }
        }
        private int _regularsNumber { get; set; } = 0;
        public int RegularsNumber
        {
            get { return _regularsNumber; }
            set
            {
                _regularsNumber = value;
                OnPropertyChanged(nameof(RegularsNumber));
            }
        }
        private int _specialsNumber { get; set; } = 0;
        public int SpecialsNumber
        {
            get { return _specialsNumber; }
            set
            {
                _specialsNumber = value;
                OnPropertyChanged(nameof(SpecialsNumber));
            }
        }
        public Visibility Visibility { get; set; } = 0;
        public RecordDayStatistics() { }

        public void CountRecords(ObservableCollection<ScheduleRow> rows, string day)
        {
            Dictionary<string, Func<ScheduleRow, ObservableCollection<ScheduleRecord>>> dayToPropertyMap = new Dictionary<string, Func<ScheduleRow, ObservableCollection<ScheduleRecord>>>
            {
                { "Monday", row => row.MondayRecords },
                { "Tuesday", row => row.TuesdayRecords },
                { "Wednesday", row => row.WednesdayRecords },
                { "Thursday", row => row.ThursdayRecords },
                { "Friday", row => row.FridayRecords },
                { "Saturday", row => row.SaturdayRecords },
                { "Sunday", row => row.SundayRecords }
            };

            Dictionary<string, Func<ScheduleRow, Visibility>> dayToVisibilityPropertyMap = new Dictionary<string, Func<ScheduleRow, Visibility>>
            {
                { "Monday", row => row.MondayVisibility },
                { "Tuesday", row => row.TuesdayVisibility },
                { "Wednesday", row => row.WednesdayVisibility },
                { "Thursday", row => row.ThursdayVisibility },
                { "Friday", row => row.FridayVisibility },
                { "Saturday", row => row.SaturdayVisibility },
                { "Sunday", row => row.SundayVisibility }
            };

            if (dayToPropertyMap.ContainsKey(day) && dayToVisibilityPropertyMap.ContainsKey(day))
            {
                Func<ScheduleRow, ObservableCollection<ScheduleRecord>> getProperty = dayToPropertyMap[day];
                Func<ScheduleRow, Visibility> getVisibilityProperty = dayToVisibilityPropertyMap[day];

                foreach (ScheduleRow row in rows)
                {
                    ObservableCollection<ScheduleRecord> records = getProperty(row);
                    Visibility visibility = getVisibilityProperty(row);

                    foreach (ScheduleRecord record in records)
                    {
                        PaletsNumber += record.PaleteNumber;
                        if (record.ShipmentType == "Regular") { RegularsNumber++; }
                        if (record.ShipmentType == "Especial") { SpecialsNumber++; }
                    }

                    if (visibility == Visibility.Collapsed)
                    {
                        Visibility = Visibility.Collapsed;
                    }
                }
            }
        }
        public static RecordDayStatistics CreateStatisticsDay(ObservableCollection<ScheduleRow> rows, string day)
        {
            RecordDayStatistics dayStatistics = new RecordDayStatistics();
            dayStatistics.CountRecords(rows, day);
            return dayStatistics;
        }
        public static ObservableCollection<RecordDayStatistics> CreateStatisticsWeek(ObservableCollection<ScheduleRow> rows)
        {
            ObservableCollection<RecordDayStatistics> newStatistics = new ObservableCollection<RecordDayStatistics>();

            // Create statistics for each day of the week
            foreach (DayOfWeek dayOfWeek in Enum.GetValues(typeof(DayOfWeek)))
            {
                string day = dayOfWeek.ToString();
                RecordDayStatistics dayStatistics = new RecordDayStatistics();
                dayStatistics.CountRecords(rows, day);
                newStatistics.Add(dayStatistics);
            }

            return newStatistics;
        }

    }
}
