using DevFreela.Application.Models;
using DevFreela.Core.Repositories;
using DevFreela.Infrastructure.Notifications;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace DevFreela.Application.Commands.PasswordRecoveryRequest
{
    public class PasswordRecoveryRequestHandler : IRequestHandler<PasswordRecoveryRequestCommand, ResultViewModel>
    {
        private readonly IUserRepository _repository;
        private readonly IEmailService _emailService;
        private readonly IMemoryCache _cache;

        public PasswordRecoveryRequestHandler(
            IUserRepository repository,
            IEmailService emailService,
            IMemoryCache cache
        )
        {
            _repository = repository;
            _emailService = emailService;
            _cache = cache;
        }

        public async Task<ResultViewModel> Handle(PasswordRecoveryRequestCommand command, CancellationToken cancellationToken)
        {
            var user = await _repository.GetByEmail(command.Email);
            if (user is null)
            {
                return ResultViewModel<int>.Failure("");
            }

            var code = new Random().Next(100000, 999999).ToString();

            var cacheKey = $"PasswordRecoveryCode:{command.Email}";

            _cache.Set(cacheKey, code, TimeSpan.FromMinutes(10));

            await _emailService.SendAsync(
                command.Email,
                "Password Recovery Code",
                $"Your password recovery code is: {code}"
            );

            return ResultViewModel.Success();
        }
    }
}
