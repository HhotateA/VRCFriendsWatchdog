using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using VRChatFriends.Views;
using VRChatFriends.ViewModels;
using VRChatFriends.Usecase;
using VRChatFriends.Entity;
using VRChatFriends.Function;

namespace VRChatFriends
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        public static App Instance;
        App() : base()
        {
            Instance = this;
            //*
            new Watchdog().LoginCheck(()=>
            {
                OpenListWindow();
            },
            () =>
            {
                OpenLoginWindow();
            });
            // */
        }

        public void OpenListWindow()
        {
            Debug.Log("AutLogin : Success");
            this.Dispatcher.BeginInvoke((Action)(() =>
            {
                var v = new ListWindow();
                Functions.ActiveWindow = v;
                var vm = new ListWindowViewModel();
                v.DataContext = vm;
                vm.OpenLoginDialog += (() =>
                {
                    OpenLoginWindow();
                    CloseActiveWindow(v);
                });
                vm.OpenWindow += (() =>
                {
                    v.Show();

                });
                v.AppShutdown += AppShutdown;
                v.Show();
            }));
        }
        public void OpenLoginWindow()
        {
            Debug.Log("AutLogin : Failed");
            this.Dispatcher.BeginInvoke((Action)(() =>
            {
                var v = new Login();
                Functions.ActiveWindow = v;
                var vm = new LoginViewModel();
                v.DataContext = vm;
                v.OnPasswordChange += (e) =>
                {
                    vm.Password = e;
                };
                vm.OnLoginSuccess += (() =>
                {
                    OpenListWindow();
                    CloseActiveWindow(v);
                });
                v.Show();
            }));
        }
        public void CloseActiveWindow(Window window)
        {
            this.Dispatcher.BeginInvoke((Action)(() =>
            {
                window?.Close();
            }));
        }

        public void AppShutdown()
        {
            Shutdown();
        }
    }
}
