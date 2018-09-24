using EZServer.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

namespace EZServer.Windows
{
    /// <summary>
    /// Interaction logic for ChatWindow.xaml
    /// </summary>
    public partial class ChatWindow : Window
    {
        public string Username { get; set; }
        public Client Client { get; set; }

        public ChatWindow(string username)
        {
            InitializeComponent();

            Username = username;
            Client = new Client(new IPEndPoint(IPAddress.Loopback, 666));
            Client.MessageReceived += ReceivedMessage;
        }

        public void SendMessage(string message)
        {
            txtMessage.Clear();
            Client.Send($"{Username}🌌{message}");
        }
        public void ReceivedMessage(string _message)
        {
            int splitIndex = _message.IndexOf("🌌");
            string user = _message.Substring(0, splitIndex);
            string message = _message.Replace($"{user}🌌", "");
            Console.WriteLine(user + ": "+message);
        }

        private void txtMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                SendMessage(txtMessage.Text);
            }
        }
        private void btnSendMessage_Click(object sender, RoutedEventArgs e)
        {
            SendMessage(txtMessage.Text);
        }
    }
}
