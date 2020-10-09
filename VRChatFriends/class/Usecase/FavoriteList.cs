using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using VRChatFriends.Entity;
using System.Windows.Threading;
using VRChatFriends.Function;
using Newtonsoft.Json;

namespace VRChatFriends.Usecase
{
    class FavoriteList
    {
        public FavoriteList(bool initialize = true)
        {
            if(initialize)
            {
                InitializeFavoriteList();
            }
        }

        public async Task InitializeFavoriteList()
        {
            await APIAdapter.Instance.GetFavorite((id) =>
            {
                if(!APIFavoriteUsers.Contains(id))
                {
                    APIFavoriteUsers.Add(id);
                }
            }).ConfigureAwait(true);
            await LoadFavorite().ConfigureAwait(false);
        }
        List<string> APIFavoriteUsers = new List<string>();
        List<string> LocalFavoriteUsers = new List<string>();
        public async Task LoadFavorite()
        {
            await Task.Run(() =>
            {
                Console.WriteLine("Load Favorite File");
                string filePath = Functions.FileCheck(ConfigData.FavoriteListPath);
                using (StreamReader sr = new StreamReader(
                    filePath,
                    Encoding.UTF8))
                {
                    var f = sr.ReadToEnd();
                    try
                    {
                        var savedUsers = JsonConvert.DeserializeObject<SavedFavoriteList>(f);
                        if (savedUsers == null) savedUsers = new SavedFavoriteList();
                        LocalFavoriteUsers = savedUsers.users;
                    }
                    catch
                    {
                        LocalFavoriteUsers = new List<string>();
                    }
                }
            }).ConfigureAwait(false);
        }
        public async Task SaveLog()
        {
            await Task.Run(() =>
            {
                Console.WriteLine("Save Favorite File");
                string filePath = Functions.FileCheck(ConfigData.FavoriteListPath);
                var s = new SavedFavoriteList();
                s.users = LocalFavoriteUsers;
                var f = JsonConvert.SerializeObject(s);
                using (StreamWriter sw = new StreamWriter(
                    filePath,
                    false, Encoding.UTF8))
                {
                    sw.Write(f);
                }
            }).ConfigureAwait(false);
        }

        public void AddFavorite(string id)
        {
            if(!LocalFavoriteUsers.Contains(id))
            {
                LocalFavoriteUsers.Add(id);
                SaveLog();
            }
        }

        public void RemoveFavorite(string id)
        {
            if(LocalFavoriteUsers.Contains(id))
            {
                LocalFavoriteUsers.Remove(id);
                SaveLog();
            }
        }
        public FavoriteType GetUserFavorite(string id)
        {
            if (LocalFavoriteUsers.FirstOrDefault(l => l == id) != null)
            {
                return FavoriteType.Local;
            }
            else
            if (APIFavoriteUsers.FirstOrDefault(l => l ==  id) != null)
            {
                return FavoriteType.API;
            }
            else
            {
                return FavoriteType.None;
            }
        }

        public void ToggleFavorite(string id,Action<bool> result=null)
        {
            var current = GetUserFavorite(id);
            switch (current)
            {
                case FavoriteType.Local:
                    RemoveFavorite(id);
                    result?.Invoke(false);
                    break;
                case FavoriteType.None:
                    AddFavorite(id);
                    result?.Invoke(true);
                    break;
                case FavoriteType.API:
                    AddFavorite(id);
                    result?.Invoke(true);
                    break;
            }
        }
    }

    [JsonObject]
    class SavedFavoriteList
    {
        [JsonProperty("users")]
        public List<string> users = new List<string>();
    }
}
