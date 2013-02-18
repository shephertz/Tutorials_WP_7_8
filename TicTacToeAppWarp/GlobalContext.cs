//Testing Again CC
using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using com.shephertz.app42.gaming.multiplayer.client;

namespace TicTacToeAppWarp
{
    public class GlobalContext
    {
        public static String localUsername;

        public static String API_KEY = "Put your API Key here";
        public static String SECRET_KEY = "Put your Secret Key here";

        // Game room id used in this tutorial. 
        // NOTE* replace with your room's id that you created from 
        // App HQ dashboard (http://apphq.shephertz.com).
        public static String GameRoomId = "1393219536";
        
        internal static bool PlayerIsFirst = false;

        public static WarpClient warpClient;
        public static ConnectionListener conListenObj;
        public static RoomReqListener roomReqListenerObj;
        public static NotificationListener notificationListenerObj;
    }
}
