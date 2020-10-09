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
        /*
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
        */
        public Action AppShutdown { get; set; }
        private void KeywordChange(object sender, RoutedEventArgs e)
        {
            OnKeywordChange?.Invoke(KeywordBox.Text);
        }
        public Action<string> OnKeywordChange { get; set; }

    }
}
