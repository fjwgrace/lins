using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Trader
{
    internal class NumericValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if(int.TryParse(value.ToString(), out int result))
            {
                return new ValidationResult(true, "");
            }
            else
            { return new ValidationResult(false, "数量必须为大于零的整数"); }
        }
    }
}
