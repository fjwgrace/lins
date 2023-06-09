using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Trader.Core;
using Trader.Models;

namespace Trader.ViewModels
{
    public class LoginViewModel: ObservableObject
    {
        static LoginRequestInfo LoginRequestInfo { get; set; }
        public event Action<bool?> SetDialogResult;
        private ICommand _loginCommand;
        public ICommand LoginCommand
        {
            get
            {
                if (_loginCommand == null)
                {
                    _loginCommand = new RelayCommand(ExecuteLoginCommand);
                }
                return _loginCommand;
            }
        }
        private string _message;
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        
        private bool _isVisible;
        public bool IsVisible
        {
            get { return _isVisible; }
            set { SetProperty(ref _isVisible, value); }
        }
        public LoginViewModel() 
        {
            LoginRequestInfo = new LoginRequestInfo();
        }
        public bool _isSuccess;
        public bool IsSuccess
        {
            get { return _isSuccess; }
            set {SetProperty(ref _isSuccess, value);  }
        }
        public async void ExecuteLoginCommand( )
        {
            IsVisible = false;
            Message = string.Empty;
            if (string.IsNullOrEmpty(LoginRequestInfo.UserName))
            {
                IsVisible = true;
                Message = "账号不能为空";
            }
            if (string.IsNullOrEmpty(LoginRequestInfo.Password))
            {
                IsVisible = true;
                Message = "密码不能为空";
            }
           
           var ts1 =Task.Run(()=>  SystemInfoProvider.GetIPAddress());
           var ts2 =Task.Run(()=>  SystemInfoProvider.GetCPUID());
           var ts3 = Task.Run(() => SystemInfoProvider.GetHDSN());
           var ts4= Task.Run(() => SystemInfoProvider.GetMac());
            Task.WaitAll(ts1, ts2, ts3, ts4);

            LoginRequestInfo.IPAddress = ts1.Result.FirstOrDefault();
            LoginRequestInfo.CPUID = ts2.Result;
            LoginRequestInfo.HdSN = ts3.Result;
            LoginRequestInfo.Mac = ts4.Result;

            var result = await DataCenter.Login(LoginRequestInfo);
            if(result.Status!=System.Net.HttpStatusCode.OK)
            {
                IsVisible = true;
                Message = result?.Error.ErrorText;
            }
            else
            {
                IsVisible = false;
                Message = "";
                IsSuccess = true;
                SetDialogResult.Invoke(true);
            }
        }
        private bool CanExecuteCommand( )
        {
            // 在此实现判断是否可执行的逻辑
            if(!string.IsNullOrEmpty(LoginRequestInfo.UserName)&&!string.IsNullOrEmpty(LoginRequestInfo.Password))
            {
                return true;
            }
            return false; // 默认为可执行
        }
        public string UserName
        {
            get
            {
                return LoginRequestInfo.UserName;
            }
            set
            {
                SetProperty(ref LoginRequestInfo.UserName, value);
            }
        }
        public string Password
        {
            get
            {
                return LoginRequestInfo.Password;
            }
            set
            {
                SetProperty(ref LoginRequestInfo.Password, value);
            }
        }
    }
}
