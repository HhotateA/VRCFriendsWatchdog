using System;
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
using Console = System.Console;

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

        public void ReloadInstance()
        {
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
        public async Task LogUsers(List<LocationData> locations,int countUp=1)
        {
            if (savedUsers==null)
            {
                Debug.Log("Create Log ...");
                savedUsers = new UsersSaveData();
            }
            Debug.Log("Write Log ...");
            for(int i=0;i<locations.Count;i++)
            {
                for(int j=0;j<locations[i].Users.Count;j++)
                {
                    UserSaveData u = null;
                    for (int k = 0; k < savedUsers.users.Count; k++)
                    {
                        if (savedUsers.users[k].id == locations[i].Users[j].Id)
                        {
                            u = savedUsers.users[k];
                            break;
                        }
                    }
                    if (u == null)
                    {
                        u = new UserSaveData();
                        u.id = locations[i].Users[j].Id;
                        u.name = locations[i].Users[j].Name;
                        u.lastUpdate = Functions.DateString;
                        u.friends = new List<FriendUserSaveData>();
                        u.footprint = new WeeksFootprint();
                        savedUsers.users.Add(u);
                    }
                    // 時間ごとのログイン履歴を記録
                    u.footprint.CountupFootorint(
                        Functions.WeekInt(),Functions.HourInt(),
                        locations[i].Status,countUp);
                }
                if (locations[i].Id == "offline")
                {
                    // continue;
                }
                else
                if (locations[i].Id == "private")
                {
                    // プラべの場合自分だけ加算
                    for (int j = 0; j < locations[i].Users.Count; j++)
                    {
                        UserSaveData u = null;
                        for (int k = 0; k < savedUsers.users.Count; k++)
                        {
                            if (savedUsers.users[k].id == locations[i].Users[j].Id)
                            {
                                u = savedUsers.users[k];
                                break;
                            }
                        }
                        if (u == null)
                        {
                            u = new UserSaveData();
                            u.id = locations[i].Users[j].Id;
                            u.name = locations[i].Users[j].Name;
                            u.lastUpdate = Functions.DateString;
                            u.friends = new List<FriendUserSaveData>();
                            savedUsers.users.Add(u);
                        }
                        {
                            var friend = locations[i].Users[j];
                            FriendUserSaveData f = null;
                            for (int m = 0; m < u.friends.Count; m++)
                            {
                                if (u.friends[m]?.id == friend.Id)
                                {
                                    f = u.friends[m];
                                    break;
                                }
                            }
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
                else
                {
                    // インスタンスないの全員を加算
                    for (int j = 0; j < locations[i].Users.Count; j++)
                    {
                        UserSaveData u = null;
                        for (int k = 0; k < savedUsers.users.Count; k++)
                        {
                            if (savedUsers.users[k].id == locations[i].Users[j].Id)
                            {
                                u = savedUsers.users[k];
                                break;
                            }
                        }
                        if (u == null)
                        {
                            u = new UserSaveData();
                            u.id = locations[i].Users[j].Id;
                            u.name = locations[i].Users[j].Name;
                            u.lastUpdate = Functions.DateString;
                            u.friends = new List<FriendUserSaveData>();
                            savedUsers.users.Add(u);
                        }
                        for (int k = 0; k < locations[i].Users.Count; k++)
                        {
                            FriendUserSaveData f = null;
                            for (int m = 0; m < u.friends.Count; m++)
                            {
                                if (u.friends[m]?.id == locations[i].Users[k].Id)
                                {
                                    f = u.friends[m];
                                    break;
                                }
                            }
                            if (f == null)
                            {
                                f = new FriendUserSaveData();
                                f.id = locations[i].Users[k].Id;
                                f.name = locations[i].Users[k].Name;
                                f.lastUpdate = Functions.DateString;
                                f.count = 0;
                                u.friends.Add(f);
                            }
                            f.count += countUp;
                        }
                        // ロケーションデータ側にも記録
                        if (!locations[i].UserHistry.ContainsKey(locations[i].Users[j].Id))
                        {
                            locations[i].UserHistry.Add(locations[i].Users[j].Id,new UserFootprints(locations[i].Users[j].Name, locations[i].Users[j].Id));
                        }
                        locations[i].UserHistry[locations[i].Users[j].Id].Count += countUp;
                    }
                }
            }
            SaveLog();
        }
        public async Task LoadLog()
        {
            Debug.Log("Load Log File");
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
        }
        public async Task SaveLog()
        {
            Debug.Log("Save Log File");
            string filePath = Functions.FileCheck(ConfigData.LogOutputPath, ConfigData.UserLogFileName);
            if(savedUsers == null) savedUsers = new UsersSaveData();
            var f = JsonConvert.SerializeObject(savedUsers);
            using (StreamWriter sw = new StreamWriter(
                filePath,
                false, Encoding.UTF8))
            {
                sw.Write(f);
            }
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
            List<FriendUserSaveData> friends = null;
            for (int i = 0; i < savedUsers.users.Count; i++)
            {
                if (savedUsers.users[i]?.id == id)
                {
                    friends = savedUsers.users[i]?.friends;
                }
            }
            var output = new Dictionary<string, int>();
            if(friends!=null)
            {
                for (int i = 0; i < friends.Count; i++)
                {
                    if (friends[i] != null)
                    {
                        output.Add(friends[i].name, friends[i].count);
                    }
                }
            }
            return output;
        }

        public WeeksFootprint GetFootPrint(string id)
        {
            WeeksFootprint footprint = new WeeksFootprint();
            for(int i=0;i<savedUsers.users.Count;i++)
            {
                if(savedUsers.users[i].id == id)
                {
                    footprint = savedUsers.users[i].footprint;
                }
            }
            footprint.InitializeColor();
            return footprint;
        }
    }
}
