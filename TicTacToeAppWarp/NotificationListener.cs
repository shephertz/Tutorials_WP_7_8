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
using System.Linq;
using com.shephertz.app42.gaming.multiplayer.client.events;
using com.shephertz.app42.gaming.multiplayer.client.command;
using com.shephertz.app42.gaming.multiplayer.client;
using System.Windows.Threading;
using System.Collections.Generic;

namespace TicTacToeAppWarp
{
    public class NotificationListener : com.shephertz.app42.gaming.multiplayer.client.listener.NotifyListener
    {
        private MainPage _page;

        public NotificationListener(MainPage page)
        {
            _page = page;
        }

        public void onRoomCreated(RoomData eventObj)
        {
            
        }

        public void onRoomDestroyed(RoomData eventObj)
        {
            
        }

        public void onUserLeftRoom(RoomData eventObj, String username)
        {
            if(GlobalContext.localUsername != username)
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    _page.UpdateStatus("Opponent left the room. You won");
                    GlobalContext.PlayerIsFirst = true;
                    _page.InitializeBoard();
                });
        }

        public void onUserJoinedRoom(RoomData eventObj, String username)
        {
            _page.UpdateStatus(username + " joined " + eventObj.getId());
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                _page.UnLockGameboard();
                _page.UpdateStatus("Click on any block to start Game");

            });

        }

        public void onUserLeftLobby(LobbyData eventObj, String username)
        {
            
        }

        public void onUserJoinedLobby(LobbyData eventObj, String username)
        {
            
        }

        public void onChatReceived(ChatEvent eventObj)
        {
            
        }

        public void onUpdatePeersReceived(UpdateEvent eventObj)
        {

            MoveMessage msg = MoveMessage.buildMessage(eventObj.getUpdate());
            if (msg.type == "new")
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    _page.NewGame();
                });
            }
            else
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    _page.UpdateUiFromMove(msg);
                });
            }
        }

        public void onUserChangeRoomProperty(RoomData roomData, string sender, Dictionary<string, object> properties)
        {

        }


        public void onUserChangeRoomProperty(RoomData roomData, string sender, Dictionary<string, object> properties, Dictionary<string, string> lockedPropertiesTable)
        {
            throw new NotImplementedException();
        }

        public void onPrivateChatReceived(string sender, string message)
        {
            throw new NotImplementedException();
        }

        public void onMoveCompleted(MoveEvent moveEvent)
        {
            throw new NotImplementedException();
        }

        public void onUserPaused(string locid, bool isLobby, string username)
        {
            throw new NotImplementedException();
        }

        public void onUserResumed(string locid, bool isLobby, string username)
        {
            throw new NotImplementedException();
        }

        public void onGameStarted(string sender, string roomId, string nextTurn)
        {
            throw new NotImplementedException();
        }

        public void onGameStopped(string sender, string roomId)
        {
            throw new NotImplementedException();
        }

        public void onPrivateUpdateReceived(string sender, byte[] update, bool fromUdp)
        {
            throw new NotImplementedException();
        }


        public void onNextTurnRequest(string lastTurn)
        {
            throw new NotImplementedException();
        }
    }
}
