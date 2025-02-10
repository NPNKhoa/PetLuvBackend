using Microsoft.AspNetCore.Mvc;
using PetLuvSystem.SharedLibrary.Logs;
using PetLuvSystem.SharedLibrary.Responses;
using ProductApi.Application.DTOs.Conversions;
using ProductApi.Application.Interfaces;

namespace ProductApi.Presentation.Controllers
{
    [Route("api/food-sizes")]
    [ApiController]
    public class FoodSizeController(IFoodSize _foodSize) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetFoodSizes([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            var response = await _foodSize.GetAllAsync(pageIndex, pageSize);
            return response.ToActionResult(this);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFoodSize([FromRoute] Guid id)
        {
            var response = await _foodSize.GetByIdAsync(id);
            return response.ToActionResult(this);
        }

        [HttpPost]
        public async Task<IActionResult> CreateFoodSize([FromBody] string foodSize)
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
                var entity = FoodSizeConversion.ToEntity(foodSize);
                var response = await _foodSize.CreateAsync(entity);
                return response.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(false, 500, "Internal Server Error"));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFoodSize([FromRoute] Guid id, [FromBody] string foodSize)
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
                var entity = FoodSizeConversion.ToEntity(foodSize);
                entity.SizeId = id;
                var response = await _foodSize.UpdateAsync(id, entity);
                return response.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(false, 500, "Internal Server Error"));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFoodSize([FromRoute] Guid id)
        {
            var response = await _foodSize.DeleteAsync(id);
            return response.ToActionResult(this);
        }
    }
}
