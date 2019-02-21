#region usings

using System.Net;
using System.Net.Mail;
using Autofac;
using CompareHare.Domain.Emails.Configuration;

#endregion

namespace CompareHare.Domain.IoC
{
    public class EmailsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(
                ctxt =>
                {
                    var configuration = ctxt.Resolve<SmtpConfiguration>();

                    var client = new SmtpClient(configuration.Host, configuration.Port)
                                 {
                                     DeliveryMethod = SmtpDeliveryMethod.Network,
                                     EnableSsl = configuration.Secure
                                 };

                    if (!string.IsNullOrEmpty(configuration.Username) && !string.IsNullOrEmpty(configuration.Password))
                    {
                        client.Credentials = new NetworkCredential(configuration.Username, configuration.Password);
                    }

                    return client;
                });
        }
    }
}
