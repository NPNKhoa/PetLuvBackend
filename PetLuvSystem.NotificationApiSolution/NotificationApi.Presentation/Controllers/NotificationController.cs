using Microsoft.AspNetCore.Mvc;
using NotificationApi.Application.DTOs;
using NotificationApi.Application.Interfaces;

namespace NotificationApi.Presentation.Controllers
{
    [Route("api/notifications")]
    [ApiController]
    public class NotificationController(IEmailService _emailService) : ControllerBase
    {
        [HttpPost("send-email")]
        public async Task<IActionResult> SendEmail([FromBody] SendEmailRequestDTO request)
        {
            bool result = await _emailService.SendEmailAsync(request);
            if (result)
                return Ok(new { Success = true, Message = "Email sent successfully" });
            else
                return StatusCode(500, new { Success = false, Message = "Failed to send email" });
        }
    }
}
