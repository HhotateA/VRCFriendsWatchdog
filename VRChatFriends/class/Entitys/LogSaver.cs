﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;
using VRChatFriends.Function;
using Newtonsoft.Json;
using System.Timers;
using System.Windows.Threading;

namespace VRChatFriends.Entity
{
    class LogSaver
    {
        static LogSaver instance;
        public static LogSaver Instance
        {
            get
            { 
                if(instance==null)
                {
                    instance = new LogSaver();
                }
                return instance;
            }
        }
        UsersSaveData savedUsers;
        Timer timer;
        public LogSaver()
        {
            LoadLog();
            if (ConfigData.LogInterval > 0)
            {
                timer = new Timer(ConfigData.LogInterval * 1000);
                timer.Elapsed += (sender, e) =>
                {
                    SaveLog();
                };
                timer.Start();
            }
            SaveFullLog();
        }
        async public void LogUsers(List<LocationData> locations,int countUp=1)
        {
            await Task.Run(() =>
            {
                if (savedUsers==null)
                {
                    Console.WriteLine("Create Log ...");
                    savedUsers = new UsersSaveData();
                }
                Console.WriteLine("Write Log ...");
                foreach (var location in locations)
                {
                    if (location.Id == "offline" || location.Id == "private") continue;
                    foreach (var user in location.Users)
                    {
                        var u = savedUsers.users.FirstOrDefault(l => l.id == user.Id);
                        if (u == null)
                        {
                            u = new UserSaveData();
                            u.id = user.Id;
                            u.name = user.Name;
                            u.lastUpdate = Functions.DateString;
                            u.friends = new List<FriendUserSaveData>();
                            savedUsers.users.Add(u);
                        }
                        foreach (var friend in location.Users)
                        {
                            var f = u.friends.FirstOrDefault(l => l.id == friend.Id);
                            if (f == null)
                            {
                                f = new FriendUserSaveData();
                                f.id = friend.Id;
                                f.name = friend.Name;
                                f.lastUpdate = Functions.DateString;
                                f.count = 0;
                                u.friends.Add(f);
                            }
                            f.count += countUp;
                        }
                    }
                }
                SaveLog();
            }).ConfigureAwait(false);
        }
        public async Task LoadLog()
        {
            await Task.Run(() =>
            {
                Console.WriteLine("Load Log File");
                string filePath = Functions.FileCheck(ConfigData.LogOutputPath, ConfigData.UserLogFileName);
                using (StreamReader sr = new StreamReader(
                    filePath,
                    Encoding.UTF8))
                {
                    var f = sr.ReadToEnd();
                    try
                    {
                        savedUsers = JsonConvert.DeserializeObject<UsersSaveData>(f);
                        if (savedUsers == null) savedUsers = new UsersSaveData();
                    }
                    catch
                    {
                        savedUsers = new UsersSaveData();
                    }
                }
            }).ConfigureAwait(false);
        }
        public async Task SaveLog()
        {
            await Task.Run(() =>
            {
                Console.WriteLine("Save Log File");
                string filePath = Functions.FileCheck(ConfigData.LogOutputPath, ConfigData.UserLogFileName);
                if(savedUsers == null) savedUsers = new UsersSaveData();
                var f = JsonConvert.SerializeObject(savedUsers);
                using (StreamWriter sw = new StreamWriter(
                    filePath,
                    false, Encoding.UTF8))
                {
                    sw.Write(f);
                }
            }).ConfigureAwait(false);
        }
        List<string> logCache = new List<string>();
        public void LogUser(UserData user)
        {
            LogMsg(Functions.DateString+","+user.Id+","+user.Location+",");
        }
        public void LogMsg(string msg)
        {
            logCache.Add(msg);
        }
        async Task SaveFullLog()
        {
            string filePath = Functions.FileCheck(ConfigData.LogOutputPath, ConfigData.LocationLogFileName);
            using (StreamWriter sw = new StreamWriter(
                filePath,
                false, Encoding.UTF8))
            {
                while(Function.ConfigData.FullLog)
                {
                    if(logCache.Count!=0)
                    {
                        sw.WriteLine(logCache[0]);
                        logCache.RemoveAt(0);
                    }
                    await Task.Delay(500);
                }
            }
        }
        public Dictionary<string,int> GetUserLog(string id)
        {
            return savedUsers?.users.
                FirstOrDefault(l => l.id == id)?.friends.
                Select(l => { return (l); }).
                ToDictionary(x => x.name, x => x.count);
        }
    }
}
