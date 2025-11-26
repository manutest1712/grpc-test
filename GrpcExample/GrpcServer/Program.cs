using GrpcServer.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();

// Configure Kestrel to listen on a specific port (e.g., 5001)
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(6000); // HTTP/2 over plain TCP (for testing)
    // For HTTPS with HTTP/2:
    // options.ListenAnyIP(5001, listenOptions =>
    // {
    //     listenOptions.UseHttps();
    //     listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
    // });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
