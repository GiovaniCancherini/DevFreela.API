using DevFreela.Application.Models;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace DevFreela.Application.Commands.ValidateRecoveryCode
{
    public class ValidateRecoveryCodeHandler : IRequestHandler<ValidateRecoveryCodeCommand, ResultViewModel>
    {
        private readonly IMemoryCache _cache;

        public ValidateRecoveryCodeHandler(
            IMemoryCache cache
        )
        {
            _cache = cache;
        }

        public async Task<ResultViewModel> Handle(ValidateRecoveryCodeCommand command, CancellationToken cancellationToken)
        {
            var cacheKey = $"PasswordRecoveryCode:{command.Email}";
            
            if (!_cache.TryGetValue<string>(cacheKey, out string? existingCode) || existingCode != command.Code)
            {
                return ResultViewModel.Failure("Invalid recovery code.");
            }   

            return ResultViewModel.Success();
        }
    }
}
