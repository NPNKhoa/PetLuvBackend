﻿using BookingApi.Application.DTOs.BookingDTOs;
using BookingApi.Application.DTOs.Conversions;
using BookingApi.Application.Interfaces;
using BookingApi.Domain.Entities;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using PetLuvSystem.SharedLibrary.Contracts.Events;
using PetLuvSystem.SharedLibrary.Logs;

namespace BookingApi.Presentation.Controllers
{
    [Route("api/bookings")]
    [ApiController]
    public class BookingController(IBooking _booking, ICheckPetService _checkPetService, IBookingStatus _bookingStatus,
        ICheckCustomerService _checkCustomerService, ICheckPaymentStatusService _checkPaymentStatusService,
        IRoomService _roomService, IServiceService _serviceService, IPublishEndpoint _bus) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllBookings([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var response = await _booking.GetAllAsync(pageIndex, pageSize);
                return response.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingById(Guid id)
        {
            try
            {
                var response = await _booking.GetByIdAsync(id);
                return response.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpGet("/api/users/{id}/bookings")]
        public async Task<IActionResult> GetBookingHistory(Guid id)
        {
            try
            {
                return (await _booking.GetBookingHistory(id)).ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] CreateUpdateBookingDTO dto)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = string.Join("; ", ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage));

                return BadRequest(new PetLuvSystem.SharedLibrary.Responses.Response(false, 400, errorMessages));
            }

            try
            {
                var entity = BookingConversion.ToEntity(dto);

                // Valiadation
                var validator = await _booking.ValidateBookingCreation(entity);
                bool isProcessed = false;

                if (!validator.Flag)
                {
                    return validator.ToActionResult(this);
                }

                if ((await _checkCustomerService.CheckCustomerAsync(dto.CustomerId)) == false)
                {
                    return (new PetLuvSystem.SharedLibrary.Responses.Response(false, 404, "Không tìm thấy khách hàng này")).ToActionResult(this);
                }

                LogException.LogInformation($"[Booking Service] Cho nay ok ne");
                if ((await _checkPetService.CheckPetAsync(dto.PetId)) == false)
                {
                    return (new PetLuvSystem.SharedLibrary.Responses.Response(false, 404, "Không tìm thấy thú cưng này")).ToActionResult(this);
                }

                string estimateTime = string.Empty;
                var totalAmount = 0m;

                // Room Booking
                if (dto.RoomId is not null)
                {
                    LogException.LogInformation($"[Booking Service] Processing Booking with roomid {dto.RoomId}");
                    if (dto.RoomRentalTime is null)
                        return (new PetLuvSystem.SharedLibrary.Responses.Response(false, 400, "Dữ liệu không hợp lệ")).ToActionResult(this);

                    var existingRoom = await _roomService.GetRoomById((Guid)dto.RoomId);

                    if (existingRoom is null || !existingRoom.IsVisible)
                        return (new PetLuvSystem.SharedLibrary.Responses.Response(false, 404, "Phòng cần tìm đã bị xóa hoặc không tồn tại")).ToActionResult(this);

                    entity.RoomBookingItem = new Domain.Entities.RoomBookingItem
                    {
                        BookingId = entity.BookingId,
                        RoomId = (Guid)dto.RoomId,
                        ItemPrice = (dto.RoomRentalTime < 24 ?
                                (decimal)(dto.RoomRentalTime * existingRoom.PricePerHour)
                                : (decimal)((dto.RoomRentalTime / 24m) * existingRoom.PricePerDay))
                    };

                    totalAmount = entity.RoomBookingItem.ItemPrice;
                    estimateTime = dto.RoomRentalTime.ToString()!;
                    isProcessed = true;
                }

                // Service Booking
                if (dto.ServiceId is not null && dto.ServiceId.Any())
                {
                    if (dto.BreedId is null || dto.PetWeightRange is null)
                    {
                        return (new PetLuvSystem.SharedLibrary.Responses.Response(false, 400, "Dữ liệu không hợp lệ, loài hoặc cân nặng rỗng")).ToActionResult(this);
                    }

                    var serviceBookingDetails = new List<ServiceBookingDetail>();
                    int totalTime = 0;
                    LogException.LogInformation($"[Booking Service] Processing Booking with service length {dto.ServiceId.Count()}");

                    var serviceIdList = dto.ServiceId.ToList();
                    var serviceNameList = dto.ServiceNames?.ToList();

                    for (int i = 0; i < serviceIdList.Count; i++)
                    {
                        var serviceId = serviceIdList[i];
                        LogException.LogInformation($"[Booking Service] Processing Booking with service id {serviceId}");

                        var service = await _serviceService.GetServiceVariantByKey(serviceId, (Guid)dto.BreedId, dto.PetWeightRange);

                        if (service is null)
                        {
                            return (new PetLuvSystem.SharedLibrary.Responses.Response(false, 400, "Dữ liệu dịch vụ không hợp lệ")).ToActionResult(this);
                        }

                        LogException.LogInformation($"[Booking Service] Processing service {serviceId} with price {service.Price}");

                        totalTime += service.EstimateTime;
                        totalAmount += service.Price;

                        serviceBookingDetails.Add(new ServiceBookingDetail
                        {
                            ServiceId = serviceId,
                            ServiceItemName = serviceNameList != null && i < serviceNameList.Count ? serviceNameList[i] : string.Empty,
                            BreedId = (Guid)dto.BreedId,
                            PetWeightRange = dto.PetWeightRange,
                            BookingItemPrice = service.Price
                        });
                    }

                    entity.ServiceBookingDetails = serviceBookingDetails;
                    estimateTime = totalTime.ToString();
                    isProcessed = true;
                }

                // Combo Booking
                if (dto.ServiceComboIds is not null && dto.ServiceComboIds.Any())
                {
                    if (dto.BreedId is null || dto.PetWeightRange is null)
                    {
                        return (new PetLuvSystem.SharedLibrary.Responses.Response(false, 400, "Dữ liệu không hợp lệ, loài hoặc cân nặng rỗng")).ToActionResult(this);
                    }

                    var serviceComboBookingDetails = new List<ServiceComboBookingDetail>();
                    LogException.LogInformation($"[Booking Service] Processing Booking with service length {dto.ServiceComboIds.Count()}");

                    var comboIdList = dto.ServiceComboIds.ToList();
                    var serviceNameList = dto.ServiceNames?.ToList();

                    for (int i = 0; i < comboIdList.Count; i++)
                    {
                        var serviceId = comboIdList[i];
                        LogException.LogInformation($"[Booking Service] Processing Booking with service id {serviceId}");

                        var service = await _serviceService.GetServiceComboVariantByKey(serviceId, (Guid)dto.BreedId, dto.PetWeightRange);
                        int totalTime = 0;

                        if (service is not null)
                        {
                            totalTime += service.EstimateTime;
                            totalAmount += service.ComboPrice;

                            serviceComboBookingDetails.Add(new ServiceComboBookingDetail
                            {
                                ServiceComboId = service.ServiceComboId,
                                ServiceComboItemName = serviceNameList != null && i < serviceNameList.Count ? serviceNameList[i] : string.Empty,
                                BreedId = (Guid)dto.BreedId,
                                PetWeightRange = dto.PetWeightRange,
                                BookingId = entity.BookingId,
                                BookingItemPrice = service.ComboPrice
                            });
                        }

                        estimateTime = totalTime.ToString();
                    }

                    entity.ServiceComboBookingDetails = serviceComboBookingDetails;
                    isProcessed = true;
                }


                if (!isProcessed)
                {
                    return (new PetLuvSystem.SharedLibrary.Responses.Response(
                        false, 400, "Có lỗi xảy ra trong quá trình xử lý booking. Vui lòng thử lại sau"
                    )).ToActionResult(this);
                }

                var paymentStatusId = await _checkPaymentStatusService.GetPaymentStatusIdByName("Chờ thanh toán");
                var bookingStatusId = await _bookingStatus.FindBookingStatusIdByName("Đang xử lý");

                if (paymentStatusId == Guid.Empty)
                    return (new PetLuvSystem.SharedLibrary.Responses.Response(false, 404, "Không tìm thấy trạng thái thanh toán yêu cầu")).ToActionResult(this);

                entity.PaymentStatusId = paymentStatusId;
                entity.BookingStatusId = bookingStatusId;
                entity.TotalEstimateTime = estimateTime;
                entity.TotalAmount = totalAmount;
                entity.DepositAmount = 0;

                LogException.LogInformation($"[Booking service] Entity Total Amount: {entity.TotalAmount} - totalAmount: {totalAmount}");

                var response = await _booking.CreateAsync(entity);

                if (response.Flag == true)
                {
                    var ipAddress = HttpContext.Connection.LocalIpAddress?.ToString();

                    if (ipAddress is null)
                    {
                        return (new PetLuvSystem.SharedLibrary.Responses.Response(false, 400, "Không tìm thấy địa chỉ IP. Vui lòng thử lại sau!")).ToActionResult(this);
                    }

                    var bookingCreatedEvent = new BookingCreatedEvent
                    {
                        BookingId = entity.BookingId,
                        CustomerId = entity.CustomerId,
                        CustomerEmail = dto.CustomerEmail,
                        PetId = entity.PetId,
                        TotalPrice = entity.TotalAmount,
                        CreatedAt = DateTime.UtcNow,
                        Description = $"Thanh_Toan_Cho_Dơn_Hang_{entity.BookingId}_So_Tien_{entity.TotalAmount}",
                        IpAddress = ipAddress
                    };

                    await _bus.Publish(bookingCreatedEvent);
                }

                return response.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);

                if (ex.InnerException is not null)
                {
                    LogException.LogError(ex.InnerException.Message);
                }

                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBooking([FromRoute] Guid id, [FromBody] UpdateBookingDTO dto)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = string.Join("; ", ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage));

                return BadRequest(new PetLuvSystem.SharedLibrary.Responses.Response(false, 400, errorMessages));
            }
            try
            {
                var entity = BookingConversion.ToEntity(dto);
                entity.BookingId = id;
                var response = await _booking.UpdateAsync(id, entity);
                return response.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking([FromRoute] Guid id)
        {
            try
            {
                var response = await _booking.DeleteAsync(id);
                return response.ToActionResult(this);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }
    }
}
