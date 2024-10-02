using Microsoft.IdentityModel.Tokens;
using ShippingScheduleMVVM.Commands;
using ShippingScheduleMVVM.Models;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace ShippingScheduleMVVM.ViewModels.Modals
{
    class PartCommentViewModel : ViewModelBase
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

        private string? _currentComment { get; set; }
        public string? CurrentComment
        {
            get
            { return _currentComment; }
            set
            {
                if (_currentComment != value)
                {
                    _currentComment = value;
                    OnPropertyChanged("CurrentComment");
                }
            }
        }

        public ICommand ConfirmChanges { get; }

        public PartCommentViewModel(Part part, double top, double left, ViewModelBase parentViewmodel) 
        {
            string ownerWindow = "ThisRecordWindow";
            System.Windows.Window? owner = System.Windows.Application.Current.Windows.OfType<System.Windows.Window>().FirstOrDefault(w => w.DataContext == parentViewmodel);
        
            if (!part.Comment.IsNullOrEmpty()) { CurrentComment = part.Comment; }

            WindowSettings = new("ThisPartCommentWindow");
            WindowSettings.Initialize(1.8, 155, 108, true, ownerWindow);

            WindowSettings.Top = top;
            WindowSettings.Left = left - WindowSettings.Width + (WindowSettings.Width * 0.05);

            ConfirmChanges = new RelayCommand(() => {
                if (!CurrentComment.IsNullOrEmpty() && CurrentComment != null)
                {
                    part.Comment = CurrentComment;
                    part.CommentFlagVisibility = Visibility.Visible;
                }
                else
                {
                    part.Comment = "";
                    part.CommentFlagVisibility = Visibility.Collapsed;
                }
                System.Windows.Window? window = Application.Current.Windows.OfType<System.Windows.Window>().FirstOrDefault(w => w.DataContext == this);
                if (window == null) { return; }
                window.Close();
            });
        }
    }
}
