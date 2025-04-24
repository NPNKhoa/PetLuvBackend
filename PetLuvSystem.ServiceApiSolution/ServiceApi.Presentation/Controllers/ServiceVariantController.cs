using Microsoft.AspNetCore.Mvc;
using PetLuvSystem.SharedLibrary.Logs;
using PetLuvSystem.SharedLibrary.Responses;
using ServiceApi.Application.DTOs.Conversions;
using ServiceApi.Application.DTOs.ServiceVariantDTOs;
using ServiceApi.Application.Interfaces;

namespace ServiceApi.Presentation.Controllers
{
    [Route("api/service-variants")]
    [ApiController]
    public class ServiceVariantController(IServiceVariant _serviceVariant) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllVariants()
        {
            var response = await _serviceVariant.GetAllAsync(1, 10);
            return response.ToActionResult(this);
        }

        [HttpGet("{serviceId}/{breedId}/{petWeightRange}")]
        public async Task<IActionResult> GetServiceVariantById([FromRoute] Guid serviceId, [FromRoute] Guid breedId, [FromRoute] string petWeightRange)
        {
            try
            {
                var serviceVariant = await _serviceVariant.GetByKeyAsync(serviceId, breedId, petWeightRange);
                return serviceVariant.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateServiceVariant([FromBody] CreateUpdateServiceVariantDTO serviceVariantDto)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = string.Join("; ",
                    ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));

                return BadRequest(new Response(false, 400, errorMessage));
            }
            try
            {
                var entity = ServiceVariantConversion.ToEntity(serviceVariantDto);
                var response = await _serviceVariant.CreateAsync(entity);
                return response.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpPut("{serviceId}/{breedId}/{petWeightRange}")]
        public async Task<IActionResult> UpdateServiceVariant([FromRoute] Guid serviceId, [FromRoute] Guid breedId, [FromRoute] string petWeightRange, [FromBody] UpdateServiceVariantDTO dto)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = string.Join("; ",
                    ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));

                return BadRequest(new Response(false, 400, errorMessage));
            }
            try
            {
                var entity = ServiceVariantConversion.ToEntity(dto);
                entity.ServiceId = serviceId;

                return (await _serviceVariant.UpdateAsync(serviceId, breedId, petWeightRange, entity)).ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpDelete("{serviceId}/{breedId}/{petWeightRange}")]
        public async Task<IActionResult> DeleteServiceVariant([FromRoute] Guid serviceId, [FromRoute] Guid breedId, [FromRoute] string petWeightRange)
        {
            try
            {
                var response = await _serviceVariant.DeleteAsync(serviceId, breedId, petWeightRange);
                return response.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }
    }
}
