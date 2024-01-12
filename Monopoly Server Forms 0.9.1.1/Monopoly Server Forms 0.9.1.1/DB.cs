using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly_Server_Forms_0._9._1._1
{
    public class DB
    {
        private MySqlConnection connection = new MySqlConnection("SERVER=localhost;DATABASE=monopoly;UID=root;PASSWORD=;");


        // Funkcija, ki odpre povezavo
        public void openConnection()
        {
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
        }

        // Funkcija, ki zapre povezavo
        public void closeConnection()
        {
            if (connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
        }

        // Funkcija, ki povrne/preveri povezavo
        public MySqlConnection getConnection()
        {
            return connection;
        }
    }
}
