using DevFreela.Application.Models;
using DevFreela.Core.Entities;
using DevFreela.Core.Repositories;
using DevFreela.Infrastructure.Auth;
using MediatR;

namespace DevFreela.Application.Commands.InsertUser
{
    public class InsertUserHandler : IRequestHandler<InsertUserCommand, ResultViewModel<int>>
    {
        private readonly IUserRepository _repository;
        private readonly IAuthService _auth;

        public InsertUserHandler(IUserRepository repository, IAuthService auth)
        {
            _repository = repository;
            _auth = auth;
        }

        public async Task<ResultViewModel<int>> Handle(InsertUserCommand request, CancellationToken cancellationToken)
        {
            var hash = _auth.ComputeSha256Hash(request.Password);

            var user = new User(request.FullName, request.Email, request.BirthDate, hash, request.Role);

            var idUserCreated = await _repository.Add(user);

            return ResultViewModel<int>.Success(idUserCreated);
        }
    }
}
