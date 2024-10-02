using ShippingScheduleMVVM.Commands;
using ShippingScheduleMVVM.Models;
using ShippingScheduleMVVM.Services;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace ShippingScheduleMVVM.ViewModels.Modals
{
    public class ExportRecordsExcelViewModel : ViewModelBase
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
        private DateTime _startDate { get; set; } = DateTime.Now;
        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                if (_startDate != value)
                {
                    _startDate = value;
                    OnPropertyChanged(nameof(StartDate));
                }
            }
        }
        private DateTime _endDate { get; set; } = DateTime.Now;
        public DateTime EndDate
        {
            get { return _endDate; }
            set
            {
                if (_endDate != value)
                {
                    _endDate = value;
                    OnPropertyChanged(nameof(EndDate));
                }
            }
        }
        private bool _exportParts = false;
        public bool ExportParts
        {
            get { return _exportParts; }
            set
            {
                _exportParts = value;
                OnPropertyChanged(nameof(ExportParts));
            }
        }
        public ICommand ConfirmChanges { get; }
        public ICommand CloseWindow { get; }
        public ICommand ChangeExportParts { get; }
        public ExportRecordsExcelViewModel()
        {
            System.Windows.Window? owner = System.Windows.Application.Current.Windows.OfType<System.Windows.Window>().FirstOrDefault(w => w.Title == "ThisMainWindow"); 
            DatabaseOperations database = new();
            
            if (owner != null)
            {
                if (owner.WindowState == WindowState.Maximized)
                {
                    WindowSettings = new("ThisExportRecordsExcelWindow");
                    WindowSettings.Initialize(1.8, 205, 190, true, "ThisMainWindow");
                    WindowSettings.CenterOnScreenSpecificWindow("ThisMainWindow",App.InitialDPI);
                }
                else
                {
                    WindowSettings = new("ThisExportRecordsExcelWindow"); 
                    WindowSettings.Initialize(1.8, 205, 190, true, "ThisMainWindow");
                    WindowSettings.Left = owner.Left + (owner.ActualWidth - WindowSettings.Width) / 2;
                    WindowSettings.Top = owner.Top + (owner.ActualHeight - WindowSettings.Height) / 2;
                }
            }  
            // Initialize Commands
            CloseWindow = new CloseWindowCommand(false);

            ChangeExportParts = new RelayCommand(() =>
            {
                if (ExportParts) { ExportParts = false; return; }
                ExportParts = true;
            });

            ConfirmChanges = new RelayCommand(() => {

                var directoryDialog = new FolderBrowserDialog();
                DialogResult result = directoryDialog.ShowDialog();

                if(result == DialogResult.OK)
                {
                    System.Windows.MessageBox.Show(directoryDialog.SelectedPath);
                }
                else { return; }

                ExcelExport.ExportRecordsBetweenDates(StartDate, EndDate, directoryDialog.SelectedPath,ExportParts);

                System.Windows.Window? window = System.Windows.Application.Current.Windows.OfType<System.Windows.Window>().FirstOrDefault(w => w.Title == "ThisExportRecordsExcelWindow");
                if (window == null) { return; }
                window.Close();
            });
        }
    }
}
