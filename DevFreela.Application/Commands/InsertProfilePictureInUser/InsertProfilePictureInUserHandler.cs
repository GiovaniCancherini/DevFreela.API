using DevFreela.Application.Models;
using DevFreela.Core.Entities;
using DevFreela.Infrastructure.Persistence;
using MediatR;

namespace DevFreela.Application.Commands.InsertProfilePictureInUser
{
    public class InsertProfilePictureInUserHandler : IRequestHandler<InsertProfilePictureInUserCommand, ResultViewModel<int>>    
    {
        private readonly DevFreelaDbContext _context;
        public InsertProfilePictureInUserHandler(DevFreelaDbContext context)
        {
            _context = context;
        }

        public async Task<ResultViewModel<int>> Handle(InsertProfilePictureInUserCommand request, CancellationToken cancellationToken)
        {
            var description = $"File: {request.File.FileName}, Size: {request.File.Length}";

            // TODO: Processa imagem
            // await _context.Users.AddAsync();
            // await _context.SaveChangesAsync(cancellationToken);

            return ResultViewModel<int>.Success(1);
        }
    }
}
