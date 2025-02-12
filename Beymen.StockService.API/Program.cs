using Beymen.StockService.API.Consumer;
using Beymen.StockService.Business;
using Beymen.StockService.Entity;
using Beymen.StockService.Service;
using Beymen.StockService.Service.Repository;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

var connectionString = builder.Configuration.GetConnectionString("StockDbConnection");
builder.Services.AddDbContext<StockDbContext>(options =>
    options.UseNpgsql(connectionString));

var rabbitMqConnectionString = builder.Configuration.GetConnectionString("RabbitMQ");
builder.Services.AddSingleton<IConnection>(sp =>
{
    var factory = new ConnectionFactory() { Uri = new Uri(rabbitMqConnectionString) };
    return factory.CreateConnection();
});

services.AddHostedService<StockConsumer>();

builder.Services.AddScoped<StockRepository>();

services.AddScoped<IStockBusiness, StockBusiness>();
services.AddScoped<IStockService, StockService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
