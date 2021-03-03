﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Sever
{
    class Server
    {
        public static int maxPlayers { get; private set; }
        public static int port { get; private set; }
        public static Dictionary<int, Client> clients = new Dictionary<int, Client>();
        public delegate void PacketHandler(int client, CustomPacket packet);
        public static Dictionary<int, PacketHandler> packetHandlers;
        private static TcpListener tcpListener;

        public static void Start(int _maxPlayers, int _port)
        {
            maxPlayers = _maxPlayers;
            port = _port;
            Console.WriteLine("Starting Server...");
            InitializeServerData();

            tcpListener = new TcpListener(IPAddress.Any, port);
            tcpListener.Start();
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null);
            Console.WriteLine($"Server started on {port}.");

            // Possibly make ManualResetEvent to manage threads.
            // ManualResetEvent - suspend execution of the main thread and signal when execution can continue.
        }

        private static void TCPConnectCallback(IAsyncResult _result)
        {
            TcpClient client = tcpListener.EndAcceptTcpClient(_result);
            // Since the main socket is now free, it can go back and wait for other clients.
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null);
            Console.WriteLine($"Incoming connection from {client.Client.RemoteEndPoint}...");
            for (int i = 1; i <= maxPlayers; ++i)
            {
                // if the slot is empty.
                if(clients[i].tcp.socket == null)
                {
                    // newly connected instance in to the dictionary.
                    clients[i].tcp.Connect(client);
                    return;
                }
            }
            Console.WriteLine($"{client.Client.RemoteEndPoint} failed to connect: the server is full!");
        }
        
        private static void InitializeServerData()
        {
            for(int i = 0;  i < maxPlayers; ++i)
            {
                clients.Add(i, new Client(i));
            }
            packetHandlers = new Dictionary<int, PacketHandler>() 
            {
                { (int)ClientPackets.CP_WELCOME_RECEIVED, ServerHandle.WelcomeReceived }
            };
            Console.WriteLine("Initialized Packets!");
        }

    }
}