using Microsoft.AspNetCore.Mvc;
using PetApi.Application.Interfaces;
using PetLuvSystem.SharedLibrary.Logs;
using PetLuvSystem.SharedLibrary.Responses;

namespace PetApi.Presentation.Controllers
{
    [Route("api/stats")]
    [ApiController]
    public class StatisticController(IStatistic _statistic) : ControllerBase
    {
        [HttpGet("pet-breeds")]
        public async Task<IActionResult> GetPetBreedRatioByType([FromQuery] string? typeName)
        {
            try
            {
                return (await _statistic.GetPetCountByBreed(typeName)).ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return (new Response(false, 500, "Internal Server Error")).ToActionResult(this);
            }
        }
    }
}
