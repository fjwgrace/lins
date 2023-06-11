using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Trader.Models;
using Trader.ViewModels;

namespace Trader.Views
{
    /// <summary>
    /// SplitStockWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SplitStockWindow : Window
    {
        PositionSetViewModel viewModel;
        public SplitStockWindow(Position data)
        {
            InitializeComponent();
            viewModel = new PositionSetViewModel(data);
            this.DataContext = viewModel;
            viewModel.SetDialogResult += ViewModel_SetDialogResult;
        }

        void ViewModel_SetDialogResult(bool? result)
        {
            this.DialogResult = result;
            this.Close();
        }

        public SplitStockWindow(PositionSetting data)
        {
            InitializeComponent();
            viewModel = new PositionSetViewModel(data);
            this.DataContext = viewModel;
            viewModel.SetDialogResult += ViewModel_SetDialogResult;
        }
        private void MoveWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSplit_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Create();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }
    }
}
