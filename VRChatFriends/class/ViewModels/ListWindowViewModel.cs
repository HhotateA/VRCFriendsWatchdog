﻿using Prism.Commands;
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
using VRChatFriends.Function;
using System.ComponentModel;

namespace VRChatFriends.ViewModels
{
    public class ListWindowViewModel : BindableBase
    {
        Watchdog watchdog;
        NotificationManager notification;
        FavoriteList favoriteList;
        public ListWindowViewModel()
        {
            Initialize();
        }
        async Task Initialize()
        {
            notification = new NotificationManager();
            favoriteList = new FavoriteList(false);
            watchdog = new Watchdog();
            Debug.OnCatchLog += (l) =>
            {
                Logs = l;
            };
            await favoriteList.InitializeFavoriteList().ConfigureAwait(false);
            watchdog.OnAddUser += (l,u)=>
            {
                try
                {
                    OnAddUser(l,u);
                }
                catch (Exception e)
                {
                    Debug.Log(e.ToString());
                    Debug.Log("Listing Error... Please Relogin");
                    throw;
                }
            };
            watchdog.OnUpdateLocation += OnUpdateLocation;
            watchdog.OnLostUser += OnLostUser;
            watchdog.OnUpdateUser += OnUpdateUser;
            watchdog.OnLogout += OpenLoginDialog;
            isReloading = true;
            watchdog.OnUpdateFinish += ListFilterUpdate;
            await watchdog.InitializeUserList(() =>
            {
                isReloading = false;
                watchdog.StartWatchdog();
            }).ConfigureAwait(false);
            OnReloadClick = new DelegateCommand(() =>
            {
                if (isReloading == false)
                {
                    isReloading = true;
                    watchdog.InitializeUserList(() =>
                    {
                        isReloading = false;
                    });
                }
            });
            OnLogoutClick = new DelegateCommand(() =>
            {
                ConfigData.UserName = "";
                ConfigData.Password = "";
                OpenLoginDialog?.Invoke();
            });
            OnTaskIconClick = new DelegateCommand(() =>
            {
                OpenWindow?.Invoke();
            });
        }
        public Action OpenLoginDialog { get;set; }
        public Action OpenWindow { get; set; }

        string logs;
        public string Logs
        {
            get => logs;
            set => SetProperty(ref logs, value);
        }

        public enum SortType
        {
            Timeline,
            Hot,
            Favorite,
            Sesrch,
            None,
        }
        public List<SortType> SortTypes { get; } = new List<SortType>((SortType[])Enum.GetValues(typeof(SortType)));
        SortType sortFilter = SortType.Hot;
        public SortType SortFilter
        {
            get => sortFilter;
            set
            {
                SetProperty(ref sortFilter, value);
                ListFilterUpdate();
            }
        }

        public void ListFilterUpdate()
        {
            FilterdLocationList = ListFilterilter(locations, SortFilter);
        }

        ObservableCollection<LocationList> locations = new ObservableCollection<LocationList>();
        public ObservableCollection<LocationList> Locations
        {
            get
            {
                return locations;
            }
            set
            {
                CountUser(value);
                SetProperty(ref locations, value);
            }
        }

        void CountUser(ObservableCollection<LocationList> locations)
        {
            UserCount = "Get : " + locations.Sum(l => l.Users.Count);
            OnlineCount = "Online : " + locations.Where(l => l.Location.Id != "offline").Sum(l => l.Users.Count);
            LocationCount = "Location : " + locations.Count;
        }
        string onlineCount;
        public string OnlineCount
        {
            get => onlineCount;
            set => SetProperty(ref onlineCount, value);
        }
        string userCount;
        public string UserCount
        {
            get => userCount;
            set => SetProperty(ref userCount, value);
        }
        string locationCount;
        public string LocationCount
        {
            get => locationCount;
            set => SetProperty(ref locationCount, value);
        }
        string filterKeyword = "";
        public string FilterKeyword
        {
            get => filterKeyword;
            set
            {
                SetProperty(ref filterKeyword, value);
                ListFilterUpdate();
            }
        }
        bool isReloading = true;
        bool IsReloading
        {
            get => isReloading;
            set
            {
                isReloading = value;
                reloadButton = value ? "Reloading..." : "Reload";
            }
        }
        string reloadButton = "Reloading...";
        public string ReloadButton
        {
            get => reloadButton;
            set => SetProperty(ref reloadButton, value);
        }
        string logTaskbar = "Majin Neko Watching YOU...";
        public string LogTaskbar
        {
            get => logTaskbar;
            set => SetProperty(ref logTaskbar, value);
        }
        
       ICommand onReloadClick;
        public ICommand OnReloadClick
        {
            get => onReloadClick;
            set => SetProperty(ref onReloadClick, value);
        }
        ICommand onFilterKeywordChange;
        public ICommand OnFilterKeywordChange
        {
            get => onFilterKeywordChange;
            set => SetProperty(ref onFilterKeywordChange, value);
        }
        ICommand onLogoutClick;
        public ICommand OnLogoutClick
        {
            get => onLogoutClick;
            set => SetProperty(ref onLogoutClick, value);
        }
        ICommand onTaskIconClick;
        public ICommand OnTaskIconClick
        {
            get => onTaskIconClick;
            set => SetProperty(ref onTaskIconClick, value);
        }
        ObservableCollection<LocationList> filterdLocationList = new ObservableCollection<LocationList>();
        public ObservableCollection<LocationList> FilterdLocationList
        {
            get => filterdLocationList;
            set => SetProperty(ref filterdLocationList,value);
        }
        DetailPanelTemplate detailPanel = new DetailPanelTemplate();
        public DetailPanelTemplate DetailPanel
        {
            get => detailPanel;
            set => SetProperty(ref detailPanel, value);
        }
        string logText;
        public string LogText
        {
            get => logText;
            set => SetProperty(ref logText, value);
        }

        void OnAddUser (LocationData location,UserData user)
        {
            // ロケーションパネル取得
            LocationList locationPanel = null;
            for (int i = 0; i < Locations.Count; i++)
            {
                if (Locations[i].Location.Id == location.Id)
                {
                    locationPanel = Locations[i];
                    break;
                }
            }
            UserList userPanel = SerchUser(user.Id);
            LocationList oldLocationPanel = SerchUserLocation(user.Id);
            var oldLocationId = oldLocationPanel?.Location.Id ?? "NULL_ID";
            if (oldLocationId != location.Id)
            {
                if (locationPanel == null)
                {
                    locationPanel = new LocationList(location);
                    Debug.Log("Initialize Location : " + location.Id);
                    locationPanel.OnClick = new DelegateCommand(() =>
                    {
                        Debug.Log("Click => " + location.Id);
                        DetailPanel.SetData(location, Functions.TimeStamp);
                        DetailPanel.Footprint = new WeeksFootprint(false);
                    });
                    Locations.Add(locationPanel);
                    if (location.Id == "offline" || location.Id == "private")
                    {
                        locationPanel.FinishInit(location);
                    }
                }

                // ユーザーパネル取得
                if (userPanel == null)
                {
                    userPanel = new UserList(user);
                    Debug.Log("Initialize User : " + user.Id);
                    userPanel.Fav = favoriteList.GetUserFavorite(user.Id);
                    userPanel.ThumbnailURL = user.ThumbnailURL;
                    userPanel.OnClick = new DelegateCommand(() =>
                    {
                        Debug.Log("Click => " + user.Id);
                        var copyStamp = Functions.TimeStamp;
                        DetailPanel.SetData(user, copyStamp);
                        DetailPanel.SetData(userPanel, copyStamp);
                        watchdog.UserDetail(user.Id, (u) =>
                        {
                            DetailPanel.SetData(u, copyStamp);
                            userPanel.User = u;
                        });
                        DetailPanel.SetUsers(watchdog.GetDatabase(user.Id), copyStamp);
                        DetailPanel.OnClickFavorite = new DelegateCommand(() =>
                            favoriteList.ToggleFavorite(user.Id, (e) =>
                            {
                                userPanel.Fav = favoriteList.GetUserFavorite(user.Id);
                                ListFilterUpdate();
                            }
                        ));
                        DetailPanel.Footprint = watchdog.GetFootPrint(user.Id);
                    });
                }
                else
                {
                    oldLocationPanel.Users.Remove(userPanel);
                }
                userPanel.UpdateTimeStamp();
                // アバターサムネイルの変更検知のため，ここでもサムネイル取得
                userPanel.ThumbnailURL = user.ThumbnailURL;
                locationPanel.Users.Insert(0, userPanel);
                locationPanel.AddOnFinishInit((l) =>
                {
                    LogText = (user.Name + " join to " + l.Name);
                    if(l.Id!="offline"&&l.Id!="private")
                    {
                        LogText += " <" + Functions.IdToURL(l.Id) + ">";
                    }
                    notification.Log(LogText);
                    if (userPanel.Fav == FavoriteType.Local)
                    {
                        notification.Notification(user.Name,l.Name,LogText);
                    }
                    userPanel.SetLocationHistry(new LocationHistryData(l));
                });

                locationPanel.UpdateTimeStamp();
                // リスト上位に追加
                Locations = new ObservableCollection<LocationList>(
                    Locations.OrderBy(l => -l.TimeStamp)
                    .Where(l => l.Users.Count != 0)
                    .ToList());
            }
            // リストをフィルター
            if (user.Location != "offline" && user.Location != "private")
            {
                ListFilterUpdate();
            }
        }
        void OnLostUser (UserData user)
        {
            Debug.Log(user.Id + " is logout");
        }

        void OnUpdateUser (UserData user)
        {
            Debug.Log(user.Id + " is ChangeAvatar");
            var userPanel = SerchUser(user.Id);
            if(userPanel!=null)
            {
                userPanel.ThumbnailURL = user.ThumbnailURL;
                userPanel.User.ThumbnailURL = user.ThumbnailURL;
            }
        }
        void OnUpdateLocation(LocationData location)
        {
            LocationList panel = null;
            for(int i=0;i<Locations.Count;i++)
            {
                if(Locations[i].Location.Id==location.Id)
                {
                    panel = Locations[i];
                }
            }
            if (panel != null)
            {
                panel.FinishInit(location);
                panel.ThumbnailURL = location.ThumbnailURL;
            }
        }

        ObservableCollection<LocationList> ListFilterilter(ObservableCollection<LocationList> origin, SortType sort)
        {
            Debug.Log("List Update => Sorting by : " + sort);
            if (sort == SortType.None) return origin;
            try
            {
                var clone = new List<LocationList>();
                for(int i = 0; i<origin.Count;i++)
                {
                    if(origin[i]!=null)
                    {
                        var llist = new LocationList(origin[i]);
                        llist.Users.Clear();
                        for (int j = 0; j < origin[i].Users.Count; j++)
                        {
                            if(origin[i].Users!=null)
                            {
                                var uList = new UserList(origin[i].Users[j]);
                                llist.Users.Add(uList);
                            }
                        }
                        clone.Add(new LocationList(llist));
                    }
                }
                if(!String.IsNullOrWhiteSpace(FilterKeyword))
                {
                    for (int i = 0; i < clone.Count; i++)
                    {
                        var users = clone[i].Users.
                            Where(l => l.User.Name.ToUpper().Contains(FilterKeyword.ToUpper())).
                            ToList();
                        clone[i].Users = new ObservableCollection<UserList>(users);

                        if (clone[i].Location.Id == "offline" || clone[i].Location.Id == "private")
                        {
                            clone[i].SortNumber = int.MaxValue / 2;
                        }
                    }
                    clone = clone.Where(l => l.Users.Count != 0).ToList();
                    for(int j=0;j<clone.Count;j++)
                    {
                        clone[j].SortNumber = clone[j].Users.Count == 0 ?
                            int.MaxValue / 2 :
                            clone[j].Users.Max(l => l.TimeStamp);
                    }
                }

                switch (sort)
                {
                    case SortType.Hot:
                        {
                            for (int i = 0; i < clone.Count; i++)
                            {
                                if (clone[i].Location.Id == "private")
                                {
                                    clone[i].SortNumber = int.MaxValue / 4;
                                }
                                else
                                if (clone[i].Location.Id == "offline")
                                {
                                    clone[i].SortNumber = int.MaxValue / 3;
                                }
                                else
                                if (clone[i].Users.Count == 0)
                                {
                                    clone[i].SortNumber = int.MaxValue / 2;
                                }
                                else
                                {
                                    clone[i].SortNumber = -clone[i].Users?.Count ?? int.MaxValue / 2;
                                }
                            };
                            break;
                        }
                    case SortType.Sesrch:
                        {
                            break;
                        }
                    case SortType.Timeline:
                        {
                            for (int i = 0; i < clone.Count; i++)
                            {
                                if (clone[i].Location.Id == "private")
                                {
                                    clone[i].SortNumber = int.MaxValue / 4;
                                }
                                else
                                if (clone[i].Location.Id == "offline")
                                {
                                    clone[i].SortNumber = int.MaxValue / 3;
                                }
                                else
                                if (clone[i].Users.Count == 0)
                                {
                                    clone[i].SortNumber = int.MaxValue / 2;
                                }
                                else
                                {
                                    clone[i].SortNumber = -clone[i].Users?.Max(u => u.TimeStamp) ?? int.MaxValue / 2;
                                }
                            };
                            break;
                        }
                    case SortType.Favorite:
                        {
                            for(int i = 0; i<clone.Count; i++)
                            {
                                if (clone[i].Location.Id == "private")
                                {
                                    clone[i].SortNumber = int.MaxValue / 4;
                                }
                                else
                                if (clone[i].Location.Id == "offline")
                                {
                                    clone[i].SortNumber = int.MaxValue / 3;
                                }
                                else
                                if (clone[i].Users.Count == 0)
                                {
                                    clone[i].SortNumber = int.MaxValue / 2;
                                }
                                else
                                {
                                    clone[i].SortNumber = clone[i].Users?.Sum(u => -(int)u.Fav) ?? int.MaxValue / 2;
                                }
                            }
                            break;
                        }
                }

                for(int i = 0;i<clone.Count;i++)
                {
                    if (clone[i].Users.Count > ConfigData.MaxUserCell)
                    {
                        var u = clone[i].Users.ToList();
                        u.RemoveRange(ConfigData.MaxUserCell, u.Count - ConfigData.MaxUserCell);
                        clone[i].Users = new ObservableCollection<UserList>(u);
                    }
                }
                return new ObservableCollection<LocationList>(clone.OrderBy(l => l.SortNumber));
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
                Debug.Log("Sorting Erorr... Please Relogin");
                throw;
            }
        }

        UserList SerchUser(string id)
        {
            for(int i=0;i<Locations.Count;i++)
            {
                if(Locations[i].Users!=null)
                {
                    for (int j = 0; j < Locations[i].Users.Count; j++)
                    {
                        if (Locations[i].Users[j]?.User != null)
                        {
                            if (Locations[i].Users[j].User.Id == id)
                            {
                                return Locations[i].Users[j];
                            }
                        }
                    }
                }
            }
            return null;
        }
        LocationList SerchUserLocation(string id)
        {
            for (int i = 0; i < Locations.Count; i++)
            {
                if (Locations[i].Users != null)
                {
                    for (int j = 0; j < Locations[i].Users.Count; j++)
                    {
                        if (Locations[i].Users[j]?.User != null)
                        {
                            if (Locations[i].Users[j].User.Id == id)
                            {
                                return Locations[i];
                            }
                        }
                    }
                }
            }
            return null;
        }
        void SerchLocation(string id,ref LocationList location,ref UserList user)
        {
            for(int i=0;i<Locations.Count;i++)
            {
                for(int j=0;j<Locations[i].Users.Count;j++)
                {
                    if(Locations[i].Users[j].User.Id == id)
                    {
                        location = Locations[i];
                        user = Locations[i].Users[j];
                    }
                }
            }
            location = null;
            user = null;
        }
    }
}