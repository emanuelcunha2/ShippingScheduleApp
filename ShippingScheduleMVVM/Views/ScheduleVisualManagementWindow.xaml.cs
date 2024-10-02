using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
namespace ShippingScheduleMVVM.Views
{
    /// <summary>
    /// Interaction logic for ScheduleVisualManagementWindow.xaml
    /// </summary>
    public partial class ScheduleVisualManagementWindow : UserControl
    {
        public ScheduleVisualManagementWindow()
        {
            InitializeComponent();
            if (GetDescendantByType(ScheduleListview, typeof(ScrollViewer)) is ScrollViewer scrollViewer) scrollViewer.PreviewMouseWheel += ScrollViewer_PreviewMouseWheel;
        }
        private static DependencyObject? GetDescendantByType(DependencyObject element, Type type)
        {
            if (element == null) return null;

            if (element.GetType() == type) return element;

            DependencyObject? foundElement = null;
            if (element is FrameworkElement el)
            {
                el.ApplyTemplate();
            }

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
            {
                DependencyObject visual = VisualTreeHelper.GetChild(element, i) as DependencyObject;
                foundElement = GetDescendantByType(visual, type);
                if (foundElement != null)
                    break;
            }
            return foundElement;
        }
        public void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var scrollViewer = sender as ScrollViewer;
            if (e.Delta > 0)
            {
                scrollViewer?.LineUp();
            }
            else
            {
                scrollViewer?.LineDown();
            }

            System.Windows.Window? window = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.Title == "ThisCopyPasteWindow");
            window?.Close();
            e.Handled = true;
        }
    }
}
