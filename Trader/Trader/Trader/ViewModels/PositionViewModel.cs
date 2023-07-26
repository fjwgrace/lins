using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Trader.Core;
using Trader.Message;
using Trader.Models;

namespace Trader.ViewModels
{
    internal class PositionViewModel:ObservableObject
    {
        public ICommand RefreshCommand { get; private set; }

        public  void ExecuteCommand()
        {
            LoadData();
        }

        public void ExportToExcel(DataGrid grid)
        {
            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            excel.Visible = true;
            Microsoft.Office.Interop.Excel.Workbook workbook = excel.Workbooks.Add(System.Reflection.Missing.Value);
            Microsoft.Office.Interop.Excel.Worksheet sheet1 = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Sheets[1];
            int StartCol = 1;
            int StartRow = 1;
            for (int j = 0; j < grid.Columns.Count; j++)
            {
                Microsoft.Office.Interop.Excel.Range myRange = (Microsoft.Office.Interop.Excel.Range)sheet1.Cells[StartRow, StartCol + j];
                myRange.Value2 = grid.Columns[j].Header;
            }
            StartRow++;
            for (int i = 0; i < grid.Columns.Count; i++)
            {
                for (int j = 0; j < grid.Items.Count; j++)
                {
                    TextBlock b = grid.Columns[i].GetCellContent(grid.Items[j]) as TextBlock;
                    Microsoft.Office.Interop.Excel.Range myRange = (Microsoft.Office.Interop.Excel.Range)sheet1.Cells[StartRow + j, StartCol + i];
                    myRange.Value2 = b.Text;
                }
            }
        }

        public ObservableCollection<string> _accounts;

        public ObservableCollection<string> Accounts
        {
            get { return _accounts; }
            set { SetProperty(ref _accounts, value); }
        }
        public ObservableCollection<string> _traders;

        public ObservableCollection<string> Traders
        {
            get { return _traders; }
            set { SetProperty(ref _traders, value); }
        }
        private ObservableCollection<Position> _positions;
        public ObservableCollection<Position> Positions
        {
            get { return _positions; }
            set { SetProperty(ref _positions, value); }
        }
        public PositionViewModel()
        {
            if (DataCenter.GlobalLogin == null) { return; }
            var usernames = DataCenter.GlobalLogin.UserNames?.Split(',');
            Accounts = new ObservableCollection<string>();
            Accounts.Add(DataCenter.GlobalLogin.UserName);
            UserName = DataCenter.GlobalLogin.UserName;

            if (usernames != null)
            {
                Traders = new ObservableCollection<string>(usernames);
            }
            else
            {
                Traders = new ObservableCollection<string>();
            }
            Traders.Add("strategy");
            Traders.Insert(0, UserName);
            TradeName = Traders.First();
            LoadData();

            RefreshCommand = new RelayCommand(ExecuteCommand);


            WeakReferenceMessenger.Default.Register<PositionSettingMsg>(this, (r, m) =>
            {
                LoadData(); 
            });
        }
 
        private string _username;
        public string UserName
        {
            get { return _username; }
            set { SetProperty(ref _username, value); }
        }
        private string _tradename;
        public string TradeName
        {
            get { return _tradename; }
            set { SetProperty(ref _tradename, value); }
        }

        private Position _currentPositon;
        public Position CurrentPositon
        {
            get { return _currentPositon; }
            set { SetProperty(ref _currentPositon, value); }
        }

        public async void LoadData()
        {
            try
            {
                if(!string.IsNullOrEmpty(UserName)&&!string.IsNullOrEmpty(TradeName))
                {
                  var result= await  DataCenter.GetPositions(UserName, TradeName);
                    if (result.Status == System.Net.HttpStatusCode.OK)
                    {
                        Positions = new ObservableCollection<Position>(result.Info);
                        if(Positions?.Count>0)
                        {
                            double? totalRealize=Positions.Select(m=>m.realize_profit).Sum();
                            double? floatProfit= Positions.Select(m => m.floating_profit).Sum();
                            Positions.Add(new Position() { index = "汇总", realize_profit = totalRealize, floating_profit = floatProfit });
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Log.Logger.Error("加载持仓数据出错", ex);
            }
        }
    }
}
