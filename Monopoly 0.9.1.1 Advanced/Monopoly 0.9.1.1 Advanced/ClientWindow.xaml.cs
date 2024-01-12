using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Monopoly_0._9._1._1_Advanced
{
    
    /// <summary>
    /// Interaction logic for ClientWindow.xaml
    /// </summary>
    public partial class ClientWindow : Window
    {
        static IPAddress serverIP; //IP na katerem je strežnik
        static int port; //Port na katerem deluje strežnik
        private byte[] data; //Uporabim pri pošiljanju podatkov na strežnik
        public static Socket client;
        public static string code;

        Monopoly monopoly;

        public ClientWindow(Monopoly m, string code)
        {
            ClientWindow.code = code;
            client = LoginRegister.client;
            monopoly = m;
            //Connect();
        }

        public void Output(string text)
        {
            Dispatcher.Invoke((Action)delegate
            {
                //clientOutput.AppendText("\n" + text);
                //set the current caret position to the end
                //serverOutput.SelectionStart = serverOutput.Text.Length;
                //scroll it automatically
                //serverOutput.ScrollToCaret();
            });
        }

        #region Commands from server

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
                    monopoly.moveCharacter(Convert.ToString(data1), Convert.ToInt32(data2));
                    if (show == true)
                        monopoly.Output(data1 + " has moved forward " + data2 + " positions.");
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
                        monopoly.addCharacter(username, figure, rank, custom_prefix, custom_color);
                }
                else if (data.Contains("remove_character:"))
                {
                    string[] data_split = data.Split(':');
                    var username = data_split[1].Replace("\0", string.Empty);
                    var figure = data_split[2].Replace("\0", string.Empty);
                    if (show == true)
                        monopoly.removeCharacter(username);
                }
                else if (data.Contains("player_turn:"))
                {
                    string[] data_split = data.Split(':');
                    var username = data_split[1].Replace("\0", string.Empty);
                    monopoly.yourTurn(username);
                    if (show == true)
                        monopoly.Output(username + " has turn!");
                }
                else if (data.Contains("dice_throw:"))
                {
                    monopoly.diceThrown(data);
                }
                else if (data.Contains("game_start:"))
                {
                    monopoly.gameStart();
                    if (show == true)
                        monopoly.Output("Game has started");
                }
                else if (data.Contains("game_has_already_started"))
                {
                    monopoly.gameFull();
                }
                else if (data.Contains("buy_property"))
                {
                    string[] data_split = data.Split(':');
                    var username = data_split[1].Replace("\0", string.Empty);
                    monopoly.buyProperty(username, Convert.ToInt32(data_split[2]), Convert.ToInt32(data_split[3]));
                }
                else if (data.Contains("sell_property"))
                {
                    string[] data_split = data.Split(':');
                    var username = data_split[1].Replace("\0", string.Empty);
                    monopoly.sellProperty(username, Convert.ToInt32(data_split[2]), Convert.ToInt32(data_split[3]));
                }
                else if (data.Contains("buy_house"))
                {
                    string[] data_split = data.Split(':');
                    var username = data_split[1].Replace("\0", string.Empty);
                    monopoly.buyHouse(username, Convert.ToInt32(data_split[2]), Convert.ToInt32(data_split[3]));
                }
                else if (data.Contains("sell_house"))
                {
                    string[] data_split = data.Split(':');
                    var username = data_split[1].Replace("\0", string.Empty);
                    monopoly.sellHouse(username, Convert.ToInt32(data_split[2]), Convert.ToInt32(data_split[3]));
                }
                else if (data.Contains("buy_hotel"))
                {
                    string[] data_split = data.Split(':');
                    var username = data_split[1].Replace("\0", string.Empty);
                    monopoly.buyHotel(username, Convert.ToInt32(data_split[2]), Convert.ToInt32(data_split[3]));
                }
                else if (data.Contains("sell_hotel"))
                {
                    string[] data_split = data.Split(':');
                    var username = data_split[1].Replace("\0", string.Empty);
                    monopoly.sellHotel(username, Convert.ToInt32(data_split[2]), Convert.ToInt32(data_split[3]));
                }
                else if (data.Contains("pay_rent"))
                {
                    string[] data_split = data.Split(':');
                    var payer = data_split[1].Replace("\0", string.Empty);
                    var reciever = data_split[2].Replace("\0", string.Empty);
                    monopoly.payRent(payer, reciever, Convert.ToInt32(data_split[3]));
                }
                else if (data.Contains("bankrupt"))
                {
                    string[] data_split = data.Split(':');
                    string username = data_split[1].Replace("\0", string.Empty);
                    monopoly.Bankrupt(username);
                    monopoly.removeCharacter(username);
                    if (show == true)
                        monopoly.Output(username + " has gone bankrupt.");
                }
                else if (data.Contains("go_jail"))
                {
                    string[] data_split = data.Split(':');
                    var username = data_split[1].Replace("\0", string.Empty);
                    monopoly.putInJail(username);
                    if (show == true)
                        monopoly.Output(username + " went to jail.");
                }
                else if (data.Contains("treasure_show"))
                {
                    string[] data_split = data.Split(':');
                    int index = Convert.ToInt32(data_split[1].Replace("\0", string.Empty));
                    monopoly.treasureShow(index);
                }
                else if (data.Contains("treasure_hide"))
                {
                    monopoly.treasureHide();
                }
                else if (data.Contains("recieve_money"))
                {
                    string[] data_split = data.Split(':');
                    var username = data_split[1].Replace("\0", string.Empty);
                    int amount = Convert.ToInt32(data_split[2].Replace("\0", string.Empty));
                    monopoly.recieveMoney(username, amount);
                    if (show == true)
                        monopoly.Output(username + " has recieved $" + amount + ".");
                }
                else if (data.Contains("take_money"))
                {
                    string[] data_split = data.Split(':');
                    var username = data_split[1].Replace("\0", string.Empty);
                    int amount = Convert.ToInt32(data_split[2].Replace("\0", string.Empty));
                    monopoly.takeMoney(username, amount);
                    if (show == true)
                        monopoly.Output("$" + amount + " has been removed from " + username + ".");
                }
                else if (data.Contains("collect_money_per_player"))
                {
                    string[] data_split = data.Split(':');
                    var username = data_split[1].Replace("\0", string.Empty);
                    int amount = Convert.ToInt32(data_split[2].Replace("\0", string.Empty));
                    monopoly.collectMoneyPerPlayer(username, amount);
                    if (show == true)
                        monopoly.Output(username + " has collected " + amount + " from each player.");
                }
                else if (data.Contains("pay_money_per_buildings"))
                {
                    string[] data_split = data.Split(':');
                    var username = data_split[1].Replace("\0", string.Empty);
                    int house_amount = Convert.ToInt32(data_split[2].Replace("\0", string.Empty));
                    int hotel_amount = Convert.ToInt32(data_split[3].Replace("\0", string.Empty));
                    monopoly.payMoneyPerBuildings(username, house_amount, hotel_amount);
                    if (show == true)
                        monopoly.Output(username + " has payed $" + house_amount + " for each houes and $" + hotel_amount + " for each hotel he owns.");
                }
                else if (data.Contains("get_out_of_jail_free:"))
                {
                    string[] data_split = data.Split(':');
                    var username = data_split[1].Replace("\0", string.Empty);
                    monopoly.getOutOfJailFree(username);
                }
                else if (data.Contains("reset_jail_count:"))
                {
                    string[] data_split = data.Split(':');
                    var username = data_split[1].Replace("\0", string.Empty);
                    monopoly.resetJailCount(username);
                }
                else if (data.Contains("decrease_jail_count:"))
                {
                    string[] data_split = data.Split(':');
                    var username = data_split[1].Replace("\0", string.Empty);
                    monopoly.decreaseJailCount(username);
                }
                else if (data.Contains("chance_show"))
                {
                    string[] data_split = data.Split(':');
                    int index = Convert.ToInt32(data_split[1].Replace("\0", string.Empty));
                    monopoly.chanceShow(index);
                }
                else if (data.Contains("chance_hide"))
                {
                    monopoly.chanceHide();
                }
                else if (data.Contains("move_to"))
                {
                    string[] data_split = data.Split(':');
                    var username = data_split[1].Replace("\0", string.Empty);
                    var position_name = data_split[2].Replace("\0", string.Empty);
                    monopoly.moveTo(username, position_name);
                    if (show == true)
                        monopoly.Output(username + " has moved to " + position_name.Substring(0, 1).ToUpper() + position_name.Substring(1).ToLower() + ".");
                }
                else if (data.Contains("nearest_railroad"))
                {
                    string[] data_split = data.Split(':');
                    var username = data_split[1].Replace("\0", string.Empty);
                    Monopoly.double_rent = 2;
                    monopoly.nearestRailroad(username);
                }
                else if (data.Contains("nearest_utility"))
                {
                    string[] data_split = data.Split(':');
                    var username = data_split[1].Replace("\0", string.Empty);
                    Monopoly.double_rent = 2;
                    monopoly.nearestUtility(username);
                }
                else if (data.Contains("move_back"))
                {
                    string[] data_split = data.Split(':');
                    var username = data_split[1].Replace("\0", string.Empty);
                    int amount = Convert.ToInt32(data_split[2].Replace("\0", string.Empty));
                    monopoly.moveBack(username, amount);
                    if (show == true)
                        monopoly.Output(username + " has moved backwards " + amount + " positions" + ".");
                }
                else if (data.Contains("pay_each_player"))
                {
                    string[] data_split = data.Split(':');
                    var username = data_split[1].Replace("\0", string.Empty);
                    int amount = Convert.ToInt32(data_split[2].Replace("\0", string.Empty));
                    monopoly.payEachPlayer(username, amount);
                    if (show == true)
                        monopoly.Output(username + " has payed each player " + amount + ".");
                }
                else if (data.Contains("close_server"))
                {
                    monopoly.closeServer();
                    if (show == true)
                        monopoly.Output("Server has been closed.");
                }
                else if (data.Contains("player_winner"))
                {
                    string[] data_split = data.Split(':');
                    var username = data_split[1].Replace("\0", string.Empty);
                    monopoly.playerWinner(username);
                    if (show == true)
                        monopoly.Output(username + " has won the game.");
                }
                else if (data.Contains("player_request:"))
                {
                    string[] data_split = data.Split(':');
                    var requester = data_split[1].Replace("\0", string.Empty);
                    var reciever = data_split[2].Replace("\0", string.Empty);
                    var index_name = data_split[3].Replace("\0", string.Empty);
                    var amount = data_split[4].Replace("\0", string.Empty);
                    monopoly.playerRequest(requester, reciever, index_name, Convert.ToInt32(amount));
                    if (show == true)
                        monopoly.Output(requester + " has sent request to buy " + index_name.Substring(0, 1).ToUpper() + index_name.Substring(1).ToLower() + " from " + reciever + " for $" + amount + ".");
                }
                else if (data.Contains("request_decline:"))
                {
                    string[] data_split = data.Split(':');
                    var requester = data_split[1].Replace("\0", string.Empty);
                    var reciever = data_split[2].Replace("\0", string.Empty);
                    var index_name = data_split[3].Replace("\0", string.Empty);
                    var amount = data_split[4].Replace("\0", string.Empty);
                    if (show == true)
                        monopoly.Output(reciever + " has declined request from " + requester + ".");
                    monopoly.EnableRequestButton();
                }
                else if (data.Contains("request_accept"))
                {
                    string[] data_split = data.Split(':');
                    var requester = data_split[1].Replace("\0", string.Empty);
                    var reciever = data_split[2].Replace("\0", string.Empty);
                    var index_name = data_split[3].Replace("\0", string.Empty);
                    var amount = data_split[4].Replace("\0", string.Empty);
                    monopoly.requestAccept(requester, reciever, index_name, Convert.ToInt32(amount));
                    if (show == true)
                        monopoly.Output(reciever + " has accepted request from " + requester + ".");
                    monopoly.EnableRequestButton();
                }
                else if(data.Contains("message:"))
                {
                    string[] data_split = data.Split(':');
                    string message = data_split[1].Replace("\0", string.Empty);
                    monopoly.Output(message);
                }
            });
        }


        #endregion


        public async void SendAddPlayer(string username, string figure, string rank, string custom_prefix, string custom_color)//Metoda, s katero pošlje strežniku ukaz čez 1 sekundo, ko jo se jo pokliče
        {
            await Task.Delay(1000);
            Send("add_player:" + username + ":" + figure + ":" + rank + ":" + custom_prefix + ":" + custom_color + ":");
            //Pošlje strežniku ukaz, ki ga on oddaja vsem povezanim odjemalcem
        }

        private void Connect()//Metoda, ki poveže odjemalca z strežnikom
        {
            try
            {
                IPEndPoint endPoint = new IPEndPoint(serverIP, port); //Nastavi EndPoint preko katerega se povežemo
                client.BeginConnect(endPoint, ConnectCallback, null); //Tukaj se poveže na strežnik
            }
            catch (SocketException ex)
            {
                Error(ex.Message, MethodBase.GetCurrentMethod().Name);
            }
            catch (ObjectDisposedException ex)
            {
                Error(ex.Message, MethodBase.GetCurrentMethod().Name);
            }
        }
        public void Send(string data)//Metoda, ki omogoči pošiljanje podatkov 
        {
            data = "game:" + code + ":" + data;
            try
            {
                data = LoginRegister.Encrypt(data);
                byte[] data_send = Encoding.ASCII.GetBytes(data);
                client.BeginSend(data_send, 0, data_send.Length, SocketFlags.None, SendCallback, null);
            }
            catch (SocketException ex)
            {
                Error(ex.Message, MethodBase.GetCurrentMethod().Name);
            }
            catch (ObjectDisposedException ex)
            {
                Error(ex.Message, MethodBase.GetCurrentMethod().Name);
            }
        }

        private void Error(string message, string method) //Prikaže error v MessageBoxu
        {
            //MessageBox.Show(message,  "Client > " + method, MessageBoxButton.OK, MessageBoxImage.Error);
            Output(message + "  Client > " + method);
        }

        private void ReceiveCallback(IAsyncResult AR) //Metoda s katero se prejme podatke od strežnika
        {
            try
            {
                int received = client.EndReceive(AR);

                if (received == 0)
                {
                    return;
                }


                string text = Encoding.ASCII.GetString(data);
                data = new byte[client.ReceiveBufferSize];
                
                Dispatcher.Invoke((Action)delegate
                {
                    Data(text);
                });
                
                //Še enkrat začne pridobivati podatke od strežnika 
                client.BeginReceive(data, 0, data.Length, SocketFlags.None, ReceiveCallback, null);
            }
            catch (SocketException ex)
            {

                Error(ex.Message, MethodBase.GetCurrentMethod().Name);
                Dispatcher.Invoke((Action)delegate
                {
                    monopoly.closeServer();
                });
            }
            catch (ObjectDisposedException ex)
            {
                Error(ex.Message, MethodBase.GetCurrentMethod().Name);
            }
        }

        private void ConnectCallback(IAsyncResult AR)//Ko se odjemalec povežen na server, je ta metoda zato, da začenja pridobivati podatke od strežnika
        {
            try
            {
                client.EndConnect(AR);
                data = new byte[client.ReceiveBufferSize];
                client.BeginReceive(data, 0, data.Length, SocketFlags.None, ReceiveCallback, null);
            }
            catch (SocketException ex)
            {
                Error(ex.Message, MethodBase.GetCurrentMethod().Name);
            }
            catch (ObjectDisposedException ex)
            {
                Error(ex.Message, MethodBase.GetCurrentMethod().Name);
            }
        }

        private void SendCallback(IAsyncResult AR) //Metoda s katero odjemalec neha pošiljati podatke
        {
            try
            {
                client.EndSend(AR);
            }
            catch (SocketException ex)
            {
                Error(ex.Message, MethodBase.GetCurrentMethod().Name);
            }
            catch (ObjectDisposedException ex)
            {
                Error(ex.Message, MethodBase.GetCurrentMethod().Name);
            }
        }
    }
}
