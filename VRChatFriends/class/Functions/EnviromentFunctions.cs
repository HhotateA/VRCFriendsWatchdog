using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using System.ComponentModel;
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Markup;
using System.Windows.Threading;

namespace VRChatFriends.Function
{
    static class Functions
    {
        static BitmapImage defaultThumbnail;
        public static BitmapImage DefaultThumbnail
        {
            get
            {
                if (defaultThumbnail == null)
                {
                    defaultThumbnail = ThumbnailLoader.Instance.Load(ConfigData.DefaultThumbnailURL);
                }
                return defaultThumbnail;
            }
        }

        public static long TimeStamp { get { return (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds; } }
        public static string DateString { get
            {
                DateTime now = DateTime.Now;
                return now.ToString("yyyy/MM/dd HH:mm:ss");
            }
        }
        public static string TimeString
        {
            get
            {
                DateTime now = DateTime.Now;
                return now.ToString("HH:mm:ss");
            }
        }

        public static int WeekInt()
        {
            var now = DateTime.Now;
            DayOfWeek dow = now.DayOfWeek;
            return (int)dow;
        }
        public static int HourInt()
        {
            var now = DateTime.Now;
            return now.Hour;
        }
        public static string CachePath { get { return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData, Environment.SpecialFolderOption.Create); } }
        
        /// <summary>
        ///     A string extension method that replace first occurence.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        /// <returns>The string with the first occurence of old value replace by new value.</returns>
        public static string ReplaceFirst(string origin, string oldValue, string newValue)
        {
            int startindex = origin.IndexOf(oldValue);

            if (startindex == -1)
            {
                return origin;
            }

            return origin.Remove(startindex, oldValue.Length).Insert(startindex, newValue);
        }
        static Random random = new Random();
        public static int RandomInt(int max)
        {
            return random.Next(max);
        }
        public static string FileCheck(string basePath,string fileName)
        {
            System.IO.Directory.CreateDirectory(basePath);
            string filePath = Path.Combine(basePath, fileName);
            if (!File.Exists(filePath))
            {
                using (FileStream fs = File.Create(filePath))
                {
                    fs.Close();
                }
            }
            return filePath;
        }

        public static Window ActiveWindow { get; set; }

        public static void DispatcheFunction(Action e)
        {
            if (ActiveWindow != null)
            {
                ActiveWindow.Dispatcher.BeginInvoke((Action) (() => { e?.Invoke(); }));
            }
        }
        public static string FileCheck(string path)
        {
            string basePath = "./";
            string fileName = "";
            var s = path.Split('/');
            if(s.Length==1)
            {
                fileName = s[0];
            }
            else
            {
                for(int i = 0; i < s.Length-1; i++)
                {
                    basePath = Path.Combine(basePath, s[i]);
                }
                fileName = s[s.Length-1];
            }
            System.IO.Directory.CreateDirectory(basePath);
            string filePath = Path.Combine(basePath, fileName);
            if (!File.Exists(filePath))
            {
                using (FileStream fs = File.Create(filePath))
                {
                    fs.Close();
                }
            }
            return filePath;
        }
        public static string IdToURL(string id)
        {
            var a = id.Split(':');
            if(a.Length<2) return "https://www.vrchat.com/home";
            return "https://www.vrchat.com/home/launch?worldId=" + a[0] + "&instanceId=" + a[1];
        }
        public static string IdToURLDetail(string id)
        {
            var a = id.Split(':');
            return "https://www.vrchat.com/home/world/" + a[0];
        }
    }
    public enum FavoriteType
    {
        None,
        API,
        Local
    }

    static class Debug
    {
        static string[] logs = new string[17]{"","","","","","","","","","","", "", "", "", "", "", "",};
        public static Action<string> OnCatchLog;

        public static string Logs
        {
            get
            {
                string t = "";
                for (int i = 0; i < logs.Length; i++)
                {
                    t += Functions.TimeString + " : " + logs[i] + Environment.NewLine;
                }
                return t;
            }
        }

        public static void Log(string log)
        {
            for (int i = 1; i < logs.Length; i++)
            {
                logs[i - 1] = logs[i];
            }

            logs[logs.Length - 1] = log;
            OnCatchLog?.Invoke(Logs);
        }
    }
}
