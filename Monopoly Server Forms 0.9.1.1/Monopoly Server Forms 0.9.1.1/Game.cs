using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly_Server_Forms_0._9._1._1
{
    public class Game
    {
        public Game(int players_needed, string code)
        {
            this.code = code;
            this.players_needed = players_needed;
            game = "GAME-" + code + "=";
            StartServer();
        }

        private string game;
        private string code;
        public List<GameUser> users;
        private List<GameUser> all_ingame_users;
        //private static int server_players = 0;
        private int players_needed = 1;
        private Boolean server_started_game = false;

        private const int BUFFER_SIZE = 2048;
        private int PORT;
        private readonly byte[] buffer = new byte[BUFFER_SIZE];

        public void Add(GameUser gameUser)
        {
            users.Add(gameUser);
        }

        /// <summary>
        /// Construct server socket and bind socket to all local network interfaces, then listen for connections
        /// with a backlog of 10. Which means there can only be 10 pending connections lined up in the TCP stack
        /// at a time. This does not mean the server can handle only 10 connections. The we begin accepting connections.
        /// Meaning if there are connections queued, then we should process them.
        /// </summary>
        /// 

        public void CloseServer()
        {
            foreach (GameUser u in users)
            {
                try
                {
                    /*u.Socket.Shutdown(SocketShutdown.Both);
                    u.Socket.Close();*/
                }
                catch (Exception)
                {
                }
            }
        }

        private void StartServer()
        {
            users = new List<GameUser>();
            server_started_game = false;
        }

        
        private void Error(string message, string method) //Prikaže error v MessageBoxu
        {
            //MessageBox.Show(message, "Server > " + method, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void Output(string text)
        {
            /*Dispatcher.Invoke((Action)delegate
            {
                serverOutput.AppendText("\n" + text);
            });*/
        }

        public async void removeUser(Socket socket)
        {
            for (int i = 0; i < users.Count; i++)
            {
                if (socket == users[i].Socket)
                {
                    Broadcast("remove_character:" + users[i].Username + ":" + users[i].Figure);
                    if (users[i].Turn && server_started_game)
                    {
                        if (server_started_game)
                            setTurn(false);
                    }
                    await Task.Delay(50);
                    SendMessage(users[i].Username + " has left the game.");
                    users.Remove(users[i]);
                }
            }
            if (all_ingame_users != null)
            {
                foreach (GameUser user in all_ingame_users)
                {
                    if (socket == user.Socket)
                    {
                        user.Left = true;
                    }
                }
            }
        }


        public async void Data(DBMethods methods, Socket current, string text)
        {
            Output(game + text);

            // first

            if (text.Contains("add_player:"))
            {
                string[] user_data = text.Split(':');

                GameUser user = methods.createGameUser(user_data[1], user_data[2], current);

                users.Add(user);
                Output(game + "added " + user.Username);
                Output(game + "user count: " + users.Count);

                foreach (GameUser u in users)
                {
                    await Task.Delay(50);
                    user.Send("add_character:" + u.Username + ":" + u.Figure + ":" + u.Rank + ":" + u.Prefix + ":" + u.PrefixColor + ":");
                }

                await Task.Delay(50);
                Broadcast("add_character:" + user.Username + ":" + user.Figure + ":" + user.Rank + ":" + user.Prefix + ":" + user.PrefixColor + ":");
                await Task.Delay(50);
                SendMessage(user.Username + " has joined.");

                if (users.Count >= players_needed)
                {
                    await Task.Delay(100);
                    setTurn(true);
                    server_started_game = true;
                    all_ingame_users = new List<GameUser>();
                    foreach (GameUser u in users)
                    {
                        all_ingame_users.Add(u);
                    }
                    Output(game + "Game has started");
                }

            }
            else if (text.Contains("exit_from_game"))
            {
                removeUser(current);
                //await Task.Delay(100);
                playerWinnerCheck();
                /*current.Shutdown(SocketShutdown.Both);
                current.Close();*/
            }
            else if (text.Contains("bankrupt:"))
            {
                string[] data = text.Split(':');

                foreach (GameUser user in users)
                {
                    if (data[1] == user.Username)
                    {
                        user.Bankrupt = true;
                    }
                }

                foreach (GameUser user in all_ingame_users)
                {
                    if (data[1] == user.Username)
                    {
                        user.Bankrupt = true;
                    }
                }

                Broadcast(text);
                await Task.Delay(100);
                playerWinnerCheck();
            }
            else if (text.Contains("message:"))
            {
                string[] message = text.Split(':');
                SendMessage(message[1]);
            }
            else if (text.Contains("requesting_to_join:"))
            {
                string[] info = text.Split(':');
                bool join = true;
                Output(game + info[1] + " > " + info[2] + " > " + " requested to join");
                string reason = "";
                foreach (GameUser user in users)
                {
                    /*Output(user.Username+"=="+info[1]);
                    Output(user.Figure+"=="+info);*/
                    if (user.Figure == info[2])
                    {
                        join = false;
                        reason = "Someone else is already using character you want to use.";
                        break;
                    }
                    else if (user.Username == info[1])
                    {
                        join = false;
                        reason = "Someone else is already ingame with your username.";
                        break;
                    }
                }

                if (server_started_game)
                {
                    join = false;
                    reason = "Game has already started.";
                }

                if (join == false)
                {
                    byte[] data = Encoding.ASCII.GetBytes("declined_to_join:" + reason + ":");
                    Output(game + info[1] + " > " + info[2] + " > " + " was declined");
                    current.Send(data);
                }
                else
                {
                    byte[] data = Encoding.ASCII.GetBytes("accepted_to_join:" + code + ":");
                    Output(game + info[1] + " > " + info[2] + " > " + " was accepted");
                    current.Send(data);
                }
                // Always Shutdown before closing
                /*current.Shutdown(SocketShutdown.Both);
                current.Close();*/
                return;
            }
            else if (text.Contains("end_turn:"))
            {
                setTurn(false);
            }
            else
            {
                try
                {
                    Broadcast(text);
                }
                catch (Exception ex)
                {

                }
            }

            try
            {
                //current.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, ServerReceiveCallback, current);
            }
            catch (Exception ex)
            {
                Error(ex.Message, MethodBase.GetCurrentMethod().Name);
            }
        }

        public void Broadcast(string text)
        {
            foreach (GameUser user in users)
            {
                user.Send(text);
            }
        }

        public void SendMessage(string text)
        {
            Broadcast("message:" + text + ":");
        }

        private async void setTurn(bool startTurn)
        {
            if (startTurn == true)
            {
                Broadcast("game_start:");
                await Task.Delay(1000);
                Broadcast("player_turn:" + users[0].Username + ":");
                users[0].Turn = true;
            }
            else
            {
                for (int i = 0; i < users.Count; i++)
                {
                    if (users[i].Turn == true)
                    {
                        users[i].Turn = false;
                        try
                        {
                            if (users[i + 1].Bankrupt == false)
                            {
                                users[i + 1].Turn = true;
                                Broadcast("player_turn:" + users[i + 1].Username + ":");
                            }
                            else
                            {
                                users[i + 1].Turn = true;
                                setTurn(false);
                            }
                        }
                        catch (Exception)
                        {
                            if (users[0].Bankrupt == false)
                            {
                                users[0].Turn = true;
                                Broadcast("player_turn:" + users[0].Username + ":");
                            }
                            else
                            {
                                users[1].Turn = true;
                                setTurn(false);
                            }
                        }
                        break;
                    }
                }
            }
        }

        private void playerWinnerCheck()
        {
            int bankrupted_players = 0;

            foreach (GameUser user in users)
            {
                if (user.Bankrupt == true)
                {
                    bankrupted_players++;
                }
            }

            if (bankrupted_players == users.Count - 1)
            {
                string username = "";
                foreach (GameUser user in users)
                {
                    if (user.Bankrupt == false)
                    {
                        username = user.Username;
                    }
                }
                Broadcast("player_winner:" + username + ":");
                Output(game + "winner = " + username);
                //string winner = username;


                foreach (GameUser user in all_ingame_users)
                {
                    
                }

                //Monopoly.menu.main_server_client.Send("match_finish:" + match_data);
                //match_finish:winner:loser:true:false:loser:true:false:loser:false:true:
            }
        }
    }
}
