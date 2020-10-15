using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRChatApi.Endpoints;
using VRChatFriends.Function;

namespace VRChatFriends.Entity
{
    class APIAdapter
    {
        static APIAdapter instance;
        public static APIAdapter Instance
        {
            get
            {
                if(instance==null)
                {
                    instance = new APIAdapter();
                }
                return instance;
            }
        }
        VRChatApi.VRChatApi api;
        public Action OnLogout { get; set; }
        public APIAdapter(string id = "",string password = "")
        {
            Login(id,password);
        }

        public void ReloadInstance()
        {
            instance.Login();
        }

        /// <summary>
        /// ログインを行う．
        /// </summary>
        public APIAdapter Login(string id = "", string password = "")
        {
            if (string.IsNullOrEmpty(id))
            {
                id = ConfigData.UserName;
            }
            else
            {
                ConfigData.UserName = id;
            }
            if (string.IsNullOrEmpty(password))
            {
                password = ConfigData.Password;
            }
            else
            {
                ConfigData.Password = password;
            }
            api = new VRChatApi.VRChatApi(id, password);
            return this;
        }
        public string LoginUserId { get; set; } = "";

        bool isLoginSuccess = false;

        public async void LoginCheck(Action onSuccess = null,Action onFailure = null)
        {
            try
            {
                var response = await api.UserApi.Login();
                if (response == null)
                {
                    Debug.Log("Login failed");
                    LoginUserId = "";
                    isLoginSuccess = false;
                    onFailure?.Invoke();
                }
                else
                {
                    Debug.Log("Login Success");
                    LoginUserId = response.id;
                    isLoginSuccess = true;
                    onSuccess?.Invoke();
                }
            }
            catch
            {
                Debug.Log(">>>APIError");
            }
        }

        void OnNullResponce()
        {
            Debug.Log("Null Response Error");
            isLoginSuccess = false;
            OnLogout?.Invoke();
        }

        /// <summary>
        /// APIにアクセスし，フレンド一覧を取得する
        /// </summary>
        /// <param name="result">フレンド分のidを返す</param>
        /// <param name="resultDetails">id,name,instanceID,thumbnail</param>
        /// <param name="onFinish">取得終了時に呼び出される</param>
        public async Task GetFriends(
            Action<UserData> result = null,
            Action onFinish = null
            )
        {
            Debug.Log("Get Online Friends");
            GetFriends(result, true).ConfigureAwait(false);
            Debug.Log("Get Offline Friends");
            GetFriends(result, false).ConfigureAwait(false);
            Debug.Log("Geted All Friends");
            onFinish?.Invoke();
        }

        public async Task GetOnlineFriend(
            Action<UserData> result = null,
            Action<List<UserData>> onFinish = null)
        {
            Debug.Log("Get Online Friends");
            GetFriends(result, true, (users)=> 
            {
                onFinish?.Invoke(users);
            });
        }


        /// <summary>
        /// APIにアクセスし，オンラインのフレンド一覧を取得する
        /// </summary>
        /// <param name="result"></param>
        /// <param name="online">falseにするとオフライン取得</param>
        /// <param name="onFinish"></param>
        public async Task GetFriends(
            Action<UserData> result = null,
            bool online = true,
            Action<List<UserData>> onFinish = null
            )
        {
            var output = new List<UserData>();
            int i = 0;
            int timeoutCount = 15;
            while (true)
            {
                try
                {
                    var response = await api.FriendsApi.Get(i * 100, 100, !online).ConfigureAwait(false);

                    if(response == null)
                    {
                        OnNullResponce();
                        break;
                    }

                    for(int r=0;r<response.Count;r++)
                    {
                        var value = response[r];
                        var o = new UserData(value.id);
                        o.Name = value.displayName;
                        o.ThumbnailURL = value.currentAvatarThumbnailImageUrl;
                        o.Location = value.location;
                        o.UserName = value.username;
                        o.Platform = value.last_platform;
                        o.Status = value.status;
                        o.StatusDescription = value.statusDescription;
                        foreach(var v in value.tags)
                        {
                            o.Tag += v;
                        }
                        output.Add(o);
                        result?.Invoke(o);
                    }
                    if (response.Count < 100)
                    {
                        timeoutCount--;
                        if(timeoutCount<0)
                        {
                            break;
                        }
                        await Task.Delay(300).ConfigureAwait(false);
                    }
                    i++;
                }
                catch
                {
                    Debug.Log(">>>APIError");
                }
            }
            Debug.Log("API Finish");
            onFinish?.Invoke(output);
        }

        /// <summary>
        /// APIにアクセスし，ユーザーIDからユーザーデータを取得する
        /// </summary>
        /// <param name="id"></param>
        /// <param name="">name,instanceID,thumbnail</param>
        public async Task GetUserData(string id,Action<UserData> result)
        {
            var o = new UserData(id);
            try
            {
                var response = await api.UserApi.GetById(o.Id).ConfigureAwait(false);
                if (response == null)
                {
                    OnNullResponce();
                    return;
                }
                o.Name = response.displayName;
                o.ThumbnailURL = response.currentAvatarThumbnailImageUrl;
                o.Location = response.location;
                o.UserName = response.username;
                o.Platform = response.last_platform;
                o.Status = response.status;
                o.StatusDescription = response.statusDescription;
                foreach (var v in response.tags)
                {
                    o.Tag += v;
                }
            }
            catch
            {
                Debug.Log(">>>APIError");
            }
            result?.Invoke(o);
        }
        public async Task GetLoginUser(Action<UserData> result)
        {
            try
            {
                var response = await api.UserApi.Login().ConfigureAwait(false);
                if (response == null)
                {
                    OnNullResponce();
                    return;
                }
                var o = new UserData(response.id);
                o.Name = response.displayName;
                o.ThumbnailURL = response.currentAvatarThumbnailImageUrl;
                o.Location = response.location;
                Debug.Log(o.Location+"Login");
                o.UserName = response.username;
                o.Platform = response.last_platform;
                o.Status = response.status;
                o.StatusDescription = response.statusDescription;
                foreach (var v in response.tags)
                {
                    o.Tag += v;
                }
                result?.Invoke(o);
            }
            catch
            {
                Debug.Log(">>>APIError");
            }
        }
        public async Task GetWorldData(string id,Action<LocationData> result)
        {
            var o = new LocationData(id);
            try
            {
                var response = await api.WorldApi.Get(o.WorldID).ConfigureAwait(false);
                if (response == null)
                {
                    OnNullResponce();
                    return;
                }
                o.Tag = "";
                response.tags.ForEach(l => o.Tag += l + " , ");
                o.Name = response.name;
                o.ThumbnailURL = response.thumbnailImageUrl;
                o.OutherName = response.authorName;
                o.Description = response.description;
                o.Capacity = response.occupants + "/" + response.capacity;
                o.ReleaseStatus = response.releaseStatus.ToString();
            }
            catch
            {
                Debug.Log(">>>APIError");
            }
            result?.Invoke(o);
        }

        public async Task GetFavorite(Action<string> result,Action onFinish = null)
        {
            try
            {
                var response = await api.FavouriteApi.GetFavourites("friend").ConfigureAwait(false);
                if (response == null)
                {
                    OnNullResponce();
                    return;
                }
                foreach (var item in response)
                {
                    result?.Invoke(item.favoriteId);
                }
            }
            catch
            {
                Debug.Log(">>>APIError");
            }
            try
            {
                for (int i = 1; i <= 3; i++)
                {
                    var response = await api.FavouriteApi.GetFavouriteUsers("group_" + i).ConfigureAwait(false);
                    if (response == null)
                    {
                        OnNullResponce();
                        return;
                    }
                    foreach (var item in response)
                    {
                        result?.Invoke(item.id);
                    }
                }
            }
            catch
            {
                Debug.Log(">>>APIError");
            }
            onFinish?.Invoke();
        }
    }
}
