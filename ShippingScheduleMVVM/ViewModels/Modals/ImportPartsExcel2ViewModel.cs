
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using ShippingScheduleMVVM.Commands;
using ShippingScheduleMVVM.Models;
using ShippingScheduleMVVM.Services;
using ShippingScheduleMVVM.Views.Modals;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace ShippingScheduleMVVM.ViewModels.Modals
{
    public class ImportPartsExcel2Viewmodel : ViewModelBase
    {
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
        private string _item1 = string.Empty;
        public string Item1
        {
            get { return _item1; }
            set
            {
                if (!_item1.IsNullOrEmpty()) { _item1 = value; ResetParts(); OnPropertyChanged(nameof(Item1)); }
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
                if (!_item2.IsNullOrEmpty()) { _item2 = value; ResetParts(); OnPropertyChanged(nameof(Item2)); }
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
                if (!_item3.IsNullOrEmpty()) { _item3 = value; ResetParts(); OnPropertyChanged(nameof(Item3)); }
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
                if (!_item4.IsNullOrEmpty()) { _item4 = value; ResetParts(); OnPropertyChanged(nameof(Item4)); }
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
                if (!_item5.IsNullOrEmpty()) { _item5 = value; ResetParts(); OnPropertyChanged(nameof(Item5)); }
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
                if (!_item6.IsNullOrEmpty()) { _item6 = value; ResetParts(); OnPropertyChanged(nameof(Item6)); }
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
                if (!_item7.IsNullOrEmpty()) { _item7 = value; ResetParts(); OnPropertyChanged(nameof(Item7)); }
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
                if (!_item8.IsNullOrEmpty()) { _item8 = value; ResetParts(); OnPropertyChanged(nameof(Item8)); }
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
                if (!_item9.IsNullOrEmpty()) { _item9 = value; ResetParts(); OnPropertyChanged(nameof(Item9)); }
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
                if (!_item10.IsNullOrEmpty()) { _item10 = value; ResetParts(); OnPropertyChanged(nameof(Item10)); }
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
                if (!_item11.IsNullOrEmpty()) { _item11 = value; ResetParts(); OnPropertyChanged(nameof(Item11)); }
                if (_item11 != value)
                {
                    _item11 = value;
                    OnPropertyChanged(nameof(Item11));
                }
            }
        }

        private string _selectedLayout = "Default";
        public string SelectedLayout
        {
            get { return _selectedLayout; }
            set
            {
                if (_selectedLayout != value)
                {
                    if (value != App.loggedUser.Username)
                    {
                        CantSelectLayoutVisibility = Visibility.Visible;
                    }
                    else { CantSelectLayoutVisibility = Visibility.Collapsed; }

                    _selectedLayout = value;
                    OnPropertyChanged(nameof(SelectedLayout));

                    SetDefaultLayout();
                    ResetParts();
                }
            }
        }
        private Visibility _cantSelectLayoutVisibility = Visibility.Visible;
        public Visibility CantSelectLayoutVisibility
        {
            get { return _cantSelectLayoutVisibility; }
            set
            {
                if (_cantSelectLayoutVisibility != value)
                {
                    _cantSelectLayoutVisibility = value;
                    OnPropertyChanged(nameof(CantSelectLayoutVisibility));
                }
            }
        }
        private string _excelImportText = string.Empty;
        public string ExcelImportText
        {
            get { return _excelImportText; }
            set
            {
                if (_excelImportText != value)
                {
                    _excelImportText = value;
                    OnPropertyChanged(nameof(ExcelImportText));
                }
            }
        }
        public ObservableCollection<string> Layouts { get; set; } = new ObservableCollection<string>() {
            "Default",
            App.loggedUser.Username,
        };
        public ObservableCollection<string> Items { get; set; } = new ObservableCollection<string>() {
            "ShipTo",
            "ShipTo Name",
            "APN",
            "CPN",
            "Description",
            "Unload. Point",
            "Quantity",
            "Delivery Note",
            "Num. Conta",
            "P.O",
            "Track. Number",
            "Null",
        };
        public ObservableCollection<ExcelPart> Parts { get; set; } = new();
        public ObservableCollection<ShipTo> AllShipTos { get; set; }
        public ObservableCollection<string> UnloadingPoints { get; set; } = new ObservableCollection<string>();
        public ICommand ImportFromExcel { get; }
        public ICommand ClearTable { get; }
        public ICommand SaveCustomLayout { get; }
        public ICommand ConfirmChanges { get; }
        public ICommand CloseWindow { get; }
        public ImportPartsExcel2Viewmodel(ViewModelBase viewmodel)
        {
            dynamic? records = null;
            string ownerTitle = "";
            var vm1 = viewmodel as RecordWindowViewModel;
            // var vm2 = viewmodel as Record2WindowViewModel;

            if (vm1 != null) { records = vm1; ownerTitle = "ThisRecordWindow"; }
            // if (vm2 != null) { records = vm2; ownerTitle = "ThisRecord2Window"; }

            DatabaseOperations database = new();
            AllShipTos = database.SelectShipTos();

            var hasLayout = Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\ShippingSchedule", "HasLayoutDHL" + App.loggedUser.Username, "Null")?.ToString() ?? "Null";
            if (hasLayout == "true") { SelectedLayout = App.loggedUser.Username; }

            SetDefaultLayout();

            ResetParts();

            System.Windows.Window? owner = Application.Current.Windows.OfType<System.Windows.Window>().FirstOrDefault(w => w.DataContext == viewmodel);

            WindowSettings = new("ThisImportPartsExcelWindow");
            WindowSettings.Initialize(1.8, 915, 400, true, ownerTitle);

            if (owner != null)
            {
                WindowSettings.Left = owner.Left + (owner.ActualWidth - WindowSettings.Width) / 2;
                WindowSettings.Top = owner.Top + (owner.ActualHeight - WindowSettings.Height) / 2;
            }
            // Initialize Commands
            CloseWindow = new CloseWindowCommand(false);

            ImportFromExcel = new RelayCommand(() =>
            {
                Import();
            });
            ClearTable = new RelayCommand(() =>
            {
                ResetParts();
                ExcelImportText = "";
            });

            SaveCustomLayout = new RelayCommand(() =>
            {
                if (SelectedLayout == App.loggedUser.Username) { SavedCustomLayout(); }
            });

            ConfirmChanges = new RelayCommand(() =>
            {
                if (Parts.Count == 1) { return; }
                var parts = CreatePartsFromTable();
                if (parts == null) { return; }

                foreach (Part part in parts)
                {
                    // Search ShipToName
                    if (!part.ShipTo.IsNullOrEmpty())
                    {
                        foreach (ShipTo st in AllShipTos)
                        {
                            if (part.ShipTo == st.ShipToNumber)
                            {
                                part.ShipToName = st.ShipToName;
                            }
                        }
                    }
                    if (!part.UnloadingPoint.IsNullOrEmpty())
                    {
                        UnloadingPoints.Clear();
                        UnloadingPoints = database.SelectUnloadingPoints(part.ShipTo);

                        bool foundUP = false;
                        foreach (string up in UnloadingPoints)
                        {
                            if (up == part.UnloadingPoint) { foundUP = true; break; }
                        }
                        if (!foundUP) { part.UnloadingPoint = ""; }
                    }
                    if (records != null)
                    {
                        records.AddPartToParts(part);
                    }
                }
                System.Windows.Window? window = Application.Current.Windows.OfType<System.Windows.Window>().FirstOrDefault(w => w.DataContext == this);
                if (window == null) { return; }
                window.Close();
            });
        }
        private void ShowErrorConversion()
        {
            var window = new CustomAlertWindow("Erro quantidades", "Quantidades com o formato errado [" + QuantitiesErrorConverting, this);
            System.Windows.Window? owner = Application.Current.Windows.OfType<System.Windows.Window>().FirstOrDefault(w => w.DataContext == this);
            if (owner == null) { return; }

            window.Owner = owner;
            window.ShowDialog();
        }

        public bool ErrorConvertingQuantity = false;
        public bool StopFlagQuantities = false;
        public string QuantitiesErrorConverting = "";
        private List<Part>? CreatePartsFromTable()
        {
            List<Part> parts = new();
            bool hasPassedTitleRow = false;

            foreach (ExcelPart excelPart in Parts)
            {
                Part part = new();
                if (!hasPassedTitleRow) { hasPassedTitleRow = true; continue; }
                AssignProperty(part, Item1, 1, excelPart);
                AssignProperty(part, Item2, 2, excelPart);
                AssignProperty(part, Item3, 3, excelPart);
                AssignProperty(part, Item4, 4, excelPart);
                AssignProperty(part, Item5, 5, excelPart);
                AssignProperty(part, Item6, 6, excelPart);
                AssignProperty(part, Item7, 7, excelPart);
                AssignProperty(part, Item8, 8, excelPart);
                AssignProperty(part, Item9, 9, excelPart);
                AssignProperty(part, Item10, 10, excelPart);
                AssignProperty(part, Item11, 11, excelPart);
                parts.Add(part);
            }
            if (ErrorConvertingQuantity)
            {
                ShowErrorConversion();
                ErrorConvertingQuantity = false;
                StopFlagQuantities = false;
                QuantitiesErrorConverting = "";
                return null;
            }
            ErrorConvertingQuantity = false;
            StopFlagQuantities = false;
            QuantitiesErrorConverting = "";

            return parts;
        }
        private void AssignProperty(Part part, string item, int itemNumber, ExcelPart excelPart)
        {
            string propertyName = "Item" + itemNumber;
            var propertyInfo = typeof(ExcelPart).GetProperty(propertyName);
            if (propertyInfo != null)
            {
                string? itemValue = (string?)propertyInfo.GetValue(excelPart);
                if (itemValue == null) { return; }

                if (item == ("APN"))
                {
                    part.APN = itemValue;
                }
                else if (item == ("CPN"))
                {
                    part.CPN = itemValue;
                }
                else if (item == ("ShipTo"))
                {
                    part.ShipTo = itemValue;
                }
                else if (item == ("ShipTo Name"))
                {
                    // part.ShipToName = itemValue;
                }
                else if (item == ("Description"))
                {
                    part.Designation = itemValue;
                }
                else if (item == ("Unload. Point"))
                {
                    part.UnloadingPoint = itemValue;
                }
                else if (item == ("Quantity"))
                {
                    if (int.TryParse(itemValue, out int result))
                    {
                        // The conversion was successful
                        part.ExpectedQuantity = result;
                    }
                    else
                    {
                        ErrorConvertingQuantity = true;
                        part.ExpectedQuantity = -1;
                        if (QuantitiesErrorConverting.Length < 70) { QuantitiesErrorConverting += " " + itemValue + " |"; }
                        else if (!StopFlagQuantities) { QuantitiesErrorConverting += " ... ]"; StopFlagQuantities = true; }
                    }
                }
                else if (item == ("Num. Conta"))
                {
                    part.AccountNumber = itemValue;
                }
                else if (item == ("P.O"))
                {
                    part.PO = itemValue;
                }
                else if (item == ("Track. Number"))
                {
                    part.TrackNumber = itemValue;
                }
                else if (item == ("Delivery Note"))
                {
                    part.DeliveryNote = itemValue;
                }
            }
        }
        private void ResetParts()
        {
            Parts?.Clear();
            Parts?.Add(GenerateTitlePart());
        }
        private ExcelPart GenerateTitlePart()
        {
            ExcelPart part = new ExcelPart()
            {
                Item1 = Item1,
                Item2 = Item2,
                Item3 = Item3,
                Item4 = Item4,
                Item5 = Item5,
                Item6 = Item6,
                Item7 = Item7,
                Item8 = Item8,
                Item9 = Item9,
                Item10 = Item10,
                Item11 = Item11,
                ItemVisibility1 = (Item1 != "Null") ? Visibility.Visible : Visibility.Collapsed,
                ItemVisibility2 = (Item2 != "Null") ? Visibility.Visible : Visibility.Collapsed,
                ItemVisibility3 = (Item3 != "Null") ? Visibility.Visible : Visibility.Collapsed,
                ItemVisibility4 = (Item4 != "Null") ? Visibility.Visible : Visibility.Collapsed,
                ItemVisibility5 = (Item5 != "Null") ? Visibility.Visible : Visibility.Collapsed,
                ItemVisibility6 = (Item6 != "Null") ? Visibility.Visible : Visibility.Collapsed,
                ItemVisibility7 = (Item7 != "Null") ? Visibility.Visible : Visibility.Collapsed,
                ItemVisibility8 = (Item8 != "Null") ? Visibility.Visible : Visibility.Collapsed,
                ItemVisibility9 = (Item9 != "Null") ? Visibility.Visible : Visibility.Collapsed,
                ItemVisibility10 = (Item10 != "Null") ? Visibility.Visible : Visibility.Collapsed,
                ItemVisibility11 = (Item11 != "Null") ? Visibility.Visible : Visibility.Collapsed,
            };
            part.PartBackground = new SolidColorBrush(Colors.Transparent);
            return part;
        }
        private void SetDefaultLayout()
        {
            if (SelectedLayout == "Default")
            {
                Item1 = "ShipTo";
                Item2 = "ShipTo Name";
                Item3 = "APN";
                Item4 = "CPN";
                Item5 = "Description";
                Item6 = "Unload. Point";
                Item7 = "Quantity";
                Item8 = "Delivery Note";
                Item9 = "Num. Conta";
                Item10 = "P.O";
                Item11 = "Track. Number";
            }
            else
            {
                Item1 = Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\ShippingSchedule", "Item1DHL" + App.loggedUser.Username, "Null")?.ToString() ?? "Null";
                Item2 = Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\ShippingSchedule", "Item2DHL" + App.loggedUser.Username, "Null")?.ToString() ?? "Null";
                Item3 = Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\ShippingSchedule", "Item3DHL" + App.loggedUser.Username, "Null")?.ToString() ?? "Null";
                Item4 = Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\ShippingSchedule", "Item4DHL" + App.loggedUser.Username, "Null")?.ToString() ?? "Null";
                Item5 = Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\ShippingSchedule", "Item5DHL" + App.loggedUser.Username, "Null")?.ToString() ?? "Null";
                Item6 = Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\ShippingSchedule", "Item6DHL" + App.loggedUser.Username, "Null")?.ToString() ?? "Null";
                Item7 = Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\ShippingSchedule", "Item7DHL" + App.loggedUser.Username, "Null")?.ToString() ?? "Null";
                Item8 = Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\ShippingSchedule", "Item8DHL" + App.loggedUser.Username, "Null")?.ToString() ?? "Null";
                Item9 = Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\ShippingSchedule", "Item9DHL" + App.loggedUser.Username, "Null")?.ToString() ?? "Null";
                Item10 = Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\ShippingSchedule", "Item10DHL" + App.loggedUser.Username, "Null")?.ToString() ?? "Null";
                Item11 = Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\ShippingSchedule", "Item11DHL" + App.loggedUser.Username, "Null")?.ToString() ?? "Null";
            }
        }
        private void SavedCustomLayout()
        {
            System.Windows.Window? owner = Application.Current.Windows.OfType<System.Windows.Window>().FirstOrDefault(w => w.DataContext == this);
            if (owner == null) { return; }

            try
            {
                Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\ShippingSchedule", "HasLayoutDHL" + App.loggedUser.Username, "true");
                Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\ShippingSchedule", "Item1DHL" + App.loggedUser.Username, Item1);
                Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\ShippingSchedule", "Item2DHL" + App.loggedUser.Username, Item2);
                Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\ShippingSchedule", "Item3DHL" + App.loggedUser.Username, Item3);
                Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\ShippingSchedule", "Item4DHL" + App.loggedUser.Username, Item4);
                Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\ShippingSchedule", "Item5DHL" + App.loggedUser.Username, Item5);
                Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\ShippingSchedule", "Item6DHL" + App.loggedUser.Username, Item6);
                Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\ShippingSchedule", "Item7DHL" + App.loggedUser.Username, Item7);
                Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\ShippingSchedule", "Item8DHL" + App.loggedUser.Username, Item8);
                Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\ShippingSchedule", "Item9DHL" + App.loggedUser.Username, Item9);
                Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\ShippingSchedule", "Item10DHL" + App.loggedUser.Username, Item10);
                Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\ShippingSchedule", "Item11DHL" + App.loggedUser.Username, Item11);

                CustomAlertWindow alert = new CustomAlertWindow("Sucesso", "Layout guardado com sucesso!", this);
                alert.Owner = owner;
                alert.ShowDialog();
            }
            catch
            {
                CustomAlertWindow alert = new CustomAlertWindow("Erro", "Erro a salvar layout", this);
                alert.Owner = owner;
                alert.ShowDialog();
                return;
            };
        }
        private ExcelPart part = new ExcelPart();
        private int counter = 0;
        private string cacheShipTo = "";

        private void Import()
        {
            if (ExcelImportText.IsNullOrEmpty()) { return; }
            char[] delimiterChars = { '\t', '\r' };
            string content = ExcelImportText;
            string[] splitContent = content.Split(delimiterChars);
            int fieldIndex = -1;

            foreach (string field in splitContent)
            {
                if (field == "\n") { continue; }
                counter++;
                fieldIndex++;
                switch (counter)
                {
                    case 1:
                        if (Item1 == "Null")
                        {
                            part.ItemVisibility1 = Visibility.Collapsed;
                            var res = SetPartInCaseOfNullPosition(field);
                            break;
                        }
                        part.Item1 = field.Replace("\n", "");
                        if (Item1 == "ShipTo")
                        {
                            if (part.Item1.IsNullOrEmpty()) { part.Item1 = cacheShipTo; }
                            else { cacheShipTo = part.Item1; }
                        }
                        if (Item2 == "Null" && Item3 == "Null" && Item4 == "Null" && Item5 == "Null" && Item6 == "Null" && Item7 == "Null" && Item8 == "Null" && Item9 == "Null" && Item10 == "Null" && Item11 == "Null")
                        {
                            part.ItemVisibility2 = Visibility.Collapsed;
                            var res = SetPartInCaseOfNullPosition(field);
                            break;
                        }
                        break;
                    case 2:
                        if (Item2 == "Null")
                        {
                            part.ItemVisibility2 = Visibility.Collapsed;
                            var res = SetPartInCaseOfNullPosition(field);
                            break;
                        }
                        part.Item2 = field.Replace("\n", "");
                        if (Item2 == "ShipTo")
                        {
                            if (part.Item1.IsNullOrEmpty()) { part.Item2 = cacheShipTo; }
                            else { cacheShipTo = part.Item2; }
                        }

                        if (Item3 == "Null" && Item4 == "Null" && Item5 == "Null" && Item6 == "Null" && Item7 == "Null" && Item8 == "Null" && Item9 == "Null" && Item10 == "Null" && Item11 == "Null")
                        {
                            part.ItemVisibility3 = Visibility.Collapsed;
                            var res = SetPartInCaseOfNullPosition(field);
                            break;
                        }
                        break;
                    case 3:
                        if (Item3 == "Null")
                        {
                            part.ItemVisibility3 = Visibility.Collapsed;
                            var res = SetPartInCaseOfNullPosition(field);
                            break;
                        }
                        part.Item3 = field.Replace("\n", "");
                        if (Item3 == "ShipTo")
                        {
                            if (part.Item1.IsNullOrEmpty()) { part.Item3 = cacheShipTo; }
                            else { cacheShipTo = part.Item3; }
                        }
                        if (Item4 == "Null" && Item5 == "Null" && Item6 == "Null" && Item7 == "Null" && Item8 == "Null" && Item9 == "Null" && Item10 == "Null" && Item11 == "Null")
                        {
                            part.ItemVisibility4 = Visibility.Collapsed;
                            var res = SetPartInCaseOfNullPosition(field);
                            break;
                        }
                        break;
                    case 4:
                        if (Item4 == "Null")
                        {
                            part.ItemVisibility4 = Visibility.Collapsed;
                            var res = SetPartInCaseOfNullPosition(field);
                            break;
                        }
                        part.Item4 = field.Replace("\n", "");
                        if (Item4 == "ShipTo")
                        {
                            if (part.Item1.IsNullOrEmpty()) { part.Item4 = cacheShipTo; }
                            else { cacheShipTo = part.Item4; }
                        }
                        if (Item5 == "Null" && Item6 == "Null" && Item7 == "Null" && Item8 == "Null" && Item9 == "Null" && Item10 == "Null" && Item11 == "Null")
                        {
                            part.ItemVisibility5 = Visibility.Collapsed;
                            var res = SetPartInCaseOfNullPosition(field);
                            break;
                        }
                        break;
                    case 5:
                        if (Item5 == "Null")
                        {
                            part.ItemVisibility5 = Visibility.Collapsed;
                            var res = SetPartInCaseOfNullPosition(field);
                            break;
                        }
                        part.Item5 = field.Replace("\n", "");
                        if (Item5 == "ShipTo")
                        {
                            if (part.Item1.IsNullOrEmpty()) { part.Item5 = cacheShipTo; }
                            else { cacheShipTo = part.Item5; }
                        }
                        if (Item6 == "Null" && Item7 == "Null" && Item8 == "Null" && Item9 == "Null" && Item10 == "Null" && Item11 == "Null")
                        {
                            part.ItemVisibility6 = Visibility.Collapsed;
                            var res = SetPartInCaseOfNullPosition(field);
                            break;
                        }
                        break;
                    case 6:
                        if (Item6 == "Null")
                        {
                            part.ItemVisibility6 = Visibility.Collapsed;
                            var res = SetPartInCaseOfNullPosition(field);
                            break;
                        }
                        part.Item6 = field.Replace("\n", "");
                        if (Item6 == "ShipTo")
                        {
                            if (part.Item1.IsNullOrEmpty()) { part.Item6 = cacheShipTo; }
                            else { cacheShipTo = part.Item6; }
                        }
                        if (Item7 == "Null" && Item8 == "Null" && Item9 == "Null" && Item10 == "Null" && Item11 == "Null")
                        {
                            part.ItemVisibility7 = Visibility.Collapsed;
                            var res = SetPartInCaseOfNullPosition(field);
                            break;
                        }
                        break;
                    case 7:
                        if (Item7 == "Null")
                        {
                            part.ItemVisibility7 = Visibility.Collapsed;
                            var res = SetPartInCaseOfNullPosition(field);
                            break;
                        }
                        part.Item7 = field.Replace("\n", "");
                        if (Item7 == "ShipTo")
                        {
                            if (part.Item1.IsNullOrEmpty()) { part.Item7 = cacheShipTo; }
                            else { cacheShipTo = part.Item7; }
                        }
                        if (Item8 == "Null" && Item9 == "Null"  && Item10 == "Null" && Item11 == "Null")
                        {
                            part.ItemVisibility8 = Visibility.Collapsed;
                            var res = SetPartInCaseOfNullPosition(field);
                            break;
                        }
                        break;
                    case 8:
                        if (Item8 == "Null")
                        {
                            part.ItemVisibility8 = Visibility.Collapsed;
                            var res = SetPartInCaseOfNullPosition(field);
                            break;
                        }
                        part.Item8 = field.Replace("\n", "");
                        if (Item8 == "ShipTo")
                        {
                            if (part.Item1.IsNullOrEmpty()) { part.Item8 = cacheShipTo; }
                            else { cacheShipTo = part.Item8; }
                        }
                        if (Item9 == "Null" && Item10 == "Null" && Item11 == "Null")
                        {
                            part.ItemVisibility9 = Visibility.Collapsed;
                            var res = SetPartInCaseOfNullPosition(field);
                            break;
                        }
                        break;
                    case 9:
                        if (Item9 == "Null")
                        {
                            part.ItemVisibility9 = Visibility.Collapsed;
                            var res = SetPartInCaseOfNullPosition(field);
                            break;
                        }
                        part.Item9 = field.Replace("\n", "");
                        if (Item9 == "ShipTo")
                        {
                            if (part.Item1.IsNullOrEmpty()) { part.Item9 = cacheShipTo; }
                            else { cacheShipTo = part.Item9; }
                        }
                        if (Item10 == "Null" && Item11 == "Null")
                        {
                            part.ItemVisibility10 = Visibility.Collapsed;
                            var res = SetPartInCaseOfNullPosition(field);
                            break;
                        }
                        break;
                    case 10:
                        if (Item10 == "Null")
                        {
                            part.ItemVisibility10 = Visibility.Collapsed;
                            var res = SetPartInCaseOfNullPosition(field);
                            break;
                        }
                        part.Item10 = field.Replace("\n", "");
                        if (Item10 == "ShipTo")
                        {
                            if (part.Item1.IsNullOrEmpty()) { part.Item10 = cacheShipTo; }
                            else { cacheShipTo = part.Item10; }
                        }
                        if (Item11 == "Null")
                        {
                            part.ItemVisibility11 = Visibility.Collapsed;
                            Parts.Add(part);
                            part = new ExcelPart();
                            counter = 0;
                            break;
                        }
                        break;
                    case 11:
                        if (Item11 == "Null")
                        {
                            part.ItemVisibility11 = Visibility.Collapsed;
                            Parts.Add(part);
                            part = new ExcelPart();
                            counter = 0;
                            break;
                        }
                        part.Item11 = field.Replace("\n", "");
                        if (Item11 == "ShipTo")
                        {
                            if (part.Item1.IsNullOrEmpty()) { part.Item11 = cacheShipTo; }
                            else { cacheShipTo = part.Item11; }
                        }
                        Parts.Add(part);
                        part = new ExcelPart();
                        counter = 0;
                        break;
                }
            }
            ExcelImportText = "";
            part = new ExcelPart();
            counter = 0;
            cacheShipTo = "";
        }
        private bool SetPartInCaseOfNullPosition(string field)
        {
            counter++;
            switch (counter)
            {
                case 1:
                    if (Item1 == "Null")
                    {
                        part.ItemVisibility1 = Visibility.Collapsed;
                        var res = SetPartInCaseOfNullPosition(field);
                        break;
                    }
                    part.Item1 = field.Replace("\n", "");
                    if (Item1 == "ShipTo")
                    {
                        if (part.Item1.IsNullOrEmpty()) { part.Item1 = cacheShipTo; }
                        else { cacheShipTo = part.Item1; }
                    }
                    if (Item2 == "Null" && Item3 == "Null" && Item4 == "Null" && Item5 == "Null" && Item6 == "Null" && Item7 == "Null" && Item8 == "Null" && Item9 == "Null" && Item10 == "Null" && Item11 == "Null")
                    {
                        part.ItemVisibility2 = Visibility.Collapsed;
                        var res = SetPartInCaseOfNullPosition(field);
                        break;
                    }
                    break;
                case 2:
                    if (Item2 == "Null")
                    {
                        part.ItemVisibility2 = Visibility.Collapsed;
                        var res = SetPartInCaseOfNullPosition(field);
                        break;
                    }
                    part.Item2 = field.Replace("\n", "");
                    if (Item2 == "ShipTo")
                    {
                        if (part.Item1.IsNullOrEmpty()) { part.Item2 = cacheShipTo; }
                        else { cacheShipTo = part.Item2; }
                    }

                    if (Item3 == "Null" && Item4 == "Null" && Item5 == "Null" && Item6 == "Null" && Item7 == "Null" && Item8 == "Null" && Item9 == "Null" && Item10 == "Null" && Item11 == "Null")
                    {
                        part.ItemVisibility3 = Visibility.Collapsed;
                        var res = SetPartInCaseOfNullPosition(field);
                        break;
                    }
                    break;
                case 3:
                    if (Item3 == "Null")
                    {
                        part.ItemVisibility3 = Visibility.Collapsed;
                        var res = SetPartInCaseOfNullPosition(field);
                        break;
                    }
                    part.Item3 = field.Replace("\n", "");
                    if (Item3 == "ShipTo")
                    {
                        if (part.Item1.IsNullOrEmpty()) { part.Item3 = cacheShipTo; }
                        else { cacheShipTo = part.Item3; }
                    }
                    if (Item4 == "Null" && Item5 == "Null" && Item6 == "Null" && Item7 == "Null" && Item8 == "Null" && Item9 == "Null" && Item10 == "Null" && Item11 == "Null")
                    {
                        part.ItemVisibility4 = Visibility.Collapsed;
                        var res = SetPartInCaseOfNullPosition(field);
                        break;
                    }
                    break;
                case 4:
                    if (Item4 == "Null")
                    {
                        part.ItemVisibility4 = Visibility.Collapsed;
                        var res = SetPartInCaseOfNullPosition(field);
                        break;
                    }
                    part.Item4 = field.Replace("\n", "");
                    if (Item4 == "ShipTo")
                    {
                        if (part.Item1.IsNullOrEmpty()) { part.Item4 = cacheShipTo; }
                        else { cacheShipTo = part.Item4; }
                    }
                    if (Item5 == "Null" && Item6 == "Null" && Item7 == "Null" && Item8 == "Null" && Item9 == "Null" && Item10 == "Null" && Item11 == "Null")
                    {
                        part.ItemVisibility5 = Visibility.Collapsed;
                        var res = SetPartInCaseOfNullPosition(field);
                        break;
                    }
                    break;
                case 5:
                    if (Item5 == "Null")
                    {
                        part.ItemVisibility5 = Visibility.Collapsed;
                        var res = SetPartInCaseOfNullPosition(field);
                        break;
                    }
                    part.Item5 = field.Replace("\n", "");
                    if (Item5 == "ShipTo")
                    {
                        if (part.Item1.IsNullOrEmpty()) { part.Item5 = cacheShipTo; }
                        else { cacheShipTo = part.Item5; }
                    }
                    if (Item6 == "Null" && Item7 == "Null" && Item8 == "Null" && Item9 == "Null" && Item10 == "Null" && Item11 == "Null")
                    {
                        part.ItemVisibility6 = Visibility.Collapsed;
                        var res = SetPartInCaseOfNullPosition(field);
                        break;
                    }
                    break;
                case 6:
                    if (Item6 == "Null")
                    {
                        part.ItemVisibility6 = Visibility.Collapsed;
                        var res = SetPartInCaseOfNullPosition(field);
                        break;
                    }
                    part.Item6 = field.Replace("\n", "");
                    if (Item6 == "ShipTo")
                    {
                        if (part.Item1.IsNullOrEmpty()) { part.Item6 = cacheShipTo; }
                        else { cacheShipTo = part.Item6; }
                    }
                    if (Item7 == "Null" && Item8 == "Null" && Item9 == "Null" && Item10 == "Null" && Item11 == "Null")
                    {
                        part.ItemVisibility7 = Visibility.Collapsed;
                        var res = SetPartInCaseOfNullPosition(field);
                        break;
                    }
                    break;
                case 7:
                    if (Item7 == "Null")
                    {
                        part.ItemVisibility7 = Visibility.Collapsed;
                        var res = SetPartInCaseOfNullPosition(field);
                        break;
                    }
                    part.Item7 = field.Replace("\n", "");
                    if (Item7 == "ShipTo")
                    {
                        if (part.Item1.IsNullOrEmpty()) { part.Item7 = cacheShipTo; }
                        else { cacheShipTo = part.Item7; }
                    }
                    if (Item8 == "Null" && Item9 == "Null" && Item10 == "Null" && Item11 == "Null")
                    {
                        part.ItemVisibility8 = Visibility.Collapsed;
                        var res = SetPartInCaseOfNullPosition(field);
                        break;
                    }
                    break;
                case 8:
                    if (Item8 == "Null")
                    {
                        part.ItemVisibility8 = Visibility.Collapsed;
                        var res = SetPartInCaseOfNullPosition(field);
                        break;
                    }
                    part.Item8 = field.Replace("\n", "");
                    if (Item8 == "ShipTo")
                    {
                        if (part.Item1.IsNullOrEmpty()) { part.Item8 = cacheShipTo; }
                        else { cacheShipTo = part.Item8; }
                    }
                    if (Item9 == "Null" && Item10 == "Null" && Item11 == "Null")
                    {
                        part.ItemVisibility9 = Visibility.Collapsed;
                        var res = SetPartInCaseOfNullPosition(field);
                        break;
                    }
                    break;
                case 9:
                    if (Item9 == "Null")
                    {
                        part.ItemVisibility9 = Visibility.Collapsed;
                        var res = SetPartInCaseOfNullPosition(field);
                        break;
                    }
                    part.Item9 = field.Replace("\n", "");
                    if (Item9 == "ShipTo")
                    {
                        if (part.Item1.IsNullOrEmpty()) { part.Item9 = cacheShipTo; }
                        else { cacheShipTo = part.Item9; }
                    }
                    if (Item10 == "Null" && Item11 == "Null")
                    {
                        part.ItemVisibility10 = Visibility.Collapsed;
                        var res = SetPartInCaseOfNullPosition(field);
                        break;
                    }
                    break;
                case 10:
                    if (Item10 == "Null")
                    {
                        part.ItemVisibility10 = Visibility.Collapsed;
                        var res = SetPartInCaseOfNullPosition(field);
                        break;
                    }
                    part.Item10 = field.Replace("\n", "");
                    if (Item10 == "ShipTo")
                    {
                        if (part.Item1.IsNullOrEmpty()) { part.Item10 = cacheShipTo; }
                        else { cacheShipTo = part.Item10; }
                    }
                    if (Item11 == "Null")
                    {
                        part.ItemVisibility11 = Visibility.Collapsed;
                        Parts.Add(part);
                        part = new ExcelPart();
                        counter = 0;
                        break;
                    }
                    break;
                case 11:
                    if (Item11 == "Null")
                    {
                        part.ItemVisibility11 = Visibility.Collapsed;
                        Parts.Add(part);
                        part = new ExcelPart();
                        counter = 0;
                        break;
                    }
                    part.Item11 = field.Replace("\n", "");
                    if (Item11 == "ShipTo")
                    {
                        if (part.Item1.IsNullOrEmpty()) { part.Item11 = cacheShipTo; }
                        else { cacheShipTo = part.Item11; }
                    }
                    Parts.Add(part);
                    part = new ExcelPart();
                    counter = 0;
                    break;
            }
            return false;
        }
    }
}
