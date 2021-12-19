using System;
using System.IO;
using System.Threading.Tasks;
using Autobarn.Messages;
using EasyNetQ;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Autobarn.Notifier {
    class Program {
        private static readonly IConfigurationRoot config = ReadConfiguration();
        private const string SUBSCRIBER_ID = "Autobarn.Notifier";
        private static HubConnection hub;
        static async Task Main(string[] args) {
            JsonConvert.DefaultSettings = JsonSettings;
            hub = new HubConnectionBuilder().WithUrl(config["AutobarnSignalRHubUrl"]).Build();
            await hub.StartAsync();
            Console.WriteLine("SignalR Hub Started!");
            var amqp = config.GetConnectionString("rabbitmq");
            using var bus = RabbitHutch.CreateBus(amqp);
            await bus.PubSub.SubscribeAsync<NewVehiclePriceMessage>(SUBSCRIBER_ID, HandleNewVehiclePriceMessage);
            Console.WriteLine("Subscribed to message bus! Listening for NewVehicleMessages...");
            Console.ReadLine();
        }

        static async Task HandleNewVehiclePriceMessage(NewVehiclePriceMessage m) {
            var json = JsonConvert.SerializeObject(m);
            Console.WriteLine(json);
            await hub.SendAsync("NotifyAllConnectedWebUsers", SUBSCRIBER_ID, json);
            Console.WriteLine("Sent message to SignalR");
        }

        private static IConfigurationRoot ReadConfiguration() {
            var basePath = Directory.GetParent(AppContext.BaseDirectory).FullName;
            return new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
        }

        private static JsonSerializerSettings JsonSettings() =>
            new JsonSerializerSettings {
                ContractResolver = new DefaultContractResolver {
                    NamingStrategy = new CamelCaseNamingStrategy()
                }
            };

    }
}
