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
        private static StateHistory<TransformState> stateHistory = new StateHistory<TransformState>();

        static void Main(string[] args)
        {
            stateHistory.Add(new TransformState()); // Empty start state

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
            var bytes = new ByteQueue(e.bytes);
            var command = (CommandEnum)bytes.GetByte();
            if (command == CommandEnum.Input)
            {
                // Receive Input:
            }
            if (command == CommandEnum.StateUpdate)
            {
                var state = new TransformState();
                state.FromBytes(bytes);
                Console.WriteLine($"TCPClient {e.client.Handle}: {state}");

                var existingState = stateHistory.GetState(state.tick);
                if (existingState != null)
                {
                    existingState.Clear();
                    existingState.entities.AddRange(state.entities);
                }
                else
                {
                    stateHistory.Add(state);
                    foreach (var client in clients)
                        server.Send(client, Packet.Create(CommandEnum.StateUpdate, state.ToBytes()));
                }
            }
            if (command == CommandEnum.DeltaStateUpdate)
            {
                var deltaState = new TransformDeltaState();
                deltaState.FromBytes(bytes);
                Console.WriteLine($"TCPClient {e.client.Handle}: {deltaState}");
                var startState = stateHistory.GetState(deltaState.startStateTick);
                if (startState != null)
                {
                    var endState = (TransformState)deltaState.GenerateEndState(startState);
                    Console.WriteLine($"End State Generated: {endState}");
                    var existingState = stateHistory.GetState(endState.tick);
                    if (existingState != null)
                    {
                        existingState.Clear();
                        existingState.entities.AddRange(endState.entities);
                    }
                    else
                    {
                        stateHistory.Add(endState);
                        foreach (var client in clients)
                            server.Send(client, Packet.Create(CommandEnum.StateUpdate, endState.ToBytes()));
                    }
                }
            }
        }
    }
}
