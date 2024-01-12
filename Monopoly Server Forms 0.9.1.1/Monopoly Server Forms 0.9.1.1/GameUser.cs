using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly_Server_Forms_0._9._1._1
{
    public class GameUser
    {
        string username;
        string figure;
        double score;
        string ip;
        string code;
        string rank;
        string custom_prefix;
        string custom_color;
        string show_console;


        Socket socket;
        bool hasTurn = false;
        bool bankrupt = false;
        bool hasLeft = false;

        public GameUser(Socket socket, string username)
        {
            this.username = username;
            this.socket = socket;
        }

        public GameUser(Socket socket)
        {
            this.socket = socket;
        }

        public void Send(string text)
        {
            text = "game:" + text;
            byte[] data = Encoding.ASCII.GetBytes(text);
            try
            {

                socket.Send(data);
            }
            catch (Exception)
            {

            }
        }

        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        public string Figure
        {
            get { return figure; }
            set { figure = value; }
        }

        public string IP
        {
            get { return ip; }
            set { ip = value; }
        }

        public string Code
        {
            get { return code; }
            set { code = value; }
        }

        public double Score
        {
            get { return score; }
            set { score = value; }
        }

        public string Rank
        {
            get { return rank; }
            set { rank = value; }
        }

        public string Prefix
        {
            get { return custom_prefix; }
            set { custom_prefix = value; }
        }

        public string PrefixColor
        {
            get { return custom_color; }
            set { custom_color = value; }
        }

        public string ShowConsole
        {
            get { return show_console; }
            set { show_console = value; }
        }

        public Socket Socket
        {
            get { return socket; }
            set { socket = value; }
        }

        public bool Turn
        {
            get { return hasTurn; }
            set { hasTurn = value; }
        }

        public bool Bankrupt
        {
            get { return bankrupt; }
            set { bankrupt = value; }
        }

        public bool Left
        {
            get { return hasLeft; }
            set { hasLeft = value; }
        }
    }
}
