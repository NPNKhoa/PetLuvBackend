using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;
using PetLuvSystem.SharedLibrary.Logs;

namespace BooingOrchestrator.Infrastructure.Data
{
    public static class MongoDbContext
    {
        public static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
        {
            var mongoClient = new MongoClient(configuration["MongoDB:ConnectionString"]);
            var database = mongoClient.GetDatabase(configuration["MongoDB:Database"]);

            try
            {
                database.RunCommandAsync((Command<BsonDocument>)"{ping:1}").Wait();
                LogException.LogInformation("MongoDB kết nối thành công!");
            }
            catch (Exception ex)
            {
                LogException.LogError($"MongoDB kết nối thất bại: {ex.Message}");
            }

            services.AddSingleton(database);
            return services;
        }
    }
}
