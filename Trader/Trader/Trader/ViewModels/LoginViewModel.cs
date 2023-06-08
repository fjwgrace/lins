using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trader.Models;

namespace Trader.ViewModels
{
    public class LoginViewModel: ObservableObject
    {
        static LoginRequestInfo LoginRequestInfo { get; set; }
        public LoginViewModel() {
        LoginRequestInfo = new LoginRequestInfo();
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
            get {
                return LoginRequestInfo.Password;
            }
            set {
                SetProperty(ref LoginRequestInfo.Password, value);
            }
        }
    }
}
