using System;
using System.IO;
using System.Threading.Tasks;
using Autobarn.Messages;
using Autobarn.PricingEngine;
using EasyNetQ;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;

namespace Autobarn.PricingClient {
    class Program {
        private static readonly IConfigurationRoot config = ReadConfiguration();
        private static IBus bus;
        private static Pricer.PricerClient grpcClient;

        static async Task Main(string[] args) {
            using var channel = GrpcChannel.ForAddress(config["AutobarnGrpcPricingServerUrl"]);
            grpcClient = new Pricer.PricerClient(channel);
            bus = RabbitHutch.CreateBus(config.GetConnectionString("rabbitmq"));
            var subscriberID = "Autobarn.PricingClient";
            await bus.PubSub.SubscribeAsync<NewVehicleMessage>(subscriberID, HandleNewVehicleMessage);
            Console.WriteLine("Listening for NewVehicleMessages. Press Enter to quit.");
            Console.ReadLine();
        }

        private static async Task HandleNewVehicleMessage(NewVehicleMessage m) {
            Console.WriteLine($"Received NewVehicleMessage: {m.ManufacturerName} {m.ModelName} ({m.Year}, {m.Color}");
            var priceRequest = new PriceRequest {
                ModelName = m.ModelName,
                ManufacturerName = m.ManufacturerName,
                Color = m.Color,
                Year = m.Year
            };
            var reply = await grpcClient.GetPriceAsync(priceRequest);
            Console.WriteLine($"Price is: {reply.Price} {reply.CurrencyCode}");
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
