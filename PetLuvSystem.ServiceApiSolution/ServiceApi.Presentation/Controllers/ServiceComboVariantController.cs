using Microsoft.AspNetCore.Mvc;
using PetLuvSystem.SharedLibrary.Logs;
using PetLuvSystem.SharedLibrary.Responses;
using ServiceApi.Application.DTOs.Conversions;
using ServiceApi.Application.DTOs.ServiceComboVariantDTOs;
using ServiceApi.Application.Interfaces;

namespace ServiceApi.Presentation.Controllers
{
    [Route("api/service-combo-variants")]
    [ApiController]
    public class ServiceComboVariantController(IServiceComboVariant _serviceComboVariant) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateServiceComboVariant([FromBody] CreateUpdateServiceComboVariantDTO serviceComboVariant)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));

                return BadRequest(new Response(false, 400, errorMessages));
            }

            try
            {
                var entity = ServiceComboVariantConversion.ToEntity(serviceComboVariant);

                var response = await _serviceComboVariant.CreateAsync(entity);

                return response.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpPut("{serviceComboId}/{breedId}/{weightRange}")]
        public async Task<IActionResult> UpdateServiceComboVariant([FromRoute] Guid serviceComboId, [FromRoute] Guid breedId, [FromRoute] string weightRange, decimal comboPrice)
        {
            try
            {
                var response = await _serviceComboVariant.UpdateAsync(serviceComboId, breedId, weightRange, comboPrice);

                return response.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpDelete("{serviceComboId}/{breedId}/{weightRange}")]
        public async Task<IActionResult> DeleteServiceComboVariant([FromRoute] Guid serviceComboId, [FromRoute] Guid breedId, [FromRoute] string weightRange)
        {
            try
            {
                var response = await _serviceComboVariant.DeleteAsync(serviceComboId, breedId, weightRange);

                return response.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpGet("{serviceComboId}/{breedId}/{weightRange}")]
        public async Task<IActionResult> GetServiceComboVariantByKey([FromRoute] Guid serviceComboId, [FromRoute] Guid breedId, [FromRoute] string weightRange)
        {
            try
            {
                var response = await _serviceComboVariant.GetByKeyAsync(serviceComboId, breedId, weightRange);

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
