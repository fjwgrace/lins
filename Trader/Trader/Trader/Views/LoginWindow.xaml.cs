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
using Trader.ViewModels;

namespace Trader.Views
{
    /// <summary>
    /// LoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow :Window
    {
        LoginViewModel viewModel;

        public LoginWindow()
        {
            InitializeComponent();
            viewModel = new LoginViewModel();
            this.DataContext = viewModel;
            this.Loaded += LoginWindow_Loaded;
            viewModel.SetDialogResult += SetDialogRsult;
        }
        void SetDialogRsult(bool? result)
        {
            this.DialogResult = result;
            this.Close();
        }
        private  void LoginWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //LoginRequestInfo loginRequestInfo = new LoginRequestInfo();
            //loginRequestInfo.IPAddress = (await SystemInfoProvider.GetIPAddress()).First();
            //loginRequestInfo.CPUID = await SystemInfoProvider.GetCPUID();
            //loginRequestInfo.HdSN=await SystemInfoProvider.GetHDSN();
            //loginRequestInfo.Mac=await SystemInfoProvider.GetMac();
            //loginRequestInfo.UserName = "1880021060";
            //loginRequestInfo.Password = "123456";
            //var result = await DataCenter.Login(loginRequestInfo);
            //Console.WriteLine(result);
            //result.Info.AccountID = result.Info.UserName;
            //var result2 = await DataCenter.GetPositions(result.Info.AccountID, result.Info.UserName);
            //Console.WriteLine(result2);
            //var result3=await  DataCenter.GetOrders(result.Info.AccountID, result.Info.UserName);
            //Console.WriteLine(result3);
            //var result4= await DataCenter.GetDeals(result.Info.AccountID, result.Info.UserName);
            //Console.WriteLine(result4);
        }
        private void MoveWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            viewModel.Password = passwordBox.Password.ToString();
            btnLogin.Command.CanExecute(null);

        }
    }
}
