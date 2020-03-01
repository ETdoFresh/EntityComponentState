using CSharpNetworking;
using EntityComponentState;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using TransformStateLibrary;

namespace TCPStateServer
{
    class Server
    {
        private static TCPServer server;
        private static List<Socket> clients = new List<Socket>();

        static void Main(string[] args)
        {
            var stateHistory = new StateHistory<TransformState>();
            var port = 9999;
            server = new TCPServer("localhost", 9999);
            server.OnOpen += OnClientConnected;
            server.OnClose += OnClientDisconnected;
            server.OnMessage += OnReceiveMessage;
            server.Open();

            // Wait Forever
            new ManualResetEvent(false).WaitOne();
        }

        private static void OnClientConnected(object sender, Socket e)
        {
            clients.Add(e);
            Console.WriteLine($"TCPServer: {e.Handle} connected!");
        }

        private static void OnClientDisconnected(object sender, Socket e)
        {
            clients.Remove(e);
            Console.WriteLine($"TCPServer: {e.Handle} disconnected!");
        }

        private static void OnReceiveMessage(object sender, Message<Socket> e)
        {
            var state = new TransformState();
            state.FromBytes(new ByteQueue(e.bytes));
            Console.WriteLine($"TCPClient {e.client.Handle}: {state}");
        }
    }
}
