using EntryPointAPI.Kafka;
using EntryPointAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.IdentityModel.Tokens;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddSingleton<NotificationConfig>(sp =>
{
    var bootstrapServers = builder.Configuration["Kafka:BootstrapServers"];
    var topic = builder.Configuration["Kafka:Topic"];
    return new NotificationConfig(bootstrapServers, topic);
});

builder.Services.AddScoped<INotificationProducer, NotificationProducer>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var authority = builder.Configuration["Authentication:Jwt:Authority"];
        var clientId = builder.Configuration["Authentication:Jwt:ClientId"];

        options.Authority = authority;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = authority,

            ValidateAudience = true,
            ValidAudience = clientId,

            ValidateLifetime = true
        };
    });

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapPost("/notificate", [Authorize](INotificationProducer producer, [FromBody] NotificationData data) =>
{
    var kafkaItem = new KafkaItem(data.UserId, data.Channels);
    producer.Produce(kafkaItem);

    return Results.Accepted();
}).RequireAuthorization();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
