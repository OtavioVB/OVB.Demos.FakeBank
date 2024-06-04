using Grpc.Core;
using OVB.Demos.FakeBank.Account.WebApi;

namespace OVB.Demos.FakeBank.Account.WebApi.Services;

public class GreeterService : Greeter.GreeterBase
{
    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        return Task.FromResult(new HelloReply
        {
            Message = "Hello " + request.Name
        });
    }
}
