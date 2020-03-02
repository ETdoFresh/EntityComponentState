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
            client.OnMessage += OnReceive;
            client.Open();

            string input;
            while ((input = Console.ReadLine()).ToLower() != "quit")
            {
                if (input.ToLower() == "state")
                {
                    var state = new TransformState();
                    state.tick = simulatedTick++;
                    client.Send(Packet.Create(CommandEnum.StateUpdate, state.ToBytes()));
                }
                if (input.ToLower() == "deltastate")
                {
                    var deltaState = new TransformDeltaState();
                    deltaState.startStateTick = simulatedTick - 1;
                    deltaState.endStateTick = simulatedTick++;
                    client.Send(Packet.Create(CommandEnum.DeltaStateUpdate, deltaState.ToBytes()));
                }
            }
            client.Close();
        }

        private static void OnReceive(object sender, Message e)
        {
            var bytes = new ByteQueue(e.bytes);
            var command = (CommandEnum)bytes.GetByte();
            if (command == CommandEnum.StateUpdate)
            {
                var state = new TransformState();
                state.FromBytes(bytes);
                Console.WriteLine($"TCPServer: {state}");
            }
        }
    }
}
