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

var rabbitMqConnectionString = builder.Configuration.GetValue<string>("RabbitMQ:ConnectionString");
builder.Services.AddSingleton<IConnection>(sp =>
{
    var factory = new ConnectionFactory() { Uri = new Uri(rabbitMqConnectionString) };
    return factory.CreateConnection();
});

services.AddHostedService<StockConsumer>();

services.AddScoped<IStockBusiness, StockBusiness>();
services.AddScoped<IStockService, StockService>();
services.AddScoped<StockRepository>();


builder.Services.AddDbContext<StockDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("StockDbConnection")));

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
