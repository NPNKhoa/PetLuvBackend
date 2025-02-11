using Microsoft.AspNetCore.Mvc;
using PetLuvSystem.SharedLibrary.Logs;
using PetLuvSystem.SharedLibrary.Responses;
using UserApi.Application.DTOs.Conversions;
using UserApi.Application.DTOs.StaffDegreeDTOs;
using UserApi.Application.Interfaces;

namespace UserApi.Presentation.Controllers
{
    [Route("api/staff-degrees")]
    [ApiController]
    public class StaffDegreeController(IStaffDegree _staffDegree) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetStaffDegrees([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            var response = await _staffDegree.GetAllAsync(pageIndex, pageSize);
            return response.ToActionResult(this);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStaffDegree(Guid id)
        {
            var response = await _staffDegree.GetByIdAsync(id);
            return response.ToActionResult(this);
        }

        [HttpPost]
        public async Task<IActionResult> CreateStaffDegree([FromForm] CreateUpdateStaffDegreeDTO staffDegreeDTO)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = string.Join("; ", ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage));

                return (new Response(false, 400, errorMessages)).ToActionResult(this);
            }

            try
            {
                if (staffDegreeDTO.DegreeImage is null)
                {
                    return (new Response(false, 400, "Ảnh bằng cấp không được để trống")).ToActionResult(this);
                }

                string degreeImagePath = await HandleUploadImage(staffDegreeDTO.DegreeImage);

                var entity = StaffDegreeConversion.ToEntity(staffDegreeDTO, degreeImagePath);
                var response = await _staffDegree.CreateAsync(entity);
                return response.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return (new Response(false, 500, "Interal Server Error")).ToActionResult(this);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStaffDegree(Guid id, [FromForm] CreateUpdateStaffDegreeDTO staffDegreeDTO)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = string.Join("; ", ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage));

                return (new Response(false, 400, errorMessages)).ToActionResult(this);
            }

            try
            {
                var existingStaffDegree = await _staffDegree.FindById(id);

                if (existingStaffDegree is null)
                {
                    return (new Response(false, 404, "Không tìm thấy bằng cấp nhân viên")).ToActionResult(this);
                }

                string degreeImagePath = existingStaffDegree.DegreeImage;

                if (staffDegreeDTO.DegreeImage is not null)
                {
                    degreeImagePath = await HandleUploadImage(staffDegreeDTO.DegreeImage);
                }

                var entity = StaffDegreeConversion.ToEntity(staffDegreeDTO, degreeImagePath);
                entity.DegreeId = id;

                var response = await _staffDegree.UpdateAsync(id, entity);
                return response.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return (new Response(false, 500, "Internal Server Error")).ToActionResult(this);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStaffDegree(Guid id)
        {
            var response = await _staffDegree.DeleteAsync(id);
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
