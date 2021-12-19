using System.ComponentModel.Design;
using GrpcDemo;
using Grpc.Net.Client;

using var channel = GrpcChannel.ForAddress("https://localhost:7037/");
var grpcClient = new Greeter.GreeterClient(channel);
Console.WriteLine("Ready! Press a key to send a request");

while (true) {
    Console.WriteLine("Press a key to get a greeting (1 = friendly, 2 = neutral, 3 = hostile");
    //if (Int32.TryParse(Console.ReadKey().KeyChar.ToString(), out var level)) {
    var request = new HelloRequest {
        Name = "ITkonekt",
        Friendliness = HelloRequest.Types.FriendlinessLevel.Friendly
    };
    var reply = grpcClient.SayHello(request);
    Console.WriteLine(reply.Message);
    // } else {
    //     Console.WriteLine("Unrecognized key");
    // }
}