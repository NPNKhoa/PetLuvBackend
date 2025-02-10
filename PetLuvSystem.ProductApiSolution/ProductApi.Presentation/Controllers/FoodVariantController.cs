using Microsoft.AspNetCore.Mvc;
using PetLuvSystem.SharedLibrary.Logs;
using PetLuvSystem.SharedLibrary.Responses;
using ProductApi.Application.DTOs.Conversions;
using ProductApi.Application.DTOs.FoodVariantDTOs;
using ProductApi.Application.Interfaces;

namespace ProductApi.Presentation.Controllers
{
    [Route("api/food-variants")]
    [ApiController]
    public class FoodVariantController(IFoodVariant _foodVariant) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetVariants([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            var response = await _foodVariant.GetAllAsync(pageIndex, pageSize);
            return response.ToActionResult(this);
        }

        [HttpGet("{FoodId}/{FlavorId}/{SizeId}")]
        public async Task<IActionResult> GetVariant(Guid FoodId, Guid FlavorId, Guid SizeId)
        {
            var response = await _foodVariant.GetByKey(FoodId, FlavorId, SizeId);
            return response.ToActionResult(this);
        }

        [HttpPost]
        public async Task<IActionResult> CreateVariant([FromBody] CreateUpdateFoodVariantDTO foodVariantDTO)
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
                var response = await _foodVariant.CreateAsync(FoodVariantConversion.ToEntity(foodVariantDTO));
                return response.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return (new Response(false, 500, "Internal Server Error")).ToActionResult(this);
            }
        }

        [HttpPut("{FoodId}/{FlavorId}/{SizeId}")]
        public async Task<IActionResult> UpdateVariant(Guid FoodId, Guid FlavorId, Guid SizeId, [FromBody] FoodVariantDTO foodVariantDTO)
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
                var response = await _foodVariant.Update(FoodId, FlavorId, SizeId, FoodVariantConversion.ToEntity(foodVariantDTO));
                return response.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return (new Response(false, 500, "Internal Server Error")).ToActionResult(this);
            }
        }

        [HttpDelete("{FoodId}/{FlavorId}/{SizeId}")]
        public async Task<IActionResult> DeleteVariant(Guid FoodId, Guid FlavorId, Guid SizeId)
        {
            var response = await _foodVariant.Delete(FoodId, FlavorId, SizeId);
            return response.ToActionResult(this);
        }
    }
}
