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
using System.Text;

namespace TicTacToeAppWarp
{
    public class RoomReqListener : com.shephertz.app42.gaming.multiplayer.client.listener.RoomRequestListener
    {
        private JoinPage _page;

        public RoomReqListener(JoinPage page)
        {
            _page = page;
        }

        public void onSubscribeRoomDone(RoomEvent eventObj)
        {
            if (eventObj.getResult() == WarpResponseResultCode.SUCCESS)
            {
                WarpClient.GetInstance().GetLiveRoomInfo(GlobalContext.GameRoomId);

            }
        }

        public void onUnSubscribeRoomDone(RoomEvent eventObj)
        {
            if (eventObj.getResult() == WarpResponseResultCode.SUCCESS)
            {
                _page.showResult("Yay! UnSubscribe room :)");
            }
        }

        public void onJoinRoomDone(RoomEvent eventObj)
        {
            if (eventObj.getResult() == WarpResponseResultCode.SUCCESS)
            {
                WarpClient.GetInstance().SubscribeRoom(GlobalContext.GameRoomId);
            }
            else
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    MessageBox.Show("There are alredy 2 user wait for some time");
                });
            }
        }

        public void onLeaveRoomDone(RoomEvent eventObj)
        {
            if (eventObj.getResult() == WarpResponseResultCode.SUCCESS)
            {
                _page.showResult("Yay! Leave room :)");
            }
        }

        public void onGetLiveRoomInfoDone(LiveRoomInfoEvent eventObj)
        {
            if (eventObj.getResult() == WarpResponseResultCode.SUCCESS && (eventObj.getJoinedUsers() != null))
            {
                if (eventObj.getJoinedUsers().Length == 1)
                {

                    GlobalContext.PlayerIsFirst = true;
                }

                else
                {
                    GlobalContext.PlayerIsFirst = false;
                }

                // navigate to game play screen
                _page.moveToPlayScreen();

            }            
        }

        public void onSetCustomRoomDataDone(LiveRoomInfoEvent eventObj)
        {

        }

    }
}
