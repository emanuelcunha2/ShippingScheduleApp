using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ShippingScheduleMVVM.Behaviours
{
 

    public class ScrollViewerBehavior : Behavior<ListView>
    {
        public static readonly DependencyProperty VerticalOffsetProperty =
            DependencyProperty.Register("VerticalOffset", typeof(double), typeof(ScrollViewerBehavior), new PropertyMetadata(0.0, OnVerticalOffsetChanged));

        public static readonly DependencyProperty HorizontalOffsetProperty =
            DependencyProperty.Register("HorizontalOffset", typeof(double), typeof(ScrollViewerBehavior), new PropertyMetadata(0.0, OnHorizontalOffsetChanged));

        public double VerticalOffset
        {
            get { return (double)GetValue(VerticalOffsetProperty); }
            set { SetValue(VerticalOffsetProperty, value); }
        }

        public double HorizontalOffset
        {
            get { return (double)GetValue(HorizontalOffsetProperty); }
            set { SetValue(HorizontalOffsetProperty, value); }
        }

        private static void OnVerticalOffsetChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is ScrollViewerBehavior scrollViewerBehavior && scrollViewerBehavior.AssociatedObject != null)
            {
                var scrollViewer = FindScrollViewer(scrollViewerBehavior.AssociatedObject);
                if (scrollViewer != null)
                {
                    scrollViewer.ScrollToVerticalOffset((double)e.NewValue);
                }
            }
        }

        private static void OnHorizontalOffsetChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is ScrollViewerBehavior scrollViewerBehavior && scrollViewerBehavior.AssociatedObject != null)
            {
                var scrollViewer = FindScrollViewer(scrollViewerBehavior.AssociatedObject);
                if (scrollViewer != null)
                {
                    scrollViewer.ScrollToHorizontalOffset((double)e.NewValue);
                }
            }
        }

        private static ScrollViewer? FindScrollViewer(DependencyObject depObj)
        {
            if (depObj is ScrollViewer scrollViewer)
            {
                return scrollViewer;
            }

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                var child = VisualTreeHelper.GetChild(depObj, i);
                var result = FindScrollViewer(child);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }
    } 
}
