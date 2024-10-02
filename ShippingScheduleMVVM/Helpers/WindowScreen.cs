 
using System;
using System.Diagnostics; 
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;

namespace ShippingScheduleMVVM.Helpers
{
    internal class WindowScreen
    {
        public static Screen? FindScreen(string windowTitle)
        {
            System.Windows.Window? window = System.Windows.Application.Current.Windows.OfType<System.Windows.Window>().FirstOrDefault(w => w.Title == windowTitle);

            if (window == null) { return null; }

            Screen? screen = System.Windows.Forms.Screen.FromHandle(new System.Windows.Interop.WindowInteropHelper(window).Handle);

            return screen;
        }

        public static Tuple<double, double> GetDPI(string windowTitle)
        {
            System.Windows.Window? window = System.Windows.Application.Current.Windows.OfType<System.Windows.Window>().FirstOrDefault(w => w.Title == windowTitle);
            double dpiX, dpiY;
            var scale = VisualTreeHelper.GetDpi(window);
            var initialScale = scale.DpiScaleY;
            var primaryScreenHeight = FindPrimaryScreenHeight();
            primaryScreenHeight = primaryScreenHeight * scale.DpiScaleY;

            dpiX = scale.DpiScaleX;
            dpiY = scale.DpiScaleY;

            var windowInteropHelper = new WindowInteropHelper(window);
            var hwnd = windowInteropHelper.Handle;
            Screen screen = Screen.FromHandle(hwnd);

            if (!screen.Primary)
            {
                if (screen.Bounds.Height > primaryScreenHeight)
                {
                    if (primaryScreenHeight * dpiY == screen.Bounds.Height)
                    {
                        dpiX = 1.0f;
                        dpiY = 1.0f;
                    }
                }
                else 
                {
                    dpiX = Math.Round(primaryScreenHeight / screen.Bounds.Height, 2); 
                    dpiY = Math.Round(primaryScreenHeight / screen.Bounds.Height, 2); 
                }
            }
             
            return Tuple.Create(dpiX, dpiY);
        }
         
        public static double FindPrimaryScreenHeight()
        {
            foreach(var screen in Screen.AllScreens)
            {
                if (screen.Primary)
                {
                    return screen.Bounds.Height;
                }
            }
            return 0f;
        }
         
        public static Screen FindPrimaryScreen()
        {
            foreach (var screen in Screen.AllScreens)
            {
                if (screen.Primary)
                {
                    return screen;
                }
            }
            return Screen.AllScreens[0];
        }

        public static Point? GetLocationMouse(string WindowName)
        {
            System.Windows.Window? window = System.Windows.Application.Current.Windows.OfType<System.Windows.Window>().FirstOrDefault(w => w.Title == WindowName);
            if (window == null) { return null; }
            Point GetMousePos() => window.PointToScreen(Mouse.GetPosition(window));
            Point point = GetMousePos();
            var transform = PresentationSource.FromVisual(window).CompositionTarget.TransformFromDevice;
            var position = transform.Transform(point);

            return position;
        }

    }
}
