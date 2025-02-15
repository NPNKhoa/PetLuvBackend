using Microsoft.AspNetCore.Mvc;
using PetApi.Application.DTOs.Conversions;
using PetApi.Application.DTOs.PetHealthBookDetailDTOs;
using PetApi.Application.Interfaces;
using PetLuvSystem.SharedLibrary.Helpers.CloudinaryHelper;
using PetLuvSystem.SharedLibrary.Logs;
using PetLuvSystem.SharedLibrary.Responses;

namespace PetApi.Presentation.Controllers
{
    [Route("api/pet-health-books")]
    [ApiController]
    public class PetHealthBookController(IPetHealthBookDetail _petHealthBookDetail) : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPetHealthBookDetailByHealthBook(Guid id, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            var response = await _petHealthBookDetail.GetByHealthBook(id);
            return response.ToActionResult(this);
        }

        [HttpGet("/api/pet-health-books/detail/{id}")]
        public async Task<IActionResult> GetPetHealthBookDetailById(Guid id)
        {
            var response = await _petHealthBookDetail.GetByIdAsync(id);
            return response.ToActionResult(this);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePetHealthBookDetail([FromForm] CreateUpdatePetHealthBookDetailDTO dto)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = string
                    .Join("; ", ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage));

                return (new Response(false, 400, errorMessages)).ToActionResult(this);
            }

            try
            {

                if (dto.TreatmentProof is null)
                {
                    return (new Response(false, 400, "TreatmentProof is required")).ToActionResult(this);
                }

                if (dto.VetDegree is null)
                {
                    return (new Response(false, 400, "VetDegree is required")).ToActionResult(this);
                }

                string proofPath = await CloudinaryHelper.UploadImageToCloudinary(dto.TreatmentProof, "PetHealthBooks");
                string vetDegreePath = await CloudinaryHelper.UploadImageToCloudinary(dto.VetDegree, "PetHealthBooks");

                var entity = PetHealthBookDetailConversion.ToEntity(dto, proofPath, vetDegreePath);
                var response = await _petHealthBookDetail.CreateAsync(entity);

                return response.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                var innerExceptionMessage = ex.InnerException?.Message ?? ex.Message;
                Console.WriteLine("Inner Exception: " + innerExceptionMessage);
                return (new Response(false, 500, "Internal Server Error")).ToActionResult(this);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePetHealthBookDetail(Guid id, [FromForm] CreateUpdatePetHealthBookDetailDTO dto)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = string
                    .Join("; ", ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage));

                return (new Response(false, 400, errorMessages)).ToActionResult(this);
            }

            try
            {
                var petHealthBookDetail = await _petHealthBookDetail.FindById(id);

                if (petHealthBookDetail is null)
                {
                    return (new Response(false, 404, "Không tìm thấy chi tiết sổ sức khỏe thú cưng")).ToActionResult(this);
                }

                string proofPath = petHealthBookDetail.TreatmentProof;
                string vetDegreePath = petHealthBookDetail.VetDegree;

                if (dto.TreatmentProof is not null)
                {
                    proofPath = await CloudinaryHelper.UploadImageToCloudinary(dto.TreatmentProof, "PetHealthBooks");
                }

                if (dto.VetDegree is not null)
                {
                    vetDegreePath = await CloudinaryHelper.UploadImageToCloudinary(dto.VetDegree, "PetHealthBooks");
                }

                var entity = PetHealthBookDetailConversion.ToEntity(dto, proofPath, vetDegreePath);
                entity.HealthBookDetailId = id;
                var response = await _petHealthBookDetail.UpdateAsync(id, entity);

                return response.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return (new Response(false, 500, "Internal Server Error")).ToActionResult(this);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePetHealthBookDetail(Guid id)
        {
            var response = await _petHealthBookDetail.DeleteAsync(id);
            return response.ToActionResult(this);
        }
    }
}
