using DevFreela.Application.Models;
using MediatR;

namespace DevFreela.Application.Commands.PasswordRecoveryRequest
{
    public class PasswordRecoveryRequestCommand : IRequest<ResultViewModel>
    {
        public PasswordRecoveryRequestCommand(string email)
        {
            Email = email;
        }

        public string Email { get; set; }
    }
}
