#region usings

using System.Net.Mail;
using System.Threading.Tasks;

#endregion

namespace CompareHare.Domain.Emails.Interfaces
{
    public interface IEmailServer
    {
        Task SendMailMessage(MailMessage message);
    }
}
