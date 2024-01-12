using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Animation;
using ZstdSharp.Unsafe;
using static System.Net.Mime.MediaTypeNames;

namespace Monopoly_Server_Forms_0._9._1._1
{
    public class Commands : IDisposable
    {
        #region Win/Lose Constants per rank

        //Win Constants per rank
        const double StarterWin = 65;
        const double AmateurWin = 60;
        const double ExperiencedWin = 50;
        const double ExpertWin = 40;
        const double MasterWin = 30;
        const double GrandmasterWin = 25;

        //Lose Constants per rank
        const double StarterLose = 40;
        const double AmateurLose = 45;
        const double ExperiencedLose = 50;
        const double ExpertLose = 55;
        const double MasterLose = 56;
        const double GrandmasterLose = 50;

        //Multipliers
        const double DeathMultiplier = 1.5;
        const double OverallMultiplier = 100;

        #endregion

        Socket current;
        Server server;
        DBMethods method;

        public Commands(Server server, Socket socket)
        {
            current = socket;
            this.server = server;
        }

        #region Menu commands

        public void requestDataAndMenu(string command)
        {
            using (method = new DBMethods())
            {
                string[] user = command.Split(':');

                string[] user_data = method.getUserData(user[1]);

                bool username_connected = false;
                foreach (User u in Server.users)
                {
                    if (u.Username == user[1])
                    {
                        username_connected = true;
                    }
                }

                string code = "";

                if (!username_connected)
                {
                    foreach (User u in Server.users)
                    {
                        if (u.Socket == current)
                        {
                            u.Username = user[1];
                            u.ID = method.getUserID(user[1]);
                            code = u.Code;
                            break;
                        }
                    }

                    string[] ips = current.RemoteEndPoint.ToString().Split(':');

                    string IP = ips[0];

                    string send_user_data = "update_user_get_menu:" + user_data[0] + ":" + IP + ":" + code + ":" + user_data[1] + ":" + user_data[2] + ":" + user_data[3] + ":" + user_data[4] + ":";

                    Send(send_user_data);
                }
                else
                {
                    Send("login_username_already_connected");
                }
            }
        }

        public void requestingJoinIP(string text)
        {
            string[] data = text.Split(':');

            /*foreach (User u in Server.users)
            {
                if (u.Code == data[1])
                {
                    Send("join_match_ip:" + u.Socket.RemoteEndPoint);
                }
            }*/
        }

        public void requestLeaderboardData()
        {
            using (method = new DBMethods())
            {
                Send("leaderboard_data:" + method.getLeaderboardData());
            }
        }

        public void requestUserData(string command)
        {
            using (method = new DBMethods())
            {
                string[] user = command.Split(':');

                string[] user_data = method.getUserData(user[1]);

                string code = "";

                foreach (User u in Server.users)
                {
                    if (u.Socket == current)
                    {
                        code = u.Code;
                    }
                }

                string[] ips = current.RemoteEndPoint.ToString().Split(':');

                string IP = ips[0];

                string send_user_data = "update_data:" + user_data[0] + ":" + IP + ":" + code + ":" + user_data[1] + ":" + user_data[2] + ":" + user_data[3] + ":" + user_data[4] + ":";

                Send(send_user_data);
                Console.WriteLine("Send client back data: " + send_user_data);
            }
        }

        public void loginUser(string command, string server_version)
        {
            using (method = new DBMethods())
            {
                Console.WriteLine("Client sent login information");
                string[] login_information = command.Split(':');
                if (method.getRank(login_information[1]) != "banned")
                {
                    if (login_information[3] == server_version)
                    {
                        Boolean login = method.loginCheck(login_information[1], login_information[2]);
                        if (login)
                        {
                            Send("login_true");
                            Console.WriteLine("Client information true - Sending 'true' to log them in");
                        }
                        else
                        {
                            Send("login_false");
                            Console.WriteLine("Client information false - Sending 'false' because information was wrong");
                        }
                    }
                    else
                    {
                        Send("wrong_version");
                        Console.WriteLine("Wrong client version - Sending 'wrong_version' to inform them");
                    }
                }
                else
                {
                    Send("you_banned:");
                    Console.WriteLine("User is banned - Sending 'you_are_banned' to inform them");
                }
            }
        }

        public void registerUser(string command, string server_version)
        {
            using (method = new DBMethods())
            {
                Console.WriteLine("Client sent register information");
                string[] register_information = command.Split(':');
                if (method.getRank(register_information[1]) != "banned")
                {
                    if (register_information[3] == server_version)
                    {
                        string register = method.registerCheck(register_information[1], register_information[2]);
                        if (register == "register_true")
                        {
                            Send("register_true");
                            Console.WriteLine("Client registered successfully - Sending 'true' to notify user");
                        }
                        else if (register == "register_exists")
                        {
                            Send("register_exists");
                            Console.WriteLine("Client tried registering with already existing username - Sending 'exists' to notify user");
                        }
                        else if (register == "register_error")
                        {
                            Send("register_error");
                            Console.WriteLine("Client tried registering, but there was error - Sending 'error' to notify user");
                        }
                    }
                    else
                    {
                        Send("wrong_version");
                        Console.WriteLine("Wrong client version - Sending 'wrong_version' to inform them");
                    }
                }
                else
                {
                    Send("you_are_banned");
                    Console.WriteLine("User is banned - Sending 'you_are_banned' to inform them");
                }
            }
        }

        public void changePassword(string command)
        {
            using (method = new DBMethods())
            {
                string[] password_information = command.Split(':');
                if (method.checkPassword(password_information[1], password_information[2]))
                {
                    if (method.setPassword(password_information[1], password_information[3]))
                    {
                        Send("change_password_successful");
                    }
                    else
                    {
                        Send("change_password_error");
                    }
                }
                else
                {
                    Send("change_password_wrong");
                }
            }
        }

        public void changeUsername(string command)
        {
            using (method = new DBMethods())
            {
                string[] user_information = command.Split(':');
                if (method.checkPassword(user_information[1], user_information[3]))
                {
                    if (!method.alreadyChangedUsername(user_information[1]))
                    {
                        if (!method.checkUsername(user_information[2]))
                        {
                            if (method.setUsername(user_information[1], user_information[2]))
                            {
                                string[] user = command.Split(':');

                                string[] user_data = method.getUserData(user_information[2]);

                                string code = "";

                                foreach (User u in Server.users)
                                {
                                    if (u.Socket == current)
                                    {
                                        code = u.Code;
                                    }
                                }

                                string[] ips = current.RemoteEndPoint.ToString().Split(':');

                                string IP = ips[0];

                                string send_user_data = user_data[0] + ":" + IP + ":" + code + ":" + user_data[1] + ":" + user_data[2] + ":" + user_data[3] + ":" + user_data[4] + ":" + user_information[2] + ":";

                                Send("change_username_successful:" + send_user_data);
                            }
                            else
                            {
                                Send("change_username_error");
                            }
                        }
                        else
                        {
                            Send("change_username_exists");
                        }
                    }
                    else
                    {
                        Send("change_username_already");
                    }
                }
                else
                {
                    Send("change_username_wrong");
                }
            }
        }

        #endregion

        #region Console Commands

        public void consoleCommands(string command, Server server)
        {
            string[] command_data = command.Split(';');

            string[] command_executer = command_data[0].Split(':');

            string[] command_user = command_data[1].Split(':');

            string executer = command_executer[1];
            string username = command_user[1];

            using (method = new DBMethods())
            {
                if (method.checkUsername(username))
                {
                    if (command.Contains("unban:"))
                    {
                        if (method.checkUnbanned(username))
                        {
                            method.unbanUser(username);
                            method.Log(executer, "unbanned", username);
                            ConsoleError("Username " + username + " has been unbanned");
                        }
                        else
                            ConsoleError("Username " + username + " is not banned");
                    }
                    else if (command.Contains("ban:"))
                    {
                        string[] e = method.getUserData(executer);
                        string[] u = method.getUserData(username);

                        if (canKickBanUser(e, u))
                        {
                            method.banUser(username);
                            method.Log(executer, "banned", username);
                            banUser(server, username);
                            ConsoleError("Username " + username + " has been banned");
                        }
                        else
                            ConsoleError("Cannot ban user");
                    }
                    else if (command.Contains("kick:"))
                    {
                        string[] e = method.getUserData(executer);
                        string[] u = method.getUserData(username);
                        if (canKickBanUser(e, u))
                        {
                            kickUser(server, username);
                            method.Log(executer, "kicked", username);
                        }
                        else
                            ConsoleError("Cannot kick user");
                    }
                    else if (command.Contains("set_rank:"))
                    {
                        string[] e = method.getUserData(executer);
                        string[] u = method.getUserData(username);
                        string rank = command_user[2];
                        if (rankExists(rank))
                        {
                            if (canSetRank(e, u, rank))
                            {
                                method.setRank(username, rank);
                                method.Log(executer, "set rank", username, "rank: " + rank);
                                ConsoleError(username + " now has rank " + rank);
                                updateUser(server, username);
                            }
                            else
                                ConsoleError("You cannot set " + rank + " to " + username);
                        }
                        else
                            ConsoleError("rank " + rank + " doesn't exist. \nAvailable ranks; \n -owner,admin,mod,moderator,starter,amateur,experienced,expert,master,grandmaster");
                    }
                    else if (command.Contains("set_prefix:"))
                    {
                        string prefix = command_user[2];

                        string[] e = method.getUserData(executer);
                        string[] u = method.getUserData(username);

                        if (canKickBanUser(e, u))
                        {
                            method.setPrefix(username, prefix);
                            method.Log(executer, "set prefix", username, "prefix: " + prefix);
                            ConsoleError(username + " now has prefix " + prefix);
                            updateUser(server, username);
                        }
                        else
                            ConsoleError("You cannot set " + prefix + " to " + username);
                    }
                    else if (command.Contains("set_prefix_color:"))
                    {
                        string prefix_color = command_user[2];

                        string[] e = method.getUserData(executer);
                        string[] u = method.getUserData(username);

                        if (canKickBanUser(e, u))
                        {
                            if (isColorValid(prefix_color))
                            {
                                method.setPrefixColor(username, prefix_color);
                                method.Log(executer, "set prefix_color", username, "color: " + prefix_color);
                                ConsoleError(username + " now has prefix color " + prefix_color);
                                updateUser(server, username);
                            }
                        }
                        else
                            ConsoleError("You cannot set " + prefix_color + " to " + username);
                    }
                    else if (command.Contains("set_score:"))
                    {
                        string[] e = method.getUserData(executer);

                        if (canSetScore(e))
                        {
                            string score = command_user[2];
                            try
                            {
                                double double_score = Convert.ToDouble(score);
                                method.setScore(username, double_score);
                                method.Log(executer, "set score", username, "score: " + score);
                                ConsoleError(username + " now has " + score + " score");
                                updateUser(server, username);
                            }
                            catch (Exception)
                            {
                                ConsoleError("Invalid number");
                            }
                        }
                        else
                            ConsoleError("You cannot set score to " + username);
                    }
                    else if (command.Contains("set_rename:"))
                    {
                        string rename = command_user[2];
                        if (rename == "true")
                        {
                            method.setRename(username, rename);
                            ConsoleError(username + " can now change his name");
                            method.Log(executer, "set rename", username, "rename: " + rename);
                        }
                        else if (rename == "false")
                        {
                            method.setRename(username, rename);
                            ConsoleError(username + " can't change his name anymore");
                            method.Log(executer, "set rename", username, "rename: " + rename);
                        }
                        else
                            ConsoleError("While using this command, you can only choose between true or false");
                    }
                    else if (command.Contains("set_console:"))
                    {
                        string console = command_user[2];
                        if (console == "true")
                        {
                            method.setConsole(username, console);
                            ConsoleError(username + " now has access to console, only commands for his rank");
                            updateUser(server, username);
                        }
                        else if (console == "false")
                        {
                            method.setConsole(username, console);
                            ConsoleError(username + " doesn't have access to console anymore");
                            updateUser(server, username);
                        }
                        else
                            ConsoleError("While using this command, you can only choose between true or false");
                        method.Log(executer, "set console", username, "console: " + console);
                    }
                    else if (command.Contains("get_info:"))
                    {
                        string[] user_info = method.getInfo(username);
                        method.Log(executer, "get info", username);
                        ConsoleError("send_info:" + username + ":" + user_info[0] + ":" + user_info[1] + ":" + user_info[2] + ":" + user_info[3] + ":" + user_info[4] + ":" + user_info[5] + ":");
                    }
                    else
                    {
                        ConsoleError("Command not found");
                    }
                }
                else
                {
                    ConsoleError("Username doesn't exist");
                }
            }
        }

        public bool usernameExists(string username)
        {
            foreach (User user in Server.users)
            {
                if (user.Username == username)
                {
                    return true;
                }
            }
            return false;
        }

        public void kickUser(Server server, string username)
        {
            if (usernameExists(username))
            {
                foreach (User user in Server.users)
                {
                    if (user.Username == username)
                    {
                        server.Send(user.Socket, "you_kicked:");
                        user.Username = null;
                        ConsoleError(username + " has been kicked");
                        break;
                    }
                }
            }
            else
            {
                ConsoleError(username + " is not online");
            }
        }

        public void banUser(Server server, string username)
        {
            if (usernameExists(username))
            {
                foreach (User user in Server.users)
                {
                    if (user.Username == username)
                    {
                        server.Send(user.Socket, "you_banned:");
                        user.Username = null;
                        break;
                    }
                }
            }
        }

        public void updateUser(Server server, string username)
        {
            if (usernameExists(username))
            {
                foreach (User user in Server.users)
                {
                    if (user.Username == username)
                    {
                        using (DBMethods method = new DBMethods())
                        {
                            string[] user_data = method.getUserData(username);

                            string code = "";

                            foreach (User u in Server.users)
                            {
                                if (u.Socket == current)
                                {
                                    code = u.Code;
                                }
                            }

                            string[] ips = current.RemoteEndPoint.ToString().Split(':');

                            string IP = ips[0];

                            string send_user_data = "update_data:" + user_data[0] + ":" + IP + ":" + code + ":" + user_data[1] + ":" + user_data[2] + ":" + user_data[3] + ":" + user_data[4] + ":";

                            server.Send(user.Socket, send_user_data);
                        }
                        break;
                    }
                }
            }
        }

        #endregion

        #region Game commands

        public void hostServer(string command)
        {
            string[] data = command.Split(':');

            User u = null; ;
            foreach(User user in Server.users)
            {
                if(user.Socket == current)
                {
                    u = user;
                    break;
                }
            }

            if (Server.games.ContainsKey(u.Code))
            {
                Send("game_already_hosted_error");
                return;
            }

            Game game = new Game(int.Parse(data[1]), u.Code);
            //GameUser gameUser = method.createGameUser(u.Username, data[2], u.Socket);
            Server.games[u.Code] = game;

            //game.Add(gameUser);
            
            Send("successfully_hosted_game");
        }

        public void game(string command)
        {
            string[] data = command.Split(':');
            string code = data[1];

            if (!Server.games.ContainsKey(code))
            {
                Send("game_not_found");
                return;
            }

            command = string.Join(":", data.Skip(2));
            using(DBMethods method = new DBMethods())
            {
                Server.games[data[1]].Data(method, current, command);
            }
        }

        #endregion

        #region Console Methods

        public bool canKickBanUser(string[] executer_data, string[] username_data)
        {
            string executer_rank = executer_data[1];
            string username_rank = username_data[1];

            if (executer_rank == "owner" || executer_rank == "moderator" || executer_rank == "mod" || executer_rank == "admin")
            {
                if (executer_rank == "owner")
                {
                    return true;
                }
                else if (executer_rank == "admin")
                {
                    if (username_rank == "owner")
                        return false;
                    else if (username_rank == "admin" && executer_data[0] != username_data[0])
                        return false;
                    else
                        return true;
                }
                else if (executer_rank == "mod" || executer_rank == "moderator")
                {
                    if (username_rank == "owner")
                        return false;
                    else if (username_rank == "admin")
                        return false;
                    else if (username_rank == "mod" || username_rank == "moderator")
                        return false;
                    else
                        return true;
                }
                else
                {
                    return false;
                }
            }
            else
                return false;
        }

        public bool canSetScore(string[] executer_data)
        {
            string executer_rank = executer_data[1];

            if (executer_rank == "owner" || executer_rank == "admin")
            {
                return true;
            }
            else
                return false;

        }

        public bool rankExists(string rank)
        {
            bool rank_exists = false;
            switch (rank)
            {
                case "owner":
                    rank_exists = true;
                    break;
                case "admin":
                    rank_exists = true;
                    break;
                case "mod":
                    rank_exists = true;
                    break;
                case "moderator":
                    rank_exists = true;
                    break;
                case "starter":
                    rank_exists = true;
                    break;
                case "amateur":
                    rank_exists = true;
                    break;
                case "experienced":
                    rank_exists = true;
                    break;
                case "expert":
                    rank_exists = true;
                    break;
                case "master":
                    rank_exists = true;
                    break;
                case "grandmaster":
                    rank_exists = true;
                    break;
            }
            return rank_exists;
        }

        public bool canSetRank(string[] executer_data, string[] username_data, string rank)
        {
            string executer_rank = executer_data[1];
            string username_rank = username_data[1];
            if (executer_rank == "owner")
            {
                return true;
            }
            else if (executer_rank == "admin")
            {
                if (rank == "owner" || rank == "admin" || rank == "mod" || rank == "moderator")
                    return false;
                else
                    return true;
            }
            else if (executer_rank == "moderator" || executer_rank == "mod")
            {
                if (username_rank == "owner" || username_rank == "admin" || username_rank == "mod" || username_rank == "moderator")
                {
                    return false;
                }
                else if (rank == "owner" || rank == "admin" || rank == "mod" || rank == "moderator")
                {
                    return false;
                }
                else
                    return true;
            }
            else
            {
                return false;
            }
        }

        public bool isColorValid(string color)
        {
            if (color == "none")
            {
                return true;
            }
            else if (color.Contains("animation")) //animation-custom-255-0-0-custom-0-0-0-10000
            {
                try
                {
                    string[] prefix_colors = color.Split('/');
                    byte red1 = Convert.ToByte(prefix_colors[2]);
                    byte green1 = Convert.ToByte(prefix_colors[3]);
                    byte blue1 = Convert.ToByte(prefix_colors[4]);
                    Color c1 = Color.FromRgb(red1, green1, blue1);

                    byte red2 = Convert.ToByte(prefix_colors[6]);
                    byte green2 = Convert.ToByte(prefix_colors[7]);
                    byte blue2 = Convert.ToByte(prefix_colors[8]);
                    Color c2 = Color.FromRgb(red2, green2, blue2);

                    int timespan_count = Convert.ToInt32(prefix_colors[9]);
                    TimeSpan timespan = new TimeSpan(0, 0, 0, 0, timespan_count);

                    ColorAnimation animation = new ColorAnimation(c1, c2, new Duration(timespan));
                    animation.AutoReverse = true;
                    animation.RepeatBehavior = RepeatBehavior.Forever;

                    SolidColorBrush animation_color = new SolidColorBrush();

                    return true;
                }
                catch (Exception)
                {
                    ConsoleError("There was an error, check your command again.\n set prefix_color animation/custom/[red]/[green]/[blue]/custom/[red]/[green]/[blue]");
                    return false;
                }
            }
            else if (color.Contains("custom/")) //custom-255-255-255
            {
                try
                {
                    string[] prefix_colors = color.Split('/');
                    byte red = Convert.ToByte(prefix_colors[1]);
                    byte green = Convert.ToByte(prefix_colors[2]);
                    byte blue = Convert.ToByte(prefix_colors[3]);
                    Color c = Color.FromRgb(red, green, blue);
                    return true;
                }
                catch (Exception)
                {
                    ConsoleError("There was an error, check your command again.\n set prefix_color custom/[red]/[green]/[blue]/[time]");
                    return false;
                }
            }
            else
            {
                try
                {
                    BrushConverter converter = new BrushConverter();
                    SolidColorBrush c = converter.ConvertFromString(color) as SolidColorBrush;
                    return true;
                }
                catch (Exception)
                {
                    ConsoleError("There was an error, color doesn't exist.\n set prefix_color custom/[red]/[green]/[blue]");
                    return false;
                }
            }
        }

        public void ConsoleError(string text)
        {
            byte[] data = Encoding.ASCII.GetBytes("console_message:" + text + ":");
            current.Send(data);
        }

        #endregion

        #region Match Finish Algorithms

        public void matchFinish(string game_data, Server server)
        {
            using (DBMethods method = new DBMethods())
            {
                string[] split_game_data = game_data.Split(':');

                string winner = split_game_data[1];
                string bankrupt_list = "";
                string left_list = "";

                int bankrupt_players = 0;

                List<string> data = new List<string>();

                for (int i = 2; i < split_game_data.Length - 1; i++)
                {
                    data.Add(split_game_data[i]);
                }


                //get bankrupt players number
                for (int i = 0; i < data.Count; i += 3)
                {
                    bool is_bankrupt = Convert.ToBoolean(data[i + 1]);
                    if (is_bankrupt)
                    {
                        bankrupt_players++;
                    }
                }

                //set winner score
                double score = getScoreSet(winner, true, false, false, bankrupt_players);
                method.setScore(winner, score);
                updateUser(server, winner);

                //set other people score
                for (int i = 0; i < data.Count; i += 3)
                {
                    string username = data[i];
                    bool is_bankrupt = Convert.ToBoolean(data[i + 1]);
                    bool has_left = Convert.ToBoolean(data[i + 2]);

                    score = getScoreSet(username, false, is_bankrupt, has_left, bankrupt_players);
                    method.setScore(username, score);
                    updateUser(server, username);

                    bankrupt_list = bankrupt_list + username + ", ";
                    if (has_left)
                        left_list = left_list + username + ", ";
                }
                method.matchLog(winner, bankrupt_list, left_list);
            }
        }

        public double getScoreSet(string username, bool is_winner, bool is_bankrupt, bool has_left, int players_bankrupt)
        {
            players_bankrupt++;
            double score = 0;
            double score_multiplier = 1;

            using (DBMethods method = new DBMethods())
            {
                string[] user_data = method.getUserData(username);
                score = Convert.ToDouble(user_data[0]);
            }

            if (is_winner)
            {
                double score_to_add = scoreToAdd(score);
                score_multiplier += Math.Log(players_bankrupt, OverallMultiplier);
                score_to_add = score_to_add * score_multiplier;

                score += score_to_add;

                return score;

            }
            else
            {
                double score_to_remove = scoreToRemove(score);
                score_multiplier += Math.Log(players_bankrupt, OverallMultiplier / 2);
                score_to_remove = score_to_remove / score_multiplier;

                if (has_left && !is_bankrupt)
                {
                    score_to_remove = score_to_remove * DeathMultiplier;
                }

                score -= score_to_remove;

                if (score >= 0)
                {
                    return score;
                }
                else
                {
                    return 0;
                }

            }
        }

        public double scoreToAdd(double score)
        {
            if (score >= 0 && score < 200)
            {
                return StarterWin;
            }
            else if (score >= 200 && score < 500)
            {
                return AmateurWin;
            }
            else if (score >= 500 && score < 1000)
            {
                return ExperiencedWin;
            }
            else if (score >= 1000 && score < 1700)
            {
                return ExpertWin;
            }
            else if (score >= 1700 && score < 2500)
            {
                return MasterWin;
            }
            else if (score >= 2500)
            {
                return GrandmasterWin;
            }

            return 0;
        }

        public double scoreToRemove(double score)
        {
            if (score >= 0 && score < 200)
            {
                return StarterLose;
            }
            else if (score >= 200 && score < 500)
            {
                return AmateurLose;
            }
            else if (score >= 500 && score < 1000)
            {
                return ExperiencedLose;
            }
            else if (score >= 1000 && score < 1700)
            {
                return ExpertLose;
            }
            else if (score >= 1700 && score < 2500)
            {
                return MasterLose;
            }
            else if (score >= 2500)
            {
                return GrandmasterLose;
            }

            return 0;
        }

        #endregion

        public void Send(string text)
        {
            byte[] data = Encoding.ASCII.GetBytes(text);
            current.Send(data);
        }

        #region fose object

        // To detect redundant calls
        private bool _disposed = false;

        // Instantiate a SafeHandle instance.
        private SafeHandle _safeHandle = new SafeFileHandle(IntPtr.Zero, true);

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose() => Dispose(true);

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                // Dispose managed state (managed objects).
                _safeHandle?.Dispose();
            }

            _disposed = true;
        }

        #endregion

    }
}
