using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

using WPF.Controls.Console;

namespace Monopoly_0._9._1._1_Advanced
{
    /// <summary>
    /// Interaction logic for Menu.xaml
    /// </summary>
    public partial class Menu : Window
    {
        public LoginRegister main_server_client;
        DispatcherTimer ConsoleTimer;
        public User user;
        public static Monopoly monopoly;

        public Menu(LoginRegister s)
        {
            main_server_client = s;
            user = s.user;
            InitializeComponent();
            JoinIP.Foreground = Brushes.SeaGreen;
            YourIP.Foreground = Brushes.White;
            HostPlayersNeeded.Foreground = Brushes.SeaGreen;

            NewPassword.Foreground = Brushes.SeaGreen;
            OldPassword.Foreground = Brushes.SeaGreen;
            OldPasswordBox.Foreground = Brushes.White;
            NewPasswordBox.Foreground = Brushes.White;

            Password.Foreground = Brushes.SeaGreen;
            PasswordBox.Foreground = Brushes.White;

            NewUsernameBox.Foreground = Brushes.SeaGreen;

            Console.WelcomeMessage = "Monopoly Console 0.0.1\nType 'help' for command list\n";
            Console.CommandLinePrefix = "Command";

            hideEverything();
            RulesGrid.Visibility = Visibility.Visible;
        }

        public void hideEverything() 
        {
            RulesGrid.Visibility = Visibility.Hidden;
            JoinServerGrid.Visibility = Visibility.Hidden;
            HostServerGrid.Visibility = Visibility.Hidden;
            SettingsGrid.Visibility = Visibility.Hidden;
            LeaderboardGrid.Visibility = Visibility.Hidden;
            Console.Visibility = Visibility.Hidden;
        }

        #region Window events

        private void Head_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            main_server_client.Close();
            this.Close();
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        double endpoint_number = 1;
        double increase_endpoint_number = 0.5;
        private void ConsoleTimer_Tick(object sender, EventArgs e)
        {
            LinearGradientBrush border = RainbowBorder;

            angle.Angle = endpoint_number;
            border.RelativeTransform = angle;

            ConsoleButton.BorderBrush = border;
            ConsoleButton.Foreground = border;


            if (endpoint_number < 360)
            {
                endpoint_number = endpoint_number + increase_endpoint_number;
            }
            else
            {
                endpoint_number = 0;
            }
        }

        private void Viewbox_MouseEnter(object sender, MouseEventArgs e)
        {
            increase_endpoint_number = 3;
        }

        private void Viewbox_MouseLeave(object sender, MouseEventArgs e)
        {
            increase_endpoint_number = 0.5;
        }

        #endregion

        public static Brush HexToBrush(string hex)
        {
            var converter = new BrushConverter();
            var brush = (Brush)converter.ConvertFromString(hex);
            return brush;
        }

        public bool isValidFigure(string figure)
        {
            switch (figure)
            {
                case "car":
                    return true;
                case "dog":
                    return true;
                case "iron":
                    return true;
                case "ship":
                    return true;
                case "shoe":
                    return true;
                case "thimble":
                    return true;
                case "hat":
                    return true;
                case "custom":
                    return true;
                default:
                    return false;
            }
        }

        public static string mouse_enter = "#FF75C588";
        public static string mouse_leave = "#FF75BB88";
        public static string select_figure = "#FFA5D3B1";
        public static string hover_figure = "#FFBBE0C5";

        public IPAddress IP;
        public string code;
        public int Port;

        #region Buttons MouseEnter/MouseLeave Events
        private void JoinServerButton_MouseEnter(object sender, MouseEventArgs e)
        {
            JoinServerLabel.Background = HexToBrush(mouse_enter);
        }

        private void JoinServerButton_MouseLeave(object sender, MouseEventArgs e)
        {
            JoinServerLabel.Background = HexToBrush(mouse_leave);
        }

        private void HostServerButton_MouseEnter(object sender, MouseEventArgs e)
        {
            HostServerLabel.Background = HexToBrush(mouse_enter);

        }

        private void HostServerButton_MouseLeave(object sender, MouseEventArgs e)
        {
            HostServerLabel.Background = HexToBrush(mouse_leave);

        }

        private void LeaderboardButton_MouseEnter(object sender, MouseEventArgs e)
        {
            LeaderboardLabel.Background = HexToBrush(mouse_enter);

        }

        private void LeaderboardButton_MouseLeave(object sender, MouseEventArgs e)
        {
            LeaderboardLabel.Background = HexToBrush(mouse_leave);

        }

        private void SettingsButton_MouseEnter(object sender, MouseEventArgs e)
        {
            SettingsLabel.Background = HexToBrush(mouse_enter);

        }

        private void SettingsButton_MouseLeave(object sender, MouseEventArgs e)
        {
            SettingsLabel.Background = HexToBrush(mouse_leave);

        }

        private void RulesButton_MouseEnter(object sender, MouseEventArgs e)
        {
            RulesLabel.Background = HexToBrush(mouse_enter);

        }

        private void RulesButton_MouseLeave(object sender, MouseEventArgs e)
        {
            RulesLabel.Background = HexToBrush("#FF75BB88");

        }


        #endregion

        #region Buttons MouseDown Events

        private void JoinServerButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            hideEverything();
            JoinError.Text = "";
            JoinServerGrid.Visibility = Visibility.Visible;
        }

        private void HostServerButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            hideEverything();
            HostError.Text = "";
            HostServerGrid.Visibility = Visibility.Visible;
        }

        private void RulesButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            hideEverything();
            RulesGrid.Visibility = Visibility.Visible;
        }

        private void LeaderboardButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            main_server_client.Send("request_leaderboard_data");
            hideEverything();
            LeaderboardGrid.Visibility = Visibility.Visible;
        }

        private void SettingsButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            hideEverything();
            SettingsGrid.Visibility = Visibility.Visible;

            NewUsernameBox.Text = "New Username";
            NewUsernameBox.Foreground = Brushes.SeaGreen;

            NewPassword.Visibility = Visibility.Visible;
            NewPasswordBox.Visibility = Visibility.Hidden;
            NewPasswordBox.Password = "";

            OldPassword.Visibility = Visibility.Visible;
            OldPasswordBox.Visibility = Visibility.Hidden;
            OldPasswordBox.Password = "";

            Password.Visibility = Visibility.Visible;
            PasswordBox.Visibility = Visibility.Hidden;
            PasswordBox.Password = "";

            

            ChangePasswordError.Text = "";
            ChangeUsernameError.Text = "";

            this.Focus();

        }

        //Console click
        private void ConsoleButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            hideEverything();
            Console.Visibility = Visibility.Visible;
            Console.Focus();
        }

        #endregion

        private void selectRemoveBorders() 
        {
            Thickness no = new Thickness(0);
            JoinSelectCar.BorderThickness = no;
            JoinSelectCar.Background = Brushes.Transparent;
            HostSelectCar.BorderThickness = no;
            HostSelectCar.Background = Brushes.Transparent;

            JoinSelectDog.BorderThickness = no;
            JoinSelectDog.Background = Brushes.Transparent;
            HostSelectDog.BorderThickness = no;
            HostSelectDog.Background = Brushes.Transparent;

            JoinSelectHat.BorderThickness = no;
            JoinSelectHat.Background = Brushes.Transparent;
            HostSelectHat.BorderThickness = no;
            HostSelectHat.Background = Brushes.Transparent;

            JoinSelectShip.BorderThickness = no;
            JoinSelectShip.Background = Brushes.Transparent;
            HostSelectShip.BorderThickness = no;
            HostSelectShip.Background = Brushes.Transparent;

            JoinSelectIron.BorderThickness = no;
            JoinSelectIron.Background = Brushes.Transparent;
            HostSelectIron.BorderThickness = no;
            HostSelectIron.Background = Brushes.Transparent;

            JoinSelectShoe.BorderThickness = no;
            JoinSelectShoe.Background = Brushes.Transparent;
            HostSelectShoe.BorderThickness = no;
            HostSelectShoe.Background = Brushes.Transparent;

            JoinSelectThimble.BorderThickness = no;
            JoinSelectThimble.Background = Brushes.Transparent;
            HostSelectThimble.BorderThickness = no;
            HostSelectThimble.Background = Brushes.Transparent;

            JoinSelectCustom.BorderThickness = no;
            JoinSelectCustom.Background = Brushes.Transparent;
            HostSelectCustom.BorderThickness = no;
            HostSelectCustom.Background = Brushes.Transparent;
        }

        #region Join Select Events

        private void JoinSelectCar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            selectRemoveBorders();
            JoinSelectCar.Background = HexToBrush(select_figure);
            JoinSelectCar.BorderBrush = Brushes.Gray;
            JoinSelectCar.BorderThickness = new Thickness(1);
            HostSelectCar.Background = HexToBrush(select_figure);
            HostSelectCar.BorderBrush = Brushes.Gray;
            HostSelectCar.BorderThickness = new Thickness(1);
            user.Figure = "car";
        }

        private void JoinSelectDog_MouseDown(object sender, MouseButtonEventArgs e)
        {
            selectRemoveBorders();
            JoinSelectDog.Background = HexToBrush(select_figure);
            JoinSelectDog.BorderBrush = Brushes.Gray;
            JoinSelectDog.BorderThickness = new Thickness(1);
            HostSelectDog.Background = HexToBrush(select_figure);
            HostSelectDog.BorderBrush = Brushes.Gray;
            HostSelectDog.BorderThickness = new Thickness(1);
            
            user.Figure = "dog";
        }

        private void JoinSelectHat_MouseDown(object sender, MouseButtonEventArgs e)
        {
            selectRemoveBorders();
            JoinSelectHat.Background = HexToBrush(select_figure);
            JoinSelectHat.BorderBrush = Brushes.Gray;
            JoinSelectHat.BorderThickness = new Thickness(1);
            HostSelectHat.Background = HexToBrush(select_figure);
            HostSelectHat.BorderBrush = Brushes.Gray;
            HostSelectHat.BorderThickness = new Thickness(1);

            user.Figure = "hat";
        }

        private void JoinSelectShip_MouseDown(object sender, MouseButtonEventArgs e)
        {
            selectRemoveBorders();
            JoinSelectShip.Background = HexToBrush(select_figure);
            JoinSelectShip.BorderBrush = Brushes.Gray;
            JoinSelectShip.BorderThickness = new Thickness(1);
            HostSelectShip.Background = HexToBrush(select_figure);
            HostSelectShip.BorderBrush = Brushes.Gray;
            HostSelectShip.BorderThickness = new Thickness(1);

            user.Figure = "ship";
        }

        private void JoinSelectIron_MouseDown(object sender, MouseButtonEventArgs e)
        {
            selectRemoveBorders();
            JoinSelectIron.Background = HexToBrush(select_figure);
            JoinSelectIron.BorderBrush = Brushes.Gray;
            JoinSelectIron.BorderThickness = new Thickness(1);
            HostSelectIron.Background = HexToBrush(select_figure);
            HostSelectIron.BorderBrush = Brushes.Gray;
            HostSelectIron.BorderThickness = new Thickness(1);

            user.Figure = "iron";
        }

        private void JoinSelectShoe_MouseDown(object sender, MouseButtonEventArgs e)
        {
            selectRemoveBorders();
            JoinSelectShoe.Background = HexToBrush(select_figure);
            JoinSelectShoe.BorderBrush = Brushes.Gray;
            JoinSelectShoe.BorderThickness = new Thickness(1);
            HostSelectShoe.Background = HexToBrush(select_figure);
            HostSelectShoe.BorderBrush = Brushes.Gray;
            HostSelectShoe.BorderThickness = new Thickness(1);

            user.Figure = "shoe";
        }

        private void JoinSelectThimble_MouseDown(object sender, MouseButtonEventArgs e)
        {
            selectRemoveBorders();
            JoinSelectThimble.Background = HexToBrush(select_figure);
            JoinSelectThimble.BorderBrush = Brushes.Gray;
            JoinSelectThimble.BorderThickness = new Thickness(1);
            HostSelectThimble.Background = HexToBrush(select_figure);
            HostSelectThimble.BorderBrush = Brushes.Gray;
            HostSelectThimble.BorderThickness = new Thickness(1);

            user.Figure = "thimble";
        }

        private void JoinSelectCustom_MouseDown(object sender, MouseButtonEventArgs e)
        {
            selectRemoveBorders();
            JoinSelectCustom.Background = HexToBrush(select_figure);
            JoinSelectCustom.BorderBrush = Brushes.Gray;
            JoinSelectCustom.BorderThickness = new Thickness(1);
            HostSelectCustom.Background = HexToBrush(select_figure);
            HostSelectCustom.BorderBrush = Brushes.Gray;
            HostSelectCustom.BorderThickness = new Thickness(1);

            user.Figure = "custom";
        }

        private void JoinSelectCar_MouseEnter(object sender, MouseEventArgs e)
        {
            if(user.Figure != "car")
                JoinSelectCar.Background = HexToBrush(hover_figure);
        }

        private void JoinSelectCar_MouseLeave(object sender, MouseEventArgs e)
        {
            if (user.Figure != "car")
                JoinSelectCar.Background = Brushes.Transparent;
        }

        private void JoinSelectDog_MouseEnter(object sender, MouseEventArgs e)
        {
            if (user.Figure != "dog")
                JoinSelectDog.Background = HexToBrush(hover_figure);
        }

        private void JoinSelectDog_MouseLeave(object sender, MouseEventArgs e)
        {
            if (user.Figure != "dog")
                JoinSelectDog.Background = Brushes.Transparent;
        }

        private void JoinSelectHat_MouseEnter(object sender, MouseEventArgs e)
        {
            if (user.Figure != "hat")
                JoinSelectHat.Background = HexToBrush(hover_figure);
        }

        private void JoinSelectHat_MouseLeave(object sender, MouseEventArgs e)
        {
            if (user.Figure != "hat")
                JoinSelectHat.Background = Brushes.Transparent;
        }

        private void JoinSelectShip_MouseEnter(object sender, MouseEventArgs e)
        {
            if (user.Figure != "ship")
                JoinSelectShip.Background = HexToBrush(hover_figure);
        }

        private void JoinSelectShip_MouseLeave(object sender, MouseEventArgs e)
        {
            if (user.Figure != "ship")
                JoinSelectShip.Background = Brushes.Transparent;
        }

        private void JoinSelectIron_MouseEnter(object sender, MouseEventArgs e)
        {
            if (user.Figure != "iron")
                JoinSelectIron.Background = HexToBrush(hover_figure);
        }

        private void JoinSelectIron_MouseLeave(object sender, MouseEventArgs e)
        {
            if (user.Figure != "iron")
                JoinSelectIron.Background = Brushes.Transparent;
        }

        private void JoinSelectShoe_MouseEnter(object sender, MouseEventArgs e)
        {
            if (user.Figure != "shoe")
                JoinSelectShoe.Background = HexToBrush(hover_figure);
        }

        private void JoinSelectShoe_MouseLeave(object sender, MouseEventArgs e)
        {
            if (user.Figure != "shoe")
                JoinSelectShoe.Background = Brushes.Transparent;
        }

        private void JoinSelectThimble_MouseEnter(object sender, MouseEventArgs e)
        {
            if (user.Figure != "thimble")
                JoinSelectThimble.Background = HexToBrush(hover_figure);
        }

        private void JoinSelectThimble_MouseLeave(object sender, MouseEventArgs e)
        {
            if (user.Figure != "thimble")
                JoinSelectThimble.Background = Brushes.Transparent;
        }

        private void JoinSelectCustom_MouseEnter(object sender, MouseEventArgs e)
        {
            if (user.Figure != "custom")
                JoinSelectCustom.Background = HexToBrush(hover_figure);
        }

        private void JoinSelectCustom_MouseLeave(object sender, MouseEventArgs e)
        {
            if (user.Figure != "custom")
                JoinSelectCustom.Background = Brushes.Transparent;
        }

        #endregion

        #region Join Events

        public async void joinMatch(string code) 
        {
            try
            {
                // await Task.Delay(1000);
                SendGame("game:" + code + ":requesting_to_join:" + user.Username + ":" + user.Figure + ":");
            }
            catch (Exception rc)
            {
                JoinError.Text = "Server not found";
                JoinError.Text = rc.Message;
            }
        }

        private async void Join_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Join.IsEnabled = false;
            if (isValidFigure(user.Figure))
            {
                //main_server_client.Send("requesting_join_ip:" + JoinIP.Text + ":");
                //join_code = JoinIP.Text;
                joinMatch(JoinIP.Text);
            }
            else
            {
                JoinError.Text = "You must select figure before joining";
            }
            await Task.Delay(1500);
            Join.IsEnabled = true;
            if (JoinError.Text == "")
                JoinError.Text = "Server not found";
        }

        private void Join_MouseEnter(object sender, MouseEventArgs e)
        {
            Join.Background = HexToBrush(mouse_enter);
        }

        private void Join_MouseLeave(object sender, MouseEventArgs e)
        {
            Join.Background = HexToBrush(mouse_leave);
        }

        private void JoinIP_GotFocus(object sender, RoutedEventArgs e)
        {
            if (JoinIP.Text.ToLower().Trim().Equals("code"))
            {
                JoinIP.Text = "";
                JoinIP.Foreground = Brushes.White;
            }
        }

        private void JoinIP_LostFocus(object sender, RoutedEventArgs e)
        {
            if (JoinIP.Text.ToLower().Trim().Equals("code") || JoinIP.Text.Trim().Equals(""))
            {
                JoinIP.Text = "Code";
                JoinIP.Foreground = Brushes.SeaGreen;
            }
        }

        private void NumberValidation(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void NumberValidationDot(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+.");
            e.Handled = regex.IsMatch(e.Text);
        }

        #endregion

        #region Methods from server

        #region Leadeboard methods

        public void setLeadboardStats(string score, TextBlock ScoreText, string rank, TextBlock RankText, string position, TextBlock PositionText) 
        {
            ScoreText.TextEffects.Clear();
            RankText.TextEffects.Clear();
            PositionText.TextEffects.Clear();

            PositionText.Text = position;
            RankText.Text = rank.Substring(0, 1).ToUpper() + rank.Substring(1).ToLower();
            RankText.Foreground = getRankColor(rank);

            ScoreText.Text = score;
        }

        public void setLeadboardUsername(string rank, string prefix, string prefix_color, string username ,TextBlock UsernameText) 
        {
            UsernameText.TextEffects.Clear();

            if (prefix != "none")
            {
                TextEffect te = new TextEffect();
                te.PositionStart = 1;
                te.PositionCount = prefix.Length;
                te.Foreground = getColor(prefix, prefix_color);
                UsernameText.Text = "[" + prefix.ToUpper() + "] " + username;
                UsernameText.TextEffects.Add(te);
            }
            else if (prefix_color != "none")
            {
                TextEffect te = new TextEffect();
                te.PositionStart = 1;
                te.PositionCount = rank.Length;
                te.Foreground = getColor(prefix, prefix_color);
                UsernameText.Text = "[" + rank.ToUpper() + "] " + username;
                UsernameText.TextEffects.Add(te);
            }
            else
            {
                UsernameText.Text = username;
            }
        }

        public void updateLeaderboard(string data)
        {
            string[] user_data = data.Split(';');
            string[] player1 = user_data[0].Split(':');
            string[] player2 = user_data[1].Split(':');
            string[] player3 = user_data[2].Split(':');
            string[] player4 = user_data[3].Split(':');
            string[] player5 = user_data[4].Split(':');
            string[] player6 = user_data[5].Split(':');
            string[] player7 = user_data[6].Split(':');
            string[] player8 = user_data[7].Split(':');
            string[] player9 = user_data[8].Split(':');
            string[] player10 = user_data[9].Split(':');

            if (!player1[1].Contains("#"))
            {
                setLeadboardStats(player1[2], number1Score, player1[3], number1Rank, "#1", number1);
                setLeadboardUsername(player1[3], player1[4], player1[5], player1[1], number1Username);
            }
            else
            {
                number1.Text = "";
                number1Score.Text = "";
                number1Rank.Text = "";
                number1Username.Text = "";
            }

            if (!player1[1].Contains("#"))
            {
                setLeadboardStats(player1[2], number1Score, player1[3], number1Rank, "#1", number1);
                setLeadboardUsername(player1[3], player1[4], player1[5], player1[1], number1Username);
            }
            else
            {
                number1.Text = "";
                number1Score.Text = "";
                number1Rank.Text = "";
                number1Username.Text = "";
            }

            if (!player1[1].Contains("#"))
            {
                setLeadboardStats(player1[2], number1Score, player1[3], number1Rank, "#1", number1);
                setLeadboardUsername(player1[3], player1[4], player1[5], player1[1], number1Username);
            }
            else
            {
                number1.Text = "";
                number1Score.Text = "";
                number1Rank.Text = "";
                number1Username.Text = "";
            }

            if (!player2[0].Contains("#"))
            {
                setLeadboardStats(player2[1], number2Score, player2[2], number2Rank, "#2", number2);
                setLeadboardUsername(player2[2], player2[3], player2[4], player2[0], number2Username);
            }
            else
            {
                number2.Text = "";
                number2Score.Text = "";
                number2Rank.Text = "";
                number2Username.Text = "";
            }

            if (!player3[0].Contains("#"))
            {
                setLeadboardStats(player3[1], number3Score, player3[2], number3Rank, "#3", number3);
                setLeadboardUsername(player3[2], player3[3], player3[4], player3[0], number3Username);
            }
            else
            {
                number3.Text = "";
                number3Score.Text = "";
                number3Rank.Text = "";
                number3Username.Text = "";
            }

            if (!player4[0].Contains("#"))
            {
                setLeadboardStats(player4[1], number4Score, player4[2], number4Rank, "#4", number4);
                setLeadboardUsername(player4[2], player4[3], player4[4], player4[0], number4Username);
            }
            else
            {
                number4.Text = "";
                number4Score.Text = "";
                number4Rank.Text = "";
                number4Username.Text = "";
            }

            if (!player5[0].Contains("#"))
            {
                setLeadboardStats(player5[1], number5Score, player5[2], number5Rank, "#5", number5);
                setLeadboardUsername(player5[2], player5[3], player5[4], player5[0], number5Username);
            }
            else
            {
                number5.Text = "";
                number5Score.Text = "";
                number5Rank.Text = "";
                number5Username.Text = "";
            }

            if (!player6[0].Contains("#"))
            {
                setLeadboardStats(player6[1], number6Score, player6[2], number6Rank, "#6", number6);
                setLeadboardUsername(player6[2], player6[3], player6[4], player6[0], number6Username);
            }
            else
            {
                number6.Text = "";
                number6Score.Text = "";
                number6Rank.Text = "";
                number6Username.Text = "";
            }

            if (!player7[0].Contains("#"))
            {
                setLeadboardStats(player7[1], number7Score, player7[2], number7Rank, "#7", number7);
                setLeadboardUsername(player7[2], player7[3], player7[4], player7[0], number7Username);
            }
            else
            {
                number7.Text = "";
                number7Score.Text = "";
                number7Rank.Text = "";
                number7Username.Text = "";
            }

            if (!player8[0].Contains("#"))
            {
                setLeadboardStats(player8[1], number8Score, player8[2], number8Rank, "#8", number8);
                setLeadboardUsername(player8[2], player8[3], player8[4], player8[0], number8Username);
            }
            else
            {
                number8.Text = "";
                number8Score.Text = "";
                number8Rank.Text = "";
                number8Username.Text = "";
            }

            if (!player9[0].Contains("#"))
            {
                setLeadboardStats(player9[1], number9Score, player9[2], number9Rank, "#9", number9);
                setLeadboardUsername(player9[2], player9[3], player9[4], player9[0], number9Username);
            }
            else
            {
                number9.Text = "";
                number9Score.Text = "";
                number9Rank.Text = "";
                number9Username.Text = "";
            }

            if (!player10[0].Contains("#"))
            {
                setLeadboardStats(player10[1], number10Score, player10[2], number10Rank, "#10", number10);
                setLeadboardUsername(player10[2], player10[3], player10[4], player10[0], number10Username);
            }
            else
            {
                number10.Text = "";
                number10Score.Text = "";
                number10Rank.Text = "";
                number10Username.Text = "";
            }

        }
        #endregion

        public void updateUser(string data)
        {
            string[] user_data = data.Split(':');
            user.Score = Convert.ToDouble(user_data[1]);
            user.IP = user_data[2];
            user.Code = user_data[3];
            user.Rank = user_data[4];
            user.Prefix = user_data[5];
            user.PrefixColor = user_data[6];
            user.ShowConsole = user_data[7];

            Welcome.TextEffects.Clear();

            YourScore.Text = "Score: " + user.Score;
            YourIP.Text = user.Code;

            if (user.Rank == "owner" || user.Rank == "admin" || user.Rank == "mod" || user.Rank == "moderator" || user.ShowConsole == "true")
            {
                if (ConsoleButtonVisible.Visibility == Visibility.Hidden)
                {
                    ConsoleTimer = new DispatcherTimer();
                    ConsoleTimer.Tick += ConsoleTimer_Tick;
                    ConsoleTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
                    ConsoleTimer.Start();
                    ConsoleButtonVisible.Visibility = Visibility.Visible;
                }
            }
            else
            {
                ConsoleButtonVisible.Visibility = Visibility.Hidden;
            }

            if (user.Rank == "owner")
            {
                JoinSelectCustom.Visibility = Visibility.Visible;
                HostSelectCustom.Visibility = Visibility.Visible;
            }
            else
            {
                JoinSelectCustom.Visibility = Visibility.Hidden;
                HostSelectCustom.Visibility = Visibility.Hidden;
            }

            if (user.Prefix == "none")
            {
                Welcome.Text = "Welcome, [" + user.Rank.ToUpper() + "] " + user.Username;
                TextEffect rank = new TextEffect();
                rank.PositionStart = 10;
                rank.PositionCount = user.Rank.Length;
                rank.Foreground = getColor(user.Rank, user.PrefixColor);
                Welcome.TextEffects.Add(rank);
            }
            else
            {
                Welcome.Text = "Welcome, [" + user.Prefix.ToUpper() + "] " + user.Username;
                TextEffect rank = new TextEffect();
                rank.PositionStart = 10;
                rank.PositionCount = user.Prefix.Length;
                rank.Foreground = getColor(user.Rank, user.PrefixColor);
                Welcome.TextEffects.Add(rank);
            }
        }

        public static Brush getRankColor(string rank)
        {
            Brush b = Brushes.Black;
            if (rank == "owner")
                b = Brushes.Red;
            else if (rank == "admin")
                b = Brushes.Red;
            else if (rank == "mod")
                b = Brushes.Green;
            else if (rank == "moderator")
                b = Brushes.Green;
            else if (rank == "starter")
                b = Brushes.Black;
            else if (rank == "amateur")
                b = Brushes.Teal;
            else if (rank == "experienced")
                b = Brushes.Navy;
            else if (rank == "expert")
                b = Brushes.BlueViolet;
            else if (rank == "master")
                b = Brushes.Goldenrod;
            else if (rank == "grandmaster")
                b = Brushes.DarkTurquoise;
            return b;
        }

        public static Brush getColor(string prefix, string prefix_color)
        {
            if (prefix_color.Contains("animation/")) //animation-custom-255-0-0-custom-0-0-0-10000
            {
                string[] prefix_colors = prefix_color.Split('/');
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
                animation_color.BeginAnimation(SolidColorBrush.ColorProperty, animation);

                return animation_color;
            }
            else if (prefix_color.Contains("custom/")) //custom-255-255-255
            {
                string[] prefix_colors = prefix_color.Split('/');
                byte red = Convert.ToByte(prefix_colors[1]);
                byte green = Convert.ToByte(prefix_colors[2]);
                byte blue = Convert.ToByte(prefix_colors[3]);
                Color c = Color.FromRgb(red, green, blue);
                return new SolidColorBrush(c);
            }
            else 
            {
                try
                {
                    BrushConverter converter = new BrushConverter();
                    SolidColorBrush color = converter.ConvertFromString(prefix_color) as SolidColorBrush;
                    return color;
                }
                catch (Exception)
                {
                    Brush b = Brushes.Black;

                    //ranks
                    b = getRankColor(prefix);

                    return b;
                }
            }
        }

        public void declinedToJoin(string data)
        {
            string[] info = data.Split(':');
            JoinError.Text = info[1];
        }

        public void acceptedToJoin(string text)
        {
            string[] data = text.Split(':');
            monopoly = new Monopoly(this, data[1], false, user);
            monopoly.Show();
            this.Hide();
        }

        #endregion

        #region client to server

        public void Data(string data)//Metoda, ki preveri kaj je ukaz in nato to izvede
        {
            Dispatcher.Invoke((Action)delegate
            {
                //output.AppendText("\n" + data);
                
                if (data.Contains("declined_to_join:"))
                {
                    declinedToJoin(data);
                }
                else if (data.Contains("accepted_to_join:"))
                {
                    acceptedToJoin(data);
                }
            });
        }

        private void SendCallbackGame(IAsyncResult AR) //Metoda s katero odjemalec neha pošiljati podatke
        {

            try
            {
                LoginRegister.client.EndSend(AR);
            }
            catch (SocketException ex)
            {
                //Error(ex.Message, MethodBase.GetCurrentMethod().Name);
            }
            catch (ObjectDisposedException ex)
            {
                // Error(ex.Message, MethodBase.GetCurrentMethod().Name);
            }
            catch (Exception ex)
            {
                //Error(ex.Message, MethodBase.GetCurrentMethod().Name);
            }
        }

        private void SendGame(string data)//Metoda, ki omogoči pošiljanje podatkov 
        {
            try
            {
                string encrypt_data = LoginRegister.Encrypt(data);
                byte[] data_send = Encoding.ASCII.GetBytes(encrypt_data);
                LoginRegister.client.BeginSend(data_send, 0, data_send.Length, SocketFlags.None, SendCallbackGame, null);
            }
            catch (SocketException ex)
            {
                //Error(ex.Message, MethodBase.GetCurrentMethod().Name);
            }
            catch (ObjectDisposedException ex)
            {
                //Error(ex.Message, MethodBase.GetCurrentMethod().Name);
            }
        }
        byte[] data;
        private void ReceiveCallbackGame(IAsyncResult AR) //Metoda s katero se prejme podatke od strežnika
        {
            try
            {
                int received = LoginRegister.client.EndReceive(AR);

                if (received == 0)
                {
                    return;
                }


                string text = Encoding.ASCII.GetString(data);
                data = new byte[LoginRegister.client.ReceiveBufferSize];
                Data(text);

                //Še enkrat začne pridobivati podatke od strežnika 
                LoginRegister.client.BeginReceive(data, 0, data.Length, SocketFlags.None, ReceiveCallbackGame, null);
            }
            catch (SocketException ex)
            {
                //Error(ex.Message, MethodBase.GetCurrentMethod().Name);
            }
            catch (ObjectDisposedException ex)
            {
                //Error(ex.Message, MethodBase.GetCurrentMethod().Name);
            }
        }

        private void ConnectCallbackGame(IAsyncResult AR)//Ko se odjemalec povežen na server, je ta metoda zato, da začenja pridobivati podatke od strežnika
        {
            try
            {
                LoginRegister.client.EndConnect(AR);
                data = new byte[LoginRegister.client.ReceiveBufferSize];
                LoginRegister.client.BeginReceive(data, 0, data.Length, SocketFlags.None, ReceiveCallbackGame, null);
            }
            catch (Exception ex)
            {
                //Error(ex.Message, MethodBase.GetCurrentMethod().Name);
            }
        }




        #endregion

        #region Host Select Events

        private void HostSelectCar_MouseEnter(object sender, MouseEventArgs e)
        {
            if (user.Figure != "car")
                HostSelectCar.Background = HexToBrush(hover_figure);
        }

        private void HostSelectCar_MouseLeave(object sender, MouseEventArgs e)
        {
            if (user.Figure != "car")
                HostSelectCar.Background = Brushes.Transparent;
        }

        private void HostSelectDog_MouseEnter(object sender, MouseEventArgs e)
        {
            if (user.Figure != "dog")
                HostSelectDog.Background = HexToBrush(hover_figure);
        }

        private void HostSelectDog_MouseLeave(object sender, MouseEventArgs e)
        {
            if (user.Figure != "dog")
                HostSelectDog.Background = Brushes.Transparent;
        }

        private void HostSelectHat_MouseEnter(object sender, MouseEventArgs e)
        {
            if (user.Figure != "hat")
                HostSelectHat.Background = HexToBrush(hover_figure);
        }

        private void HostSelectHat_MouseLeave(object sender, MouseEventArgs e)
        {
            if (user.Figure != "hat")
                HostSelectHat.Background = Brushes.Transparent;
        }

        private void HostSelectShip_MouseEnter(object sender, MouseEventArgs e)
        {
            if (user.Figure != "ship")
                HostSelectShip.Background = HexToBrush(hover_figure);
        }

        private void HostSelectShip_MouseLeave(object sender, MouseEventArgs e)
        {
            if (user.Figure != "ship")
                HostSelectShip.Background = Brushes.Transparent;
        }

        private void HostSelectIron_MouseEnter(object sender, MouseEventArgs e)
        {
            if (user.Figure != "iron")
                HostSelectIron.Background = HexToBrush(hover_figure);
        }

        private void HostSelectIron_MouseLeave(object sender, MouseEventArgs e)
        {
            if (user.Figure != "iron")
                HostSelectIron.Background = Brushes.Transparent;
        }

        private void HostSelectShoe_MouseEnter(object sender, MouseEventArgs e)
        {
            if (user.Figure != "shoe")
                HostSelectShoe.Background = HexToBrush(hover_figure);
        }

        private void HostSelectShoe_MouseLeave(object sender, MouseEventArgs e)
        {
            if (user.Figure != "shoe")
                HostSelectShoe.Background = Brushes.Transparent;
        }

        private void HostSelectThimble_MouseEnter(object sender, MouseEventArgs e)
        {
            if (user.Figure != "thimble")
                HostSelectThimble.Background = HexToBrush(hover_figure);
        }

        private void HostSelectThimble_MouseLeave(object sender, MouseEventArgs e)
        {
            if (user.Figure != "thimble")
                HostSelectThimble.Background = Brushes.Transparent;
        }

        private void HostSelectCustom_MouseEnter(object sender, MouseEventArgs e)
        {
            if (user.Figure != "custom")
                HostSelectCustom.Background = HexToBrush(hover_figure);
        }

        private void HostSelectCustom_MouseLeave(object sender, MouseEventArgs e)
        {
            if (user.Figure != "custom")
                HostSelectCustom.Background = Brushes.Transparent;
        }

        #endregion

        #region Host Events

        private void HostPlayersNeeded_GotFocus(object sender, RoutedEventArgs e)
        {
            if (HostPlayersNeeded.Text.ToLower().Trim().Equals("how many players"))
            {
                HostPlayersNeeded.Text = "";
                HostPlayersNeeded.Foreground = Brushes.White;
            }
        }

        private void HostPlayersNeeded_LostFocus(object sender, RoutedEventArgs e)
        {
            if (HostPlayersNeeded.Text.ToLower().Trim().Equals("how many players") || HostPlayersNeeded.Text.Trim().Equals(""))
            {
                HostPlayersNeeded.Text = "How many players";
                HostPlayersNeeded.Foreground = Brushes.SeaGreen;
            }
        }

        private void HostPort_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (Convert.ToInt32(HostPlayersNeeded.Text) > 65535)
                {
                    HostPlayersNeeded.Text = "65535";
                }
            }
            catch (Exception)
            {

            }
            HostPlayersNeeded.CaretIndex = HostPlayersNeeded.Text.Length;
        }

        private void HostPort_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (Convert.ToInt32(HostPlayersNeeded.Text) > 65535)
                {
                    HostPlayersNeeded.Text = "65535";
                }
            }
            catch (Exception)
            {

            }
            HostPlayersNeeded.CaretIndex = HostPlayersNeeded.Text.Length;
        }

        private void HostPlayersNeeded_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (Convert.ToInt32(HostPlayersNeeded.Text) > 6)
                {
                    HostPlayersNeeded.Text = "6";
                }
                if (Convert.ToInt32(HostPlayersNeeded.Text) < 2)
                {
                    HostPlayersNeeded.Text = "2";
                }
            }
            catch (Exception)
            {

            }
            HostPlayersNeeded.CaretIndex = HostPlayersNeeded.Text.Length;
        }

        private void HostPlayersNeeded_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (Convert.ToInt32(HostPlayersNeeded.Text) > 6)
                {
                    HostPlayersNeeded.Text = "6";
                }
                if (Convert.ToInt32(HostPlayersNeeded.Text) < 2)
                {
                    HostPlayersNeeded.Text = "2";
                }
            }
            catch (Exception)
            {

            }
            HostPlayersNeeded.CaretIndex = HostPlayersNeeded.Text.Length;
        }


        private void Host_MouseEnter(object sender, MouseEventArgs e)
        {
            Host.Background = HexToBrush(mouse_enter);
        }

        private void Host_MouseLeave(object sender, MouseEventArgs e)
        {
            Host.Background = HexToBrush(mouse_leave);
        }

        private void Host_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (isValidFigure(user.Figure))
            {
                int players_needed = 0;
                try
                {
                    players_needed = Convert.ToInt32(HostPlayersNeeded.Text);
                }
                catch (Exception)
                {

                }

                if (players_needed > 6 )
                {
                    players_needed = 6;
                }
                else if (players_needed < 2)
                {
                    players_needed = 2;
                }

                try
                {
                    // HOST SERVER
                    SendGame("host_server:" + players_needed + ":" + user.Figure + ":");

                    //monopoly = new Monopoly(this, user.IP, Convert.ToInt32(HostPort.Text), players_needed, true, user);
                    //monopoly.Show();
                    //this.Hide();
                }
                catch (Exception eeee)
                {
                    HostError.Text = "You need to set how many players are needed to start game.";
                    MessageBox.Show(eeee.Message);
                }
            }
            else
            {
                HostError.Text = "No character selected";
            }
        }

        public void HostGame()
        {
            monopoly = new Monopoly(this, user.Code, true, user);
            monopoly.Show();
            this.Hide();
        }

        #endregion

        #region Settings methods
        private void OldPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            OldPassword.Visibility = Visibility.Hidden;
            OldPasswordBox.Visibility = Visibility.Visible;
            OldPasswordBox.Focus();
        }

        private void NewPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            NewPassword.Visibility = Visibility.Hidden;
            NewPasswordBox.Visibility = Visibility.Visible;
            NewPasswordBox.Focus();
        }

        private void OldPasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (OldPasswordBox.Password.Trim().Equals(""))
            {
                OldPassword.Visibility = Visibility.Visible;
                OldPasswordBox.Visibility = Visibility.Hidden;
                Head.Focus();
            }
        }

        private void NewPasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (NewPasswordBox.Password.Trim().Equals(""))
            {
                NewPassword.Visibility = Visibility.Visible;
                NewPasswordBox.Visibility = Visibility.Hidden;
                Head.Focus();
            }
        }

        private void ChangePassword_MouseEnter(object sender, MouseEventArgs e)
        {
            ChangePassword.Background = HexToBrush(mouse_enter);
        }

        private void ChangePassword_MouseLeave(object sender, MouseEventArgs e)
        {
            ChangePassword.Background = HexToBrush(mouse_leave);
        }

        private void ChangePassword_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (NewPasswordBox.Password.Length > 5)
            {
                main_server_client.Send("user_change_password:" + user.Username + ":" + OldPasswordBox.Password + ":" + NewPasswordBox.Password + ":");
            }
            else 
            {
                ChangePasswordError.Foreground = Brushes.Red;
                ChangePasswordError.Text = "New password too short.";
            }
        }

        private void PasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (PasswordBox.Password.Trim().Equals(""))
            {
                Password.Visibility = Visibility.Visible;
                PasswordBox.Visibility = Visibility.Hidden;
                Head.Focus();
            }
        }

        private void Password_GotFocus(object sender, RoutedEventArgs e)
        {
            PasswordBox.Visibility = Visibility.Visible;
            Password.Visibility = Visibility.Hidden;
            PasswordBox.Focus();
        }

        private void NewUsernameBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (NewUsernameBox.Text.ToLower().Trim().Equals("new username"))
            {
                NewUsernameBox.Text = "";
                NewUsernameBox.Foreground = Brushes.White;
            }
        }

        private void NewUsernameBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (NewUsernameBox.Text.ToLower().Trim().Equals("new username") || NewUsernameBox.Text.Trim().Equals(""))
            {
                NewUsernameBox.Text = "New Username";
                NewUsernameBox.Foreground = Brushes.SeaGreen;
            }
        }

        private void ChangeUsername_MouseEnter(object sender, MouseEventArgs e)
        {
            ChangeUsername.Background = HexToBrush(mouse_enter);
        }

        private void ChangeUsername_MouseLeave(object sender, MouseEventArgs e)
        {
            ChangeUsername.Background = HexToBrush(mouse_leave);
        }

        private void ChangeUsername_MouseDown(object sender, MouseButtonEventArgs e)
        {
            string username = NewUsernameBox.Text.Replace(" ", String.Empty);
            if (username.Length > 3 && !NewUsernameBox.Text.ToLower().Trim().Equals("new username"))
            {
                main_server_client.Send("user_change_username:" + user.Username + ":" + username + ":" + PasswordBox.Password + ":");
            }
            else
            {
                ChangeUsernameError.Foreground = Brushes.Red;
                ChangeUsernameError.Text = "Your new username is too short";
            }
        }

        private void NewUsernameBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (NewUsernameBox.Text.Length < 12)
            {
                e.Handled = true;
                switch (e.Key) //Odspodaj je določeno kaj se zapiše v textbox glede na pritisk gumba
                {
                    case Key.OemPeriod:
                        e.Handled = false; break;
                    case Key.Q: //Primer: Če je na tipkovnici pritisnjen "q", v textbox izpiše "q"
                        e.Handled = false; break;
                    case Key.W:
                        e.Handled = false; break;
                    case Key.E:
                        e.Handled = false; break;
                    case Key.R:
                        e.Handled = false; break;
                    case Key.T:
                        e.Handled = false; break;
                    case Key.Z:
                        e.Handled = false; break;
                    case Key.U:
                        e.Handled = false; break;
                    case Key.I:
                        e.Handled = false; break;
                    case Key.O:
                        e.Handled = false; break;
                    case Key.P:
                        e.Handled = false; break;
                    case Key.A:
                        e.Handled = false; break;
                    case Key.S:
                        e.Handled = false; break;
                    case Key.D:
                        e.Handled = false; break;
                    case Key.F:
                        e.Handled = false; break;
                    case Key.G:
                        e.Handled = false; break;
                    case Key.H:
                        e.Handled = false; break;
                    case Key.J:
                        e.Handled = false; break;
                    case Key.K:
                        e.Handled = false; break;
                    case Key.L:
                        e.Handled = false; break;
                    case Key.Y:
                        e.Handled = false; break;
                    case Key.X:
                        e.Handled = false; break;
                    case Key.C:
                        e.Handled = false; break;
                    case Key.V:
                        e.Handled = false; break;
                    case Key.B:
                        e.Handled = false; break;
                    case Key.N:
                        e.Handled = false; break;
                    case Key.M:
                        e.Handled = false; break;
                    case Key.D0:
                        if (NewUsernameBox.Text.Length > 0)
                            e.Handled = false; break;
                    case Key.D1:
                        if (NewUsernameBox.Text.Length > 0)
                            e.Handled = false; break;
                    case Key.D2:
                        if (NewUsernameBox.Text.Length > 0)
                            e.Handled = false; break;
                    case Key.D3:
                        if (NewUsernameBox.Text.Length > 0)
                            e.Handled = false; break;
                    case Key.D4:
                        if (NewUsernameBox.Text.Length > 0)
                            e.Handled = false; break;
                    case Key.D5:
                        if (NewUsernameBox.Text.Length > 0)
                            e.Handled = false; break;
                    case Key.D6:
                        if (NewUsernameBox.Text.Length > 0)
                            e.Handled = false; break;
                    case Key.D7:
                        if (NewUsernameBox.Text.Length > 0)
                            e.Handled = false; break;
                    case Key.D8:
                        if (NewUsernameBox.Text.Length > 0)
                            e.Handled = false; break;
                    case Key.D9:
                        if (NewUsernameBox.Text.Length > 0)
                            e.Handled = false; break;
                    case Key.NumPad0:
                        if (NewUsernameBox.Text.Length > 0)
                            e.Handled = false; break;
                    case Key.NumPad1:
                        if (NewUsernameBox.Text.Length > 0)
                            e.Handled = false; break;
                    case Key.NumPad2:
                        if (NewUsernameBox.Text.Length > 0)
                            e.Handled = false; break;
                    case Key.NumPad3:
                        if (NewUsernameBox.Text.Length > 0)
                            e.Handled = false; break;
                    case Key.NumPad4:
                        if (NewUsernameBox.Text.Length > 0)
                            e.Handled = false; break;
                    case Key.NumPad5:
                        if (NewUsernameBox.Text.Length > 0)
                            e.Handled = false; break;
                    case Key.NumPad6:
                        if (NewUsernameBox.Text.Length > 0)
                            e.Handled = false; break;
                    case Key.NumPad7:
                        if (NewUsernameBox.Text.Length > 0)
                            e.Handled = false; break;
                    case Key.NumPad8:
                        if (NewUsernameBox.Text.Length > 0)
                            e.Handled = false; break;
                    case Key.NumPad9:
                        if (NewUsernameBox.Text.Length > 0)
                            e.Handled = false; break;
                    default:
                        e.Handled = true;
                        break;
                }
            }
            NewUsernameBox.Text = NewUsernameBox.Text.Replace(" ", String.Empty);
            NewUsernameBox.CaretIndex = NewUsernameBox.Text.Length; //Cursor v textboxu premakne na konec.
        }

        private void NewUsernameBox_KeyUp(object sender, KeyEventArgs e)
        {
            NewUsernameBox.Text = NewUsernameBox.Text.Replace(" ", String.Empty);
            NewUsernameBox.CaretIndex = NewUsernameBox.Text.Length; //Cursor v textboxu premakne na konec.
        }


        #endregion

        #region Console

        public bool show_command = false;
        private void Console_Execute(object sender, ExecuteEventArgs e)
        {
            string[] argument = e.Command.Split(' ');
            string command = argument[0];
            string you = user.Username;

            switch (command)
            {
                case "ban":
                    try 
                    {
                        string user = argument[1];
                        main_server_client.Send("console_command:" + you + ";ban:" + user + ":");
                    }
                    catch(Exception)
                    {
                        validArguments("ban [username]");
                    }
                    break;
                case "unban":
                    try
                    {
                        string user = argument[1];
                        main_server_client.Send("console_command:" + you + ";unban:" + user + ":");
                    }
                    catch (Exception)
                    {
                        validArguments("unban [username]");
                    }
                    break;
                case "kick":
                    try
                    {
                        string username = argument[1];
                        main_server_client.Send("console_command:" + you + ";kick:" + username + ":");
                    }
                    catch (Exception)
                    {
                        validArguments("kick [username]");
                    }
                    break;
                case "set":
                    string option = "";
                    try
                    {
                        option = argument[1];
                        try
                        {
                            string username = argument[2];
                            if (option == "rank")
                            {
                                string rank = argument[3];
                                main_server_client.Send("console_command:" + you + ";set_rank:" + username + ":" + rank + ":");
                            }
                            else if (option == "prefix")
                            {
                                string prefix = argument[3];
                                main_server_client.Send("console_command:" + you + ";set_prefix:" + username + ":" + prefix + ":");
                            }
                            else if (option == "prefix_color")
                            {
                                string prefix_color = argument[3];
                                main_server_client.Send("console_command:" + you + ";set_prefix_color:" + username + ":" + prefix_color + ":");
                            }
                            else if (option == "score")
                            {
                                string score = argument[3];
                                main_server_client.Send("console_command:" + you + ";set_score:" + username + ":" + score + ":");
                            }
                            else if (option == "rename")
                            {
                                string rename_option = argument[3];
                                main_server_client.Send("console_command:" + you + ";set_rename:" + username + ":" + rename_option + ":");
                            }
                            else if (option == "console")
                            {
                                string console_option = argument[3];
                                main_server_client.Send("console_command:" + you + ";set_console:" + username + ":" + console_option + ":");
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("\nInvalid command arguments");
                            Console.WriteLine("Command set accepts following arguments:");
                            Console.WriteLine("  set rank [username] [rank]");
                            Console.WriteLine("  set prefix [username] [prefix]");
                            Console.WriteLine("  set prefix_color [username] [color]");
                            Console.WriteLine("  set score [username] [score]");
                            Console.WriteLine("  set rename [username] [true/false]");
                            Console.WriteLine("  set console [username] [true/false]\n");
                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("\nInvalid command arguments");
                        Console.WriteLine("Command set accepts following arguments:");
                        Console.WriteLine("  set rank [username] [rank]");
                        Console.WriteLine("  set prefix [username] [prefix]");
                        Console.WriteLine("  set prefix_color [username] [color]");
                        Console.WriteLine("  set score [username] [score]");
                        Console.WriteLine("  set rename [username] [true/false]");
                        Console.WriteLine("  set console [username] [true/false]\n");
                    }
                    break;

                case "get":
                    try
                    {
                        option = argument[1];
                        if (argument[1] == "info")
                        {
                            try 
                            {
                                string username = argument[2];
                                main_server_client.Send("console_command:" + you + ";get_info:" + username + ":");
                            }
                            catch (Exception)
                            {
                                validArguments("get info [username]");
                            }
                        }
                        else
                        {
                            validArguments("get info [username]");
                        }
                    }
                    catch (Exception)
                    {
                        validArguments("get info [username]");
                    }
                    break;

                case "customize":
                    
                    break;

                case "cls":
                case "clear":
                    Console.Clear();
                    break;
                case "help":
                    if (e.Command == "help")
                    {
                        Console.WriteLine("\nList of commands:");
                        Console.WriteLine("┌──────────────────────────────────────────────────────────────────────┐");
                        Console.WriteLine("│     cls                      - clears console                        │");
                        Console.WriteLine("├──────────────────────────────────────────────────────────────────────┤");
                        Console.WriteLine("│     clear                    - clears console                        │");
                        Console.WriteLine("├──────────────────────────────────────────────────────────────────────┤");
                        Console.WriteLine("│     ban                      - bans user                             │");
                        Console.WriteLine("├──────────────────────────────────────────────────────────────────────┤");
                        Console.WriteLine("│     unban                    - unbans user                           │");
                        Console.WriteLine("├──────────────────────────────────────────────────────────────────────┤");
                        Console.WriteLine("│     kick                     - kicks user                            │");
                        Console.WriteLine("├──────────────────────────────────────────────────────────────────────┤");
                        Console.WriteLine("│     set rank                 - sets rank for user                    │");
                        Console.WriteLine("├──────────────────────────────────────────────────────────────────────┤");
                        Console.WriteLine("│     set prefix               - set prefix for user                   │");
                        Console.WriteLine("├──────────────────────────────────────────────────────────────────────┤");
                        Console.WriteLine("│     set prefix_color         - set prefix color for user             │");
                        Console.WriteLine("├──────────────────────────────────────────────────────────────────────┤");
                        Console.WriteLine("│     set score                - set score for user                    │");
                        Console.WriteLine("├──────────────────────────────────────────────────────────────────────┤");
                        Console.WriteLine("│     set rename               - set if user can change his username   │");
                        Console.WriteLine("├──────────────────────────────────────────────────────────────────────┤");
                        Console.WriteLine("│     set console              - set if user can see and use console   │");
                        Console.WriteLine("├──────────────────────────────────────────────────────────────────────┤");
                        Console.WriteLine("│     get info                 - get info about user                   │");
                        Console.WriteLine("├──────────────────────────────────────────────────────────────────────┤");
                        Console.WriteLine("│     customize                - customize your UI                     │");
                        Console.WriteLine("├──────────────────────────────────────────────────────────────────────┤");
                        Console.WriteLine("│     help [command]           - shows help for specific command       │");
                        Console.WriteLine("└──────────────────────────────────────────────────────────────────────┘");
                        Console.Update();
                    }
                    else
                    {
                        switch (e.Command)
                        {
                            case "help ban":
                                Console.WriteLine("\nHelp for command 'ban':");
                                Console.WriteLine("┌──────────────────────────────────────────────────────────────────────┐");
                                Console.WriteLine("│     Correct formats:                                                 │");
                                Console.WriteLine("│      - ban [username]                                                │");
                                Console.WriteLine("│     Description:                                                     │");
                                Console.WriteLine("│        If you ban person, person will be kicked out of game          │");
                                Console.WriteLine("│        and he wont be able to connect anymore.                       │");
                                Console.WriteLine("└──────────────────────────────────────────────────────────────────────┘");
                                Console.Update();
                                break;
                            case "help kick":
                                Console.WriteLine("\nHelp for command 'kick':");
                                Console.WriteLine("┌──────────────────────────────────────────────────────────────────────┐");
                                Console.WriteLine("│     Correct formats:                                                 │");
                                Console.WriteLine("│      - kick [username]                                               │");
                                Console.WriteLine("│     Description:                                                     │");
                                Console.WriteLine("│        If you kick person, person will be kicked out of game         │");
                                Console.WriteLine("│        and he will be able to reconnect.                             │");
                                Console.WriteLine("└──────────────────────────────────────────────────────────────────────┘");
                                Console.Update();
                                break;
                            case "help unban":
                                Console.WriteLine("\nHelp for command 'unban':");
                                Console.WriteLine("┌──────────────────────────────────────────────────────────────────────┐");
                                Console.WriteLine("│     Correct formats:                                                 │");
                                Console.WriteLine("│      - unban [username]                                              │");
                                Console.WriteLine("│     Description:                                                     │");
                                Console.WriteLine("│        If you unban person, person's rank will be set based on       │");
                                Console.WriteLine("│        his score.                                                    │");
                                Console.WriteLine("└──────────────────────────────────────────────────────────────────────┘");
                                Console.Update();
                                break;
                            case "help set rank":
                                Console.WriteLine("\nHelp for command 'set rank':");
                                Console.WriteLine("┌──────────────────────────────────────────────────────────────────────┐");
                                Console.WriteLine("│     Correct formats:                                                 │");
                                Console.WriteLine("│      - set rank [username] [rank]                                    │");
                                Console.WriteLine("│     Description:                                                     │");
                                Console.WriteLine("│        If you set rank to a person, rank will be overriden when      │");
                                Console.WriteLine("│        person finishes a match (will be set based on score),         │");
                                Console.WriteLine("│        Useful if want to set rank to admin, mod etc.                 │");
                                Console.WriteLine("└──────────────────────────────────────────────────────────────────────┘");
                                Console.Update();
                                break;
                            case "help set prefix":
                                Console.WriteLine("\nHelp for command 'set prefix':");
                                Console.WriteLine("┌──────────────────────────────────────────────────────────────────────┐");
                                Console.WriteLine("│     Correct formats:                                                 │");
                                Console.WriteLine("│      - set prefix [username] [prefix]                                │");
                                Console.WriteLine("│     Description:                                                     │");
                                Console.WriteLine("│        If you set prefix to username, prefix will be shown before    │");
                                Console.WriteLine("│        users username.                                               │");
                                Console.WriteLine("└──────────────────────────────────────────────────────────────────────┘");
                                Console.Update();
                                break;
                            case "help set prefix_color":
                                Console.WriteLine("\nHelp for command 'set prefix_color':");
                                Console.WriteLine("┌──────────────────────────────────────────────────────────────────────┐");
                                Console.WriteLine("│     Correct formats:                                                 │");
                                Console.WriteLine("│      - set prefix_color [username] [color]                           │");
                                Console.WriteLine("│     Correct color formats:                                           │");
                                Console.WriteLine("│      - animation/custom/[R]/[G]/[B]/custom/[R]/[G]/[B]/[Time]        │");
                                Console.WriteLine("│      - custom/[R]/[G]/[B]                                            │");
                                Console.WriteLine("│      - [color name]                                                  │");
                                Console.WriteLine("│      - none                                                          │");
                                Console.WriteLine("│     Description:                                                     │");
                                Console.WriteLine("│        Prefix color changes color of prefix.                         │");
                                Console.WriteLine("│        Animation color changes color from one color to another       │");
                                Console.WriteLine("│        and backwards in certain time which is set on last parameter. │");
                                Console.WriteLine("│        If you set color to 'none' user wont have prefix color.       │");
                                Console.WriteLine("└──────────────────────────────────────────────────────────────────────┘");
                                Console.Update();
                                break;
                            case "help set score":
                                Console.WriteLine("\nHelp for command 'set score':");
                                Console.WriteLine("┌──────────────────────────────────────────────────────────────────────┐");
                                Console.WriteLine("│     Correct formats:                                                 │");
                                Console.WriteLine("│      - set score [username] [score]                                  │");
                                Console.WriteLine("│     Description:                                                     │");
                                Console.WriteLine("│        When you set score to user, it automaticly updates his rank.  │");
                                Console.WriteLine("└──────────────────────────────────────────────────────────────────────┘");
                                Console.Update();
                                break;
                            case "help set rename":
                                Console.WriteLine("\nHelp for command 'set rename':");
                                Console.WriteLine("┌──────────────────────────────────────────────────────────────────────┐");
                                Console.WriteLine("│     Correct formats:                                                 │");
                                Console.WriteLine("│      - set rename [username] [true/false]                            │");
                                Console.WriteLine("│     Description:                                                     │");
                                Console.WriteLine("│        Every user has ability to change his username once.           │");
                                Console.WriteLine("│        Set rename to true if want to give ability to change          │");
                                Console.WriteLine("│        username or set it to false to take it away.                  │");
                                Console.WriteLine("└──────────────────────────────────────────────────────────────────────┘");
                                Console.Update();
                                break;
                            case "help set console":
                                Console.WriteLine("\nHelp for command 'set console':");
                                Console.WriteLine("┌──────────────────────────────────────────────────────────────────────┐");
                                Console.WriteLine("│     Correct formats:                                                 │");
                                Console.WriteLine("│      - set console [username] [true/false]                           │");
                                Console.WriteLine("│     Description:                                                     │");
                                Console.WriteLine("│        Only owners,admins,mods,moderators have access to console.    │");
                                Console.WriteLine("│        If user doesn't have access to console this parameter msut    │");
                                Console.WriteLine("│        be set to true.                                               │");
                                Console.WriteLine("│     IMPORTANT: If user can see console, he doesn't have access       │");
                                Console.WriteLine("│        to commands. Only command he can access is 'customize' and    │");
                                Console.WriteLine("│        'get info' (does not output any passwords)                    │");
                                Console.WriteLine("│        Every command issued is based on rank. So if user is not      │");
                                Console.WriteLine("│        admin he cannot change scores.                                │");
                                Console.WriteLine("└──────────────────────────────────────────────────────────────────────┘");
                                Console.Update();
                                break;
                            case "help get info":
                                Console.WriteLine("\nHelp for command 'get info':");
                                Console.WriteLine("┌──────────────────────────────────────────────────────────────────────┐");
                                Console.WriteLine("│     Correct formats:                                                 │");
                                Console.WriteLine("│      - get info [username]                                           │");
                                Console.WriteLine("│     Description:                                                     │");
                                Console.WriteLine("│        Gets and outputs the info of user                             │");
                                Console.WriteLine("└──────────────────────────────────────────────────────────────────────┘");
                                Console.Update();
                                break;
                            default:
                                Console.WriteLine("\nCommand not found. Type 'help' for command list\n");
                                Console.Update();
                                break;
                        }
                    }
                    break;
                default:
                    Console.WriteLine("\nCommand not found. Type 'help' for command list\n"); 
                    Console.Update();
                    break;
            }
            updateCaret();
        }

        public void validArguments(string validArgument)
        {
            Console.WriteLine("\nInvalid command arguments");
            Console.WriteLine("Command set accepts following arguments:");
            Console.WriteLine("  " + validArgument + "\n");
            Console.Update();
        }

        public void ConsoleOutput(string message)
        {
            Console.WriteLine(Environment.NewLine + message + Environment.NewLine);
        }

        public void ConsoleOutputNoNewLine(string message)
        {
            Console.WriteLine(message);
        }

        public void updateCaret() 
        {
            while (Console.CaretIndex() < Console.CaretIndexMax())
            {
                Console.UpdateConsoleCaret();
                Console.Update();
            }
        }
        #endregion

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            if (!(monopoly is null))
            {
                monopoly.Close();
            }
            if (LoginRegister.kicked != true)
            {
                main_server_client.Close();
            }
            /*if (inGame == true)
            {
                monopoly.Close();
            }*/
        }

    }
}
