using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VRChatFriends
{
    public class LocationData : DataTemplate
    {
        public string WorldID { get; set; }
        public string InstanceID { get; set; }
        public List<UserData> Users { get; set; } = new List<UserData>();
        public Action OnUpdate;

        public LocationData(string id)
        {
            var world = id.Split(':');
            WorldID = world[0];
            if (world.Length > 1)
            {
                var instance = world[1].Split('~');
                InstanceID = instance[0];
            }
            base.Id = id;
        }

        public LocationData(LocationData origin) : base(origin)
        {
            WorldID = origin.WorldID;
            InstanceID = origin.InstanceID;
            Users = origin.Users;
            OnUpdate = origin.OnUpdate;
        }
    }

    public class UserData : DataTemplate
    {
        public string Location { get; set; }
        public string UserName { get; set; }
        public string Platform { get; set; }
        public string Status { get; set; }
        public string StatusDescription { get; set; }
        public UserData(string id)
        {
            base.Id = id;
        }

        public UserData(UserData origin) : base(origin)
        {
            Location = origin.Location;
            UserName = origin.UserName;
            Platform = origin.Platform;
            Status = origin.Status;
            StatusDescription = origin.StatusDescription;
        }
    }
    public class DataTemplate
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ThumbnailURL { get; set; }
        public string Tag { get; set; }
        public Action<LocationData> OnInitializeFinish;
        public DataTemplate()
        {
        }

        public DataTemplate(DataTemplate origin)
        {
            Id = origin.Id;
            Name = origin.Name;
            ThumbnailURL = origin.ThumbnailURL;
            Tag = origin.Tag;
            OnInitializeFinish = origin.OnInitializeFinish;
        }
    }
}
