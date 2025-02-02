using Microsoft.AspNetCore.Mvc;
using PetApi.Application.DTOs.Conversions;
using PetApi.Application.DTOs.PetDTOs;
using PetApi.Application.Interfaces;
using PetApi.Domain.Entities;
using PetLuvSystem.SharedLibrary.Logs;
using PetLuvSystem.SharedLibrary.Responses;

namespace PetApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PetController(IPet _pet, IPetBreed _petBreed) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllPets([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var response = await _pet.GetAllAsync(pageIndex, pageSize);
                return response.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPetById(Guid id)
        {
            try
            {
                var response = await _pet.GetByIdAsync(id);
                return response.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreatePet([FromForm] CreateUpdatePetDTO dto, [FromForm] IFormFileCollection imageFiles)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = string.Join("; ", ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage));

                return BadRequest(new Response(false, 400, errorMessages));
            }

            try
            {
                var existingBreed = await _petBreed.FindById(dto.BreedId, true, false);

                if (existingBreed is null)
                {
                    return BadRequest(new Response(false, 400, "Can not find any pet breed with this id"));
                }

                var entity = PetConversion.ToEntity(dto);

                if (imageFiles is not null)
                {
                    entity.PetImagePaths = new List<PetImage>();
                    foreach (var imageFile in imageFiles)
                    {
                        var imagePath = await HandleUploadImage(imageFile);
                        entity.PetImagePaths.Add(new PetImage
                        {
                            PetImagePath = imagePath,
                            PetId = entity.PetId,
                        });
                    }
                }

                var response = await _pet.CreateAsync(entity);
                return response.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePet(Guid id, [FromForm] CreateUpdatePetDTO dto, [FromForm] IFormFileCollection imageFiles)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = string.Join("; ", ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage));

                return BadRequest(new Response(false, 400, errorMessages));
            }

            try
            {
                var existingPet = await _pet.FindById(id);

                if (existingPet is null)
                {
                    return BadRequest(new Response(false, 400, "Can not find any pet with this id"));
                }

                var entity = PetConversion.ToEntity(dto);
                entity.PetId = id;

                if (imageFiles is not null)
                {
                    entity.PetImagePaths = new List<PetImage>();
                    foreach (var imageFile in imageFiles)
                    {
                        var imagePath = await HandleUploadImage(imageFile);
                        entity.PetImagePaths.Add(new PetImage
                        {
                            PetImagePath = imagePath,
                            PetId = entity.PetId,
                        });
                    }
                }

                var response = await _pet.UpdateAsync(id, entity);
                return response.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePet(Guid id)
        {
            try
            {
                var response = await _pet.DeleteAsync(id);
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
