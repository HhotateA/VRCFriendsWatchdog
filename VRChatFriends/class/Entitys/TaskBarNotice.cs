using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hardcodet.Wpf.TaskbarNotification;
using VRChatFriends.Function;
using System.Windows.Media;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Media.Imaging;
using Microsoft.Toolkit.Uwp.Notifications;using System;
using System.Windows;
using Notifications.Wpf;

namespace VRChatFriends.Entity
{
    class TaskBarNotice
    {
        NotificationManager notificationManager = new NotificationManager();

        public void Notice(string title,string msg)
        {
            if(ConfigData.NoticeTimer>0)
            {
                var content = new NotificationContent
                {
                    Type = NotificationType.Information,
                    Title = title,
                    Message = msg,
                };

                notificationManager.Show(
                    content, expirationTime: TimeSpan.FromSeconds(ConfigData.NoticeTimer));
            }
        }
    }
}
