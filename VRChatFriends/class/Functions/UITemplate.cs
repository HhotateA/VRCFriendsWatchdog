using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Mvvm;
using System.Reactive;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using VRChatFriends;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using VRChatFriends.Usecase;
using System.Timers;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using Windows.System;
using VRChatFriends.Function;

namespace VRChatFriends.Function
{
    public class LocationList : ListTemplate
    {
        public LocationList(LocationData data) : base(data)
        {
            Location = data;
        }
        public LocationList(LocationList origin) : base(origin)
        {
            Location = origin?.Location ?? Location;
            for(int i = 0;i< origin.Users.Count;i++)
            {
                if(origin.Users[i]!=null)
                {
                    Users.Add(new UserList(origin.Users[i]));
                }
            }

        }
        public LocationData location = new LocationData("id");
        public LocationData Location
        {
            get => location;
            set
            {
                SetProperty(ref location, value);
            }
        }
        ObservableCollection<UserList> users = new ObservableCollection<UserList>();
        public ObservableCollection<UserList> Users
        {
            get => users;
            set
            {
                Functions.DispatcheFunction(() =>
                {
                    SetProperty(ref users, value);
                });
            }
        }
        public bool IsInit = false;
        Action<LocationData> onFinishInit;
        public void AddOnFinishInit(Action<LocationData> value)
        {
            if (IsInit)
            {
                value(Location);
            }
            onFinishInit += value;
        }
        public void FinishInit(LocationData data)
        {
            if (!IsInit)
            {
                IsInit = true;
                Location = data;
                // Debug.Log("FinishInit" + Location.Id);
                onFinishInit?.Invoke(Location);
            }
        }
    }
    public class UserList : ListTemplate
    {
        public UserList(UserData data) : base(data)
        {
            User = data;
        }
        public UserList(UserList origin) : base(origin)
        {
            User = origin?.User ?? User;
        }
        public UserData user = new UserData("id");
        public UserData User
        {
            get => user;
            set => SetProperty(ref user, value);
        }
        public bool IsInit = false;
        Action<UserData> onFinishInit;
        public void AddOnFinishInit(Action<UserData> value)
        {
            if (IsInit)
            {
                value(User);
            }
            onFinishInit += value;
        }
        public void FinishInit(UserData data)
        {
            if (!IsInit)
            {
                IsInit = true;
                User = data;
                onFinishInit?.Invoke(User);
            }
        }
    }
    public class ListTemplate : BindableBase
    {
        public ListTemplate(DataTemplate data) { }
        public ListTemplate(ListTemplate origin)
        {
            Fav = origin?.Fav ?? Fav;
            LastUpdateTime = origin?.LastUpdateTime ?? LastUpdateTime;
            TimeStamp = origin?.TimeStamp ?? TimeStamp;
            BGColor = origin?.BGColor ?? BGColor;
            ThumbnailURL = origin?.ThumbnailURL ?? ThumbnailURL;
            OnClick = origin?.OnClick ?? OnClick;
        }
        public void UpdateTimeStamp()
        {
            TimeStamp = Functions.TimeStamp;
            lastUpdateTime = DateTime.Now.ToString();
        }
        long sortNumber = 0;
        public long SortNumber
        {
            get => sortNumber;
            set => SetProperty(ref sortNumber, value);
        }
        long timeStamp = 0;
        public long TimeStamp
        {
            get => timeStamp;
            set => SetProperty(ref timeStamp, value);
        }
        string bgColor = "White";
        public string BGColor
        {
            get => bgColor;
            set
            {
                SetProperty(ref bgColor, value);
                OnPropertyChanged(nameof(BGColor));
            }
        }
        public FavoriteType fav = FavoriteType.None;
        public FavoriteType Fav
        {
            get => fav;
            set
            {
                SetProperty(ref fav, value);
                switch(value)
                {
                    case FavoriteType.Local :
                        BGColor = "Red";
                        break;
                    case FavoriteType.API:
                        BGColor = "Yellow";
                        break;
                    case FavoriteType.None:
                        BGColor = "White";
                        break;
                }
            }
        }
        string thumbnailURL = ConfigData.DefaultThumbnailURL;
        public string ThumbnailURL
        {
            get => thumbnailURL;
            set
            {
                if (!String.IsNullOrWhiteSpace(value))
                {
                    SetProperty(ref thumbnailURL, value);
                }
            }
        }
        public List<LocationHistryData> LocationHistryList { get; set; } = new List<LocationHistryData>();
        public void SetLocationHistry(LocationHistryData newLocation)
        {
            LocationHistryList.Insert(0, newLocation);
            if (LocationHistryList.Count > 100)
            {
                LocationHistryList.RemoveRange(99, Math.Max(0, LocationHistryList.Count - 99));
            }
        }
        public Dictionary<string, int> Friends { get; set; }
        string lastUpdateTime = "";
        public string LastUpdateTime
        {
            get => lastUpdateTime;
            set => SetProperty(ref lastUpdateTime, value);
        }
        string capacityUsers = "FRIENDS";
        public string CapacityUsers
        {
            get => capacityUsers;
            set => SetProperty(ref capacityUsers, value);
        }
        ICommand onClick;
        public ICommand OnClick
        {
            get => onClick;
            set => SetProperty(ref onClick, value);
        }
        ICommand onToggleFavorite;
        public ICommand OnClickFavorite
        {
            get => onToggleFavorite;
            set => SetProperty(ref onToggleFavorite, value);
        }
    }
    public class DetailPanelTemplate : BindableBase
    {
        public DetailPanelTemplate() { }
        public DetailPanelTemplate(UserData data)
        {
            Name = data.Name;
            Id = data.Id;
            ThumbnailURL = data.ThumbnailURL;

            Platform = data.Platform;
            Status = data.Status;
            Description = data.StatusDescription;
            Tags = data.Tag;
        }
        public DetailPanelTemplate(DataTemplate data)
        {
            Name = data.Name;
            Id = data.Id;
            ThumbnailURL = data.ThumbnailURL;
        }
        public void SetData(UserData data, long copyStamp = 0)
        {
            if (TimeStamp <= copyStamp || copyStamp == 0)
            {
                Name = data.Name;
                Id = data.Id;
                ThumbnailURL = data.ThumbnailURL;

                Platform = data.Platform;
                Status = data.Status;
                Description = data.StatusDescription;
                Tags = data.Tag;
                TimeStamp = Functions.TimeStamp;
                OnJoinClick = new DelegateCommand(() =>
                {
                    System.Diagnostics.Process.Start(Functions.IdToURL(data.Location));
                });
            }
        }
        public void SetData(LocationData data, long copyStamp = 0)
        {
            if (TimeStamp <= copyStamp || copyStamp == 0)
            {
                Functions.DispatcheFunction(()=>{});
                Name = data.Name;
                Id = data.Id;
                ThumbnailURL = data.ThumbnailURL;
                Platform =  "Owner : " + data.OwnerName + "     (Author: " + data.OutherName + " )";
                Status = data.Status.ToString() + " <= " + data.ReleaseStatus;
                Description = data.Description;
                if (data.Users.Count(l => l.Platform == "standalonewindows") != 0) Platform += "Windows";
                if (data.Users.Count(l => l.Platform == "android") != 0) Platform += "Quest";
                Tags = data.Tag;
                Histry = data.Users.Count + " Friends <= " + data.Capacity;

                TimeStamp = Functions.TimeStamp;
                HistryDetail = new ObservableCollection<string>();
                Users = new ObservableCollection<string>();
                var usersList = new List<string>();
                if(data.Id != "private" && data.Id != "offline")
                {
                    for(int i=0;i<data.Users.Count;i++)
                    {
                        Function.Debug.Log("aa" + data.Users[i].Name);
                        usersList.Add(data.Users[i].Name);
                    }
                    Users = new ObservableCollection<string>(usersList);
                    HistryDetail = new ObservableCollection<string>(data.UserHistry.Select(l=>l.Value.DetailData));
                }

                WatchTitle = "Detail";
                OnClickFavorite = new DelegateCommand(() =>
                {
                    System.Diagnostics.Process.Start(Functions.IdToURLDetail(data.Id));
                });
                OnJoinClick = new DelegateCommand(() =>
                {
                    System.Diagnostics.Process.Start(Functions.IdToURL(data.Id));
                });
            }
        }
        public void SetData(ListTemplate data, long copyStamp = 0)
        {
            if (TimeStamp <= copyStamp || copyStamp == 0)
            {
                Date = data.LastUpdateTime;
                SetLocationHistry(new ObservableCollection<LocationHistryData>(data.LocationHistryList));

                TimeStamp = Functions.TimeStamp;
            }
        }
        public void SetUsers(List<string> data, long copyStamp = 0)
        {
            if (TimeStamp <= copyStamp || copyStamp == 0)
            {
                Users = new ObservableCollection<string>(data);

                TimeStamp = Functions.TimeStamp;
            }
        }
        public string GetLocationHistry(ObservableCollection<LocationHistryData> source)
        {
            string output = "";
            if (source.Count > 0)
            {
                var histrys = source.
                    OrderBy(l => -l.TimeStamp).
                    Select(l=>l.SimpleData).
                    ToList();
                for (int i = 0; i < 5; i++)
                {
                    output += histrys[i];
                    if (i + 1 < histrys.Count)
                    {
                        output += " <= ";
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return output;
        }
        public void SetLocationHistry(ObservableCollection<LocationHistryData> value)
        {
            HistryDetail = new ObservableCollection<string>(value.Select(l => l.DetailData).ToList());
            Histry = GetLocationHistry(value);
        }

        long timeStamp = 0;
        public long TimeStamp
        {
            get => timeStamp;
            set => SetProperty(ref timeStamp, value);
        }

        String name = "NAME";
        public String Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }
        String thumbnailURL = ConfigData.DefaultThumbnailURL;
        public string ThumbnailURL
        {
            get => thumbnailURL;
            set
            {
                if (!String.IsNullOrWhiteSpace(value))
                {
                    if (thumbnailURL != value)
                    {
                        SetProperty(ref thumbnailURL, value);
                    }
                }
                else
                {
                    SetProperty(ref thumbnailURL, ConfigData.DefaultThumbnailURL);
                }
            }
        }
        string id = "ID";
        public string Id
        {
            get => id;
            set => SetProperty(ref id, value);
        }
        string platform = "PLATFORM";
        public string Platform
        {
            get => platform;
            set => SetProperty(ref platform, value);
        }
        string description = "COMMENT";
        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }
        string status = "STATUS";
        public string Status
        {
            get => status;
            set => SetProperty(ref status, value);
        }
        string tags = "TAGS";
        public string Tags
        {
            get => tags;
            set => SetProperty(ref tags, value);
        }
        string histry = "HISTRY";
        public string Histry
        {
            get => histry;
            set => SetProperty(ref histry, value);
        }
        string date = "UPDATEDATE";
        public string Date
        {
            get => date;
            set => SetProperty(ref date, value);
        }
        ObservableCollection<string> users;
        public ObservableCollection<string> Users
        {
            get => users;
            set
            {
                Functions.DispatcheFunction(() =>
                {
                    SetProperty(ref users, value);
                });
            }
        }
        ObservableCollection<string> histryDetail;
        public ObservableCollection<string> HistryDetail
        {
            get => histryDetail;
            set
            {
                Functions.DispatcheFunction(() =>
                {
                    SetProperty(ref histryDetail, value);
                });
            }
        }

        WeeksFootprint footprint = new WeeksFootprint(false);

        public WeeksFootprint Footprint
        {
            get => footprint;
            set => SetProperty(ref footprint, value);
        } 

        ICommand onClickFavorite;
        public ICommand OnClickFavorite
        {
            get => onClickFavorite;
            set => SetProperty(ref onClickFavorite, value);
        }
        ICommand onJoinClick;
        public ICommand OnJoinClick
        {
            get => onJoinClick;
            set => SetProperty(ref onJoinClick, value);
        }
        string watchTitle = "Watch";
        public string WatchTitle
        {
            get => watchTitle;
            set => SetProperty(ref watchTitle, value);
        }
        string joinTitle = "Join";

        public string JoinTitle
        {
            get => joinTitle;
            set => SetProperty(ref joinTitle, value);
        }
    }

    public class LocationHistryData
    {
        public long TimeStamp { get;set; }
        public string UpdateTime { get; set; }
        public string Name { get; set; }
        public string ID { get; set; }

        public LocationHistryData(LocationData data)
        {
            TimeStamp = Functions.TimeStamp;
            UpdateTime = Functions.TimeString;
            Name = data.Name;
            ID = data.Id;
        }

        public string SimpleData
        {
            get => ToString();
        }
        public string DetailData
        {
            get => ToStringDetail();
        }

        public static implicit operator string(LocationHistryData data)
        {
            return data.DetailData;
        }

        string ToString()
        {
            return Name + " (" + UpdateTime + ")";
        }
        string ToStringDetail()
        {
            return UpdateTime + " , " + Name + " , " + ID;
        }
    }
}
