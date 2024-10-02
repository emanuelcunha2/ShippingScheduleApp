using ShippingScheduleMVVM.Models;
using ShippingScheduleMVVM.ViewModels;

namespace ShippingScheduleMVVM.Commands
{
    class SelectCalendarDayCommand : CommandBase
    {
        private readonly CalendarViewModel _calendarViewModel;
        public SelectCalendarDayCommand(CalendarViewModel calendarViewModel)
        {
            _calendarViewModel = calendarViewModel;
        }
        public override void Execute(object? parameter)
        {
            if (parameter is CalendarDay calendarDay)
            {
                _calendarViewModel.CurrentlySelectedDate = calendarDay.SelectDay();
                CalendarDay.SetDaysOfMonth(_calendarViewModel.DaysOfMonth, _calendarViewModel.CurrentlySelectedDate);
                CalendarDay.SelectDayByDate(_calendarViewModel.DaysOfMonth, _calendarViewModel.CurrentlySelectedDate);
            }
        }
    }
}
