using System.Net.Mail;

namespace CompareHare.Api.Features.Shared.Services.Email.Interfaces
{
    public interface IEmailSender
    {
        void SendMailMessage(MailMessage email);
    }
}
