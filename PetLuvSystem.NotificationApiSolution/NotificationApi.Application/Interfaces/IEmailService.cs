using NotificationApi.Application.DTOs;

namespace NotificationApi.Application.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(SendEmailRequestDTO request);
    }
}
