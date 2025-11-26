using Grpc.Core;
using Grpc.Net.Client;
using TestGrpc.FileService;
using TestGrpc.proto;


// The port number must match the one used by the gRPC server.
// The default for the gRPC template is usually 5001 (https) or 5000 (http).
var channel = GrpcChannel.ForAddress("http://localhost:6000");
var client = new Greeter.GreeterClient(channel);

// --- 1. Calling the simple SayHello method ---
Console.WriteLine("--- Calling SayHello ---");
var helloRequest = new HelloRequest { Name = "C# Client" };
var helloReply = await client.SayHelloAsync(helloRequest);
Console.WriteLine($"Server response: {helloReply.Message}");
Console.WriteLine();


// --- 2. Calling the SendData method ---
Console.WriteLine("--- Calling SendData ---");
var dataRequest = new DataRequest
{
    Id = 12345,
    Payload = "This is the actual data payload sent from the C# client.",
    Tags = { "priority", "critical", "gRPC" } // Populate the repeated string field
};
var dataReply = await client.SendDataAsync(dataRequest);
Console.WriteLine($"Data Sent Status: {dataReply.Status}");
Console.WriteLine($"Server Acknowledged ID: {dataReply.ReceivedId}");

var fileClient = new FileStreamer.FileStreamerClient(channel);

using var call = fileClient.DownloadFile(new FileRequest { FileName = "11.zip" });

await foreach (var chunk in call.ResponseStream.ReadAllAsync())
{
    string output = Path.Combine("C:\\TestData\\Downloads", chunk.FileName);

    using var fs = new FileStream(output, FileMode.Append, FileAccess.Write);
    fs.Write(chunk.Data.ToByteArray());
}
