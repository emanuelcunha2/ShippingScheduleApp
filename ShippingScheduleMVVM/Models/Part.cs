using Microsoft.IdentityModel.Tokens;
using ShippingScheduleMVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace ShippingScheduleMVVM.Models
{
    public class Part : ViewModelBase
    {
        public int Id { get; set; } = -1;
        public int ListIndex { get; set; } = -69;
        public int RecordId { get; set; } = -1;
        private string _shipTo = string.Empty;
        public string ShipTo
        {
            get { return _shipTo; }
            set
            {
                if (_shipTo != value)
                {
                    _shipTo = value;
                    OnPropertyChanged(nameof(ShipTo));
                }
            }
        }

        private string _shipToName = string.Empty;
        public string ShipToName
        {
            get { return _shipToName; }
            set
            {
                if (_shipToName != value)
                {
                    _shipToName = value;
                    OnPropertyChanged(nameof(ShipToName));
                }
            }
        }

        private string _apn = string.Empty;
        public string APN
        {
            get { return _apn; }
            set
            {
                if (_apn != value)
                {
                    _apn = value;
                    OnPropertyChanged(nameof(APN));
                }
            }
        }
        private string _cpn = string.Empty;
        public string CPN
        {
            get { return _cpn; }
            set
            {
                if (_cpn != value)
                {
                    _cpn = value;
                    OnPropertyChanged(nameof(CPN));
                }
            }
        }

        private bool _selected = false;
        public bool Selected
        {
            get { return _selected; }
            set
            {
                if (_selected != value)
                {
                    if (value == true) { Color = new SolidColorBrush(System.Windows.Media.Color.FromRgb(232, 244, 255)); }
                    else { Color = new SolidColorBrush(Colors.White);  }
                    _selected = value;
                    OnPropertyChanged(nameof(Selected));
                }
            }
        }
        private SolidColorBrush _color = new SolidColorBrush(Colors.White);
        public SolidColorBrush Color
        {
            get { return _color; }
            set
            {
                if (_color != value)
                {
                    _color = value;
                    OnPropertyChanged(nameof(Color));
                }
            }
        }
        private string _designation = string.Empty;
        public string Designation
        {
            get { return _designation; }
            set
            {
                if (_designation != value)
                {
                    _designation = value;
                    OnPropertyChanged(nameof(Designation));
                }
            }
        }
        private string _unloadingPoint = string.Empty;
        public string UnloadingPoint
        {
            get { return _unloadingPoint; }
            set
            {
                if (_unloadingPoint != value)
                {
                    _unloadingPoint = value;
                    OnPropertyChanged(nameof(UnloadingPoint));
                }
            }
        }
        private int _sapQuantity = 0;
        public int SapQuantity
        {
            get { return _sapQuantity; }
            set
            {
                if (_sapQuantity != value)
                {
                    _sapQuantity = value;
                    OnPropertyChanged(nameof(SapQuantity));
                }
            }
        }


        private int _expectedQuantity = 0;
        public int ExpectedQuantity
        {
            get { return _expectedQuantity; }
            set
            {
                if (_expectedQuantity != value)
                {
                    _expectedQuantity = value;
                    OnPropertyChanged(nameof(ExpectedQuantity));
                }
            }
        }
        private int _finalQuantity = 0;
        public int FinalQuantity
        {
            get { return _finalQuantity; }
            set
            {
                if (_finalQuantity != value)
                {
                    _finalQuantity = value;
                    OnPropertyChanged(nameof(FinalQuantity));
                }
            }
        }
        private string _deliveryNote = string.Empty;
        public string DeliveryNote
        {
            get { return _deliveryNote; }
            set
            {
                if (_deliveryNote != value)
                {
                    _deliveryNote = value;
                    OnPropertyChanged(nameof(DeliveryNote));
                }
            }
        }
        private string _transportNumber = string.Empty;
        public string TransportNumber
        {
            get { return _transportNumber; }
            set
            {
                if (_transportNumber != value)
                {
                    _transportNumber = value;
                    OnPropertyChanged(nameof(TransportNumber));
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
        private Visibility _commentFlagVisibility = Visibility.Collapsed;
        public Visibility CommentFlagVisibility
        {
            get { return _commentFlagVisibility; }
            set
            {
                if (_commentFlagVisibility != value)
                {
                    _commentFlagVisibility = value;
                    OnPropertyChanged(nameof(CommentFlagVisibility));
                }
            }
        }
        private bool _shipToEnabled = true;
        public bool ShipToEnabled
        {
            get { return _shipToEnabled; }
            set
            {
                _shipToEnabled = value;
                OnPropertyChanged(nameof(ShipToEnabled));
            }
        }

        private bool _apnEnabled = true;
        public bool ApnEnabled
        {
            get { return _apnEnabled; }
            set
            {
                _apnEnabled = value;
                OnPropertyChanged(nameof(ApnEnabled));
            }
        }

        private bool _cpnEnabled = true;
        public bool CpnEnabled
        {
            get { return _cpnEnabled; }
            set
            {
                _cpnEnabled = value;
                OnPropertyChanged(nameof(CpnEnabled));
            }
        }

        private bool _unloadingPointEnabled = true;
        public bool UnloadingPointEnabled
        {
            get { return _unloadingPointEnabled; }
            set
            {
                _unloadingPointEnabled = value;
                OnPropertyChanged(nameof(UnloadingPointEnabled));
            }
        }

        private bool _expectedQuantityEnabled = true;
        public bool ExpectedQuantityEnabled
        {
            get { return _expectedQuantityEnabled; }
            set
            {
                _expectedQuantityEnabled = value;
                OnPropertyChanged(nameof(ExpectedQuantityEnabled));
            }
        }

        private bool _finalQuantityEnabled = true;
        public bool FinalQuantityEnabled
        {
            get { return _finalQuantityEnabled; }
            set
            {
                _finalQuantityEnabled = value;
                OnPropertyChanged(nameof(FinalQuantityEnabled));
            }
        }

        private bool _deliveryNoteEnabled = true;
        public bool DeliveryNoteEnabled
        {
            get { return _deliveryNoteEnabled; }
            set
            {
                _deliveryNoteEnabled = value;
                OnPropertyChanged(nameof(DeliveryNoteEnabled));
            }
        }

        private bool _transportNumberEnabled = true;
        public bool TransportNumberEnabled
        {
            get { return _transportNumberEnabled; }
            set
            {
                _transportNumberEnabled = value;
                OnPropertyChanged(nameof(TransportNumberEnabled));
            }
        }

        private bool _designationEnabled = true;
        public bool DesignationEnabled
        {
            get { return _designationEnabled; }
            set
            {
                _designationEnabled = value;
                OnPropertyChanged(nameof(DesignationEnabled));
            }
        }
        /////////////////////////////////////////////////////////
        // In case of DHL/TNT                                  //
        /////////////////////////////////////////////////////////
        public Dictionary<string, Action<Part>> ColorSelector = new Dictionary<string, Action<Part>>
        {
            { "C", part => part.ColorCategory = new SolidColorBrush(System.Windows.Media.Color.FromRgb(235, 235, 235)) },
            { "R", part => part.ColorCategory = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 242, 199)) },
            { "H", part => part.ColorCategory = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 217, 171)) },
            { "P", part => part.ColorCategory = new SolidColorBrush(System.Windows.Media.Color.FromRgb(212, 228, 255)) },
            { "S", part => part.ColorCategory = new SolidColorBrush(System.Windows.Media.Color.FromRgb(194, 255, 210)) },
        };
        private string _selectedCategory = string.Empty;
        public string SelectedCategory 
        {
            get { return _selectedCategory; }
            set
            {
                _selectedCategory = value;
                var changeColor = ColorSelector[value];
                changeColor(this);

                OnPropertyChanged(nameof(SelectedCategory));
            }
        }

        private Project _selectedProject = new();
        public Project SelectedProject
        {
            get { return _selectedProject; }
            set
            {
                _selectedProject = value;
                OnPropertyChanged(nameof(SelectedProject));
            }
        }

        public int SelectedProjectInternalId{ get; set; } = -1;
        private string _price { get; set; } = "0";
        public string Price
        {
            get { return _price; }
            set
            {
                _price = value;
                OnPropertyChanged(nameof(Price));
            }
        }

        private string _accountNumber { get; set; } = string.Empty;
        public string AccountNumber
        {
            get { return _accountNumber; }
            set
            {
                _accountNumber = value;
                OnPropertyChanged(nameof(AccountNumber));
            }
        }
        private string _pO { get; set; } = string.Empty;
        public string PO
        {
            get { return _pO; }
            set
            {
                _pO = value;
                OnPropertyChanged(nameof(PO));
            }
        }
        private string _trackNumber { get; set; } = string.Empty;
        public string TrackNumber
        {
            get { return _trackNumber; }
            set
            {
                _trackNumber = value;
                OnPropertyChanged(nameof(TrackNumber));
            }
        }
        private Visibility _partVisibility = Visibility.Visible;
        public Visibility PartVisibility
        {
            get { return _partVisibility; }
            set
            {
                if(_partVisibility != value )
                {
                    _partVisibility = value;
                    OnPropertyChanged("PartVisibility");
                } 
            }
        }
        private SolidColorBrush _colorCategory = new SolidColorBrush(Colors.White);
        public SolidColorBrush ColorCategory
        {
            get { return _colorCategory; }
            set
            {
                if (_colorCategory != value)
                {
                    _colorCategory = value;
                    OnPropertyChanged(nameof(ColorCategory));
                }
            }
        }
        private bool _isAccountNumberComboboxOpen { get; set; } = false;
        public bool IsAccountNumberComboboxOpen
        {
            get { return _isAccountNumberComboboxOpen; }
            set
            {
                _isAccountNumberComboboxOpen = value;
                OnPropertyChanged("IsAccountNumberComboboxOpen");
            }
        }
        /////////////////////////////////////////////////////////
        // In case of DHL/TNT - END                            //
        /////////////////////////////////////////////////////////

        public static string ReturnShipToList(ObservableCollection<Part> Parts)
        {
            HashSet<string> uniqueShipTos = new HashSet<string>();
            foreach (Part part in Parts)
            {
                if (!part.ShipToName.IsNullOrEmpty())
                {
                    uniqueShipTos.Add(part.ShipToName);
                }
            }

            return string.Join(", ", uniqueShipTos);
        }

        public static string ReturnUniqueShipTosInParts(ObservableCollection<Part> parts)
        {
            HashSet<string> uniqueShipTos = new HashSet<string>();
            foreach (Part part in parts)
            {
                if (!part.ShipToName.IsNullOrEmpty())
                {
                    uniqueShipTos.Add(part.ShipToName);
                }
            }
            return string.Join(", ", uniqueShipTos);
        }
    }
}
