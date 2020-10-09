using System;
using System.Collections.Generic;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace VRChatFriends.Function
{
    class ThumbnailLoader
    {
        static ThumbnailLoader instance;
        public static ThumbnailLoader Instance 
        {
            get 
            {
                if(instance==null)
                {
                    instance = new ThumbnailLoader();
                }
                return instance;
            } 
        }
        public async void LoadAsync(string url,Action<BitmapImage> result)
        {
            if(!String.IsNullOrWhiteSpace(url))
            {
                var a = new Action( ()=>
                    {
                        var bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.UriSource = new Uri(url);
                        bitmap.EndInit();
                        result?.Invoke(bitmap);
                    }
                );
                await Task.Run(a);
            }
        }
        public BitmapImage Load(string url)
        {
            if (!String.IsNullOrWhiteSpace(url))
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(url);
                bitmap.EndInit();
                return bitmap;
            }
            else
            {
                return new BitmapImage();
            }
        }
    }
}
