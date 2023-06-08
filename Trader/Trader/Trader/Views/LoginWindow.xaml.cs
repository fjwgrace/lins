using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Trader.Core;
using Trader.Models;

namespace Trader.Views
{
    /// <summary>
    /// LoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : HandyControl.Controls.Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            Log.Logger.Information("It's just a test");
            this.Loaded += LoginWindow_Loaded;
        }

        private async void LoginWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoginRequestInfo loginRequestInfo = new LoginRequestInfo();
            loginRequestInfo.IPAddress = (await SystemInfoProvider.GetIPAddress()).First();
            loginRequestInfo.CPUID = await SystemInfoProvider.GetCPUID();
            loginRequestInfo.HdSN=await SystemInfoProvider.GetHDSN();
            loginRequestInfo.Mac=await SystemInfoProvider.GetMac();
            loginRequestInfo.UserName = "1880021060";
            loginRequestInfo.Password = "123456";
            var result = await DataCenter.Login(loginRequestInfo);
            Console.WriteLine(result);

        }
    }
}
