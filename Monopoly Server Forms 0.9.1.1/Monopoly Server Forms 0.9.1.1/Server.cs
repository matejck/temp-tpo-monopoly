using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BC = BCrypt.Net.BCrypt;

namespace Monopoly_Server_Forms_0._9._1._1
{
    public partial class Server : Form
    {
        private static readonly Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //public static readonly List<Socket> clientSockets = new List<Socket>();
        public static readonly List<User> users = new List<User>();
        private const int BUFFER_SIZE = 2048;
        private const int PORT = 6001;
        private static string server_version = "0xZbriR1Ifuv3eouLa0.9.1.1";
        private static readonly byte[] buffer = new byte[BUFFER_SIZE];
        private RichTextBox serveroutput = new RichTextBox();

        public static Dictionary<string, Game> games = new Dictionary<string, Game>();
        /*private static NetworkStream stream = new NetworkStream(serverSocket);
        private static SslStream SSL = new SslStream(stream);

        private void SSLStream()
        {
            
        }*/
        
        public Server()
        {
            InitializeComponent();

            this.Text = "Monopoly login server      Version: " + server_version;
            this.ShowInTaskbar = false;
            this.Controls.Add(serveroutput);

            serveroutput.BorderStyle = BorderStyle.None;
            serveroutput.ForeColor = Color.White;
            serveroutput.BackColor = Color.Black;
            serveroutput.Dock = DockStyle.Fill;
            serveroutput.Font = new Font("Console", 14);
            serveroutput.ReadOnly = true;

            SetupServer();
        }

        private void SetupServer()
        {
            serverSocket.Bind(new IPEndPoint(IPAddress.Any, PORT));
            serverSocket.Listen(0);
            serverSocket.BeginAccept(AcceptCallback, null);
        }

        public void Output(string text)
        {
            serveroutput.Invoke((Action)delegate 
            {
                serveroutput.AppendText(text + Environment.NewLine);
            });
        }

        private void AcceptCallback(IAsyncResult AR)
        {
            Socket socket;

            try
            {
                socket = serverSocket.EndAccept(AR);
            }
            catch (ObjectDisposedException) 
            {
                return;
            }

            //clientSockets.Add(socket);

            User user = new User();
            user.Socket = socket;
            user.Code = generateCode();

            users.Add(user);

            socket.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCallback, socket);
            Output(socket.RemoteEndPoint + " Client connected, waiting for request...");
            serverSocket.BeginAccept(AcceptCallback, null);
        }

        private string generateCode()
        {
            string generated_number = "";
            Random r = new Random();
            for (int i = 0; i < 10; i++)
            {
                generated_number += r.Next(0, 10);
            }

            foreach (User u in users)
            {
                if (u.Code == generated_number)
                {
                    generated_number = generateCode();
                }
            }

            return generated_number;
        }

        private void removeUser(Socket socket)
        {
            Invoke((Action)delegate
            {
                foreach (User user in users)
                {
                    if (user.Socket == socket)
                    {
                        users.Remove(user);
                        break;
                    }
                }
            });
        }

        public void Send(Socket socket, string command)
        {
            byte[] data = Encoding.ASCII.GetBytes(command);
            socket.Send(data);
        }

        private void ReceiveCallback(IAsyncResult AR)
        {
            Socket current = (Socket)AR.AsyncState;
            int received;

            try
            {
                received = current.EndReceive(AR);
            }
            catch (SocketException)
            {
                Output("" + current.RemoteEndPoint + " > " + "Client disconnected");

                removeUser(current);
                foreach(Game game in games.Values)
                {
                    game.removeUser(current);
                }

                /*foreach (var user in users)
                {
                    Output("" + user.RemoteEndPoint);
                }*/
                // Don't shutdown because the socket may be disposed and its disconnected anyway.
                current.Close();
                //clientSockets.Remove(current);
                current.Dispose();
                return;
            }

            byte[] recBuf = new byte[received];
            Array.Copy(buffer, recBuf, received);
            string decrypt = Encoding.ASCII.GetString(recBuf);
            string text = Decrypt(decrypt);
            //string text = Decrypt(decrypt);
            if (text != null)
            {
                Output("" + current.RemoteEndPoint + " > " + text);
            }

            if (!Directory.Exists("Logs"))
                Directory.CreateDirectory("Logs");

            using (StreamWriter w = new StreamWriter("Logs/logs.txt",true))
            {
                w.WriteLine(DateTime.Now.ToString("dd/MM/yyyy") + " > " + DateTime.Now.ToString("HH:mm:ss") + " > " + current.RemoteEndPoint + " > " + text);
            }



            using (Commands c = new Commands(this, current))
            {
                if(text.Contains("game:"))
                {
                    c.game(text);
                }
                else if (text.Contains("request_user_data_get_menu:"))
                {
                    c.requestDataAndMenu(text);
                }
                else if (text.Contains("request_leaderboard_data"))
                {
                    c.requestLeaderboardData();
                }
                else if (text.Contains("request_user_data:"))
                {
                    c.requestUserData(text);
                }
                else if (text.Contains("login_user"))
                {
                    c.loginUser(text, server_version);
                }
                else if (text.Contains("register_user"))
                {
                    c.registerUser(text, server_version);
                }
                else if (text.Contains("user_change_password:"))
                {
                    c.changePassword(text);
                }
                else if (text.Contains("user_change_username:"))
                {
                    c.changeUsername(text);
                }
                else if (text.Contains("console_command:"))
                {
                    c.consoleCommands(text, this);
                }
                else if (text.Contains("host_server:"))
                {
                    c.hostServer(text);
                }
                else if (text.Contains("match_finish:"))
                {
                    c.matchFinish(text, this);
                }
                else
                {
                    /*
                    byte[] data = Encoding.ASCII.GetBytes("Invalid request");
                    current.Send(data);*/
                }
            }

            current.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCallback, current);
        }

        private void UserList_Click(object sender, EventArgs e)
        {
            if (users.Count > 0)
            {
                Output("User list:");
                foreach (User user in users)
                {
                    Output("Username: " + user.Username + "     Code: " + user.Code + "     IP: " + user.Socket.RemoteEndPoint + "     ID: " + user.ID);
                }
            }
            else
            {
                Output("User list empty");
            }

        }

        private void Server_SizeChanged(object sender, EventArgs e)
        {

        }

        private void NotifyIcon_Click(object sender, EventArgs e)
        {
            if(WindowState == FormWindowState.Normal)
                this.WindowState = FormWindowState.Minimized;
            else if(WindowState == FormWindowState.Minimized)
                this.WindowState = FormWindowState.Normal;
        }

        public string Decrypt(string text)
        {
            string decrypted = "";
            for (int i = 0; i < text.Length; i++)
            {
                switch (Convert.ToString(text[i]))
                {
                    case "a":
                        decrypted += "q";
                        break;
                    case "b":
                        decrypted += "w";
                        break;
                    case "c":
                        decrypted += "e";
                        break;
                    case "d":
                        decrypted += "r";
                        break;
                    case "e":
                        decrypted += "t";
                        break;
                    case "f":
                        decrypted += "z";
                        break;
                    case "g":
                        decrypted += "u";
                        break;
                    case "h":
                        decrypted += "i";
                        break;
                    case "i":
                        decrypted += "o";
                        break;
                    case "j":
                        decrypted += "p";
                        break;
                    case "k":
                        decrypted += "a";
                        break;
                    case "l":
                        decrypted += "s";
                        break;
                    case "m":
                        decrypted += "d";
                        break;
                    case "n":
                        decrypted += "f";
                        break;
                    case "o":
                        decrypted += "g";
                        break;
                    case "p":
                        decrypted += "h";
                        break;
                    case "r":
                        decrypted += "j";
                        break;
                    case "s":
                        decrypted += "k";
                        break;
                    case "t":
                        decrypted += "l";
                        break;
                    case "u":
                        decrypted += "y";
                        break;
                    case "v":
                        decrypted += "x";
                        break;
                    case "z":
                        decrypted += "c";
                        break;
                    case "x":
                        decrypted += "v";
                        break;
                    case "y":
                        decrypted += "b";
                        break;
                    case "q":
                        decrypted += "n";
                        break;
                    case "w":
                        decrypted += "m";
                        break;
                    case ",":
                        decrypted += "_";
                        break;
                    case "?":
                        decrypted += ":";
                        break;
                    case "=":
                        decrypted += ";";
                        break;
                    default:
                        decrypted += text[i];
                        break;
                }
            }
            return decrypted;
        }
    }
}
