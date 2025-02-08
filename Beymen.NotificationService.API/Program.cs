using Beymen.NotificationService.API;
using Beymen.NotificationService.API.Consumer;
using Beymen.NotificationService.Business;
using Beymen.NotificationService.Service;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);


var services = builder.Services;

var rabbitMqConnectionString = builder.Configuration.GetValue<string>("RabbitMQ:ConnectionString");
builder.Services.AddSingleton<IConnection>(sp =>
{
    var factory = new ConnectionFactory() { Uri = new Uri(rabbitMqConnectionString) };
    return factory.CreateConnection();
});


builder.Services.AddHostedService<NotificationConsumer>();


services.AddScoped<INotificationBusiness, NotificationBusiness>();
services.AddScoped<INotificationService, NotificationService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
