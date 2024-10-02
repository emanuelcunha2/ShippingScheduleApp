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
    /// Interaction logic for CopyPasteWindow.xaml
    /// </summary>
    public partial class CopyPasteWindow : Window
    {
        public bool WindowClosing = false;
        public object? Parameter { get; set; }
        public CopyPasteWindow(object? @object)
        {
            InitializeComponent();
            this.ContentRendered += OnContentRendered;
            this.Deactivated += OnDeactivated;
            this.Closing += OnClosingWindow;
            Parameter = @object;
        }
        private void OnClosingWindow(object? sender, CancelEventArgs e)
        {
            WindowClosing = true;
        }
        private void OnContentRendered(object? sender, EventArgs e)
        { 
            Activate();
        }
        private int _flag = 0;
        private void OnDeactivated(object? sender, EventArgs e)
        {
            if(Parameter is Border && _flag == 0)
            {
                _flag++;
                return;
            }

            if (!WindowClosing)
            {
                this.Owner = null;
                this.Close();
                WindowClosing = true;
                return;
            }
        }
    }
}
