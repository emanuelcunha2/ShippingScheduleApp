using ShippingScheduleMVVM.Commands;
using ShippingScheduleMVVM.Models;

using System.Linq;

using System.Windows.Input;
using System.Windows;
using System.Collections.ObjectModel;

namespace ShippingScheduleMVVM.ViewModels.Modals
{

    public class AddProjectViewModel : ViewModelBase
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
        public ObservableCollection<string> TypeItems { get; set; } = new ObservableCollection<string>() {
            "Discovery",
            "Economy",
            "Express"
        };
        private string _name { get; set; } = string.Empty;
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged("Name");
                }
            }
        }
        private string _selectedType { get; set; } = string.Empty;
        public string SelectedType
        {
            get { return _selectedType; }
            set
            {
                if(_selectedType != value)
                {
                    _selectedType = value;
                    OnPropertyChanged("SelectedType");
                }
            }
        }
        
        public ICommand ConfirmChanges { get; }
        public ICommand CloseWindow { get; }
 
        public AddProjectViewModel(ViewModelBase originViewmodel)
        {
            dynamic? viewmodel = null;
            string ownerTitle = "";
            var vm1 = originViewmodel as RecordWindowViewModel;
            // var vm2 = originViewmodel as Record2WindowViewModel;

            if (vm1 != null) { viewmodel = vm1; ownerTitle = "ThisRecordWindow"; }
            // if (vm2 != null) { viewmodel = vm2; ownerTitle = "ThisRecord2Window"; }

            // Initialize Commands
            CloseWindow = new CloseWindowCommand(false);

            if (viewmodel != null)
            {
                System.Windows.Window? owner = Application.Current.Windows.OfType<System.Windows.Window>().FirstOrDefault(w => w.Title == ownerTitle);

                WindowSettings = new("ThisAddProjectWindow");
                WindowSettings.Initialize(1.8, 185, 160, false, ownerTitle);

                if (owner != null)
                {
                    // Center the window acording to the windows owner position
                    WindowSettings.Left = owner.Left + (owner.ActualWidth - WindowSettings.Width) / 2;
                    WindowSettings.Top = owner.Top + (owner.ActualHeight - WindowSettings.Height) / 2;
                }

                ConfirmChanges = new RelayCommand(() => {
                    
                });
            }
            else
            {
                ConfirmChanges = new RelayCommand(() =>
                {
                });
            }
        }
    }
}
