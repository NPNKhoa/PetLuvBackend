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
                body {{ font-family: 'Segoe UI', Arial, sans-serif; background-color: #f7f9fc; margin: 0; padding: 20px; color: #333; }}
                .container {{ max-width: 600px; margin: 0 auto; background: #ffffff; padding: 30px; border-radius: 12px; box-shadow: 0 4px 12px rgba(0,0,0,0.05); }}
                .logo {{ text-align: center; margin-bottom: 20px; }}
                .logo img {{ max-height: 60px; }}
                .header {{ text-align: center; padding: 10px 0 20px; border-bottom: 1px solid #eaeaea; margin-bottom: 25px; }}
                .header h1 {{ margin: 0; color: #2c3e50; font-size: 24px; font-weight: 600; }}
                .content {{ padding: 0 10px 20px; }}
                .content p {{ line-height: 1.7; color: #4a4a4a; font-size: 16px; margin-bottom: 15px; }}
                .booking-info {{ background-color: #f8f9fa; padding: 15px; border-radius: 8px; margin: 20px 0; border-left: 4px solid #f79400; }}
                .booking-info p {{ margin: 5px 0; }}
                .btn {{ display: inline-block; padding: 14px 30px; background-color: #f79400; color: #f8f9fa; text-decoration: none; border-radius: 6px; font-weight: bold; font-size: 16px; transition: background-color 0.3s; }}
                .btn:hover {{ background-color: #ffb347; }}
                .cta-section {{ text-align: center; margin: 30px 0; }}
                .footer {{ text-align: center; font-size: 14px; color: #888; margin-top: 30px; padding-top: 20px; border-top: 1px solid #eaeaea; }}
                .social-links {{ margin: 15px 0; }}
                .social-links a {{ display: inline-block; margin: 0 10px; color: #007BFF; text-decoration: none; }}
                .contact-info {{ margin-top: 10px; font-size: 13px; }}
                @media only screen and (max-width: 480px) {{
                    .container {{ padding: 20px; }}
                    .header h1 {{ font-size: 20px; }}
                }}
            </style>
        </head>
        <body>
            <div class=""container"">
                <div class=""header"">
                    <h1>Thông Báo Đặt Lịch Hẹn</h1>
                </div>
                <div class=""content"">
                    <p>Xin chào Quý khách,</p>
                    <p>Cảm ơn Quý khách đã sử dụng dịch vụ đặt lịch của chúng tôi. Dưới đây là thông tin đặt lịch của Quý khách:</p>
                    
                    <div class=""booking-info"">
                        <p><strong>Mã đặt lịch:</strong> {request.BookingId}</p>
                    </div>
                    
                    <p>Để hoàn tất quá trình đặt lịch, vui lòng thanh toán để xác nhận lịch hẹn của Quý khách.</p>
                    
                    <div class=""cta-section"">
                        <a class=""btn"" href=""{request.PaymentUrl}"">Thanh Toán Ngay</a>
                    </div>
                    
                    <p>Lưu ý: Đường dẫn thanh toán này sẽ hết hạn sau 24 giờ.</p>
                    
                    <p>Nếu Quý khách có bất kỳ thắc mắc hoặc cần hỗ trợ, vui lòng liên hệ với chúng tôi qua email <a href=""mailto:support@petluv.com"">support@petluv.com</a> hoặc số điện thoại <a href=""tel:+84916380593"">0916 380 593</a>.</p>
                    
                    <p>Trân trọng,<br>Đội ngũ PetLuv</p>
                </div>
            </div>
        </body>
        </html>
    ";
        }

    }
}
