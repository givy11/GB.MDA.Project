using Restaurant.Notification;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHostedService<Worker>();

var app = builder.Build();

app.Run();
