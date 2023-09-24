using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Trader.Core
{
    internal class ValidateHelper
    {
        public static bool IsValidIP(string input) {
            // 使用正则表达式检查输入是否匹配IPv4或IPv6格式
            string ipv4Pattern = @"^(25[0-5]|2[0-4][0-9]|[0-1]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[0-1]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[0-1]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[0-1]?[0-9][0-9]?)$";


            if (Regex.IsMatch(input, ipv4Pattern) || IsIPV6(input))
            {
                // 使用IPAddress类尝试解析IP地址，以确保它是有效的
                if (IPAddress.TryParse(input, out IPAddress address))
                {
                    return true;
                }
            }

            return false;

        }
        private static bool IsIPV6(string input)
        {
            IPAddress ipAddress;
            if (IPAddress.TryParse(input, out ipAddress))
            {
                // 验证IP地址是否是IPv6地址
                if (ipAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                {
                    return true;
                }
            }
            return false;
        }
        public static bool IsValidPort(string input) {
            if (int.TryParse(input, out int portNumber))
            {
                // 确保端口号在有效的范围内（1到65535之间）
                if (portNumber >= 1 && portNumber <= 65535)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
