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
using com.shephertz.app42.gaming.multiplayer.client.events;
using com.shephertz.app42.gaming.multiplayer.client.command;
using com.shephertz.app42.gaming.multiplayer.client;

namespace TicTacToeAppWarp
{
    public class ConnectionListener : com.shephertz.app42.gaming.multiplayer.client.listener.ConnectionRequestListener
    {
        private JoinPage _page;

        public ConnectionListener(JoinPage result)
        {
            _page = result;
        }

        public void onConnectDone(ConnectEvent eventObj)
        {
            if (eventObj.getResult() == WarpResponseResultCode.SUCCESS)
            {
                // Successfully connected to the server. Lets go ahead and join the room.
                WarpClient.GetInstance().JoinRoom(GlobalContext.GameRoomId); 
            }
            else
            {
                _page.showResult("connection failed");
            }

        }

        public void onDisconnectDone(ConnectEvent eventObj)
        {
            if (eventObj.getResult() == WarpResponseResultCode.SUCCESS)
            {
                _page.showResult("disconnection success");
            }
            else
            {
                _page.showResult("diconnection failed");
            }
        }
        public void onInitUDPDone(byte resultCode)
        {
            throw new NotImplementedException();
        }
    }
}
