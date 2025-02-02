using Microsoft.AspNetCore.Mvc;
using PetApi.Application.DTOs.Conversions;
using PetApi.Application.DTOs.PetTypeDTOs;
using PetApi.Application.Interfaces;
using PetLuvSystem.SharedLibrary.Logs;
using PetLuvSystem.SharedLibrary.Responses;

namespace PetApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PetTypeController(IPetType _petType) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllPetTypes([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var response = await _petType.GetAllAsync(pageIndex, pageSize);
                return response.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPetTypeById(Guid id)
        {
            try
            {
                var response = await _petType.GetByIdAsync(id);
                return response.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreatePetType([FromBody] CreateUpdatePetTypeDTO dto)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = string.Join("; ", ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage));

                return BadRequest(new Response(false, 400, errorMessages));
            }

            try
            {
                var entity = PetTypeConversion.ToEntity(dto);
                var response = await _petType.CreateAsync(entity);
                return response.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePetType([FromRoute] Guid id, [FromBody] CreateUpdatePetTypeDTO dto)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = string.Join("; ", ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage));

                return BadRequest(new Response(false, 400, errorMessages));
            }
            try
            {
                var entity = PetTypeConversion.ToEntity(dto);
                entity.PetTypeId = id;
                var response = await _petType.UpdateAsync(id, entity);
                return response.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePetType([FromRoute] Guid id)
        {
            try
            {
                var response = await _petType.DeleteAsync(id);
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
