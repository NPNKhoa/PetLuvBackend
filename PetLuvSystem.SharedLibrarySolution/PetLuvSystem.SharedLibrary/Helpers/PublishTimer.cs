using PetLuvSystem.SharedLibrary.Logs;
using Quartz;

namespace PetLuvSystem.SharedLibrary.Helpers
{
    public class PublishTimer : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            var serviceId = context.MergedJobDataMap.GetString("serviceId");

            LogException.LogInformation($"Publishing service with id {serviceId}");


            throw new NotImplementedException();
        }
    }
}
