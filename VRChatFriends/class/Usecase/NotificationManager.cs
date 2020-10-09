using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRChatFriends.Entity;

namespace VRChatFriends.Usecase
{
    class NotificationManager
    {
        DiscordAdapter api;
        TaskBarNotice taskbar;
        public NotificationManager()
        {
            api = DiscordAdapter.Instance;
            taskbar = new TaskBarNotice();
        }
        public void Notification(string user,string locate,string msg)
        {
            api.SendMessage(msg);
            taskbar.Notice(user,locate);
        }
        public void Log(string msg)
        {
            api.SendLog(msg);
        }
    }
}