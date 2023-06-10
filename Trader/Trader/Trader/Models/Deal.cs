using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trader.Models
{
    public class Deal
    {
        public int index { get; set; }
        public string fund_account_id { get; set; }         // 资产账号
        public string username { get; set; }                // 交易员账号
        public string inner_account_id { get; set; }        // 内部资金号
        public string trading_day { get; set; }         // 交易日
        public string cli_order_id { get; set; }         // 客户端委托编号
        public string tbox_order_id { get; set; }          // 系统委托编号
        public string symbol { get; set; }              // 证券代码
        public string business { get; set; }             // 业务类型
        public string side { get; set; }               // 委托方向
        public string match_id { get; set; }           // 成交编号
        public double price { get; set; }            // 成交价
        public UInt64 quantity { get; set; }            // 成交数量
        public string broker_order_id { get; set; }       // 柜台委托编号
        public string match_time { get; set; }           // 成交时间 HHMMSSsss
        public string recv_time { get; set; }            // 柜台响应时间 HHMMSSsss
        public Int32 update_times { get; set; }        // 更新次数
        public string last_update_time { get; set; }       // 最后更新时间
        public string last_update_date { get; set; }       // 最后更新日期
        public string account_type { get; set; }       // 账户类型
        public string security_name { get; set; }          // 证券名称
        public Dictionary<string, string> extra_info { get; set; }	// 扩展信息，暂时用不到
    }
}
