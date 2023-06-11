using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Trader.Converts
{
    internal class EntrustDirConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                string entrustDir = value.ToString();
                switch(entrustDir)
                {

                    case "B":
                        return "买入";
                    case "S":
                        return "卖出";
                    case "SS":
                        return "融资卖出";
                    case "BC":
                        return "买券换券";
                    case "FB":
                        return "融资买入";
                    case "SR":
                        return "卖券还款";
                    default:
                        return "未知委托方向";
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
