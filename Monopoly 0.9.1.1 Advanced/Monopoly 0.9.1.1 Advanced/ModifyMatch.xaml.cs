using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Monopoly_0._9._1._1_Advanced
{
    /// <summary>
    /// Interaction logic for ModifyMatch.xaml
    /// </summary>
    public partial class ModifyMatch : Window
    {
        public static string command = "recieve_money";
        public static string username;
        public static string value;


        public ModifyMatch()
        {
            InitializeComponent();
            foreach (Player p in Monopoly.players)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = p.Username();
                Users.Items.Add(item);
                item.IsSelected = true;
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Head_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string c =  Convert.ToString(Command.SelectedValue).Replace("System.Windows.Controls.ComboBoxItem: ",String.Empty);
            if(Value != null) 
            { 
                Value.Visibility = Visibility.Hidden;
                Value.Items.Clear();
            }
            
            
            switch (c)
            {
                case "Give money":
                    command = "recieve_money";
                    Value.Visibility = Visibility.Visible;
                    Value.IsEditable = true; 
                    ComboBoxItem item = new ComboBoxItem();
                    item.Content = "200";
                    Value.Items.Add(item);
                    item = new ComboBoxItem();
                    item.Content = "100";
                    Value.Items.Add(item);
                    item.IsSelected = true;
                    break;
                case "Take money":
                    command = "take_money";
                    Value.Visibility = Visibility.Visible;
                    Value.IsEditable = true;
                    item = new ComboBoxItem();
                    item.Content = "200";
                    Value.Items.Add(item);
                    item = new ComboBoxItem();
                    item.Content = "100";
                    Value.Items.Add(item);
                    item.IsSelected = true;
                    break;
                case "Move forward":
                    command = "move_forward";
                    Value.Visibility = Visibility.Visible;
                    Value.IsEditable = true;
                    item = new ComboBoxItem();
                    item.Content = "4";
                    Value.Items.Add(item);
                    item = new ComboBoxItem();
                    item.Content = "3";
                    Value.Items.Add(item);
                    item = new ComboBoxItem();
                    item.Content = "2";
                    Value.Items.Add(item);
                    item = new ComboBoxItem();
                    item.Content = "1";
                    Value.Items.Add(item);
                    item.IsSelected = true;
                    break;
                case "Move backward":
                    command = "move_backward";
                    Value.Visibility = Visibility.Visible;
                    Value.IsEditable = true;
                    item = new ComboBoxItem();
                    item.Content = "4";
                    Value.Items.Add(item);
                    item = new ComboBoxItem();
                    item.Content = "3";
                    Value.Items.Add(item);
                    item = new ComboBoxItem();
                    item.Content = "2";
                    Value.Items.Add(item);
                    item = new ComboBoxItem();
                    item.Content = "1";
                    Value.Items.Add(item);
                    item.IsSelected = true;
                    break;
                case "Move to":
                    command = "move_to";
                    Value.Visibility = Visibility.Visible;
                    Value.IsEditable = false;
                    foreach (Position p in Monopoly.properties)
                    {
                        item = new ComboBoxItem();
                        item.Content = p.getIndexName();
                        Value.Items.Add(item);
                        item.IsSelected = true;
                    }
                    break;
                case "Nearest utility":
                    command = "nearest_utility";
                    break;
                case "Nearest railroad":
                    command = "nearest_railroad";
                    break;
                case "Remove jail count":
                    command = "reset_jail_count";
                    break;
                case "Decrease jail count":
                    command = "decrease_jail_count";
                    break;
                case "Get out of jail free":
                    command = "get_out_of_jail_free";
                    break;
                case "Go to jail":
                    command = "go_jail";
                    break;
                case "Bankrupt":
                    command = "bankrupt";
                    break;
                case "Player turn":
                    command = "player_turn";
                    break;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            username = Convert.ToString(Users.SelectedValue).Replace("System.Windows.Controls.ComboBoxItem: ", String.Empty);
            value = Convert.ToString(Value.Text).Replace("System.Windows.Controls.ComboBoxItem: ", String.Empty);
            if (command != "move_to")
            {
                try
                {
                    int amount = Convert.ToInt32(value);

                   MessageBox.Show("Executing command> " + command + ":" + username + ":" + value + ":");

                    Monopoly.client.Send(command + ":" + username + ":" + value + ":dont_show");
                }
                catch (Exception)
                {
                    Value.Text = "0";
                }
            }
            else
            {
                MessageBox.Show("Executing command> " + command + ":" + username + ":" + value + ":");

                Monopoly.client.Send(command + ":" + username + ":" + value + ":dont_show");
            }
        }
    }
}
