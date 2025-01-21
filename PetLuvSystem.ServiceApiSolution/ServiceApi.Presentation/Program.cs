using ServiceApi.Infrastructure.DependencyInjection;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructureService(builder.Configuration);

try
{
    var app = builder.Build();
    app.UseInfrastructurePolicy();
    //app.UseSwagger();
    //app.UseSwaggerUI();
    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred during startup: {ex.Message}");
    throw;
}
