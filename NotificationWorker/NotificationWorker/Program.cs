using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using NotificationWorker.Consumer;
using NotificationWorker.Database;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<NtfContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DbConnection")));

builder.Services.Configure<NotificationWorker.Consumer.ConsumerConfig>(opt =>
{
    opt.BootstrapServer = builder.Configuration["Kafka:BootstrapServer"];
    opt.GroupId = builder.Configuration["Kafka:GroupId"];
    opt.Topic = builder.Configuration["Kafka:Topic"];
    opt.AutoCommit = builder.Configuration.GetValue<bool>("Kafka:AutoCommit");
    opt.AutoOffsetReset = AutoOffsetReset.Earliest;

});

builder.Services.AddHostedService<ConsumeService>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
