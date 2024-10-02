using ShippingScheduleMVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace ShippingScheduleMVVM.Models
{
    public class Path : ViewModelBase
    {
        private int _rowSpan = 1;
        public int RowSpan
        {
            get => _rowSpan;
            set 
            {
                _rowSpan = value;
                OnPropertyChanged("RowSpan");
            }
        }
        private int _colSpan = 1;
        public int ColSpan
        {
            get => _colSpan;
            set
            {
                _colSpan = value;
                OnPropertyChanged("ColSpan");
            }
        }
        private HorizontalAlignment _horizontalAlignment = HorizontalAlignment.Left;
        public HorizontalAlignment HorizontalAlignment
        {
            get => _horizontalAlignment;
            set
            {
                _horizontalAlignment = value;
                OnPropertyChanged("HorizontalAlignment");
            }
        }

        private VerticalAlignment _verticalAlignment = VerticalAlignment.Top;
        public VerticalAlignment VerticalAlignment
        {
            get => _verticalAlignment;
            set
            {
                _verticalAlignment = value;
                OnPropertyChanged("VerticalAlignment");
            }
        }

        private Geometry _data = Geometry.Parse("");
        public Geometry Data
        {
            get => _data;
            set
            {
                _data = value;
                OnPropertyChanged("Data");
            }
        }
        public Path() 
        {

        }
    }
}
