using System;
using System.Windows;

namespace ShippingScheduleMVVM.Converters
{
    public class EventParemeters
    {
        public EventArgs E { get; set; } = new EventArgs();
        public UIElement Sender { get; set; } = new UIElement();
        public EventParemeters(EventArgs e, UIElement sender)
        {
            E = e;
            Sender = sender;
        }
    }
}
