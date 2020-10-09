using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VRChatFriends
{
    [JsonObject]
    class UsersSaveData
    {
        [JsonProperty("users")]
        public List<UserSaveData> users = new List<UserSaveData>();
    }
    [JsonObject]
    class UserSaveData
    {
        [JsonProperty("id")]
        public string id = "";
        [JsonProperty("name")]
        public string name = "";
        [JsonProperty("lastUpdate")]
        public string lastUpdate;
        [JsonProperty("friends")]
        public List<FriendUserSaveData> friends = new List<FriendUserSaveData>();
    }
    [JsonObject]
    class LocationSaveData
    {
        [JsonProperty("id")]
        public string id = "";
        [JsonProperty("name")]
        public string name = "";
        [JsonProperty("lastUpdate")]
        public string lastUpdate;
        [JsonProperty("users")]
        public List<LocationUserSaveData> users = new List<LocationUserSaveData>();
    }
    [JsonObject]
    class LocationUserSaveData
    {
        [JsonProperty("id")]
        public string id = "";
        [JsonProperty("name")]
        public string name = "";
    }
    [JsonObject]
    class FriendUserSaveData
    {
        [JsonProperty("id")]
        public string id = "";
        [JsonProperty("name")]
        public string name = "";
        [JsonProperty("lastUpdate")]
        public string lastUpdate = "";
        [JsonProperty("count")]
        public int count = 0;
    }
}
