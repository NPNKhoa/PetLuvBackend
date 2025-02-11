using Microsoft.AspNetCore.Mvc;
using PetLuvSystem.SharedLibrary.Logs;
using PetLuvSystem.SharedLibrary.Responses;
using UserApi.Application.DTOs.Conversions;
using UserApi.Application.DTOs.WorkScheduleDetailDTOs;
using UserApi.Application.Interfaces;

namespace UserApi.Presentation.Controllers
{
    [Route("api/work-schedule-details")]
    [ApiController]
    public class WorkScheduleDetailController(IWorkScheduleDetail _workScheduleDetail) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllWorkScheduleDetails([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            var response = await _workScheduleDetail.GetAllAsync(pageIndex, pageSize);
            return response.ToActionResult(this);
        }

        [HttpGet("{workScheduleId}/{workingDate}")]
        public async Task<IActionResult> GetWorkScheduleDetailByKey(Guid workScheduleId, DateTime workingDate)
        {
            var response = await _workScheduleDetail.GetByKeyAsync(workingDate, workScheduleId);
            return response.ToActionResult(this);
        }

        [HttpPost]
        public async Task<IActionResult> CreateWorkScheduleDetail([FromBody] WorkScheduleDetailDTO dto)
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
                var entity = WorkScheduleDetailConversion.ToEntity(dto);
                var response = await _workScheduleDetail.CreateAsync(entity);
                return response.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return (new Response(false, 500, "Internal Server Error")).ToActionResult(this);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWorkScheduleDetail(Guid id, [FromBody] WorkScheduleDetailDTO dto)
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
                var entity = WorkScheduleDetailConversion.ToEntity(dto);
                var response = await _workScheduleDetail.UpdateAsync(id, entity);
                return response.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return (new Response(false, 500, "Internal Server Error")).ToActionResult(this);
            }
        }
    }
}
