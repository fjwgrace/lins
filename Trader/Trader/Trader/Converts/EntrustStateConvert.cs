using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Trader.Converts
{
    internal class EntrustStateConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                string entrustState = value.ToString();
                switch (entrustState)
                {

                    case "0":
                        return "未报";
                    case "1":
                        return "待报";
                    case "2":
                        return "已报";
                    case "3":
                        return "待撤";
                    case "4":
                        return "部分成交";
                    case "5":
                        return "部成待撤";
                    case "6":
                        return "部成已撤";
                    case "F":
                        return "已成";
                    case "C":
                        return "撤单";
                    case "R":
                        return "废单";
                    default:
                        return "未知委托状态";
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
