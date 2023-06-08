using JsonFx.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trader.Models
{
    public  class LoginRequestInfo
    {
        [JsonName("username")]
        public string UserName;
        [JsonName("password")]
        public string Password;
        [JsonName("mac")]
        public string Mac;
        [JsonName("lan_ip")]
        public string IPAddress;
        [JsonName("hd_sn")]
        public string HdSN;
        [JsonName("cpu_id")]
        public string CPUID;
    }
}
