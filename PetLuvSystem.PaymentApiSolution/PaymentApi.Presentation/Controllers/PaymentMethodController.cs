using Microsoft.AspNetCore.Mvc;
using PaymentApi.Application.DTOs.Conversions;
using PaymentApi.Application.DTOs.PaymentMethodDTOs;
using PaymentApi.Application.Interfaces;
using PetLuvSystem.SharedLibrary.Logs;
using PetLuvSystem.SharedLibrary.Responses;

namespace PaymentApi.Presentation.Controllers
{
    [Route("api/payment-methods")]
    [ApiController]
    public class PaymentMethodController(IPaymentMethod _paymentMethod) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetPaymentMethod([FromQuery] int? pageIndex, [FromQuery] int? pageSize)
        {
            int validPageIndex = pageIndex.GetValueOrDefault(1);
            int validPageSize = pageSize.GetValueOrDefault(10);

            if (validPageIndex <= 0 || validPageSize <= 0)
            {
                return (new Response(false, 400, "PageIndex và PageSize phải lớn hơn 0")).ToActionResult(this);
            }

            var response = await _paymentMethod.GetAllAsync(validPageIndex, validPageSize);
            return response.ToActionResult(this);
        }

        [HttpGet("by-name")]
        public async Task<IActionResult> GetPaymentMethodIdByName([FromQuery] string paymentStatusName)
        {
            var paymentStatus = await _paymentMethod.FindByName(paymentStatusName);

            if (paymentStatus is null)
            {
                return (new Response(false, 404, "Không tìm thấy phương thức thanh toán này")).ToActionResult(this);
            }

            return (new Response(true, 200, "Found")
            {
                Data = paymentStatus.PaymentMethodId
            }).ToActionResult(this);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPaymentMethod(Guid id)
        {
            var response = await _paymentMethod.GetByIdAsync(id);
            return response.ToActionResult(this);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePaymentMethod([FromBody] CreateUpdatePaymentMethodDTO dto)
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
                var response = await _paymentMethod.CreateAsync(PaymentMethodConversion.ToEntity(dto));
                return response.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return (new Response(false, 500, "Internal Server Error")).ToActionResult(this);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePaymentStatus(Guid id, [FromBody] CreateUpdatePaymentMethodDTO dto)
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
                var response = await _paymentMethod.UpdateAsync(id, PaymentMethodConversion.ToEntity(dto));
                return response.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return (new Response(false, 500, "Internal Server Error")).ToActionResult(this);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePaymentMethod(Guid id)
        {
            var response = await _paymentMethod.DeleteAsync(id);
            return response.ToActionResult(this);
        }
    }
}
