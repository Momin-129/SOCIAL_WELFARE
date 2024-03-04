namespace SendEmails
{
    public interface IEmailSender
    {
        Task SendSanctionLetter(string email, string subject, string message, byte[] pdfAttachment);
        Task SendOTP(string email, string subject, string message);
    }

}