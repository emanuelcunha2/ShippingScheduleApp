using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Finance.FinancialDayCount;
using ShippingScheduleMVVM.Services;
using ShippingScheduleMVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ShippingScheduleMVVM.Models
{
    public class Record : ViewModelBase
    {
        private int _id = -1;
        public int Id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }
        private string _category = string.Empty;
        public string Category
        {
            get { return _category; }
            set
            {
                if (_category != value)
                {
                    _category = value;
                    OnPropertyChanged(nameof(Category));
                }
            }
        }
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
        private bool _isCancelled = false;
        
        public bool IsCancelled
        {
            get { return _isCancelled; }
            set
            {
                if (_isCancelled != value)
                {
                    _isCancelled = value;
                    OnPropertyChanged(nameof(IsCancelled));
                }
            }
        }
        private string _carrier = string.Empty;
        public string Carrier
        {
            get { return _carrier; }
            set
            {
                if (_carrier != value)
                {
                    _carrier = value;
                    OnPropertyChanged(nameof(Carrier));
                }
            }
        }
        private string _plate = string.Empty;
        public string Plate
        {
            get { return _plate; }
            set
            {
                if (_plate != value)
                {
                    _plate = value;
                    OnPropertyChanged(nameof(Plate));
                }
            }
        }
        private DateTime _leavingTime;
        public DateTime LeavingTime
        {
            get { return _leavingTime; }
            set
            {
                if (_leavingTime != value)
                {
                    _leavingTime = value;
                    OnPropertyChanged(nameof(LeavingTime));
                }
            }
        }
        private string _time = string.Empty;
        public string Time
        {
            get { return _time; }
            set
            {
                if (_time != value)
                {
                    _time = value;
                    OnPropertyChanged(nameof(Time));
                }
            }
        }
        private string _day = string.Empty;
        public string Day
        {
            get { return _day; }
            set
            {
                if (_day != value)
                {
                    _day = value;
                    OnPropertyChanged(nameof(Day));
                }
            }
        }
        private string _shipmentType = string.Empty;
        public string ShipmentType
        {
            get { return _shipmentType; }
            set
            {
                if (_shipmentType != value)
                {
                    DatabaseOperations database = new();
                    TransportModes.Clear();
                    TransportModes = database.SelectTransportModes(value);

                    _shipmentType = value;
                    OnPropertyChanged(nameof(ShipmentType));
                }
            }
        }
        private string _transportMode = string.Empty;
        public string TransportMode
        {
            get { return _transportMode; }
            set
            {
                if (_transportMode != value)
                {
                    _transportMode = value;
                    OnPropertyChanged(nameof(TransportMode));
                }
            }
        }
        private string _bid = string.Empty;
        public string BID
        {
            get { return _bid; }
            set
            {
                if (_bid != value)
                {
                    _bid = value;
                    OnPropertyChanged(nameof(BID));
                }
            }
        }
        private string _pta = string.Empty;
        public string PTA
        {
            get { return _pta; }
            set
            {
                _pta = value;
                var project = RecordProjects.Where(prjt => prjt.Name == SelectedProject).FirstOrDefault();
                if (project != null)
                {
                    if (project.PTA != value) { project.PTA = value; }
                }

                OnPropertyChanged(nameof(PTA));
            }
        }
        private string _unloadingId = string.Empty;
        public string UnloadingId
        {
            get { return _unloadingId; }
            set
            {
                if (_unloadingId != value)
                {
                    _unloadingId = value;
                    OnPropertyChanged(nameof(UnloadingId));
                }
            }
        }
        private string _comment = string.Empty;
        public string Comment
        {
            get { return _comment; }
            set
            {
                _comment = value;
                var project = RecordProjects.Where(prjt => prjt.Name == SelectedProject).FirstOrDefault();
                if (project != null)
                {
                    if (project.Comment != value) { project.Comment = value; }
                }

                OnPropertyChanged(nameof(Comment));
            }
        }

        private bool _isWaitingForHandle = false;
        public bool IsWaitingForHandle
        {
            get { return _isWaitingForHandle; }
            set
            {
                _isWaitingForHandle = value;
                
                OnPropertyChanged(nameof(IsWaitingForHandle));
            }
        }

        private DateTime? _deliveryDate;
        public DateTime? DeliveryDate
        {
            get { return _deliveryDate; }
            set
            {
                var project = RecordProjects.Where(prjt => prjt.Name == SelectedProject).FirstOrDefault();
                if (project != null)
                {
                    if (project.DeliveryDate != value) { project.DeliveryDate = value; }
                }

                _deliveryDate = value;
                OnPropertyChanged(nameof(DeliveryDate));
            }
        }
        private DateTime _creationDate;
        public DateTime CreationDate
        {
            get { return _creationDate; }
            set
            {
                if (_creationDate != value)
                {
                    _creationDate = value;
                    OnPropertyChanged(nameof(CreationDate));
                }
            }
        }
        private DateTime _updatedDate;
        public DateTime UpdatedDate
        {
            get { return _updatedDate; }
            set
            {
                if (_updatedDate != value)
                {
                    _updatedDate = value;
                    OnPropertyChanged(nameof(UpdatedDate));
                }
            }
        }
        private string _createdBy = string.Empty;
        public string CreatedBy
        {
            get { return _createdBy; }
            set
            {
                if (_createdBy != value)
                {
                    _createdBy = value;
                    OnPropertyChanged(nameof(CreatedBy));
                }
            }
        }
        private string _releasedBy = string.Empty;
        public string ReleasedBy
        {
            get { return _releasedBy; }
            set
            {
                if (_releasedBy != value)
                {
                    _releasedBy = value;
                    OnPropertyChanged(nameof(ReleasedBy));
                }
            }
        }
        private string _processBy = string.Empty;
        public string ProcessBy
        {
            get { return _processBy; }
            set
            {
                if (_processBy != value)
                {
                    _processBy = value;
                    OnPropertyChanged(nameof(ProcessBy));
                }
            }
        }
        private string _preparedBy = string.Empty;
        public string PreparedBy
        {
            get { return _preparedBy; }
            set
            {
                if (_preparedBy != value)
                {
                    _preparedBy = value;
                    OnPropertyChanged(nameof(PreparedBy));
                }
            }
        }
        private string _shippedBy = string.Empty;
        public string ShippedBy
        {
            get { return _shippedBy; }
            set
            {
                if (_shippedBy != value)
                {
                    _shippedBy = value;
                    OnPropertyChanged(nameof(ShippedBy));
                }
            }
        }
        private DateTime _releasedDate;
        public DateTime ReleasedDate
        {
            get { return _releasedDate; }
            set
            {
                if (_releasedDate != value)
                {
                    _releasedDate = value;
                    OnPropertyChanged(nameof(ReleasedDate));
                }
            }
        }
        private DateTime _processDate;
        public DateTime ProcessDate
        {
            get { return _processDate; }
            set
            {
                if (_processDate != value)
                {
                    _processDate = value;
                    OnPropertyChanged(nameof(ProcessDate));
                }
            }
        }
        private DateTime _preparedDate;
        public DateTime PreparedDate
        {
            get { return _preparedDate; }
            set
            {
                if (_preparedDate != value)
                {
                    _preparedDate = value;
                    OnPropertyChanged(nameof(PreparedDate));
                }
            }
        }
        private DateTime _shippedDate;
        public DateTime ShippedDate
        {
            get { return _shippedDate; }
            set
            {
                if (_shippedDate != value)
                {
                    _shippedDate = value;
                    OnPropertyChanged(nameof(ShippedDate));
                }
            }
        }
        private string _actionType = string.Empty;
        public string ActionType
        {
            get { return _actionType; }
            set
            {
                if (_actionType != value)
                {
                    _actionType = value;
                    OnPropertyChanged(nameof(ActionType));
                }
            }
        }
        private bool _wasNotified = false;
        public bool WasNotified
        {
            get { return _wasNotified; }
            set
            {
                if (_wasNotified != value)
                {
                    _wasNotified = value;
                    OnPropertyChanged(nameof(WasNotified));
                }
            }
        }
        private string _paleteNumber = "0";
        public string PaleteNumber
        {
            get { return _paleteNumber; }
            set
            {
                if (_paleteNumber != value)
                {
                    if (value.IsNullOrEmpty())
                    {
                        _paleteNumber = "0";
                    }
                    else if(value.StartsWith("0") && value.Length != 1)
                    {
                        int i = 0;
                        while (value.Substring(i).StartsWith("0") && i < value.Length)
                        { 
                            i++;
                        }
                        if (value.Substring(i) == "") { _paleteNumber = "0"; }
                        else { _paleteNumber = value.Substring(i); }
                       
                    }
                    else
                    {
                        _paleteNumber = value;
                    }
                    OnPropertyChanged(nameof(PaleteNumber));
                }
            }
        } 
        private bool _rescheduled = false;
        public bool Rescheduled
        {
            get { return _rescheduled; }
            set
            {
                if (_rescheduled != value)
                {
                    _rescheduled = value;
                    OnPropertyChanged(nameof(Rescheduled));
                }
            }
        }
        private string _originalSchedule = string.Empty;
        public string OriginalSchedule
        {
            get { return _originalSchedule; }
            set
            {
                if (_originalSchedule != value)
                {
                    _originalSchedule = value;
                    OnPropertyChanged(nameof(OriginalSchedule));
                }
            }
        }
        private DateTime _transportArrivalDate;
        public DateTime TransportArrivalDate
        {
            get { return _transportArrivalDate; }
            set
            {
                if (_transportArrivalDate != value)
                {
                    _transportArrivalDate = value;
                    OnPropertyChanged(nameof(TransportArrivalDate));
                }
            }
        }
        private bool _hasTransportArrived;
        public bool HasTransportArrived
        {
            get { return _hasTransportArrived; }
            set
            {
                if (_hasTransportArrived != value)
                {
                    _hasTransportArrived = value;
                    OnPropertyChanged(nameof(HasTransportArrived));
                }
            }
        }
        private List<string> _transportModes = new();
        public List<string> TransportModes
        {
            get { return _transportModes; }
            set
            {
                if (_transportModes != value)
                {
                    _transportModes = value;
                    OnPropertyChanged(nameof(TransportModes));
                }
            }
        }
        private List<string> _shipmentTypes = new();
        public List<string> ShipmentTypes
        {
            get { return _shipmentTypes; }
            set
            {
                if (_shipmentTypes != value)
                {
                    _shipmentTypes = value;
                    OnPropertyChanged(nameof(ShipmentTypes));
                }
            }
        }
        public bool HasTransportArrivePreviously = false;
        public bool HasRecordBeenCancelledPreviously = false;

        /////////////////////////////////////////////////////////
        // In case of DHL/TNT                                  //
        /////////////////////////////////////////////////////////
        public ObservableCollection<string> Projects { get; set; } = new();

        public ObservableCollection<Project> RecordProjects { get; set; } = new();

        private string _selectedProject = string.Empty;
        public string SelectedProject
        {
            get { return _selectedProject; }
            set
            {
                if (_selectedProject != value)
                {
                    if(value == "Geral") { ProjectDetailsVisible = Visibility.Collapsed; }
                    else { ProjectDetailsVisible = Visibility.Visible; }

                    _selectedProject = value;
                    OnPropertyChanged(nameof(SelectedProject));
                }
            }
        }

        private Project _selectedProjectX = new();
        public Project SelectedProjectX
        {
            get { return _selectedProjectX; }
            set
            {
                SolidColorBrush color;
                if (_selectedProjectX != value)
                {
                    foreach(Project p in RecordProjects)
                    {
                        if(p.Name == "Geral") { color = new SolidColorBrush(Color.FromRgb(0, 62, 110)); }
                        else 
                        { 
                            color = ColorConverter.GetSolidColorBrushFromHSL(p.UniqueHSVColorHue, 0.8, 0.6);
                        }

                        if (p.Name == value.Name)
                        {
                            ChangeProjectColor(p, color, true);
                        }
                        else
                        {
                            ChangeProjectColor(p, color, false);
                        }
                    }
                    _selectedProjectX = value;
                    OnPropertyChanged(nameof(SelectedProjectX));
                }
            }
        }
        public void ChangeProjectColor(Project project, SolidColorBrush color, bool active) 
        {
            if (active)
            {
                project.BackgroundColor = new SolidColorBrush(Colors.White);
                project.ColorForeground = color;
                project.ColorBorder = color;
            }
            else
            {
                project.BackgroundColor = color;
                project.ColorForeground = new SolidColorBrush(Colors.White);
                project.ColorBorder = color;
            }
        }
        private string _insertingProject = string.Empty;
        public string InsertingProject
        {
            get { return _insertingProject; }
            set
            {
                if (_insertingProject != value)
                {
                    _insertingProject = value;
                    OnPropertyChanged(nameof(InsertingProject));
                }
            }
        }
        private int _internalId = -1;
        public int InternalId
        {
            get { return _internalId; }
            set
            {
                if (_internalId != value)
                {
                    _internalId = value;
                    OnPropertyChanged(nameof(InternalId));
                }
            }
        }

        private string _person = string.Empty;
        public string Person
        {
            get { return _person; }
            set
            {
                var project = RecordProjects.Where(prjt => prjt.Name == SelectedProject).First();
                if(project.Person != value) { project.Person = value; }

                _person = value;
                OnPropertyChanged(nameof(Person));
            }
        }

        private string _phone = string.Empty;
        public string Phone
        {
            get { return _phone; }
            set
            {
                var project = RecordProjects.Where(prjt => prjt.Name == SelectedProject).First();
                if (project.Phone != value) { project.Phone = value; }

                _phone = value;
                OnPropertyChanged(nameof(Phone));
            }
        }
        private string _selectedType { get; set; } = string.Empty;
        public string SelectedType
        {
            get { return _selectedType; }
            set
            {
                var project = RecordProjects.Where(prjt => prjt.Name == SelectedProject).First();
                project.Type = value;

                _selectedType = value;
                OnPropertyChanged(nameof(SelectedType));
            }
        }

        private string _address = string.Empty;
        public string Address
        {
            get { return _address; }
            set
            {
                var project = RecordProjects.Where(prjt => prjt.Name == SelectedProject).First();
                if (project.Address != value) { project.Address = value; }

                _address = value;
                OnPropertyChanged(nameof(Address));
            }
        }

        private Visibility _projectDetailsVisible { get; set; } = Visibility.Visible;
        public Visibility ProjectDetailsVisible
        {
            get { return _projectDetailsVisible; }
            set
            {
                if (_projectDetailsVisible != value)
                {
                    _projectDetailsVisible = value;
                    OnPropertyChanged(nameof(ProjectDetailsVisible));
                }
            }
        }

        /////////////////////////////////////////////////////////
        // In case of DHL/TNT - END                            //
        /////////////////////////////////////////////////////////

        public void SetDataTimeValues(string dateData)
        {
            // Date is stored like 12/01/2023+10:00
            char[] delimiterChars = { '+' };
            string[] splitContent = dateData.Split(delimiterChars);

            Day = splitContent[0];

            string date = splitContent[0];
            string[] splitDate = date.Split("/");
            date = splitDate[1] + "/" + splitDate[0] + "/" + splitDate[2];
            
            Day = date;
            Time = splitContent[1];
        }
        public static DateTime GetDateFromTag(string dateData)
        {
            // Date is stored like 13/01/2023+10:00
            char[] delimiterChars = { '+' };
            string[] splitContent = dateData.Split(delimiterChars);

            string date = splitContent[0];
            string[] splitDate = date.Split("/");
            date = splitDate[0] + "/" + splitDate[1] + "/" + splitDate[2];

            return DateTime.Parse(date + " " + splitContent[1] + ":00");
        }
    }
}
