using ShippingScheduleMVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ShippingScheduleMVVM.Models
{
    public class Project : ViewModelBase
    {
        public string Name { get; set; } = string.Empty;
        public string Person { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int Id { get; set; } = -1;
        public int InternalId { get; set; } = -1;
        public int UserId { get; set; } = -1;

        private DateTime? _deliveryDate;
        public DateTime? DeliveryDate
        {
            get { return _deliveryDate; }
            set
            {
                if (_deliveryDate != value)
                {
                    _deliveryDate = value;
                    OnPropertyChanged(nameof(DeliveryDate));
                }
            }
        }
        private string _pta = string.Empty;
        public string PTA
        {
            get { return _pta; }
            set
            {
                if (_pta != value)
                {
                    _pta = value;
                    OnPropertyChanged(nameof(PTA));
                }
            }
        }
        private string _comment = string.Empty;
        public string Comment
        {
            get { return _comment; }
            set
            {
                if (_comment != value)
                {
                    _comment = value;
                    OnPropertyChanged(nameof(Comment));
                }
            }
        }
        public int UniqueHSVColorHue { get; set; } = 0;

        private SolidColorBrush _color = new SolidColorBrush(Color.FromRgb(0, 62, 110));
        public SolidColorBrush BackgroundColor
        {
            get { return _color; }
            set
            {
                if (_color != value)
                {
                    _color = value;
                    OnPropertyChanged(nameof(BackgroundColor));
                }
            }
        }

        private SolidColorBrush _colorForeground = new SolidColorBrush(Colors.White);
        public SolidColorBrush ColorForeground
        {
            get { return _colorForeground; }
            set
            {
                if (_colorForeground != value)
                {
                    _colorForeground = value;
                    OnPropertyChanged(nameof(ColorForeground));
                }
            }
        }

        private SolidColorBrush _colorBorder = new SolidColorBrush(Colors.White);
        public SolidColorBrush ColorBorder
        {
            get { return _colorBorder; }
            set
            {
                if (_colorBorder != value)
                {
                    _colorBorder = value;
                    OnPropertyChanged(nameof(ColorBorder));
                }
            }
        }
    }
}
