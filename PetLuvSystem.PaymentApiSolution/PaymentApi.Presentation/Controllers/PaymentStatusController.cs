using Microsoft.AspNetCore.Mvc;
using PaymentApi.Application.DTOs.Conversions;
using PaymentApi.Application.DTOs.PaymentStatusDTOs;
using PaymentApi.Application.Interfaces;
using PetLuvSystem.SharedLibrary.Logs;
using PetLuvSystem.SharedLibrary.Responses;

namespace PaymentApi.Presentation.Controllers
{
    [Route("api/payment-status")]
    [ApiController]
    public class PaymentStatusController(IPaymentStatus _paymentStatus) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetPaymentStatuses([FromQuery] int pageIndex, [FromQuery] int pageSize)
        {
            var response = await _paymentStatus.GetAllAsync(pageIndex, pageSize);
            return response.ToActionResult(this);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPaymentStatus(Guid id)
        {
            var response = await _paymentStatus.GetByIdAsync(id);
            return response.ToActionResult(this);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePaymentStatus([FromBody] CreateUpdatePaymentStatusDTO dto)
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
                var response = await _paymentStatus.CreateAsync(PaymentStatusConversion.ToEntity(dto));
                return response.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return (new Response(false, 500, "Internal Server Error")).ToActionResult(this);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePaymentStatus(Guid id, [FromBody] CreateUpdatePaymentStatusDTO dto)
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
                var response = await _paymentStatus.UpdateAsync(id, PaymentStatusConversion.ToEntity(dto));
                return response.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return (new Response(false, 500, "Internal Server Error")).ToActionResult(this);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePaymentStatus(Guid id)
        {
            var response = await _paymentStatus.DeleteAsync(id);
            return response.ToActionResult(this);
        }
    }
}
