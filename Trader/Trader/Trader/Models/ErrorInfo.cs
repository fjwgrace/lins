using JsonFx.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trader.Models
{
    public class ErrorInfo
    {
        public int error_code { get; set; }

        public string error_text { get; set; }
    }
}
