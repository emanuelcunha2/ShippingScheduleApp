using ShippingScheduleMVVM.ViewModels;
using System;

namespace ShippingScheduleMVVM.SharedServices
{
    public class SharedDateDataService : ViewModelBase
    {
        private DateTime _currentlySelectedDate { get; set; } = DateTime.MinValue;
        public DateTime CurrentlySelectedDate
        {
            get { return _currentlySelectedDate; }
            set
            {
                if (_currentlySelectedDate != value)
                {
                    _currentlySelectedDate = value;
                    SelectedDateChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        public event EventHandler? SelectedDateChanged;
    }
}
