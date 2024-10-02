using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ShippingScheduleMVVM.Views
{
    /// <summary>
    /// Interaction logic for RecordWindow.xaml
    /// </summary>
    public partial class RecordWindow : Window
    {
        public RecordWindow()
        {
            InitializeComponent();
            if (GetDescendantByType(PartsListview, typeof(ScrollViewer)) is ScrollViewer scrollViewer) scrollViewer.PreviewMouseWheel += ScrollViewer_PreviewMouseWheel;

            this.Focus();
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

            System.Windows.Window? window = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.Title == "ThisPartCommentWindow");
            window?.Close();
            e.Handled = true;
        }

        private void ListView_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            // Get the current text box value
            string currentText = (sender as TextBox)?.Text ?? "";

            // Concatenate the current text with the new input
            string newText = currentText + e.Text;

            // If the resulting text is null or empty, replace it with "0"
            if (string.IsNullOrEmpty(newText))
            {
                newText = "0";
            }

            // Use the updated text for further validation
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(newText);
        }

        private bool _isRectrated = true;
        private void ExtendRetractTable(object sender, MouseButtonEventArgs e)
        {
            if (_isRectrated)
            {
                GridRecord.RowDefinitions[1].Height = new GridLength(0, GridUnitType.Star);
                GridRecord.RowDefinitions[2].Height = new GridLength(0, GridUnitType.Star);
                GridRecord.RowDefinitions[3].Height = new GridLength(0, GridUnitType.Star);
                GridRecord.RowDefinitions[4].Height = new GridLength(0, GridUnitType.Star);
                GridRecord.RowDefinitions[5].Height = new GridLength(0, GridUnitType.Star);
                GridRecord.RowDefinitions[6].Height = new GridLength(0, GridUnitType.Star);
                GridRecord.RowDefinitions[7].Height = new GridLength(0, GridUnitType.Star);
                GridRecord.RowDefinitions[8].Height = new GridLength(0, GridUnitType.Star);
                GridRecord.RowDefinitions[9].Height = new GridLength(130, GridUnitType.Star);

                GridParts.RowDefinitions[0].Height = new GridLength(8, GridUnitType.Star);
                GridParts.RowDefinitions[1].Height = new GridLength(100, GridUnitType.Star);

                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri("/Resources/Images/double_down_thk.png", UriKind.RelativeOrAbsolute);
                bitmapImage.EndInit();
                ExtRetImg.Margin = new Thickness(0,0,1,0);
                ExtRetImg.Source = bitmapImage;

                _isRectrated = false;
            }
            else
            { 
                GridRecord.RowDefinitions[1].Height = new GridLength(10, GridUnitType.Star);
                GridRecord.RowDefinitions[2].Height = new GridLength(10, GridUnitType.Star);
                GridRecord.RowDefinitions[3].Height = new GridLength(10, GridUnitType.Star);
                GridRecord.RowDefinitions[4].Height = new GridLength(10, GridUnitType.Star);
                GridRecord.RowDefinitions[5].Height = new GridLength(10, GridUnitType.Star);
                GridRecord.RowDefinitions[6].Height = new GridLength(10, GridUnitType.Star);
                GridRecord.RowDefinitions[7].Height = new GridLength(10, GridUnitType.Star);
                GridRecord.RowDefinitions[8].Height = new GridLength(10, GridUnitType.Star);
                GridRecord.RowDefinitions[9].Height = new GridLength(50, GridUnitType.Star);

                GridParts.RowDefinitions[0].Height = new GridLength(19, GridUnitType.Star);
                GridParts.RowDefinitions[1].Height = new GridLength(100, GridUnitType.Star);

                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri("/Resources/Images/double_up_thk.png", UriKind.RelativeOrAbsolute);
                bitmapImage.EndInit();
                ExtRetImg.Margin = new Thickness(0,0,1,3);
                ExtRetImg.Source = bitmapImage;

                _isRectrated = true;
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void MainGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            } 
        }

        private void Border_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if(sender is Border border) 
            {
                if (border.IsEnabled) { return; }
                border.Background = new SolidColorBrush(Color.FromRgb(235, 235, 235));    
            }
        }

    }
}
