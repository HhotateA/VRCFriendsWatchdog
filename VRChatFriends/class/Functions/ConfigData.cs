using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Windows.Input;

namespace VRChatFriends.Function
{
    static class ConfigData
    {
        public static string UserName
        {
            get { return Properties.Settings.Default.UserName; }
            set { Properties.Settings.Default.UserName = value; }
        }
        public static string PassWord
        {
            get { return Properties.Settings.Default.Password; }
            set { Properties.Settings.Default.Password = value; }
        }
        public static string PassWord_Hash
        {
            get
            {
                var hash = Properties.Settings.Default.KeyHash;
                if (!String.IsNullOrWhiteSpace(hash))
                {
                    return Function.AESCryption.Decode(hash);
                }
                else
                {
                    return "";
                }
            }
            set 
            { 
                Properties.Settings.Default.KeyHash = Function.AESCryption.Encode(value); 
            }
        }
        public static string DefaultThumbnailURL
        {
            get { return Properties.Settings.Default.ThumbnailURL; }
            set { Properties.Settings.Default.ThumbnailURL = value; }
        }
        public static string LogOutputPath
        {
            get { return Properties.Settings.Default.LogOutputPath; }
            set { Properties.Settings.Default.LogOutputPath = value; }
        }
        public static string UserLogFileName
        {
            get { return "UserLog.txt"; }
            set { Properties.Settings.Default.LogOutputPath = value; }
        }
        public static string LocationLogFileName
        {
            get { return "LocationLog.txt"; }
            set { Properties.Settings.Default.LogOutputPath = value; }
        }
        public static int APIUpdateInterval
        {
            get { return Properties.Settings.Default.APIUpdateInterval; }
            set { Properties.Settings.Default.APIUpdateInterval = value; }
        }
        public static int LogInterval
        {
            get { return Properties.Settings.Default.LogInterval; }
            set { Properties.Settings.Default.LogInterval = value; }
        }
        public static int MaxUserCell
        {
            get { return Properties.Settings.Default.MaxUserCell; }
            set { Properties.Settings.Default.MaxUserCell = value; }
        }
        public static string FavoriteListPath
        {
            get { return Properties.Settings.Default.FavoriteListPath; }
            set { Properties.Settings.Default.FavoriteListPath = value; }
        }
        public static string DiscordConfigPath
        {
            get { return Properties.Settings.Default.DiscordConfigPath; }
            set { Properties.Settings.Default.DiscordConfigPath = value; }
        }
        public static bool FullLog
        {
            get { return Properties.Settings.Default.FullLog == "enable"; }
            set { Properties.Settings.Default.FullLog = value ? "enable" : "disable"; }
        }
        public static int NoticeTimer
        {
            get { return Properties.Settings.Default.NoticeTimer; }
            set { Properties.Settings.Default.NoticeTimer = value; }
        }
    }
}
