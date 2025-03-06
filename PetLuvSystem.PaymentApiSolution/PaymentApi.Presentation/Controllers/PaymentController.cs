using MassTransit;
using Microsoft.AspNetCore.Mvc;
using PaymentApi.Application.DTOs.Conversions;
using PaymentApi.Application.DTOs.PaymentDTOs;
using PaymentApi.Application.Interfaces;
using PaymentApi.Domain.Entities;
using PetLuvSystem.SharedLibrary.Contracts.Events;
using PetLuvSystem.SharedLibrary.Logs;
using VNPAY.NET;
using VNPAY.NET.Enums;
using VNPAY.NET.Models;
using VNPAY.NET.Utilities;

namespace PaymentApi.Presentation.Controllers
{
    [Route("api/payments")]
    [ApiController]
    public class PaymentController(IVnpay _vnpay, IConfiguration _configuration, IPublishEndpoint _bus,
        IPaymentStatus _paymentStatus, IPaymentMethod _paymentMethod, IPayment _payment) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetPayments([FromQuery] int? pageIndex = 1, [FromQuery] int? pageSize = 10)
        {
            int validPageIndex = pageIndex.GetValueOrDefault(1);
            int validPageSize = pageSize.GetValueOrDefault(10);

            if (validPageIndex <= 0 || validPageSize <= 0)
            {
                return (new PetLuvSystem.SharedLibrary.Responses.Response(false, 400, "PageIndex và PageSize phải lớn hơn 0")).ToActionResult(this);
            }

            var response = await _payment.GetAllAsync(validPageIndex, validPageSize);
            return response.ToActionResult(this);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPayment(Guid id)
        {
            var response = await _payment.GetByIdAsync(id);
            return response.ToActionResult(this);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePayment([FromBody] CreateUpdatePaymentDTO dto)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = string.Join("; ", ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage));

                return (new PetLuvSystem.SharedLibrary.Responses.Response(false, 400, errorMessages)).ToActionResult(this);
            }

            try
            {
                var response = await _payment.CreateAsync(PaymentConversion.ToEntity(dto));
                return response.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return (new PetLuvSystem.SharedLibrary.Responses.Response(false, 500, "Internal Server Error")).ToActionResult(this);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePayment(Guid id, [FromBody] CreateUpdatePaymentDTO dto)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = string.Join("; ", ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage));

                return (new PetLuvSystem.SharedLibrary.Responses.Response(false, 400, errorMessages)).ToActionResult(this);
            }

            try
            {
                var response = await _payment.UpdateAsync(id, PaymentConversion.ToEntity(dto));
                return response.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return (new PetLuvSystem.SharedLibrary.Responses.Response(false, 500, "Internal Server Error")).ToActionResult(this);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment(Guid id)
        {
            var response = await _payment.DeleteAsync(id);
            return response.ToActionResult(this);
        }

        [HttpGet("create-payment-url")]
        public async Task<IActionResult> CreatePaymentUrl([FromQuery] Guid bookingId, [FromQuery] Guid userId, [FromQuery] double money, [FromQuery] string description)
        {
            try
            {
                var ipAddress = NetworkHelper.GetIpAddress(HttpContext); // Lấy địa chỉ IP của thiết bị thực hiện giao dịch

                var transactionRef = DateTime.UtcNow.Ticks.ToString();

                var request = new PaymentRequest
                {
                    PaymentId = long.Parse(transactionRef),
                    Money = money,
                    Description = description,
                    IpAddress = ipAddress,
                    BankCode = BankCode.ANY, // Tùy chọn. Mặc định là tất cả phương thức giao dịch
                    CreatedDate = DateTime.Now, // Tùy chọn. Mặc định là thời điểm hiện tại
                    Currency = Currency.VND, // Tùy chọn. Mặc định là VND (Việt Nam đồng)
                    Language = DisplayLanguage.Vietnamese // Tùy chọn. Mặc định là tiếng Việt
                };

                var paymentUrl = _vnpay.GetPaymentUrl(request);

                var paymentStatus = await _paymentStatus.FindByName("Chờ thanh toán");

                if (paymentStatus is null)
                {
                    return (new PetLuvSystem.SharedLibrary.Responses.Response(false, 404, "Không tìm thấy trạng thái thanh toán")).ToActionResult(this);
                }

                var paymentMethod = await _paymentMethod.FindByName("Thanh toán qua VNPay");

                if (paymentMethod is null)
                {
                    return (new PetLuvSystem.SharedLibrary.Responses.Response(false, 404, "Không tìm thấy trạng thái thanh toán")).ToActionResult(this);
                }

                var currentTime = DateTime.UtcNow;

                var payment = new Payment
                {
                    PaymentId = Guid.NewGuid(),
                    Amount = (decimal)money,
                    TransactionRef = transactionRef,
                    CreatedAt = currentTime,
                    UpdatedTime = currentTime,
                    OrderId = bookingId,
                    UserId = userId,
                    PaymentMethodId = paymentMethod.PaymentMethodId,
                    PaymentStatusId = paymentStatus.PaymentStatusId,
                };

                await _payment.CreateAsync(payment);

                return Created(paymentUrl, paymentUrl);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("IpnAction")]
        public async Task<IActionResult> IpnAction()
        {
            if (Request.QueryString.HasValue)
            {
                try
                {
                    var paymentResult = _vnpay.GetPaymentResult(Request.Query);
                    var payment = await _payment.FindByRef(paymentResult.PaymentId.ToString());


                    if (paymentResult.IsSuccess)
                    {
                        // Update Payment Status
                        var paymentStatus = await _paymentStatus.FindByName("Đã đặt cọc");

                        if (paymentStatus is null)
                        {
                            return (new PetLuvSystem.SharedLibrary.Responses.Response(false, 500, "Không tìm thấy paymnet status")).ToActionResult(this);
                        }

                        payment.PaymentStatusId = paymentStatus.PaymentStatusId;
                        await _payment.UpdateAsync(payment.PaymentId, payment);

                        // Publish the PaymentCompletedEvent
                        await _bus.Publish(
                            new PaymentCompletedEvent
                            {
                                BookingId = payment.OrderId,
                                PaidAt = paymentResult.Timestamp
                            }
                        );

                        return Ok();
                    }

                    // Thực hiện hành động nếu thanh toán thất bại tại đây. Ví dụ: Hủy đơn hàng.

                    await _bus.Publish(
                        new PaymentRejectedEvent
                        {
                            BookingId = payment.OrderId,
                            FailedAt = paymentResult.Timestamp,
                            Reason = paymentResult.PaymentResponse.Description
                        }
                    );

                    return BadRequest("Thanh toán thất bại");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            return NotFound("Không tìm thấy thông tin thanh toán.");
        }

        [HttpGet("Callback")]
        public ActionResult<PaymentResult> Callback()
        {
            if (Request.QueryString.HasValue)
            {
                try
                {
                    var paymentResult = _vnpay.GetPaymentResult(Request.Query);

                    if (paymentResult.IsSuccess)
                    {
                        return Ok(paymentResult);
                    }

                    return BadRequest(paymentResult);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            return NotFound("Không tìm thấy thông tin thanh toán.");
        }
    }
}
