using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRChatFriends.Entity;
using System.Timers;
using System.Windows.Threading;
using VRChatFriends.Function;

namespace VRChatFriends.Usecase
{
    class Watchdog
    {
        APIAdapter api;
        DataStore data;
        LogSaver log;
        public Watchdog()
        {
            api = APIAdapter.Instance.Login();
            data = new DataStore();
            log = LogSaver.Instance;
            data.OnInitializeLocation += (location) =>
            {
                if (location.WorldID == "offline" || location.WorldID == "private")
                {
                    location.Name = location.WorldID;
                }
                else
                {
                    api.GetWorldData(location.Id, (l) =>
                    {
                        location.Name = l.Name;
                        location.ThumbnailURL = l.ThumbnailURL;
                        location.Tag = l.Tag;
                        location.OwnerName = "OwnerUser" ;
                        if (String.IsNullOrWhiteSpace(l.OwnerId))
                        {
                            OnUpdateLocation?.Invoke(location);
                        }
                        else
                        {
                            api.GetUserData(location.OwnerId, (u) =>
                            {
                                location.OwnerId = l.OwnerId;
                                location.OwnerName = u.Name;
                                OnUpdateLocation?.Invoke(location);
                            });
                        }
                    });
                }
            };
            data.OnAddUser += (a, b) =>
            {
                OnAddUser?.Invoke(a, b);
                log.LogUser(b);
            };
            data.OnRemoveUser += (a, b) => OnRemoveUser?.Invoke(a, b);
            
            //ログアウト検知
            data.OnLostUser += (a) =>
            {
                api.GetUserData(a.Id, (b) =>
                {
                    data.UpdateUser(b);
                });
            };
        }
        public void ReLogin(string id = "", string password = "", Action onSuccess = null)
        {
            api = APIAdapter.Instance.Login(id, password);
            api.LoginCheck(onSuccess);
        }
        public void LoginCheck(Action onSuccess = null, Action onFailure = null)
        {
            api = APIAdapter.Instance.Login();
            api.LoginCheck(onSuccess, onFailure);
        }
        public Action<LocationData, UserData> OnAddUser { get; set; }
        public Action OnUpdateFinish { get; set; }
        public Action<LocationData, UserData> OnRemoveUser { get; set; }
        public Action<UserData> OnUpdateUser { get; set; }
        public Action<LocationData> OnUpdateLocation { get; set; }
        public Action<UserData> OnLostUser { get; set; }
        public Action<LocationData> OnLostLocation { get; set; }
        public Action OnLogout { get; set; }
        long lastUpdate = 0;
        int deltaTime = 0;
        public void InitializeUserList(Action onFinish = null)
        {
            lastUpdate = Functions.TimeStamp;
            deltaTime = (int)(Functions.TimeStamp - lastUpdate);
            api.GetFriends( user =>
             {
                 data.UpdateUser(user);
             }
             ,()=>
             {
                 onFinish?.Invoke();
                 OnUpdateFinish?.Invoke();
             }
            );
        }
        public void UpdateUserList(Action onFinish = null)
        {
            api.GetOnlineFriend(user =>
            {
                data.UpdateUser(user);
            },
            (users) =>
            {
                OnUpdateFinish?.Invoke();
                data.UpdateOfflineUser(users, (u, l) =>
                {
                    OnUpdateFinish?.Invoke();
                    onFinish?.Invoke();
                    log.LogUsers(l, deltaTime);
                });
            });
        }
        public void UserDetail(string id,Action<UserData> result)
        {
            api.GetUserData(id,result);
        }

        Timer timer;
        public void StartWatchdog(Action tick = null)
        {
            if (ConfigData.APIUpdateInterval > 0)
            {
                Debug.Log("Start Wathcdog");
                timer = new Timer(ConfigData.APIUpdateInterval*1000);
                timer.Elapsed += (sender, e) =>
                {
                    Debug.Log("Update");
                    tick?.Invoke();
                    deltaTime = (int)(Functions.TimeStamp - lastUpdate);
                    lastUpdate = Functions.TimeStamp;
                    UpdateUserList();
                };
                timer.Start();
            }
        }
        public void StopWatchdog()
        {
            Debug.Log("Stop Wathcdog");
            timer?.Stop();
        }
        public List<string> GetDatabase(string id)
        {
            List<string> o = new List<string>();
            var u = log.GetUserLog(id);
            if(u!=null)
            {
                var sorted = u.OrderBy(l=> -l.Value).ToList();
                foreach (var item in sorted)
                {
                    if(ConfigData.FriendData)
                    {
                        o.Add(item.Key + " : " + (item.Value + 59) / 60 + "min");
                    }
                    else
                    {
                        o.Add(item.Key);
                    }
                }
            }
            return o;
        }
        
        public WeeksFootprint GetFootPrint(string id)
        {
            return log.GetFootPrint(id);
        }
    }
}