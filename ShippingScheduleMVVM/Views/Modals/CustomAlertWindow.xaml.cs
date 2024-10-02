using ShippingScheduleMVVM.ViewModels;
using ShippingScheduleMVVM.ViewModels.Modals;
using System.Windows;
using System.Windows.Input;

namespace ShippingScheduleMVVM.Views.Modals
{
    /// <summary>
    /// Interaction logic for CustomAlertWindow.xaml
    /// </summary>
    public partial class CustomAlertWindow : Window
    {
        public CustomAlertWindow(string title, string content,ViewModelBase parentViewodel)
        {
            InitializeComponent();
            CustomAlertWindowViewModel viewmodel = new(title, content, parentViewodel);
            this.DataContext = viewmodel;
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
        }
    }
}
