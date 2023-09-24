using JsonFx.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trader.Models
{
    internal class GroupInfo
    {
        [JsonName("username")]
        public string UserName;
        [JsonName("group")]
        public string Group;
    }
}
