using System.Windows.Media;

namespace ShippingScheduleMVVM.Models
{
    public class Category
    {
        public static SolidColorBrush GetColorFromCategory(string category)
        {
            return category switch
            {
                "Created" => new SolidColorBrush(Color.FromRgb(189, 189, 189)),
                "Released" => new SolidColorBrush(Color.FromRgb(255, 203, 33)),
                "Process" => new SolidColorBrush(Color.FromRgb(245, 147, 0)),
                "Prepared" => new SolidColorBrush(Color.FromRgb(128, 176, 255)),
                "Shipped" => new SolidColorBrush(Color.FromRgb(24, 219, 131)),
                _ => new SolidColorBrush(Color.FromRgb(24, 219, 131)),
            };
        }
    }
}
