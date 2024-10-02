using ShippingScheduleMVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ShippingScheduleMVVM.Models
{
    public class ShipTo : ViewModelBase
    {
        public int Id { get; set; } = -1;
        public int UnloadingPointId { get; set;} = -1;
        public string ShipToNumber { get; set; } = string.Empty;
        public string ShipToName { get; set; } = string.Empty;
        public string UnloadingPoint { get; set; } = string.Empty;
        private SolidColorBrush _background { get; set; } = new SolidColorBrush(Colors.Transparent);
        public SolidColorBrush Background
        {
            get { return _background; }
            set
            {
                if (_background != value)
                {
                    _background = value;
                    OnPropertyChanged("Background");
                }
            }
        }
    }
}
