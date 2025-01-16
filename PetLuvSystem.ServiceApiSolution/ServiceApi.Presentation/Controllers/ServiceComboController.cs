using Microsoft.AspNetCore.Mvc;
using PetLuvSystem.SharedLibrary.Logs;
using PetLuvSystem.SharedLibrary.Responses;
using ServiceApi.Application.DTOs.Conversions;
using ServiceApi.Application.DTOs.ServiceComboDTOs;
using ServiceApi.Application.Interfaces;

namespace ServiceApi.Presentation.Controllers
{
    [Route("api/service-combos")]
    [ApiController]
    public class ServiceComboController(IServiceCombo _serviceCombo) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllServiceCombos([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var response = await _serviceCombo.GetAllAsync(pageIndex, pageSize);
                return response.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetServiceComboById(Guid id)
        {
            try
            {
                var response = await _serviceCombo.GetByIdAsync(id);
                return response.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateServiceCombo([FromBody] CreateUpdateServiceComboDTO dto)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = string.Join("; ",
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return BadRequest(new Response(false, 400, errorMessage));
            }

            try
            {
                var entity = ServiceComboConversion.ToEntity(dto);
                var response = await _serviceCombo.CreateAsync(entity);
                return response.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateServiceCombo([FromRoute] Guid id, [FromBody] CreateUpdateServiceComboDTO dto)
        {
            try
            {
                var entity = ServiceComboConversion.ToEntity(dto);
                var response = await _serviceCombo.UpdateAsync(id, entity);
                return response.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServiceCombo(Guid id)
        {
            try
            {
                var response = await _serviceCombo.DeleteAsync(id);
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
