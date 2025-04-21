using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetLuvSystem.SharedLibrary.Logs;
using PetLuvSystem.SharedLibrary.Responses;
using RoomApi.Application.DTOs.Conversions;
using RoomApi.Application.DTOs.RoomDTOs;
using RoomApi.Application.Interfaces;
using RoomApi.Domain.Entities;

namespace RoomApi.Presentation.Controllers
{
    [Route("api/rooms")]
    [ApiController]
    public class RoomController(IRoom _room, IRoomType _roomType) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllRooms([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var response = await _room.GetAllAsync(pageIndex, pageSize);
                return response.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoomById([FromRoute] Guid id)
        {
            try
            {
                var response = await _room.GetByIdAsync(id);
                return response.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpGet("available")]
        public async Task<IActionResult> GetRoomsByBreed([FromQuery] IEnumerable<Guid> breedIds)
        {
            try
            {
                return (await _room.GetValidRoom(breedIds)).ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoom([FromForm] CreateUpdateRoomDTO dto, [FromForm] IFormFileCollection imageFiles)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage));

                return BadRequest(new Response(false, 400, errorMessages));
            }

            try
            {
                var existingRoomType = await _roomType.FindById(dto.RoomTypeId);
                if (existingRoomType is null)
                {
                    return BadRequest(new Response(false, 404, "Room Type not found"));
                }

                var entity = RoomConversion.ToEntity(dto);

                if (imageFiles is not null)
                {
                    entity.RoomImages = new List<RoomImage>();
                    foreach (var imageFile in imageFiles)
                    {
                        var imagePath = await HandleUploadImage(imageFile);
                        entity.RoomImages.Add(new RoomImage
                        {
                            RoomImagePath = imagePath,
                            RoomId = entity.RoomId,
                        });
                    }
                }

                entity.RoomTypeId = dto.RoomTypeId;

                var response = await _room.CreateAsync(entity);
                return response.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoom([FromRoute] Guid id, [FromForm] CreateUpdateRoomDTO dto, [FromForm] IFormFileCollection imageFiles)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage));

                return BadRequest(new Response(false, 400, errorMessages));
            }

            try
            {
                var existingRoomType = await _roomType.FindById(dto.RoomTypeId);
                if (existingRoomType is null)
                {
                    return BadRequest(new Response(false, 404, "Room Type not found"));
                }
                var entity = RoomConversion.ToEntity(dto);

                entity.RoomId = id;
                entity.RoomTypeId = dto.RoomTypeId;

                if (imageFiles is not null)
                {
                    entity.RoomImages = new List<RoomImage>();
                    foreach (var imageFile in imageFiles)
                    {
                        var imagePath = await HandleUploadImage(imageFile);
                        entity.RoomImages.Add(new RoomImage
                        {
                            RoomImagePath = imagePath,
                            RoomId = entity.RoomId,
                        });
                    }
                }

                var response = await _room.UpdateAsync(id, entity);
                return response.ToActionResult(this);
            }
            catch (DbUpdateException ex)
            {
                // Log lỗi chi tiết
                var errorMessage = $"DbUpdateException: {ex.Message}";
                if (ex.InnerException != null)
                {
                    errorMessage += $"\nInner Exception: {ex.InnerException.Message}";
                    if (ex.InnerException.InnerException != null)
                    {
                        errorMessage += $"\nInner-Inner Exception: {ex.InnerException.InnerException.Message}";
                    }
                }

                LogException.LogExceptions(ex); // Gọi log lỗi
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error"); // Trả về lỗi chi tiết
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom([FromRoute] Guid id)
        {
            try
            {
                var response = await _room.DeleteAsync(id);
                return response.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        private static async Task<string> HandleUploadImage(IFormFile imageFile)
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
