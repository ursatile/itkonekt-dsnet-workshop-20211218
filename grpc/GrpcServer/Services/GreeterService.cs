using Grpc.Core;
using GrpcDemo;

namespace GrpcServer.Services;

public class GreeterService : Greeter.GreeterBase {
    private readonly ILogger<GreeterService> _logger;
    public GreeterService(ILogger<GreeterService> logger) {
        _logger = logger;
    }

    static int messageNumber = 0;

    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context) {
        messageNumber++;
        return Task.FromResult(new HelloReply {
            Message = request.Friendliness switch {
                HelloRequest.Types.FriendlinessLevel.Friendly => $"Hello, {request.Name}! It is WONDERFUL to see you! Message #{messageNumber}",
                HelloRequest.Types.FriendlinessLevel.Neutral => $"Hello, {request.Name}. Message #{messageNumber}",
                HelloRequest.Types.FriendlinessLevel.Hostile => $"Oh no it's, {request.Name}. Message #{messageNumber}",
                _ => $"(unknown greeting), {request.Name} Message #{messageNumber}"
            }
        });
    }
}
