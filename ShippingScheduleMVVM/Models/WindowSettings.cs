using ShippingScheduleMVVM.Helpers;
using ShippingScheduleMVVM.ViewModels;
using System;
using System.Windows;
using System.Windows.Forms;

namespace ShippingScheduleMVVM.Models
{
    public class WindowSettings : ViewModelBase
    {
        private bool _isEnabled;
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                if (_isEnabled != value)
                {
                    _isEnabled = value;
                    OnPropertyChanged("IsEnabled");
                }
            }
        }
        private Visibility _visibility;
        public Visibility Visibility
        {
            get { return _visibility; }
            set
            {
                if (_visibility != value)
                {
                    _visibility = value;
                    OnPropertyChanged("Visibility");
                }
            }
        }

        private Visibility _ResizeButtonVisibility = Visibility.Visible;
        public Visibility ResizeButtonVisibility 
        {
            get { return _ResizeButtonVisibility; }
            set
            {
                if (_ResizeButtonVisibility != value)
                {
                    _ResizeButtonVisibility = value;
                    OnPropertyChanged("ResizeButtonVisibility");
                }
            }
        }

         

        private double _width;
        public double Width
        {
            get { return _width; }
            set
            {
                if (_width != value)
                {
                    _width = value;
                    OnPropertyChanged("Width");
                }
            }
        }
        private double _height;
        public double Height
        {
            get { return _height; }
            set
            {
                if (_height != value)
                {
                    _height = value;
                    OnPropertyChanged("Height");
                }
            }
        }
        private double _left;
        public double Left
        {
            get { return _left; }
            set
            {
                if (_left != value)
                {
                    _left = value;
                    OnPropertyChanged("Left");
                }
            }
        }
        private double _top;
        public double Top
        {
            get { return _top; }
            set
            {
                if (_top != value)
                {
                    _top = value;
                    OnPropertyChanged("Top");
                }
            }
        }

        private double _margin;
        public double Margin
        {
            get { return _margin; }
            set
            {
                if (_margin != value)
                {
                    _margin = value;
                    OnPropertyChanged("Margin");
                }
            }
        }

        private CornerRadius _roundness;
        public CornerRadius Roundness
        {
            get { return _roundness; }
            set
            {
                if (_roundness != value)
                {
                    _roundness = value;
                    OnPropertyChanged("Roundness");
                }
            }
        }

        private double _scaleX;
        public double ScaleX
        {
            get { return _scaleX; }
            set
            {
                if (_scaleX != value)
                {
                    _scaleX = value;
                    OnPropertyChanged("ScaleX");
                }
            }
        }
        private double _scaleY;
        public double ScaleY
        {
            get { return _scaleY; }
            set
            {
                if (_scaleY != value)
                {
                    _scaleY = value;
                    OnPropertyChanged("ScaleY");
                }
            }
        }
        private double _scaleSlider;
        public double ScaleSlider
        {
            get { return _scaleSlider; }
            set
            {
                if (_scaleSlider != value)
                {
                    _scaleSlider = value;

                    OnPropertyChanged("ScaleSlider");
                    if (value != 0.69)
                    {
                        Recalculate(OriginalScale - value / 1000);
                    }
                }
            }
        }

        private WindowState _state;
        public WindowState State
        {
            get { return _state; }
            set
            {
                if (_state != value)
                {
                    _state = value;
                    OnPropertyChanged("State");
                }
            }
        }
        private string? _name;
        public string Name
        {
            get { return _name ?? ""; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged("Name");
                }
            }
        }
        public double OriginalHeight { get; set; }
        public double OriginalWidth { get; set; }
        public double OriginalScale { get; set; }
        public double OriginalDpi { get; set; }

        public WindowSettings(string name)
        {
            Name = name;
        }

        public void Initialize(double scale, double width, double height, bool centerManual, string focusWindow)
        {
            IsEnabled = true;
            Visibility = Visibility.Visible;

            Screen? screen = WindowScreen.FindScreen(focusWindow);
            if (screen == null) { return; }

            Tuple<double, double> dpi = WindowScreen.GetDPI(focusWindow);

            double dpiX = dpi.Item1;
            double dpiY = dpi.Item2;


            double screenWidth = WindowScreen.FindPrimaryScreen().Bounds.Width / dpiX;
            double screenHeight = WindowScreen.FindPrimaryScreen().Bounds.Height  / dpiY;

            OriginalDpi = dpiX;
            OriginalHeight = height;
            OriginalWidth = width;

            scale = (scale * screenWidth) / 1920;

            OriginalScale = scale;

            width = width * scale;
            height = height * scale;
            double top = (screenHeight - height) / 2;
            double left = (screenWidth - width) / 2;

            ScaleX = scale;
            ScaleY = scale;
            Width = width;
            Height = height;

            if (centerManual)
            {
                Top = top;
                Left = left;
            }

            State = WindowState.Normal;
            Margin = 5;
            Roundness = new CornerRadius(18);

            ScaleSlider = 0.69;
        }

        public void RecalculateXY(double scaleX, double scaleY)
        {
            double width, height;

            height = OriginalHeight;
            width = OriginalWidth;

            Screen? screen = WindowScreen.FindScreen(Name);
            if (screen == null) { return; }
            Tuple<double, double> dpi = WindowScreen.GetDPI(Name);

            double dpiX = dpi.Item1;
            double dpiY = dpi.Item2;

            double screenWidth = WindowScreen.FindPrimaryScreen().Bounds.Width / dpiX;
            double screenHeight = WindowScreen.FindPrimaryScreen().Bounds.Height  / dpiY;

            scaleX = (scaleX * screenWidth) / 1920;
            scaleY = (scaleY * screenHeight) / 1080;

            width = width * scaleX;
            height = height * scaleY;

            Width = WindowScreen.FindPrimaryScreen().Bounds.Width / dpiX;
            Height = WindowScreen.FindPrimaryScreen().Bounds.Height  / dpiY;

            ScaleX = scaleX;
            ScaleY = scaleY;
        }

        public void Recalculate(double scale)
        {
            double width, height;

            height = OriginalHeight;
            width = OriginalWidth;

            Screen? screen = WindowScreen.FindScreen(Name);
            if (screen == null) { return; }
            Tuple<double, double> dpi = WindowScreen.GetDPI(Name);

            double dpiX = dpi.Item1;
            double dpiY = dpi.Item2;

            double screenWidth = WindowScreen.FindPrimaryScreen().Bounds.Width;
            scale = (scale * screenWidth) / 1920;

            Width = WindowScreen.FindPrimaryScreen().Bounds.Width / dpiX;
            Height = WindowScreen.FindPrimaryScreen().Bounds.Height  / dpiY;

            ScaleX = scale;
            ScaleY = scale;
        }

        public void RecalculateWindow()
        {
            double width, height;

            width = OriginalWidth * ScaleX;
            height = OriginalHeight * ScaleY;

            Width = width;
            Height = height;
        }

        public void Maximize()
        {
            ResizeButtonVisibility = Visibility.Collapsed;
            Screen? screen = WindowScreen.FindScreen(Name);
            if (screen == null) { return; }

            Tuple<double, double> dpi = WindowScreen.GetDPI(Name);

            double dpiX = dpi.Item1;
            double dpiY = dpi.Item2;

            double screenWidth = WindowScreen.FindPrimaryScreen().Bounds.Width / dpiX;
            double screenHeight = WindowScreen.FindPrimaryScreen().Bounds.Height  / dpiY;

            double startingWidth = Width;
            double startingHeight = Height;

            double scaleX = ScaleX;
            double scaleY = ScaleY;

            if (startingWidth <= screenWidth)
            {
                while (startingWidth <= screenWidth)
                {
                    scaleX += 0.001;
                    startingWidth += OriginalWidth * 0.001;
                }

                while (startingHeight <= screenHeight)
                {
                    scaleY += 0.001;
                    startingHeight += OriginalHeight * 0.001;
                }
            }
            else
            {
                while (startingWidth > screenWidth)
                {
                    scaleX -= 0.001;
                    startingWidth -= OriginalWidth * 0.001;
                }

                while (startingHeight > screenHeight)
                {
                    scaleY -= 0.001;
                    startingHeight -= OriginalHeight * 0.001;
                }
            }
            Margin = 0;
            Roundness = new CornerRadius(0);
            ScaleX = scaleX;
            ScaleY = scaleY;

            State = WindowState.Maximized;
        }

        public void Normalize(double normalScale)
        {
            ResizeButtonVisibility = Visibility.Visible;
            Screen? screen = WindowScreen.FindScreen(Name);
            if (screen == null) { return; }

            Tuple<double, double> dpi = WindowScreen.GetDPI(Name);
            double dpiX = dpi.Item1;

            double screenWidth = WindowScreen.FindPrimaryScreen().Bounds.Width / dpiX;

            normalScale = (normalScale * screenWidth) / 1920;

            double width = OriginalWidth * normalScale;
            double height = OriginalHeight * normalScale;

            ScaleX = normalScale;
            ScaleY = normalScale;
            Width = width;
            Height = height;
            Margin = 5;
            Roundness = new CornerRadius(18);

            CenterOnScreen();

            State = WindowState.Normal;
            ScaleSlider = 0.69;
        }
        public void CenterOnScreen()
        {
            
            Screen? screen = WindowScreen.FindScreen(Name);
            if (screen == null) { return; }

            Tuple<double, double> dpi = WindowScreen.GetDPI(Name);

            double dpiX = dpi.Item1;
            double dpiY = dpi.Item2;

            double bLeft = screen.Bounds.Left / OriginalDpi;
            if (screen.Primary) { bLeft = screen.Bounds.Left; }
            double bTop = screen.Bounds.Top; 

            double bWidth = WindowScreen.FindPrimaryScreen().Bounds.Width / dpiX;
            double bHeight = WindowScreen.FindPrimaryScreen().Bounds.Height   / dpiY;

            var centerX = bLeft + (bWidth - Width) / 2;
            var centerY = bTop + (bHeight - Height) / 2;

            Left = centerX;
            Top = centerY;
        }

        public void CenterOnScreenSpecificWindow(string Window, double initialDpi)
        {
            Screen? screen = WindowScreen.FindScreen(Window);
            if (screen == null) { return; }

            Tuple<double, double> dpi = WindowScreen.GetDPI(Window);

            double dpiX = dpi.Item1;
            double dpiY = dpi.Item2;

            double bLeft = screen.Bounds.Left / initialDpi;
            if (screen.Primary) { bLeft = screen.Bounds.Left; }
            double bTop = screen.Bounds.Top;

            double bWidth = WindowScreen.FindPrimaryScreen().Bounds.Width / dpiX;
            double bHeight = WindowScreen.FindPrimaryScreen().Bounds.Height / dpiY;

            var centerX = bLeft + (bWidth - Width) / 2;
            var centerY = bTop + (bHeight - Height) / 2;

            Left = centerX;
            Top = centerY;
        }
    }
}
