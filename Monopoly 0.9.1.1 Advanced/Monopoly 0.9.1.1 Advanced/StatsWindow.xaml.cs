using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for StatsWindow.xaml
    /// </summary>
    public partial class StatsWindow : Window
    {
        public StatsWindow(Monopoly m)
        {
            InitializeComponent();
            monopoly = m;
            CheckingPlayerClose.Visibility = Visibility.Hidden;
            CheckingPlayer.Visibility = Visibility.Hidden;
            RequestAmount.Foreground = new SolidColorBrush(Color.FromArgb(100, 102, 102, 102));
            RequestAmount.MaxLength = 5;
            hideYourPropertyButtons();
        }

        #region Variables

        string request_figure = "";
        string request_username = "";

        Player options_player;
        Position options_property;

        Player current_player;
        Position current_property;

        Monopoly monopoly;

        #endregion

        public void setUsername()
        {
            foreach (Player p in Monopoly.players)
            {
                if (p.Username() == Monopoly.username)
                {
                    if (p.Prefix == "none")
                    {
                        YourUsername.Text = "Username: [" + p.Rank.ToUpper() + "] " + p.Username();
                        TextEffect rank = new TextEffect();
                        rank.PositionStart = 11;
                        rank.PositionCount = p.Rank.Length;
                        rank.Foreground = Menu.getColor(p.Rank, p.PrefixColor);
                        YourUsername.TextEffects.Add(rank);
                    }
                    else
                    {
                        YourUsername.Text = p.Username();
                        YourUsername.Text = "Username: [" + p.Prefix.ToUpper() + "] " + p.Username();
                        TextEffect rank = new TextEffect();
                        rank.PositionStart = 11;
                        rank.PositionCount = p.Prefix.Length;
                        rank.Foreground = Menu.getColor(p.Rank, p.PrefixColor);
                        YourUsername.TextEffects.Add(rank);
                    }
                }
            }
        }

        private void Head_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        #region Your Position

        public void updateYourPosition()
        {
            YourBalance.Content = "";
            YourPosition.Content = "";
            YourPositionOwner.Content = "";
            YourPositionPrice.Content = "";
            YourPositionHH.Content = "";
            YourPositonRentPrice.Content = "";
            foreach (Player p in Monopoly.players)
            {
                if (p.Username() == Monopoly.username)
                {
                    foreach (Position pos in Monopoly.properties)
                    {
                        if (p.Position == pos.getIndex())
                        {
                            current_player = p;
                            current_property = pos;
                            YourBalance.Content = "Balance: $" + p.Balance;
                            YourPosition.Content = "Position: " + pos.getIndexName().Substring(0, 1) + pos.getIndexName().Substring(1).ToLower();

                            if (pos.isBuyable())
                            {
                                YourPositionOwner.Content = "Owner: " + pos.ownedBy;
                                YourPositionPrice.Content = "Price: $" + pos.Price();
                                if (Monopoly.utility_chance_rent == true)
                                {
                                    if (Monopoly.double_rent == 1)
                                    {
                                        YourPositonRentPrice.Content = "Rent: $" + (Monopoly.dice_amount * 4);
                                    }
                                    else
                                    {
                                        YourPositonRentPrice.Content = "Rent: $" + (Monopoly.dice_amount * 10);
                                    }
                                }
                                else
                                {
                                    if (Monopoly.double_rent > 1)
                                    {
                                        YourPositonRentPrice.Content = "Rent: $" + (pos.CalculateRent() * Monopoly.double_rent);
                                    }
                                    else
                                    {
                                        YourPositonRentPrice.Content = "Rent: $" + pos.CalculateRent();
                                    }
                                }

                                if (pos.BuildingsBuyable())
                                {
                                    YourPositionHH.Content = "Houses: " + pos.Houses + "/4  Hotels: " + pos.Hotels + "/1";
                                }
                            }
                            YourPositionPicture.Background = GetImage(@"pack://application:,,,/" + "Resources/MonopolyData/" + pos.getIndex() + ".png");

                            try
                            {
                                YourPositionTitleDeed.Background = GetImage(@"pack://application:,,,/" + "Resources/MonopolyData/Title Deeds/" + pos.getIndexName() + ".png");
                            }
                            catch (Exception)
                            {
                                YourPositionTitleDeed.Background = Brushes.Transparent;
                            }
                        }
                    }
                }
            }
        }

        public void hideYourPropertyButtons()
        {
            PropertyOptions.Visibility = Visibility.Hidden;
            EndTurn.Visibility = Visibility.Hidden;
            PayRent.Visibility = Visibility.Hidden;

            OutOfJailButton.Visibility = Visibility.Hidden;
            OutOfJailText.Visibility = Visibility.Hidden;
        }

        public void checkYourPropertyButtons(Position pos, Player p)
        {
            hideYourPropertyButtons();
            if (Monopoly.your_turn)
            { 
                if (Monopoly.username == pos.ownedBy)
                {
                    PropertyOptions.Visibility = Visibility.Visible;
                    EndTurn.Visibility = Visibility.Visible;
                }
                else if (pos.ownedBy == "/" && pos.getIndex() == p.Position)
                {
                    PropertyOptions.Visibility = Visibility.Visible;
                    options_player = p;
                    options_property = pos;
                    EndTurn.Visibility = Visibility.Visible;
                }
                else if (!Monopoly.rent_paid)
                {
                    PayRent.Visibility = Visibility.Visible;
                }

                if (!pos.isBuyable())
                {
                    PropertyOptions.Visibility = Visibility.Hidden;
                }
            }
        }

        public void checkWhereYouLanded()
        {
            foreach (Player pl in Monopoly.players)
            {
                if (pl.Username() == Monopoly.username)
                {
                    foreach (Position p in Monopoly.properties)
                    {
                        if (p.getIndex() == pl.Position)
                        {
                            hideYourPropertyButtons();
                            if (p.getIndex() == 7 || p.getIndex() == 22 || p.getIndex() == 36)//chance
                            {
                                Monopoly.chance_enabled = true;
                                monopoly.Chance.Cursor = Cursors.Hand;
                            }
                            else if (p.getIndex() == 2 || p.getIndex() == 17 || p.getIndex() == 33)//community chest
                            {
                                Monopoly.treasure_enabled = true;
                                monopoly.Treasure.Cursor = Cursors.Hand;
                            }
                            else if (p.getIndex() == 10 || p.getIndex() == 20 || p.getIndex() == 0) //empty properties
                            {
                                EndTurn.Visibility = Visibility.Visible;
                            }
                            else if (p.getIndex() == 4 || p.getIndex() == 38) //income tax, luxury tax
                            {
                                PayRent.Visibility = Visibility.Visible;
                            }
                            else if (p.getIndex() == 30) //jail
                            {
                                Monopoly.client.Send("go_jail:" + Monopoly.username + ":");
                                Monopoly.client.Send("end_turn:" + Monopoly.username + ":");
                            }
                            else
                            {
                                checkYourPropertyButtons(p, pl);
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region Property Options

        public void propertyOptions(object senderr, EventArgs ee, WrapPanel f)
        {
            if (showPanel() && Monopoly.your_turn || YourPropertyOptions.Visibility == Visibility.Visible && Monopoly.your_turn)
            {
                int index = Convert.ToInt32(f.Name.Replace("index", String.Empty));
                foreach (Position pos in Monopoly.properties)
                {
                    if (pos.getIndex() == index && pos.isBuyable() == true)
                    {
                        foreach (Player p in Monopoly.players)
                        {
                            if (p.Username() == Monopoly.username && pos.ownedBy == Monopoly.username)
                            {
                                options_player = p;
                                options_property = pos;
                                propertyOptionsShow();
                            }
                        }
                    }
                }
            }
        }

        public void updatePropertyOptions()
        {
            int index = options_property.getIndex();
            Monopoly.client.Output("" + index);
            foreach (Position pos in Monopoly.properties)
            {
                if (pos.getIndex() == index && pos.isBuyable() == true)
                {
                    foreach (Player p in Monopoly.players)
                    {
                        if (p.Username() == Monopoly.username)
                        {
                            options_player = p;
                            options_property = pos;
                            propertyOptionsShow();
                        }
                    }
                }
            }
        }

        public void propertyOptionsShow()
        {
            RequestSellBox.Visibility = Visibility.Hidden;
            RequestUsernameBox.Visibility = Visibility.Hidden;
            RequestAmountBox.Visibility = Visibility.Hidden;
            RequestErrorBox.Visibility = Visibility.Hidden;
            RequestGrid.Visibility = Visibility.Hidden;

            PropertyOptionsHouses.Visibility = Visibility.Visible;
            PropertyOptionsHotels.Visibility = Visibility.Visible;
            PropertyOptionsRent.Visibility = Visibility.Visible;
            PropertyOptionsPrice.Visibility = Visibility.Visible;
            PropertyOptionsTitleDeed.Visibility = Visibility.Visible;
            PropertyOptionsTitleDeed.Background = GetImage(@"pack://application:,,,/" + "Resources/MonopolyData/Title Deeds/" + options_property.getIndexName() + ".png");

            PropertyOptionsClose.Visibility = Visibility.Visible;

            RequestAmount.Text = "Amount";
            RequestError.Content = "";

            if (options_property.BuildingsBuyable())
            {
                PropertyOptionsHouses.Content = "Houses: " + options_property.Houses + "/4";
                PropertyOptionsHotels.Content = "Hotels: " + options_property.Hotels + "/1";
            }
            else
            {
                PropertyOptionsHouses.Content = "";
                PropertyOptionsHotels.Content = "";
            }

            PropertyOptionsRent.Content = "Rent: $" + options_property.CalculateRent();
            PropertyOptionsPrice.Content = "Price: $" + options_property.Price();

            hideEverything();
            propertyOptionsHideButtons();
            YourPropertyOptions.Visibility = Visibility.Visible;
            if (options_property.ownedBy == Monopoly.username || current_property.ownedBy == "/")
            {
                PropertyOptionsName.Content = "Property options for: " + options_property.getIndexName().Substring(0, 1) + options_property.getIndexName().Substring(1).ToLower();
                int button_counter = 0;
                if (options_property.getIndex() == options_player.Position && Monopoly.username != options_property.ownedBy && options_property.ownedBy == "/")
                {
                    BuyProperty.Visibility = Visibility.Visible;
                    BuyProperty1.Content = "BUY $" + options_property.Price();
                    addButton(BuyProperty, button_counter);
                    button_counter++;
                }
                if (options_property.ownedBy == Monopoly.username)
                {
                    SellProperty.Visibility = Visibility.Visible;
                    SellProperty1.Content = "SELL $" + (options_property.Price() / 2);
                    addButton(SellProperty, button_counter);
                    button_counter++;
                    SellToPlayer.Visibility = Visibility.Visible;
                    addButton(SellToPlayer, button_counter);
                    button_counter++;
                }

                if (options_property.BuildingsBuyable())
                {
                    if (options_property.allColorGroup())
                    {
                        if (options_property.Houses < 4 && options_property.Hotels < 1 && options_property.isBuildingEvenly() && options_property.ownedBy == Monopoly.username)
                        {
                            BuyHouse.Visibility = Visibility.Visible;
                            BuyHouse1.Content = "BUY HOUSE $" + options_property.BuyHousePrice();
                            addButton(BuyHouse, button_counter);
                            button_counter++;
                        }
                        if (options_property.Houses >= 4 && options_property.Hotels < 1)
                        {
                            BuyHotel.Visibility = Visibility.Visible;
                            BuyHotel1.Content = "BUY HOTEL $" + options_property.BuyHotelPrice();
                            addButton(BuyHotel, button_counter);
                            button_counter++;
                        }
                    }

                    if (options_property.Houses > 0)
                    {
                        SellHouse.Visibility = Visibility.Visible;
                        SellHouse1.Content = "SELL HOUSE $" + (options_property.BuyHousePrice() / 2);
                        addButton(SellHouse, button_counter);
                        button_counter++;
                    }
                    if (options_property.Hotels > 0)
                    {
                        SellHotel.Visibility = Visibility.Visible;
                        SellHotel1.Content = "SELL HOTEL $" + (options_property.BuyHotelPrice() / 2);
                        addButton(SellHotel, button_counter);
                        button_counter++;
                    }
                }
            }
            else
            {
                PropertyOptionsName.Content = "You don't own this property";
                PropertyOptionsHouses.Visibility = Visibility.Hidden;
                PropertyOptionsHotels.Visibility = Visibility.Hidden;
                PropertyOptionsRent.Visibility = Visibility.Hidden;
                PropertyOptionsPrice.Visibility = Visibility.Hidden;
                PropertyOptionsTitleDeed.Visibility = Visibility.Hidden;
            }
        }

        private void addButton(UIElement b, int counter)
        {
            switch (counter)
            {
                case 0:
                    Grid.SetRow(b, 3);
                    Grid.SetColumn(b, 1);
                    YourPropertyOptions.Children.Add(b);
                    break;
                case 1:
                    Grid.SetRow(b, 4);
                    Grid.SetColumn(b, 1);
                    YourPropertyOptions.Children.Add(b);
                    break;
                case 2:
                    Grid.SetRow(b, 5);
                    Grid.SetColumn(b, 1);
                    YourPropertyOptions.Children.Add(b);
                    break;
                case 3:
                    Grid.SetRow(b, 6);
                    Grid.SetColumn(b, 1);
                    YourPropertyOptions.Children.Add(b);
                    break;
                case 4:
                    Grid.SetRow(b, 7);
                    Grid.SetColumn(b, 1);
                    YourPropertyOptions.Children.Add(b);
                    break;
                case 5:
                    Grid.SetRow(b, 8);
                    Grid.SetColumn(b, 1);
                    YourPropertyOptions.Children.Add(b);
                    break;
                case 6:
                    Grid.SetRow(b, 9);
                    Grid.SetColumn(b, 1);
                    YourPropertyOptions.Children.Add(b);
                    break;
                case 7:
                    Grid.SetRow(b, 10);
                    Grid.SetColumn(b, 1);
                    YourPropertyOptions.Children.Add(b);
                    break;
                case 8:
                    Grid.SetRow(b, 11);
                    Grid.SetColumn(b, 1);
                    YourPropertyOptions.Children.Add(b);
                    break;
            }
        }

        private void PropertyOptionsClose_Click(object sender, RoutedEventArgs e)
        {
            YourPropertyOptions.Visibility = Visibility.Hidden;
            CheckingPosition.Visibility = Visibility.Visible;
        }

        private void propertyOptionsHideButtons()
        {
            BuyProperty.Visibility = Visibility.Hidden;
            SellProperty.Visibility = Visibility.Hidden;
            BuyHouse.Visibility = Visibility.Hidden;
            SellHouse.Visibility = Visibility.Hidden;
            BuyHotel.Visibility = Visibility.Hidden;
            SellHotel.Visibility = Visibility.Hidden;
            SellToPlayer.Visibility = Visibility.Hidden;
            Mortgage.Visibility = Visibility.Hidden;

            YourPropertyOptions.Children.Remove(BuyProperty);
            YourPropertyOptions.Children.Remove(SellProperty);
            YourPropertyOptions.Children.Remove(BuyHouse);
            YourPropertyOptions.Children.Remove(SellHouse);
            YourPropertyOptions.Children.Remove(BuyHotel);
            YourPropertyOptions.Children.Remove(SellHotel);
            YourPropertyOptions.Children.Remove(SellToPlayer);
            YourPropertyOptions.Children.Remove(Mortgage);
        }

        #endregion  

        #region Check property

        public void positionCheck(object senderr, EventArgs ee, WrapPanel f)
        {
            if (showPanel())
            {
                hideEverything();
                int index = Convert.ToInt32(f.Name.Replace("index", String.Empty));
                foreach (Position pos in Monopoly.properties)
                {
                    CheckingPosition.Visibility = Visibility.Visible;
                    if (pos.getIndex() == index)
                    {
                        CheckingPositionPicture.Background = GetImage(@"pack://application:,,,/" + "Resources/MonopolyData/" + pos.getIndex() + ".png");
                        try
                        {
                            CheckingPositionTitleDeed.Background = GetImage(@"pack://application:,,,/" + "Resources/MonopolyData/Title Deeds/" + pos.getIndexName() + ".png");
                        }
                        catch (Exception)
                        {
                            CheckingPositionTitleDeed.Background = Brushes.Transparent;
                        }
                        CheckingPositionName.Content = "Position: " + pos.getIndexName().Substring(0, 1) + pos.getIndexName().Substring(1).ToLower();
                        if (pos.isBuyable())
                        {
                            CheckingPositionOwner.Content = "Owner: " + pos.ownedBy;
                            CheckingPositionPrice.Content = "Price: $" + pos.Price();
                        }
                        else
                        {
                            CheckingPositionOwner.Content = "";
                            CheckingPositionPrice.Content = "";
                        }

                        if (pos.BuildingsBuyable())
                        {
                            CheckingPositionHouses.Content = "Houses: " + pos.Houses + "/4";
                            CheckingPositionHotels.Content = "Hotels: " + pos.Hotels + "/1";
                            CheckingPositionHousesPrice.Content = "House price: $" + pos.BuyHousePrice();
                            CheckingPositionHotelsPrice.Content = "Hotel price: $" + pos.BuyHotelPrice();
                            CheckingPositionRentPrice.Content = "Rent: $" + pos.CalculateRent();
                        }
                        else
                        {
                            CheckingPositionHouses.Content = "";
                            CheckingPositionHotels.Content = "";
                            CheckingPositionHousesPrice.Content = "";
                            CheckingPositionHotelsPrice.Content = "";
                            CheckingPositionRentPrice.Content = "";
                        }

                        if (pos.CalculateRent() > 0)
                        {
                            CheckingPositionRentPrice.Content = "Rent: $" + pos.CalculateRent();
                        }
                    }
                }
            }

        }

        #endregion

        #region Check figure of player

        public void figureCheckLock(object sender, EventArgs e)
        {
            if (showPanel())
                CheckingPlayerClose.Visibility = Visibility.Visible;
        }

        private void CheckingPlayerClose_Click(object sender, RoutedEventArgs e)
        {
            CheckingPlayerClose.Visibility = Visibility.Hidden;
            CheckingPlayer.Visibility = Visibility.Hidden;
        }

        public void figureCheck(object senderr, EventArgs ee, Canvas f)
        {
            if (showPanel())
            {
                hideEverything();
                CheckingPlayer.Visibility = Visibility.Visible;
                PageGrid.Visibility = Visibility.Hidden;
                CheckingPlayerTitleDeeds1.Visibility = Visibility.Visible;
                CheckingPlayerTitleDeeds2.Visibility = Visibility.Hidden;
                CheckingTitleDeedGrid.Visibility = Visibility.Hidden;
                Page.Content = "Page 1";
                CheckingPlayerTitleDeeds1.Children.Clear();
                CheckingPlayerTitleDeeds2.Children.Clear();

                foreach (Player p in Monopoly.players)
                {
                    Monopoly.client.Output(p.Username() + ":" + f.Name);

                    if (f.Name == p.Username())
                    {
                        CheckingPlayerName.TextEffects.Clear();
                        CheckingPlayerRank.TextEffects.Clear();
                        if (p.Prefix == "none")
                        {
                            CheckingPlayerName.Text = "Username: [" + p.Rank.ToUpper() + "] " + p.Username();
                            TextEffect effect1 = new TextEffect();
                            effect1.PositionStart = 11;
                            effect1.PositionCount = p.Rank.Length;
                            effect1.Foreground = Menu.getColor(p.Rank, p.PrefixColor);
                            CheckingPlayerName.TextEffects.Add(effect1);
                        }
                        else
                        {
                            CheckingPlayerName.Text = "Username: [" + p.Prefix.ToUpper() + "] " + p.Username();
                            TextEffect effect2 = new TextEffect();
                            effect2.PositionStart = 11;
                            effect2.PositionCount = p.Prefix.Length;
                            effect2.Foreground = Menu.getColor(p.Rank, p.PrefixColor);
                            CheckingPlayerName.TextEffects.Add(effect2);
                        }


                        CheckingPlayerRank.Text = "Rank: " + p.Rank.Substring(0, 1).ToUpper() + p.Rank.Substring(1).ToLower();
                        TextEffect rank = new TextEffect();
                        rank.PositionStart = 6;
                        rank.PositionCount = p.Rank.Length;
                        rank.Foreground = Menu.getColor(p.Rank, "none");
                        CheckingPlayerRank.TextEffects.Add(rank);

                        CheckingPlayerBalance.Content = "Balance: $" + p.Balance;
                        CheckingPlayerPosition.Content = "Position: " + (p.Position + 1);
                        CheckingPlayerPicture.Background = GetImage(@"pack://application:,,,/" + "Resources/MonopolyData/" + p.Figure() + ".png");
                        int deed_counter = 0;
                        foreach (Position pos in Monopoly.properties)
                        {
                            if (pos.ownedBy == p.Username())
                            {
                                Canvas title_deed = new Canvas();
                                title_deed.Background = GetImage(@"pack://application:,,,/" + "Resources/MonopolyData/Title Deeds/" + pos.getIndexName() + ".png");
                                title_deed.MouseDown += (sender, e) => checkingTitleDeedPos(sender, e, pos);

                                if (deed_counter < 15)
                                    addTitleDeedToPlayer(CheckingPlayerTitleDeeds1, title_deed, deed_counter);
                                else
                                    addTitleDeedToPlayer(CheckingPlayerTitleDeeds2, title_deed, deed_counter);
                                deed_counter++;
                                if (deed_counter > 15)
                                {
                                    PageGrid.Visibility = Visibility.Visible;
                                }
                            }
                        }
                    }
                }
            }
        }

        public void figureCheckRemove(object senderr, EventArgs ee, Canvas f)
        {
            if(CheckingPlayerClose.Visibility == Visibility.Hidden)
            {
                CheckingPlayer.Visibility = Visibility.Hidden;
            }
        }

        private void checkingTitleDeedPos(object sender, EventArgs e, Position pos)
        {
            CheckingPlayerTitleDeeds1.Visibility = Visibility.Hidden;
            CheckingPlayerTitleDeeds2.Visibility = Visibility.Hidden;
            CheckingTitleDeedGrid.Visibility = Visibility.Visible;
            CheckingTitleDeedHouses.Content = "Houses: " + pos.Houses;
            CheckingTitleDeedHotels.Content = "Hotels: " + pos.Houses;
            CheckingTitleDeedRent.Content = "Rent: $" + pos.CalculateRent();
            CheckingTitleDeedPicture.Background = GetImage(@"pack://application:,,,/" + "Resources/MonopolyData/Title Deeds/" + pos.getIndexName() + ".png");
        }

        private void addTitleDeedToPlayer(Grid grid, Canvas title_deed, int counter)
        {
            if (grid.Name == "CheckingPlayerTitleDeeds2")
            {
                counter = counter - 15;
            }
            switch (counter)
            {
                case 0:
                    Grid.SetRow(title_deed, 0);
                    Grid.SetColumn(title_deed, 0);
                    grid.Children.Add(title_deed);
                    break;
                case 1:
                    Grid.SetRow(title_deed, 0);
                    Grid.SetColumn(title_deed, 2);
                    grid.Children.Add(title_deed);
                    break;
                case 2:
                    Grid.SetRow(title_deed, 0);
                    Grid.SetColumn(title_deed, 4);
                    grid.Children.Add(title_deed);
                    break;
                case 3:
                    Grid.SetRow(title_deed, 0);
                    Grid.SetColumn(title_deed, 6);
                    grid.Children.Add(title_deed);
                    break;
                case 4:
                    Grid.SetRow(title_deed, 0);
                    Grid.SetColumn(title_deed, 8);
                    grid.Children.Add(title_deed);
                    break;
                case 5:
                    Grid.SetRow(title_deed, 2);
                    Grid.SetColumn(title_deed, 0);
                    grid.Children.Add(title_deed);
                    break;
                case 6:
                    Grid.SetRow(title_deed, 2);
                    Grid.SetColumn(title_deed, 2);
                    grid.Children.Add(title_deed);
                    break;
                case 7:
                    Grid.SetRow(title_deed, 2);
                    Grid.SetColumn(title_deed, 4);
                    grid.Children.Add(title_deed);
                    break;
                case 8:
                    Grid.SetRow(title_deed, 2);
                    Grid.SetColumn(title_deed, 6);
                    grid.Children.Add(title_deed);
                    break;
                case 9:
                    Grid.SetRow(title_deed, 2);
                    Grid.SetColumn(title_deed, 8);
                    grid.Children.Add(title_deed);
                    break;
                case 10:
                    Grid.SetRow(title_deed, 4);
                    Grid.SetColumn(title_deed, 0);
                    grid.Children.Add(title_deed);
                    break;
                case 11:
                    Grid.SetRow(title_deed, 4);
                    Grid.SetColumn(title_deed, 2);
                    grid.Children.Add(title_deed);
                    break;
                case 12:
                    Grid.SetRow(title_deed, 4);
                    Grid.SetColumn(title_deed, 4);
                    grid.Children.Add(title_deed);
                    break;
                case 13:
                    Grid.SetRow(title_deed, 4);
                    Grid.SetColumn(title_deed, 6);
                    grid.Children.Add(title_deed);
                    break;
                case 14:
                    Grid.SetRow(title_deed, 4);
                    Grid.SetColumn(title_deed, 8);
                    grid.Children.Add(title_deed);
                    break;
            }
        }

        private void PageBack_Click(object sender, RoutedEventArgs e)
        {
            if (CheckingPlayerTitleDeeds2.Visibility == Visibility.Visible)
            {
                CheckingPlayerTitleDeeds1.Visibility = Visibility.Visible;
                CheckingPlayerTitleDeeds2.Visibility = Visibility.Hidden;
                Page.Content = "Page 1";
            }
        }

        private void PageForward_Click(object sender, RoutedEventArgs e)
        {
            if (CheckingPlayerTitleDeeds1.Visibility == Visibility.Visible)
            {
                CheckingPlayerTitleDeeds2.Visibility = Visibility.Visible;
                CheckingPlayerTitleDeeds1.Visibility = Visibility.Hidden;
                Page.Content = "Page 2";
            }
        }

        private void CheckingTitleDeedPicture_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Convert.ToString(Page.Content) == "Page 1")
            {
                CheckingTitleDeedGrid.Visibility = Visibility.Hidden;
                CheckingPlayerTitleDeeds1.Visibility = Visibility.Visible;
                CheckingPlayerTitleDeeds2.Visibility = Visibility.Hidden;
            }
            else if (Convert.ToString(Page.Content) == "Page 2")
            {
                CheckingTitleDeedGrid.Visibility = Visibility.Hidden;
                CheckingPlayerTitleDeeds2.Visibility = Visibility.Visible;
                CheckingPlayerTitleDeeds1.Visibility = Visibility.Hidden;
            }
        }
        #endregion

        private ImageBrush GetImage(string path)
        {
            BitmapImage bitmap_image = new BitmapImage(new Uri(path));
            ImageBrush image = new ImageBrush();
            image.ImageSource = bitmap_image;
            image.Stretch = Stretch.Uniform;
            image.AlignmentX = AlignmentX.Left;
            image.AlignmentY = AlignmentY.Top;
            return image;
        }

        public void hideEverything()
        {
            CheckingPlayer.Visibility = Visibility.Hidden;
            CheckingPosition.Visibility = Visibility.Hidden;
            YourPropertyOptions.Visibility = Visibility.Hidden;
        }

        private bool showPanel()
        {
            if (CheckingPlayerClose.Visibility == Visibility.Visible)
            {
                return false;
            }
            else if (YourPropertyOptions.Visibility == Visibility.Visible)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void positionMouseLeave(object sender, EventArgs e)
        {
            if (CheckingPosition.Visibility == Visibility.Visible)
            {
                hideEverything();
            }
        }

        #region Textbox methods

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }


        private void RequestAmount_GotFocus(object sender, RoutedEventArgs e)
        {
            if (RequestAmount.Text.ToLower().Trim().Equals("amount"))
            {
                RequestAmount.Text = "";
                RequestAmount.Foreground = Brushes.Black;
            }
        }

        private void RequestAmount_LostFocus(object sender, RoutedEventArgs e)
        {
            if (RequestAmount.Text.ToLower().Trim().Equals("amount") || RequestAmount.Text.Trim().Equals(""))
            {
                RequestAmount.Text = "Amount";
                RequestAmount.Foreground = new SolidColorBrush(Color.FromArgb(100, 102, 102, 102));
            }
        }

        private void RequestAmount_KeyDown(object sender, KeyEventArgs e)
        {
            if (!RequestAmount.Text.Contains('$'))
            {
                RequestAmount.Text = RequestAmount.Text + "$";
                RequestAmount.Text = RequestAmount.Text.Replace(" ", String.Empty);
                RequestAmount.CaretIndex = RequestAmount.Text.Length; //Cursor v textboxu premakne na konec.
            }
        }

        private void RequestAmount_KeyUp(object sender, KeyEventArgs e)
        {
            RequestAmount.Text = RequestAmount.Text.Replace(" ", String.Empty);
            RequestAmount.CaretIndex = RequestAmount.Text.Length; //Cursor v textboxu premakne na konec.
        }

        #endregion

        #region Property options button click events

        private void BuyProperty1_Click(object sender, RoutedEventArgs e)
        {
            if (options_player.Balance > options_property.Price())
            {
                Monopoly.client.Send("buy_property:" + Monopoly.username + ":" + options_property.getIndex() + ":" + options_property.Price() + ":");
            }
            else
            {
                MessageBox.Show("You do not have enough money to buy this property.", "Insufficient funds", MessageBoxButton.OK);
            }
        }

        private void SellProperty1_Click(object sender, RoutedEventArgs e)
        {
            if (options_property.Hotels == 0 && options_property.Houses == 0)
            {
                Monopoly.client.Send("sell_property:" + Monopoly.username + ":" + options_property.getIndex() + ":" + options_property.Price() / 2 + ":");
            }
            else
            {
                MessageBox.Show("You cannot sell property if property has buildings on it.", "Cannot sell property", MessageBoxButton.OK);
            }
        }

        private void BuyHouse1_Click(object sender, RoutedEventArgs e)
        {
            if (options_player.Balance > options_property.BuyHousePrice())
            {
                Monopoly.client.Send("buy_house:" + Monopoly.username + ":" + options_property.getIndex() + ":" + options_property.BuyHousePrice() + ":");
            }
            else
            {
                MessageBox.Show("You do not have enough money to buy House.", "Insufficient funds", MessageBoxButton.OK);
            }
        }

        private void SellHouse1_Click(object sender, RoutedEventArgs e)
        {
            Monopoly.client.Send("sell_house:" + Monopoly.username + ":" + options_property.getIndex() + ":" + options_property.BuyHousePrice() / 2 + ":");
        }

        private void BuyHotel1_Click(object sender, RoutedEventArgs e)
        {
            if (options_player.Balance > options_property.BuyHotelPrice())
            {
                Monopoly.client.Send("buy_hotel:" + Monopoly.username + ":" + options_property.getIndex() + ":" + options_property.BuyHotelPrice() + ":");
            }
            else
            {
                MessageBox.Show("You do not have enough money to buy Hotel.", "Insufficient funds", MessageBoxButton.OK);
            }
        }

        private void SellHotel1_Click(object sender, RoutedEventArgs e)
        {
            Monopoly.client.Send("sell_hotel:" + Monopoly.username + ":" + options_property.getIndex() + ":" + options_property.BuyHotelPrice() / 2 + ":");
        }

        private void SellToPlayer1_Click(object sender, RoutedEventArgs e)
        {
            if (RequestSellBox.Visibility == Visibility.Hidden)
            {
                RequestSellBox.Visibility = Visibility.Visible;
                RequestUsernameBox.Visibility = Visibility.Visible;
                RequestAmountBox.Visibility = Visibility.Visible;
                RequestErrorBox.Visibility = Visibility.Visible;

                RequestError.Content = "";
                RequestAmount.Text = "Amount";
                RequestAmount.Foreground = new SolidColorBrush(Color.FromArgb(100, 102, 102, 102));
                RequestUsernameBox.Visibility = Visibility.Visible;
                RequestUsername.Content = "";
                RequestGrid.Visibility = Visibility.Visible;
                request_figure = "";
                selectRemoveBorders();
                requestGridSetButtons();

                PropertyOptionsHouses.Visibility = Visibility.Hidden;
                PropertyOptionsHotels.Visibility = Visibility.Hidden;
                PropertyOptionsRent.Visibility = Visibility.Hidden;
                PropertyOptionsPrice.Visibility = Visibility.Hidden;
                PropertyOptionsTitleDeed.Visibility = Visibility.Hidden;
            }
            else
            {
                RequestSellBox.Visibility = Visibility.Hidden;
                RequestUsernameBox.Visibility = Visibility.Hidden;
                RequestAmountBox.Visibility = Visibility.Hidden;
                RequestErrorBox.Visibility = Visibility.Hidden;
                RequestGrid.Visibility = Visibility.Hidden;

                PropertyOptionsHouses.Visibility = Visibility.Visible;
                PropertyOptionsHotels.Visibility = Visibility.Visible;
                PropertyOptionsRent.Visibility = Visibility.Visible;
                PropertyOptionsPrice.Visibility = Visibility.Visible;
                PropertyOptionsTitleDeed.Visibility = Visibility.Visible;
            }
        }

        public void requestGridSetButtons() 
        {
            RequestGrid.Children.Remove(SelectCar);
            RequestGrid.Children.Remove(SelectDog);
            RequestGrid.Children.Remove(SelectHat);
            RequestGrid.Children.Remove(SelectShip);
            RequestGrid.Children.Remove(SelectIron);
            RequestGrid.Children.Remove(SelectShoe);
            RequestGrid.Children.Remove(SelectThimble);
            RequestGrid.Children.Remove(SelectCustom);
            int buttons = 0;
            foreach (Player pl in Monopoly.players)
            {
                if (pl.Bankrupt == false && pl.Username() != Monopoly.username)
                {
                    setRequestButtons(pl.Figure(), buttons);
                    buttons++;
                }
            }
        }

        public void setRequestButtons(string figure, int buttons) 
        {
            UIElement f = SelectCar;
            if (figure == "car")
            {
                f = SelectCar;
            }
            else if (figure == "dog")
            {
                f = SelectDog;
            }
            else if(figure == "hat")
            {
                f = SelectHat;
            }
            else if(figure == "ship")
            {
                f = SelectShip;
            }
            else if(figure == "iron")
            {
                f = SelectIron;
            }
            else if(figure == "shoe")
            {
                f = SelectShoe;
            }
            else if(figure == "thimble")
            {
                f = SelectThimble;
            }
            else if(figure == "custom")
            {
                f = SelectCustom;
            }

            switch (buttons)
            {
                case 0:
                    Grid.SetRow(f, 0);
                    Grid.SetColumn(f, 0);
                    RequestGrid.Children.Add(f);
                    break;
                case 1:
                    Grid.SetRow(f, 0);
                    Grid.SetColumn(f, 1);
                    RequestGrid.Children.Add(f);
                    break;
                case 2:
                    Grid.SetRow(f, 0);
                    Grid.SetColumn(f, 2);
                    RequestGrid.Children.Add(f);
                    break;
                case 3:
                    Grid.SetRow(f, 0);
                    Grid.SetColumn(f, 3);
                    RequestGrid.Children.Add(f);
                    break;
                case 4:
                    Grid.SetRow(f, 1);
                    Grid.SetColumn(f, 0);
                    RequestGrid.Children.Add(f);
                    break;
                case 5:
                    Grid.SetRow(f, 1);
                    Grid.SetColumn(f, 1);
                    RequestGrid.Children.Add(f);
                    break;
                case 6:
                    Grid.SetRow(f, 1);
                    Grid.SetColumn(f, 2);
                    RequestGrid.Children.Add(f);
                    break;
                case 7:
                    Grid.SetRow(f, 1);
                    Grid.SetColumn(f, 3);
                    RequestGrid.Children.Add(f);
                    break;
            }
        }


        #endregion

        #region Main 3 Buttons (PayRent, PropertyOptions, EndTurn)

        private void PayRent_Click(object sender, RoutedEventArgs e)
        {
            if (current_player.Balance > current_property.CalculateRent())
            {
                if (Monopoly.rent_paid == false)
                {
                    payRent();
                }
            }
            else
            {
                if (checkIfPlayerOwnsProperty())
                {
                    MessageBox.Show("You cannot go bankrupt because you still own property.", "You still own property", MessageBoxButton.OK);
                }
                else
                {
                    MessageBox.Show("You have gone bankrupt.", "Monopoly - GAME OVER", MessageBoxButton.OK);
                    goneBankrupt();
                }
            }
        }

        private void PropertyOptions_Click(object sender, RoutedEventArgs e)
        {
            options_player = current_player;
            options_property = current_property;
            propertyOptionsShow();
        }

        private void EndTurn_Click(object sender, RoutedEventArgs e)
        {
            Monopoly.client.Send("end_turn:" + Monopoly.username + ":");
            hideYourPropertyButtons();
            if (YourPropertyOptions.Visibility == Visibility.Visible)
            {
                YourPropertyOptions.Visibility = Visibility.Hidden;
                CheckingPosition.Visibility = Visibility.Visible;
            }
            Monopoly.end_turn_timer.Stop();
            monopoly.endTurnTimer.Visibility = Visibility.Hidden;
        }

        #endregion

        private void payRent()
        {
            if (Monopoly.utility_chance_rent == true)
            {
                Monopoly.client.Send("pay_rent:" + Monopoly.username + ":" + current_property.ownedBy + ":" + (Monopoly.dice_amount * 10) + ":");
            }
            else
            {
                Monopoly.client.Send("pay_rent:" + Monopoly.username + ":" + current_property.ownedBy + ":" + current_property.CalculateRent() * Monopoly.double_rent + ":");
            }
            Monopoly.rent_paid = true;
            Monopoly.double_rent = 1;
            Monopoly.utility_chance_rent = false;
            PayRent.Visibility = Visibility.Hidden;
            EndTurn.Visibility = Visibility.Visible;
        }

        private bool checkIfPlayerOwnsProperty()
        {
            foreach (Position position in Monopoly.properties)
            {
                if (position.ownedBy == Monopoly.username)
                {
                    return true;
                }
            }
            return false;
        }

        private void goneBankrupt()
        {
            Monopoly.client.Send("bankrupt:" + Monopoly.username + ":");
            Monopoly.client.Send("end_turn:" + Monopoly.username + ":");
            this.Hide();
        }

        private void sendRequest(string username, int amount) 
        {
            RequestSell.IsEnabled = false;
            Monopoly.client.Send("player_request:" + Monopoly.username + ":" + username + ":" + options_property.getIndexName() + ":" + amount + ":");
        }

        #region Request to other player 

        private void RequestSell_Click(object sender, RoutedEventArgs e)
        {
            string username = request_username;

            string amount_string = RequestAmount.Text;

            if (playerExists(username))
            {
                if (amount_string != "Amount")
                {
                    try
                    {
                        int amount = Convert.ToInt32(amount_string.TrimStart('$'));
                        if (playerHasMoney(username, amount))
                        {
                            if (username != Monopoly.username)
                            {
                                sendRequest(username, amount);
                                RequestError.Foreground = Brushes.Green;
                                RequestError.Content = "Request sent";
                            }
                            else
                            {
                                RequestError.Foreground = Brushes.Red;
                                RequestError.Content = "You can't send request to yourself";
                            }
                        }
                        else
                        {
                            RequestError.Foreground = Brushes.Red;
                            RequestError.Content = "User doesn't have enough money";
                        }
                    }
                    catch (Exception)
                    {
                        RequestError.Foreground = Brushes.Red;
                        RequestError.Content = "Invalid amount";
                    }
                }
                else
                {
                    RequestError.Foreground = Brushes.Red;
                    RequestError.Content = "Invalid amount";
                }
            }
            else
            {
                RequestError.Foreground = Brushes.Red;
                RequestError.Content = "User not in game";
            }
        }

        private bool playerExists(string username)
        {
            foreach (Player pl in Monopoly.players)
            {
                if (pl.Username() == username)
                {
                    return true;
                }
            }
            return false;
        }

        private bool playerHasMoney(string username, int amount)
        {
            foreach (Player pl in Monopoly.players)
            {
                if (pl.Username() == username && pl.Balance > amount)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

        private async void OutOfJailButton_Click(object sender, RoutedEventArgs e)
        {
            OutOfJailButton.Visibility = Visibility.Hidden;
            OutOfJailText.Visibility = Visibility.Hidden;
            Monopoly.client.Send("reset_jail_count:" + Monopoly.username + ":");
            await Task.Delay(100);
            Monopoly.client.Send("decrease_jail_count:" + Monopoly.username + ":");
        }

        #region SelectRequestFigure methods

        private void SelectCar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            selectRemoveBorders();
            SelectCar.Background = Menu.HexToBrush(Menu.select_figure);
            SelectCar.BorderBrush = Brushes.Gray;
            SelectCar.BorderThickness = new Thickness(1);
            foreach (Player pl in Monopoly.players)
            {
                if (pl.Figure() == "car")
                {
                    RequestUsername.Content = "Username: " + pl.Username();
                    request_figure = pl.Figure();
                    request_username = pl.Username();
                }
            }
        }

        private void SelectDog_MouseDown(object sender, MouseButtonEventArgs e)
        {
            selectRemoveBorders();
            SelectDog.Background = Menu.HexToBrush(Menu.select_figure);
            SelectDog.BorderBrush = Brushes.Gray;
            SelectDog.BorderThickness = new Thickness(1);
            foreach (Player pl in Monopoly.players)
            {
                if (pl.Figure() == "dog")
                {
                    RequestUsername.Content = "Username: " + pl.Username();
                    request_figure = pl.Figure();
                    request_username = pl.Username();
                }
            }
        }

        private void SelectHat_MouseDown(object sender, MouseButtonEventArgs e)
        {
            selectRemoveBorders();
            SelectHat.Background = Menu.HexToBrush(Menu.select_figure);
            SelectHat.BorderBrush = Brushes.Gray;
            SelectHat.BorderThickness = new Thickness(1);
            foreach (Player pl in Monopoly.players)
            {
                if (pl.Figure() == "hat")
                {
                    RequestUsername.Content = "Username: " + pl.Username();
                    request_figure = pl.Figure();
                    request_username = pl.Username();
                }
            }
        }

        private void SelectShip_MouseDown(object sender, MouseButtonEventArgs e)
        {
            selectRemoveBorders();
            SelectShip.Background = Menu.HexToBrush(Menu.select_figure);
            SelectShip.BorderBrush = Brushes.Gray;
            SelectShip.BorderThickness = new Thickness(1);
            foreach (Player pl in Monopoly.players)
            {
                if (pl.Figure() == "ship")
                {
                    RequestUsername.Content = "Username: " + pl.Username();
                    request_figure = pl.Figure();
                    request_username = pl.Username();
                }
            }
        }

        private void SelectIron_MouseDown(object sender, MouseButtonEventArgs e)
        {
            selectRemoveBorders();
            SelectIron.Background = Menu.HexToBrush(Menu.select_figure);
            SelectIron.BorderBrush = Brushes.Gray;
            SelectIron.BorderThickness = new Thickness(1);
            foreach (Player pl in Monopoly.players)
            {
                if (pl.Figure() == "iron")
                {
                    RequestUsername.Content = "Username: " + pl.Username();
                    request_figure = pl.Figure();
                    request_username = pl.Username();
                }
            }
        }

        private void SelectShoe_MouseDown(object sender, MouseButtonEventArgs e)
        {
            selectRemoveBorders();
            SelectShoe.Background = Menu.HexToBrush(Menu.select_figure);
            SelectShoe.BorderBrush = Brushes.Gray;
            SelectShoe.BorderThickness = new Thickness(1);
            foreach (Player pl in Monopoly.players)
            {
                if (pl.Figure() == "shoe")
                {
                    RequestUsername.Content = "Username: " + pl.Username();
                    request_figure = pl.Figure();
                    request_username = pl.Username();
                }
            }
        }

        private void SelectThimble_MouseDown(object sender, MouseButtonEventArgs e)
        {
            selectRemoveBorders();
            SelectThimble.Background = Menu.HexToBrush(Menu.select_figure);
            SelectThimble.BorderBrush = Brushes.Gray;
            SelectThimble.BorderThickness = new Thickness(1);
            foreach (Player pl in Monopoly.players)
            {
                if (pl.Figure() == "thimble")
                {
                    RequestUsername.Content = "Username: " + pl.Username();
                    request_figure = pl.Figure();
                    request_username = pl.Username();
                }
            }
        }

        private void SelectCustom_MouseDown(object sender, MouseButtonEventArgs e)
        {
            selectRemoveBorders();
            SelectCustom.Background = Menu.HexToBrush(Menu.select_figure);
            SelectCustom.BorderBrush = Brushes.Gray;
            SelectCustom.BorderThickness = new Thickness(1);
            foreach (Player pl in Monopoly.players)
            {
                if (pl.Figure() == "custom")
                {
                    RequestUsername.Content = "Username: " + pl.Username();
                    request_figure = pl.Figure();
                    request_username = pl.Username();
                }
            }
        }

        private void SelectCar_MouseEnter(object sender, MouseEventArgs e)
        {
            if (request_figure != "car")
                SelectCar.Background = Menu.HexToBrush(Menu.hover_figure);
        }

        private void SelectCar_MouseLeave(object sender, MouseEventArgs e)
        {
            if (request_figure != "car")
                SelectCar.Background = Brushes.Transparent;
        }

        private void SelectDog_MouseEnter(object sender, MouseEventArgs e)
        {
            if (request_figure != "dog")
                SelectDog.Background = Menu.HexToBrush(Menu.hover_figure);
        }

        private void SelectDog_MouseLeave(object sender, MouseEventArgs e)
        {
            if (request_figure != "dog")
                SelectDog.Background = Brushes.Transparent;
        }

        private void SelectHat_MouseEnter(object sender, MouseEventArgs e)
        {
            if (request_figure != "hat")
                SelectHat.Background = Menu.HexToBrush(Menu.hover_figure);
        }

        private void SelectHat_MouseLeave(object sender, MouseEventArgs e)
        {
            if (request_figure != "hat")
                SelectHat.Background = Brushes.Transparent;
        }

        private void SelectShip_MouseEnter(object sender, MouseEventArgs e)
        {
            if (request_figure != "ship")
                SelectShip.Background = Menu.HexToBrush(Menu.hover_figure);
        }

        private void SelectShip_MouseLeave(object sender, MouseEventArgs e)
        {
            if (request_figure != "ship")
                SelectShip.Background = Brushes.Transparent;
        }

        private void SelectIron_MouseEnter(object sender, MouseEventArgs e)
        {
            if (request_figure != "iron")
                SelectIron.Background = Menu.HexToBrush(Menu.hover_figure);
        }

        private void SelectIron_MouseLeave(object sender, MouseEventArgs e)
        {
            if (request_figure != "iron")
                SelectIron.Background = Brushes.Transparent;
        }

        private void SelectShoe_MouseEnter(object sender, MouseEventArgs e)
        {
            if (request_figure != "shoe")
                SelectShoe.Background = Menu.HexToBrush(Menu.hover_figure);
        }

        private void SelectShoe_MouseLeave(object sender, MouseEventArgs e)
        {
            if (request_figure != "shoe")
                SelectShoe.Background = Brushes.Transparent;
        }

        private void SelectThimble_MouseEnter(object sender, MouseEventArgs e)
        {
            if (request_figure != "thimble")
                SelectThimble.Background = Menu.HexToBrush(Menu.hover_figure);
        }

        private void SelectThimble_MouseLeave(object sender, MouseEventArgs e)
        {
            if (request_figure != "thimble")
                SelectThimble.Background = Brushes.Transparent;
        }

        private void SelectCustom_MouseEnter(object sender, MouseEventArgs e)
        {
            if (request_figure != "custom")
                SelectThimble.Background = Menu.HexToBrush(Menu.hover_figure);
        }

        private void SelectCustom_MouseLeave(object sender, MouseEventArgs e)
        {
            if (request_figure != "custom")
                SelectThimble.Background = Brushes.Transparent;
        }

        private void selectRemoveBorders()
        {
            Thickness no = new Thickness(0);
            SelectCar.BorderThickness = no;
            SelectCar.Background = Brushes.Transparent;

            SelectDog.BorderThickness = no;
            SelectDog.Background = Brushes.Transparent;

            SelectHat.BorderThickness = no;
            SelectHat.Background = Brushes.Transparent;

            SelectShip.BorderThickness = no;
            SelectShip.Background = Brushes.Transparent;

            SelectIron.BorderThickness = no;
            SelectIron.Background = Brushes.Transparent;

            SelectShoe.BorderThickness = no;
            SelectShoe.Background = Brushes.Transparent;

            SelectThimble.BorderThickness = no;
            SelectThimble.Background = Brushes.Transparent;

            SelectCustom.BorderThickness = no;
            SelectCustom.Background = Brushes.Transparent;
        }

        #endregion

    }
}
