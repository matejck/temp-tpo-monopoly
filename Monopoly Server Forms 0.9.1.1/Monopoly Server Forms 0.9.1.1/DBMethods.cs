using Microsoft.Win32.SafeHandles;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly_Server_Forms_0._9._1._1
{
    public class DBMethods : IDisposable
    {

        public DBMethods()
        {

        }

        public GameUser createGameUser(string username, string figure, Socket socket)
        {
            GameUser gameUser = new GameUser(socket);

            string[] user_data = getUserData(username);

            /*
             
             ser_data[0] = (reader["score"].ToString());
                user_data[1] = (reader["rank"].ToString());
                user_data[2] = (reader["custom_prefix"].ToString());
                user_data[3] = (reader["custom_color"].ToString());
                user_data[4] = (reader["show_console"].ToString());
             */
            gameUser.Score = int.Parse(user_data[0]);
            gameUser.Rank = user_data[1];
            gameUser.Prefix = user_data[2];
            gameUser.PrefixColor = user_data[3];
            gameUser.ShowConsole = user_data[4];
            gameUser.Username = username;
            gameUser.Bankrupt = false;
            gameUser.Turn = false;
            gameUser.Left = false;
            gameUser.Figure = figure;
            foreach(User user in Server.users)
            {
                if(user.Username == username)
                {
                    gameUser.Code = user.Code;
                }
            }

            return gameUser;
        }

        public string[] getUserData(string username)
        {
            string[] user_data = new string[5];

            DB db = new DB();

            db.openConnection();

            DataTable table = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand command = new MySqlCommand("SELECT * FROM `users` WHERE `username` = @usn", db.getConnection());

            command.Parameters.Add("@usn", MySqlDbType.VarChar).Value = username;

            adapter.SelectCommand = command;

            adapter.Fill(table);

            MySqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                user_data[0] = (reader["score"].ToString());
                user_data[1] = (reader["rank"].ToString());
                user_data[2] = (reader["custom_prefix"].ToString());
                user_data[3] = (reader["custom_color"].ToString());
                user_data[4] = (reader["show_console"].ToString());
            }
            db.closeConnection();
            return user_data;
        }

        public string getUserID(string username) 
        {
            DB db = new DB();

            db.openConnection();

            DataTable table = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand command = new MySqlCommand("SELECT iduser FROM `users` WHERE `username` = @usn", db.getConnection());

            command.Parameters.Add("@usn", MySqlDbType.VarChar).Value = username;

            adapter.SelectCommand = command;

            adapter.Fill(table);

            MySqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                return (reader["iduser"].ToString());
            }
            db.closeConnection();

            return "N/A";
        }

        public string registerCheck(string username, string password)
        {
            if (checkUsername(username) == true)
            {
                return "register_exists"; //Nastavi text za napake 
            }
            else
            {
                DB db = new DB();
                MySqlCommand command = new MySqlCommand("INSERT INTO `users`(`username`, `password`) VALUES (@usn, @pass)", db.getConnection());

                command.Parameters.Add("@usn", MySqlDbType.VarChar).Value = username; //doda parameter ukazu
                command.Parameters.Add("@pass", MySqlDbType.VarChar).Value = password; //doda paremeter ukazu
                db.openConnection();
                if (command.ExecuteNonQuery() == 1)
                {
                    db.closeConnection();
                    return "register_true";
                }
                else
                {
                    return "register_error";
                }
            }
        }

        public bool setPassword(string username, string new_password)
        {
            DB db = new DB();
            MySqlCommand command = new MySqlCommand("UPDATE `users` SET `password`=@pass  WHERE `username`=@usn", db.getConnection());

            command.Parameters.Add("@usn", MySqlDbType.VarChar).Value = username; //doda parameter ukazu
            command.Parameters.Add("@pass", MySqlDbType.VarChar).Value = new_password; //doda parameter ukazu
            db.openConnection();
            if (command.ExecuteNonQuery() == 1)
            {
                db.closeConnection();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool setUsername(string username, string new_username)
        {
            DB db = new DB();
            MySqlCommand command = new MySqlCommand("UPDATE `users` SET `username`=@new,`changed_username`=@option  WHERE `username`=@usn", db.getConnection());

            command.Parameters.Add("@usn", MySqlDbType.VarChar).Value = username; //doda parameter ukazu
            command.Parameters.Add("@new", MySqlDbType.VarChar).Value = new_username; //doda paremeter ukazu
            command.Parameters.Add("@option", MySqlDbType.VarChar).Value = "true"; //doda paremeter ukazu
            db.openConnection();
            if (command.ExecuteNonQuery() == 1)
            {
                db.closeConnection();
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean alreadyChangedUsername(string username)
        {
            DB db = new DB();

            DataTable table = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand command = new MySqlCommand("SELECT * FROM `users` WHERE `username` = @usn and `changed_username` = @true", db.getConnection());

            command.Parameters.Add("@usn", MySqlDbType.VarChar).Value = username;
            command.Parameters.Add("@true", MySqlDbType.VarChar).Value = "true";

            adapter.SelectCommand = command;

            adapter.Fill(table);

            // tukaj preveri
            if (table.Rows.Count > 0)
            {
                db.closeConnection();
                return true;
            }
            else
            {
                db.closeConnection();
                return false;
            }
        }

        public string getLeaderboardData()
        {
            string data = "";

            DB db = new DB();

            db.openConnection();

            DataTable table = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand command = new MySqlCommand("SELECT username, score, `rank`, custom_prefix, custom_color FROM users WHERE `rank` != 'banned' ORDER BY score DESC LIMIT 10   ", db.getConnection());

            adapter.SelectCommand = command;

            adapter.Fill(table);

            try
            {
                data = data + table.Rows[0][0] + ":";
                data = data + table.Rows[0][1] + ":";
                data = data + table.Rows[0][2] + ":";
                data = data + table.Rows[0][3] + ":";
                data = data + table.Rows[0][4] + ";";
            }
            catch (Exception)
            {
                data = data + "#:";
                data = data + "#:";
                data = data + "#:";
                data = data + "#:";
                data = data + "#;";
            }
            try
            {
                data = data + table.Rows[1][0] + ":";
                data = data + table.Rows[1][1] + ":";
                data = data + table.Rows[1][2] + ":";
                data = data + table.Rows[1][3] + ":";
                data = data + table.Rows[1][4] + ";";
            }
            catch (Exception)
            {
                data = data + "#:";
                data = data + "#:";
                data = data + "#:";
                data = data + "#:";
                data = data + "#;";
            }
            try
            {
                data = data + table.Rows[2][0] + ":";
                data = data + table.Rows[2][1] + ":";
                data = data + table.Rows[2][2] + ":";
                data = data + table.Rows[2][3] + ":";
                data = data + table.Rows[2][4] + ";";
            }
            catch (Exception)
            {
                data = data + "#:";
                data = data + "#:";
                data = data + "#:";
                data = data + "#:";
                data = data + "#;";
            }
            try
            {
                data = data + table.Rows[3][0] + ":";
                data = data + table.Rows[3][1] + ":";
                data = data + table.Rows[3][2] + ":";
                data = data + table.Rows[3][3] + ":";
                data = data + table.Rows[3][4] + ";";
            }
            catch (Exception)
            {
                data = data + "#:";
                data = data + "#:";
                data = data + "#:";
                data = data + "#:";
                data = data + "#;";
            }
            try
            {
                data = data + table.Rows[4][0] + ":";
                data = data + table.Rows[4][1] + ":";
                data = data + table.Rows[4][2] + ":";
                data = data + table.Rows[4][3] + ":";
                data = data + table.Rows[4][4] + ";";
            }
            catch (Exception)
            {
                data = data + "#:";
                data = data + "#:";
                data = data + "#:";
                data = data + "#:";
                data = data + "#;";
            }
            try
            {
                data = data + table.Rows[5][0] + ":";
                data = data + table.Rows[5][1] + ":";
                data = data + table.Rows[5][2] + ":";
                data = data + table.Rows[5][3] + ":";
                data = data + table.Rows[5][4] + ";";
            }
            catch (Exception)
            {
                data = data + "#:";
                data = data + "#:";
                data = data + "#:";
                data = data + "#:";
                data = data + "#;";
            }
            try
            {
                data = data + table.Rows[6][0] + ":";
                data = data + table.Rows[6][1] + ":";
                data = data + table.Rows[6][2] + ":";
                data = data + table.Rows[6][3] + ":";
                data = data + table.Rows[6][4] + ";";
            }
            catch (Exception)
            {
                data = data + "#:";
                data = data + "#:";
                data = data + "#:";
                data = data + "#:";
                data = data + "#;";
            }
            try
            {
                data = data + table.Rows[7][0] + ":";
                data = data + table.Rows[7][1] + ":";
                data = data + table.Rows[7][2] + ":";
                data = data + table.Rows[7][3] + ":";
                data = data + table.Rows[7][4] + ";";
            }
            catch (Exception)
            {
                data = data + "#:";
                data = data + "#:";
                data = data + "#:";
                data = data + "#:";
                data = data + "#;";
            }
            try
            {
                data = data + table.Rows[8][0] + ":";
                data = data + table.Rows[8][1] + ":";
                data = data + table.Rows[8][2] + ":";
                data = data + table.Rows[8][3] + ":";
                data = data + table.Rows[8][4] + ";";
            }
            catch (Exception)
            {
                data = data + "#:";
                data = data + "#:";
                data = data + "#:";
                data = data + "#:";
                data = data + "#;";
            }
            try
            {
                data = data + table.Rows[9][0] + ":";
                data = data + table.Rows[9][1] + ":";
                data = data + table.Rows[9][2] + ":";
                data = data + table.Rows[9][3] + ":";
                data = data + table.Rows[9][4] + ";";
            }
            catch (Exception)
            {
                data = data + "#:";
                data = data + "#:";
                data = data + "#:";
                data = data + "#:";
                data = data + "#;";
            }
            /*if (reader.Read())
            {
                user_data[0] = (reader["username"].ToString());
                user_data[1] = (reader["score"].ToString());
                user_data[2] = (reader["rank"].ToString());
                user_data[3] = (reader["custom_prefix"].ToString());
                user_data[4] = (reader["custom_color"].ToString());
                reader.NextResult();
            }*/

            db.closeConnection();
            return data;
        }

        public string getRank(string username)
        {
            string rank = "";

            DB db = new DB();

            db.openConnection();

            DataTable table = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand command = new MySqlCommand("SELECT * FROM `users` WHERE `username` = @usn", db.getConnection());

            command.Parameters.Add("@usn", MySqlDbType.VarChar).Value = username;

            adapter.SelectCommand = command;

            adapter.Fill(table);

            MySqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                rank = (reader["rank"].ToString());
            }
            db.closeConnection();
            return rank;
        }

        public Boolean checkPassword(string username, string password)
        {
            DB db = new DB();

            DataTable table = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand command = new MySqlCommand("SELECT * FROM `users` WHERE `username` = @usn AND `password` = @pass", db.getConnection());

            command.Parameters.Add("@usn", MySqlDbType.VarChar).Value = username;
            command.Parameters.Add("@pass", MySqlDbType.VarChar).Value = password;

            adapter.SelectCommand = command;

            adapter.Fill(table);

            // tukaj preveri
            if (table.Rows.Count > 0)
            {
                db.closeConnection();
                return true;
            }
            else
            {
                db.closeConnection();
                return false;
            }
        }

        public Boolean checkUsername(string username) //Metoda, ki preveri, če uporabnikovo ime že obstaja
        {
            DB db = new DB();

            DataTable table = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand command = new MySqlCommand("SELECT * FROM `users` WHERE `username` = @usn", db.getConnection());

            command.Parameters.Add("@usn", MySqlDbType.VarChar).Value = username;

            adapter.SelectCommand = command;

            adapter.Fill(table);

            // tukaj preveri
            if (table.Rows.Count > 0)
            {
                db.closeConnection();
                return true;
            }
            else
            {
                db.closeConnection();
                return false;
            }
        }

        public Boolean checkUnbanned(string username) //Metoda, ki preveri, če uporabnikovo ime že obstaja
        {
            DB db = new DB();

            DataTable table = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand command = new MySqlCommand("SELECT * FROM `users` WHERE `username` = @usn and `rank`='banned'", db.getConnection());

            command.Parameters.Add("@usn", MySqlDbType.VarChar).Value = username;

            adapter.SelectCommand = command;

            adapter.Fill(table);

            // tukaj preveri
            if (table.Rows.Count > 0)
            {
                db.closeConnection();
                return true;
            }
            else
            {
                db.closeConnection();
                return false;
            }
        }

        public Boolean loginCheck(string username, string password)
        {
            DB db = new DB(); //deklarira podatkovno bazo

            DataTable table = new DataTable(); //deklarira tabelo

            MySqlDataAdapter adapter = new MySqlDataAdapter(); //deklarira adapter

            MySqlCommand command = new MySqlCommand("SELECT * FROM `users` WHERE `username` = @usn and `password` = BINARY @pass", db.getConnection());
            //^^ deklarira se ukaz, kater se kasneje izvede v adapterju (polje za vpisovanje ukazov)


            command.Parameters.Add("@usn", MySqlDbType.VarChar).Value = username; //Doda se parameter uporabniškega imena 
            command.Parameters.Add("@pass", MySqlDbType.VarChar).Value = password; //Doda se parameter uporabniškega gesla 
            adapter.SelectCommand = command; //Vpiše ukaz v adapter
            try
            {
                adapter.Fill(table); //Izpiše tabelo
                if (table.Rows.Count > 0) //Če v adeptrju/tabeli prikaže uporabnika z geslom in imenom, ki je vpisano v textboxih, pomeni da je uporabniško ime in geslo pravilno vpisano oz. da uporabnik že obstaja
                {
                    db.closeConnection();
                    return true;
                }
                else
                {
                    // check if the username field is empty
                    if (username.Trim().Equals(""))
                    {
                        db.closeConnection();
                        return false;
                    }
                    // check if the password field is empty
                    else if (password.Trim().Equals(""))
                    {
                        db.closeConnection();
                        return false;
                    }

                    // check if the username or the password don't exist
                    else
                    {
                        db.closeConnection();
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                db.closeConnection();
                return false;
            }
        }

        public void setScoreRank(string username)
        {

            string[] user_data = getUserData(username);

            double score = Convert.ToDouble(user_data[0]);

            string rank = "";

            if (user_data[1] != "mod" && user_data[1] != "moderator" && user_data[1] != "admin" && user_data[1] != "owner")
            {
                if (score >= 0 && score < 200)
                {
                    rank = "starter";
                }
                else if (score >= 200 && score < 500)
                {
                    rank = "amateur";
                }
                else if (score >= 500 && score < 1000)
                {
                    rank = "experienced";
                }
                else if (score >= 1000 && score < 1700)
                {
                    rank = "expert";
                }
                else if (score >= 1700 && score < 2500)
                {
                    rank = "master";
                }
                else if (score >= 2500)
                {
                    rank = "grandmaster";
                }
                DB db = new DB(); //deklarira podatkovno bazo
                db.openConnection();
                DataTable table = new DataTable(); //deklarira tabelo

                MySqlDataAdapter adapter = new MySqlDataAdapter(); //deklarira adapter

                MySqlCommand command = new MySqlCommand("UPDATE `users` SET `rank`=@rank WHERE `username` = @username", db.getConnection());
                //^^ deklarira se ukaz, kater se kasneje izvede v adapterju (polje za vpisovanje ukazov)

                command.Parameters.Add("@rank", MySqlDbType.VarChar).Value = rank;
                command.Parameters.Add("@username", MySqlDbType.VarChar).Value = username;
                object column = command.ExecuteScalar();
                db.closeConnection();
            }
        }

        #region Methods from console

        public void banUser(string username) 
        {
            DB db = new DB(); //deklarira podatkovno bazo
            db.openConnection();
            DataTable table = new DataTable(); //deklarira tabelo

            MySqlDataAdapter adapter = new MySqlDataAdapter(); //deklarira adapter

            MySqlCommand command = new MySqlCommand("UPDATE `users` SET `rank`=@option WHERE `username` = @username", db.getConnection());
            //^^ deklarira se ukaz, kater se kasneje izvede v adapterju (polje za vpisovanje ukazov)

            command.Parameters.Add("@option", MySqlDbType.VarChar).Value = "banned";
            command.Parameters.Add("@username", MySqlDbType.VarChar).Value = username;
            object column = command.ExecuteScalar();
            db.closeConnection();
        }

        public void unbanUser(string username)
        {
            setScoreRank(username);
        }

        public void setRank(string username, string rank)
        {
            DB db = new DB(); //deklarira podatkovno bazo
            db.openConnection();
            DataTable table = new DataTable(); //deklarira tabelo

            MySqlDataAdapter adapter = new MySqlDataAdapter(); //deklarira adapter

            MySqlCommand command = new MySqlCommand("UPDATE `users` SET `rank`=@option WHERE `username` = @username", db.getConnection());
            //^^ deklarira se ukaz, kater se kasneje izvede v adapterju (polje za vpisovanje ukazov)

            command.Parameters.Add("@option", MySqlDbType.VarChar).Value = rank;
            command.Parameters.Add("@username", MySqlDbType.VarChar).Value = username;
            object column = command.ExecuteScalar();
            db.closeConnection();
        }

        public void setPrefix(string username, string prefix)
        {
            DB db = new DB(); //deklarira podatkovno bazo
            db.openConnection();
            DataTable table = new DataTable(); //deklarira tabelo

            MySqlDataAdapter adapter = new MySqlDataAdapter(); //deklarira adapter

            MySqlCommand command = new MySqlCommand("UPDATE `users` SET `custom_prefix`=@option WHERE `username` = @username", db.getConnection());
            //^^ deklarira se ukaz, kater se kasneje izvede v adapterju (polje za vpisovanje ukazov)

            command.Parameters.Add("@option", MySqlDbType.VarChar).Value = prefix;
            command.Parameters.Add("@username", MySqlDbType.VarChar).Value = username;
            object column = command.ExecuteScalar();
            db.closeConnection();
        }

        public void setPrefixColor(string username, string prefix_color)
        {
            DB db = new DB(); //deklarira podatkovno bazo
            db.openConnection();
            DataTable table = new DataTable(); //deklarira tabelo

            MySqlDataAdapter adapter = new MySqlDataAdapter(); //deklarira adapter

            MySqlCommand command = new MySqlCommand("UPDATE `users` SET `custom_color`=@option WHERE `username` = @username", db.getConnection());
            //^^ deklarira se ukaz, kater se kasneje izvede v adapterju (polje za vpisovanje ukazov)

            command.Parameters.Add("@option", MySqlDbType.VarChar).Value = prefix_color;
            command.Parameters.Add("@username", MySqlDbType.VarChar).Value = username;
            object column = command.ExecuteScalar();
            db.closeConnection();
        }

        public void setScore(string username, double score)
        {
            score = Math.Round(score, 2);
            DB db = new DB(); //deklarira podatkovno bazo
            db.openConnection();
            DataTable table = new DataTable(); //deklarira tabelo

            MySqlDataAdapter adapter = new MySqlDataAdapter(); //deklarira adapter

            MySqlCommand command = new MySqlCommand("UPDATE `users` SET `score`=@option WHERE `username` = @username", db.getConnection());
            //^^ deklarira se ukaz, kater se kasneje izvede v adapterju (polje za vpisovanje ukazov)

            command.Parameters.Add("@option", MySqlDbType.Double).Value = score;
            command.Parameters.Add("@username", MySqlDbType.VarChar).Value = username;
            object column = command.ExecuteScalar();
            db.closeConnection();
            setScoreRank(username);
        }

        public void setRename(string username, string rename)
        {
            DB db = new DB(); //deklarira podatkovno bazo
            db.openConnection();
            DataTable table = new DataTable(); //deklarira tabelo

            MySqlDataAdapter adapter = new MySqlDataAdapter(); //deklarira adapter

            MySqlCommand command = new MySqlCommand("UPDATE `users` SET `changed_username`=@option WHERE `username` = @username", db.getConnection());
            //^^ deklarira se ukaz, kater se kasneje izvede v adapterju (polje za vpisovanje ukazov)

            command.Parameters.Add("@option", MySqlDbType.VarChar).Value = rename;
            command.Parameters.Add("@username", MySqlDbType.VarChar).Value = username;
            object column = command.ExecuteScalar();
            db.closeConnection();
        }

        public void setConsole(string username, string console)
        {
            DB db = new DB(); //deklarira podatkovno bazo
            db.openConnection();
            DataTable table = new DataTable(); //deklarira tabelo

            MySqlDataAdapter adapter = new MySqlDataAdapter(); //deklarira adapter

            MySqlCommand command = new MySqlCommand("UPDATE `users` SET `show_console`=@option WHERE `username` = @username", db.getConnection());
            //^^ deklarira se ukaz, kater se kasneje izvede v adapterju (polje za vpisovanje ukazov)

            command.Parameters.Add("@option", MySqlDbType.VarChar).Value = console;
            command.Parameters.Add("@username", MySqlDbType.VarChar).Value = username;
            object column = command.ExecuteScalar();
            db.closeConnection();
        }

        public string[] getInfo(string username)
        {
            string[] user_data = new string[6];

            DB db = new DB();

            db.openConnection();

            DataTable table = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand command = new MySqlCommand("SELECT * FROM `users` WHERE `username` = @usn", db.getConnection());

            command.Parameters.Add("@usn", MySqlDbType.VarChar).Value = username;

            adapter.SelectCommand = command;

            adapter.Fill(table);

            MySqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                user_data[0] = (reader["score"].ToString());
                user_data[1] = (reader["rank"].ToString());
                user_data[2] = (reader["custom_prefix"].ToString());
                user_data[3] = (reader["custom_color"].ToString());
                user_data[4] = (reader["changed_username"].ToString());
                user_data[5] = (reader["show_console"].ToString());
            }
            db.closeConnection();
            return user_data;
        }

        public void Log(string executer, string cmd, string username)
        {
            DB db = new DB();
            MySqlCommand command = new MySqlCommand("INSERT INTO `logs`(`date`, `time`,`executer`,`command`,`username`) VALUES (@date, @time, @executer,@command,@username)", db.getConnection());

            command.Parameters.Add("@date", MySqlDbType.VarChar).Value = DateTime.Now.ToString("MM/dd/yyyy");
            command.Parameters.Add("@time", MySqlDbType.VarChar).Value = DateTime.Now.ToString("HH:mm:ss");
            command.Parameters.Add("@executer", MySqlDbType.Int32).Value = Convert.ToInt32(getUserID(executer));
            command.Parameters.Add("@command", MySqlDbType.VarChar).Value = cmd;
            command.Parameters.Add("@username", MySqlDbType.Int32).Value = Convert.ToInt32(getUserID(username));
            db.openConnection();
            if (command.ExecuteNonQuery() == 1)
            {
                db.closeConnection();
            }
            else
            {
                db.closeConnection();
            }
        }

        public void Log(string executer, string cmd, string username, string option)
        {
            DB db = new DB();
            MySqlCommand command = new MySqlCommand("INSERT INTO `logs`(`date`, `time`,`executer`,`command`,`username`,`option`) VALUES (@date, @time, @executer,@command,@username,@option)", db.getConnection());

            command.Parameters.Add("@date", MySqlDbType.VarChar).Value = DateTime.Now.ToString("MM/dd/yyyy");
            command.Parameters.Add("@time", MySqlDbType.VarChar).Value = DateTime.Now.ToString("HH:mm:ss");
            command.Parameters.Add("@executer", MySqlDbType.Int32).Value = Convert.ToInt32(getUserID(executer));
            command.Parameters.Add("@command", MySqlDbType.VarChar).Value = cmd;
            command.Parameters.Add("@username", MySqlDbType.Int32).Value = Convert.ToInt32(getUserID(username));
            command.Parameters.Add("@option", MySqlDbType.VarChar).Value = option;
            db.openConnection();
            if (command.ExecuteNonQuery() == 1)
            {
                db.closeConnection();
            }
            else
            {
                db.closeConnection();
            }
        }

        #endregion

        public void matchLog(string winner, string bankrupt_data, string left_list)
        {
            DB db = new DB();
            MySqlCommand command = new MySqlCommand("INSERT INTO `match`(`date`, `time`,`winner`,`bankrupt_list`,`left_list`) VALUES (@date, @time, @winner, @bankrupt, @left)", db.getConnection());

            command.Parameters.Add("@date", MySqlDbType.VarChar).Value = DateTime.Now.ToString("MM/dd/yyyy");
            command.Parameters.Add("@time", MySqlDbType.VarChar).Value = DateTime.Now.ToString("HH:mm:ss");
            command.Parameters.Add("@winner", MySqlDbType.Int32).Value = Convert.ToInt32(getUserID(winner));
            command.Parameters.Add("@bankrupt", MySqlDbType.VarChar).Value = bankrupt_data;
            command.Parameters.Add("@left", MySqlDbType.VarChar).Value = left_list;
            db.openConnection();
            if (command.ExecuteNonQuery() == 1)
            {
                db.closeConnection();
            }
            else
            {
                db.closeConnection();
            }
        }

        #region Dispose object

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
