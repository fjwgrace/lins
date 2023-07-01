using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Trader.Models
{
    internal class SyncResponse
    {
        public HttpStatusCode Status { get; set; }
        public ErrorInfo Error { get; set; }
    }
}
