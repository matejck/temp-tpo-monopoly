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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Monopoly_0._9._1._1_Advanced
{
    /// <summary>
    /// Interaction logic for xaml
    /// </summary>
    public partial class Monopoly : Window
    {
        public Monopoly(Menu m, string code, bool isHosting, User user)
        {

            //IP = "192.168.1.4";

            username = user.Username;
            figure = user.Figure;
            menu = m;

            statsWindow = new StatsWindow(this);
            InitializeComponent();
            //statsWindow.Show();
            statsWindow.ShowInTaskbar = true;

            players = new List<Player>();
            properties = new List<Position>();
            figures = new List<Canvas>();

            is_hosting = isHosting;
            server_closed = false;
            started = false;


            your_turn = false;
            rent_paid = false;
            dice_amount = 0;
            dice_enabled = false;
            double_rent = 1;
            utility_chance_rent = false;
            started = false;
            chance_enabled = false;
            treasure_enabled = false;


            /*if (isHosting)
            {
                server = new ServerWindow(code, players_needed);
                //server.Show();
            }*/

            Treasure.Cursor = Cursors.No;
            Chance.Cursor = Cursors.No;

            client = new ClientWindow(this, code);
            //client.Show();
            setIndexesPositions();
            setIndexesHover();
            if (user.Rank == "owner" || user.Rank == "admin")
            {
                SettingsIcon.Visibility = Visibility.Visible;
            }
            else
            {
                SettingsIcon.Visibility = Visibility.Hidden;
            }
            client.SendAddPlayer(user.Username, user.Figure, user.Rank, user.Prefix, user.PrefixColor);
            WaitingForPlayers("Waiting for players ");
            statsWindow.updateYourPosition();
        }

        #region Variables

        //Spremenljivke za igralca
        public static string username = "";
        public static string figure = "";
        public static bool your_turn = false;
        public static bool rent_paid = false;
        public static int dice_amount = 0;
        public static bool dice_enabled = false;
        public static int double_rent = 1;
        public static bool utility_chance_rent = false;
        public static bool started = false;
        public static bool chance_enabled = false;
        public static bool treasure_enabled = false;

        bool is_hosting;
        bool server_closed = false;
        bool game_over = false;

        public static ClientWindow client;
        static StatsWindow statsWindow; //novo okno, katero kaže statistiko igralcev, pozicij itd.
        public static Menu menu; //novo okno, katero kaže statistiko igralcev, pozicij itd.

        public static List<Player> players;
        public static List<Position> properties;
        public static List<Canvas> figures;

        private static int req_expires;
        private static string req_requester;
        private static string req_u;
        private static string req_index_name;
        private static int req_amount;
        private static DispatcherTimer request_timer;
        public static DispatcherTimer end_turn_timer = new DispatcherTimer();
        private static int endTurnMinutes = 0;
        private static int endTurnSeconds = 0;

        #endregion

        #region Events (MouseDown,Click,Unloaded...)

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            if (is_hosting == false)
            {
                client.Send("exit_from_game");
                /*ClientWindow.client.Close();
                client.Close();*/
            }
            else
            {
                //ClientWindow.client.Close();
                //client.Close();

                /*server.Broadcast("close_server");
                ServerWindow.CloseServer();
                server.Close();*/

                // CLOSE SERVER

            }

            statsWindow.Close();

            try
            {
                menu.Show();
                menu.JoinError.Text = "";
                menu.HostError.Text = "";
            }
            catch (Exception)
            {

            }
        }

        public void TreasureVisible() 
        {
            Treasure.Visibility = Visibility.Visible;
        }

        public void ChanceVisible()
        {
            Chance.Visibility = Visibility.Visible;
        }

        private void Treasure_MouseEnter(object sender, MouseEventArgs e)
        {
            if (treasure_enabled)
            {
                TreasureBorder.BorderThickness = new Thickness(2);
            }
        }
        
        private void Treasure_MouseLeave(object sender, MouseEventArgs e)
        {
            TreasureBorder.BorderThickness = new Thickness(0);
        }

        private void Chance_MouseEnter(object sender, MouseEventArgs e)
        {
            if(chance_enabled)
            {
                ChanceBorder.BorderThickness = new Thickness(2);
            }
        }

        private void Chance_MouseLeave(object sender, MouseEventArgs e)
        {
            ChanceBorder.BorderThickness = new Thickness(0);
        }

        private void dice1_MouseEnter(object sender, MouseEventArgs e)
        {
            if (dice_enabled)
            {
                dice1border.BorderThickness = new Thickness(1);
                dice1.Cursor = Cursors.Hand;
            }
            else
            {
                dice1.Cursor = Cursors.Arrow;
            }
        }

        private void dice1_MouseLeave(object sender, MouseEventArgs e)
        {
            dice1border.BorderThickness = new Thickness(0);
        }

        private void dice2_MouseEnter(object sender, MouseEventArgs e)
        {
            if (dice_enabled)
            {
                dice2border.BorderThickness = new Thickness(1);
                dice2.Cursor = Cursors.Hand;
            }
            else
            {
                dice2.Cursor = Cursors.Arrow;
            }
        }

        private void dice2_MouseLeave(object sender, MouseEventArgs e)
        {
            dice2border.BorderThickness = new Thickness(0);
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                statsWindow.WindowState = WindowState.Normal;
                this.WindowState = WindowState.Normal;
            }
            else if (WindowState == WindowState.Normal)
            {
                statsWindow.WindowState = WindowState.Minimized;
                this.WindowState = WindowState.Minimized;
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Head_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            foreach (Canvas f in figures)
            {
                f.Height = this.Height / 30;
                f.Width = this.Width / 30;
            }
        }

        private async void Treasure_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (treasure_enabled)
            {
                treasure_enabled = false;
                Treasure.Cursor = Cursors.No;
                Treasure t = new Treasure();
                client.Send("treasure_show:" + t.GetIndex() + ":");
                await Task.Delay(100);
                MessageBox.Show(t.Description(), t.Caption(), t.Buttons(), t.Icon());
                client.Send(t.Command());
                if (!t.Command().Contains("move_to") && !t.Command().Contains("nearest_railroad") && !t.Command().Contains("nearest_utility") && !t.Command().Contains("move_back"))
                    statsWindow.EndTurn.Visibility = Visibility.Visible;
                await Task.Delay(1000);
                client.Send("treasure_hide");
            }
        }

        private async void Chance_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(chance_enabled)
            {
                chance_enabled = false;
                Chance.Cursor = Cursors.No;
                Chance s = new Chance();
                client.Send("chance_show:" + s.GetIndex() + ":");
                await Task.Delay(100);
                MessageBox.Show(s.Description(), s.Caption(), s.Buttons(), s.Icon());
                client.Send(s.Command());
                if (!s.Command().Contains("move_to") && !s.Command().Contains("nearest_railroad") && !s.Command().Contains("nearest_utility") && !s.Command().Contains("move_back"))
                    statsWindow.EndTurn.Visibility = Visibility.Visible;
                if (s.Description().Contains("Advance token to nearest Utility."))
                {
                    utility_chance_rent = true;
                    dice_enabled = false;
                }
                await Task.Delay(1000);
                client.Send("chance_hide");
            }
        }

        private void dice1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(dice_enabled)
                generationDice();
            dice_enabled = false;
        }

        private void dice2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(dice_enabled)
                generationDice();
            dice_enabled = false;
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Normal)
            {
                statsWindow.WindowState = WindowState.Normal;
                statsWindow.Topmost = true;
                this.Topmost = true;
            }
            else if (WindowState == WindowState.Minimized)
            {
                statsWindow.WindowState = WindowState.Minimized;
                statsWindow.Topmost = false;
                this.Topmost = false;
            }
        }

        private bool is_rotating = false;

        private async void Settings_MouseLeave(object sender, MouseEventArgs e)
        {
            if (is_rotating == false)
            {
                is_rotating = true;
                for (int i = 0; i < 18; i++)
                {
                    angle.Angle+=5;
                    await Task.Delay(1);
                }
                is_rotating = false;
            }
        }

        private async void Settings_MouseEnter(object sender, MouseEventArgs e)
        {
            if (is_rotating == false)
            {
                is_rotating = true;
                for (int i = 0; i < 18; i++)
                {
                    angle.Angle-=5;
                    await Task.Delay(1);
                }
                is_rotating = false;
            }
        }

        private void Settings_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ModifyMatch m = new ModifyMatch();
            m.Show();
        }



        #endregion

        #region Execute Commands

        #region add_character
        public void addCharacter(string username, string figure, string rank, string custom_prefix, string custom_color)
        {
            bool player_exists = false;
            foreach (Canvas f in figures)
            {
                if (f.Name == username)
                {
                    player_exists = true;
                    break;
                }
            }
            if (!player_exists)
            {
                Canvas f = new Canvas();
                f.Name = username;
                //p.Margin = new Margin(0, 3, 0, 0);
                //BitmapImage bitmap_image = new BitmapImage(new Uri(@"MonopolyData\" + figure + ".png", UriKind.Relative));
                try
                {
                    BitmapImage bitmap_image = new BitmapImage(new Uri(@"pack://application:,,,/" + "Resources/MonopolyData/" + figure + ".png"));
                    f.Background = new ImageBrush(bitmap_image);
                }
                catch (Exception)
                {
                    
                }
                /*Image image1 = Image.FromFile(@"MonopolyData\" + figure + ".png");
                p.BackgroundImage = image1;
                p.BackgroundImageLayout = ImageLayout.Stretch;
                p.Size = figure_size*/
                f.Width = this.Width / 30;
                f.Height = this.Width / 30;
                f.Margin = new Thickness(3, 3, 3, 3);
                index0.Children.Add(f);
                Player p = new Player(username, figure, rank, custom_prefix, custom_color);
                players.Add(p);
                figures.Add(f);
                f.MouseEnter += (sender, e) => statsWindow.figureCheck(sender, e, f);
                f.MouseLeave += (sender, e) => statsWindow.figureCheckRemove(sender, e, f);
                f.MouseLeftButtonDown += new MouseButtonEventHandler(statsWindow.figureCheckLock);
                statsWindow.updateYourPosition();
                statsWindow.setUsername();
            }
        }
        #endregion
        #region dice_thrown
        public async void diceThrown(string dice_generation)
        {
            int how_many_positions = 0;

            //dice_throw:matejck:1-3:4-2:5-1:4-1:6-1:1:1:1:1:1:2:1:2:2:1:;5-4:4-2:1-3:2-2:3-1:1:2:1:2:2:1:1:2:2:2:

            string[] dice_thrown = dice_generation.Split(';');

            string[] dice1 = dice_thrown[0].Split(':');
            string[] dice2 = dice_thrown[1].Split(':');

            string user = dice1[1];

            for (int i = 0; i < 5; i++)
            {
                await Task.Delay(350);
                moveDice(this.dice1, dice1[i + 2], Convert.ToInt32(dice1[i + 7]), Convert.ToInt32(dice1[i + 12]));
                moveDice(this.dice2, dice2[i], Convert.ToInt32(dice2[i + 5]), Convert.ToInt32(dice2[i + 10]));
            }

            string[] dice1_last_throw = dice1[6].Split('-');
            string[] dice2_last_throw = dice2[4].Split('-');

            how_many_positions = Convert.ToInt32(dice1_last_throw[0]) + Convert.ToInt32(dice2_last_throw[0]);
            dice_amount = how_many_positions;


            foreach (Player pl in players)
            {
                if (user == pl.Username())
                {
                    if (pl.JailCount > 0)
                    {
                        if (Convert.ToInt32(dice1_last_throw[0]) == Convert.ToInt32(dice2_last_throw[0]))
                        {
                            moveCharacter(user, how_many_positions);
                            pl.JailCount = 0;
                        }
                        else
                        {
                            pl.JailCount--;
                            if(Monopoly.username == user)
                                statsWindow.EndTurn.Visibility = Visibility.Visible;
                        }
                    }
                    else
                    {
                        moveCharacter(user, how_many_positions);
                    }
                }
            }

            await Task.Delay(1000);
            resetDice();
        }

        public void moveDice(Canvas dice, string complete_number, int x, int y)
        {
            int row = Grid.GetRow(dice);
            int column = Grid.GetColumn(dice);

            DiceGrid.Children.Remove(dice);

            Grid.SetRow(dice, row-x);
            Grid.SetColumn(dice, column-y);
            DiceGrid.Children.Add(dice);

            BitmapImage bitmap_image = new BitmapImage(new Uri(@"pack://application:,,,/" + "Resources/MonopolyData/dice" + complete_number + ".png"));
            dice.Background = new ImageBrush(bitmap_image);
        }
        private void resetDice() 
        {
            DiceGrid.Children.Remove(dice1);
            DiceGrid.Children.Remove(dice2);
            Grid.SetRow(dice1, 14);
            Grid.SetRow(dice2, 13);
            Grid.SetColumn(dice1, 13);
            Grid.SetColumn(dice2, 19);
            DiceGrid.Children.Add(dice1);
            DiceGrid.Children.Add(dice2);
        }
        #endregion
        #region move_forward
        public async void moveCharacter(string u, int how_many_positions)
        {
            int counter = how_many_positions;
            foreach (Canvas f in figures)
            {
                if (f.Name == u)
                {   
                    for (int i = 0; i < counter; i++)
                    {
                        await Task.Delay(370);
                        increasePosition(f, u);
                    }
                    statsWindow.updateYourPosition();
                    await Task.Delay(200);
                    if (u == Monopoly.username)
                    {
                        statsWindow.checkWhereYouLanded();
                    }
                    break;
                }
            }
        }
        #endregion
        #region remove_character
        public void removeCharacter(string username)
        {
            foreach (Canvas f in figures)
            {
                if (f.Name == username)
                {
                    for (int i = 0; i < players.Count; i++)
                    {
                        if (players[i].Username() == username)
                        {
                            removeFigureFromIndex(players[i], f);
                            figures.Remove(f);
                            players.Remove(players[i]);
                        }
                    }
                    break;
                }
            }
        }
        #endregion
        #region player_turn
        public async void yourTurn(string u)
        {
            if (game_over)
                return;
            if (u == username && server_closed != true)
            {
                endTurnTimer.Visibility = Visibility.Visible;
                endTurnMinutes = 3;
                endTurnSeconds = 1;
                end_turn_timer.Start();


                your_turn = true;
                rent_paid = false;
                dice_enabled = true;
                double_rent = 1;
                utility_chance_rent = false;
                statsWindow.WindowState = WindowState.Normal;
                popUpWindow("Your turn!", Brushes.DarkGreen);
                await Task.Delay(2000);
                popUpWindowClose();
                foreach (Player pl in players)
                {
                    if (pl.Username() == u && pl.JailCount > 0 && pl.GetOutOfJailFree > 0)
                    {
                        statsWindow.OutOfJailButton.Visibility = Visibility.Visible;
                        statsWindow.OutOfJailText.Visibility = Visibility.Visible;
                    }
                }

                statsWindow.hideEverything();
                statsWindow.PropertyOptionsClose.Visibility = Visibility.Hidden;
                statsWindow.CheckingPlayerClose.Visibility = Visibility.Hidden;

            }
            else 
            {
                endTurnTimer.Visibility = Visibility.Hidden;
                end_turn_timer.Start();
                statsWindow.EndTurn.Visibility = Visibility.Hidden;
                statsWindow.PropertyOptions.Visibility = Visibility.Hidden;

                your_turn = false;
                rent_paid = false;
                dice_enabled = false;
                double_rent = 1;
                utility_chance_rent = false;

                statsWindow.hideEverything();
                statsWindow.PropertyOptionsClose.Visibility = Visibility.Hidden;
                statsWindow.CheckingPlayerClose.Visibility = Visibility.Hidden;

                if (statsWindow.YourPropertyOptions.Visibility == Visibility.Hidden)
                {
                    statsWindow.YourPropertyOptions.Visibility = Visibility.Hidden;
                }
                else
                {
                    statsWindow.CheckingPosition.Visibility = Visibility.Visible;
                }
                popUpWindow(u + " has turn!", Brushes.Black);
                await Task.Delay(2000);
                popUpWindowClose();
            }
        }
        #endregion
        #region game_start
        public void gameStart()
        {
            started = true;
        }
        #endregion
        #region game_has_already_started
        public void gameFull()
        {
            popUpWindow("Game is full", Brushes.DarkRed);
        }
        #endregion
        #region buy_property
        public void buyProperty(string username, int index, int price)
        {
            foreach (Position p in properties)
            {
                if (p.getIndex() == index)
                {
                    foreach (Player pl in players)
                    {
                        if (pl.Username() == username)
                        {
                            pl.Balance = pl.Balance - price;
                            p.ownedBy = pl.Username();
                            statsWindow.updateYourPosition();
                            if (username == Monopoly.username)
                            {
                                //UserData.total_cards_bought++;
                                statsWindow.updatePropertyOptions();
                            }
                        }
                    }
                }
            }
        }
        #endregion
        #region sell_property
        public void sellProperty(string username, int index, int price)
        {
            foreach (Position p in properties)
            {
                if (p.getIndex() == index)
                {
                    foreach (Player pl in players)
                    {
                        if (pl.Username() == username)
                        {
                            pl.Balance = pl.Balance + price;
                            p.ownedBy = "/";
                            statsWindow.updateYourPosition();
                            if (username == Monopoly.username)
                            {
                                //UserData.total_balance += price;
                                statsWindow.updatePropertyOptions();
                            }
                        }
                    }
                }
            }
        }
        #endregion
        #region buy_house
        public void buyHouse(string u, int index, int house_price)
        {
            foreach (Position p in properties)
            {
                if (p.getIndex() == index)
                {
                    foreach (Player pl in players)
                    {
                        if (pl.Username() == u)
                        {
                            pl.Balance = pl.Balance - house_price;
                            p.Houses++;
                            statsWindow.updateYourPosition();
                            if (u == Monopoly.username)
                            {
                                //UserData.total_houses_bought++;
                                statsWindow.updatePropertyOptions();
                            }
                            break;
                        }
                    }
                    break;
                }
            }
        }
        #endregion
        #region sell_house
        public void sellHouse(string username, int index, int house_price)
        {
            foreach (Position p in properties)
            {
                if (p.getIndex() == index)
                {
                    foreach (Player pl in players)
                    {
                        if (pl.Username() == username)
                        {
                            pl.Balance = pl.Balance + house_price;
                            p.Houses--;
                            statsWindow.updateYourPosition();
                            if (username == Monopoly.username)
                            {
                                //UserData.total_balance += house_price;
                                statsWindow.updatePropertyOptions();
                            }
                        }
                    }
                }
            }
        }
        #endregion
        #region buy_hotel
        public void buyHotel(string username, int index, int hotel_price)
        {
            foreach (Position p in properties)
            {
                if (p.getIndex() == index)
                {
                    foreach (Player pl in players)
                    {
                        if (pl.Username() == username)
                        {
                            pl.Balance = pl.Balance - hotel_price;
                            p.Hotels++;
                            p.Houses = 0;
                            statsWindow.updateYourPosition();
                            if (username == Monopoly.username)
                            {
                                //UserData.total_hotels_bought++;
                                statsWindow.updatePropertyOptions();
                            }
                        }
                    }
                }
            }
        }
        #endregion
        #region sell_hotel
        public void sellHotel(string username, int index, int hotel_price)
        {
            foreach (Position p in properties)
            {
                if (p.getIndex() == index)
                {
                    foreach (Player pl in players)
                    {
                        if (pl.Username() == username)
                        {
                            pl.Balance = pl.Balance + hotel_price;
                            p.Hotels--;
                            statsWindow.updateYourPosition();
                            if (username == Monopoly.username)
                            {
                                //UserData.total_balance += hotel_price;
                                statsWindow.updatePropertyOptions();
                            }
                        }
                    }
                }
            }
        }
        #endregion
        #region pay_rent
        public void payRent(string payer, string reciever, int amount)
        {
            Output(payer + " has payed $" + amount + " to " + reciever + ".");
            foreach (Player pl in players)
            {
                if (pl.Username() == reciever)
                {
                    pl.Balance = pl.Balance + amount;
                    break;
                }
            }
            foreach (Player pl in players)
            {
                if (pl.Username() == payer)
                {
                    pl.Balance = pl.Balance - amount;
                    break;
                }
            }
            statsWindow.updateYourPosition();
        }
        #endregion
        #region bankrupt
        public void Bankrupt(string u)
        {
            foreach (Player pl in players)
            {
                if (pl.Username() == u)
                {
                    pl.Bankrupt = true;
                    foreach (Canvas character in figures)
                    {
                        if (character.Name == u)
                        {
                            if (u == username)
                            {
                                statsWindow.updateYourPosition();
                            }
                            break;
                        }
                    }
                }
            }
        }
        #endregion
        #region go_jail
        public void putInJail(string u)
        {
            foreach (Player pl in players)
            {
                if (u == pl.Username())
                {
                    foreach (Canvas p in figures)
                    {
                        if (p.Name == u)
                        {
                            pl.JailCount = 3;
                            pl.WasInJail = true;
                            removeFigureFromIndex(pl,p);
                            //index30.Children.Add(p);
                            pl.Position = 10;
                            index10.Children.Add(p);
                        }
                    }
                }
            }
            statsWindow.updateYourPosition();
        }
        #endregion
        #region treasure_show
        public void treasureShow(int index)
        {
            TreasureBorder.Background = GetImage(@"pack://application:,,,/" + "Resources/MonopolyData/Treasure/" + index + ".png");
            /*Image image = Image.FromFile(@"MonopolyData\Treasure\" + index + ".png");
            treasure.Size = new Size(378, 218);
            treasure.BackgroundImageLayout = ImageLayout.Stretch;
            treasure.BackgroundImage = image;*/

        }
        #endregion
        #region treasure_hide
        public void treasureHide()
        {
            TreasureBorder.Background = Brushes.Transparent;
            /*treasure.BackgroundImage = null;
            treasure.Size = new Size(217, 130);*/
        }
        #endregion
        #region receive_money
        public void recieveMoney(string u, int amount)
        {
            foreach (Player pl in players)
            {
                if (pl.Username() == u)
                {
                    pl.Balance = pl.Balance + amount;
                    if (username == u)
                        //UserData.total_balance += amount;
                    statsWindow.updateYourPosition();
                }
            }
        }
        #endregion
        #region take_money
        public void takeMoney(string u, int amount)
        {
            foreach (Player pl in players)
            {
                if (pl.Username() == u)
                {
                    pl.Balance = pl.Balance - amount;
                    statsWindow.updateYourPosition();
                }
            }
        }
        #endregion
        #region collect_money_per_player
        public void collectMoneyPerPlayer(string u, int amount)
        {
            foreach (Player reciever in players)
            {
                if (reciever.Username() == u)
                {
                    foreach (Player payer in players)
                    {
                        if (payer.Username() != u)
                        {
                            payer.Balance = payer.Balance - amount;
                            reciever.Balance = reciever.Balance + amount;
                            if (username == reciever.Username())
                            {
                                //UserData.total_balance += amount;
                            }
                        }
                    }
                }
            }
            statsWindow.updateYourPosition();
        }
        #endregion
        #region pay_money_per_buildings
        public void payMoneyPerBuildings(string u, int house_price, int hotel_price)
        {
            foreach (Player pl in players)
            {
                if (pl.Username() == u)
                {
                    foreach (Position p in properties)
                    {
                        if (p.ownedBy == u)
                        {
                            pl.Balance = pl.Balance - (p.Houses * house_price);
                            pl.Balance = pl.Balance - (p.Hotels * hotel_price);
                        }
                    }
                }
            }
            statsWindow.updateYourPosition();
        }
        #endregion
        #region get_out_of_jail_free
        public void getOutOfJailFree(string username)
        {
            foreach (Player pl in players)
            {
                if (pl.Username() == username)
                {
                    pl.GetOutOfJailFree++;
                }
            }
            if (Monopoly.username == username)
            {
                Output("You've gotten Get out of jail free card. You can use it while in jail.");
            }
        }
        #endregion
        #region reset_jail_count
        public void resetJailCount(string username) 
        {
            foreach (Player pl in players)
            {
                if (pl.Username() == username)
                {
                    pl.JailCount = 0;
                }
            }
        }
        #endregion
        #region decrease_jail_count
        public void decreaseJailCount(string username)
        {
            foreach (Player pl in players)
            {
                if (pl.Username() == username)
                {
                    pl.GetOutOfJailFree--;
                }
            }
        }
        #endregion 
        #region chance_show
        public void chanceShow(int index)
        {
            ChanceBorder.Background = GetImage(@"pack://application:,,,/" + "Resources/MonopolyData/Chance/" + index + ".png");

            /*Image image = Image.FromFile(@"MonopolyData\Chance\" + index + ".png");
            chance.Size = new Size(378, 218);
            chance.BackgroundImageLayout = ImageLayout.Stretch;
            chance.BackgroundImage = image;*/
        }
        #endregion
        #region chance_hide
        public void chanceHide()
        {
            ChanceBorder.Background = Brushes.Transparent;

            /*chance.BackgroundImage = null;
            chance.Size = new Size(217, 130);*/
        }
        #endregion
        #region move_to
        public async void moveTo(string u, string position_name)
        {
            foreach (Canvas character in figures)
            {
                if (character.Name == u)
                {
                    foreach (Player pl in players)
                    {
                        if (pl.Username() == u)
                        {
                            foreach (Position p in properties)
                            {
                                if (p.getIndex() == pl.Position)
                                {
                                    if (p.getIndexName() != position_name)
                                    {
                                        await Task.Delay(370);
                                        increasePosition(character, u);
                                    }
                                }
                            }
                            if (pl.Position == 0 && position_name != "GO!")
                            {
                                moveTo(u, position_name);
                            }
                            else
                            {
                                statsWindow.updateYourPosition();
                                await Task.Delay(200);
                                if(Monopoly.username == u)
                                    statsWindow.checkWhereYouLanded();
                            }
                        }
                    }
                }
            }
        }
        #endregion
        #region nearest_railroad
        public async void nearestRailroad(string u)
        {
            foreach (Canvas character in figures)
            {
                if (character.Name == u)
                {
                    foreach (Player pl in players)
                    {
                        if (pl.Username() == u)
                        {
                            foreach (Position p in properties)
                            {
                                if (p.getIndex() == pl.Position)
                                {
                                    if (p.getIndexName() != "B. AND O. RAILROAD" && p.getIndexName() != "SHORT LINE" && p.getIndexName() != "READING RAILROAD" && p.getIndexName() != "PENNSYLVANIA RAILROAD")
                                    {
                                        await Task.Delay(370);
                                        increasePosition(character, u);
                                    }
                                }
                            }
                            if (pl.Position == 0)
                            {
                                nearestRailroad(u);
                            }
                        }
                    }
                }
            }
            statsWindow.updateYourPosition();
            await Task.Delay(200);
            if(u == Monopoly.username)
                statsWindow.checkWhereYouLanded();
        }
        #endregion
        #region nearest_utility
        public async void nearestUtility(string u)
        {
            foreach (Canvas character in figures)
            {
                if (character.Name == u)
                {
                    foreach (Player pl in players)
                    {
                        if (pl.Username() == u)
                        {
                            foreach (Position p in properties)
                            {
                                if (p.getIndex() == pl.Position)
                                {
                                    if (p.getIndexName() != "WATER WORKS" && p.getIndexName() != "ELECTRIC COMPANY")
                                    {
                                        await Task.Delay(370);
                                        increasePosition(character, u);
                                    }
                                }
                            }
                            if (pl.Position == 0)
                            {
                                nearestUtility(u);
                            }
                        }
                    }
                }
            }
            statsWindow.updateYourPosition();
            await Task.Delay(200);
            if(u == Monopoly.username)
                statsWindow.checkWhereYouLanded();
        }
        #endregion
        #region move_back
        public async void moveBack(string u, int how_many_positions)
        {
            int counter = how_many_positions;
            foreach (Canvas f in figures)
            {
                if (f.Name == u)
                {
                    for (int i = 0; i < counter; i++)
                    {
                        await Task.Delay(370);
                        decreasePosition(f, u);
                    }
                    statsWindow.updateYourPosition();
                    await Task.Delay(200);
                    if (u == Monopoly.username)
                    {
                        statsWindow.checkWhereYouLanded();
                    }
                    break;
                }
            }
        }
        #endregion
        #region pay_each_player
        public void payEachPlayer(string u, int amount)
        {
            foreach (Player payer in players)
            {
                if (payer.Username() == u)
                {
                    foreach (Player receiver in players)
                    {
                        if (receiver.Username() != payer.Username())
                        {
                            payer.Balance = payer.Balance - amount;
                            receiver.Balance = receiver.Balance + amount;
                        }
                    }
                }
            }
            statsWindow.updateYourPosition();
        }
        #endregion
        #region close_server
        public async void closeServer()
        {
            server_closed = true;
            await Task.Delay(100);
            statsWindow.Hide();
            PopUpText.Foreground = Brushes.DarkRed;
            PopUpText.Content = "Server closed";
            PopUp.Visibility = Visibility.Visible;
        }
        #endregion
        #region player_winner
        public void playerWinner(string u)
        {
            game_over = true;
            statsWindow.Hide();
            if (u.Trim() == username.Trim())
            {
                //Chat(u + "==" + username + ";win");
                PopUpText.Foreground = Brushes.DarkGreen;
                PopUpText.Content = "YOU WIN!";
                PopUp.Visibility = Visibility.Visible;
            }
            else
            {
                //Chat(u + "==" + username + ";gameover");
                PopUpText.Foreground = Brushes.DarkRed;
                PopUpText.Content = "Game over";
                PopUp.Visibility = Visibility.Visible;
            }
            statsWindow.Hide();
        }
        #endregion
        #region player_request
        public void playerRequest(string requester, string u, string index_name, int amount)
        {
            if (u == username)
            {
                req_requester = requester;
                req_u = u;
                req_index_name = index_name;
                req_amount = amount;
                //RequestedBuy.Visible = true;
                req_expires = 20;
                RequestPopUpTimer.Text = "" + req_expires;
                requestPopUp("Player " + requester + " wants to sell you property ", index_name.Substring(0, 1) + index_name.Substring(1).ToLower() + " for $" + amount);

                request_timer = new DispatcherTimer();
                request_timer.Tick += request_timer_tick;
                request_timer.Interval = new TimeSpan(0, 0, 0, 1, 0);
                request_timer.Start();

                //ReqExpiresTimer.Enabled = true;
                //Req.Text = "Player " + requester + " wants to sell you property " + index_name.Substring(0, 1) + index_name.Substring(1).ToLower() + " for $" + amount;

            }
        }

        private void request_timer_tick(object sender, EventArgs e)
        {
            req_expires--;
            RequestPopUpTimer.Text = "" + req_expires;
            if (req_expires <= 0)
            {
                client.Send("request_decline:" + req_requester + ":" + req_u + ":" + req_index_name + ":" + req_amount + ":");
                requestPopUpClose();
            }
        }

        #endregion
        #region request_accept
        public void requestAccept(string requester, string u, string index_name, int amount)
        {
            foreach (Player pl in players)
            {
                if (pl.Username() == requester)
                {
                    pl.Balance = pl.Balance + amount;
                }
                else if (pl.Username() == u)
                {
                    pl.Balance = pl.Balance - amount;
                }
            }
            foreach (Position p in properties)
            {
                if (p.getIndexName() == index_name)
                {
                    p.ownedBy = u;
                }
            }

            if (username == requester)
            {
                statsWindow.YourPropertyOptions.Visibility = Visibility.Hidden;
                statsWindow.updateYourPosition();
            }
            else if (username == u)
            {
                statsWindow.updateYourPosition();
            }
        }
        #endregion

#endregion

        #region Other methods

        public void EnableRequestButton() 
        {
            statsWindow.RequestSell.IsEnabled = true;
        }

        public void Output(string text)
        {
            ChatBox.AppendText(Environment.NewLine + text);
            Scroll.ScrollToEnd();
        }

        public void requestPopUp(string text1, string text2)
        {
            RequestPopUpText1.Text = text1;
            RequestPopUpText2.Text = text2;
            RequestPopUp.Visibility = Visibility.Visible;
        }

        public void requestPopUpClose()
        {
            request_timer.Stop();
            RequestPopUp.Visibility = Visibility.Hidden;
        }

        public void popUpWindow(string text, SolidColorBrush brush)
        {
            if(game_over == false && server_closed == false)
            {
                PopUpText.Foreground = brush;
                PopUpText.Content = text;
                PopUp.Visibility = Visibility.Visible;
            }
        }

        public void popUpWindowClose() 
        {
            if(game_over == false && server_closed == false)
                PopUp.Visibility = Visibility.Hidden;
        }

        public void generationDice()
        {
            Random r = new Random();
            string dice_generation = "dice_throw:" + username + ":";

            string dice1 = "";
            string dice2 = "";

            /*
             * Dice1 generation
             */

            for (int i = 0; i < 5; i++)//generira katera kocka bo vrzena
            {
                dice1 = dice1 + r.Next(1, 7) + "-" + r.Next(1, 5) + ":"; //6-4:
            }
            for (int i = 0; i < 5; i++)//generira koordinate x kamor se bo kocka postavla (vrgla)
            {
                dice1 = dice1 + r.Next(1, 3) + ":";
            }
            for (int i = 0; i < 5; i++)//generira koordinate y kamor se bo kocka postavla (vrgla)
            {
                dice1 = dice1 + r.Next(1, 3) + ":";
            }

            /*
             * Dice2 generation
             */

            for (int i = 0; i < 5; i++)//generira katera kocka bo vrzena
            {
                dice2 = dice2 + r.Next(1, 7) + "-" + r.Next(1, 5) + ":";
            }
            for (int i = 0; i < 5; i++)//generira koordinate x kamor se bo kocka postavla (vrgla)
            {
                dice2 = dice2 + r.Next(1, 3) + ":";
            }
            for (int i = 0; i < 5; i++)//generira koordinate y kamor se bo kocka postavla (vrgla)
            {
                dice2 = dice2 + r.Next(1, 3) + ":";
            }

            dice_generation = dice_generation + dice1 + ";" + dice2;

            client.Send(dice_generation);
        }

        private async void WaitingForPlayers(string text) //S pomočjo te metode se '.' premikajo, ko se čaka na igralce
        {
            PopUpText.Foreground = Brushes.DarkRed;
            PopUp.Visibility = Visibility.Visible;
            if (started == false)
            {
                await Task.Delay(400);
                if (server_closed == false)
                {
                    if ("Waiting for players " == text)
                    {
                        PopUpText.Content = "Waiting for players .";
                    }
                    else if ("Waiting for players ." == text)
                    {
                        PopUpText.Content = "Waiting for players . .";
                    }
                    else if ("Waiting for players . ." == text)
                    {
                        PopUpText.Content = "Waiting for players . . .";
                    }
                    else if ("Waiting for players . . ." == text)
                    {
                        PopUpText.Content = "Waiting for players ";
                    }

                    WaitingForPlayers(Convert.ToString(PopUpText.Content));
                }
            }
            else
            {
                PopUp.Visibility = Visibility.Hidden;
                statsWindow.Show();
                statsWindow.ShowInTaskbar = false;
                end_turn_timer = new DispatcherTimer();
                end_turn_timer.Tick += endTurnTick;
                end_turn_timer.Interval = new TimeSpan(0, 0, 0, 1, 0);
                //statsWindow.updateYourPosition();
            }
        }

        private void endTurnTick(object sender, EventArgs e)
        {
            endTurnSeconds--;
            if (endTurnSeconds < 10)
            {
                endTurnTimer.Text = "" + endTurnMinutes + ":0" + endTurnSeconds;
            }
            else
            {
                endTurnTimer.Text = "" + endTurnMinutes + ":" + endTurnSeconds;
            }

            if (endTurnSeconds == 0 && endTurnMinutes == 0)
            {
                client.Send("end_turn:" + Monopoly.username + ":");
                statsWindow.hideYourPropertyButtons();
                if (statsWindow.YourPropertyOptions.Visibility == Visibility.Visible)
                {
                    statsWindow.YourPropertyOptions.Visibility = Visibility.Hidden;
                    statsWindow.CheckingPosition.Visibility = Visibility.Visible;
                }
                end_turn_timer.Stop();
                endTurnTimer.Visibility = Visibility.Hidden;
            }
            else if (endTurnSeconds == 0)
            {
                endTurnSeconds = 60;
                endTurnMinutes--;
            }

        }

        private ImageBrush GetImage(string path)
        {
            BitmapImage bitmap_image = new BitmapImage(new Uri(path));
            ImageBrush image = new ImageBrush();
            image.ImageSource = bitmap_image;
            image.Stretch = Stretch.Fill;
            image.AlignmentX = AlignmentX.Left;
            image.AlignmentY = AlignmentY.Top;
            return image;
        }

        #endregion

        #region Long methods

        private void removeFigureFromIndex(Player pl,Canvas f)
        {
            switch (pl.Position)
            {
                case 0: index0.Children.Remove(f); break;
                case 1: index1.Children.Remove(f); break;
                case 2: index2.Children.Remove(f); break;
                case 3: index3.Children.Remove(f); break;
                case 4: index4.Children.Remove(f); break;
                case 5: index5.Children.Remove(f); break;
                case 6: index6.Children.Remove(f); break;
                case 7: index7.Children.Remove(f); break;
                case 8: index8.Children.Remove(f); break;
                case 9: index9.Children.Remove(f); break;
                case 10: index10.Children.Remove(f); break;
                case 11: index11.Children.Remove(f); break;
                case 12: index12.Children.Remove(f); break;
                case 13: index13.Children.Remove(f); break;
                case 14: index14.Children.Remove(f); break;
                case 15: index15.Children.Remove(f); break;
                case 16: index16.Children.Remove(f); break;
                case 17: index17.Children.Remove(f); break;
                case 18: index18.Children.Remove(f); break;
                case 19: index19.Children.Remove(f); break;
                case 20: index20.Children.Remove(f); break;
                case 21: index21.Children.Remove(f); break;
                case 22: index22.Children.Remove(f); break;
                case 23: index23.Children.Remove(f); break;
                case 24: index24.Children.Remove(f); break;
                case 25: index25.Children.Remove(f); break;
                case 26: index26.Children.Remove(f); break;
                case 27: index27.Children.Remove(f); break;
                case 28: index28.Children.Remove(f); break;
                case 29: index29.Children.Remove(f); break;
                case 30: index30.Children.Remove(f); break;
                case 31: index31.Children.Remove(f); break;
                case 32: index32.Children.Remove(f); break;
                case 33: index33.Children.Remove(f); break;
                case 34: index34.Children.Remove(f); break;
                case 35: index35.Children.Remove(f); break;
                case 36: index36.Children.Remove(f); break;
                case 37: index37.Children.Remove(f); break;
                case 38: index38.Children.Remove(f); break;
                case 39: index39.Children.Remove(f); break;
            }
        }

        private void increasePosition(Canvas character, string username)
        {
            foreach (Player player in players)
            {
                if (player.Username() == username)
                {
                    if (player.Position == 0)
                    {
                        index0.Children.Remove(character);
                        index1.Children.Add(character);
                        player.Position = 1;
                    }
                    else if (player.Position == 1)
                    {
                        index1.Children.Remove(character);
                        index2.Children.Add(character);
                        player.Position = 2;

                    }
                    else if (player.Position == 2)
                    {
                        index2.Children.Remove(character);
                        index3.Children.Add(character);
                        player.Position = 3;

                    }
                    else if (player.Position == 3)
                    {
                        index3.Children.Remove(character);
                        index4.Children.Add(character);
                        player.Position = 4;

                    }
                    else if (player.Position == 4)
                    {
                        index4.Children.Remove(character);
                        index5.Children.Add(character);
                        player.Position = 5;


                    }
                    else if (player.Position == 5)
                    {
                        index5.Children.Remove(character);
                        index6.Children.Add(character);
                        player.Position = 6;


                    }
                    else if (player.Position == 6)
                    {
                        index6.Children.Remove(character);
                        index7.Children.Add(character);
                        player.Position = 7;

                    }
                    else if (player.Position == 7)
                    {
                        index7.Children.Remove(character);
                        index8.Children.Add(character);
                        player.Position = 8;

                    }
                    else if (player.Position == 8)
                    {
                        index8.Children.Remove(character);
                        index9.Children.Add(character);

                        player.Position = 9;

                    }
                    else if (player.Position == 9)
                    {
                        index9.Children.Remove(character);
                        index10.Children.Add(character);

                        player.Position = 10;

                    }
                    else if (player.Position == 10)
                    {
                        index10.Children.Remove(character);
                        index11.Children.Add(character);

                        player.Position = 11;

                    }
                    else if (player.Position == 11)
                    {
                        index11.Children.Remove(character);
                        index12.Children.Add(character);

                        player.Position = 12;

                    }
                    else if (player.Position == 12)
                    {
                        index12.Children.Remove(character);
                        index13.Children.Add(character);

                        player.Position = 13;

                    }
                    else if (player.Position == 13)
                    {
                        index13.Children.Remove(character);
                        index14.Children.Add(character);

                        player.Position = 14;

                    }
                    else if (player.Position == 14)
                    {
                        index14.Children.Remove(character);
                        index15.Children.Add(character);

                        player.Position = 15;

                    }
                    else if (player.Position == 15)
                    {
                        index15.Children.Remove(character);
                        index16.Children.Add(character);

                        player.Position = 16;

                    }
                    else if (player.Position == 16)
                    {
                        index16.Children.Remove(character);
                        index17.Children.Add(character);

                        player.Position = 17;

                    }
                    else if (player.Position == 17)
                    {
                        index17.Children.Remove(character);
                        index18.Children.Add(character);

                        player.Position = 18;

                    }
                    else if (player.Position == 18)
                    {
                        index18.Children.Remove(character);
                        index19.Children.Add(character);

                        player.Position = 19;

                    }
                    else if (player.Position == 19)
                    {
                        index19.Children.Remove(character);
                        index20.Children.Add(character);

                        player.Position = 20;

                    }
                    else if (player.Position == 20)
                    {
                        index20.Children.Remove(character);
                        index21.Children.Add(character);

                        player.Position = 21;

                    }
                    else if (player.Position == 21)
                    {
                        index21.Children.Remove(character);
                        index22.Children.Add(character);

                        player.Position = 22;

                    }
                    else if (player.Position == 22)
                    {
                        index22.Children.Remove(character);
                        index23.Children.Add(character);

                        player.Position = 23;

                    }
                    else if (player.Position == 23)
                    {
                        index23.Children.Remove(character);
                        index24.Children.Add(character);

                        player.Position = 24;

                    }
                    else if (player.Position == 24)
                    {
                        index24.Children.Remove(character);
                        index25.Children.Add(character);

                        player.Position = 25;

                    }
                    else if (player.Position == 25)
                    {
                        index25.Children.Remove(character);
                        index26.Children.Add(character);

                        player.Position = 26;

                    }
                    else if (player.Position == 26)
                    {
                        index26.Children.Remove(character);
                        index27.Children.Add(character);

                        player.Position = 27;

                    }
                    else if (player.Position == 27)
                    {
                        index27.Children.Remove(character);
                        index28.Children.Add(character);

                        player.Position = 28;

                    }
                    else if (player.Position == 28)
                    {
                        index28.Children.Remove(character);
                        index29.Children.Add(character);

                        player.Position = 29;

                    }
                    else if (player.Position == 29)
                    {
                        index29.Children.Remove(character);
                        index30.Children.Add(character);

                        player.Position = 30;

                    }
                    else if (player.Position == 30)
                    {
                        index30.Children.Remove(character);
                        index31.Children.Add(character);

                        player.Position = 31;

                    }
                    else if (player.Position == 31)
                    {
                        index31.Children.Remove(character);
                        index32.Children.Add(character);

                        player.Position = 32;

                    }
                    else if (player.Position == 32)
                    {
                        index32.Children.Remove(character);
                        index33.Children.Add(character);

                        player.Position = 33;

                    }
                    else if (player.Position == 33)
                    {
                        index33.Children.Remove(character);
                        index34.Children.Add(character);

                        player.Position = 34;

                    }
                    else if (player.Position == 34)
                    {
                        index34.Children.Remove(character);
                        index35.Children.Add(character);

                        player.Position = 35;

                    }
                    else if (player.Position == 35)
                    {
                        index35.Children.Remove(character);
                        index36.Children.Add(character);

                        player.Position = 36;

                    }
                    else if (player.Position == 36)
                    {
                        index36.Children.Remove(character);
                        index37.Children.Add(character);

                        player.Position = 37;

                    }
                    else if (player.Position == 37)
                    {
                        index37.Children.Remove(character);
                        index38.Children.Add(character);

                        player.Position = 38;

                    }
                    else if (player.Position == 38)
                    {
                        index38.Children.Remove(character);
                        index39.Children.Add(character);
                        player.Position = 39;

                    }
                    else if (player.Position == 39)
                    {
                        index39.Children.Remove(character);
                        index0.Children.Add(character);
                        if (player.WasInJail == false)
                        {
                            player.Balance = player.Balance + 200;
                        }
                        player.WasInJail = false;
                        player.Position = 0;
                        
                    }
                }
            }
        }

        private void decreasePosition(Panel character, string username)
        {
            foreach (Player player in players)
            {
                if (player.Username() == username)
                {
                    if (player.Position == 0)
                    {
                        index0.Children.Remove(character);
                        index39.Children.Add(character);
                        player.Position = 39;
                    }
                    else if (player.Position == 1)
                    {
                        index1.Children.Remove(character);
                        index0.Children.Add(character);
                        player.Position = 0;
                    }
                    else if (player.Position == 2)
                    {
                        index2.Children.Remove(character);
                        index1.Children.Add(character);
                        player.Position = 1;
                    }
                    else if (player.Position == 3)
                    {
                        index3.Children.Remove(character);
                        index2.Children.Add(character);
                        player.Position = 2;
                    }
                    else if (player.Position == 4)
                    {
                        index4.Children.Remove(character);
                        index3.Children.Add(character);
                        player.Position = 3;
                    }
                    else if (player.Position == 5)
                    {
                        index5.Children.Remove(character);
                        index4.Children.Add(character);
                        player.Position = 4;
                    }
                    else if (player.Position == 6)
                    {
                        index6.Children.Remove(character);
                        index5.Children.Add(character);
                        player.Position = 5;
                    }
                    else if (player.Position == 7)
                    {
                        index7.Children.Remove(character);
                        index6.Children.Add(character);
                        player.Position = 6;
                    }
                    else if (player.Position == 8)
                    {
                        index8.Children.Remove(character);
                        index7.Children.Add(character);
                        player.Position = 7;
                    }
                    else if (player.Position == 9)
                    {
                        index9.Children.Remove(character);
                        index8.Children.Add(character);
                        player.Position = 8;
                    }
                    else if (player.Position == 10)
                    {
                        index10.Children.Remove(character);
                        index9.Children.Add(character);
                        player.Position = 9;
                    }
                    else if (player.Position == 11)
                    {
                        index11.Children.Remove(character);
                        index10.Children.Add(character);
                        player.Position = 10;
                    }
                    else if (player.Position == 12)
                    {
                        index12.Children.Remove(character);
                        index11.Children.Add(character);
                        player.Position = 11;
                    }
                    else if (player.Position == 13)
                    {
                        index13.Children.Remove(character);
                        index12.Children.Add(character);
                        player.Position = 12;
                    }
                    else if (player.Position == 14)
                    {
                        index14.Children.Remove(character);
                        index13.Children.Add(character);
                        player.Position = 13;
                    }
                    else if (player.Position == 15)
                    {
                        index15.Children.Remove(character);
                        index14.Children.Add(character);
                        player.Position = 14;
                    }
                    else if (player.Position == 16)
                    {
                        index16.Children.Remove(character);
                        index15.Children.Add(character);
                        player.Position = 15;
                    }
                    else if (player.Position == 17)
                    {
                        index17.Children.Remove(character);
                        index16.Children.Add(character);
                        player.Position = 16;
                    }
                    else if (player.Position == 18)
                    {
                        index18.Children.Remove(character);
                        index17.Children.Add(character);
                        player.Position = 17;
                    }
                    else if (player.Position == 19)
                    {
                        index19.Children.Remove(character);
                        index18.Children.Add(character);
                        player.Position = 18;
                    }
                    else if (player.Position == 20)
                    {
                        index20.Children.Remove(character);
                        index19.Children.Add(character);
                        player.Position = 19;
                    }
                    else if (player.Position == 21)
                    {
                        index21.Children.Remove(character);
                        index20.Children.Add(character);
                        player.Position = 20;
                    }
                    else if (player.Position == 22)
                    {
                        index22.Children.Remove(character);
                        index21.Children.Add(character);
                        player.Position = 21;
                    }
                    else if (player.Position == 23)
                    {
                        index23.Children.Remove(character);
                        index22.Children.Add(character);
                        player.Position = 22;
                    }
                    else if (player.Position == 24)
                    {
                        index24.Children.Remove(character);
                        index23.Children.Add(character);
                        player.Position = 23;
                    }
                    else if (player.Position == 25)
                    {
                        index25.Children.Remove(character);
                        index24.Children.Add(character);
                        player.Position = 24;
                    }
                    else if (player.Position == 26)
                    {
                        index26.Children.Remove(character);
                        index25.Children.Add(character);
                        player.Position = 25;
                    }
                    else if (player.Position == 27)
                    {
                        index27.Children.Remove(character);
                        index26.Children.Add(character);
                        player.Position = 26;
                    }
                    else if (player.Position == 28)
                    {
                        index28.Children.Remove(character);
                        index27.Children.Add(character);
                        player.Position = 27;
                    }
                    else if (player.Position == 29)
                    {
                        index29.Children.Remove(character);
                        index28.Children.Add(character);
                        player.Position = 28;
                    }
                    else if (player.Position == 30)
                    {
                        index30.Children.Remove(character);
                        index29.Children.Add(character);
                        player.Position = 29;
                    }
                    else if (player.Position == 31)
                    {
                        index31.Children.Remove(character);
                        index30.Children.Add(character);
                        player.Position = 30;
                    }
                    else if (player.Position == 32)
                    {
                        index32.Children.Remove(character);
                        index31.Children.Add(character);
                        player.Position = 31;
                    }
                    else if (player.Position == 33)
                    {
                        index33.Children.Remove(character);
                        index32.Children.Add(character);
                        player.Position = 32;
                    }
                    else if (player.Position == 34)
                    {
                        index34.Children.Remove(character);
                        index33.Children.Add(character);
                        player.Position = 33;
                    }
                    else if (player.Position == 35)
                    {
                        index35.Children.Remove(character);
                        index34.Children.Add(character);
                        player.Position = 34;
                    }
                    else if (player.Position == 36)
                    {
                        index36.Children.Remove(character);
                        index35.Children.Add(character);
                        player.Position = 35;
                    }
                    else if (player.Position == 37)
                    {
                        index37.Children.Remove(character);
                        index36.Children.Add(character);
                        player.Position = 36;
                    }
                    else if (player.Position == 38)
                    {
                        index38.Children.Remove(character);
                        index37.Children.Add(character);
                        player.Position = 37;
                    }
                    else if (player.Position == 39)
                    {
                        index39.Children.Remove(character);
                        index38.Children.Add(character);
                        player.Position = 38;
                    }
                }
            }
        }

        private void setIndexesPositions()
        {
            Position position = new Position(0); properties.Add(position);
            position = new Position(1); properties.Add(position);
            position = new Position(2); properties.Add(position);
            position = new Position(3); properties.Add(position);
            position = new Position(4); properties.Add(position);
            position = new Position(5); properties.Add(position);
            position = new Position(6); properties.Add(position);
            position = new Position(7); properties.Add(position);
            position = new Position(8); properties.Add(position);
            position = new Position(9); properties.Add(position);
            position = new Position(10); properties.Add(position);
            position = new Position(11); properties.Add(position);
            position = new Position(12); properties.Add(position);
            position = new Position(13); properties.Add(position);
            position = new Position(14); properties.Add(position);
            position = new Position(15); properties.Add(position);
            position = new Position(16); properties.Add(position);
            position = new Position(17); properties.Add(position);
            position = new Position(18); properties.Add(position);
            position = new Position(19); properties.Add(position);
            position = new Position(20); properties.Add(position);
            position = new Position(21); properties.Add(position);
            position = new Position(22); properties.Add(position);
            position = new Position(23); properties.Add(position);
            position = new Position(24); properties.Add(position);
            position = new Position(25); properties.Add(position);
            position = new Position(26); properties.Add(position);
            position = new Position(27); properties.Add(position);
            position = new Position(28); properties.Add(position);
            position = new Position(29); properties.Add(position);
            position = new Position(30); properties.Add(position);
            position = new Position(31); properties.Add(position);
            position = new Position(32); properties.Add(position);
            position = new Position(33); properties.Add(position);
            position = new Position(34); properties.Add(position);
            position = new Position(35); properties.Add(position);
            position = new Position(36); properties.Add(position);
            position = new Position(37); properties.Add(position);
            position = new Position(38); properties.Add(position);
            position = new Position(39); properties.Add(position);
        }

        private void setIndexesHover()
        {
            index0.MouseLeave += (sender, e) => statsWindow.positionMouseLeave(sender, e);
            index1.MouseLeave += (sender, e) => statsWindow.positionMouseLeave(sender, e);
            index2.MouseLeave += (sender, e) => statsWindow.positionMouseLeave(sender, e);
            index3.MouseLeave += (sender, e) => statsWindow.positionMouseLeave(sender, e);
            index4.MouseLeave += (sender, e) => statsWindow.positionMouseLeave(sender, e);
            index5.MouseLeave += (sender, e) => statsWindow.positionMouseLeave(sender, e);
            index6.MouseLeave += (sender, e) => statsWindow.positionMouseLeave(sender, e);
            index7.MouseLeave += (sender, e) => statsWindow.positionMouseLeave(sender, e);
            index8.MouseLeave += (sender, e) => statsWindow.positionMouseLeave(sender, e);
            index9.MouseLeave += (sender, e) => statsWindow.positionMouseLeave(sender, e);
            index10.MouseLeave += (sender, e) => statsWindow.positionMouseLeave(sender, e);
            index11.MouseLeave += (sender, e) => statsWindow.positionMouseLeave(sender, e);
            index12.MouseLeave += (sender, e) => statsWindow.positionMouseLeave(sender, e);
            index13.MouseLeave += (sender, e) => statsWindow.positionMouseLeave(sender, e);
            index14.MouseLeave += (sender, e) => statsWindow.positionMouseLeave(sender, e);
            index15.MouseLeave += (sender, e) => statsWindow.positionMouseLeave(sender, e);
            index16.MouseLeave += (sender, e) => statsWindow.positionMouseLeave(sender, e);
            index17.MouseLeave += (sender, e) => statsWindow.positionMouseLeave(sender, e);
            index18.MouseLeave += (sender, e) => statsWindow.positionMouseLeave(sender, e);
            index19.MouseLeave += (sender, e) => statsWindow.positionMouseLeave(sender, e);
            index20.MouseLeave += (sender, e) => statsWindow.positionMouseLeave(sender, e);
            index21.MouseLeave += (sender, e) => statsWindow.positionMouseLeave(sender, e);
            index22.MouseLeave += (sender, e) => statsWindow.positionMouseLeave(sender, e);
            index23.MouseLeave += (sender, e) => statsWindow.positionMouseLeave(sender, e);
            index24.MouseLeave += (sender, e) => statsWindow.positionMouseLeave(sender, e);
            index25.MouseLeave += (sender, e) => statsWindow.positionMouseLeave(sender, e);
            index26.MouseLeave += (sender, e) => statsWindow.positionMouseLeave(sender, e);
            index27.MouseLeave += (sender, e) => statsWindow.positionMouseLeave(sender, e);
            index28.MouseLeave += (sender, e) => statsWindow.positionMouseLeave(sender, e);
            index29.MouseLeave += (sender, e) => statsWindow.positionMouseLeave(sender, e);
            index30.MouseLeave += (sender, e) => statsWindow.positionMouseLeave(sender, e);
            index31.MouseLeave += (sender, e) => statsWindow.positionMouseLeave(sender, e);
            index32.MouseLeave += (sender, e) => statsWindow.positionMouseLeave(sender, e);
            index33.MouseLeave += (sender, e) => statsWindow.positionMouseLeave(sender, e);
            index34.MouseLeave += (sender, e) => statsWindow.positionMouseLeave(sender, e);
            index35.MouseLeave += (sender, e) => statsWindow.positionMouseLeave(sender, e);
            index36.MouseLeave += (sender, e) => statsWindow.positionMouseLeave(sender, e);
            index37.MouseLeave += (sender, e) => statsWindow.positionMouseLeave(sender, e);
            index38.MouseLeave += (sender, e) => statsWindow.positionMouseLeave(sender, e);
            index39.MouseLeave += (sender, e) => statsWindow.positionMouseLeave(sender, e);

            index0.MouseEnter += (sender, e) => statsWindow.positionCheck(sender, e, index0);
            index1.MouseEnter += (sender, e) => statsWindow.positionCheck(sender, e, index1);
            index2.MouseEnter += (sender, e) => statsWindow.positionCheck(sender, e, index2);
            index3.MouseEnter += (sender, e) => statsWindow.positionCheck(sender, e, index3);
            index4.MouseEnter += (sender, e) => statsWindow.positionCheck(sender, e, index4);
            index5.MouseEnter += (sender, e) => statsWindow.positionCheck(sender, e, index5);
            index6.MouseEnter += (sender, e) => statsWindow.positionCheck(sender, e, index6);
            index7.MouseEnter += (sender, e) => statsWindow.positionCheck(sender, e, index7);
            index8.MouseEnter += (sender, e) => statsWindow.positionCheck(sender, e, index8);
            index9.MouseEnter += (sender, e) => statsWindow.positionCheck(sender, e, index9);
            index10.MouseEnter += (sender, e) => statsWindow.positionCheck(sender, e, index10);
            index11.MouseEnter += (sender, e) => statsWindow.positionCheck(sender, e, index11);
            index12.MouseEnter += (sender, e) => statsWindow.positionCheck(sender, e, index12);
            index13.MouseEnter += (sender, e) => statsWindow.positionCheck(sender, e, index13);
            index14.MouseEnter += (sender, e) => statsWindow.positionCheck(sender, e, index14);
            index15.MouseEnter += (sender, e) => statsWindow.positionCheck(sender, e, index15);
            index16.MouseEnter += (sender, e) => statsWindow.positionCheck(sender, e, index16);
            index17.MouseEnter += (sender, e) => statsWindow.positionCheck(sender, e, index17);
            index18.MouseEnter += (sender, e) => statsWindow.positionCheck(sender, e, index18);
            index19.MouseEnter += (sender, e) => statsWindow.positionCheck(sender, e, index19);
            index20.MouseEnter += (sender, e) => statsWindow.positionCheck(sender, e, index20);
            index21.MouseEnter += (sender, e) => statsWindow.positionCheck(sender, e, index21);
            index22.MouseEnter += (sender, e) => statsWindow.positionCheck(sender, e, index22);
            index23.MouseEnter += (sender, e) => statsWindow.positionCheck(sender, e, index23);
            index24.MouseEnter += (sender, e) => statsWindow.positionCheck(sender, e, index24);
            index25.MouseEnter += (sender, e) => statsWindow.positionCheck(sender, e, index25);
            index26.MouseEnter += (sender, e) => statsWindow.positionCheck(sender, e, index26);
            index27.MouseEnter += (sender, e) => statsWindow.positionCheck(sender, e, index27);
            index28.MouseEnter += (sender, e) => statsWindow.positionCheck(sender, e, index28);
            index29.MouseEnter += (sender, e) => statsWindow.positionCheck(sender, e, index29);
            index30.MouseEnter += (sender, e) => statsWindow.positionCheck(sender, e, index30);
            index31.MouseEnter += (sender, e) => statsWindow.positionCheck(sender, e, index31);
            index32.MouseEnter += (sender, e) => statsWindow.positionCheck(sender, e, index32);
            index33.MouseEnter += (sender, e) => statsWindow.positionCheck(sender, e, index33);
            index34.MouseEnter += (sender, e) => statsWindow.positionCheck(sender, e, index34);
            index35.MouseEnter += (sender, e) => statsWindow.positionCheck(sender, e, index35);
            index36.MouseEnter += (sender, e) => statsWindow.positionCheck(sender, e, index36);
            index37.MouseEnter += (sender, e) => statsWindow.positionCheck(sender, e, index37);
            index38.MouseEnter += (sender, e) => statsWindow.positionCheck(sender, e, index38);
            index39.MouseEnter += (sender, e) => statsWindow.positionCheck(sender, e, index39);

            index0.MouseLeftButtonDown += (sender, e) => statsWindow.propertyOptions(sender, e, index0);
            index1.MouseLeftButtonDown += (sender, e) => statsWindow.propertyOptions(sender, e, index1);
            index2.MouseLeftButtonDown += (sender, e) => statsWindow.propertyOptions(sender, e, index2);
            index3.MouseLeftButtonDown += (sender, e) => statsWindow.propertyOptions(sender, e, index3);
            index4.MouseLeftButtonDown += (sender, e) => statsWindow.propertyOptions(sender, e, index4);
            index5.MouseLeftButtonDown += (sender, e) => statsWindow.propertyOptions(sender, e, index5);
            index6.MouseLeftButtonDown += (sender, e) => statsWindow.propertyOptions(sender, e, index6);
            index7.MouseLeftButtonDown += (sender, e) => statsWindow.propertyOptions(sender, e, index7);
            index8.MouseLeftButtonDown += (sender, e) => statsWindow.propertyOptions(sender, e, index8);
            index9.MouseLeftButtonDown += (sender, e) => statsWindow.propertyOptions(sender, e, index9);
            index10.MouseLeftButtonDown += (sender, e) => statsWindow.propertyOptions(sender, e, index10);
            index11.MouseLeftButtonDown += (sender, e) => statsWindow.propertyOptions(sender, e, index11);
            index12.MouseLeftButtonDown += (sender, e) => statsWindow.propertyOptions(sender, e, index12);
            index13.MouseLeftButtonDown += (sender, e) => statsWindow.propertyOptions(sender, e, index13);
            index14.MouseLeftButtonDown += (sender, e) => statsWindow.propertyOptions(sender, e, index14);
            index15.MouseLeftButtonDown += (sender, e) => statsWindow.propertyOptions(sender, e, index15);
            index16.MouseLeftButtonDown += (sender, e) => statsWindow.propertyOptions(sender, e, index16);
            index17.MouseLeftButtonDown += (sender, e) => statsWindow.propertyOptions(sender, e, index17);
            index18.MouseLeftButtonDown += (sender, e) => statsWindow.propertyOptions(sender, e, index18);
            index19.MouseLeftButtonDown += (sender, e) => statsWindow.propertyOptions(sender, e, index19);
            index20.MouseLeftButtonDown += (sender, e) => statsWindow.propertyOptions(sender, e, index20);
            index21.MouseLeftButtonDown += (sender, e) => statsWindow.propertyOptions(sender, e, index21);
            index22.MouseLeftButtonDown += (sender, e) => statsWindow.propertyOptions(sender, e, index22);
            index23.MouseLeftButtonDown += (sender, e) => statsWindow.propertyOptions(sender, e, index23);
            index24.MouseLeftButtonDown += (sender, e) => statsWindow.propertyOptions(sender, e, index24);
            index25.MouseLeftButtonDown += (sender, e) => statsWindow.propertyOptions(sender, e, index25);
            index26.MouseLeftButtonDown += (sender, e) => statsWindow.propertyOptions(sender, e, index26);
            index27.MouseLeftButtonDown += (sender, e) => statsWindow.propertyOptions(sender, e, index27);
            index28.MouseLeftButtonDown += (sender, e) => statsWindow.propertyOptions(sender, e, index28);
            index29.MouseLeftButtonDown += (sender, e) => statsWindow.propertyOptions(sender, e, index29);
            index30.MouseLeftButtonDown += (sender, e) => statsWindow.propertyOptions(sender, e, index30);
            index31.MouseLeftButtonDown += (sender, e) => statsWindow.propertyOptions(sender, e, index31);
            index32.MouseLeftButtonDown += (sender, e) => statsWindow.propertyOptions(sender, e, index32);
            index33.MouseLeftButtonDown += (sender, e) => statsWindow.propertyOptions(sender, e, index33);
            index34.MouseLeftButtonDown += (sender, e) => statsWindow.propertyOptions(sender, e, index34);
            index35.MouseLeftButtonDown += (sender, e) => statsWindow.propertyOptions(sender, e, index35);
            index36.MouseLeftButtonDown += (sender, e) => statsWindow.propertyOptions(sender, e, index36);
            index37.MouseLeftButtonDown += (sender, e) => statsWindow.propertyOptions(sender, e, index37);
            index38.MouseLeftButtonDown += (sender, e) => statsWindow.propertyOptions(sender, e, index38);
            index39.MouseLeftButtonDown += (sender, e) => statsWindow.propertyOptions(sender, e, index39);
        }

        #endregion

        #region Request buttons (accept, decline)

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            client.Send("request_accept:"+req_requester+":" + req_u + ":" + req_index_name + ":" + req_amount + ":");
            requestPopUpClose();
        }

        private void Decline_Click(object sender, RoutedEventArgs e)
        {
            client.Send("request_decline:" + req_requester + ":" + req_u + ":" + req_index_name + ":" + req_amount + ":");
            requestPopUpClose();
        }

        #endregion

        // MONOPOLY DATA
        public void Data(string data)//Metoda, ki preveri kaj je ukaz in nato to izvede
        {
            Dispatcher.Invoke((Action)delegate
            {
                bool show = true;
                //Output(data);
                if (data.Contains("dont_show"))
                {
                    show = false;
                }
                
                if (data.Contains("move_forward"))
                {
                    string[] data_split = data.Split(':');
                    var data1 = data_split[1].Replace("\0", string.Empty);
                    var data2 = data_split[2].Replace("\0", string.Empty);
                    moveCharacter(Convert.ToString(data1), Convert.ToInt32(data2));
                    if (show == true)
                        Output(data1 + " has moved forward " + data2 + " positions.");
                }
                else if (data.Contains("add_character:"))
                {
                    string[] data_split = data.Split(':');
                    var username = data_split[1].Replace("\0", string.Empty);
                    var figure = data_split[2].Replace("\0", string.Empty);
                    string rank = data_split[3].Replace("\0", string.Empty);
                    string custom_prefix = data_split[4].Replace("\0", string.Empty);
                    string custom_color = data_split[5].Replace("\0", string.Empty);
                    if (show == true)
                        addCharacter(username, figure, rank, custom_prefix, custom_color);
                }
                else if (data.Contains("remove_character:"))
                {
                    string[] data_split = data.Split(':');
                    var username = data_split[1].Replace("\0", string.Empty);
                    var figure = data_split[2].Replace("\0", string.Empty);
                    if (show == true)
                        removeCharacter(username);
                }
                else if (data.Contains("player_turn:"))
                {
                    string[] data_split = data.Split(':');
                    var username = data_split[1].Replace("\0", string.Empty);
                    yourTurn(username);
                    if (show == true)
                        Output(username + " has turn!");
                }
                else if (data.Contains("dice_throw:"))
                {
                    diceThrown(data);
                }
                else if (data.Contains("game_start:"))
                {
                    gameStart();
                    if (show == true)
                        Output("Game has started");
                }
                else if (data.Contains("game_has_already_started"))
                {
                    gameFull();
                }
                else if (data.Contains("buy_property"))
                {
                    string[] data_split = data.Split(':');
                    var username = data_split[1].Replace("\0", string.Empty);
                    buyProperty(username, Convert.ToInt32(data_split[2]), Convert.ToInt32(data_split[3]));
                }
                else if (data.Contains("sell_property"))
                {
                    string[] data_split = data.Split(':');
                    var username = data_split[1].Replace("\0", string.Empty);
                    sellProperty(username, Convert.ToInt32(data_split[2]), Convert.ToInt32(data_split[3]));
                }
                else if (data.Contains("buy_house"))
                {
                    string[] data_split = data.Split(':');
                    var username = data_split[1].Replace("\0", string.Empty);
                    buyHouse(username, Convert.ToInt32(data_split[2]), Convert.ToInt32(data_split[3]));
                }
                else if (data.Contains("sell_house"))
                {
                    string[] data_split = data.Split(':');
                    var username = data_split[1].Replace("\0", string.Empty);
                    sellHouse(username, Convert.ToInt32(data_split[2]), Convert.ToInt32(data_split[3]));
                }
                else if (data.Contains("buy_hotel"))
                {
                    string[] data_split = data.Split(':');
                    var username = data_split[1].Replace("\0", string.Empty);
                    buyHotel(username, Convert.ToInt32(data_split[2]), Convert.ToInt32(data_split[3]));
                }
                else if (data.Contains("sell_hotel"))
                {
                    string[] data_split = data.Split(':');
                    var username = data_split[1].Replace("\0", string.Empty);
                    sellHotel(username, Convert.ToInt32(data_split[2]), Convert.ToInt32(data_split[3]));
                }
                else if (data.Contains("pay_rent"))
                {
                    string[] data_split = data.Split(':');
                    var payer = data_split[1].Replace("\0", string.Empty);
                    var reciever = data_split[2].Replace("\0", string.Empty);
                    payRent(payer, reciever, Convert.ToInt32(data_split[3]));
                }
                else if (data.Contains("bankrupt"))
                {
                    string[] data_split = data.Split(':');
                    string username = data_split[1].Replace("\0", string.Empty);
                    Bankrupt(username);
                    removeCharacter(username);
                    if (show == true)
                        Output(username + " has gone bankrupt.");
                }
                else if (data.Contains("go_jail"))
                {
                    string[] data_split = data.Split(':');
                    var username = data_split[1].Replace("\0", string.Empty);
                    putInJail(username);
                    if (show == true)
                        Output(username + " went to jail.");
                }
                else if (data.Contains("treasure_show"))
                {
                    string[] data_split = data.Split(':');
                    int index = Convert.ToInt32(data_split[1].Replace("\0", string.Empty));
                    treasureShow(index);
                }
                else if (data.Contains("treasure_hide"))
                {
                    treasureHide();
                }
                else if (data.Contains("recieve_money"))
                {
                    string[] data_split = data.Split(':');
                    var username = data_split[1].Replace("\0", string.Empty);
                    int amount = Convert.ToInt32(data_split[2].Replace("\0", string.Empty));
                    recieveMoney(username, amount);
                    if (show == true)
                        Output(username + " has recieved $" + amount + ".");
                }
                else if (data.Contains("take_money"))
                {
                    string[] data_split = data.Split(':');
                    var username = data_split[1].Replace("\0", string.Empty);
                    int amount = Convert.ToInt32(data_split[2].Replace("\0", string.Empty));
                    takeMoney(username, amount);
                    if (show == true)
                        Output("$" + amount + " has been removed from " + username + ".");
                }
                else if (data.Contains("collect_money_per_player"))
                {
                    string[] data_split = data.Split(':');
                    var username = data_split[1].Replace("\0", string.Empty);
                    int amount = Convert.ToInt32(data_split[2].Replace("\0", string.Empty));
                    collectMoneyPerPlayer(username, amount);
                    if (show == true)
                        Output(username + " has collected " + amount + " from each player.");
                }
                else if (data.Contains("pay_money_per_buildings"))
                {
                    string[] data_split = data.Split(':');
                    var username = data_split[1].Replace("\0", string.Empty);
                    int house_amount = Convert.ToInt32(data_split[2].Replace("\0", string.Empty));
                    int hotel_amount = Convert.ToInt32(data_split[3].Replace("\0", string.Empty));
                    payMoneyPerBuildings(username, house_amount, hotel_amount);
                    if (show == true)
                        Output(username + " has payed $" + house_amount + " for each houes and $" + hotel_amount + " for each hotel he owns.");
                }
                else if (data.Contains("get_out_of_jail_free:"))
                {
                    string[] data_split = data.Split(':');
                    var username = data_split[1].Replace("\0", string.Empty);
                    getOutOfJailFree(username);
                }
                else if (data.Contains("reset_jail_count:"))
                {
                    string[] data_split = data.Split(':');
                    var username = data_split[1].Replace("\0", string.Empty);
                    resetJailCount(username);
                }
                else if (data.Contains("decrease_jail_count:"))
                {
                    string[] data_split = data.Split(':');
                    var username = data_split[1].Replace("\0", string.Empty);
                    decreaseJailCount(username);
                }
                else if (data.Contains("chance_show"))
                {
                    string[] data_split = data.Split(':');
                    int index = Convert.ToInt32(data_split[1].Replace("\0", string.Empty));
                    chanceShow(index);
                }
                else if (data.Contains("chance_hide"))
                {
                    chanceHide();
                }
                else if (data.Contains("move_to"))
                {
                    string[] data_split = data.Split(':');
                    var username = data_split[1].Replace("\0", string.Empty);
                    var position_name = data_split[2].Replace("\0", string.Empty);
                    moveTo(username, position_name);
                    if (show == true)
                        Output(username + " has moved to " + position_name.Substring(0, 1).ToUpper() + position_name.Substring(1).ToLower() + ".");
                }
                else if (data.Contains("nearest_railroad"))
                {
                    string[] data_split = data.Split(':');
                    var username = data_split[1].Replace("\0", string.Empty);
                    Monopoly.double_rent = 2;
                    nearestRailroad(username);
                }
                else if (data.Contains("nearest_utility"))
                {
                    string[] data_split = data.Split(':');
                    var username = data_split[1].Replace("\0", string.Empty);
                    Monopoly.double_rent = 2;
                    nearestUtility(username);
                }
                else if (data.Contains("move_back"))
                {
                    string[] data_split = data.Split(':');
                    var username = data_split[1].Replace("\0", string.Empty);
                    int amount = Convert.ToInt32(data_split[2].Replace("\0", string.Empty));
                    moveBack(username, amount);
                    if (show == true)
                        Output(username + " has moved backwards " + amount + " positions" + ".");
                }
                else if (data.Contains("pay_each_player"))
                {
                    string[] data_split = data.Split(':');
                    var username = data_split[1].Replace("\0", string.Empty);
                    int amount = Convert.ToInt32(data_split[2].Replace("\0", string.Empty));
                    payEachPlayer(username, amount);
                    if (show == true)
                        Output(username + " has payed each player " + amount + ".");
                }
                else if (data.Contains("close_server"))
                {
                    closeServer();
                    if (show == true)
                        Output("Server has been closed.");
                }
                else if (data.Contains("player_winner"))
                {
                    string[] data_split = data.Split(':');
                    var username = data_split[1].Replace("\0", string.Empty);
                    playerWinner(username);
                    if (show == true)
                        Output(username + " has won the game.");
                }
                else if (data.Contains("player_request:"))
                {
                    string[] data_split = data.Split(':');
                    var requester = data_split[1].Replace("\0", string.Empty);
                    var reciever = data_split[2].Replace("\0", string.Empty);
                    var index_name = data_split[3].Replace("\0", string.Empty);
                    var amount = data_split[4].Replace("\0", string.Empty);
                    playerRequest(requester, reciever, index_name, Convert.ToInt32(amount));
                    if (show == true)
                        Output(requester + " has sent request to buy " + index_name.Substring(0, 1).ToUpper() + index_name.Substring(1).ToLower() + " from " + reciever + " for $" + amount + ".");
                }
                else if (data.Contains("request_decline:"))
                {
                    string[] data_split = data.Split(':');
                    var requester = data_split[1].Replace("\0", string.Empty);
                    var reciever = data_split[2].Replace("\0", string.Empty);
                    var index_name = data_split[3].Replace("\0", string.Empty);
                    var amount = data_split[4].Replace("\0", string.Empty);
                    if (show == true)
                        Output(reciever + " has declined request from " + requester + ".");
                    EnableRequestButton();
                }
                else if (data.Contains("request_accept"))
                {
                    string[] data_split = data.Split(':');
                    var requester = data_split[1].Replace("\0", string.Empty);
                    var reciever = data_split[2].Replace("\0", string.Empty);
                    var index_name = data_split[3].Replace("\0", string.Empty);
                    var amount = data_split[4].Replace("\0", string.Empty);
                    requestAccept(requester, reciever, index_name, Convert.ToInt32(amount));
                    if (show == true)
                        Output(reciever + " has accepted request from " + requester + ".");
                    EnableRequestButton();
                }
                else if (data.Contains("message:"))
                {
                    string[] data_split = data.Split(':');
                    string message = data_split[1].Replace("\0", string.Empty);
                    Output(message);
                }
            });
        }
    }
}
