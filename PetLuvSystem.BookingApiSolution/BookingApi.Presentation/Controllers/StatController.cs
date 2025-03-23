using BookingApi.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PetLuvSystem.SharedLibrary.Logs;
using PetLuvSystem.SharedLibrary.Responses;

namespace BookingApi.Presentation.Controllers
{
    [Route("api/stats/bookings")]
    [ApiController]
    public class StatController(IStatistic _statistic) : ControllerBase
    {
        [HttpGet("services")]
        public async Task<IActionResult> GetServicesBookedAsync([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, [FromQuery] int? month, [FromQuery] int? year)
        {
            try
            {
                return (await _statistic.GetServicesBookedAsync(startDate, endDate, month, year)).ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return (new Response(false, 500, "Internal Server Error")).ToActionResult(this);
            }
        }

        [HttpGet("breeds")]
        public async Task<IActionResult> GetBreedsBookedAsync([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, [FromQuery] int? month, [FromQuery] int? year)
        {
            try
            {
                return (await _statistic.GetBreedsBookedAsync(startDate, endDate, month, year)).ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return (new Response(false, 500, "Internal Server Error")).ToActionResult(this);
            }
        }

        [HttpGet("revenue")]
        public async Task<IActionResult> GetRevenue([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, [FromQuery] int? month, [FromQuery] int? year)
        {
            try
            {
                return (await _statistic.GetRevenue(startDate, endDate, month, year)).ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return (new Response(false, 500, "Internal Server Error")).ToActionResult(this);
            }
        }
    }
}
