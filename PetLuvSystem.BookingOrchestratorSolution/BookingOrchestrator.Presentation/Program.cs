using BooingOrchestrator.Infrastructure.Repositories;
using BookingOrchestrator.Application.Interfaces;
using BookingOrchestrator.Application.Saga;
using BookingOrchestrator.Domain.Entities;
using MassTransit;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using PetLuvSystem.SharedLibrary.DependencyInjections;
using PetLuvSystem.SharedLibrary.Middlewares;
using Quartz;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCorsPolicy();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

BsonSerializer.RegisterSerializer(new GuidSerializer(MongoDB.Bson.BsonType.String));

builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var connectionString = builder.Configuration.GetConnectionString("MongoDb") ?? "mongodb://localhost:27017";
    return new MongoClient(connectionString);
});

builder.Services.AddScoped<IMongoDatabase>(sp =>
{
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase("BookingSagaDb");
});

builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();

    x.AddSagaStateMachine<BookingSagaStateMachine, BookingState>()
        .MongoDbRepository(r =>
        {
            r.Connection = "mongodb://localhost:27017";
            r.DatabaseName = "BookingSagaDb";
            r.CollectionName = "BookingState";
        });

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq://localhost", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddQuartz(q =>
{
    q.UseInMemoryStore();
});

builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

builder.Services.AddScoped<IBookingStateRepository, BookingStateRepository>();

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Debug()
    .WriteTo.Console()
    .CreateLogger();

var app = builder.Build();

app.UseCorsPolicy();
app.UseMiddleware<GlobalException>();
app.MapGet("/", () => "BookingOrchestrator is running...");
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
