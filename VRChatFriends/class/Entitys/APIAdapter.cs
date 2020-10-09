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
                password = ConfigData.PassWord;
            }
            else
            {
                ConfigData.PassWord = id;
            }
            api = new VRChatApi.VRChatApi(id, password);
            return this;
        }

        bool isLoginSuccess = false;

        public async void LoginCheck(Action onSuccess = null,Action onFailure = null)
        {
            var response = await api.FriendsApi.Get(0,1).ConfigureAwait(false);
            if(response==null)
            {
                Console.WriteLine("Login failed");
                isLoginSuccess = false;
                onFailure?.Invoke();
            }
            else
            {
                Console.WriteLine("Login Success");
                isLoginSuccess = true;
                onSuccess?.Invoke();
            }
        }

        void OnNullResponce()
        {
            Console.WriteLine("Null Response Error");
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
            Console.WriteLine("Get Online Friends");
            await GetOnlineFriends(result, true).ConfigureAwait(false);
            Console.WriteLine("Get Offline Friends");
            await GetOnlineFriends(result, false).ConfigureAwait(false);
            Console.WriteLine("Geted All Friends");
            onFinish?.Invoke();
        }

        /// <summary>
        /// APIにアクセスし，オンラインのフレンド一覧を取得する
        /// </summary>
        /// <param name="result"></param>
        /// <param name="online">falseにするとオフライン取得</param>
        /// <param name="onFinish"></param>
        public async Task GetOnlineFriends(
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
                var response = await api.FriendsApi.Get(i * 100, 100, !online).ConfigureAwait(false);

                if(response == null)
                {
                    OnNullResponce();
                    break;
                }

                foreach (var value in response)
                {
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
            Console.WriteLine("API Finish");
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
            result?.Invoke(o);
        }

        public async Task GetWorldData(string id,Action<LocationData> result)
        {
            var o = new LocationData(id);
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
            result?.Invoke(o);
        }

        public async Task GetFavorite(Action<string> result,Action onFinish = null)
        {
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
            for(int i=1;i<=3;i++)
            {
                var response = await api.FavouriteApi.GetFavouriteUsers("group_"+i).ConfigureAwait(false);
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
            onFinish?.Invoke();
        }
    }
}
