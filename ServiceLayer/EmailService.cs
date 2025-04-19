using Core.Identity.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace ServiceLayer
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;
        private readonly string _fromEmail;
        private readonly string _fromName;
        private readonly bool _enableSsl;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
            _smtpServer = _configuration["EmailSettings:SmtpServer"] ?? throw new ArgumentNullException("SmtpServer is not configured");
            _smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"] ?? "587");
            _smtpUsername = _configuration["EmailSettings:SmtpUsername"] ?? throw new ArgumentNullException("SmtpUsername is not configured");
            _smtpPassword = _configuration["EmailSettings:SmtpPassword"] ?? throw new ArgumentNullException("SmtpPassword is not configured");
            _fromEmail = _configuration["EmailSettings:FromEmail"] ?? throw new ArgumentNullException("FromEmail is not configured");
            _fromName = _configuration["EmailSettings:FromName"] ?? "FitGuide";
            _enableSsl = bool.Parse(_configuration["EmailSettings:EnableSsl"] ?? "true");
        }

        public async Task SendEmailConfirmationAsync(string email, string confirmationLink)
        {
            var subject = "Confirm your email";
            var body = new StringBuilder()
                .AppendLine("<h2>Welcome to FitGuide!</h2>")
                .AppendLine("<p>Please confirm your email by clicking the link below:</p>")
                .AppendLine($"<p><a href='{confirmationLink}'>Confirm Email</a></p>")
                .AppendLine("<p>If you didn't create an account with us, please ignore this email.</p>")
                .ToString();

            await SendEmailAsync(email, subject, body);
        }

        public async Task SendPasswordResetEmailAsync(string email, string resetLink)
        {
            var subject = "Reset Your Password";
            var body = new StringBuilder()
                .AppendLine("<h2>Reset Your Password</h2>")
                .AppendLine("<p>You have requested to reset your password. Click the link below to proceed:</p>")
                .AppendLine($"<p><a href='{resetLink}'>Reset Password</a></p>")
                .AppendLine("<p>If you didn't request a password reset, please ignore this email.</p>")
                .ToString();

            await SendEmailAsync(email, subject, body);
        }

        public async Task SendWelcomeEmailAsync(string email, string userName)
        {
            var subject = "Welcome to FitGuide!";
            var body = new StringBuilder()
                .AppendLine($"<h2>Welcome {userName}!</h2>")
                .AppendLine("<p>Thank you for joining FitGuide. We're excited to have you on board!</p>")
                .AppendLine("<p>Start your fitness journey today by exploring our features:</p>")
                .AppendLine("<ul>")
                .AppendLine("<li>Create personalized workout plans</li>")
                .AppendLine("<li>Track your progress</li>")
                .AppendLine("<li>Set and achieve your fitness goals</li>")
                .AppendLine("</ul>")
                .AppendLine("<p>If you have any questions, feel free to contact our support team.</p>")
                .ToString();

            await SendEmailAsync(email, subject, body);
        }

        public async Task SendEmailAsync(string to, string subject, string body, bool isHtml = true)
        {
            try
            {
                using var client = new SmtpClient(_smtpServer, _smtpPort)
                {
                    Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                    EnableSsl = _enableSsl,
                    DeliveryMethod = SmtpDeliveryMethod.Network
                };

                var message = new MailMessage
                {
                    From = new MailAddress(_fromEmail, _fromName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = isHtml,
                    BodyEncoding = Encoding.UTF8,
                    SubjectEncoding = Encoding.UTF8
                };
                message.To.Add(to);

                await client.SendMailAsync(message);
            }
            catch (Exception ex)
            {
                // Log the exception (you should implement proper logging)
                throw new Exception($"Failed to send email: {ex.Message}", ex);
            }
        }
    }
} 