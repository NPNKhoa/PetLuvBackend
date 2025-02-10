using Microsoft.AspNetCore.Mvc;
using PetLuvSystem.SharedLibrary.Logs;
using PetLuvSystem.SharedLibrary.Responses;
using ProductApi.Application.DTOs.Conversions;
using ProductApi.Application.Interfaces;

namespace ProductApi.Presentation.Controllers
{
    [Route("api/food-flavors")]
    [ApiController]
    public class FoodFlavorController(IFoodFlavor _foodFlavor) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetFlavors([FromQuery] int pageIndex = 1, int pageSize = 10)
        {
            var response = await _foodFlavor.GetAllAsync(pageIndex, pageSize);
            return response.ToActionResult(this);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFlavor(Guid id)
        {
            var response = await _foodFlavor.GetByIdAsync(id);
            return response.ToActionResult(this);
        }

        [HttpPost]
        public async Task<IActionResult> CreateFlavor([FromBody] string flavor)
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
                var entity = FoodFlavorConversion.ToEntity(flavor);
                var response = await _foodFlavor.CreateAsync(entity);
                return response.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(500, new Response(false, 500, "Internal Server Error"));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFlavor(Guid id, [FromBody] string flavor)
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
                var entity = FoodFlavorConversion.ToEntity(flavor);
                entity.FlavorId = id;
                var response = await _foodFlavor.UpdateAsync(id, entity);
                return response.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(500, new Response(false, 500, "Internal Server Error"));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFlavor(Guid id)
        {
            var response = await _foodFlavor.DeleteAsync(id);
            return response.ToActionResult(this);
        }
    }
}
