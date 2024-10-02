using Microsoft.IdentityModel.Tokens;
using ShippingScheduleMVVM.Commands;
using ShippingScheduleMVVM.Models;

using System.Linq;

using System.Windows;
using System.Windows.Input;

namespace ShippingScheduleMVVM.ViewModels.Modals
{
    public class CancelRecordViewModel : ViewModelBase
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

        private string? _reasonForCancel { get; set; }
        public string? ReasonForCancel
        {
            get
            { return _reasonForCancel; }
            set
            {
                if (_reasonForCancel != value)
                {
                    _reasonForCancel = value;
                    OnPropertyChanged("ReasonForCancel");
                }
            }
        }
        public ICommand CloseWindow { get; }
        public ICommand ConfirmChanges { get; } 
        public CancelRecordViewModel(ViewModelBase originViewmodel) 
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
                System.Windows.Window? owner = Application.Current.Windows.OfType<System.Windows.Window>().FirstOrDefault(w => w.DataContext == originViewmodel);

                WindowSettings = new("ThisCancelRecordWindow");
                WindowSettings.Initialize(1.8, 200, 180, false, ownerTitle);

                if (owner != null)
                {
                    // Center the window acording to the windows owner position
                    WindowSettings.Left = owner.Left + (owner.ActualWidth - WindowSettings.Width) / 2;
                    WindowSettings.Top = owner.Top + (owner.ActualHeight - WindowSettings.Height) / 2;
                }

                ConfirmChanges = new RelayCommand(() => {
                    if (ReasonForCancel.IsNullOrEmpty()) { return; }

                    string reason = ". Razão de cancelamento: ";
                    if (string.IsNullOrEmpty(viewmodel.SelectedRecord.Comment)) 
                    { 
                        reason = "Razão de cancelamento: "; 
                    }
                    reason += ReasonForCancel;
                    viewmodel.SelectedRecord.Comment += reason;
                    viewmodel.IsCancelled = true;
                    viewmodel.SelectedRecord.IsCancelled = true;
                    viewmodel.VisualCancelVisibility = Visibility.Visible;

                    System.Windows.Window? window = Application.Current.Windows.OfType<System.Windows.Window>().FirstOrDefault(w => w.DataContext == this);
                    if (window == null) { return; }
                    window.Close();
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
