using Beymen.OrderService.API.Processor;
using Beymen.OrderService.Business;
using Beymen.OrderService.Business.Publisher;
using Beymen.OrderService.Entity;
using Beymen.OrderService.Service.Order;
using Beymen.OrderService.Service.OutboxMessage;
using Beymen.OrderService.Service.Repository;
using Beymen.Service.Message;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("OrderDbConnection");

builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseNpgsql(connectionString));


var rabbitMqConnectionString = builder.Configuration.GetConnectionString("RabbitMQ");

builder.Services.AddSingleton<IConnection>(sp =>
{
    var factory = new ConnectionFactory() { Uri = new Uri(rabbitMqConnectionString) };
    return factory.CreateConnection();
});


builder.Services.AddSingleton<IMessageQueuePublisher, OrderPublisher>();

builder.Services.AddHostedService<OutboxProcessor>();

builder.Services.AddScoped<IOutboxMessageService, OutboxMessageService>();
builder.Services.AddScoped<IOrderBusiness, OrderBusiness>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<OutboxMessageRepository, OutboxMessageRepository>();
builder.Services.AddScoped<OrderRepository, OrderRepository>();

builder.Services.AddLogging();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddDbContext<OrderDbContext>(options =>
//    options.UseNpgsql(builder.Configuration.GetConnectionString("OrderDbConnection")));


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
