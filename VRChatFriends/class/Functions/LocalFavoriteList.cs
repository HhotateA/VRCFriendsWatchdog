using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VRChatFriends.Function
{
    class LocalFavoriteList
    {
    }
    [JsonObject]
    class SavedFavoriteList
    {
        [JsonProperty("token")]
        List<string> Users;
    }
}
