using ShippingScheduleMVVM.Helpers; 
using ShippingScheduleMVVM.ViewModels;
using System; 
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace ShippingScheduleMVVM
{ 
    public partial class MainWindow : Window
    {
        // Declare a DispatcherTimer object
        private DispatcherTimer timer;
        private DispatcherTimer timerResize;
        MainWindowViewModel? mainWindowViewModel = null;
        ScheduleManagementViewModel? scheduleViewModel = null;
        double relativeMousePos = 0;

        public MainWindow()
        {
            InitializeComponent();

            // Create a new DispatcherTimer object
            timer = new DispatcherTimer();
            timerResize = new DispatcherTimer();
            // Set the interval to 5 seconds
            timer.Interval = TimeSpan.FromMilliseconds(250); 

            // Set the tick event handler
            timer.Tick += timer_Tick;
            timerResize.Tick += timer_Tick_Resize;

            // Start the timer
            timer.Start(); 

            //private void timer_Tick(object? sender, EventArgs e)
            // Subscribe to the DataContextChanged event
            this.DataContextChanged += OnDataContextChanged; 
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            mainWindowViewModel = (MainWindowViewModel)this.DataContext;

            if (mainWindowViewModel.CurrentViewModel != null)
            {
                scheduleViewModel = (ScheduleManagementViewModel)mainWindowViewModel.CurrentViewModel;
            }
        }

        private void timer_Tick_Resize(object? sender, EventArgs e)
        {
            System.Drawing.Point currentPoint = System.Windows.Forms.Cursor.Position;

            double distanceItHasToGetMouseToRight = currentPoint.X - _xPositionOfRight;

            distanceItHasToGetMouseToRight = distanceItHasToGetMouseToRight / 1.25;
            double currentWidth = _startingWidth; 

            double resultWidth = currentWidth + distanceItHasToGetMouseToRight;
      
            double resultingScaleX = resultWidth * _startingScaleX / currentWidth; 

            mainWindowViewModel?.WindowSettings.Recalculate(resultingScaleX);
        }

        private void ScaleEveryWindow(double scale)
        {
            foreach (Window wind in Application.Current.Windows)
            {
                if (wind.DataContext is RecordWindowViewModel viewmodel)
                {
                    viewmodel.WindowSettings.Recalculate(scale);
                }
            }
        }

        private void RecalculateForEveryWindow()
        {
            foreach (Window wind in Application.Current.Windows)
            {
                if (wind.DataContext is RecordWindowViewModel viewmodel)
                {
                    viewmodel.WindowSettings.RecalculateWindow();
                }
            }
        }
         
        // The tick event handler method
        private async void timer_Tick(object? sender, EventArgs e)
        {   
            if(scheduleViewModel == null) { return; }
            if (!scheduleViewModel.IsDraggingRecord) { return; }

            await Task.Delay(1000);
            if (scheduleViewModel.IsDraggingRecord)
            {
                if (scheduleViewModel.ScrollPosition > -1)
                {
                    if (relativeMousePos < 25)
                    {
                        scheduleViewModel.ScrollPosition--;
                    } 
                }
                if (scheduleViewModel.ScrollPosition < scheduleViewModel.ScheduleRows.Count - 5)
                { 

                    if (relativeMousePos > 90)
                    {
                        scheduleViewModel.ScrollPosition++;
                    }
                }
            }
        }
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
            if (e.ChangedButton == MouseButton.Right)
            {
                var screen = System.Windows.Forms.Screen.FromHandle(new System.Windows.Interop.WindowInteropHelper(Application.Current.MainWindow).Handle);
                MessageBox.Show(screen.Bounds.Width.ToString(), screen.Bounds.Height.ToString());
            }
        }
        private void MainGrid_DragOver(object sender, DragEventArgs e)
        {
            if (scheduleViewModel != null && scheduleViewModel.IsDraggingRecord)
            {
                Point mousePos = e.GetPosition(this); // Get mouse position relative to the window
                double windowHeight = this.ActualHeight; // Get the actual height of the window

                // Calculate the position of the mouse relative to the height of the window
                relativeMousePos = (mousePos.Y / windowHeight) * 100;
                //scheduleViewModel.TestValue = (int)relativeMousePos;
            }
            e.Handled= true;
        }
        private void ThisMainWindow_LocationChanged(object sender, EventArgs e)
        {
            return;

            if(this.WindowState != WindowState.Maximized)
            {
                System.Windows.Window? window = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.Title == "ThisRecordWindow");

                if (window == null) 
                {
                    window = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.Title == "ThisRecord2Window");

                    if (window == null) { return; }

                    // Record2WindowViewModel vm2 = (Record2WindowViewModel)window.DataContext;

                    // vm2.WindowSettings.Left = this.Left + (this.ActualWidth - vm2.WindowSettings.Width) / 2;
                    // vm2.WindowSettings.Top = this.Top + (this.ActualHeight - vm2.WindowSettings.Height) / 2;
                }
                else
                {
                    RecordWindowViewModel vm = (RecordWindowViewModel)window.DataContext;

                    vm.WindowSettings.Left = this.Left + (this.ActualWidth - vm.WindowSettings.Width) / 2;
                    vm.WindowSettings.Top = this.Top + (this.ActualHeight - vm.WindowSettings.Height) / 2;
                }
            }
        }
        private System.Drawing.Point startPoint;
        private double _startingWidth = 0;
        private double _startingheight = 0;
        private double _startingScaleX = 0;
        private double _startingScaleY = 0;
        private double _xPositionOfRight = 0;
        private double _xPositionOfBottom = 0;

        private bool _isResizing = false;
        private void ResizeSpace_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Forms.Screen? screen = WindowScreen.FindScreen("ThisMainWindow");

            startPoint = System.Windows.Forms.Cursor.Position;
            _startingScaleX = mainWindowViewModel.WindowSettings.ScaleX;
            _startingScaleY = mainWindowViewModel.WindowSettings.ScaleY;

            _startingWidth = mainWindowViewModel.WindowSettings.Width;
            _startingheight = mainWindowViewModel.WindowSettings.Height;

            _xPositionOfRight = startPoint.X; 
            
            _xPositionOfBottom = this.Top + mainWindowViewModel.WindowSettings.Height;
            
            timerResize.Start();
            _isResizing = true;
            DragDrop.DoDragDrop(ResizeSpace, new object(), DragDropEffects.None);
            timerResize.Stop();
            _isResizing = false;
            mainWindowViewModel.WindowSettings.RecalculateWindow();
        }
    

        protected override void OnGiveFeedback(System.Windows.GiveFeedbackEventArgs e)
        {
            if (_isResizing) { Mouse.SetCursor(Cursors.SizeWE); e.Handled = true; }
            else { Mouse.SetCursor(Cursors.SizeAll); e.Handled = true; }
        }
    }
}
