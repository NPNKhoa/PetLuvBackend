using MassTransit;
using NotificationApi.Application.Consumers;
using NotificationApi.Application.Interfaces;
using NotificationApi.Infrestructure.Services;
using NotificationApi.Infrestructure.Settings;
using PetLuvSystem.SharedLibrary.DependencyInjections;
using PetLuvSystem.SharedLibrary.Middlewares;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCorsPolicy();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Debug()
    .WriteTo.Console()
    .CreateLogger();

builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));

builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();

    x.AddConsumer<SendBookingPaymentEmailConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq://localhost", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ReceiveEndpoint("notification-service", e =>
        {
            e.ConfigureConsumer<SendBookingPaymentEmailConsumer>(context);
        });
    });
});

var app = builder.Build();
app.UseCorsPolicy();
app.UseMiddleware<GlobalException>();
app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
