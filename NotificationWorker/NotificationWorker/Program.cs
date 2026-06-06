using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using NotificationWorker.Consumer;
using NotificationWorker.Database;
using NotificationWorker.Models;
using NotificationWorker.Repositories;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<NtfContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DbConnection")));
builder.Services.AddScoped<IRepository<NotificationInfo>, NotificationRepository>();
builder.Services.Configure<NotificationWorker.Consumer.ConsumerConfig>(opt =>
{
    opt.BootstrapServer = builder.Configuration["Kafka:BootstrapServer"];
    opt.GroupId = builder.Configuration["Kafka:GroupId"];
    opt.Topic = builder.Configuration["Kafka:Topic"];
    opt.AutoCommit = builder.Configuration.GetValue<bool>("Kafka:AutoCommit");
    opt.AutoOffsetReset = AutoOffsetReset.Earliest;

});
builder.Services.AddSingleton<IConnectionMultiplexer, ConnectionMultiplexer>(opt =>
{
    return ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("RedisConnection"));
});

builder.Services.AddHostedService<ConsumeService>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
