using DevFreela.Application.Models;
using DevFreela.Infrastructure.Persistence;
using MediatR;

namespace DevFreela.Application.Commands.LoginUser
{
    public class LoginUserHandler : IRequestHandler<LoginUserCommand, ResultViewModel>    
    {
        private readonly DevFreelaDbContext _context;
        public LoginUserHandler(DevFreelaDbContext context)
        {
            _context = context;
        }

        public async Task<ResultViewModel> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            // TODO: Implementar o login do usuário
            return ResultViewModel.Success();
        }
    }
}
