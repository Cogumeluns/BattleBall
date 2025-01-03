var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR()
    .AddJsonProtocol(options =>
    {
        options.PayloadSerializerOptions.PropertyNamingPolicy = null; // Use PascalCase for properties
    });
var app = builder.Build();
app.MapHub<GameHub>("/GameHub");
app.Run();