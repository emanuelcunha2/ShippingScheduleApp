using ShippingScheduleMVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace ShippingScheduleMVVM.Models
{
    public class ScheduleRow : ViewModelBase
    {

        private string? _time;
        public string Time
        {
            get { return _time ?? ""; }
            set
            {
                if (_time != value)
                {
                    _time = value;
                    OnPropertyChanged("Time");
                }
            }
        }
        private DateTime _dateStart;
        public DateTime DateStart
        {
            get { return _dateStart; }
            set
            {
                if (_dateStart != value)
                {
                    _dateStart = value;
                    DateSU = value.Date.ToShortDateString() + "+" + Time;
                    DateMO = value.AddDays(1).Date.ToShortDateString() + "+" + Time;
                    DateTU = value.AddDays(2).Date.ToShortDateString() + "+" + Time;
                    DateWE = value.AddDays(3).Date.ToShortDateString() + "+" + Time;
                    DateTH = value.AddDays(4).Date.ToShortDateString() + "+" + Time;
                    DateFR = value.AddDays(5).Date.ToShortDateString() + "+" + Time;
                    DateST = value.AddDays(6).Date.ToShortDateString() + "+" + Time;
                }
            }
        }

        public string DateSU { get; set; } = string.Empty;
        public string DateMO { get; set; } = string.Empty;
        public string DateTU { get; set; } = string.Empty;
        public string DateWE { get; set; } = string.Empty;
        public string DateTH { get; set; } = string.Empty;
        public string DateFR { get; set; } = string.Empty;
        public string DateST { get; set; } = string.Empty;

        public int TagSU { get; set; } = 0;
        public int TagMO { get; set; } = 1;
        public int TagTU { get; set; } = 2;
        public int TagWE { get; set; } = 3;
        public int TagTH { get; set; } = 4;
        public int TagFR { get; set; } = 5;
        public int TagST { get; set; } = 6;

        public ObservableCollection<ScheduleRecord> SundayRecords { get; set; } = new();
        public ObservableCollection<ScheduleRecord> MondayRecords { get; set; } = new();
        public ObservableCollection<ScheduleRecord> TuesdayRecords { get; set; } = new();
        public ObservableCollection<ScheduleRecord> WednesdayRecords { get; set; } = new();
        public ObservableCollection<ScheduleRecord> ThursdayRecords { get; set; } = new();
        public ObservableCollection<ScheduleRecord> FridayRecords { get; set; } = new();
        public ObservableCollection<ScheduleRecord> SaturdayRecords { get; set; } = new();

        private Visibility _sundayVisibility = Visibility.Visible;
        public Visibility SundayVisibility
        {
            get { return _sundayVisibility; }
            set
            {
                if (_sundayVisibility != value)
                {
                    _sundayVisibility = value;
                    OnPropertyChanged("SundayVisibility");
                }
            }
        }

        private Visibility _mondayVisibility = Visibility.Visible;
        public Visibility MondayVisibility
        {
            get { return _mondayVisibility; }
            set
            {
                if (_mondayVisibility != value)
                {
                    _mondayVisibility = value;
                    OnPropertyChanged("MondayVisibility");
                }
            }
        }

        private Visibility _tuesdayVisibility = Visibility.Visible;
        public Visibility TuesdayVisibility
        {
            get { return _tuesdayVisibility; }
            set
            {
                if (_tuesdayVisibility != value)
                {
                    _tuesdayVisibility = value;
                    OnPropertyChanged("TuesdayVisibility");
                }
            }
        }

        private Visibility _wednesdayVisibility = Visibility.Visible;
        public Visibility WednesdayVisibility
        {
            get { return _wednesdayVisibility; }
            set
            {
                if (_wednesdayVisibility != value)
                {
                    _wednesdayVisibility = value;
                    OnPropertyChanged("WednesdayVisibility");
                }
            }
        }

        private Visibility _thursdayVisibility = Visibility.Visible;
        public Visibility ThursdayVisibility
        {
            get { return _thursdayVisibility; }
            set
            {
                if (_thursdayVisibility != value)
                {
                    _thursdayVisibility = value;
                    OnPropertyChanged("ThursdayVisibility");
                }
            }
        }

        private Visibility _fridayVisibility = Visibility.Visible;
        public Visibility FridayVisibility
        {
            get { return _fridayVisibility; }
            set
            {
                if (_fridayVisibility != value)
                {
                    _fridayVisibility = value;
                    OnPropertyChanged("FridayVisibility");
                }
            }
        }

        private Visibility _saturdayVisibility = Visibility.Visible;
        public Visibility SaturdayVisibility
        {
            get { return _saturdayVisibility; }
            set
            {
                if (_saturdayVisibility != value)
                {
                    _saturdayVisibility = value;
                    OnPropertyChanged("SaturdayVisibility");
                }
            }
        }

        public ScheduleRow(string time)
        {
            Time = time;
        }

        public static ObservableCollection<ScheduleRow> InitializeScheduleRows(DateTime date, bool showWhites, bool showDaily)
        {
            DateTime sunday = date.AddDays(-(int)date.DayOfWeek);

            ObservableCollection<ScheduleRecord> scheduleRecords = ScheduleRecord.GetScheduleRecords(sunday);

            ObservableCollection<ScheduleRow> scheduleRows = new();

            int hours = 0, minutes = -30, countRow = 0;

            // Loop time +30min until 23
            while (hours != 23 || minutes != 0)
            {
                minutes += 30;

                if (minutes == 60)
                {
                    minutes = 0;
                    hours++;
                }

                string hour = hours.ToString();
                string minute = minutes.ToString();

                if (hour.Length == 1) { hour = "0" + hour; }
                if (minute.Length == 1) { minute = "0" + minute; }

                ScheduleRow newScheduleRow = new(hour + ":" + minute)
                {
                    DateStart = sunday
                };

                scheduleRows.Add(newScheduleRow);
                countRow++;
            }
            InsertScheduleRecordsWeek(scheduleRows, scheduleRecords);

            if (!showWhites & !showDaily)
            {
                HideEmptyHourRows(scheduleRows);
            }

            if (showWhites & showDaily)
            {
                ShowDayColumnOnly(scheduleRows, (int)date.DayOfWeek);
            }

            if (!showWhites & showDaily)
            {
                HideEmptyHourRowsDaily(scheduleRows, (int)date.DayOfWeek);
                ShowDayColumnOnly(scheduleRows, (int)date.DayOfWeek);
            }

            return scheduleRows;
        }
        public static void InsertScheduleRecordsWeek(ObservableCollection<ScheduleRow> scheduleRows, ObservableCollection<ScheduleRecord> scheduleRecords)
        {
            foreach (ScheduleRecord record in scheduleRecords)
            {
                var scheduleRow = scheduleRows.FirstOrDefault(sr => sr.Time == record.Time);

                if ((int)record.Date.DayOfWeek == 0)
                {
                    scheduleRow?.SundayRecords.Add(record);
                }
                if ((int)record.Date.DayOfWeek == 1)
                {
                    scheduleRow?.MondayRecords.Add(record);
                }
                if ((int)record.Date.DayOfWeek == 2)
                {
                    scheduleRow?.TuesdayRecords.Add(record);
                }
                if ((int)record.Date.DayOfWeek == 3)
                {
                    scheduleRow?.WednesdayRecords.Add(record);
                }
                if ((int)record.Date.DayOfWeek == 4)
                {
                    scheduleRow?.ThursdayRecords.Add(record);
                }
                if ((int)record.Date.DayOfWeek == 5)
                {
                    scheduleRow?.FridayRecords.Add(record);
                }
                if ((int)record.Date.DayOfWeek == 6)
                {
                    scheduleRow?.SaturdayRecords.Add(record);
                }
            }
            foreach (ScheduleRow row in scheduleRows)
            {
                ScheduleRecord.InitializeRecordsStyle(row.SundayRecords);
                ScheduleRecord.InitializeRecordsStyle(row.MondayRecords);
                ScheduleRecord.InitializeRecordsStyle(row.TuesdayRecords);
                ScheduleRecord.InitializeRecordsStyle(row.WednesdayRecords);
                ScheduleRecord.InitializeRecordsStyle(row.ThursdayRecords);
                ScheduleRecord.InitializeRecordsStyle(row.FridayRecords);
                ScheduleRecord.InitializeRecordsStyle(row.SaturdayRecords);
            }
        }
        public static void UpdateScheduleRecordsWeek(DateTime date, ObservableCollection<ScheduleRow> scheduleRows, bool showWhites)
        {
            DateTime sunday = date.AddDays(-(int)date.DayOfWeek);
            ObservableCollection<ScheduleRecord> scheduleRecords = ScheduleRecord.GetScheduleRecords(sunday);

            ClearScheduleRowsDaysInfo(scheduleRows);
            SetScheduleRowsDate(scheduleRows, sunday);
            InsertScheduleRecordsWeek(scheduleRows, scheduleRecords);
        }
        public static void SetScheduleRowsDate(ObservableCollection<ScheduleRow> scheduleRows, DateTime date)
        {
            foreach (ScheduleRow row in scheduleRows)
            {
                row.DateStart = date;
            }
        }
        public static void SetScheduleRowsHitTest(ObservableCollection<ScheduleRow> scheduleRows, bool value)
        {
            foreach (ScheduleRow row in scheduleRows)
            {
                foreach (var record in row.SundayRecords)
                    record.HitTestVisibility = value;

                foreach (var record in row.MondayRecords)
                    record.HitTestVisibility = value;

                foreach (var record in row.TuesdayRecords)
                    record.HitTestVisibility = value;

                foreach (var record in row.WednesdayRecords)
                    record.HitTestVisibility = value;

                foreach (var record in row.ThursdayRecords)
                    record.HitTestVisibility = value;

                foreach (var record in row.FridayRecords)
                    record.HitTestVisibility = value;

                foreach (var record in row.SaturdayRecords)
                    record.HitTestVisibility = value;
            }
        }
        public static void SetScheduleRowsHitTestRow(ScheduleRow row, bool value)
        {
            foreach (var record in row.SundayRecords)
                record.HitTestVisibility = value;

            foreach (var record in row.MondayRecords)
                record.HitTestVisibility = value;

            foreach (var record in row.TuesdayRecords)
                record.HitTestVisibility = value;

            foreach (var record in row.WednesdayRecords)
                record.HitTestVisibility = value;

            foreach (var record in row.ThursdayRecords)
                record.HitTestVisibility = value;

            foreach (var record in row.FridayRecords)
                record.HitTestVisibility = value;

            foreach (var record in row.SaturdayRecords)
                record.HitTestVisibility = value;
        }
        public static void ClearScheduleRowsDaysInfo(ObservableCollection<ScheduleRow> scheduleRows)
        {
            foreach (ScheduleRow row in scheduleRows)
            {
                row.SundayRecords.Clear();
                row.MondayRecords.Clear();
                row.TuesdayRecords.Clear();
                row.WednesdayRecords.Clear();
                row.ThursdayRecords.Clear();
                row.FridayRecords.Clear();
                row.SaturdayRecords.Clear();
            }
        }
        public static void HideEmptyHourRows(ObservableCollection<ScheduleRow> scheduleRows)
        {
            ObservableCollection<ScheduleRow> scheduleRowsToDelete = new();
            foreach (ScheduleRow row in scheduleRows)
            {
                int countRecords = row.SundayRecords.Count + row.MondayRecords.Count + row.TuesdayRecords.Count + row.WednesdayRecords.Count + row.ThursdayRecords.Count
                                 + row.FridayRecords.Count + row.SaturdayRecords.Count;
                if (countRecords == 0)
                {
                    scheduleRowsToDelete.Add(row);
                }
            }
            foreach (ScheduleRow rowToDelete in scheduleRowsToDelete)
            {
                scheduleRows.Remove(rowToDelete);
            }
        }
        public static void HideEmptyHourRowsDaily(ObservableCollection<ScheduleRow> scheduleRows, int DayOfTheWeek)
        {
            ObservableCollection<ScheduleRow> scheduleRowsToDelete = new();

            switch (DayOfTheWeek)
            {
                case 0:
                    foreach (ScheduleRow row in scheduleRows)
                    {
                        int countRecords = row.SundayRecords.Count;
                        if (countRecords == 0)
                        {
                            scheduleRowsToDelete.Add(row);
                        }
                    }
                    break;
                case 1:
                    foreach (ScheduleRow row in scheduleRows)
                    {
                        int countRecords = row.MondayRecords.Count;
                        if (countRecords == 0)
                        {
                            scheduleRowsToDelete.Add(row);
                        }
                    }
                    break;
                case 2:
                    foreach (ScheduleRow row in scheduleRows)
                    {
                        int countRecords = row.TuesdayRecords.Count;
                        if (countRecords == 0)
                        {
                            scheduleRowsToDelete.Add(row);
                        }
                    }
                    break;
                case 3:
                    foreach (ScheduleRow row in scheduleRows)
                    {
                        int countRecords = row.WednesdayRecords.Count;
                        if (countRecords == 0)
                        {
                            scheduleRowsToDelete.Add(row);
                        }
                    }
                    break;
                case 4:
                    foreach (ScheduleRow row in scheduleRows)
                    {
                        int countRecords = row.ThursdayRecords.Count;
                        if (countRecords == 0)
                        {
                            scheduleRowsToDelete.Add(row);
                        }
                    }
                    break;
                case 5:
                    foreach (ScheduleRow row in scheduleRows)
                    {
                        int countRecords = row.FridayRecords.Count;
                        if (countRecords == 0)
                        {
                            scheduleRowsToDelete.Add(row);
                        }
                    }
                    break;
                case 6:
                    foreach (ScheduleRow row in scheduleRows)
                    {
                        int countRecords = row.SaturdayRecords.Count;
                        if (countRecords == 0)
                        {
                            scheduleRowsToDelete.Add(row);
                        }
                    }
                    break;
            }

            foreach (ScheduleRow rowToDelete in scheduleRowsToDelete)
            {
                scheduleRows.Remove(rowToDelete);
            }
        }
        public static string GetDateFromTag(ScheduleRow row, string tag)
        {
            Dictionary<string, Func<ScheduleRow, string>> DaySelector = new Dictionary<string, Func<ScheduleRow, string>>
            {
                { "0", row => row.DateSU },
                { "1", row => row.DateMO },
                { "2", row => row.DateTU },
                { "3", row => row.DateWE },
                { "4", row => row.DateTH },
                { "5", row => row.DateFR },
                { "6", row => row.DateST }
            };
            var selector = DaySelector[tag];
   
            return selector(row);
        }

        public static void ShowDayColumnOnly(ObservableCollection<ScheduleRow> scheduleRows, int DayOfTheWeek)
        {
            foreach (ScheduleRow row in scheduleRows)
            {
                switch (DayOfTheWeek)
                {
                    case 0:
                        row.SundayVisibility = Visibility.Visible;
                        row.MondayVisibility = Visibility.Collapsed;
                        row.TuesdayVisibility = Visibility.Collapsed;
                        row.WednesdayVisibility = Visibility.Collapsed;
                        row.ThursdayVisibility = Visibility.Collapsed;
                        row.FridayVisibility = Visibility.Collapsed;
                        row.SaturdayVisibility = Visibility.Collapsed;
                        break;
                    case 1:
                        row.SundayVisibility = Visibility.Collapsed;
                        row.MondayVisibility = Visibility.Visible;
                        row.TuesdayVisibility = Visibility.Collapsed;
                        row.WednesdayVisibility = Visibility.Collapsed;
                        row.ThursdayVisibility = Visibility.Collapsed;
                        row.FridayVisibility = Visibility.Collapsed;
                        row.SaturdayVisibility = Visibility.Collapsed;
                        break;
                    case 2:
                        row.SundayVisibility = Visibility.Collapsed;
                        row.MondayVisibility = Visibility.Collapsed;
                        row.TuesdayVisibility = Visibility.Visible;
                        row.WednesdayVisibility = Visibility.Collapsed;
                        row.ThursdayVisibility = Visibility.Collapsed;
                        row.FridayVisibility = Visibility.Collapsed;
                        row.SaturdayVisibility = Visibility.Collapsed;
                        break;
                    case 3:
                        row.SundayVisibility = Visibility.Collapsed;
                        row.MondayVisibility = Visibility.Collapsed;
                        row.TuesdayVisibility = Visibility.Collapsed;
                        row.WednesdayVisibility = Visibility.Visible;
                        row.ThursdayVisibility = Visibility.Collapsed;
                        row.FridayVisibility = Visibility.Collapsed;
                        row.SaturdayVisibility = Visibility.Collapsed;
                        break;
                    case 4:
                        row.SundayVisibility = Visibility.Collapsed;
                        row.MondayVisibility = Visibility.Collapsed;
                        row.TuesdayVisibility = Visibility.Collapsed;
                        row.WednesdayVisibility = Visibility.Collapsed;
                        row.ThursdayVisibility = Visibility.Visible;
                        row.FridayVisibility = Visibility.Collapsed;
                        row.SaturdayVisibility = Visibility.Collapsed;
                        break;
                    case 5:
                        row.SundayVisibility = Visibility.Collapsed;
                        row.MondayVisibility = Visibility.Collapsed;
                        row.TuesdayVisibility = Visibility.Collapsed;
                        row.WednesdayVisibility = Visibility.Collapsed;
                        row.ThursdayVisibility = Visibility.Collapsed;
                        row.FridayVisibility = Visibility.Visible;
                        row.SaturdayVisibility = Visibility.Collapsed;
                        break;
                    case 6:
                        row.SundayVisibility = Visibility.Collapsed;
                        row.MondayVisibility = Visibility.Collapsed;
                        row.TuesdayVisibility = Visibility.Collapsed;
                        row.WednesdayVisibility = Visibility.Collapsed;
                        row.ThursdayVisibility = Visibility.Collapsed;
                        row.FridayVisibility = Visibility.Collapsed;
                        row.SaturdayVisibility = Visibility.Visible;
                        break;
                }
            }
        }
    }
}
