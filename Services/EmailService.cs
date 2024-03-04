using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Options;


namespace SendEmails
{
    public class EmailSender : IEmailSender
    {

        private readonly EmailSettings _emailSettings;
        private readonly ILogger<EmailSender> _logger;
        public EmailSender(IOptions<EmailSettings> emailSettings, ILogger<EmailSender> logger)
        {
            _emailSettings = emailSettings.Value;
            _logger = logger;
        }
        public async Task SendSanctionLetter(string email, string subject, string message, byte[] pdfAttachment)
        {

            try
            {
                string? mail = _emailSettings.SenderEmail;
                string? pass = _emailSettings.Password;

                using (var client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(mail, pass),
                    Timeout = 30000
                })
                {
                    var mailMessage = new MailMessage(from: mail!, to: email, subject, message);

                    // Attach the PDF file
                    using (var pdfStream = new MemoryStream(pdfAttachment))
                    {
                        mailMessage.Attachments.Add(new Attachment(pdfStream, "Sanction Letter.pdf", "application/pdf"));
                        await client.SendMailAsync(mailMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception details
                _logger.LogError($"Error sending email: {ex}");
                throw; // Rethrow the exception to propagate it
            }


        }

        public async Task SendOTP(string email, string subject, string message)
        {
            try
            {
                string? mail = _emailSettings.SenderEmail;
                string? pass = _emailSettings.Password;

                using (var client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(mail, pass),
                    Timeout = 30000
                })
                {
                    var mailMessage = new MailMessage(from: mail!, to: email, subject, message);
                    await client.SendMailAsync(mailMessage);

                }
            }
            catch (Exception ex)
            {
                // Log the exception details
                _logger.LogError($"Error sending email: {ex}");
                throw; // Rethrow the exception to propagate it
            }
        }
    }
}