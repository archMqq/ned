using EntryPointAPI.Kafka;
using EntryPointAPI.Models;
using EntryPointAPI.Validators;
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

builder.Services.AddTransient<IValidator<NotificationData>, NotificationDataValidator>();

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

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapPost("/notificate", (INotificationProducer producer,
    IValidator<NotificationData> validator,
    [FromBody] NotificationData data) =>
{
    var res = validator.Validate(data);
    if (!res.IsValid)
    {
        return Results.BadRequest(res.Errors);
    }

    var kafkaItem = new KafkaItem(data.UserId, data.Channels);
    producer.Produce(kafkaItem);

    return Results.Accepted();
}).RequireAuthorization();

app.MapControllers();

app.Run();
