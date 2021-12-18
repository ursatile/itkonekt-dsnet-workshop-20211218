using System;
using Messages;
using EasyNetQ;

namespace Subscriber {
    class Program {
        const string AMQP_URL = "amqps://fmvuwdnx:qGTa9koTU7TzYKxx-eRJquXMI3lAX6Rs@whale.rmq.cloudamqp.com/fmvuwdnx";

        static void Main(string[] args) {
            using var bus = RabbitHutch.CreateBus(AMQP_URL);
            bus.PubSub.Subscribe<DemoMessage>("itkonekt-subscriber", message => {
                Console.WriteLine(message.Body);
            });
            Console.WriteLine("Subscriber connected! Listening for messages...");
            Console.ReadLine();
        }
    }
}
