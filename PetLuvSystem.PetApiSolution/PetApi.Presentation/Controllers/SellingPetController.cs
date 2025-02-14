using Microsoft.AspNetCore.Mvc;
using PetApi.Application.DTOs.Conversions;
using PetApi.Application.DTOs.SellingPetDTOs;
using PetApi.Application.Interfaces;
using PetApi.Domain.Entities;
using PetLuvSystem.SharedLibrary.Logs;
using PetLuvSystem.SharedLibrary.Responses;

namespace PetApi.Presentation.Controllers
{
    [Route("api/selling-pets")]
    [ApiController]
    public class SellingPetController(ISellingPet _sellingPet) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllSellingPets([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            var response = await _sellingPet.GetAllAsync(pageIndex, pageSize);
            return response.ToActionResult(this);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSellingPetById(Guid id)
        {
            var response = await _sellingPet.GetByIdAsync(id);
            return response.ToActionResult(this);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSellingPet([FromForm] CreateUpdateSellingPetDTO dto, [FromForm] IFormFileCollection imageFiles)
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
                var entity = SellingPetConversion.ToEntity(dto);

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

                var response = await _sellingPet.CreateAsync(entity);
                return response.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSellingPet(Guid id, [FromForm] CreateUpdateSellingPetDTO dto, [FromForm] IFormFileCollection imageFiles)
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
                var existingSellingPet = await _sellingPet.FindById(id);

                if (existingSellingPet is null)
                {
                    return BadRequest(new Response(false, 400, "Can not find any selling pet with this id"));
                }

                var entity = SellingPetConversion.ToEntity(dto);
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

                var response = await _sellingPet.UpdateAsync(id, entity);
                return response.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSellingPet(Guid id)
        {
            var response = await _sellingPet.DeleteAsync(id);
            return response.ToActionResult(this);
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
