using System;
using System.Collections.Generic;
using System.IO;
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

        public static string GetPCName()
        {
            return  Environment.MachineName;
        }
        public static string GetPCSerielNo()
        {
            string serialNumber = string.Empty;
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_BIOS");
                ManagementObjectCollection managementObjects = searcher.Get();

                foreach (ManagementObject obj in managementObjects)
                {
                    serialNumber = obj["SerialNumber"].ToString();
                    break; // 获取到第一个序列号就退出循环
                }
            }
            catch (Exception ex)
            {
                // 处理异常
               Log.Logger.Error("获取计算机序列号时发生错误：" + ex.Message);
            }
            return serialNumber;
        }

        public static string GetSystemVol()
        {
            string systemVol = string.Empty;
            try
            {
                DriveInfo systemDrive = DriveInfo.GetDrives()[0]; // 获取系统盘
                string volumeLabel = systemDrive.VolumeLabel;

                if (!string.IsNullOrWhiteSpace(volumeLabel))
                {
                    systemVol = volumeLabel;
                }
                else
                {
                    Log.Logger.Information("系统盘未设置卷标号。");
                }
            }
            catch (Exception ex)
            {
                Log.Logger.Error("获取系统盘卷标号时出错: " + ex.Message);
            }
            return systemVol;
        }

        public static string GetSystemPI()
        {
            return "NA";
        }
    }
}
