using CSharpNetworking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestClient
{
    class Client
    {
        static void Main(string[] args)
        {
            var host = "localhost";
            var port = 9999;
            Console.WriteLine($"This is an example TCP Client. Press any key to connect to tcp://{host}:{port}");
            var client = new TCPClient(host, port);
            client.Open();

            string input;
            while((input = Console.ReadLine()).ToLower() != "quit")
            {
                client.Send(input);
            }
            client.Close();
        }
    }
}
