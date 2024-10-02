using ShippingScheduleMVVM.Services;
using ShippingScheduleMVVM.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;

namespace ShippingScheduleMVVM.Models
{
    public class ScheduleRecord : ViewModelBase
    {
        public string TransportMode { get; set; } = string.Empty;
        private bool _hitTestVisibility = true;
        public bool HitTestVisibility
        {
            get { return _hitTestVisibility; }
            set
            {
                if (_hitTestVisibility != value)
                {
                    _hitTestVisibility = value;
                    OnPropertyChanged(nameof(HitTestVisibility));
                }
            }
        }
        public int _id;
        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
                if (ShipmentType == "Regular")
                {
                    IdDescription = "#R" + value;
                    return;
                }

                if (ShipmentType == "Especial")
                {
                    IdDescription = "#S" + value;
                    return;
                }
                IdDescription = "Erro";
            }
        }
        public string IdDescription { get; set; } = string.Empty;
        public int _paleteNumber;
        public int PaleteNumber
        {
            get { return _paleteNumber; }
            set
            {
                _paleteNumber = value;
                PaleteNumberDescription = value + " Pallets";
            }
        }
        public string PaleteNumberDescription { get; set; } = "0 Pallets";
        public string Plate { get; set; } = string.Empty;
        public string Time { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string ShipmentType { get; set; } = string.Empty;
        public bool WasNotified { get; set; }
        public bool WasRescheduled { get; set; }
        public bool WasCancelled { get; set; }
        public string OriginalSchedule { get; set; } = string.Empty;
        public string ShipTos { get; set; } = string.Empty;
        public string Carrier { get; set; } = string.Empty;
        private DateTime _shippedDate;
        public DateTime ShippedDate
        {
            get { return _shippedDate; }
            set
            {
                _shippedDate = value;
                ShippedDateDescription = ShippedDate.ToString("HH:mm");
            }
        }
        public string ShippedDateDescription { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public DateTime PreparedDate { get; set; }
        public Uri NotificationImageSource { get; set; } = new Uri("pack://application:,,,/ShippingScheduleMVVM;component/Resources/Images/Danger_r.png");
        public Visibility AddRecordVisibility { get; set; } = Visibility.Visible;
        public Visibility RescheduledVisibility { get; set; } = Visibility.Collapsed;
        public Visibility CanceledVisibility { get; set; } = Visibility.Collapsed;
        public Visibility ShippedHourVisibility { get; set; } = Visibility.Collapsed;
        public Visibility RecordBottomInfoVisibility { get; set; } = Visibility.Visible;
        public Visibility NotificationAlertVisibility { get; set; } = Visibility.Collapsed;
        public SolidColorBrush BackgroundColor { get; set; } = new SolidColorBrush(Colors.White);
        public SolidColorBrush OpacityMask { get; set; } = new SolidColorBrush(Colors.White);
        public CornerRadius Corner { get; set; } = new CornerRadius(3, 27, 3, 3);


        public GridLength PercentagePlanTruck { get; set; } = new GridLength(1,GridUnitType.Star);
        public GridLength PercentagePlanTruckTotal { get; set; } = new GridLength(22, GridUnitType.Star);

        public GridLength PercentagePreparedTruck { get; set; } = new GridLength(1, GridUnitType.Star);
        public GridLength PercentagePreparedTruckTotal { get; set; } = new GridLength(22, GridUnitType.Star);

        public GridLength PercentageActualTruck { get; set; } = new GridLength(1, GridUnitType.Star);
        public GridLength PercentageActualTruckTotal { get; set; } = new GridLength(22, GridUnitType.Star);

        public static ObservableCollection<ScheduleRecord> GetScheduleRecords(DateTime date)
        {
            DatabaseOperations database = new();
            ObservableCollection<ScheduleRecord> scheduleRecords = database.SelectScheduleRecords(date);
            return scheduleRecords;
        }

        public static ObservableCollection<ScheduleRecord> SetScheduleRecordsDayMargins(DateTime date)
        {
            DatabaseOperations database = new();
            ObservableCollection<ScheduleRecord> scheduleRecords = database.SelectScheduleRecordsDay(date);
            scheduleRecords = SetTrucksMargins(scheduleRecords);

            return scheduleRecords;
        }
        public static ObservableCollection<ScheduleRecord> SetTrucksMargins(ObservableCollection<ScheduleRecord> scheduleRecords)
        {
            foreach(ScheduleRecord record in scheduleRecords)
            {
                 
                // Plan
                TimeSpan planTime = TimeSpan.Parse(record.Time);
                var pi1 = planTime.Hours + (planTime.Minutes / 60.0f) - 6;
                var pi2 = 17.0f - pi1;
                // Prepared
                var pi3 = record.PreparedDate.Hour + (record.PreparedDate.Minute / 60.0f) - 6;
                var pi4 = 17.0f - pi3;
                // Actual
                var pi5 = record.ShippedDate.Hour + (record.ShippedDate.Minute / 60.0f) - 6;
                var pi6 = 17.0f - pi5;

                if (record.PreparedDate == DateTime.MinValue)
                {
                    pi3 = 0;
                    pi4 = 17;
                }

                record.PercentagePlanTruck = new GridLength(pi1, GridUnitType.Star);
                record.PercentagePlanTruckTotal = new GridLength(pi2, GridUnitType.Star);

                record.PercentagePreparedTruck = new GridLength(pi3, GridUnitType.Star);
                record.PercentagePreparedTruckTotal = new GridLength(pi4, GridUnitType.Star);

                record.PercentageActualTruck = new GridLength(pi5, GridUnitType.Star);
                record.PercentageActualTruckTotal = new GridLength(pi6, GridUnitType.Star); 

            }
            return scheduleRecords;
        }
        public static void InitializeRecordsStyle(ObservableCollection<ScheduleRecord> scheduleRecords)
        {
            int count = 0;
            foreach (ScheduleRecord record in scheduleRecords)
            {
                if (scheduleRecords.Count > 1)
                {
                    record.AddRecordVisibility = Visibility.Collapsed;
                    record.Corner = new CornerRadius(3);
                }
                if (record.WasRescheduled) { record.RescheduledVisibility = Visibility.Visible; }
                if (record.WasCancelled) { record.CanceledVisibility = Visibility.Visible; }

                record.BackgroundColor = Models.Category.GetColorFromCategory(record.Category); 

                if (record.ShipmentType == "Regular")
                {
                    record.NotificationAlertVisibility = Visibility.Visible;

                    if (!record.WasNotified)
                    {
                        record.NotificationImageSource = new Uri("pack://application:,,,/ShippingScheduleMVVM;component/Resources/Images/Danger_r.png");
                    }
                    else
                    {
                        record.NotificationImageSource = new Uri("pack://application:,,,/ShippingScheduleMVVM;component/Resources/Images/Check_g.png");
                    }
                }


                if (record.Category == "Shipped")
                {
                    record.ShippedHourVisibility = Visibility.Visible;
                    record.NotificationAlertVisibility = Visibility.Collapsed;
                }
                else
                {
                    if (!record.WasCancelled) { record.RecordBottomInfoVisibility = Visibility.Collapsed; }
                }


                if (record.TransportMode == "DHL")
                {
                    record.NotificationAlertVisibility = Visibility.Visible;
                    record.NotificationImageSource = new Uri("pack://application:,,,/ShippingScheduleMVVM;component/Resources/Images/dhl.png");
                }

                if (record.TransportMode == "TNT")
                {
                    record.NotificationAlertVisibility = Visibility.Visible;
                    record.NotificationImageSource = new Uri("pack://application:,,,/ShippingScheduleMVVM;component/Resources/Images/tnt.png");
                }



                count++;
            }
        }
        public ScheduleRecord Clone()
        {
            ScheduleRecord newRecord = new ScheduleRecord
            {
                HitTestVisibility = this.HitTestVisibility,
                Id = this.Id,
                IdDescription = this.IdDescription,
                PaleteNumber = this.PaleteNumber,
                PaleteNumberDescription = this.PaleteNumberDescription,
                Plate = this.Plate,
                Time = this.Time,
                Category = this.Category,
                ShipmentType = this.ShipmentType,
                WasNotified = this.WasNotified,
                WasRescheduled = this.WasRescheduled,
                WasCancelled = this.WasCancelled,
                OriginalSchedule = this.OriginalSchedule,
                ShipTos = this.ShipTos,
                ShippedDate = this.ShippedDate,
                ShippedDateDescription = this.ShippedDateDescription,
                Date = this.Date,
                NotificationImageSource = new Uri(this.NotificationImageSource.ToString()),
                AddRecordVisibility = this.AddRecordVisibility,
                RescheduledVisibility = this.RescheduledVisibility,
                CanceledVisibility = this.CanceledVisibility,
                ShippedHourVisibility = this.ShippedHourVisibility,
                RecordBottomInfoVisibility = this.RecordBottomInfoVisibility,
                NotificationAlertVisibility = this.NotificationAlertVisibility,
                BackgroundColor = new SolidColorBrush(this.BackgroundColor.Color),
                Corner = new CornerRadius(this.Corner.TopLeft, this.Corner.TopRight, this.Corner.BottomRight, this.Corner.BottomLeft)
            };
            return newRecord;
        }
    }
}
