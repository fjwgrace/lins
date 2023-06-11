using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trader.Models
{
    public class PositionSetting
    {
        public int index { get; set; }
        public string id { get; set; }
        public string username { get; set; }
        public string symbol { get; set; }
        public string security_name { get; set; }
        public UInt64 authorized_qty { get; set; }
        public string buy_model { get; set; }
        public string sell_model { get; set; }
        public string last_closing_date { get; set; }
    }
}
