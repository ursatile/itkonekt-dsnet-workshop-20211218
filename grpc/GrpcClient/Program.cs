using GrpcDemo;
using Grpc.Net.Client;

using var channel = GrpcChannel.ForAddress("https://localhost:7037/");
var grpcClient = new Greeter.GreeterClient(channel);
Console.WriteLine("Ready! Press a key to send a request");

while (true) {
    Console.ReadKey(true);
    var request = new HelloRequest { Name = "ITkonekt" };
    var reply = grpcClient.SayHello(request);
    Console.WriteLine(reply.Message);
}