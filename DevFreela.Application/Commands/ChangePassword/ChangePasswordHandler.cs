using DevFreela.Application.Models;
using DevFreela.Core.Repositories;
using DevFreela.Infrastructure.Auth;
using DevFreela.Infrastructure.Notifications;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace DevFreela.Application.Commands.ChangePassword
{
    public class ChangePasswordHandler : IRequestHandler<ChangePasswordCommand, ResultViewModel>
    {
        private readonly IUserRepository _repository;
        private readonly IEmailService _emailService;
        private readonly IMemoryCache _cache;
        private readonly IAuthService _auth;

        public ChangePasswordHandler(
            IUserRepository repository,
            IEmailService emailService,
            IMemoryCache cache,
            IAuthService auth
        )
        {
            _repository = repository;
            _emailService = emailService;
            _cache = cache;
            _auth = auth;
        }

        public async Task<ResultViewModel> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var cacheKey = $"PasswordRecoveryCode:{request.Email}";
            
            if (!_cache.TryGetValue<string>(cacheKey, out string? existingCode) || existingCode != request.Code)
            {
                return ResultViewModel.Failure("Invalid recovery code.");
            }   

            _cache.Remove(cacheKey);

            var user = await _repository.GetByEmail(request.Email);
            if (user is null)
            {
                return ResultViewModel.Failure("User not found.");

            }

            var hash = _auth.ComputeSha256Hash(request.NewPassword);

            var updated = await _repository.UpdatePassword(user.Id, hash);
            if (!updated)
            {
                return ResultViewModel.Failure("Error updating password.");
            }

            return ResultViewModel.Success();
        }
    }
}
