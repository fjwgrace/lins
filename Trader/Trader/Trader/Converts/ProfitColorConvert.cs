using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Trader.Converts
{
    public class ProfitColorConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && double.TryParse(value.ToString(), out double number))
            {
                if(number>0)
                {
                    return (SolidColorBrush)Application.Current.Resources["BrushRed"];
                }
                else if(number<0)
                {
                    return (SolidColorBrush)Application.Current.Resources["BrushGreen"];
                }
                else
                {
                    return (SolidColorBrush)Application.Current.Resources["BrushBlack"];
                }
            }
            else
            {
                return (SolidColorBrush)Application.Current.Resources["BrushBlack"];
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
