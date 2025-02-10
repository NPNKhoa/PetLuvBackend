using Microsoft.AspNetCore.Mvc;
using PetLuvSystem.SharedLibrary.Logs;
using PetLuvSystem.SharedLibrary.Responses;
using ProductApi.Application.DTOs.Conversions;
using ProductApi.Application.DTOs.FoodDTOs;
using ProductApi.Application.Interfaces;
using ProductApi.Domain.Entities;

namespace ProductApi.Presentation.Controllers
{
    [Route("api/foods")]
    [ApiController]
    public class FoodController(IFood _food) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetFoods([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            var response = await _food.GetAllAsync(pageIndex, pageSize);
            return response.ToActionResult(this);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFood(Guid id)
        {
            var response = await _food.GetByIdAsync(id);
            return response.ToActionResult(this);
        }

        [HttpPost]
        public async Task<IActionResult> CreateFood([FromForm] CreateUpdateFoodDTO dto, [FromForm] IFormFileCollection imageFiles)
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
                var entity = FoodConversion.ToEntity(dto);

                if (imageFiles is not null)
                {
                    entity.FoodImages = new List<FoodImage>();
                    foreach (var imageFile in imageFiles)
                    {
                        var imagePath = await HandleUploadImage(imageFile);
                        entity.FoodImages.Add(new FoodImage
                        {
                            FoodImagePath = imagePath,
                            FoodId = entity.FoodId
                        });
                    }
                }

                var response = await _food.CreateAsync(entity);
                return response.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(500, new Response(false, 500, "Internal Server Error"));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFood(Guid id, [FromForm] CreateUpdateFoodDTO dto, [FromForm] IFormFileCollection imageFiles)
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
                var entity = FoodConversion.ToEntity(dto);
                entity.FoodId = id;

                if (imageFiles is not null)
                {
                    entity.FoodImages = new List<FoodImage>();
                    foreach (var imageFile in imageFiles)
                    {
                        var imagePath = await HandleUploadImage(imageFile);
                        entity.FoodImages.Add(new FoodImage
                        {
                            FoodImagePath = imagePath,
                            FoodId = entity.FoodId
                        });
                    }
                }

                var response = await _food.UpdateAsync(id, entity);
                return response.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(500, new Response(false, 500, "Internal Server Error"));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFood(Guid id)
        {
            var response = await _food.DeleteAsync(id);
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
