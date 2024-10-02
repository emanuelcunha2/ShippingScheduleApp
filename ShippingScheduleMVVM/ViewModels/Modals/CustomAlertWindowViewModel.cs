using ShippingScheduleMVVM.Commands;
using ShippingScheduleMVVM.Models;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace ShippingScheduleMVVM.ViewModels.Modals
{
    public class CustomAlertWindowViewModel : ViewModelBase
    {
        private WindowSettings _windowSettings { get; set; } = new("ThisAlertWindow");
        public WindowSettings WindowSettings
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

        private string? _content { get; set; }
        public string? Content
        {
            get
            { return _content; }
            set
            {
                if (_content != value)
                {
                    _content = value;
                    OnPropertyChanged("Content");
                }
            }
        }
        private string? _title { get; set; }
        public string? Title
        {
            get
            { return _title; }
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged("Title");
                }
            }
        }
        public bool WasConfirmed = false;
        public ICommand CloseWindow { get; }
        public ICommand ConfirmChanges { get; }
        public CustomAlertWindowViewModel(string title, string content, ViewModelBase parentViewModel)
        { 
            System.Windows.Window? owner = Application.Current.Windows.OfType<System.Windows.Window>().FirstOrDefault(w => w.DataContext == parentViewModel);
             
            if (owner != null)
            {
                WindowSettings.Initialize(1.8, 210, 157, false, owner.Title);
                WindowSettings.Left = owner.Left + (owner.ActualWidth - WindowSettings.Width) / 2;
                WindowSettings.Top = owner.Top + (owner.ActualHeight - WindowSettings.Height) / 2;
            }
            // Initialize Commands
            CloseWindow = new CloseWindowCommand(false);

            ConfirmChanges = new RelayCommand(() => {
                WasConfirmed = true;
                System.Windows.Window? window = Application.Current.Windows.OfType<System.Windows.Window>().FirstOrDefault(w => w.DataContext == this);
                if (window == null) { return; }
                window.Close();
            });

            Title = title;
            Content = content;
        }
    }
}
