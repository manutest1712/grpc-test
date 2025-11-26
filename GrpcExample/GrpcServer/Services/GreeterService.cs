using Grpc.Core;
using TestGrpc.proto;

namespace GrpcServer.Services
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = "Hello I am manu " + request.Name
            });
        }

        public override Task<DataReply> SendData(DataRequest request, ServerCallContext context)
        {
            // Read values from incoming request
     
            // Example processing
            Console.WriteLine($"Received data: ID = {request.Id}, Payload = {request.Payload}");

            // Build response
            var reply = new DataReply
            {
                Status = "success",
                ReceivedId = request.Id
            };

            return Task.FromResult(reply);
        }

    }
}
