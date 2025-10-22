using System;
using System.Threading.Tasks;

namespace DevFreela.Infrastructure.Notifications
{
    public interface IEmailService
    {
        Task SendAsync(string email, string subject, string body);
    }
}
