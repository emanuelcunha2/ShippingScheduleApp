using Microsoft.IdentityModel.Tokens;
using ShippingScheduleMVVM.Helpers;
using ShippingScheduleMVVM.Models;
using System;
using System.Linq;
using System.Windows.Forms;

namespace ShippingScheduleMVVM.ViewModels.Modals
{
    public class CategoryHoverViewModel : ViewModelBase
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

        private string? _user { get; set; }
        public string? User
        {
            get
            { return _user; }
            set
            {
                if (_user != value)
                {
                    _user = value;
                    OnPropertyChanged("User");
                }
            }
        }

        private string? _date { get; set; }
        public string? Date
        {
            get
            { return _date; }
            set
            {
                if (_date != value)
                {
                    _date = value;
                    OnPropertyChanged("Date");
                }
            }
        }
        
        private string? _category { get; set; }
        public string? Category
        {
            get
            { return _category; }
            set
            {
                if (_category != value)
                {
                    _category = value;
                    OnPropertyChanged("Category");
                }
            }
        } 
        public CategoryHoverViewModel(double top, double left, Record record, string category, ViewModelBase parentViewModel)
        {
            string ownerWindow = "ThisRecordWindow";
            System.Windows.Window? owner = System.Windows.Application.Current.Windows.OfType<System.Windows.Window>().FirstOrDefault(w => w.DataContext == parentViewModel);
           
            WindowSettings = new("ThisCategoryHoverWindow");
            WindowSettings.Initialize(1.7, 120, 75, true, ownerWindow);

            Screen? screen = WindowScreen.FindScreen(ownerWindow);
            if (screen == null) { return; }
            Tuple<double, double> dpi = WindowScreen.GetDPI(ownerWindow);

            double dpiX = dpi.Item1;
            double dpiY = dpi.Item2;

            WindowSettings.Top = top / App.InitialDPI;
            WindowSettings.Left = left / App.InitialDPI;
            WindowSettings.Left -= (WindowSettings.Width * 1);
            
            switch (category)
            {
                case "Created":
                    Category = "Created";
                    Date = record.CreationDate.ToString();
                    User = record.CreatedBy;
                    break;
                case "Released":
                    Category = "Released";
                    Date = record.ReleasedDate.ToString();
                    User = record.ReleasedBy;
                    break;
                case "Process":
                    Category = "In Process";
                    Date = record.ProcessDate.ToString();
                    User = record.ProcessBy;
                    break;
                case "Prepared":
                    Category = "Prepared";
                    Date = record.PreparedDate.ToString();
                    User = record.PreparedBy;
                    break;
                case "Shipped":
                    Category = "Shipped";
                    Date = record.ShippedDate.ToString();
                    User = record.ShippedBy;
                    break;
                default:
                    Category = "Unknown";
                    Date = "";
                    User = "";
                    break;
            }

            if(Category == "Unknown" || Date.IsNullOrEmpty() || User.IsNullOrEmpty())
            {
                Date = "Sem data";
                User = "Sem user";
            }
        }
    }
}
