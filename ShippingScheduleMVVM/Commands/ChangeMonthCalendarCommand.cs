using ShippingScheduleMVVM.Models;
using ShippingScheduleMVVM.ViewModels;
using System;
using static ShippingScheduleMVVM.Models.Calendar;

namespace ShippingScheduleMVVM.Commands
{
    class ChangeMonthCalendarCommand : CommandBase
    {
        private readonly CalendarViewModel _calendarViewModel;
        private readonly CalendarAction _action;
        public ChangeMonthCalendarCommand(CalendarViewModel calendarViewModel, CalendarAction action)
        {
            _calendarViewModel = calendarViewModel;
            _action = action;
        }
        public override void Execute(object? parameter)
        {
            int month = _calendarViewModel.CurrentlySelectedDate.Month;
            int year = _calendarViewModel.CurrentlySelectedDate.Year;
            int number = 0;

            DateTime firstDayOfMonth = new(year, month, 1);

            if (_action == CalendarAction.AddMonth)
            {
                number = 1;
            }
            if (_action == CalendarAction.RemoveMonth)
            {
                number = -1;
            }

            _calendarViewModel.CurrentlySelectedDate = firstDayOfMonth.AddMonths(number);
            CalendarDay.SetDaysOfMonth(_calendarViewModel.DaysOfMonth, _calendarViewModel.CurrentlySelectedDate);
            CalendarDay.SelectDayByDate(_calendarViewModel.DaysOfMonth, _calendarViewModel.CurrentlySelectedDate);
        }
    }
}
