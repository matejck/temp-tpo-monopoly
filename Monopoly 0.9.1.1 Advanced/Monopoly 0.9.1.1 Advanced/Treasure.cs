using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Monopoly_0._9._1._1_Advanced
{
    class Treasure
    {
        string description;
        string caption;
        string command;
        int index;
        MessageBoxImage icon = MessageBoxImage.Information;
        MessageBoxButton buttons = MessageBoxButton.OK;

        public string Description()
        {
            return description;
        }
        public string Caption()
        {
            return caption;
        }
        public string Command()
        {
            return command;
        }
        public int GetIndex()
        {
            return index;
        }
        public MessageBoxImage Icon()
        {
            return icon;
        }
        public MessageBoxButton Buttons()
        {
            return buttons;
        }

        public Treasure()
        {
            Random r = new Random();

            switch (r.Next(1, 18))
            {
                case 1:
                    index = 1;
                    caption = "COMMUNITY CHEST";
                    description = "Advance to GO. Collect $200";
                    command = "move_to:" + Monopoly.username + ":GO!:";
                    break;
                case 2:
                    index = 2;
                    caption = "COMMUNITY CHEST";
                    description = "Doctor's fees. Pay $50";
                    command = "take_money:" + Monopoly.username + ":" + 50 + ":";
                    break;
                case 3:
                    index = 3;
                    caption = "COMMUNITY CHEST";
                    description = "From sale of stock you get $50";
                    command = "recieve_money:" + Monopoly.username + ":" + 50 + ":";
                    break;
                case 4:
                    index = 4;
                    caption = "COMMUNITY CHEST";
                    description = "Bank error in your favor. Collect $200";
                    command = "recieve_money:" + Monopoly.username + ":" + 200 + ":";
                    break;
                case 5:
                    index = 5;
                    caption = "COMMUNITY CHEST";
                    description = "Get Out of Jail Free";
                    command = "get_out_of_jail_free:" + Monopoly.username + ":";
                    break;
                case 6:
                    index = 6;
                    caption = "COMMUNITY CHEST";
                    description = "Go directly to jail. Do not pass Go, Do not collect $200";
                    command = "go_jail:" + Monopoly.username + ":";
                    break;
                case 7:
                    index = 7;
                    caption = "COMMUNITY CHEST";
                    description = "Collect $50 from every player for opening night seats.";
                    command = "collect_money_per_player:" + Monopoly.username + ":" + 50 + ":";
                    break;
                case 8:
                    index = 8;
                    caption = "COMMUNITY CHEST";
                    description = "Holiday Fund matures. Collect $100";
                    command = "recieve_money:" + Monopoly.username + ":" + 100 + ":";
                    break;
                case 9:
                    index = 9;
                    caption = "COMMUNITY CHEST";
                    description = "Income tax refund. Collect $20";
                    command = "recieve_money:" + Monopoly.username + ":" + 20 + ":";
                    break;
                case 10:
                    index = 10;
                    caption = "COMMUNITY CHEST";
                    description = "It's your birthday. Collect $10 from every player";
                    command = "collect_money_per_player:" + Monopoly.username + ":" + 10 + ":";
                    break;
                case 11:
                    index = 11;
                    caption = "COMMUNITY CHEST";
                    description = "Life insurance matures. Collect $100";
                    command = "recieve_money:" + Monopoly.username + ":" + 100 + ":";
                    break;
                case 12:
                    index = 12;
                    caption = "COMMUNITY CHEST";
                    description = "Hospital fees, Pay $100";
                    command = "take_money:" + Monopoly.username + ":" + 100 + ":";
                    break;
                case 13:
                    index = 13;
                    caption = "COMMUNITY CHEST";
                    description = "School fees, Pay $50";
                    command = "take_money:" + Monopoly.username + ":" + 50 + ":";
                    break;
                case 14:
                    index = 14;
                    caption = "COMMUNITY CHEST";
                    description = "Recieve $25 consultancy fee";
                    command = "take_money:" + Monopoly.username + ":" + 25 + ":";
                    break;
                case 15:
                    index = 15;
                    caption = "COMMUNITY CHEST";
                    description = "You are assessed for street repairs:\nPay $40 per house and $115 per hotel you own";
                    command = "pay_money_per_buildings:" + Monopoly.username + ":" + 40 + ":" + 115 + ":";
                    break;
                case 16:
                    index = 16;
                    caption = "COMMUNITY CHEST";
                    description = "You have won second prize in a beautiy contest.\nCollect $10";
                    command = "recieve_money:" + Monopoly.username + ":" + 10 + ":";
                    break;
                case 17:
                    index = 17;
                    caption = "COMMUNITY CHEST";
                    description = "You inherit $100";
                    command = "recieve_money:" + Monopoly.username + ":" + 100 + ":";
                    break;
            }
        }
    }
}
