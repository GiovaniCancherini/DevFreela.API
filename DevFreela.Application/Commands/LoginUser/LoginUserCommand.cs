using DevFreela.Application.Models;
using MediatR;

namespace DevFreela.Application.Commands.LoginUser
{
    public class LoginUserCommand : IRequest<ResultViewModel>
    {
        public LoginUserCommand(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
