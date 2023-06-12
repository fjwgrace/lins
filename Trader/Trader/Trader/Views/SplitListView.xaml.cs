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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Trader.ViewModels;

namespace Trader.Views
{
    /// <summary>
    /// SplitListView.xaml 的交互逻辑
    /// </summary>
    public partial class SplitListView : UserControl
    {
        SplitListViewModel viewModel;
        public SplitListView()
        {
            InitializeComponent();
            viewModel = new SplitListViewModel();
            this.DataContext = viewModel;
        }
        private void MIRefresh_Click(object sender, RoutedEventArgs e)
        {
            viewModel.LoadData( );
        }

        private void MIModify_Click(object sender, RoutedEventArgs e)
        {
            if(viewModel.CurrentSetting==null)
            {
                MessageBox.Show("请选中一条记录进行操作");
                return;
            }
            SplitStockWindow win = new SplitStockWindow(viewModel.CurrentSetting);
        
            win.ShowDialog();
            if(win.DialogResult==true)
            {
                win.Owner=null;
                win = null;
            }
        }

        private void MIDelete_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel.CurrentSetting == null)
            {
                MessageBox.Show("请选中一条记录进行操作");
                return;
            }
            viewModel.Delete();
        }

        private void MenuItemData_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
