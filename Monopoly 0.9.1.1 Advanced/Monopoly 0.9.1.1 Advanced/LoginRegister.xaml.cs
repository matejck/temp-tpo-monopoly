using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Cryptography;
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
using BC = BCrypt.Net.BCrypt;

namespace Monopoly_0._9._1._1_Advanced
{
    /// <summary>
    /// Interaction logic for LoginRegister.xaml
    /// </summary>

    public partial class LoginRegister : Window
    {
        public static Socket client;
        public static IPAddress IP;
        public static int PORT = 6001;
        private static string thisform;
        private static byte[] data;
        private static string server_version = "0xZbriR1Ifuv3eouLa0.9.1.1";
        Menu menu;

        public static bool kicked = false;

        public Brush head_color;
        public Brush back_color;

        public LoginRegister()
        {
            InitializeComponent();
            defaultFormVariables();
            head_color = Head.Background;
            back_color = MainGrid.Background;
            Connect();
        }

        public void defaultFormVariables()
        {
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IP = IPAddress.Parse("");

            thisform = "Login";
            Username.MaxLength = 12;
            //pass_font = new FontFamily(new Uri("pack://application:,,,/Password.ttf"), "Password");
            ConfirmPasswordBox.Visibility = Visibility.Hidden;
            PasswordBox.Visibility = Visibility.Hidden;
            ConfirmPasswordBox.Foreground = Brushes.White;
            PasswordBox.Foreground = Brushes.White;

            Username.Foreground = Brushes.SeaGreen;
            Password.Foreground = Brushes.SeaGreen;
            ConfirmPassword.Foreground = Brushes.SeaGreen;
            ConfirmPassword.Visibility = Visibility.Hidden;
            Password.FontFamily = new FontFamily("Segoe UI");
            ConfirmPassword.FontFamily = new FontFamily("Segoe UI");
            Error.Text = "";
        }

        private void ShowError(string message, string method) //Prikaže error v MessageBoxu
        {
            MessageBox.Show(message, thisform + " > " + method, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void loginSuccessful()
        {
            Send("request_user_data_get_menu:" + user.Username);
        }

        #region Exit+Header
        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Head_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        #endregion

        #region Textbox Methods
        private void UsernameFocus(object sender, RoutedEventArgs e)
        {
            if (Username.Text.ToLower().Trim().Equals("username"))
            {
                Username.Text = "";
                Username.Foreground = Brushes.White;
            }
        }

        private void UsernameFocusLost(object sender, RoutedEventArgs e)
        {
            if (Username.Text.ToLower().Trim().Equals("username") || Username.Text.Trim().Equals(""))
            {
                Username.Text = "Username";
                Username.Foreground = Brushes.SeaGreen;
            }
        }

        private void PasswordFocus(object sender, RoutedEventArgs e)
        {
            PasswordBox.Visibility = Visibility.Visible;
            Password.Visibility = Visibility.Hidden;
            PasswordBox.Focus();
            /*if (Password.Text.ToLower().Trim().Equals("password"))
            {
                Password.Text = "";
                Password.FontFamily = pass_font;
                Password.Foreground = Brushes.White;
            }*/
        }

        private void ConfirmPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            ConfirmPasswordBox.Visibility = Visibility.Visible;
            ConfirmPassword.Visibility = Visibility.Hidden;
            ConfirmPasswordBox.Focus();
            /*if (ConfirmPassword.Text.ToLower().Trim().Equals("confirm password"))
            {
                ConfirmPassword.Text = "";
                ConfirmPassword.FontFamily = pass_font;
                ConfirmPassword.Foreground = Brushes.White;
            }*/
        }


        private void Username_KeyDown(object sender, KeyEventArgs e)
        {
            if (Username.Text.Length < 12)
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
                        if (Username.Text.Length > 0)
                            e.Handled = false; break;
                    case Key.D1:
                        if (Username.Text.Length > 0)
                            e.Handled = false; break;
                    case Key.D2:
                        if (Username.Text.Length > 0)
                            e.Handled = false; break;
                    case Key.D3:
                        if (Username.Text.Length > 0)
                            e.Handled = false; break;
                    case Key.D4:
                        if (Username.Text.Length > 0)
                            e.Handled = false; break;
                    case Key.D5:
                        if (Username.Text.Length > 0)
                            e.Handled = false; break;
                    case Key.D6:
                        if (Username.Text.Length > 0)
                            e.Handled = false; break;
                    case Key.D7:
                        if (Username.Text.Length > 0)
                            e.Handled = false; break;
                    case Key.D8:
                        if (Username.Text.Length > 0)
                            e.Handled = false; break;
                    case Key.D9:
                        if (Username.Text.Length > 0)
                            e.Handled = false; break;
                    case Key.NumPad0:
                        if (Username.Text.Length > 0)
                            e.Handled = false; break;
                    case Key.NumPad1:
                        if (Username.Text.Length > 0)
                            e.Handled = false; break;
                    case Key.NumPad2:
                        if (Username.Text.Length > 0)
                            e.Handled = false; break;
                    case Key.NumPad3:
                        if (Username.Text.Length > 0)
                            e.Handled = false; break;
                    case Key.NumPad4:
                        if (Username.Text.Length > 0)
                            e.Handled = false; break;
                    case Key.NumPad5:
                        if (Username.Text.Length > 0)
                            e.Handled = false; break;
                    case Key.NumPad6:
                        if (Username.Text.Length > 0)
                            e.Handled = false; break;
                    case Key.NumPad7:
                        if (Username.Text.Length > 0)
                            e.Handled = false; break;
                    case Key.NumPad8:
                        if (Username.Text.Length > 0)
                            e.Handled = false; break;
                    case Key.NumPad9:
                        if (Username.Text.Length > 0)
                            e.Handled = false; break;
                    default:
                        e.Handled = true;
                        break;
                }
            }
            Username.Text = Username.Text.Replace(" ", String.Empty);
            Username.CaretIndex = Username.Text.Length; //Cursor v textboxu premakne na konec.
        }

        private void Username_KeyUp(object sender, KeyEventArgs e)
        {
            Username.Text = Username.Text.Replace(" ", String.Empty);
            Username.CaretIndex = Username.Text.Length; //Cursor v textboxu premakne na konec.
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

        private void ConfirmPasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ConfirmPasswordBox.Password.Trim().Equals(""))
            {
                ConfirmPassword.Visibility = Visibility.Visible;
                ConfirmPasswordBox.Visibility = Visibility.Hidden;
                Head.Focus();
            }
        }
        #endregion

        #region Login and Register Buttons

        public static string HashPassword(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                // Convert the input string to a byte array
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);

                // Compute the hash value of the input bytes
                byte[] hashBytes = sha256.ComputeHash(inputBytes);

                // Convert the hash bytes to a hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }

                return sb.ToString().Replace(":", "_");
            }
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            string username = Username.Text.Replace(" ", String.Empty);
            if(ConfirmPasswordBox.Password.Trim().Equals("") && ConfirmPassword.Visibility == Visibility.Hidden)
            {
                ConfirmPassword.Visibility = Visibility.Visible;
            }
            else 
            {
                if (PasswordBox.Password == ConfirmPasswordBox.Password)
                {
                    if (PasswordBox.Password.Length > 5)
                    {
                        if (username.Length > 3)
                        {
                            Error.Foreground = Brushes.Blue;
                            Error.Text = "Connecting . . .";
                            //string password = Encrypt(Encrypt(Encrypt(PasswordBox.Password)));
                            string password = HashPassword(PasswordBox.Password);
                            Send("register_user:" + username + ":" + password + ":" + server_version + ":");
                        }
                        else
                        {
                            Error.Foreground = Brushes.Red;
                            Error.Text = "Your username is too short";
                        }
                    }
                    else
                    {
                        Error.Foreground = Brushes.Red;
                        Error.Text = "Your password is too short";
                    }
                }
                else
                {
                    Error.Foreground = Brushes.Red;
                    Error.Text = "Confirm password must match password";
                }
            }
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            Error.Foreground = Brushes.Blue;
            Error.Text = "Connecting . . .";
            //string password = Encrypt(Encrypt(Encrypt(PasswordBox.Password)));
            string password = HashPassword(PasswordBox.Password);
            Send("login_user:" + Username.Text + ":" + password + ":"+ server_version + ":");
        }

        #endregion

        #region Server
        public void Send(string data)//Metoda, ki omogoči pošiljanje podatkov 
        {
            try
            {
                string encrypt_data = Encrypt(data);
                byte[] data_send = Encoding.ASCII.GetBytes(encrypt_data);
                client.BeginSend(data_send, 0, data_send.Length, SocketFlags.None, SendCallback, null);
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

        private void Connect()//Metoda, ki poveže odjemalca z strežnikom
        {
            try
            {
                IPEndPoint endPoint = new IPEndPoint(IP, PORT); //Nastavi EndPoint preko katerega se povežemo
                client.BeginConnect(endPoint, ConnectCallback, null); //Tukaj se poveže na strežnik
            }
            catch (SocketException ex)
            {
                //ShowError(ex.Message, MethodBase.GetCurrentMethod().Name);
                Dispatcher.Invoke((Action)delegate
                {
                    Error.Foreground = Brushes.Red;
                    Error.Text = "Couldn't connect to server";
                });
            }
            catch (ObjectDisposedException ex)
            {
                ShowError(ex.Message, MethodBase.GetCurrentMethod().Name);
            }   
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
                Data(text);

                //Še enkrat začne pridobivati podatke od strežnika 
                client.BeginReceive(data, 0, data.Length, SocketFlags.None, ReceiveCallback, null);
            }
            catch (SocketException ex)
            {
                ShowError(ex.Message, MethodBase.GetCurrentMethod().Name);
            }
            catch (ObjectDisposedException ex)
            {
                ShowError(ex.Message, MethodBase.GetCurrentMethod().Name);
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
                //ShowError(ex.Message, MethodBase.GetCurrentMethod().Name);
                Dispatcher.Invoke((Action)delegate
                {
                    Error.Foreground = Brushes.Red;
                    Error.Text = "There is no connection to server";
                });
            }
            catch (ObjectDisposedException ex)
            {
                //ShowError(ex.Message, MethodBase.GetCurrentMethod().Name);
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
                ShowError(ex.Message, MethodBase.GetCurrentMethod().Name);
            }
            catch (ObjectDisposedException ex)
            {
                ShowError(ex.Message, MethodBase.GetCurrentMethod().Name);
            }
        }
        #endregion

        #region Server Recieve Commands

        public User user;

        public void Data(string data)//Metoda, ki preveri kaj je ukaz in nato to izvede
        {
            Dispatcher.Invoke((Action)delegate
            {   
                if(data.Contains("game:"))
                {
                    string[] arr = data.Split(':');
                    data = string.Join(":", arr.Skip(1));

                    Menu.monopoly.Data(data);
                }
                else if (data.Contains("declined_to_join:"))
                {
                    menu.declinedToJoin(data);
                }
                else if (data.Contains("accepted_to_join:"))
                {
                    menu.acceptedToJoin(data);
                }
                else if(data.Contains("game_already_hosted_error"))
                {
                    menu.HostError.Text = "Game with this code is already hosted";
                }
                else if(data.Contains("successfully_hosted_game"))
                {
                    menu.HostGame();
                }
                else if (data.Contains("register_true"))
                {
                    Error.Foreground = Brushes.Green;
                    Error.Text = "Registered successfuly";
                }
                else if (data.Contains("register_exists"))
                {
                    Error.Foreground = Brushes.Red;
                    Error.Text = "Username already exists";
                }
                else if (data.Contains("register_error"))
                {
                    Error.Foreground = Brushes.Red;
                    Error.Text = "There was an error registering you";
                }
                else if (data.Contains("login_true"))
                {
                    Error.Foreground = Brushes.Green;
                    Error.Text = "Successfuly loged in";
                    user = new User(Username.Text);
                    loginSuccessful();
                }
                else if (data.Contains("login_username_already_connected"))
                {
                    Error.Foreground = Brushes.Red;
                    Error.Text = "Username already connected";
                }
                else if (data.Contains("login_false"))
                {
                    Error.Foreground = Brushes.Red;
                    Error.Text = "Login information was wrong";
                }
                else if (data.Contains("wrong_version"))
                {
                    Error.Foreground = Brushes.Red;
                    Error.Text = "This version is outdated";
                }
                else if (data.Contains("update_data:"))
                {
                    menu.updateUser(data);
                }
                else if (data.Contains("update_user_get_menu"))
                {
                    menu = new Menu(this);
                    this.Hide();
                    menu.Show();
                    menu.updateUser(data);
                }
                else if (data.Contains("change_password_successful"))
                {
                    menu.ChangePasswordError.Foreground = Brushes.Green;
                    menu.ChangePasswordError.Text = "Your password changed successfuly";
                }
                else if (data.Contains("change_password_wrong"))
                {
                    menu.ChangePasswordError.Foreground = Brushes.Red;
                    menu.ChangePasswordError.Text = "Your old password is wrong";
                }
                else if (data.Contains("change_username_error"))
                {
                    menu.ChangeUsernameError.Foreground = Brushes.Red;
                    menu.ChangeUsernameError.Text = "There was an error changing your username";
                }
                else if (data.Contains("change_username_successful"))
                {
                    menu.ChangeUsernameError.Foreground = Brushes.Green;
                    menu.ChangeUsernameError.Text = "Successfuly changed your username";


                    string[] user_data = data.Split(':');
                    menu.user.Username = user_data[8];
                    menu.updateUser(data);
                }
                else if (data.Contains("change_username_exists"))
                {
                    menu.ChangeUsernameError.Foreground = Brushes.Red;
                    menu.ChangeUsernameError.Text = "Username already exists";
                }
                else if (data.Contains("change_username_wrong"))
                {
                    menu.ChangeUsernameError.Foreground = Brushes.Red;
                    menu.ChangeUsernameError.Text = "Wrong password";
                }
                else if (data.Contains("change_username_already"))
                {
                    menu.ChangeUsernameError.Foreground = Brushes.Red;
                    menu.ChangeUsernameError.Text = "You already changed username once";
                }
                else if (data.Contains("leaderboard_data:"))
                {
                    menu.updateLeaderboard(data);
                }
                else if (data.Contains("console_message:"))
                {
                    if (data.Contains("send_info:"))
                    {
                        string[] send_info = data.Split(':');
                        menu.ConsoleOutputNoNewLine("\nInfo for " + send_info[2] + ":");
                        menu.ConsoleOutputNoNewLine("    Score: " + send_info[3]);
                        menu.ConsoleOutputNoNewLine("    Rank: " + send_info[4]);
                        menu.ConsoleOutputNoNewLine("    Prefix: " + send_info[5]);
                        menu.ConsoleOutputNoNewLine("    Prefix color: " + send_info[6]);
                        menu.ConsoleOutputNoNewLine("    Change username: " + send_info[7]);
                        menu.ConsoleOutputNoNewLine("    Show console: " + send_info[8] + "\n");
                        menu.Console.Update();

                    }
                    else
                    {
                        string[] split_message = data.Split(':');
                        menu.ConsoleOutput(split_message[1]);
                        menu.Console.Update();
                    }
                }
                else if (data.Contains("you_kicked:"))
                {
                    if (menu != null)
                    {
                        kicked = true;
                        menu.Close();
                        this.Show();
                        this.WindowState = WindowState.Normal;
                        Error.Foreground = Brushes.Red;
                        Error.Text = "You have been kicked";
                    }
                }
                else if (data.Contains("you_banned:"))
                {
                    Error.Foreground = Brushes.Red;
                    Error.Text = "You are banned";
                    if (menu != null)
                    {
                        kicked = true;
                        WindowState = WindowState.Normal;
                        menu.Close();
                        this.Show();
                    }
                }
                else if (data.Contains("join_match_code_not_found"))
                {
                    menu.JoinError.Text = "Code not found";
                    menu.JoinError.Background = Brushes.Red;
                }
                else if (data.Contains("join_match_ip"))
                {
                    string[] ip = data.Split(':');
                    menu.joinMatch(ip[1]);
                }
                else if(data.Contains("game_not_found"))
                {
                    menu.JoinError.Text = "Game not found";
                }
            });
        }

        #endregion

        #region Settings methods

        public void executeSettings(string file_name)
        {
            
        }

        #endregion

        #region Encrypt

        public static string Encrypt(string text) 
        {
            string encrypted = "";
            for (int i = 0; i < text.Length; i++)
            {
                switch (Convert.ToString(text[i]))
                {
                    case "q":
                        encrypted += "a";
                        break;
                    case "w":
                        encrypted += "b";
                        break;
                    case "e":
                        encrypted += "c";
                        break;
                    case "r":
                        encrypted += "d";
                        break;
                    case "t":
                        encrypted += "e";
                        break;
                    case "z":
                        encrypted += "f";
                        break;
                    case "u":
                        encrypted += "g";
                        break;
                    case "i":
                        encrypted += "h";
                        break;
                    case "o":
                        encrypted += "i";
                        break;
                    case "p":
                        encrypted += "j";
                        break;
                    case "a":
                        encrypted += "k";
                        break;
                    case "s":
                        encrypted += "l";
                        break;
                    case "d":
                        encrypted += "m";
                        break;
                    case "f":
                        encrypted += "n";
                        break;
                    case "g":
                        encrypted += "o";
                        break;
                    case "h":
                        encrypted += "p";
                        break;
                    case "j":
                        encrypted += "r";
                        break;
                    case "k":
                        encrypted += "s";
                        break;
                    case "l":
                        encrypted += "t";
                        break;
                    case "y":
                        encrypted += "u";
                        break;
                    case "x":
                        encrypted += "v";
                        break;
                    case "c":
                        encrypted += "z";
                        break;
                    case "v":
                        encrypted += "x";
                        break;
                    case "b":
                        encrypted += "y";
                        break;
                    case "n":
                        encrypted += "q";
                        break;
                    case "m":
                        encrypted += "w";
                        break;
                    case "_":
                        encrypted += ",";
                        break;
                    case ":":
                        encrypted += "?";
                        break;
                    case ";":
                        encrypted += "=";
                        break;
                    default:
                        encrypted += text[i];
                        break;
                }
            }
            return encrypted;
        }

        #endregion
    }
}
