using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Trader.Models
{
    internal class LoginResponse
    {
        public HttpStatusCode Status { get; set; }
        public LoginResponseInfo Info { get; set; }
        public ErrorInfo Error { get; set; }
    }
}
