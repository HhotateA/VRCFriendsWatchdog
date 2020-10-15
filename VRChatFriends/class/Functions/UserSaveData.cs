using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VRChatFriends.Function;

namespace VRChatFriends
{
    [JsonObject]
    class UsersSaveData
    {
        [JsonProperty("users")] public List<UserSaveData> users = new List<UserSaveData>();
    }

    [JsonObject]
    class UserSaveData
    {
        [JsonProperty("id")] public string id = "";
        [JsonProperty("name")] public string name = "";
        [JsonProperty("lastUpdate")] public string lastUpdate;
        [JsonProperty("friends")] public List<FriendUserSaveData> friends = new List<FriendUserSaveData>();
        [JsonProperty("footPrint")] public WeeksFootprint footprint = new WeeksFootprint();
    }

    [JsonObject]
    class LocationSaveData
    {
        [JsonProperty("id")] public string id = "";
        [JsonProperty("name")] public string name = "";
        [JsonProperty("lastUpdate")] public string lastUpdate;
        [JsonProperty("users")] public List<LocationUserSaveData> users = new List<LocationUserSaveData>();
    }

    [JsonObject]
    class LocationUserSaveData
    {
        [JsonProperty("id")] public string id = "";
        [JsonProperty("name")] public string name = "";
    }

    [JsonObject]
    class FriendUserSaveData
    {
        [JsonProperty("id")] public string id = "";
        [JsonProperty("name")] public string name = "";
        [JsonProperty("lastUpdate")] public string lastUpdate = "";
        [JsonProperty("count")] public int count = 0;
    }

    [JsonObject]
    public class WeeksFootprint
    {
        [JsonProperty("weeks")]
        DaysFootprint[] weeks = new DaysFootprint[7];

        [JsonIgnore]
        public DaysFootprint[] Weeks
        {
            get => weeks;
            set => weeks = value;
        }

        public WeeksFootprint(bool initialize = true)
        {
            var w = new string[7]{"Sun","Mon","Tue","Wed","Thu","Fri","Sat"};
            if (initialize && ConfigData.Heatmap==true)
            {
                for (int i = 0; i < Weeks.Length; i++)
                {
                    Weeks[i] = new DaysFootprint(w[i]);
                }
            }
            else
            {
                weeks = Array.Empty<DaysFootprint>();
            }
        }

        public void CountupFootorint(int week,int hour,LocationType type,int c = 1)
        {
            if (Weeks.Length != 0)
            {
                weeks[week].Days[hour].CountFootprint(type,c);
            }
        }
        public void InitializeColor()
        {
            var w = new string[7]{"Sun","Mon","Tue","Wed","Thu","Fri","Sat"};
            if (ConfigData.Heatmap==true)
            {
                for (int i = 0; i < Weeks.Length; i++)
                {
                    Weeks[i].InitializeColor(w[i]);
                }
            }
        }
    }

    [JsonObject]
    public class DaysFootprint
    {
        [JsonProperty("days")]
        UserFootprint[] days = new UserFootprint[24];

        [JsonIgnore]
        public UserFootprint[] Days
        {
            get => days;
            set => days = value;
        }

        public DaysFootprint(string t="")
        {
            for (int i = 0; i < days.Length; i++)
            {
                Days[i] = new UserFootprint(t+":"+i);
            }
        }
        public void InitializeColor(string t = "")
        {
            for (int i = 0; i < Days.Length; i++)
            {
                Days[i].InitializeColor(t + Environment.NewLine
                                         + i + " H");
            }
        }
    }

    public class UserFootprint
    {
        [JsonProperty("hours")]
        int[] footprint = new int[4];

        public void CountFootprint(LocationType type, int c = 1)
        {
            if (type == LocationType.Public)
            {
                footprint[0] += c;
            }
            else
            if (type == LocationType.Friends || type == LocationType.FriendPlus)
            {
                footprint[1] += c;
            }
            else
            if (type == LocationType.Private || type == LocationType.Invite || type == LocationType.InvitePlus)
            {
                footprint[2] += c;
            }
            else
            if(type == LocationType.Offline)
            {
                footprint[3] += c;
            }
        }

        public UserFootprint(string t = "")
        {
            for (int i = 0; i < footprint.Length; i++)
            {
                footprint[i] = 0;
            }
            Title = t + Environment.NewLine
                     + OnlineScore() + "%";
        }
        public void InitializeColor(string t = "")
        {
            Title = t + Environment.NewLine
                      + OnlineScore() + "%";
        }

        [JsonIgnore] public string Title { get; set; } = "";

        public int OnlineScore()
        {
            if (OnlineCount() == 0 || SumCount() == 0) return 0;
            return (100 * OnlineCount()) / SumCount();
        }

        public int PublicCount()
        {
            return footprint[0];
        }
        public int HideCount()
        {
            return footprint[1];
        }
        public int PrivateCount()
        {
            return footprint[2];
        }

        public int OnlineCount()
        {
            return PublicCount() + HideCount() + PrivateCount();
        }

        public int OfflineCount()
        {
            return footprint[3];
        }

        public int SumCount()
        {
            return OnlineCount() + OfflineCount();
        }

        [JsonIgnore]
        public string HeatColor
        {
            get
            {
                if (SumCount() == 0) return "#FFFFFF";
                int r = (255 * PublicCount()) / SumCount();
                int g = (255 * HideCount()) / SumCount();
                int b = (255 * PrivateCount()) / SumCount();
                
                return "#"
                       +r.ToString("x2").ToUpper()
                       +g.ToString("x2").ToUpper()
                       +b.ToString("x2").ToUpper();
            }
        }
    }
}
