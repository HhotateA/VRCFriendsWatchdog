using System.Windows;
using VRChatFriends;
using System.Collections.ObjectModel;
using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Forms.VisualStyles;
using VRChatFriends.Function;

namespace VRChatFriends.Views
{
    /// <summary>
    /// Interaction logic for ListWindow.xaml
    /// </summary>
    public partial class ListWindow : Window
    {
        public ListWindow()
        {
            InitializeComponent();
        }
        private void ClosingWindow(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
        private void OpenApp(object sender, RoutedEventArgs e)
        {
            this.Show();
        }
        private void ExitApp(object sender, RoutedEventArgs e)
        {
            this.Close();
            AppShutdown?.Invoke();
        }
        public Action AppShutdown { get; set; }
        private void ListWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var data = this.FindName("UpdateData") as Border;
            var list = this.FindName("LocationList") as ListBox;
            var log = this.FindName("Log") as TextBox;
            if (e.NewSize.Height> ConfigData.DataWindowHeight)
            {
                data.Visibility = Visibility.Visible;
                data.Height = Double.NaN;
                var size = e.NewSize.Height - (1250 - 350);
                list.Height = Math.Max(size, ConfigData.ListMinHeight);
            }
            else
            {
                data.Visibility = Visibility.Hidden;
                data.Height = 0;
                var size = e.NewSize.Height - (1250 - 450);
                list.Height = Math.Max(size, ConfigData.ListMinHeight);
            }
            var logsize = e.NewSize.Width - (1400 - 100);
            log.Width = Math.Max(0,logsize);
        }
    }
}
