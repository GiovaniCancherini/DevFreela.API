using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Configuration;

namespace DevFreela.Infrastructure.Notifications
{
    public class EmailService : IEmailService
    {
        private readonly ISendGridClient _client;
        private readonly string _fromEmail;
        private readonly string _fromName;

        public EmailService(ISendGridClient client, IConfiguration configuration)
        {
            _client = client;

            _fromEmail = configuration.GetValue<string>("SendGrid:FromEmail") ?? throw new InvalidOperationException("SendGrid:FromEmail configuration value is missing.");
            _fromName = configuration.GetValue<string>("SendGrid:FromName") ?? throw new InvalidOperationException("SendGrid:FromName configuration value is missing.");
        }

        public Task SendAsync(string email, string subject, string body)
        {
            var sendGridMessage = new SendGridMessage
            {
                From = new EmailAddress(_fromEmail, _fromName),
                Subject = subject
            };

            sendGridMessage.AddContent(MimeType.Text, body);
            sendGridMessage.AddTo(new EmailAddress(email));

            var response = _client.SendEmailAsync(sendGridMessage);

            return Task.CompletedTask;
        }
    }
}
