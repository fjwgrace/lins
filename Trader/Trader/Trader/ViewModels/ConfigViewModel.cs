using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Trader.Core;
using Trader.Models;

namespace Trader.ViewModels
{
    public class ConfigViewModel: ObservableObject
    {
        private string _message;
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        private string _ip;
        public string IP
        {
            get { return _ip; }
            set {SetProperty(ref _ip, value);}
        }
        private string _port;
        public string Port
        {
            get { return _port; }
            set { SetProperty(ref _port, value); }
        }


        private bool _isVisible;
        public bool IsVisible
        {
            get { return _isVisible; }
            set { SetProperty(ref _isVisible, value); }
        }

        private bool _isSslEnbale;
        public bool IsSslEnable
        {
            get { return _isSslEnbale; }
            set
            {
                SetProperty(ref _isSslEnbale, value);
                _isSslDisable = !_isSslEnbale;
            }
        }
        private bool _isSslDisable;
        public bool IsSslDisable
        {
            get { return _isSslDisable; }
            set
            {
                SetProperty(ref _isSslDisable, value);
                _isSslEnbale = !_isSslDisable;
            }
        }
        public ConfigViewModel()
        {
            _ip = ConfigurationManager.AppSettings["ip"];
            _port = ConfigurationManager.AppSettings["port"];
            bool ssl = bool.TryParse(ConfigurationManager.AppSettings["isSslEnable"], out _isSslEnbale);
            if(!ssl)
            {
                Log.Logger.Error("Ssl Enable 配置有误");
            }
            else
            {
                _isSslDisable=!_isSslEnbale;
            }
        }
        public bool SaveConfig()
        {
            _isVisible = false;
            _message = string.Empty;
            if(_isSslDisable==false&&_isSslEnbale==false)
            {
                _isVisible = true;
                _message = "请选择是否启用SSL";
                return false;
            }
            if(string.IsNullOrEmpty(_ip))
            {
                _isVisible = true;
                _message = "IP地址不能为空";
                return false;
            }
            if (string.IsNullOrEmpty(_port))
            {
                _isVisible = true;
                _message = "端口号不能为空";
                return false;
            }
            if(!ValidateHelper.IsValidIP(_ip))
            {
                _isVisible = true;
                _message = "请输入正确格式的IP地址";
                return false;
            }
            if (!ValidateHelper.IsValidPort(_port))
            {
                _isVisible = true;
                _message = "请输入正确格式的端口号";
                return false;
            }
            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                // 更新或添加配置项的值
                config.AppSettings.Settings["ip"].Value = _ip;
                config.AppSettings.Settings["port"].Value = _port;
                config.AppSettings.Settings["isSslEnable"].Value = _isSslEnbale == true ? "true" : "false";
                // 保存配置文件
                config.Save(ConfigurationSaveMode.Modified);

                // 强制重新加载配置文件以使更改生效
                ConfigurationManager.RefreshSection("appSettings");
                return true;
            }
            catch(Exception ex)
            {
                Log.Logger.Error("保存配置失败", ex);
                _message = "保存失败";
                return false;
            }
        }
    }
}
