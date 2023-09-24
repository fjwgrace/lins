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

        private void Config_Click(object sender, RoutedEventArgs e)
        {
            ConfigView configView = new ConfigView();
            if (configView.ShowDialog() == true)
            {
                DataCenter.Init();
            }
        }
    }
}
