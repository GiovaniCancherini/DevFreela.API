using brevo_csharp.Api;
using brevo_csharp.Model;
using Microsoft.Extensions.Configuration;

namespace DevFreela.Infrastructure.Notifications
{
    public class EmailService : IEmailService
    {
        private readonly TransactionalEmailsApi _client;
        private readonly string _fromEmail;
        private readonly string _fromName;

        public EmailService(TransactionalEmailsApi client, IConfiguration configuration)
        {
            _client = client;

            _fromEmail = configuration.GetValue<string>("Brevo:FromEmail") ?? throw new InvalidOperationException("Brevo:FromEmail configuration value is missing.");
            _fromName = configuration.GetValue<string>("Brevo:FromName") ?? throw new InvalidOperationException("Brevo:FromName configuration value is missing.");
        }

        public async System.Threading.Tasks.Task SendAsync(string email, string subject, string body)
        {
            var sendSmtpEmail = new SendSmtpEmail(
                to: new List<SendSmtpEmailTo> { new SendSmtpEmailTo(email) },
                sender: new SendSmtpEmailSender(_fromName, _fromEmail),
                subject: subject,
                htmlContent: body
            );

            try
            {
                await _client.SendTransacEmailAsync(sendSmtpEmail);
            }
            catch (Exception ex)
            {
                throw new Exception("Error on sending mail with Brevo.", ex);
            }
        }
    }
}
