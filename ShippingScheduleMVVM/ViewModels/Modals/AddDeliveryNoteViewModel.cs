
using Microsoft.IdentityModel.Tokens;
using ShippingScheduleMVVM.Commands;
using ShippingScheduleMVVM.Models;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Windows;
using System.Windows.Input;

namespace ShippingScheduleMVVM.ViewModels.Modals
{
    public class AddDeliveryNoteViewModel : ViewModelBase
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

        private string _deliveryNote { get; set; } = string.Empty;
        public string DeliveryNote
        {
            get { return _deliveryNote; }
            set
            {
                if (_deliveryNote != value)
                {
                    _deliveryNote = value;
                    OnPropertyChanged("DeliveryNote");
                }
            }
        }

        public ICommand ConfirmChanges { get; }
        public ICommand CloseWindow { get; }

        public static List<string> SplitAndRemoveEmptySpaces(string input)
        {
            string[] parts = input.Split(new char[] { ',', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> resultList = new List<string>();

            foreach (string part in parts)
            {
                string trimmedPart = part.Trim();
                resultList.Add(trimmedPart);
            }

            return resultList;
        }

        public AddDeliveryNoteViewModel(ViewModelBase originViewmodel) 
        {
            // Initialize Commands
            CloseWindow = new CloseWindowCommand(false);

            ConfirmChanges = new RelayCommand(() =>
            {
                if(originViewmodel is RecordWindowViewModel vm)
                {
                    List<string> dns = SplitAndRemoveEmptySpaces(DeliveryNote);

                    foreach(string dn in dns)
                    {
                        if (!vm.DeliveryNotes.Any(x => x.Number.Substring(1) == dn))
                        {
                            if (dn.IsNullOrEmpty() || dn == " ") {  }
                            else { vm.DeliveryNotes.Add(new DeliveryNote(dn)); }
                        } 
                    } 

                    System.Windows.Window? window = Application.Current.Windows.OfType<System.Windows.Window>().FirstOrDefault(w => w.DataContext == this);
                    if (window == null) { return; }
                    window.Close();
                }  
            });

            System.Windows.Window? owner = Application.Current.Windows.OfType<System.Windows.Window>().FirstOrDefault(w => w.DataContext == originViewmodel);
       
            WindowSettings = new("ThisAddDeliveryNoteWindow");
            WindowSettings.Initialize(1.8, 320, 180, false, owner.Title);
            
            // Center the window acording to the windows owner position
            WindowSettings.Left = owner.Left + (owner.ActualWidth - WindowSettings.Width) / 2;
            WindowSettings.Top = owner.Top + (owner.ActualHeight - WindowSettings.Height) / 2;
        }
    }
}
