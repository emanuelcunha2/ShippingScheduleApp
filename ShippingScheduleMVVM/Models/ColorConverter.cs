using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ShippingScheduleMVVM.Models
{
    public static class ColorConverter
    {
        public static SolidColorBrush GetSolidColorBrushFromHSL(double hue, double saturation, double lightness)
        {
            // Ensure hue is within [0, 360) range
            hue %= 360;
            if (hue < 0) hue += 360;

            // Ensure saturation and lightness are within [0, 1] range
            saturation = Math.Max(0, Math.Min(1, saturation));
            lightness = Math.Max(0, Math.Min(1, lightness));

            // Calculate intermediate values for HSL-to-RGB conversion
            double chroma = (1 - Math.Abs(2 * lightness - 1)) * saturation;
            double huePrime = hue / 60.0;
            double x = chroma * (1 - Math.Abs(huePrime % 2 - 1));

            double red = 0, green = 0, blue = 0;
            if (huePrime >= 0 && huePrime < 1)
            {
                red = chroma;
                green = x;
            }
            else if (huePrime >= 1 && huePrime < 2)
            {
                red = x;
                green = chroma;
            }
            else if (huePrime >= 2 && huePrime < 3)
            {
                green = chroma;
                blue = x;
            }
            else if (huePrime >= 3 && huePrime < 4)
            {
                green = x;
                blue = chroma;
            }
            else if (huePrime >= 4 && huePrime < 5)
            {
                red = x;
                blue = chroma;
            }
            else if (huePrime >= 5 && huePrime < 6)
            {
                red = chroma;
                blue = x;
            }

            double m = lightness - chroma / 2;
            red += m;
            green += m;
            blue += m;

            // Convert RGB to WPF Color
            byte byteRed = (byte)(255 * red);
            byte byteGreen = (byte)(255 * green);
            byte byteBlue = (byte)(255 * blue);

            Color color = Color.FromRgb(byteRed, byteGreen, byteBlue);

            // Create and return the SolidColorBrush
            return new SolidColorBrush(color);
        }
    }
}
