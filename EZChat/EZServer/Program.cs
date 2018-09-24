using EZServer.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EZServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server(666);
            Console.ReadLine();

            server.SendMessage("Heiaa");
            //Socket socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            
            //socket.Bind(new IPEndPoint(IPAddress.Loopback, 666));
            //socket.Listen(10);
            //socket.BeginAccept((ar) =>
            //{
            //    Socket client = socket.EndAccept(ar);
            //    string message = "";
            //    for (int i = 0; i < 1024; i++)
            //    {
            //        message += i % 2;
            //    }

            //    client.Send(Encoding.UTF8.GetBytes(message));

            //    Console.WriteLine("Sent message");
            //}, null);
            
            Console.ReadLine();
        }
    }
}
