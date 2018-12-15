using MagicOnion.Server;
using System;
using Grpc.Core;

namespace MagicOnionTestServer
{
    class Program
    {
        static void Main(string[] args)
        {
            GrpcEnvironment.SetLogger(new Grpc.Core.Logging.ConsoleLogger());

            var service = MagicOnionEngine.BuildServerServiceDefinition(true);
            var server = new Grpc.Core.Server()
            {
                Services = { service },
                Ports = { new ServerPort("localhost", 9000, ServerCredentials.Insecure) }
            };

            server.Start();

            Console.WriteLine("ServerStarted");
            Console.ReadLine();
        }
    }
}
