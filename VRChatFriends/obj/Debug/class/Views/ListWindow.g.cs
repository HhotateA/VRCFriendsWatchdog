﻿#pragma checksum "..\..\..\..\class\Views\ListWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "1A4BC8AFF27A3F0CA961300280A828180B54247A14F98E9EBAF857A9F1824699"
//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.42000
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------

using Hardcodet.Wpf.TaskbarNotification;
using Prism.Interactivity;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Regions.Behaviors;
using Prism.Services.Dialogs;
using Prism.Unity;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace VRChatFriends.Views {
    
    
    /// <summary>
    /// ListWindow
    /// </summary>
    public partial class ListWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 16 "..\..\..\..\class\Views\ListWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem OpenMenu;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\..\..\class\Views\ListWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem ExitMenu;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\..\..\class\Views\ListWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button reload;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\..\..\class\Views\ListWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button logout;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\..\..\class\Views\ListWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox KeywordBox;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\..\..\class\Views\ListWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button serch;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\..\..\class\Views\ListWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border UpdateData;
        
        #line default
        #line hidden
        
        
        #line 44 "..\..\..\..\class\Views\ListWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox LocationList;
        
        #line default
        #line hidden
        
        
        #line 95 "..\..\..\..\class\Views\ListWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Fav;
        
        #line default
        #line hidden
        
        
        #line 96 "..\..\..\..\class\Views\ListWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Join;
        
        #line default
        #line hidden
        
        
        #line 162 "..\..\..\..\class\Views\ListWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Log;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/VRChatFriends;component/class/views/listwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\class\Views\ListWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 9 "..\..\..\..\class\Views\ListWindow.xaml"
            ((VRChatFriends.Views.ListWindow)(target)).Closing += new System.ComponentModel.CancelEventHandler(this.ClosingWindow);
            
            #line default
            #line hidden
            
            #line 10 "..\..\..\..\class\Views\ListWindow.xaml"
            ((VRChatFriends.Views.ListWindow)(target)).SizeChanged += new System.Windows.SizeChangedEventHandler(this.ListWindow_SizeChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            this.OpenMenu = ((System.Windows.Controls.MenuItem)(target));
            
            #line 16 "..\..\..\..\class\Views\ListWindow.xaml"
            this.OpenMenu.Click += new System.Windows.RoutedEventHandler(this.OpenApp);
            
            #line default
            #line hidden
            return;
            case 3:
            this.ExitMenu = ((System.Windows.Controls.MenuItem)(target));
            
            #line 17 "..\..\..\..\class\Views\ListWindow.xaml"
            this.ExitMenu.Click += new System.Windows.RoutedEventHandler(this.ExitApp);
            
            #line default
            #line hidden
            return;
            case 4:
            this.reload = ((System.Windows.Controls.Button)(target));
            return;
            case 5:
            this.logout = ((System.Windows.Controls.Button)(target));
            return;
            case 6:
            this.KeywordBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 28 "..\..\..\..\class\Views\ListWindow.xaml"
            this.KeywordBox.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.KeywordChange);
            
            #line default
            #line hidden
            return;
            case 7:
            this.serch = ((System.Windows.Controls.Button)(target));
            return;
            case 8:
            this.UpdateData = ((System.Windows.Controls.Border)(target));
            return;
            case 9:
            this.LocationList = ((System.Windows.Controls.ListBox)(target));
            return;
            case 10:
            this.Fav = ((System.Windows.Controls.Button)(target));
            return;
            case 11:
            this.Join = ((System.Windows.Controls.Button)(target));
            return;
            case 12:
            this.Log = ((System.Windows.Controls.TextBox)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

