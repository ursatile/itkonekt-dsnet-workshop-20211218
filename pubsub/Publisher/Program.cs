using System;
using EasyNetQ;
using Messages;

namespace Publisher {
    class Program {
        const string AMQP_URL = "amqps://fmvuwdnx:qGTa9koTU7TzYKxx-eRJquXMI3lAX6Rs@whale.rmq.cloudamqp.com/fmvuwdnx";
        static void Main(string[] args) {
            var bus = RabbitHutch.CreateBus(AMQP_URL);
            var i = 0;
            while (true) {
                Console.WriteLine("Press any key to send a message...");
                Console.ReadKey(false);
                var body = $"Message #{i++} from {Environment.MachineName} at {DateTime.Now}";
                var message = new DemoMessage {
                    Body = body
                };
                bus.PubSub.Publish(message);
                Console.WriteLine($"Published: {body}");
            }
        }
    }
}

