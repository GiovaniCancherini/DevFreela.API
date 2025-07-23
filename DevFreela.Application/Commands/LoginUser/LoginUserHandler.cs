using DevFreela.Application.Models;
using DevFreela.Core.Repositories;
using MediatR;

namespace DevFreela.Application.Commands.LoginUser
{
    public class LoginUserHandler : IRequestHandler<LoginUserCommand, ResultViewModel>    
    {
        private readonly IUserRepository _repository;

        public LoginUserHandler(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<ResultViewModel> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            // TODO: Implementar o login do usuário
            // await _repository.Login(new User(request.UserName, request.Password));

            return ResultViewModel.Success();
        }
    }
}
