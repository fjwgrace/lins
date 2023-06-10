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
    /// Position.xaml 的交互逻辑
    /// </summary>
    public partial class PositionView : UserControl
    {
        PositionViewModel viewModel;
        public PositionView()
        {
            InitializeComponent();
            viewModel = new PositionViewModel();
            this.DataContext = viewModel;
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            viewModel.LoadData();
        }

        private void MenuItemStock_Click(object sender, RoutedEventArgs e)
        {
            viewModel.ExecuteSplitStockCommand( );
        }

        private void MenuItemData_Click(object sender, RoutedEventArgs e)
        {
            viewModel.ExportToExcel(positionGrid);
        }
    }
}
