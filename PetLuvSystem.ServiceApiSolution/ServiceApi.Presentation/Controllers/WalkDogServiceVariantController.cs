using Microsoft.AspNetCore.Mvc;
using PetLuvSystem.SharedLibrary.Logs;
using ServiceApi.Application.DTOs.Conversions;
using ServiceApi.Application.DTOs.WalkDogServiceVariantDTOs;
using ServiceApi.Application.Interfaces;

namespace ServiceApi.Presentation.Controllers
{
    [Route("api/walk-dog-variants")]
    [ApiController]
    public class WalkDogServiceVariantController(IWalkDogServiceVariant _walkDogServiceVariant) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllVariants()
        {
            var response = await _walkDogServiceVariant.GetAllAsync(1, 10);
            return response.ToActionResult(this);
        }

        [HttpPost]
        public async Task<IActionResult> CreateWalkDogServiceVariant([FromBody] WalkDogServiceVariantDTO dto)
        {
            try
            {
                var serviceVariant = WalkDogServiceVariantConversion.ToEntity(dto);
                var response = await _walkDogServiceVariant.CreateAsync(serviceVariant);

                return response.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpPut("{serviceId}/{breedId}")]
        public async Task<IActionResult> UpdateWalkDogServiceVariant([FromRoute] Guid serviceId, [FromRoute] Guid breedId, [FromBody] decimal pricePerPeriod)
        {
            try
            {
                var response = await _walkDogServiceVariant.UpdateAsync(serviceId, breedId, pricePerPeriod);
                return response.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpDelete("{serviceId}/{breedId}")]
        public async Task<IActionResult> DeleteWalkDogServiceVariant([FromRoute] Guid serviceId, [FromRoute] Guid breedId)
        {
            try
            {
                var response = await _walkDogServiceVariant.DeleteAsync(serviceId, breedId);
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
