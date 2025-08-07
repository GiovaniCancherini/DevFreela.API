using DevFreela.Application.Models;
using DevFreela.Core.Repositories;
using DevFreela.Infrastructure.Auth;
using MediatR;

namespace DevFreela.Application.Commands.LoginUser
{
    public class LoginUserHandler : IRequestHandler<LoginUserCommand, ResultViewModel>    
    {
        private readonly IUserRepository _repository;
        private readonly IAuthService _auth;
        protected internal const string LOGIN_ERROR_MESSAGE = "Login Error.";

        public LoginUserHandler(IUserRepository repository, IAuthService auth)
        {
            _repository = repository;
            _auth = auth;
        }

        public async Task<ResultViewModel> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var hash = _auth.ComputeSha256Hash(request.Password);

            var user = await _repository.GetByLogin(request.Email, hash);

            if (user is null)
            {
                return ResultViewModel<int>.Failure(LOGIN_ERROR_MESSAGE);
            }

            var token = _auth.GenerateJwtToken(user.Email, user.Role);

            var viewModel = new LoginViewModel(token);

            return ResultViewModel<LoginViewModel>.Success(viewModel);
        }
    }
}
