using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Monopoly_0._9._1._1_Advanced
{
    public class Player
    {
        string username;
        string figure;
        string rank;
        string custom_prefix;
        string custom_color;

        int balance = 1000;
        int position;
        int jail = 0;
        bool been_in_jail = false;
        bool bankrupt = false;
        int get_out_of_jail_free = 0;

        public Player(string username, string figure, string rank, string custom_prefix, string custom_color)
        {
            this.username = username;
            this.figure = figure;
            this.rank = rank;
            this.custom_prefix = custom_prefix;
            this.custom_color = custom_color;
            this.position = 0;
        }
        public Player()
        {
            this.username = "";
            this.figure = "no figure selected";
            this.position = 0;
        }

        public string Username()
        {
            return username;
        }

        public string Figure()
        {
            return figure;
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

        public int Balance
        {
            get { return balance; }
            set { balance = value; }
        }
        public int JailCount
        {
            get { return jail; }
            set { jail = value; }
        }
        public int Position
        {
            get { return position; }
            set { position = value; }
        }

        public bool WasInJail
        {
            get { return been_in_jail; }
            set { been_in_jail = value; }
        }

        public bool Bankrupt
        {
            get { return bankrupt; }
            set { bankrupt = value; }
        }

        public int GetOutOfJailFree
        {
            get { return get_out_of_jail_free; }
            set { get_out_of_jail_free = value; }
        }

    }
}
