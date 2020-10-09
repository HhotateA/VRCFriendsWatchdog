using System.Windows;
using VRChatFriends;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using VRChatFriends.Usecase;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using VRChatFriends.Function;

namespace VRChatFriends.Views
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }
        private void PasswordChanged(object sender, RoutedEventArgs e)
        {
            OnPasswordChange?.Invoke(PasswordBox.Password);
        }
        public Action<string> OnPasswordChange{get;set;}
    }
}
