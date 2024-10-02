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
    public class ChatMessage : ViewModelBase
    {
        public int Id { get; set; } = -1;

        private HorizontalAlignment _horizontalAlignment = HorizontalAlignment.Left;
        public HorizontalAlignment HorizontalAlign
        {
            get => _horizontalAlignment;
            set
            {
                _horizontalAlignment = value;
                OnPropertyChanged(nameof(HorizontalAlign));
            }
        }
        private SolidColorBrush _colorBrush = new SolidColorBrush(Color.FromRgb(199, 216, 255));
        public SolidColorBrush ColorBrush 
        {
            get => _colorBrush;
            set
            {
                _colorBrush = value;
                OnPropertyChanged(nameof(ColorBrush));
            }
        }
        private string _messageContent = string.Empty;
        public string MessageContent
        {
            get { return _messageContent; }
            set
            {
                if (_messageContent != value)
                {
                    _messageContent = value;
                    OnPropertyChanged(nameof(MessageContent));
                }
            }
        }

        private string _user = string.Empty;
        public string User
        {
            get { return _user; }
            set
            {
                if (_user != value)
                {
                    _user = value;
                    OnPropertyChanged(nameof(User));
                }
            }
        }

        private string _date = string.Empty;
        public string Date
        {
            get { return _date; }
            set
            {
                if (_date != value)
                {
                    _date = value;
                    OnPropertyChanged(nameof(Date));
                }
            }
        }

        private Visibility _userVisibility = Visibility.Collapsed;
        public Visibility UserVisibility
        {
            get => _userVisibility;
            set 
            {
                _userVisibility = value;
                OnPropertyChanged(nameof(UserVisibility));
            }
        }


        public ChatMessage()
        {

        }

    }
}
