using System.Windows;

namespace ShippingScheduleMVVM.Models
{
    public class MyDependencyObject<T> : DependencyObject
    {
        public DependencyProperty Property { get; set; }
        public T GetProperty()
        {
            return (T)GetValue(Property);
        }

        public void SetProperty(T value)
        {
            SetValue(Property, value);
        }
        public MyDependencyObject(DependencyProperty property)
        {
            Property = property;
        }
    }
}
