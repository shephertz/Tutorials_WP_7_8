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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TicTacToeAppWarp
{
    /// <summary>
    /// TicTacToe gameplay message class. Objects of this class represent actions of the user
    /// and are used to serialize/deserialize JSON exchanged between users in the room
    /// </summary>
    public class MoveMessage
    {
        public String sender;
        public String TextBoxName;
        public String piece;
        public String type;

        public static MoveMessage buildMessage(byte[] update)
        {
            JObject jsonObj = JObject.Parse(System.Text.Encoding.UTF8.GetString(update, 0, update.Length));
            MoveMessage msg = new MoveMessage();
            msg.sender = jsonObj["sender"].ToString();
            msg.type = jsonObj["type"].ToString();
            if(msg.type == "move")
            {
                msg.TextBoxName = jsonObj["TextBoxName"].ToString();
                msg.piece = jsonObj["piece"].ToString();
            }
            return msg;
        }

        public static byte[] buildMessageBytes(TextBlock tb, String piece)
        {
            JObject moveObj = new JObject();
            moveObj.Add("TextBoxName", tb.Name);
            moveObj.Add("sender", GlobalContext.localUsername);
            moveObj.Add("piece", piece);
            moveObj.Add("type", "move");
            return System.Text.Encoding.UTF8.GetBytes(moveObj.ToString());
        }

        public static byte[] buildNewGameMessageBytes()
        {
            JObject moveObj = new JObject();
            moveObj.Add("sender", GlobalContext.localUsername);
            moveObj.Add("type", "new");
            return System.Text.Encoding.UTF8.GetBytes(moveObj.ToString());
        }

    }
}
