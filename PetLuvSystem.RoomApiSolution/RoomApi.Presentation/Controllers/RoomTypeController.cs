using Microsoft.AspNetCore.Mvc;
using PetLuvSystem.SharedLibrary.Logs;
using PetLuvSystem.SharedLibrary.Responses;
using RoomApi.Application.DTOs.Conversions;
using RoomApi.Application.DTOs.RoomTypeDTOs;
using RoomApi.Application.Interfaces;

namespace RoomApi.Presentation.Controllers
{
    [Route("api/room-types")]
    [ApiController]
    public class RoomTypeController(IRoomType _roomType) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetRoomTypes([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var response = await _roomType.GetAllAsync(pageIndex, pageSize);
                return response.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoomTypeById([FromRoute] Guid id)
        {
            try
            {
                var response = await _roomType.GetByIdAsync(id);
                return response.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoomType([FromBody] CreateUpdateRoomTypeDTO dto)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));

                return BadRequest(new Response(false, 400, errorMessages));
            }

            try
            {
                var entity = RoomTypeConversion.ToEntity(dto);
                var response = await _roomType.CreateAsync(entity);
                return response.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoomType([FromRoute] Guid id, [FromBody] CreateUpdateRoomTypeDTO dto)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));

                return BadRequest(new Response(false, 400, errorMessages));
            }

            try
            {
                var entity = RoomTypeConversion.ToEntity(dto);
                entity.RoomTypeId = id;
                var response = await _roomType.UpdateAsync(id, entity);
                return response.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoomType([FromRoute] Guid id)
        {
            try
            {
                var response = await _roomType.DeleteAsync(id);
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
