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
    internal class EntrustDirColorConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                string entrustDir=value.ToString();
                if (entrustDir.Contains("买")||entrustDir.Contains("B"))
                {
                    return (SolidColorBrush)Application.Current.Resources["BrushRed"];
                }
                else if (entrustDir.Contains("卖") || entrustDir.Contains("S"))
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
