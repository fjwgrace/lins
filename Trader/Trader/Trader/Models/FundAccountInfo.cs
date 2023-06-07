using JsonFx.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Trader.Models
{
    internal class FundAccountInfo
    {
        [JsonName("username")]
        public string Username { get; set; }
        [JsonName("inner_account_id")]
        public string InnerAccountId { get; set; }
        [JsonName("institution")]
        public string Institution { get; set; }
        [JsonName("realname")]
        public string Realname { get; set; }
        [JsonName("password")]
        public string Password { get; set; }
        [JsonName("id_no")]
        public string IdNo { get; set; }
        /// <summary>
        /// 印花税
        /// </summary>
        [JsonName("stamp_duty")]
        public double StampDuty { get; set; }
        /// <summary>
        /// 过户费
        /// </summary>
        [JsonName("transfer_fee")]
        public double TransferFee { get; set; }
        /// <summary>
        /// 交易员佣金
        /// </summary>
        [JsonName("commission")]
        public double Commission { get; set; }
        /// <summary>
        /// 最大亏损额度
        /// </summary>
        [JsonName("max_loss")]
        public double MaxLoss { get; set; }
        /// <summary>
        /// 最大亏损额度是否启用
        /// </summary>
        [JsonName("ax_loss_active")]
        public bool MaxLossActive { get; set; }
        /// <summary>
        /// 用户角色，有admin、inverstor、trader三种
        /// </summary>
        [JsonName("role")]
        public string Role { get; set; }

        [JsonName("ip")]
        public string Ip { get; set; }
        
        [JsonName("mac")]
        public string Mac { get; set; }
        
        [JsonName("active")]
        public bool Active { get; set; }
        
        [JsonName("fund_account_ids")]
        public string FundAccountIds { get; set; }
        
        [JsonName("usernames")]
        public string Usernames { get; set; }

        [JsonName("extra_info")]
        public string ExtraInfo { get; set; }

    }
}
