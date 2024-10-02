using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ShippingScheduleMVVM.Views.Modals
{
    /// <summary>
    /// Interaction logic for PartComment.xaml
    /// </summary>
    public partial class PartComment : Window
    {
        public bool WindowClosing = false;
        public PartComment()
        {
            InitializeComponent();
            this.ContentRendered += OnContentRendered;
            this.Deactivated += OnDeactivated;
            this.Closing += OnClosingWindow;
        }
        private void OnClosingWindow(object? sender, CancelEventArgs e)
        {
            WindowClosing = true;
        }
        private void OnContentRendered(object? sender, EventArgs e)
        {
            Activate();
        }
        bool flag = false;
        private void OnDeactivated(object? sender, EventArgs e)
        {
            if (!WindowClosing && flag)
            {
                this.Owner = null;
                this.Close();
                WindowClosing = true;
                return;
            }

            flag = true;
        }
    }
}
