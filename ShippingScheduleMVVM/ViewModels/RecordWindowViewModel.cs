
using Microsoft.IdentityModel.Tokens;
using ShippingScheduleMVVM.Commands;
using ShippingScheduleMVVM.Helpers;
using ShippingScheduleMVVM.Models;
using ShippingScheduleMVVM.Services;
using ShippingScheduleMVVM.ViewModels.Modals;
using ShippingScheduleMVVM.Views;
using ShippingScheduleMVVM.Views.Modals;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace ShippingScheduleMVVM.ViewModels
{
    public class RecordWindowViewModel : ViewModelBase
    {

        private int _sliderValueMax = 0;
        public int SliderValueMax
        {
            get => _sliderValueMax;
            set
            {
                if (_sliderValueMax != value)
                {
                    _sliderValueMax = value;
                    OnPropertyChanged(nameof(SliderValueMax));
                }
            }
        }

        private double _sliderValue = 1;
        public double SliderValue
        {
            get => _sliderValue;
            set
            {
                if (_sliderValue != value)
                {
                    _sliderValue = value;
                    OnPropertyChanged(nameof(SliderValue));
                }
            }
        }

        private Visibility _toBeHandledVisibility = Visibility.Collapsed;
        public Visibility ToBeHandledVisibility
        {
            get { return _toBeHandledVisibility; }
            set
            {
                _toBeHandledVisibility = value;
                OnPropertyChanged("ToBeHandledVisibility");
            }
        }

        private Visibility _updateShippedSettingsVisibility = Visibility.Collapsed;
        public Visibility UpdateShippedSettingsVisibility
        {
            get { return _updateShippedSettingsVisibility; }
            set
            {
                _updateShippedSettingsVisibility = value;
                OnPropertyChanged("UpdateShippedSettingsVisibility");
            }
        }


        private double _preparedNumber{ get; set; } = 0;
        public double PreparedNumber
        {
            get { return _preparedNumber; }
            set
            {
                _preparedNumber = value;
                OnPropertyChanged("PreparedNumber");
            }
        } 
        private GridLength _preparedPercentage { get; set; } = new GridLength(100, GridUnitType.Star);
        public GridLength PreparedPercentage
        {
            get { return _preparedPercentage; }
            set
            {
                _preparedPercentage = value;
                OnPropertyChanged("PreparedPercentage");
            }
        }
        private GridLength _nonPreparedPercentage { get; set; } = new GridLength(0, GridUnitType.Star);
        public GridLength NonPreparedPercentage
        {
            get { return _nonPreparedPercentage; }
            set
            {
                _nonPreparedPercentage = value;
                OnPropertyChanged("PreparedPercentage");
            }
        }
        private bool _isNotificationChangesEnabled { get; set; } = true;
        public bool IsNotificationChangesEnabled
        {
            get { return _isNotificationChangesEnabled; }
            set
            {
                _isNotificationChangesEnabled = value;
                OnPropertyChanged("IsNotificationChangesEnabled");
            }
        }
        public bool IsTransportArrivedChangesEnabled { get; set; } = true;
        public bool IsCreatedEnabled { get; set; } = true;
        public bool IsReleasedEnabled { get; set; } = true;
        public bool IsHandledEnabled { get; set; } = true;
        public bool IsPreparedEnabled { get; set; } = true;
        public bool IsShippedEnabled { get; set; } = true;
        public bool IsRecordShipped { get; set; } = false;
        public bool IsRecordCreatedReleased{ get; set; } = false;
        public bool IsShipmentTypeEnabled { get; set; } = true;
        public bool IsTransportModeEnabled { get; set; } = true;
        public bool IsCarrierEnabled { get; set; } = true;
        public bool UpdateTablesButtonEnabled { get; set; } = true;
        private string _recordTitle { get; set; } = "";
        public string RecordTitle
        {
            get
            { return _recordTitle; }
            set
            {
                if (_recordTitle != value)
                {
                    _recordTitle = value;

                    OnPropertyChanged("RecordTitle");
                }
            }
        }
        private WindowSettings? _windowSettings { get; set; }
        public WindowSettings WindowSettings
        {
            get
            { return _windowSettings ?? new WindowSettings("ThisRecordWindow"); }
            set
            {
                if (_windowSettings != value)
                {
                    _windowSettings = value;

                    OnPropertyChanged("WindowSettings");
                }
            }
        }
        private Visibility _wasNotifiedVisible;
        public Visibility WasNotifiedVisible
        {
            get { return _wasNotifiedVisible; }
            set
            {
                if (_wasNotifiedVisible != value)
                {
                    _wasNotifiedVisible = value;
                    OnPropertyChanged(nameof(WasNotifiedVisible));
                }
            }
        }
        private SolidColorBrush _borderCreated = new SolidColorBrush(Color.FromRgb(211, 211, 211));
        public SolidColorBrush BorderCreated
        {
            get { return _borderCreated; }
            set
            {
                if (_borderCreated != value)
                {
                    _borderCreated = value;
                    OnPropertyChanged(nameof(BorderCreated));
                    if (value.Color == Color.FromRgb(189, 189, 189))
                    {
                        // Perform specific action for "Created" category
                        ThicknessBorderCreated = new Thickness(2);
                        ThicknessBorderHandled = new Thickness(1);
                        ThicknessBorderPrepared= new Thickness(1);
                        ThicknessBorderReleased= new Thickness(1);
                        ThicknessBorderShipped= new Thickness(1);
                    }
                }
            }
        }
        private SolidColorBrush _borderReleased = new SolidColorBrush(Color.FromRgb(211, 211, 211));
        public SolidColorBrush BorderReleased
        {
            get { return _borderReleased; }
            set
            {
                if (_borderReleased != value)
                {
                    _borderReleased = value;
                    OnPropertyChanged(nameof(BorderReleased));
                    if (value.Color == Color.FromRgb(255, 203, 33))
                    {
                        // Perform specific action for "Released" category
                        ThicknessBorderCreated = new Thickness(1);
                        ThicknessBorderHandled = new Thickness(1);
                        ThicknessBorderPrepared = new Thickness(1);
                        ThicknessBorderReleased = new Thickness(2);
                        ThicknessBorderShipped = new Thickness(1);
                    }
                }
            }
        }
        private SolidColorBrush _borderHandled = new SolidColorBrush(Color.FromRgb(211, 211, 211));
        public SolidColorBrush BorderHandled
        {
            get { return _borderHandled; }
            set
            {
                if (_borderHandled != value)
                {
                    _borderHandled = value;
                    OnPropertyChanged(nameof(BorderHandled));
                    if (value.Color == Color.FromRgb(245, 147, 0))
                    {
                        // Perform specific action for "Process" category
                        ThicknessBorderCreated = new Thickness(1);
                        ThicknessBorderHandled = new Thickness(2);
                        ThicknessBorderPrepared = new Thickness(1);
                        ThicknessBorderReleased = new Thickness(1);
                        ThicknessBorderShipped = new Thickness(1);
                    }
                }
            }
        }
        private SolidColorBrush _borderPrepared = new SolidColorBrush(Color.FromRgb(211, 211, 211));
        public SolidColorBrush BorderPrepared
        {
            get { return _borderPrepared; }
            set
            {
                if (_borderPrepared != value)
                {
                    _borderPrepared = value;
                    OnPropertyChanged(nameof(BorderPrepared));
                    if (value.Color == Color.FromRgb(128, 176, 255))
                    {
                        // Perform specific action for "Prepared" category
                        ThicknessBorderCreated = new Thickness(1);
                        ThicknessBorderHandled = new Thickness(1);
                        ThicknessBorderPrepared = new Thickness(2);
                        ThicknessBorderReleased = new Thickness(1);
                        ThicknessBorderShipped = new Thickness(1);
                    }
                }
            }
        }
        private SolidColorBrush _borderShipped = new SolidColorBrush(Color.FromRgb(211, 211, 211));
        public SolidColorBrush BorderShipped
        {
            get { return _borderShipped; }
            set
            {
                if (_borderShipped != value)
                {
                    _borderShipped = value;
                    OnPropertyChanged(nameof(BorderShipped));
                    if (value.Color == Color.FromRgb(24, 219, 131))
                    {
                        // Perform specific action for "Shipped" category
                        ThicknessBorderCreated = new Thickness(1);
                        ThicknessBorderHandled = new Thickness(1);
                        ThicknessBorderPrepared = new Thickness(1);
                        ThicknessBorderReleased = new Thickness(1);
                        ThicknessBorderShipped = new Thickness(2);
                    }
                }
            }
        }
        private Visibility _visualCancelVisibility = Visibility.Hidden;
        public Visibility VisualCancelVisibility
        {
            get { return _visualCancelVisibility; }
            set
            {
                if (_visualCancelVisibility != value)
                {
                    _visualCancelVisibility = value;
                    OnPropertyChanged("VisualCancelVisibility");
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
        private Thickness _thicknessBorderCreated = new(1);
        public Thickness ThicknessBorderCreated
        {
            get { return _thicknessBorderCreated; }
            set
            {
                if (_thicknessBorderCreated != value)
                {
                    _thicknessBorderCreated = value;
                    OnPropertyChanged(nameof(ThicknessBorderCreated));
                }
            }
        }
        private Thickness _thicknessBorderReleased = new(1);
        public Thickness ThicknessBorderReleased
        {
            get { return _thicknessBorderReleased; }
            set
            {
                if (_thicknessBorderReleased != value)
                {
                    _thicknessBorderReleased = value;
                    OnPropertyChanged(nameof(ThicknessBorderReleased));
                }
            }
        }
        private Thickness _thicknessBorderHandled = new(1);
        public Thickness ThicknessBorderHandled
        {
            get { return _thicknessBorderHandled; }
            set
            {
                if (_thicknessBorderHandled != value)
                {
                    _thicknessBorderHandled = value;
                    OnPropertyChanged(nameof(ThicknessBorderHandled));
                }
            }
        }
        private Thickness _thicknessBorderPrepared = new(1);
        public Thickness ThicknessBorderPrepared
        {
            get { return _thicknessBorderPrepared; }
            set
            {
                if (_thicknessBorderPrepared != value)
                {
                    _thicknessBorderPrepared = value;
                    OnPropertyChanged(nameof(ThicknessBorderPrepared));
                }
            }
        }
        private Thickness _thicknessBorderShipped = new(1);
        public Thickness ThicknessBorderShipped
        {
            get { return _thicknessBorderShipped; }
            set
            {
                if (_thicknessBorderShipped != value)
                {
                    _thicknessBorderShipped = value;
                    OnPropertyChanged(nameof(ThicknessBorderShipped));
                }
            }
        }
        public ICommand ShipmentTypeChanged { get; }
        public ICommand BorderCategoryClicked { get; }
        public ICommand SelectPart { get; }
        public ICommand DeselectPart { get; }
        public ICommand AddPart { get; }
        public ICommand RemovePart { get; }
        public ICommand DuplicatePart { get; }
        public ICommand TransferPart { get; }
        public ICommand ChangeShipTo { get; }
        public ICommand ChangeUnloadingPoint{ get; }
        public ICommand CancelRecord { get; }
        public ICommand DeleteRecord { get; }
        public ICommand PrintRecord { get; }
        public ICommand CloseWindow { get; }
        public ICommand ChangeLineRecord { get; }
        public ICommand TextBoxChangedFocus { get; }
        public ICommand OpenImportExcel { get; }
        public ICommand OpenCommentPart { get; }
        public ICommand HoverCategoryEnter { get; }
        public ICommand HoverCategoryLeave { get; }
        public ICommand ConfirmChanges { get; }
        public ICommand TransportModeChanged { get; }
        public ICommand MinimizeWindow { get; }
        public ICommand OpenInsertDeliveryNotesMenu { get; }
        public ICommand DeleteDeliveryNote { get; }
        public ICommand UpdateShippedSettings { get; }
         
        public ICommand ToBeHandledSliderChanged { get; }
        private DispatcherTimer _toHandletimer = new();
        public DateTime ToHandleDate;
        public ObservableCollection<Part> Parts { get; set; }
        public ObservableCollection<DeliveryNote> DeliveryNotes { get; set; } = new();
     
        public Path ToBeHandledPath { get; set; } = new();
        public Record SelectedRecord { get; set; }
        public Dictionary<string, Action<RecordWindowViewModel>> BorderSelector = new Dictionary<string, Action<RecordWindowViewModel>>
        {
            { "Created", viewmodel => viewmodel.BorderCreated = new SolidColorBrush(Color.FromRgb(189, 189, 189)) },
            { "Released", viewmodel => viewmodel.BorderReleased = new SolidColorBrush(Color.FromRgb(255, 203, 33)) },
            { "Process", viewmodel => viewmodel.BorderHandled = new SolidColorBrush(Color.FromRgb(245, 147, 0)) },
            { "Prepared", viewmodel => viewmodel.BorderPrepared = new SolidColorBrush(Color.FromRgb(128, 176, 255)) },
            { "Shipped", viewmodel => viewmodel.BorderShipped = new SolidColorBrush(Color.FromRgb(24, 219, 131)) },
        };

        public int _currentListIndex = -1;
        private bool _isChangingLines = false;
        private bool _isChangingLinesRight = false;

        public string previousCategory = "";
        public bool isRecordPrevioslyCreated = false;
        public bool isRecordPreviouslyHandled = false;
        public RecordWindowViewModel(int id, MainWindowViewModel mainWindowVm, ScheduleManagementViewModel scheduleVm, string action, string originalSchedule)
        {
            if(action == "Updating")
            {
                RecordTitle = "Update a Record";
                DatabaseOperations database = new();

 
                SelectedRecord = database.SelectRecordById(id);
                SelectedRecord.ShipmentTypes = database.SelectTypesOfShipment();
                IsCancelled = SelectedRecord.IsCancelled;

                DeliveryNotes = database.SelectRecordDeliveryNotes(id);

                if(SelectedRecord.Category == "Shipped")
                {
                    UpdateShippedSettingsVisibility = Visibility.Visible;
                }

                if ((SelectedRecord.Category == "Released" || SelectedRecord.Category == "Created") && DeliveryNotes.Count > 0)
                {
                    Parts = database.SelectRecordParts(id);
                    database.DeleteAllParts(SelectedRecord.Id);

                    foreach (DeliveryNote dNote in DeliveryNotes)
                    {
                        database.InsertPartBasedOnDeliveryNote(dNote, SelectedRecord.Id);
                    }

                    database.ReInsertPartComments(SelectedRecord.Id, Parts);
                }

                if (SelectedRecord.IsWaitingForHandle && SelectedRecord.Category == "Process")
                {
                    ToHandleDate = database.SelectZVT11Update();
                    SliderValueMax = (int)(ToHandleDate - DateTime.Now).TotalSeconds;
                    // Set the tick event handler
                    _toHandletimer = new DispatcherTimer();
                    _toHandletimer.Interval = TimeSpan.FromMilliseconds(500);
                    _toHandletimer.Tick += UpdateToHandle_timer;
                    _toHandletimer.Start();
                    ToBeHandledVisibility = Visibility.Visible; 
                }

                Parts = database.SelectRecordParts(id);
                Parts.CollectionChanged += Parts_CollectionChanged;

                foreach (Part pt in Parts)
                {
                    _currentListIndex++;
                    pt.ListIndex = _currentListIndex;

                    if (!pt.Comment.IsNullOrEmpty()) { pt.CommentFlagVisibility = Visibility.Visible; }
                }

                Action<RecordWindowViewModel> selector = BorderSelector[SelectedRecord.Category];
                selector(this);

                if (SelectedRecord.ShipmentType == "Especial") { WasNotifiedVisible = Visibility.Collapsed; }
                if (SelectedRecord.ShipmentType == "Regular") { WasNotifiedVisible = Visibility.Visible; }

                if (SelectedRecord.IsCancelled) 
                {
                    VisualCancelVisibility = Visibility.Visible;
                    SelectedRecord.HasRecordBeenCancelledPreviously = true;
                }
                if (SelectedRecord.HasTransportArrived)
                {
                    SelectedRecord.HasTransportArrivePreviously = true;
                }

                previousCategory = SelectedRecord.Category;

                if(SelectedRecord.Category == "Shipped") { IsRecordShipped = true; }
                if (SelectedRecord.Category == "Created") { isRecordPrevioslyCreated = true; }
                if (SelectedRecord.Category == "Released") { isRecordPreviouslyHandled = true; }

                double preparedPercentage = GetPreparationPercentage();
                preparedPercentage = Math.Round(preparedPercentage, 2);
                if(double.IsNaN(preparedPercentage))
                {
                    preparedPercentage = 0;
                }

                PreparedNumber = preparedPercentage;
                PreparedPercentage = new GridLength(preparedPercentage, GridUnitType.Star);
                NonPreparedPercentage = new GridLength(100 - preparedPercentage, GridUnitType.Star);
            }
            else
            {
                WasNotifiedVisible = Visibility.Visible;
                NonPreparedPercentage = new GridLength(100, GridUnitType.Star);
                PreparedPercentage = new GridLength(0, GridUnitType.Star);
                RecordTitle = "Create a Record";
                Parts = new();
                SelectedRecord = new();

                SelectedRecord.Id = 0;
                SelectedRecord.PaleteNumber = "0";
                SelectedRecord.Category = "Created";
                
                Action<RecordWindowViewModel> selector = BorderSelector[SelectedRecord.Category];
                selector(this);

                Parts.CollectionChanged += Parts_CollectionChanged;
                
                WasNotifiedVisible = Visibility.Collapsed;
                
                DatabaseOperations database = new();
                SelectedRecord.ShipmentTypes = database.SelectTypesOfShipment(); 
            }

            // Set general permissions
            UpdateTablesButtonEnabled = RolePermissions.CanAlterPartsTables(SelectedRecord.Category);
            RolePermissions.ChangePartsAfterUpdating(SelectedRecord.Category, Parts, this);
            IsCreatedEnabled = RolePermissions.CanSetCategoryCreated(SelectedRecord.Category);
            IsReleasedEnabled = RolePermissions.CanSetCategoryReleased(SelectedRecord.Category);
            IsHandledEnabled = RolePermissions.CanSetCategoryHandled(SelectedRecord.Category); ;
            IsPreparedEnabled = RolePermissions.CanSetCategoryPrepared(SelectedRecord.Category);
            IsShippedEnabled = RolePermissions.CanSetCategoryShipped(SelectedRecord.Category);
            IsShipmentTypeEnabled = RolePermissions.CanSetTypeOfShipment(SelectedRecord.Category);
            IsTransportModeEnabled  = RolePermissions.CanSetTransportMode(SelectedRecord.Category);
            IsCarrierEnabled = RolePermissions.CanSetCarrier(SelectedRecord.Category);

            System.Windows.Window? owner = Application.Current.Windows.OfType<System.Windows.Window>().FirstOrDefault(w => w.Title == "ThisMainWindow");
            if (owner != null)
            {
                if (owner.WindowState == WindowState.Maximized)
                {
                    WindowSettings = new("ThisRecordWindow");
                    WindowSettings.Initialize(1.9, 875, 520, true, "ThisMainWindow");
                    WindowSettings.CenterOnScreenSpecificWindow("ThisMainWindow", App.InitialDPI);
                }
                else
                {
                    WindowSettings = new("ThisRecordWindow");
                    WindowSettings.Initialize(1.9, 875, 520, true, "ThisMainWindow");
                    WindowSettings.Left = owner.Left + (owner.ActualWidth - WindowSettings.Width) / 2;
                    WindowSettings.Top = owner.Top + (owner.ActualHeight - WindowSettings.Height) / 2;
                }
            }

            OpenInsertDeliveryNotesMenu = new RelayCommand(() =>
            {
                System.Windows.Window? window = System.Windows.Application.Current.Windows.OfType<System.Windows.Window>().FirstOrDefault(w => w.DataContext == this);
                if (window == null) { return; }

                AddDeliveryNote popup = new() { DataContext = new AddDeliveryNoteViewModel(this) };
                popup.Owner = window;

                popup.ShowDialog();
            });


            // Initialize commands
            ShipmentTypeChanged = new RelayCommand(() => {
                if (SelectedRecord.ShipmentType == "Especial") { WasNotifiedVisible = Visibility.Collapsed; }
                if (SelectedRecord.ShipmentType == "Regular") { WasNotifiedVisible = Visibility.Visible; }
            });

            TransportModeChanged = new RelayCommand(() => {
                if(SelectedRecord.TransportMode == "DHL" || SelectedRecord.TransportMode == "TNT")
                {
                    // Create
                    if (action == "Creating")
                    {
                        /*
                        ClosingWindow(mainWindowVm, scheduleVm);
                        System.Windows.Window? owner = Application.Current.Windows.OfType<System.Windows.Window>().FirstOrDefault(w => w.Title == "ThisMainWindow");
                        if (owner == null) { return; }


                        var window = new Record2Window() { DataContext = new Record2WindowViewModel(-1, mainWindowVm, scheduleVm, action, originalSchedule, SelectedRecord.TransportMode) };
                        window.Owner = owner;

                        scheduleVm.IsRecordOpen = false;
                        mainWindowVm.VisibilityOverlay = Visibility.Visible;
                        window.Show();
                        */
                    }

                }
            });

            MinimizeWindow = new MinimizeWindowCommand(() => WindowSettings.State, value => WindowSettings.State = value);

            BorderCategoryClicked = new RelayCommandParemeters(parameter => {
                ClickedCategory(parameter);
            });

            UpdateShippedSettings = new RelayCommand(() => {
                DatabaseOperations database = new();
                database.UpdateRecordShippedData(SelectedRecord, App.loggedUser.Username);
            });

            DeleteDeliveryNote = new RelayCommandParemeters(parameter =>
            {
                if (SelectedRecord.Category != "Created") { return; }

                if (parameter is DeliveryNote dn)
                {
                    DeliveryNotes.Remove(dn);
                }
            });
            SelectPart = new RelayCommandParemeters(parameter => {
                SelectedPart(parameter);
            });

            DeselectPart = new RelayCommand(() => {
                _lastIndex = -69;
            });

            ChangeShipTo = new RelayCommandParemeters(parameter => {
                ChangedShipTo(parameter);
            });
            ChangeUnloadingPoint = new RelayCommandParemeters(parameter => {
                ChangedUnloadingPoint(parameter);
            });

            CancelRecord = new RelayCommandParemeters(parameter => {
                CanceledRecord(parameter);
            });

            PrintRecord = new RelayCommandParemeters(parameter => {
                if (SelectedRecord.Category == "Created") { return; }

               
                System.Windows.Window? owner = Application.Current.Windows.OfType<System.Windows.Window>().FirstOrDefault(w => w.DataContext == this);
                if (owner == null) { return; }

                RecordPrint printer = new(Parts, SelectedRecord);
                printer.Owner = owner;
                printer.ShowDialog();
            });

            DeleteRecord = new RelayCommand(() => {
                if (!RolePermissions.CanDeleteRecord(SelectedRecord.Category, (previousCategory == "Created" || action == "Creating"))) { return; }
                if (action == "Updating")
                {
                    System.Windows.Window? owner = Application.Current.Windows.OfType<System.Windows.Window>().FirstOrDefault(w => w.DataContext == this);
                    if (owner == null) { return; }

                    var window = new CustomAlertWindow("Delete Record?", "Are you sure you want to delete this Record? [" + SelectedRecord.ShipmentType + " #" + SelectedRecord.Id + "]", this);
                    window.Owner = owner;

                    window.ShowDialog();

                    if (window.DataContext is CustomAlertWindowViewModel vm)
                    {
                        if (vm.WasConfirmed)
                        {
                            DatabaseOperations database = new();
                            database.DeleteRecord(SelectedRecord.Id);
                        }
                        else { return; } 
                    }
                    ClosingWindow(mainWindowVm, scheduleVm);
                    scheduleVm.UpdateTimerManualy();
                }
            });
            OpenCommentPart = new RelayCommandParemeters(parameter => {
                if(parameter is Part part)
                {
                    System.Windows.Window? owner = Application.Current.Windows.OfType<System.Windows.Window>().FirstOrDefault(w => w.DataContext == this);
                    if (owner == null) { return; }

                    var position = WindowScreen.GetLocationMouse("ThisRecordWindow");
                    if (position == null) { return; }
                    var point = (Point)position;

                    var window = new PartComment() { DataContext = new PartCommentViewModel(part, point.Y, point.X, this) };

                    window.Owner = owner;

                    window.Show();
                }
            });

            RemovePart = new RelayCommand(() => {
                List<int> indexes = new();
                foreach(Part part in Parts)
                {
                    if(part.Selected == true)
                    {
                        indexes.Add(part.ListIndex);
                    }
                }
                
                foreach(int index in indexes)
                {
                    Parts.Remove(Parts.Where(p => p.ListIndex == index).Single());
                }
            });

            DuplicatePart = new RelayCommand(() =>
            {
                List<Part> indexes = new();
                foreach (Part part in Parts)
                {
                    if (part.Selected == true)
                    {
                        indexes.Add(new Part()
                        {
                            ShipTo = part.ShipTo,
                            ShipToName = part.ShipToName,
                            UnloadingPoint = part.UnloadingPoint,
                            DeliveryNote = part.DeliveryNote,
                            TransportNumber = part.TransportNumber,
                            APN = part.APN,
                            CPN = part.CPN,
                            Designation = part.Designation,
                            ExpectedQuantity = part.ExpectedQuantity,
                            FinalQuantity = part.FinalQuantity
                        });
                    }
                }

                foreach (Part index in indexes)
                {
                    _currentListIndex++;
                    index.ListIndex = _currentListIndex;
                    Parts.Add(index);                
                }
            });

            TransferPart = new RelayCommand(() =>
            { 

            });

            AddPart = new RelayCommand(() => {
                _currentListIndex++;
                Parts.Add(new Part() { ListIndex = _currentListIndex });
            });

            HoverCategoryEnter = new RelayCommandParemeters( parameter => {

                System.Windows.Window? owner = Application.Current.Windows.OfType<System.Windows.Window>().FirstOrDefault(w => w.DataContext == this);
                if (owner == null) { return; }

                var image = (Border?)parameter;
                if (image == null) { return; }

                // Get the position of the element relative to the window
                Point relativePosition = image.TranslatePoint(new Point(0, 0), owner);

                // Convert the relative position to screen coordinates
                Point screenPosition = owner.PointToScreen(relativePosition);

                var window = new CategoryHoverWindow() { DataContext = new CategoryHoverViewModel(screenPosition.Y, screenPosition.X, SelectedRecord, image.Tag.ToString() ?? "", this) };
                window.Owner = owner;

                window.Show();
            });

            HoverCategoryLeave = new RelayCommand(() =>
            {
                System.Windows.Window? window = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.Title == "ThisCategoryHoverWindow");
                System.Windows.Window? owner = Application.Current.Windows.OfType<System.Windows.Window>().FirstOrDefault(w => w.DataContext == this);
                window?.Close();
                owner?.Focus();
            });

            OpenImportExcel = new RelayCommand(() =>{

            });

            TextBoxChangedFocus = new RelayCommandParemeters(parameter => {
                if (parameter is TextChangedEventArgs e)
                {
                    if (e.OriginalSource is TextBox textbox)
                    {
                        if (!textbox.IsFocused)
                        {
                            textbox.Focus();
                        }
                        if (_isChangingLines)
                        {
                            _isChangingLines = false;
                            textbox.CaretIndex = textbox.Text.Length;
                        }
                        if (_isChangingLinesRight)
                        {
                            _isChangingLinesRight = false;
                            textbox.CaretIndex = 0;
                        }
                    }
                }
            });

            ChangeLineRecord = new RelayCommandParemeters(parameter => {
                
                if(parameter is KeyEventArgs e)
                {
                    if (e.Key == Key.Down)
                    {
                        if (e.OriginalSource is TextBox textbox)
                        {
                            var datacontext = (Part)textbox.DataContext;

                            int index = Parts.ToList().FindIndex(p => p.ListIndex == datacontext.ListIndex);
                            if(index + 1 == Parts.Count) { return; }


                            var nextPart = Parts[index + 1];
                            ChangeTextboxTextByTag(textbox.Tag.ToString() ?? "",nextPart);
                        }
                    }
                    if (e.Key == Key.Up)
                    {
                        if (e.OriginalSource is TextBox textbox)
                        {
                            var datacontext = (Part)textbox.DataContext;

                            int index = Parts.ToList().FindIndex(p => p.ListIndex == datacontext.ListIndex);
                            if (index - 1 == -1) { return; }

                            var nextPart = Parts[index - 1];
                            ChangeTextboxTextByTag(textbox.Tag.ToString() ?? "", nextPart);
                        }
                    }
                    if (e.Key == Key.Left)
                    {
                        if (e.OriginalSource is TextBox textbox)
                        {
                            if (textbox.CaretIndex != 0) { return; }
                            if((textbox.Tag.ToString() ?? "") == "APN") { return; }
                            var datacontext = (Part)textbox.DataContext;

                            int index = Parts.ToList().FindIndex(p => p.ListIndex == datacontext.ListIndex);
                            

                            var nextPart = Parts[index];
                            ChangeTextboxTextByTagLeft(textbox.Tag.ToString() ?? "", nextPart);
                        }
                    }
                    if (e.Key == Key.Right)
                    {
                        if (e.OriginalSource is TextBox textbox)
                        {
                            if (textbox.CaretIndex != textbox.Text.Length) { return; }
                            if ((textbox.Tag.ToString() ?? "") == "TransportNumber") { return; }
                            var datacontext = (Part)textbox.DataContext;

                            int index = Parts.ToList().FindIndex(p => p.ListIndex == datacontext.ListIndex);


                            var nextPart = Parts[index];
                            ChangeTextboxTextByTagRight(textbox.Tag.ToString() ?? "", nextPart);
                        }
                    }
                }
            });

            CloseWindow = new RelayCommand(() =>
            {
                ClosingWindow(mainWindowVm, scheduleVm);
            });

            ToBeHandledSliderChanged = new RelayCommand(() =>
            {
                SliderValueChanged();
            });
          
            ConfirmChanges = new RelayCommand(() =>
            {
                // Conditions to be confirmed
                if (SelectedRecord.ShipmentType.IsNullOrEmpty() || SelectedRecord.TransportMode.IsNullOrEmpty() )
                { SendAlert("Error","Record needs at least a shipment type, and a transport mode"); return; }
                
                if (!RolePermissions.CanSaveRecordChanges(IsRecordShipped, previousCategory, SelectedRecord.Category, ToBeHandledVisibility)) { return; }
                
                // To add to role permissions class : if there is no delivery notes dont save 
                if(DeliveryNotes.Count == 0 & SelectedRecord.Category != "Created") { return; }

                if(action == "Creating")
                {
                    DatabaseOperations database = new();
                    SelectedRecord.SetDataTimeValues(originalSchedule);
                    SelectedRecord.CreationDate = DateTime.Now;
                    SelectedRecord.Id = database.InsertNewRecord(SelectedRecord, App.loggedUser.Username, originalSchedule, ReturnUniqueShipTosInParts(Parts));


                    // Insert Delivery Notes
                    foreach (DeliveryNote dNote in DeliveryNotes)
                    {
                        database.InsertDeliveryNote(dNote, SelectedRecord.Id);
                    }

                    // Insert Delivery Notes Parts
                    foreach (DeliveryNote dNote in DeliveryNotes)
                    {
                        database.InsertPartBasedOnDeliveryNote(dNote, SelectedRecord.Id);
                    }

                    SelectedRecord.UpdatedDate = DateTime.Now;

                    // Update parts title
                    Parts = database.SelectRecordParts(SelectedRecord.Id);
                    database.UpdateRecord(SelectedRecord, App.loggedUser.Username, ReturnUniqueShipTosInParts(Parts));

                    ClosingWindow(mainWindowVm, scheduleVm);
                    scheduleVm.UpdateTimerManualy();
                }

                if (action == "Updating")
                {
                    DatabaseOperations database = new();
 
                    SelectedRecord.UpdatedDate = DateTime.Now;
                    
                    database.UpdateRecord(SelectedRecord, App.loggedUser.Username, ReturnUniqueShipTosInParts(Parts));

                    // In Parts on Released
                    if (SelectedRecord.Category == "Process" || SelectedRecord.Category == "Created")
                    {
                        database.DeleteAllDeliveryNotes(SelectedRecord.Id);

                        // Insert Delivery Notes
                        foreach (DeliveryNote dNote in DeliveryNotes)
                        {
                                database.InsertDeliveryNote(dNote, SelectedRecord.Id);
                        }

                        database.DeleteAllParts(SelectedRecord.Id);
                        foreach (DeliveryNote dNote in DeliveryNotes)
                        {
                            database.InsertPartBasedOnDeliveryNote(dNote, SelectedRecord.Id);
                        } 
                    }

                    database.ReInsertPartComments(SelectedRecord.Id, Parts);

                    ClosingWindow(mainWindowVm, scheduleVm);
                    scheduleVm.UpdateTimerManualy();
                }
            });
        }
        private string _previousCategory = "";
        private int _previousCount = 0;
        private void Parts_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if(_previousCategory != SelectedRecord.Category || Parts.Count != _previousCount )
            {
                _previousCategory = SelectedRecord.Category;
                _previousCount = Parts.Count;
                RolePermissions.ChangePartsAfterUpdating(SelectedRecord.Category, Parts, this);
            }
        }

        public void ClosingWindow(MainWindowViewModel mainWindowVm, ScheduleManagementViewModel scheduleVm)
        {
            System.Windows.Window? window = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.DataContext == this);
            window?.Close();
            mainWindowVm.VisibilityOverlay = Visibility.Collapsed;
            scheduleVm.IsRecordOpen = false;
        }

        private double GetPreparationPercentage()
        {
            double total = 0;
            double prepared = 0;

            DatabaseOperations database = new();
            
            foreach(Part part in Parts)
            {
                if (database.IsDeliveryNotePrepared(part.DeliveryNote.Replace(" ", "")))
                {
                    prepared += part.ExpectedQuantity;
                }
                total += part.ExpectedQuantity;
            }
            return ((prepared/total) * 100);
        }

        private double _sliderValueInSeconds = 0;
        private void UpdateToHandle_timer(object? sender, EventArgs e)
        { 
            DateTime now = DateTime.Now;
            if(DateTime.Now < ToHandleDate)
            {
                _sliderValueInSeconds = _sliderValueInSeconds + 0.5;
                SliderValue = 360 * (_sliderValueInSeconds / _sliderValueMax);
            }
            else
            {
                _toHandletimer.Stop();
                ToBeHandledVisibility = Visibility.Collapsed;
            }
        }

        public string ReturnUniqueShipTosInParts(ObservableCollection<Part> parts)
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

        public void ChangeTextboxTextByTag(string tag, Part part)
        {
            switch (tag)
            {
                case "CPN":
                    part.CPN += "x";
                    _isChangingLines = true;
                    part.CPN = part.CPN.Substring(0, part.CPN.Length - 1);
                    break;
                case "APN":
                    part.APN += "x";
                    _isChangingLines = true;
                    part.APN = part.APN.Substring(0, part.APN.Length - 1);
                    break;
                case "Designation":
                    part.Designation += "x";
                    _isChangingLines = true;
                    part.Designation = part.Designation.Substring(0, part.Designation.Length - 1);
                    break;
                case "ExpectedQuantity":
                    part.ExpectedQuantity += 1;
                    _isChangingLines = true;
                    part.ExpectedQuantity -= 1;
                    break;
                case "FinalQuantity":
                    part.FinalQuantity += 1;
                    _isChangingLines = true;
                    part.FinalQuantity -= 1;
                    break;
                case "DeliveryNote":
                    part.DeliveryNote += "x";
                    _isChangingLines = true;
                    part.DeliveryNote = part.DeliveryNote.Substring(0, part.DeliveryNote.Length - 1);
                    break;
                case "TransportNumber":
                    part.TransportNumber += "x";
                    _isChangingLines = true;
                    part.TransportNumber = part.TransportNumber.Substring(0, part.TransportNumber.Length - 1);
                    break;
                default:
                    break;
            }
        }
        // create an initial record and initial parts
        private bool HasRecordChanged()
        {
            DatabaseOperations database = new();

            ObservableCollection<Part> parts = database.SelectRecordParts(SelectedRecord.Id);
            Record selectedRecord = database.SelectRecordById(SelectedRecord.Id);

            int count = 0;
            foreach (Part part in Parts)
            {
                if (part.Id != parts[count].Id || 
                    part.APN != parts[count].APN ||
                    part.CPN != parts[count].CPN ||
                    part.ExpectedQuantity != parts[count].ExpectedQuantity ||
                    part.FinalQuantity != parts[count].FinalQuantity)
                {
                    return true;
                }

                count++;
            }
            if (parts[0] == Parts[0] && selectedRecord == SelectedRecord) { return false; }
            return true;
        }

        public void ChangeTextboxTextByTagRight(string tag, Part part)
        {
            switch (tag)
            {
                case "APN":
                    part.CPN += "x";
                    _isChangingLinesRight = true;
                    part.CPN = part.CPN.Substring(0, part.CPN.Length - 1);
                    break;
                case "CPN":
                    part.Designation += "x";
                    _isChangingLinesRight = true;
                    part.Designation = part.Designation.Substring(0, part.Designation.Length - 1);
                    break;
                case "Designation":
                    part.ExpectedQuantity += 1;
                    _isChangingLinesRight = true;
                    part.ExpectedQuantity -= 1;
                    break;
                case "ExpectedQuantity":
                    part.FinalQuantity += 1;
                    _isChangingLinesRight = true;
                    part.FinalQuantity -= 1;
                    break;
                case "FinalQuantity":
                    part.DeliveryNote += "x";
                    _isChangingLinesRight = true;
                    part.DeliveryNote = part.DeliveryNote.Substring(0, part.DeliveryNote.Length - 1);
                    break;
                case "DeliveryNote":
                    part.TransportNumber += "x";
                    _isChangingLines = true;
                    part.TransportNumber = part.TransportNumber.Substring(0, part.TransportNumber.Length - 1);
                    break;
                default:
                    break;
            }
        }
        public void ChangeTextboxTextByTagLeft(string tag, Part part)
        {
            switch (tag)
            {
                case "CPN":
                    part.APN += "x";
                    _isChangingLines = true;
                    part.APN = part.APN.Substring(0, part.APN.Length - 1);
                    break;
                case "Designation":
                    part.CPN += "x";
                    _isChangingLines = true;
                    part.CPN = part.CPN.Substring(0, part.CPN.Length - 1);
                    break;
                case "ExpectedQuantity":
                    part.Designation += "x";
                    _isChangingLines = true;
                    part.Designation = part.Designation.Substring(0, part.Designation.Length - 1);
                    break;
                case "FinalQuantity":
                    part.ExpectedQuantity += 1;
                    _isChangingLines = true;
                    part.ExpectedQuantity -= 1;
                    break;
                case "DeliveryNote":
                    part.FinalQuantity += 1;
                    _isChangingLines = true;
                    part.FinalQuantity -= 1;
                    break;
                case "TransportNumber":
                    part.DeliveryNote += "x";
                    _isChangingLines = true;
                    part.DeliveryNote = part.DeliveryNote.Substring(0, part.DeliveryNote.Length - 1);
                    break;
                default:
                    break;
            }
        }

        private void CanceledRecord(object? parameter)
        {
            if (IsCancelled)
            {
                IsCancelled = false;
                SelectedRecord.IsCancelled = false;
                VisualCancelVisibility = Visibility.Hidden;
            }
            else
            {
                var window = new CancelRecordWindow() { DataContext = new CancelRecordViewModel(this) };

                System.Windows.Window? owner = Application.Current.Windows.OfType<System.Windows.Window>().FirstOrDefault(w => w.DataContext == this);
                if (owner == null) { return; }

                window.Owner = owner;

                window.ShowDialog();
            }
        }
        private void ChangedUnloadingPoint(object? parameter)
        {

        } 
        private bool TransferedBills()
        {
            return false;
        } 
        private void ChangedShipTo(object? parameter)
        {

        }
        private int _lastIndex = -69;
        private void SelectedPart(object? parameter)
        {
            var part = (Part?)parameter;

            if(part != null)
            {
                // Find part index in observable collection Parts 
                List<Part> partsList = Parts.ToList();
                int index = partsList.FindIndex(p => p.ListIndex == part.ListIndex);

                // If pressed shift and doesn't have a last index 
                if ((Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)) && _lastIndex == -69)
                {
                    _lastIndex = index;
                    return;
                }
                // If pressed shift and has a last index 
                if (_lastIndex != -69 && (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)))
                {
                    int aux = index;
                    if (_lastIndex > index)
                    {
                        while (_lastIndex > index)
                        {
                            Parts[index].Selected = true;
                            index++;
                        }
                    }
                    if (_lastIndex < index)
                    {
                        while (_lastIndex < index)
                        {
                            Parts[index].Selected = true;
                            index--;
                        }
                    }
                    _lastIndex = index;
                    return;
                }

                // If didn't pressed shift and doesn't have a last index
                if (!Keyboard.IsKeyDown(Key.LeftShift) && !Keyboard.IsKeyDown(Key.RightShift))
                {
                    _lastIndex = index;
                    return;
                }
            } 
        } 
        private void ClickedCategory(object? parameter)
        {
            string? tag = (string?)parameter;
            if (tag != null)
            {
                IsNotificationChangesEnabled = false;
                // To add to role Permissions : Set Permissions to notifications                 
                if (tag == "Created" || tag == "Released") { IsNotificationChangesEnabled = true;  }
 
                if (!RolePermissions.AllRequirementsMetHandled(Parts,tag, this)) { return; }
                if (!RolePermissions.AllRequirementsMetShipped(Parts, tag, this)) { return; }
                ResetBordersCategories();

                SelectedRecord.Category = tag;
                _previousCategory = tag;
                RolePermissions.ChangePartsAfterUpdating(SelectedRecord.Category, Parts, this);
                Action<RecordWindowViewModel> selector = BorderSelector[tag];
                selector(this);
            }
        } 
        private void ResetBordersCategories()
        {
            BorderCreated = new(Color.FromRgb(211, 211, 211));
            BorderReleased = new(Color.FromRgb(211, 211, 211));
            BorderHandled = new(Color.FromRgb(211, 211, 211));
            BorderPrepared = new(Color.FromRgb(211, 211, 211));
            BorderShipped = new(Color.FromRgb(211, 211, 211));
        }
        private void SendAlert(string title,string message)
        {
            System.Windows.Window? owner = Application.Current.Windows.OfType<System.Windows.Window>().FirstOrDefault(w => w.DataContext == this);
            if (owner == null) { return; }

            var window = new CustomAlertWindow($"{title}", $"{message}", this);
            window.Owner = owner;

            window.ShowDialog();
        }
        private void SliderValueChanged()
        {
            double radians = SliderValue * (Math.PI / 180);

            var value1 = 100 * Math.Sin(radians);
            value1 = Math.Round(value1, 1);
            var value1Formated = value1.ToString().Replace(",", ".");

            var value2 = -100 * Math.Cos(radians);
            value2 = Math.Round(value2, 1);
            var value2Formated = value2.ToString().Replace(",", ".");

            string pathData;

            // Check if the angle is in the right or left half of the circle
            if (SliderValue <= 180)
            {
                ToBeHandledPath.ColSpan = 2;
                ToBeHandledPath.RowSpan = 2;
                ToBeHandledPath.HorizontalAlignment = HorizontalAlignment.Left;
                ToBeHandledPath.VerticalAlignment = VerticalAlignment.Top;

                // Creating a path data for the right half of the circle using 'A' (arc) command
                pathData = $"M0,0 L0,-100 A100,100 0 1 0 {value1Formated},{value2Formated} z";
            }
            else
            {
                // Creating a path data for the left half of the circle using 'A' (arc) command 
                pathData = $"M0,0 L0,-100 A100,100 1 0 0 {value1Formated},{value2Formated} z";
            }

            ToBeHandledPath.Data = Geometry.Parse(pathData);

            if (SliderValue > 180)
            {
                ToBeHandledPath.ColSpan = 1;
                ToBeHandledPath.RowSpan = 2;
                ToBeHandledPath.HorizontalAlignment = HorizontalAlignment.Left;
                ToBeHandledPath.VerticalAlignment = VerticalAlignment.Top;
            }

            if (SliderValue > 270)
            {
                ToBeHandledPath.ColSpan = 1;
                ToBeHandledPath.RowSpan = 1;
                ToBeHandledPath.HorizontalAlignment = HorizontalAlignment.Right;
                ToBeHandledPath.VerticalAlignment = VerticalAlignment.Top;
            }
        }

    }
}
