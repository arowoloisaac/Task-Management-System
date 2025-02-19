namespace Project_Manager.ExternalServices.EmailService
{
    public interface IEmailService
    {
        Task<string> SendEmail(string subject, string content, string receiver);
    }
}
