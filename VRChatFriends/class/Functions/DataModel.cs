using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace VRChatFriends
{
    public class LocationData : DataTemplate
    {
        public string WorldID { get; set; }
        public string InstanceID { get; set; }
        public List<UserData> Users { get; set; } = new List<UserData>();
        // <id,<name,count>>
        public Dictionary<string,UserFootprints> UserHistry { get; set; } = new Dictionary<string, UserFootprints>();
        public Action OnUpdate;
        public string OwnerId { get; set; }
        public string OwnerName { get; set; }
        public string OutherName { get; set; }
        public string Description { get; set; }
        public string Capacity { get; set; }
        public string ReleaseStatus { get; set; }

        public LocationType Status
        {
            get
            {
                if(Id == "private")
                {
                    return LocationType.Private;
                }
                else
                if(Id == "offline")
                {
                    return LocationType.Offline;
                }
                else
                if (!Id.Contains('~'))
                {
                    return LocationType.Public;
                }
                else
                if(Id.Contains("hidden"))
                {
                    return LocationType.FriendPlus;
                }
                else
                if (Id.Contains("friends"))
                {
                    return  LocationType.Friends;
                }
                else 
                if(Tag.Contains("canRequestInvite"))
                {
                    return LocationType.InvitePlus;
                }
                else
                if(Tag.Contains("private"))
                {
                    return LocationType.Invite;
                }
                else
                {
                    return LocationType.Null;
                }
            }
        }

        public LocationData(string id)
        {
            var world = id.Split(':');
            WorldID = world[0];
            if (world.Length > 1)
            {
                var instance = world[1].Split('~');
                InstanceID = instance[0];
                if (instance.Length > 1)
                {
                    var a = instance[1].Split('(');
                    foreach (var b in a)
                    {
                        var c = b.Split(')');
                        foreach (var d in c)
                        {
                            if (d.StartsWith("usr_"))
                            {
                                OwnerId = d;
                            }
                        }
                    }
                }
            }
            base.Id = id;
        }

        public LocationData(LocationData origin)
        {
            SetData(origin);
        }
        public void SetData(LocationData origin)
        {
            WorldID = origin.WorldID;
            InstanceID = origin.InstanceID;
            Users = origin.Users;
            UserHistry = origin.UserHistry;
            OwnerId = origin.OwnerId;
            OwnerName = origin.OwnerName;
            OnUpdate = origin.OnUpdate;
            Description = origin.Description;
            OutherName = origin.OutherName;
            Capacity = origin.Capacity;
            ReleaseStatus = origin.ReleaseStatus;
            base.SetData(origin);
        }
        public void SetStructData(LocationData origin)
        {
            WorldID = origin.WorldID;
            InstanceID = origin.InstanceID;
            OwnerId = origin.OwnerId;
            OwnerName = origin.OwnerName;
            Description = origin.Description;
            OutherName = origin.OutherName;
            Capacity = origin.Capacity;
            ReleaseStatus = origin.ReleaseStatus;
            base.SetStructData(origin);
        }
    }
    public class UserFootprints
    {
        public string Id { get; set; } = "";
        public string Name { get; private set; } = "";
        public int Count { get; set; } = 0;

        public string DetailData
        {
            get
            {
                return ToString();
            }
        }
        public override string ToString()
        {
            return (Count+59)/60 + "min" + " : " + Name + " : " + Id;
        }

        public static implicit operator string(UserFootprints origin)
        {
            return origin?.ToString() ?? "";
        }
        public UserFootprints(string name,string id = null)
        {
            Id = id;
            Name = name;
            Count = 0;
        }
    }

    public enum LocationType
    {
        Public,
        FriendPlus,
        Friends,
        Invite,
        InvitePlus,
        Private,
        Offline,
        Null,
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
            SetData(origin);
        }
        public void SetData(DataTemplate origin)
        {
            Id = origin.Id;
            Name = origin.Name;
            ThumbnailURL = origin.ThumbnailURL;
            Tag = origin.Tag;
            OnInitializeFinish = origin.OnInitializeFinish;
        }
        public void SetStructData(DataTemplate origin)
        {
            Id = origin.Id;
            Name = origin.Name;
            ThumbnailURL = origin.ThumbnailURL;
            Tag = origin.Tag;
        }
    }
}
