using PetLuvSystem.SharedLibrary.Logs;
using Quartz;
using ServiceApi.Application.Interfaces;
using ServiceApi.Domain.Entities;

namespace ServiceApi.Application.Jobs
{
    public class DeleteServiceJob : IJob
    {
        private readonly IService _serviceInterface;

        public DeleteServiceJob(IService serviceInterface)
        {
            _serviceInterface = serviceInterface;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            if (!context.JobDetail.JobDataMap.ContainsKey("ServiceId"))
            {
                LogException.LogError("JobDataMap not contain ServiceId.");
                return;
            }

            var serviceId = context.JobDetail.JobDataMap.GetGuid("ServiceId");
            if (serviceId == Guid.Empty)
            {
                LogException.LogError("ServiceId invalid.");
                return;
            }

            var service = await _serviceInterface.FindServiceById(serviceId);
            if (service == null) return;

            // Kiểm tra ServiceVariants và WalkDogServiceVariants
            if ((service.ServiceVariants == null || service.ServiceVariants.Count == 0) &&
                (service.WalkDogServiceVariants == null || service.WalkDogServiceVariants.Count == 0))
            {
                await _serviceInterface.DeleteAsync(serviceId);
                LogException.LogInformation($"Service with id {serviceId} is automatically deleted");
            }
            else
            {
                var updatedService = new Service
                {
                    ServiceId = service.ServiceId,
                    ServiceName = service.ServiceName,
                    ServiceDesc = service.ServiceDesc,
                    IsVisible = true,
                    ServiceTypeId = service.ServiceTypeId,
                    ServiceVariants = service.ServiceVariants,
                    WalkDogServiceVariants = service.WalkDogServiceVariants
                };

                var response = await _serviceInterface.UpdateAsync(serviceId, updatedService);

                if (response.Flag == false)
                {
                    LogException.LogInformation($"Service {serviceId} is fail to update");
                    return;
                }

                // Hủy job khi đã cập nhật thành công
                var scheduler = context.Scheduler;
                await scheduler.DeleteJob(context.JobDetail.Key);

                LogException.LogInformation($"Service {serviceId} is updated IsVisible = true and job was cancelled.");
            }
        }
    }
}
