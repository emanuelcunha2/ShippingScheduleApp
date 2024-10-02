using ShippingScheduleMVVM.Commands;
using ShippingScheduleMVVM.Models;

using System.Linq;

using System.Windows;
using System.Windows.Input;

namespace ShippingScheduleMVVM.ViewModels.Modals
{
    public class InsertPriceViewModel : ViewModelBase
    {
        private WindowSettings? _windowSettings { get; set; }
        public WindowSettings? WindowSettings
        {
            get
            { return _windowSettings; }
            set
            {
                if (_windowSettings != value)
                {
                    _windowSettings = value;
                    OnPropertyChanged("WindowSettings");
                }
            }
        }
        private string? _selectedItem { get; set; }
        public string? SelectedItem
        {
            get
            { return _selectedItem; }
            set
            {
                if (_selectedItem != value)
                {
                    _selectedItem = value;
                    OnPropertyChanged("SelectedItem");
                }
            }
        }

        public bool Confirmed { get; set; } = false;
        public ICommand ConfirmChanges { get; }
        public ICommand CloseWindow { get; }
        public InsertPriceViewModel(ViewModelBase parentViewmodel)
        {
            string ownerWindow = "ThisRecordWindow";
            System.Windows.Window? owner = Application.Current.Windows.OfType<System.Windows.Window>().FirstOrDefault(w => w.DataContext == parentViewmodel);
           

            WindowSettings = new("ThisInsertPriceWindow");
            WindowSettings.Initialize(1.8, 155, 126, true, ownerWindow);
   
            if (owner != null)
            {
                // center the window to the current MainWindow
                WindowSettings.Left = owner.Left + (owner.ActualWidth - WindowSettings.Width) / 2;
                WindowSettings.Top = owner.Top + (owner.ActualHeight - WindowSettings.Height) / 2;
            }

            // Initialize Commands
            CloseWindow = new CloseWindowCommand(false);

            ConfirmChanges = new RelayCommand(() => {
                if (SelectedItem != null)
                {
                    System.Windows.Window? window = Application.Current.Windows.OfType<System.Windows.Window>().FirstOrDefault(w => w.DataContext == this);
                    if (window == null) { return; }
                    Confirmed = true;
                    window.Close();
                }
            });
        }
    }
}
