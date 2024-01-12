using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Monopoly_0._9._1._1_Advanced
{
    class Chance
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

        public Chance()
        {
            Random r = new Random();

            switch (r.Next(1, 16))
            {
                case 1:
                    index = 1;
                    caption = "CHANCE";
                    description = "Advance to GO. Collect $200";
                    command = "move_to:" + Monopoly.username + ":GO!:";
                    break;
                case 2:
                    index = 2;
                    caption = "CHANCE";
                    description = "Advance to Illinois Avenue";
                    command = "move_to:" + Monopoly.username + ":ILLINOIS AVENUE:";
                    break;
                case 3:
                    index = 3;
                    caption = "CHANCE";
                    description = "Advance to ST.Charles place";
                    command = "move_to:" + Monopoly.username + ":ST.CHARLES PLACE:";
                    break;
                case 4:
                    index = 4;
                    caption = "CHANCE";
                    description = "Advance token to the nearest Railroad and pay owner twice the rental to which he/she is otherwise entitled. \nIf Railroad is unowned, you may buy it from the Bank.";
                    command = "nearest_railroad:" + Monopoly.username + ":" + "true:";
                    break;
                case 5:
                    index = 5;
                    caption = "CHANCE";
                    description = "Advance token to nearest Utility. \nIf unowned, you may buy it from the Bank. \nIf owned, throw dice and pay owner a total 10 times the amount thrown.";
                    command = "nearest_utility:" + Monopoly.username + ":";
                    break;
                case 6:
                    index = 6;
                    caption = "CHANCE";
                    description = "Bank pays you dividend of $50.";
                    command = "recieve_money:" + Monopoly.username + ":" + 50 + ":";
                    break;
                case 7:
                    index = 7;
                    caption = "CHANCE";
                    description = "Get Out of Jail Free";
                    command = "get_out_of_jail_free:" + Monopoly.username + ":";
                    break;
                case 8:
                    index = 8;
                    caption = "CHANCE";
                    description = "Go back three spaces";
                    command = "move_back:" + Monopoly.username + ":" + 3 + ":";
                    break;
                case 9:
                    index = 9;
                    caption = "CHANCE";
                    description = "Go directly to jail. Do not pass Go, Do not collect $200";
                    command = "go_jail:" + Monopoly.username + ":";
                    break;
                case 10:
                    index = 10;
                    caption = "CHANCE";
                    description = "Make general repairs on all your property: \nFor each house pay $25, For each hotel pay $100.";
                    command = "pay_money_per_buildings:" + Monopoly.username + ":" + 25 + ":" + 100 + ":";
                    break;
                case 11:
                    index = 11;
                    caption = "CHANCE";
                    description = "Speeding fine $15";
                    command = "take_money:" + Monopoly.username + ":" + 15 + ":";
                    break;
                case 12:
                    index = 12;
                    caption = "CHANCE";
                    description = "Take a trip to Reading Railroad.";
                    command = "move_to:" + Monopoly.username + ":READING RAILROAD:";
                    break;
                case 13:
                    index = 13;
                    caption = "CHANCE";
                    description = "Advance to Boardwalk";
                    command = "move_to:" + Monopoly.username + ":BOARDWALK:";
                    break;
                case 14:
                    index = 14;
                    caption = "CHANCE";
                    description = "Your building loan matures. Collect $150.";
                    command = "recieve_money:" + Monopoly.username + ":" + 150 + ":";
                    break;
                case 15:
                    index = 15;
                    caption = "CHANCE";
                    description = "You have been elected Chairman of the Board. Pay each player $50.";
                    command = "pay_each_player:" + Monopoly.username + ":" + 50 + ":";
                    break;
            }
        }
    }
}
