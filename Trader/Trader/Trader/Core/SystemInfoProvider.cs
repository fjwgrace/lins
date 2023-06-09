using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Trader.Core
{
    internal class SystemInfoProvider
    {
        /// <summary>
        /// 获取本地Mac地址
        /// </summary>
        /// <returns></returns>
       public static string GetMac()
        {
            string macAddress = string.Empty;

                foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if ((nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet
                        && nic.OperationalStatus == OperationalStatus.Up)|| (nic.NetworkInterfaceType == NetworkInterfaceType.Wireless80211
                        && nic.OperationalStatus == OperationalStatus.Up))
                    {
                        macAddress = nic.GetPhysicalAddress().ToString();
                        break;
                    }
                }
            if (macAddress ==string.Empty)
            {
                throw new Exception("Could not find a MAC address.");
            }
            return macAddress;
        }
        /// <summary>
        /// 获取本地IP
        /// </summary>
        /// <returns></returns>
        public static  List<string> GetIPAddress()
        {
            List<string> ipAdd = new List<string>(8);

                foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (ni.OperationalStatus == OperationalStatus.Up && ni.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                    {
                        foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                        {
                            if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                            {
                                ipAdd.Add(ip.Address.ToString());
                            }
                        }
                    }
                }
            return ipAdd;
        }
        /// <summary>
        /// 获取本地硬盘序列号
        /// </summary>
        /// <returns></returns>
        public static string GetHDSN()
        {
            string result = string.Empty;
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
                foreach (ManagementObject diskDrive in searcher.Get())
                {
                    result = diskDrive["SerialNumber"].ToString().Trim();
                    break;
                }
            return result;
        }
        /// <summary>
        /// 获取CPU ID
        /// </summary>
        /// <returns></returns>
        public static  string GetCPUID()
        {
            string result = string.Empty;
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT ProcessorId FROM Win32_Processor");
                foreach (ManagementObject processor in searcher.Get())
                {
                    result = processor["ProcessorId"].ToString().Trim();
                    break;
                }
            return result;
        }
    }
}
