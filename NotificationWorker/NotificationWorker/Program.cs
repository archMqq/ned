
using Confluent.Kafka;
using NotificationWorker.Consumer;

var builder = WebApplication.CreateBuilder(args);
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
