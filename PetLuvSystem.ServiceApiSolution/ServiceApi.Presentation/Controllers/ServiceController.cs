using Microsoft.AspNetCore.Mvc;
using PetLuvSystem.SharedLibrary.Logs;
using PetLuvSystem.SharedLibrary.Responses;
using Quartz;
using ServiceApi.Application.DTOs.Conversions;
using ServiceApi.Application.DTOs.ServiceDTOs;
using ServiceApi.Application.Interfaces;
using ServiceApi.Application.Jobs;
using ServiceApi.Domain.Entities;

namespace ServiceApi.Presentation.Controllers
{
    [Route("api/services")]
    [ApiController]
    public class ServiceController(IService serviceInterface, IServiceType serviceTypeInterface, IServiceVariant serviceVariantInterface, IWalkDogServiceVariant walkDogServiceVariantInterface, ISchedulerFactory schedulerFactory) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetServices([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var response = await serviceInterface.GetAllAsync(pageIndex, pageSize);
                return response.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetServiceById([FromRoute] Guid id)
        {
            try
            {
                var response = await serviceInterface.GetByIdAsync(id);
                return response.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateService([FromForm] CreateServiceDTO dto, [FromForm] IFormFileCollection imageFiles)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = string.Join("; ",
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return BadRequest(new Response(false, 400, errorMessage));
            }

            if (imageFiles == null || !imageFiles.Any())
            {
                return BadRequest(new Response(false, 400, "At least one image is required."));
            }

            try
            {
                var service = ServiceConversion.ToEntity(dto);

                service.ServiceType = await serviceTypeInterface.FindByIdAsync(dto.ServiceTypeId);

                service.ServiceImages = new List<ServiceImage>();
                foreach (var imageFile in imageFiles)
                {
                    var imagePath = await SaveImageToStorage(imageFile);
                    service.ServiceImages.Add(new ServiceImage
                    {
                        ServiceImagePath = imagePath,
                        ServiceId = service.ServiceId,
                    });
                }

                service.ServiceVariants = new List<ServiceVariant>();

                service.WalkDogServiceVariants = new List<WalkDogServiceVariant>();

                var response = await serviceInterface.CreateAsync(service);

                if (response.Flag == true)
                {
                    var scheduler = await schedulerFactory.GetScheduler();

                    var jobDetail = JobBuilder.Create<DeleteServiceJob>()
                        .WithIdentity($"DeleteServiceJob-{service.ServiceId}")
                        .UsingJobData("ServiceId", service.ServiceId)
                        .Build();

                    var trigger = TriggerBuilder.Create()
                        .WithIdentity($"DeleteServiceTrigger-{service.ServiceId}")
                        .StartAt(DateTimeOffset.Now.AddMinutes(30))
                        .Build();

                    await scheduler.ScheduleJob(jobDetail, trigger);
                }

                return response.ToActionResult(this);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    LogException.LogError($"Inner Exception: {ex.InnerException.Message}");
                }
                LogException.LogExceptions(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateService([FromRoute] Guid id, [FromForm] UpdateServiceDTO dto)
        {
            try
            {
                var service = ServiceConversion.ToEntity(dto);

                service.ServiceId = id;

                if (dto.ServiceTypeId.HasValue)
                {
                    service.ServiceType = await serviceTypeInterface.FindByIdAsync(dto.ServiceTypeId.Value);
                }

                var response = await serviceInterface.UpdateAsync(id, service);
                return response.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteService([FromRoute] Guid id)
        {
            try
            {
                var response = await serviceInterface.DeleteAsync(id);
                return response.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpGet("{serviceId}/service-variants")]
        public async Task<IActionResult> GetVariantsByService([FromRoute] Guid serviceId)
        {
            try
            {
                var serviceVariants = await serviceVariantInterface.GetByServiceAsync(serviceId);
                return serviceVariants.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpGet("{serviceId}/walk-dog-variants")]
        public async Task<IActionResult> GetWalkDogVariantsByService([FromRoute] Guid serviceId)
        {
            try
            {
                var walkDogServiceVariants = await walkDogServiceVariantInterface.GetByServiceAsync(serviceId);
                return walkDogServiceVariants.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        private static async Task<string> SaveImageToStorage(IFormFile imageFile)
        {
            var randomSuffix = new Random().Next(1000, 9999);
            var fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" +
                randomSuffix + Path.GetExtension(imageFile.FileName);

            var directoryPath = Path.Combine("wwwroot", "images");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var filePath = Path.Combine(directoryPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return $"/images/{fileName}";
        }
    }
}
