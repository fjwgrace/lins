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
                    MessageBox.Show("操作正确");
                }
                else
                {
                    if (string.IsNullOrEmpty(result.Error?.error_text))
                    {
                        MessageBox.Show("操作错误");
                    }
                    else
                    {
                        MessageBox.Show(string.Format("操作错误，错误原因:{0}", result.Error.error_text));
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Logger.Error("删除分券数据出错", ex);
            }
        }

        public async void SetAdapter()
        {
            try
            {
                var result = await DataCenter.SetAdapter( );
                if (result.Status != System.Net.HttpStatusCode.Accepted)
                {
                    MessageBox.Show(result.Error.error_text,"提示", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show("操作成功", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                Log.Logger.Error("分券同步出错", ex);
            }
        }
    }
}
