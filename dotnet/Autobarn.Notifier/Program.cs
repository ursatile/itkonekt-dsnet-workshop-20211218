using System;
using System.IO;
using System.Threading.Tasks;
using Autobarn.Messages;
using EasyNetQ;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Autobarn.Notifier {
    class Program {
        private static readonly IConfigurationRoot config = ReadConfiguration();
        private const string SUBSCRIBER_ID = "Autobarn.Notifier";

        static async Task Main(string[] args) {
            var amqp = config.GetConnectionString("rabbitmq");
            using var bus = RabbitHutch.CreateBus(amqp);
            await bus.PubSub.SubscribeAsync<NewVehiclePriceMessage>(SUBSCRIBER_ID, HandleNewVehiclePriceMessage);
            Console.WriteLine("Subscribed to message bus! Listening for NewVehicleMessages...");
            Console.ReadLine();
        }

        static async Task HandleNewVehiclePriceMessage(NewVehiclePriceMessage m) {
            Console.WriteLine(JsonConvert.SerializeObject(m));
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
