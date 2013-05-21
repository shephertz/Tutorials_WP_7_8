using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

using com.shephertz.app42.gaming.multiplayer.client;

namespace TicTacToeAppWarp
{
    public partial class JoinPage : BasePage
    {
        public JoinPage()
        {    
            InitializeComponent();

            // Initialize the SDK with your applications credentials that you received
            // after creating the app from http://apphq.shephertz.com
            WarpClient.initialize(GlobalContext.API_KEY, GlobalContext.SECRET_KEY);
            
            // Keep a reference of the SDK singleton handy for later use.
            GlobalContext.warpClient = WarpClient.GetInstance();
        }

        public void showResult(String result)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                MessageBox.Show("Connection Error. Ensure that your keys are correct.");
            });
        }
        /// <summary>
        /// Explicit saving of settings
        /// </summary>
        /// <param name="UserName"></param>
        /// <remarks>Settings are update when the user
        /// click Join.</remarks>
        private void joinButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUserName.Text))
                MessageBox.Show("Please Specifiy user name");
            else
            {
                // Initiate the connection
                // Create and add listener objects to receive callback events for the APIs used
                GlobalContext.conListenObj = new ConnectionListener(this);
                GlobalContext.roomReqListenerObj = new RoomReqListener(this);
                GlobalContext.warpClient.AddConnectionRequestListener(GlobalContext.conListenObj);
                GlobalContext.warpClient.AddRoomRequestListener(GlobalContext.roomReqListenerObj);
                GlobalContext.localUsername = txtUserName.Text;
                WarpClient.GetInstance().Connect(GlobalContext.localUsername);
            }
        }

        internal void moveToPlayScreen()
        {
            Dispatcher.BeginInvoke(() =>
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.RelativeOrAbsolute)));
        }

    }
}