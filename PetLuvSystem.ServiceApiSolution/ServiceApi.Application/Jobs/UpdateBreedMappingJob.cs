using Quartz;
using ServiceApi.Application.Interfaces;

namespace ServiceApi.Application.Jobs
{
    public class UpdateBreedMappingJob : IJob
    {
        private readonly IBreedMappingService _breedMappingClient;

        public UpdateBreedMappingJob(IBreedMappingService breedMappingClient)
        {
            _breedMappingClient = breedMappingClient;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _breedMappingClient.GetBreedMappingAsync();
        }
    }
}
