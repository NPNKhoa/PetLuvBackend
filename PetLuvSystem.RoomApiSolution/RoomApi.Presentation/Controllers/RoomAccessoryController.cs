using Microsoft.AspNetCore.Mvc;
using PetLuvSystem.SharedLibrary.Logs;
using PetLuvSystem.SharedLibrary.Responses;
using RoomApi.Application.DTOs.Conversions;
using RoomApi.Application.DTOs.RoomAccessoryDTOs;
using RoomApi.Application.Interfaces;

namespace RoomApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomAccessoryController(IRoomAccessory _roomAccessory) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetRoomAccessories([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));

                return BadRequest(new Response(false, 400, errorMessages));
            }

            try
            {
                var response = await _roomAccessory.GetAllAsync(pageIndex, pageSize);
                return response.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoomAccessory([FromRoute] Guid id)
        {
            try
            {
                var response = await _roomAccessory.GetByIdAsync(id);
                return response.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoomAccessory([FromForm] CreateUpdateRoomAccessoryDTO dto, IFormFile imageFile)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));

                return BadRequest(new Response(false, 400, errorMessages));
            }

            try
            {
                string imagePath = await SaveImageToStorage(imageFile);

                var entity = RoomAccessoryConversion.ToEntity(dto);
                entity.RoomAccessoryImagePath = imagePath;

                var response = await _roomAccessory.CreateAsync(entity);
                return response.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoomAccessory([FromRoute] Guid id, [FromForm] CreateUpdateRoomAccessoryDTO dto, IFormFile imageFile)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));

                return BadRequest(new Response(false, 400, errorMessages));
            }

            try
            {
                string imagePath = await SaveImageToStorage(imageFile);

                var entity = RoomAccessoryConversion.ToEntity(dto);
                entity.RoomAccessoryId = id;
                entity.RoomAccessoryImagePath = imagePath;

                var response = await _roomAccessory.UpdateAsync(id, entity);
                return response.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoomAccessory([FromRoute] Guid id)
        {
            try
            {
                var response = await _roomAccessory.DeleteAsync(id);
                return response.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
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
