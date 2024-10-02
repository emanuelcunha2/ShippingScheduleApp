using ShippingScheduleMVVM.Models;
using ShippingScheduleMVVM.ViewModels;
using ShippingScheduleMVVM.Views;
using System.Linq;
using System.Windows;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ShippingScheduleMVVM.Commands
{
    public class OpenRecordDataWindow : CommandBase
    {
        private readonly ScheduleManagementViewModel _scheduleManagementViewModel;
        public OpenRecordDataWindow(ScheduleManagementViewModel scheduleManagementViewModel)
        { 
            _scheduleManagementViewModel = scheduleManagementViewModel;
        }
        public override void Execute(object? parameter)
        {
            // Update
            if (parameter is ScheduleRecord record)
            {
                System.Windows.Window? owner = Application.Current.Windows.OfType<System.Windows.Window>().FirstOrDefault(w => w.Title == "ThisMainWindow");
                if (owner == null) { return; } 

                MainWindowViewModel vm = (MainWindowViewModel)owner.DataContext;

                if(record.TransportMode == "TNT" || record.TransportMode == "DHL")
                {
                    /*
                    Record2Window window;
                    window = new Record2Window() { DataContext = new Record2WindowViewModel(record.Id, vm, _scheduleManagementViewModel, "Updating", "",record.TransportMode) };
                    window.Owner = owner;
                    window.Show();
                    */

                    RecordWindow window;
                    window = new RecordWindow() { DataContext = new RecordWindowViewModel(record.Id, vm, _scheduleManagementViewModel, "Updating", "") };
                    window.Show();
                }
                else
                {
                    RecordWindow window;
                    window = new RecordWindow() { DataContext = new RecordWindowViewModel(record.Id, vm, _scheduleManagementViewModel, "Updating", "") };
                    window.Show();
                }
                
                _scheduleManagementViewModel.IsRecordOpen = false;
                vm.VisibilityOverlay = Visibility.Visible;
            }

            // Create
            if (parameter is string newRecord)
            {
                System.Windows.Window? owner = Application.Current.Windows.OfType<System.Windows.Window>().FirstOrDefault(w => w.Title == "ThisMainWindow");
                if (owner == null) { return; }

                MainWindowViewModel vm = (MainWindowViewModel)owner.DataContext;

                var window = new RecordWindow() { DataContext = new RecordWindowViewModel(-1, vm, _scheduleManagementViewModel, "Creating", newRecord) };
                window.Owner = owner;

                _scheduleManagementViewModel.IsRecordOpen = false;
                vm.VisibilityOverlay = Visibility.Visible;
                window.Show();
            }
        }
    }
}
