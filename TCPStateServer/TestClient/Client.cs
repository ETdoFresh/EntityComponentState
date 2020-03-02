using CSharpNetworking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EntityComponentState;
using TransformStateLibrary;

namespace TestClient
{
    class Client
    {
        private static int simulatedTick;

        static void Main(string[] args)
        {
            var host = "localhost";
            var port = 9999;
            Console.WriteLine($"This is an example TCP Client. Press any key to connect to tcp://{host}:{port}");
            var client = new TCPClient(host, port);
            client.Open();

            string input;
            while ((input = Console.ReadLine()).ToLower() != "quit")
            {
                if (input.ToLower() == "state")
                {
                    var state = new TransformState();
                    state.tick = simulatedTick++;
                    var packet = new List<byte>();
                    packet.Add((byte)CommandEnum.StateUpdate);
                    packet.AddRange(state.ToBytes());
                    client.Send(packet.ToArray());
                }
                if (input.ToLower() == "deltastate")
                {
                    var deltaState = new TransformDeltaState();
                    deltaState.startStateTick = simulatedTick - 1;
                    deltaState.endStateTick = simulatedTick++;
                    var packet = new List<byte>();
                    packet.Add((byte)CommandEnum.DeltaStateUpdate);
                    packet.AddRange(deltaState.ToBytes());
                    client.Send(packet.ToArray());
                }
            }
            client.Close();
        }
    }
}
