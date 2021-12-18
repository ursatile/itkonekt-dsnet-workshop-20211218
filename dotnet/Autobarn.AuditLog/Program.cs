using System;
using System.IO;
using System.Threading.Tasks;
using Autobarn.Messages;
using EasyNetQ;
using Microsoft.Extensions.Configuration;

namespace Autobarn.AuditLog {
    class Program {
        private static readonly IConfigurationRoot config = ReadConfiguration();
        private const string SUBSCRIBER_ID = "autobarn.auditlog";
        static async Task Main(string[] args) {
            var amqp = config.GetConnectionString("rabbitmq");
            using var bus = RabbitHutch.CreateBus(amqp);
            await bus.PubSub.SubscribeAsync<NewVehicleMessage>(SUBSCRIBER_ID, HandleNewVehicleMessage);
            Console.WriteLine("Subscribed to message bus! Listening for NewVehicleMessages...");
            Console.ReadLine();
        }

        static void HandleNewVehicleMessage(NewVehicleMessage m) {
            Console.WriteLine("Received new NewVehicleMessage:");
            var csv = $"{m.Registration},{m.Color},{m.Year},{m.ManufacturerName},{m.ModelName},{m.ListedAt:O}";
            Console.WriteLine(csv);
            File.AppendAllText("vehicles.log", csv + Environment.NewLine);
        }

        private static IConfigurationRoot ReadConfiguration() {
            var basePath = Directory.GetParent(AppContext.BaseDirectory).FullName;
            return new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
        }
    }
}
