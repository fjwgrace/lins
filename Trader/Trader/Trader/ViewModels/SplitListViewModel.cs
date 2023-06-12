using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Trader.Core;
using Trader.Message;
using Trader.Models;

namespace Trader.ViewModels
{
    internal class SplitListViewModel:ObservableObject
    {
        public SplitListViewModel() {
            if (DataCenter.GlobalLogin == null) { return; }
            LoadData();
            RefreshCommand = new RelayCommand(ExecuteCommand);
            WeakReferenceMessenger.Default.Register<PositionMsg>(this, (r, m) =>
            {
                LoadData(); 
            });
        }
        public ICommand RefreshCommand { get; private set; }

        public void ExecuteCommand()
        {
            LoadData();
        }
        private PositionSetting _currentSetting;
        public PositionSetting CurrentSetting
        {
            get { return _currentSetting; }
            set { SetProperty(ref _currentSetting, value); }
        }
        private ObservableCollection<PositionSetting> _positionSettings;
        public ObservableCollection<PositionSetting> PositionSettings
        {
            get { return _positionSettings; }
            set { SetProperty(ref _positionSettings, value); }
        }
        public async void LoadData( )
        {
              var result = await DataCenter.GetPositionSetting( );
              if (result.Status == System.Net.HttpStatusCode.OK)
              {
                   PositionSettings = new ObservableCollection<PositionSetting>(result.Info);
              }

        }


        public async void Delete( )
        {
            try
            {
                var result = await DataCenter.UpdatePositionSetting(CurrentSetting, "DELETE");
                if (result.Status == System.Net.HttpStatusCode.NoContent)
                {
                    LoadData();
                }
                else
                {
                    MessageBox.Show(string.Format("删除分券数据出错:{0}", result.Error.ErrorText));
                }
            }
            catch (Exception ex)
            {
                Log.Logger.Error("删除分券数据出错", ex);
            }
        }
    }
}
