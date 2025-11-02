namespace neuro_kinetic_backend.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body, bool isHtml = true);
        Task SendCollaborationRequestEmailAsync(string contactEmail, string institutionName, string contactName);
        Task SendCollaborationResponseEmailAsync(string contactEmail, string institutionName, string status, string? responseNotes);
        Task SendUserRegistrationEmailAsync(string email, string firstName);
    }
}

