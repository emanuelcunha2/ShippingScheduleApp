using Microsoft.IdentityModel.Tokens;
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
    public class ExcelPart : ViewModelBase
    {
        private string _item1 = string.Empty;
        public string Item1
        {
            get { return _item1; }
            set
            {
                if (_item1 != value)
                {
                    _item1 = value;
                    OnPropertyChanged(nameof(Item1));
                }
            }
        }
        private string _item2 = string.Empty;
        public string Item2
        {
            get { return _item2; }
            set
            {
                if (_item2 != value)
                {
                    _item2 = value;
                    OnPropertyChanged(nameof(Item2));
                }
            }
        }
        private string _item3 = string.Empty;
        public string Item3
        {
            get { return _item3; }
            set
            {
                if (_item3 != value)
                {
                    _item3 = value;
                    OnPropertyChanged(nameof(Item3));
                }
            }
        }
        private string _item4 = string.Empty;
        public string Item4
        {
            get { return _item4; }
            set
            {
                if (_item4 != value)
                {
                    _item4 = value;
                    OnPropertyChanged(nameof(Item4));
                }
            }
        }
        private string _item5 = string.Empty;
        public string Item5
        {
            get { return _item5; }
            set
            {
                if (_item5 != value)
                {
                    _item5 = value;
                    OnPropertyChanged(nameof(Item5));
                }
            }
        }
        private string _item6 = string.Empty;
        public string Item6
        {
            get { return _item6; }
            set
            {
                if (_item6 != value)
                {
                    _item6 = value;
                    OnPropertyChanged(nameof(Item6));
                }
            }
        }
        private string _item7 = string.Empty;
        public string Item7
        {
            get { return _item7; }
            set
            {
                if (_item7 != value)
                {
                    _item7 = value;
                    OnPropertyChanged(nameof(Item7));
                }
            }
        }
        private string _item8 = string.Empty;
        public string Item8
        {
            get { return _item8; }
            set
            {
                if (_item8 != value)
                {
                    _item8 = value;
                    OnPropertyChanged(nameof(Item8));
                }
            }
        }
        private string _item9 = string.Empty;
        public string Item9
        {
            get { return _item9; }
            set
            {
                if (_item9 != value)
                {
                    _item9 = value;
                    OnPropertyChanged(nameof(Item9));
                }
            }
        }
        private string _item10 = string.Empty;
        public string Item10
        {
            get { return _item10; }
            set
            {
                if (_item10 != value)
                {
                    _item10 = value;
                    OnPropertyChanged(nameof(Item10));
                }
            }
        }
        private string _item11 = string.Empty;
        public string Item11
        {
            get { return _item11; }
            set
            {
                if (_item11 != value)
                {
                    _item11 = value;
                    OnPropertyChanged(nameof(Item11));
                }
            }
        }

        private SolidColorBrush _partBackground = Brushes.White;
        public SolidColorBrush PartBackground
        {
            get { return _partBackground; }
            set
            {
                if (_partBackground != value)
                {
                    _partBackground = value;
                    OnPropertyChanged("PartBackground");
                }
            }
        }
        private Visibility _itemVisibility1 = Visibility.Visible;
        public Visibility ItemVisibility1
        {
            get { return _itemVisibility1; }
            set
            {
                if (_itemVisibility1 != value)
                {
                    _itemVisibility1 = value;
                    OnPropertyChanged(nameof(ItemVisibility1));
                }
            }
        }
        private Visibility _itemVisibility2 = Visibility.Visible;
        public Visibility ItemVisibility2
        {
            get { return _itemVisibility2; }
            set
            {
                if (_itemVisibility2 != value)
                {
                    _itemVisibility2 = value;
                    OnPropertyChanged(nameof(ItemVisibility2));
                }
            }
        }
        private Visibility _itemVisibility3 = Visibility.Visible;
        public Visibility ItemVisibility3
        {
            get { return _itemVisibility3; }
            set
            {
                if (_itemVisibility3 != value)
                {
                    _itemVisibility3 = value;
                    OnPropertyChanged(nameof(ItemVisibility3));
                }
            }
        }
        private Visibility _itemVisibility4 = Visibility.Visible;
        public Visibility ItemVisibility4
        {
            get { return _itemVisibility4; }
            set
            {
                if (_itemVisibility4 != value)
                {
                    _itemVisibility4 = value;
                    OnPropertyChanged(nameof(ItemVisibility4));
                }
            }
        }
        private Visibility _itemVisibility5 = Visibility.Visible;
        public Visibility ItemVisibility5
        {
            get { return _itemVisibility5; }
            set
            {
                if (_itemVisibility5 != value)
                {
                    _itemVisibility5 = value;
                    OnPropertyChanged(nameof(ItemVisibility5));
                }
            }
        }
        private Visibility _itemVisibility6 = Visibility.Visible;
        public Visibility ItemVisibility6
        {
            get { return _itemVisibility6; }
            set
            {
                if (_itemVisibility6 != value)
                {
                    _itemVisibility6 = value;
                    OnPropertyChanged(nameof(ItemVisibility6));
                }
            }
        }
        private Visibility _itemVisibility7 = Visibility.Visible;
        public Visibility ItemVisibility7
        {
            get { return _itemVisibility7; }
            set
            {
                if (_itemVisibility7 != value)
                {
                    _itemVisibility7 = value;
                    OnPropertyChanged(nameof(ItemVisibility7));
                }
            }
        }
        private Visibility _itemVisibility8 = Visibility.Visible;
        public Visibility ItemVisibility8
        {
            get { return _itemVisibility8; }
            set
            {
                if (_itemVisibility8 != value)
                {
                    _itemVisibility8 = value;
                    OnPropertyChanged(nameof(ItemVisibility8));
                }
            }
        }
        private Visibility _itemVisibility9 = Visibility.Visible;
        public Visibility ItemVisibility9
        {
            get { return _itemVisibility9; }
            set
            {
                if (_itemVisibility9 != value)
                {
                    _itemVisibility9 = value;
                    OnPropertyChanged(nameof(ItemVisibility9));
                }
            }
        }
        private Visibility _itemVisibility10 = Visibility.Visible;
        public Visibility ItemVisibility10
        {
            get { return _itemVisibility10; }
            set
            {
                if (_itemVisibility10 != value)
                {
                    _itemVisibility10 = value;
                    OnPropertyChanged(nameof(ItemVisibility10));
                }
            }
        }

        private Visibility _itemVisibility11 = Visibility.Visible;
        public Visibility ItemVisibility11
        {
            get { return _itemVisibility11; }
            set
            {
                if (_itemVisibility11 != value)
                {
                    _itemVisibility11 = value;
                    OnPropertyChanged(nameof(ItemVisibility11));
                }
            }
        }

    }
}
