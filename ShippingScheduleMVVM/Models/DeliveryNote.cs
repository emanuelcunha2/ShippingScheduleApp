using ShippingScheduleMVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingScheduleMVVM.Models
{
    public class DeliveryNote : ViewModelBase
    {
        private int _id = -1;
        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        private string _number = string.Empty;
        public string Number
        {
            get => _number;
            set
            {
                Designation = value;
                _number = "#" + value;
                OnPropertyChanged("Number");
            }
        }

        private string _designation = string.Empty;
        public string Designation
        {
            get => _designation;
            set
            {
                _designation = value;
                OnPropertyChanged("Designation");
            }
        }


        public DeliveryNote(string number)
        {
            Number = number;
        }

    }
}
