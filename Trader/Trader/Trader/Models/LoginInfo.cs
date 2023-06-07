using JsonFx.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trader.Models
{
    internal class LoginInfo
    {
        [JsonName("username")]
        public string UserName { get; set; }
        [JsonName("password")]
        public string Password { get; set; }
        [JsonName("mac")]
        public string Mac { get; set; }
        [JsonName("lan_ip")]
        public string IPAddress { get; set; }
        [JsonName("hd_sn")]
        public string HdSN { get; set; }
        [JsonName("cpu_id")]
        public string CPUID { get; set; }
    }
}
