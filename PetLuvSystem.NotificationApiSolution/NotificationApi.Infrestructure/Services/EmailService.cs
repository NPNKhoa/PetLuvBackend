using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using NotificationApi.Application.DTOs;
using NotificationApi.Application.Interfaces;
using NotificationApi.Infrestructure.Settings;
using PetLuvSystem.SharedLibrary.Logs;

namespace NotificationApi.Infrestructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSettings _smtpSettings;

        public EmailService(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }

        public async Task<bool> SendEmailAsync(SendEmailRequestDTO request)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_smtpSettings.From));
            email.To.Add(MailboxAddress.Parse(request.ToEmail));
            email.Subject = request.Subject;

            var builder = new BodyBuilder();
            builder.HtmlBody = GenerateEmailHtml(request);
            email.Body = builder.ToMessageBody();

            LogException.LogInformation($"[EmailService] Sending email to: {request.ToEmail}");

            try
            {
                using var smtp = new SmtpClient();

                await smtp.ConnectAsync(_smtpSettings.Host, _smtpSettings.Port, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_smtpSettings.Username, _smtpSettings.Password);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);

                LogException.LogInformation($"[Notification Service] Email sent successfully to {request.ToEmail} for Booking {request.BookingId}");

                return true;
            }
            catch (Exception ex)
            {
                LogException.LogError($"[EmailService] Email sending failed: {ex.Message}");
                return false;
            }

        }

        private string GenerateEmailHtml(SendEmailRequestDTO request)
        {
            return $@"
                <!DOCTYPE html>
                <html lang=""en"">
                <head>
                    <meta charset=""UTF-8"">
                    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                    <title>{request.Subject}</title>
                    <style>
                        body {{ font-family: Arial, sans-serif; background-color: #f4f4f4; margin: 0; padding: 0; }}
                        .container {{ max-width: 600px; margin: 0 auto; background: #ffffff; padding: 20px; border-radius: 8px; box-shadow: 0 2px 4px rgba(0,0,0,0.1); }}
                        .header {{ text-align: center; padding: 10px 0; }}
                        .header h1 {{ margin: 0; color: #333333; }}
                        .content {{ padding: 20px 0; }}
                        .content p {{ line-height: 1.6; color: #555555; }}
                        .btn {{ display: inline-block; padding: 12px 20px; background-color: #007BFF; color: #ffffff; text-decoration: none; border-radius: 5px; }}
                        .footer {{ text-align: center; font-size: 12px; color: #aaaaaa; margin-top: 20px; }}
                    </style>
                </head>
                <body>
                    <div class=""container"">
                        <div class=""header"">
                            <h1>Thông báo đặt lịch hẹn</h1>
                        </div>
                        <div class=""content"">
                            <p>Xin chào,</p>
                            <p>{request.BookingId}</p>
                            <p>Để hoàn tất thanh toán, vui lòng nhấn vào nút dưới đây:</p>
                            <p style=""text-align: center;"">
                                <a class=""btn"" href=""{request.PaymentUrl}"">Thanh toán ngay</a>
                            </p>
                            <p>Nếu bạn có bất kỳ thắc mắc nào, vui lòng liên hệ với chúng tôi.</p>
                        </div>
                        <div class=""footer"">
                            <p>© 2025 Booking Service. All rights reserved.</p>
                        </div>
                    </div>
                </body>
                </html>
            ";
        }
    }
}
