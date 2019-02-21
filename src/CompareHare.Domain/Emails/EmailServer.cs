#region usings

using System.Net.Mail;
using System.Threading.Tasks;
using CompareHare.Domain.Emails.Interfaces;
using CompareHare.Domain.Features.Interfaces;

#endregion

namespace CompareHare.Domain.Emails
{
    public class EmailServer : IFeatureService, IEmailServer
    {
        private readonly SmtpClient _smtpClient;

        public EmailServer(SmtpClient smtpClient)
        {
            _smtpClient = smtpClient;
        }

        public async Task SendMailMessage(MailMessage message)
        {
            await _smtpClient.SendMailAsync(message);
        }
    }
}
