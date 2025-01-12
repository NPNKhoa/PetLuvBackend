using Microsoft.AspNetCore.Mvc;
using PetLuvSystem.SharedLibrary.Logs;
using PetLuvSystem.SharedLibrary.Responses;
using ServiceApi.Application.DTOs.Conversions;
using ServiceApi.Application.DTOs.ServiceDTOs;
using ServiceApi.Application.Interfaces;
using ServiceApi.Domain.Entities;

namespace ServiceApi.Presentation.Controllers
{
    [Route("api/services")]
    [ApiController]
    public class ServiceController(IService serviceInterface) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetServices()
        {
            try
            {
                var response = await serviceInterface.GetAllAsync();
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
        public async Task<IActionResult> CreateService([FromForm] CreateUpdateServiceDTO dto, [FromForm] IFormFileCollection imageFiles)
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

                service.ServiceImages = (ICollection<ServiceImage>?)imageFiles.Select(async file => new ServiceImage
                {
                    ServiceImagePath = await SaveImageToStorage(file),
                    ServiceId = service.ServiceId
                }).ToList();

                //if (dto.ServiceVariants != null)
                //{
                //    service.ServiceVariants = dto.ServiceVariants.Select(p => new ServiceVariant
                //    {
                //        ServiceId = service.ServiceId,
                //        BreedId = p.BreedId,
                //        PetWeightRange = p.PetWeightRange,
                //        Price = p.ServicePrice
                //    }).ToList();
                //}
                //if (dto.WalkDogServiceVariants != null)
                //{
                //    service.WalkDogServiceVariants = dto.WalkDogServiceVariants.Select(p => new WalkDogServiceVariant
                //    {
                //        ServiceId = service.ServiceId,
                //        PricePerPeriod = p.PricePerPeriod
                //    }).ToList();
                //}

                var response = await serviceInterface.CreateAsync(service);
                return response.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateService([FromRoute] Guid id, [FromBody] CreateUpdateServiceDTO dto)
        {
            try
            {
                var service = ServiceConversion.ToEntity(dto);
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

        private async Task<string> SaveImageToStorage(IFormFile imageFile)
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
