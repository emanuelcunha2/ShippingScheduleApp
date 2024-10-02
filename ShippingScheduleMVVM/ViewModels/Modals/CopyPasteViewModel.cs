using ShippingScheduleMVVM.Commands;
using ShippingScheduleMVVM.Models;
using ShippingScheduleMVVM.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;

namespace ShippingScheduleMVVM.ViewModels.Modals
{
    public class CopyPasteViewModel : ViewModelBase
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
        private SolidColorBrush _brushCopyForeground = new(System.Windows.Media.Color.FromRgb(0, 0, 0));
        public SolidColorBrush BrushCopyForeground
        {
            get { return _brushCopyForeground; }
            set
            {
                if (_brushCopyForeground != value)
                {
                    _brushCopyForeground = value;
                    OnPropertyChanged("BrushCopyForeground");
                }
            }
        }
        private SolidColorBrush _brushPasteForeground = new(System.Windows.Media.Color.FromRgb(0, 0, 0));
        public SolidColorBrush BrushPasteForeground
        {
            get { return _brushPasteForeground; }
            set
            {
                if (_brushPasteForeground != value)
                {
                    _brushPasteForeground = value;
                    OnPropertyChanged("BrushPasteForeground");
                }
            }
        }
        private SolidColorBrush _brushCopy = new(System.Windows.Media.Color.FromRgb(250, 250, 250));
        public SolidColorBrush BrushCopy
        {
            get { return _brushCopy; }
            set
            {
                if (_brushCopy != value)
                {
                    _brushCopy = value;
                    OnPropertyChanged("BrushCopy");
                }
            }
        }
        private SolidColorBrush _brushPaste = new(System.Windows.Media.Color.FromRgb(250, 250, 250));
        public SolidColorBrush BrushPaste
        {
            get { return _brushPaste; }
            set
            {
                if (_brushPaste != value)
                {
                    _brushPaste = value;
                    OnPropertyChanged("BrushPaste");
                }
            }
        }
        public ICommand HoverCopy { get; }
        public ICommand HoverPaste { get; }
        public ICommand LeaveCopy { get; }
        public ICommand LeavePaste { get; }
        public ICommand ClickPaste { get; }
        public ICommand ClickCopy { get; }
        public CopyPasteViewModel(double top, double left, ScheduleRow? row, ScheduleRecord? record, ScheduleManagementViewModel originViewmodel, string pasteTag)
        {
            if (originViewmodel.RecordPastingCache == null)
            {
                BrushPasteForeground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(135, 135, 135));
            }
            if (row != null && record == null)
            {
                BrushCopyForeground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(135, 135, 135));
            }

            WindowSettings = new("ThisCopyPasteWindow");
            WindowSettings.Initialize(1.8, 75, 50, true, "ThisMainWindow");

            WindowSettings.Top = top;
            WindowSettings.Left = left;

            HoverCopy = new RelayCommand(() => {
                BrushCopy = new SolidColorBrush(System.Windows.Media.Color.FromRgb(233, 233, 233));
                BrushPaste = new SolidColorBrush(Colors.Transparent);
            });
            HoverPaste = new RelayCommand(() => {
                BrushCopy = new SolidColorBrush(Colors.Transparent);
                BrushPaste = new SolidColorBrush(System.Windows.Media.Color.FromRgb(233, 233, 233));
            });

            LeaveCopy = new RelayCommand(() => {
                BrushCopy = new SolidColorBrush(Colors.Transparent);
            });
            LeavePaste = new RelayCommand(() => {
                BrushPaste = new SolidColorBrush(Colors.Transparent);
            });

            ClickCopy = new RelayCommand(() => {
                // If is copy
                if (row == null && record != null)
                {
                    originViewmodel.RecordPastingCache = record;
                    System.Windows.Window? window = System.Windows.Application.Current.Windows.OfType<System.Windows.Window>().FirstOrDefault(w => w.Title == "ThisCopyPasteWindow");
                    if (window != null)
                    {
                        window.Owner = null;
                        window.Close();
                    }
                }
            });
            ClickPaste = new RelayCommand(() => {
                // If is paste
                if (originViewmodel.RecordPastingCache != null)
                {
                    if (row == null) { return; }
                    System.Windows.Window? window = System.Windows.Application.Current.Windows.OfType<System.Windows.Window>().FirstOrDefault(w => w.Title == "ThisCopyPasteWindow");
                    if (window != null)
                    {
                        DatabaseOperations database = new();
                        Record sr = database.SelectRecordById(originViewmodel.RecordPastingCache.Id);

                        ObservableCollection<Part> Parts;

                        if (sr.TransportMode == "DHL" || sr.TransportMode == "TNT") { Parts = database.SelectRecordPartsTNT(originViewmodel.RecordPastingCache.Id); }
                        else { Parts = database.SelectRecordParts(originViewmodel.RecordPastingCache.Id); }

                        var date = ScheduleRow.GetDateFromTag(row, pasteTag);
                        var dropDate = Record.GetDateFromTag(date);

                        sr.Time = row.Time;
                        sr.Day = dropDate.Month + "/" + dropDate.Day + "/" + dropDate.Year;

                        sr.Id = database.InsertCopyRecord(sr, App.loggedUser.Username, date, Part.ReturnShipToList(Parts));

                        ObservableCollection<Project> projects;
                        if (sr.TransportMode == "DHL" || sr.TransportMode == "TNT")
                        {
                            projects = database.SelectProjects(originViewmodel.RecordPastingCache.Id);
                            foreach (Project p in projects)
                            {
                                p.Id = -69;
                                sr.RecordProjects.Add(p);
                                sr.Projects.Add(p.Name);
                            }
                            database.InsertProjects(sr);
                        }

                        foreach (Part part in Parts)
                        {
                            part.FinalQuantity = 0;
                            part.DeliveryNote = "";
                            part.TransportNumber = "";
                            
                            part.SelectedProject = sr.RecordProjects.FirstOrDefault(prj => prj.InternalId == part.SelectedProject.InternalId) ?? new Project();
                            if (sr.TransportMode == "DHL" || sr.TransportMode == "TNT") { database.InsertPartTNT(part, sr.Id); }
                            else { database.InsertPart(part, sr.Id); }
                        }

                        window.Owner = null;
                        window.Close();
                        originViewmodel.UpdateTimerManualy();
                    }
                }
            });
        }
    }
}
