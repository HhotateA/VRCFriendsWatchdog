using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using VRChatFriends.Usecase;

namespace VRChatFriends.Function
{
    class UIManager
    {
        static UIManager instance;
        public static UIManager Instance
        {
            get
            {
                if(instance ==null)
                {
                    instance = new UIManager();
                }
                return instance;
            }
        }

        public Panel FriendList { get; set; } = new StackPanel();
    }
    static class UI
    {
        /// <summary>
        /// メインスレッドからUIを操作する
        /// </summary>
        /// <param name="dispatcherObject"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        internal static DispatcherOperation DisBegInv(Action action)
        {
            return UIManager.Instance.FriendList.Dispatcher.BeginInvoke(action);
        }
    }
}
