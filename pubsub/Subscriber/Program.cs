using System;
using Messages;
using EasyNetQ;

namespace Subscriber {
    class Program {
        const string AMQP_URL = "amqps://fmvuwdnx:qGTa9koTU7TzYKxx-eRJquXMI3lAX6Rs@whale.rmq.cloudamqp.com/fmvuwdnx";

        static void Main(string[] args) {
            using var bus = RabbitHutch.CreateBus(AMQP_URL);
            bus.PubSub.Subscribe<DemoMessage>("dylan-in-london", message => {
                if (message.Body.Contains("#4")) throw new InvalidOperationException("NO! 4 IS NOT ALLOWED");
                Console.WriteLine(message.Body);
            });
            Console.WriteLine("Subscriber connected! Listening for messages... press 'enter' to exit");
            Console.ReadLine();
            Console.WriteLine("Thank you! Bye!");
        }
    }
}
