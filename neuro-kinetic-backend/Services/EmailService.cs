using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace neuro_kinetic_backend.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;
        
        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        
        public async Task SendEmailAsync(string to, string subject, string body, bool isHtml = true)
        {
            try
            {
                var smtpSettings = _configuration.GetSection("SmtpSettings");
                var smtpHost = smtpSettings["Host"];
                var smtpPort = int.Parse(smtpSettings["Port"] ?? "587");
                var smtpUser = smtpSettings["Username"];
                var smtpPassword = smtpSettings["Password"];
                var fromEmail = smtpSettings["FromEmail"] ?? "noreply@neurokinetic.com";
                var fromName = smtpSettings["FromName"] ?? "Neuro-Kinetic Research Platform";
                
                // If SMTP is not configured, log and skip sending (for development)
                if (string.IsNullOrEmpty(smtpHost))
                {
                    _logger.LogWarning("SMTP not configured. Email would be sent to: {To}, Subject: {Subject}", to, subject);
                    _logger.LogInformation("Email body: {Body}", body);
                    return;
                }
                
                using var client = new SmtpClient(smtpHost, smtpPort)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(smtpUser, smtpPassword)
                };
                
                using var message = new MailMessage
                {
                    From = new MailAddress(fromEmail, fromName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = isHtml
                };
                
                message.To.Add(to);
                
                await client.SendMailAsync(message);
                _logger.LogInformation("Email sent successfully to: {To}", to);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email to: {To}", to);
                // Don't throw - allow application to continue even if email fails
            }
        }
        
        public async Task SendCollaborationRequestEmailAsync(string contactEmail, string institutionName, string contactName)
        {
            var subject = "Collaboration Request Received - Neuro-Kinetic Research Platform";
            var body = $@"
                <html>
                <body>
                    <h2>Thank you for your collaboration interest!</h2>
                    <p>Dear {contactName},</p>
                    <p>We have received your collaboration request from <strong>{institutionName}</strong>.</p>
                    <p>Our team will review your proposal and get back to you soon.</p>
                    <p>Best regards,<br/>Neuro-Kinetic Research Team</p>
                </body>
                </html>";
            
            await SendEmailAsync(contactEmail, subject, body);
            
            // Also notify admin (optional)
            var adminEmail = _configuration["AdminEmail"];
            if (!string.IsNullOrEmpty(adminEmail))
            {
                var adminSubject = $"New Collaboration Request from {institutionName}";
                var adminBody = $@"
                    <html>
                    <body>
                        <h2>New Collaboration Request</h2>
                        <p><strong>Institution:</strong> {institutionName}</p>
                        <p><strong>Contact:</strong> {contactName}</p>
                        <p><strong>Email:</strong> {contactEmail}</p>
                        <p>Please review in the admin panel.</p>
                    </body>
                    </html>";
                
                await SendEmailAsync(adminEmail, adminSubject, adminBody);
            }
        }
        
        public async Task SendCollaborationResponseEmailAsync(string contactEmail, string institutionName, string status, string? responseNotes)
        {
            var statusText = status == "Approved" ? "approved" : status == "Rejected" ? "declined" : "under review";
            var subject = $"Collaboration Request {statusText} - Neuro-Kinetic Research Platform";
            var body = $@"
                <html>
                <body>
                    <h2>Collaboration Request Update</h2>
                    <p>Dear representative of <strong>{institutionName}</strong>,</p>
                    <p>Your collaboration request has been <strong>{statusText}</strong>.</p>
                    {(string.IsNullOrEmpty(responseNotes) ? "" : $"<p><strong>Response:</strong> {responseNotes}</p>")}
                    <p>Best regards,<br/>Neuro-Kinetic Research Team</p>
                </body>
                </html>";
            
            await SendEmailAsync(contactEmail, subject, body);
        }
        
        public async Task SendUserRegistrationEmailAsync(string email, string firstName)
        {
            var subject = "Welcome to Neuro-Kinetic Research Platform";
            var body = $@"
                <html>
                <body>
                    <h2>Welcome to Neuro-Kinetic!</h2>
                    <p>Dear {firstName},</p>
                    <p>Thank you for registering with the Neuro-Kinetic Research Platform.</p>
                    <p>You can now access research publications, performance metrics, and collaborate with our research team.</p>
                    <p>Best regards,<br/>Neuro-Kinetic Research Team</p>
                </body>
                </html>";
            
            await SendEmailAsync(email, subject, body);
        }
    }
}

