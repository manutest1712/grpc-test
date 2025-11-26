using GrpcServer.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(6000, listenOptions =>
    {
        listenOptions.Protocols =
            Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
    });
});

builder.Services.AddGrpcReflection();
var app = builder.Build();

app.MapGrpcReflectionService();
// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGrpcService<FileStreamerService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
