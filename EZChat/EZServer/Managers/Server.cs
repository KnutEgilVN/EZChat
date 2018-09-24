using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EZServer.Managers
{
    public class Server
    {
        Socket socket { get; set; }
        List<Client> clients { get; set; }

        public Server(int port)
        {
            clients = new List<Client>();
            socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(new IPEndPoint(IPAddress.Loopback, port));
            socket.Listen(10);

            socket.BeginAccept(BeginAcceptCallback, null);
        }

        public void SendMessage(string message)
        {
            clients.ForEach(c =>
            {
                c.socket.Send(Encoding.UTF8.GetBytes(message));
            });
        }

        public void BeginAcceptCallback(IAsyncResult ar)
        {
            Console.WriteLine("ACCEPTED");
            Socket _socket = socket.EndAccept(ar);
            Client client = new Client(_socket);

            clients.Add(client);
            client.socket.BeginReceive(client.buffer, 0, client.buffer.Length, SocketFlags.None, BeginReceiveCallback, client);

            socket.BeginAccept(BeginAcceptCallback, null);
        }
        public void BeginReceiveCallback(IAsyncResult ar)
        {
            try
            {
                Console.WriteLine("RECEIVED");
                Client client = (Client)ar.AsyncState;

                int available = client.socket.Available;
                int received = client.socket.EndReceive(ar);

                if (available > 0)
                {
                    Console.WriteLine("Not finished reading data!");
                    client.dataBuffer.AddRange(client.buffer);
                    client.buffer = new byte[1024];
                    client.socket.BeginReceive(client.buffer, 0, client.buffer.Length, SocketFlags.None, BeginReceiveCallback, client);
                }
                else
                {
                    client.dataBuffer.AddRange(client.buffer);
                    client.buffer = new byte[1024];
                    string message = Encoding.UTF8.GetString(client.dataBuffer.ToArray());
                    client.dataBuffer.Clear();

                    clients.ForEach(c =>
                    {
                        c.socket.Send(Encoding.UTF8.GetBytes(message));
                        Console.WriteLine("Sent data!");
                    });

                    client.socket.BeginReceive(client.buffer, 0, client.buffer.Length, SocketFlags.None, BeginReceiveCallback, client);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error occured.\nClient probably exited.");
                Client client = (Client)ar.AsyncState;
                clients.Remove(client);
            }
        }
    }

    public class Client
    {
        public Socket socket { get; set; }
        public byte[] buffer { get; set; }
        public List<byte> dataBuffer { get; set; }

        public Client(Socket _socket)
        {
            socket = _socket;
            buffer = new byte[1024];
            dataBuffer = new List<byte>();
        }
    }
}
