using Microsoft.AspNetCore.Mvc;
using PetApi.Application.DTOs.Conversions;
using PetApi.Application.DTOs.PetBreedDTOs;
using PetApi.Application.Interfaces;
using PetLuvSystem.SharedLibrary.Helpers.CloudinaryHelper;
using PetLuvSystem.SharedLibrary.Logs;
using PetLuvSystem.SharedLibrary.Responses;

namespace PetApi.Presentation.Controllers
{
    [Route("api/pet-breeds")]
    [ApiController]
    public class PetBreedController(IPetBreed _petBreed, IPetType _petType) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllBreeds([FromQuery] string? petType, [FromQuery] int? pageIndex = 1, [FromQuery] int? pageSize = 10)
        {
            try
            {
                int validPageIndex = pageIndex is not null ? (int)pageIndex : 0;
                int validPageSize = pageSize is not null ? (int)pageSize : 0;

                var response = string.IsNullOrEmpty(petType)
                    ? await _petBreed.GetAllAsync(validPageIndex, validPageSize)
                    : await _petBreed.GetByAsync(b => b.PetType.PetTypeName.Trim().ToLower().Equals(petType.ToLower().Trim()), true);

                return response.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpGet("mapping")]
        public async Task<IActionResult> GetBreedMapping([FromQuery] bool showHidden = false)
        {
            try
            {
                var response = await _petBreed.GetBreedMapping();
                return response.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpGet("pet-types/{id}")]
        public async Task<IActionResult> GetBreedsByPetType([FromRoute] Guid id)
        {
            try
            {
                var response = await _petBreed.GetByAsync(x => x.PetTypeId == id, true);
                return response.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return (new Response(false, 500, "Internal Server Error")).ToActionResult(this);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBreedById([FromRoute] Guid id)
        {
            try
            {
                var response = await _petBreed.GetByIdAsync(id);
                return response.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateBreed([FromForm] CreateUpdatePetBreedDTO dto)
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
                var petType = await _petType.FindById(dto.PetTypeId, true, false);

                if (petType is null)
                {
                    return BadRequest(new Response(false, 400, "Can not find pet type with this ID"));
                }

                string illustrationImage = String.Empty;

                if (dto.IllustrationImage is not null)
                {
                    illustrationImage = await CloudinaryHelper.UploadImageToCloudinary(dto.IllustrationImage, "PetBreed");
                }

                var entity = PetBreedConversion.ToEntity(dto, illustrationImage);

                var response = await _petBreed.CreateAsync(entity);
                return response.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBreed([FromRoute] Guid id, [FromForm] CreateUpdatePetBreedDTO dto)
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
                string illustrationImage = String.Empty;

                if (dto.IllustrationImage is not null)
                {
                    illustrationImage = await CloudinaryHelper.UploadImageToCloudinary(dto.IllustrationImage, "PetBreed");
                }

                var entity = PetBreedConversion.ToEntity(dto, illustrationImage);
                entity.BreedId = id;

                var response = await _petBreed.UpdateAsync(id, entity);
                return response.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBreed([FromRoute] Guid id)
        {
            try
            {
                var response = await _petBreed.DeleteAsync(id);
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
