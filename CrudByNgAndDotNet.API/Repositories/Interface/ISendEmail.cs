using CrudByNgAndDotNet.API.Models.Domain;
using CrudByNgAndDotNet.API.Models.DTO;

namespace CrudByNgAndDotNet.API.Repositories.Interface
{
    public interface ISendEmail
    {
        Task SendEmailAsync(MailRequest mailRequest);
        Task ForgotPasswordSendEmailAsync(Message message);
    }
}