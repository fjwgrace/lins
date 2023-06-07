using JsonFx.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trader.Models
{
    internal class LoginResponseInfo
    {
        [JsonName("expire")]
        public string Expire { get; set; }
        [JsonName("fund_account_ids")]
        public string AccountID {get; set;}
        [JsonName("realname")]
        public string RealName { get; set; }
        [JsonName("role")]
        public string Role { get; set; }
        [JsonName("token")]
        public string Token { get; set; }
        [JsonName("username")]
        public string UserName { get; set; }
    }
}
