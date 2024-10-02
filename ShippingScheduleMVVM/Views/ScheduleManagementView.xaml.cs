using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ShippingScheduleMVVM.Views
{
    /// <summary>
    /// Interaction logic for ScheduleManagementView.xaml
    /// </summary>
    public partial class ScheduleManagementView : UserControl
    {
        public ScheduleManagementView()
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

            // On Scroll close all the copy and paste windows
            System.Windows.Window? window = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.Title == "ThisCopyPasteWindow");
            window?.Close();
            e.Handled = true;
        }

        bool _isRectrated = true;
        private void ExtendRetractTable(object sender, MouseButtonEventArgs e)
        {
            if (_isRectrated)
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri("/Resources/Images/view.png", UriKind.RelativeOrAbsolute);
                bitmapImage.EndInit();
                ExtRetImg.Margin = new Thickness(0, 0, 0, 0);
                ExtRetImg.Source = bitmapImage;

                ScheduleBackLayer.SetValue(Grid.RowSpanProperty, 5);
                ScheduleFrontLayer.SetValue(Grid.RowSpanProperty, 5);
                ScheduleListview.Height = 325;

                ScheduleFrontLayerCorner.CornerRadius = new CornerRadius(0);
                ScheduleFrontLayerCornerFill.Visibility = Visibility.Collapsed;

                ExpandIcon.Margin = new Thickness(-8, 0, 0, -8);

                WeeklyRecordsStatisticsTable.Visibility = Visibility.Visible;
                _isRectrated = false;
            }
            else
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri("/Resources/Images/hidden.png", UriKind.RelativeOrAbsolute);
                bitmapImage.EndInit();
                ExtRetImg.Margin = new Thickness(0, 0, 0, 0);
                ExtRetImg.Source = bitmapImage;

                ScheduleBackLayer.SetValue(Grid.RowSpanProperty, 7);
                ScheduleFrontLayer.SetValue(Grid.RowSpanProperty, 7);
                WeeklyRecordsStatisticsTable.Visibility = Visibility.Collapsed;
                ScheduleListview.Height = 400;

                ExpandIcon.Margin = new Thickness(0);

                ScheduleFrontLayerCorner.CornerRadius = new CornerRadius(0,0,25,25);
                ScheduleFrontLayerCornerFill.Visibility = Visibility.Visible;

                _isRectrated = true;
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(sender is TextBox tb)
            {
                if(tb.Text.Length == 0)
                { 
                    SearchIdPlaceholder.Visibility = Visibility.Visible;
                    return;
                }

                SearchIdPlaceholder.Visibility = Visibility.Collapsed;
            } 
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb)
            {
                if (tb.Text.Length > 3)
                {
                    tb.Text = "";
                }
            }
        }

        private void ListView_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (GetDescendantByType((ListView)sender, typeof(ScrollViewer)) is ScrollViewer scrollViewer)
            {
                // Change the scrolling sensitivity by adjusting the multiplier
                int scrollLines = - e.Delta / 15;
                scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset + scrollLines);
                e.Handled = true;
            }
        }

        private bool _chatExpanded = false;
        private void ChatBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_chatExpanded)
            {
                ChangeChatSizePath.Data = Geometry.Parse("M21,21H16v3h5.546A2.457,2.457,0,0,0,24,21.545V16H21Z M0,2.455V8H3V3H8V0H2.454A2.457,2.457,0,0,0,0,2.455Z M3,16H0v5.545A2.457,2.457,0,0,0,2.454,24H8V21H3Z M21.546,0H16V3h5V8h3V2.455A2.457,2.457,0,0,0,21.546,0Z");

                _chatExpanded = false;
                ForumChatBorder.Margin = new Thickness(0, 0, 0, 0);
                return;
            }
            ChangeChatSizePath.Data = Geometry.Parse("M5.5,5.5H0v3H6A2.5,2.5,0,0,0,8.5,6V0h-3Z M15.5,18v6h3V18.5H24v-3H18A2.5,2.5,0,0,0,15.5,18Z M18.5,5.5V0h-3V6A2.5,2.5,0,0,0,18,8.5h6v-3Z M6,15.5H0v3H5.5V24h3V18A2.5,2.5,0,0,0,6,15.5Z");
           
            _chatExpanded = true;
            ForumChatBorder.Margin = new Thickness(-210, -113, 0, 0);
        }
    }
    
}
