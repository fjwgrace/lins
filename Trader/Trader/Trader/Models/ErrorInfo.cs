using JsonFx.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trader.Models
{
    internal class ErrorInfo
    {
        [JsonName("error_code")]
        public string ErrorCode { get; set; }

        [JsonName("error_text")]
        public string ErrorText { get; set; }
    }
}
