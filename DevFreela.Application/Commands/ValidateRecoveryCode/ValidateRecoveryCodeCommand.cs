using DevFreela.Application.Models;
using MediatR;

namespace DevFreela.Application.Commands.ValidateRecoveryCode
{
    public class ValidateRecoveryCodeCommand : IRequest<ResultViewModel>
    {
        public ValidateRecoveryCodeCommand(string email, string code)
        {
            Email = email;
            Code = code;
        }

        public string Email { get; set; }
        public string Code { get; set; }
    }
}
