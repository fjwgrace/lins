﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Trader.Core;
using Trader.Models;

namespace Trader.ViewModels
{
    internal class DealViewModel : ObservableObject
    {
        public ICommand RefreshCommand { get; private set; }

        public void ExecuteCommand()
        {
            LoadData();
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
        private ObservableCollection<Deal> _deals;
        public ObservableCollection<Deal> Deals
        {
            get { return _deals; }
            set { SetProperty(ref _deals, value); }
        }
        public DealViewModel()
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
            Traders.Insert(0, UserName);
            TradeName = Traders.First();
            LoadData();
            RefreshCommand = new RelayCommand(ExecuteCommand);
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
        public async void LoadData()
        {
            try
            {
                if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(TradeName))
                {
                    var result = await DataCenter.GetDeals(UserName, TradeName);
                    if (result.Status == System.Net.HttpStatusCode.OK)
                    {
                       Deals = new ObservableCollection<Deal>(result.Info);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Logger.Error("加载成交数据出错", ex);
            }
        }
    }
}
