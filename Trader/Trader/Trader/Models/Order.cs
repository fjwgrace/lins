using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trader.Models
{
    public class Order
    {
        public int index { get; set; }
        public string fund_account_id { get; set; }
        public string username { get; set; }
        public string inner_account_id { get; set; }
        public string trading_day { get; set; }
        public string cli_order_id { get; set; }
        public string tbox_order_id { get; set; }
        public string symbol { get; set; }
        public string business { get; set; }
        public string side { get; set; }
        public string price_type { get; set; }
        public double price { get; set; }
        public UInt64 quantity { get; set; }
        public string broker_order_id { get; set; }
        public double average_price { get; set; }
        public UInt64 filled_quantity { get; set; }
        public UInt64 leaves_quantity { get; set; }
        public string status { get; set; }
        public bool  done { get; set; }
        public Int32 error_code { get; set; }
        public string error_text { get; set; }
        public string source { get; set; }
        public string accept_time { get; set; }
        public string submit_time { get; set; }
        public string recv_time { get; set; }
        public string broker_insert_time { get; set; }
        public string broker_update_time { get; set; }
        public Int32 update_times { get; set; }
        public string last_update_time { get; set; }
        public string last_update_date { get; set; }
        public string account_type { get; set; }
        public string security_name { get; set; }
        public Dictionary<string,string> extra_info { get; set; }
        public double deal_price { get; set; }
    }
}
