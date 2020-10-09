using Prism.Commands;
using Prism.Mvvm;
using System.Windows.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using VRChatFriends.Usecase;

namespace VRChatFriends.ViewModels
{
	public class LoginViewModel : BindableBase
    {
        public LoginViewModel()
        {
            OnClickLogin = new DelegateCommand(() =>
            {
                new Watchdog().ReLogin(UserName,Password, OnLoginSuccess);
            });
        }
        public Action OnLoginSuccess;
        string userName = "";
        public string UserName
        {
            get => userName;
            set => SetProperty(ref userName, value);
        }
        string password = "";
        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }
        string logmsg = "Login";
        public string LogMsg
        {
            get => logmsg;
            set => SetProperty(ref logmsg, value);
        }

        ICommand onClickLogin;
        public ICommand OnClickLogin
        {
            get => onClickLogin;
            set => SetProperty(ref onClickLogin, value);
        }
    }
}