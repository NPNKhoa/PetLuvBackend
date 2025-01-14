using PetLuvSystem.SharedLibrary.Logs;
using Quartz;
using ServiceApi.Application.Interfaces;

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
                LogException.LogError("JobDataMap không chứa ServiceId.");
                return;
            }

            var serviceId = context.JobDetail.JobDataMap.GetGuid("ServiceId");
            if (serviceId == Guid.Empty)
            {
                LogException.LogError("ServiceId không hợp lệ.");
                return;
            }

            var service = await _serviceInterface.FindServiceById(serviceId);
            if (service == null) return;

            // Kiểm tra ServiceVariants và WalkDogServiceVariants
            if ((service.ServiceVariants == null || service.ServiceVariants.Count == 0) &&
                (service.WalkDogServiceVariants == null || service.WalkDogServiceVariants.Count == 0))
            {
                await _serviceInterface.DeleteAsync(serviceId);
                LogException.LogInformation($"Service {serviceId} đã bị xóa tự động.");
            }
            else
            {
                service.IsVisible = true; // Cập nhật trạng thái
                await _serviceInterface.UpdateAsync(serviceId, service);

                // Hủy job khi đã cập nhật thành công
                var scheduler = context.Scheduler;
                await scheduler.DeleteJob(context.JobDetail.Key);

                LogException.LogInformation($"Service {serviceId} đã được cập nhật IsVisible = true và job đã bị hủy.");
            }
        }
    }
}
