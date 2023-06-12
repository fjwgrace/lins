using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Trader.Core;
using Trader.Message;
using Trader.Models;

namespace Trader.ViewModels
{
    internal class PositionSetViewModel : ObservableObject
    {
        public event Action<bool?> SetDialogResult;
        private ObservableCollection<string> _traders;

        public ObservableCollection<string> Traders
        {
            get { return _traders; }
            set { SetProperty(ref _traders, value); }
        }
        public ObservableCollection<string> BuyModes { get; set; } = new ObservableCollection<string>(new List<string>() { "买入", "融资买入", "买券还券" });
        public ObservableCollection<string> SellModes { get; set; } = new ObservableCollection<string>(new List<string>() { "卖出", "融资卖出", "卖券还款" });
        private string _currentTrader;
        public string CurrentTrader
        {
            get { return _currentTrader; }
            set { SetProperty(ref _currentTrader, value); }
        }
        private string _buyMode;
        public string BuyMode
        {
            get { return _buyMode; }
            set { SetProperty(ref _buyMode, value); }
        }
        private string _sellMode;
        public string SellMode
        {
            get { return _sellMode; }
            set { SetProperty(ref _sellMode, value); }
        }
        private string _stockCode;
        public string StockCode
        {
            get { return _stockCode; }
            set { SetProperty(ref _stockCode, value); }
        }
        private string _stockName;
        public string StockName
        {
            get { return _stockName; }
            set { SetProperty(ref _stockName, value); }
        }

        private string _closeDate;
        public string CloseDate
        {
            get { return _closeDate; }
            set { SetProperty(ref _closeDate, value); }
        }

        private string _message;
        public string Message {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }
        private bool _isVisible;
        public bool IsVisible
        {
            get { return _isVisible; }
            set { SetProperty(ref _isVisible, value); }
        }
        private UInt64 _qty;
        public UInt64 Qty
        {
            get { return _qty; }
            set { SetProperty(ref _qty, value); }
        }
       public bool IsCreate { get; set; }
       public PositionSetViewModel(Position data)
        {
            IsCreate = true;
            var usernames = DataCenter.GlobalLogin.UserNames.Split(',');
            Traders = new ObservableCollection<string>(usernames);
            Traders.Add("strategy");
            CurrentTrader = Traders.First();
            StockCode = data.symbol;
            StockName = data.security_name;
            BuyMode = data.buy_model;
            SellMode = data.sell_model;
            CloseDate = data.last_closing_date;
            BuyMode = "买入";
            SellMode = "卖出";
        }

        public PositionSetViewModel(PositionSetting data)
        {
            IsCreate = false;
            var usernames = DataCenter.GlobalLogin.UserNames.Split(',');
            Traders = new ObservableCollection<string>(usernames);
            Traders.Add("strategy");
            CurrentTrader = Traders.First();
            StockCode = data.symbol;
            StockName = data.security_name;
            Qty = data.authorized_qty;
            CloseDate = data.last_closing_date;
            string bm = "";
            switch (data.buy_model)
            {
                case "B":
                    bm = "买入";
                    break;
                case "FB":
                    bm = "融资买入";
                    break;
                case "BC":
                    bm = "买券还券";
                    break;
            }
            string sm = "";
            switch (data.sell_model)
            {
                case "S":
                    sm = "卖出";
                    break;
                case "SS":
                    sm = "融资卖出";
                    break;
                case "SR":
                    sm = "卖券还款";
                    break;
            }
            BuyMode = bm;
            SellMode = sm;
        }
        
        public async void Create( )
        {
            IsVisible = false;
            PositionSetting ps = new PositionSetting();
            ps.symbol = StockCode;
            ps.security_name = StockName;
            ps.username = CurrentTrader;
            ps.id = DataCenter.GlobalLogin.UserName;
            ps.last_closing_date = CloseDate;
            ps.authorized_qty = Qty;
            string bm = "B";
            switch(BuyMode)
            {
                case "买入":
                    bm="B";
                    break;
                case "融资买入":
                    bm = "FB";
                    break;
                case "买券还券":
                    bm = "BC";
                    break;
            }
            string sm = "S";
            switch (SellMode)
            {
                case "卖出":
                    sm = "S";
                    break;
                case "融资卖出":
                    sm = "SS";
                    break;
                case "卖券还款":
                    sm = "SR";
                    break;
            }
            ps.buy_model = bm;
            ps.sell_model = sm;

            var result = await DataCenter.UpdatePositionSetting(ps,"POST");
            if (result.Status != System.Net.HttpStatusCode.OK)
            {
                Message = result.Error.ErrorText;
                IsVisible = true;
            }
            else
            {
                SetDialogResult.Invoke(true);
                WeakReferenceMessenger.Default.Send(new PositionMsg(null));
            }

        }

        public async void Modify( )
        {
            try
            {
                IsVisible = false;
                PositionSetting ps = new PositionSetting();
                ps.symbol = StockCode;
                ps.security_name = StockName;
                ps.username = CurrentTrader;
                ps.id = DataCenter.GlobalLogin.UserName;
                ps.last_closing_date = CloseDate;
                ps.authorized_qty = Qty;
                string bm = "B";
                switch (BuyMode)
                {
                    case "买入":
                        bm = "B";
                        break;
                    case "融资买入":
                        bm = "FB";
                        break;
                    case "买券还券":
                        bm = "BC";
                        break;
                }
                string sm = "S";
                switch (SellMode)
                {
                    case "卖出":
                        sm = "S";
                        break;
                    case "融资卖出":
                        sm = "SS";
                        break;
                    case "卖券还款":
                        sm = "SR";
                        break;
                }
                ps.buy_model = bm;
                ps.sell_model = sm;
                var result = await DataCenter.UpdatePositionSetting(ps, "PUT");
                if (result.Status != System.Net.HttpStatusCode.NoContent)
                {
                    Message = result.Error.ErrorText;
                    IsVisible = true;
                }
                else
                {
                    SetDialogResult.Invoke(true);
                   WeakReferenceMessenger.Default.Send(new PositionSettingMsg(ps));
                }
            }
            catch (Exception ex)
            {
                Log.Logger.Error("修改分券数据出错", ex);
            }
        }
    }
}
