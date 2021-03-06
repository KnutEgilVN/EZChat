﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Windows;

namespace EZServer.Managers
{
    public class Client
    {
        byte[] buffer { get; set; }
        List<byte> dataBuffer { get; set; }
        Socket socket { get; set; }

        public delegate void MessageReceivedCallback(string message);
        public event MessageReceivedCallback MessageReceived;

        public Client(IPEndPoint ep)
        {
            buffer = new byte[1024];
            dataBuffer = new List<byte>();

            socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(ep);
            socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, BeginReceiveCallback, null);
        }

        public void Send(string message)
        {
            try
            {
                socket.Send(Encoding.UTF8.GetBytes(message));
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occured,\nplease restart the application.");
            }
        }
        public void BeginReceiveCallback(IAsyncResult ar)
        {
            try
            {
                int available = socket.Available;
                int received = socket.EndReceive(ar);

                if(available > 0)
                {
                    Console.WriteLine("Not finished reading data!");
                    dataBuffer.AddRange(buffer);
                    buffer = new byte[1024];
                    socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, BeginReceiveCallback, null);
                }
                else
                {
                    dataBuffer.AddRange(buffer);
                    buffer = new byte[1024];
                    dataBuffer.RemoveAll(b => b == 0);
                    string message = Encoding.UTF8.GetString(dataBuffer.ToArray());
                    dataBuffer.Clear();

                    MessageReceived(message);
                    socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, BeginReceiveCallback, null);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("An error occured,\nplease restart the application.");
            }
        }
    }
}
