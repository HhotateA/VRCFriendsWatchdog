using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using VRChatFriends.Function;

namespace VRChatFriends.Entity
{
    class DataStore
    {
        public Dictionary<string, LocationData> LocationDataList { get; set; } = new Dictionary<string, LocationData>();
        public Dictionary<string, UserData> UserDataList { get; set; } = new Dictionary<string, UserData>();
        public Action<LocationData, UserData> OnAddUser { get; set; }
        public Action<LocationData, UserData> OnRemoveUser { get; set; }
        public Action<UserData> OnLostUser { get; set; }
        public Action<UserData> OnUpdateUser { get; set; }
        public Action<LocationData> OnLostLocation { get; set; }
        public Action<LocationData> OnInitializeLocation { get; set; }
        public Action<UserData> OnInitializeUser { get; set; }

        public void ReloadInstance()
        {
            LocationDataList = new Dictionary<string, LocationData>();
            UserDataList = new Dictionary<string, UserData>();
        }

        public void UpdateUser(UserData data)
        {
            UserData user;
            if(UserDataList.ContainsKey(data.Id))
            {
                user = UserDataList[data.Id];
            }
            else
            {
                user = new UserData(data.Id);
                user.Name = data.Name;
                user.ThumbnailURL = data.ThumbnailURL;
                OnInitializeUser?.Invoke(user);
                UserDataList.Add(data.Id, user);
            }
            
            if (user.Location == data.Location)
            {
                // インスタンス移動なし
                // サムネイル変更
                if(user.ThumbnailURL!=data.ThumbnailURL)
                {
                    user.ThumbnailURL = data.ThumbnailURL;
                    OnUpdateUser?.Invoke(user);
                }
            }
            else
            {
                if(!String.IsNullOrWhiteSpace(user.Location) && LocationDataList.ContainsKey(user.Location))
                {
                    var oldLocation = LocationDataList[user.Location];
                    oldLocation.Users.Remove(user);
                    if(oldLocation.Users.Count==0)
                    {
                        LocationDataList.Remove(oldLocation.Id);
                        OnLostLocation?.Invoke(oldLocation);
                    }
                    OnRemoveUser?.Invoke(oldLocation, user);
                }
                user.Location = data.Location;

                LocationData newLocation;
                if (LocationDataList.ContainsKey(data.Location))
                {
                    newLocation = LocationDataList[data.Location];
                }
                else
                {
                    newLocation = new LocationData(data.Location);
                    LocationDataList.Add(data.Location,newLocation);
                    OnInitializeLocation?.Invoke(newLocation);
                }
                newLocation.Users.Add(user);
                OnAddUser?.Invoke(newLocation, user);
            }
        }
        public async Task UpdateOfflineUser(List<UserData> onlineUser, Action<List<UserData>, List<LocationData>> result = null)
        {
            Debug.Log("Update Offline User");
            var keys = UserDataList.Keys.ToArray();
            var values = UserDataList.Values.ToArray();
            for (int i = 0; i<UserDataList.Count;i++)
            {
                UserData a = null;
                for(int j=0;j<onlineUser.Count;j++)
                {
                    if(onlineUser[j].Id == keys[i])
                    {
                        a = onlineUser[j];
                        break;
                    }
                }
                if(a==null)
                {
                    var u = values[i];
                    if (u.Location != "offline")
                    {
                        OnLostUser?.Invoke(u);
                    }
                }
            }
            result?.Invoke(
                UserDataList.Select(l => l.Value).ToList(),
                LocationDataList.Select(l => l.Value).ToList());
        }
        public void GetUserLocations(Action<List<UserData>, List<LocationData>> result)
        {
            result(
                UserDataList.Select(l=>l.Value).ToList(),
                LocationDataList.Select(l => l.Value).ToList());
        }
    }
}
