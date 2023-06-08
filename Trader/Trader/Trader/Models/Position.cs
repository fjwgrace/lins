using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Trader.Models
{
    public class Position
    {
        public string fund_account_id { get; set; }
        public string inner_account_id { get; set; }
        public string username { get; set; }
        public string trading_day { get;set; }
        public string symbol { get; set; }  
        public string security_name { get; set; }
        public string buy_model { get; set; }
        public string sell_model { get; set;  }
        public string account_type { get; set; }
        public UInt64 initial { get; set; }
        public UInt64 total { get; set; }
        public UInt64 available { get; set; }
        public UInt64 buy_frozen { get; set; }
        public UInt64 sell_frozen { get; set; }
        public double buy_cost_price { get; set; }
        public double sell_cost_price { get;set; }
        public double total_cost_price { get; set; }
        public UInt64 buy_filled_quantity { get; set; }
        public UInt64 sell_filled_quantity { get; set; }
        public double buy_fee {  get; set; }
        public double sell_fee { get; set; }
        public UInt64 initial_allocated_quantity { get; set; }
        public UInt64 buy_quantity { get; set; }
        public UInt64 sell_quantity { get;set; }
        public UInt64 selling_short_quantity { get; set; }
        public UInt64 max_credit_stock { get; set; }
        public UInt64 available_credit_stock { get; set; }
        public UInt64 allocable_quantity { get; set; }
        public UInt64 allocated_quantity { get; set; }
        public Int32 update_times { get; set; }
        public string last_update_time { get; set; }
        public string last_update_date { get; set; }
        public double realize_profit { get; set; }
        public double floating_profit { get; set; }
        public string last_closing_date { get; set; }
        public Dictionary<string, string> extra_info { get; set; }

    }
}
