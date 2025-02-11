using Microsoft.AspNetCore.Mvc;
using PetLuvSystem.SharedLibrary.Logs;
using PetLuvSystem.SharedLibrary.Responses;
using UserApi.Application.DTOs.Conversions;
using UserApi.Application.DTOs.WorkScheduleDTOs;
using UserApi.Application.Interfaces;

namespace UserApi.Presentation.Controllers
{
    [Route("api/work-schedules")]
    [ApiController]
    public class WorkScheduleController(IWorkSchedule _schedule) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllSchedules([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            var response = await _schedule.GetAllAsync(pageIndex, pageSize);
            return response.ToActionResult(this);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetScheduleById(Guid id)
        {
            var response = await _schedule.GetByIdAsync(id);
            return response.ToActionResult(this);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSchedule([FromBody] CreateUpdateWorkScheduleDTO dto)
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
                var entity = WorkScheduleConversion.ToEntity(dto);
                var response = await _schedule.CreateAsync(entity);
                return response.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return (new Response(false, 500, "Internal Server Error")).ToActionResult(this);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSchedule(Guid id, [FromBody] CreateUpdateWorkScheduleDTO dto)
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
                var entity = WorkScheduleConversion.ToEntity(dto);
                var response = await _schedule.UpdateAsync(id, entity);
                return response.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return (new Response(false, 500, "Internal Server Error")).ToActionResult(this);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSchedule(Guid id)
        {
            var response = await _schedule.DeleteAsync(id);
            return response.ToActionResult(this);
        }
    }
}
