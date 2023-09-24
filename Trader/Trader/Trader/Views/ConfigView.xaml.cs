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
using Trader.ViewModels;

namespace Trader.Views
{
    /// <summary>
    /// ConfigView.xaml 的交互逻辑
    /// </summary>
    public partial class ConfigView : Window
    {
        ConfigViewModel viewModel;
        public ConfigView()
        {
            InitializeComponent();
            viewModel=new ConfigViewModel(); ;
            this.DataContext = viewModel;
        }

        private void btnConfig_Click(object sender, RoutedEventArgs e)
        {
           if( viewModel.SaveConfig()==true ) { this.DialogResult = true; }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
