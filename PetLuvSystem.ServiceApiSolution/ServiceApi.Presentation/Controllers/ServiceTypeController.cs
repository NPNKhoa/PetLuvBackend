using Microsoft.AspNetCore.Mvc;
using PetLuvSystem.SharedLibrary.Logs;
using PetLuvSystem.SharedLibrary.Responses;
using ServiceApi.Application.DTOs.Conversions;
using ServiceApi.Application.DTOs.ServiceTypeDTOs;
using ServiceApi.Application.Interfaces;

namespace ServiceApi.Presentation.Controllers
{
    [Route("api/service-types")]
    [ApiController]
    public class ServiceTypeController(IServiceType serviceTypeInterface) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllServiceType()
        {
            try
            {
                var products = await serviceTypeInterface.GetAllAsync();
                return products.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(500, new Response(false, 500, "Internal Server Error"));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetServiceTypeById([FromRoute] Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(new Response(false, 400, "Invalid ID provided"));
            }
            try
            {
                var product = await serviceTypeInterface.GetByIdAsync(id);
                return product.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(500, new Response(false, 500, "Internal Server Error"));
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateServiceType([FromBody] CreateUpdateServiceTypeDTO serviceTypeDto)
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
                var serviceTypeEntity = ServiceTypeConversion.ToEntity(serviceTypeDto);
                var product = await serviceTypeInterface.CreateAsync(serviceTypeEntity);
                return product.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(500, new Response(false, 500, "Internal Server Error"));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateServiceType([FromRoute] Guid id, [FromBody] CreateUpdateServiceTypeDTO serviceTypeDto)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(new Response(false, 400, "Invalid ID provided"));
            }
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
                var serviceTypeEntity = ServiceTypeConversion.ToEntity(serviceTypeDto);
                serviceTypeEntity.ServiceTypeId = id;
                var serviceType = await serviceTypeInterface.UpdateAsync(id, serviceTypeEntity);
                return serviceType.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(500, new Response(false, 500, "Internal Server Error"));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServiceType([FromRoute] Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(new Response(false, 400, "Invalid ID provided"));
            }
            try
            {
                var serviceType = await serviceTypeInterface.DeleteAsync(id);
                return serviceType.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(500, new Response(false, 500, "Internal Server Error"));
            }
        }
    }
}
