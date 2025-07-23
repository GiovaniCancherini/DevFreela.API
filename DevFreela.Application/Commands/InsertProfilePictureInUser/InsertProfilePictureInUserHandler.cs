using DevFreela.Application.Models;
using DevFreela.Core.Repositories;
using MediatR;

namespace DevFreela.Application.Commands.InsertProfilePictureInUser
{
    public class InsertProfilePictureInUserHandler : IRequestHandler<InsertProfilePictureInUserCommand, ResultViewModel<int>>    
    {
        private readonly IProjectRepository _repository;

        public InsertProfilePictureInUserHandler(IProjectRepository repository)
        {
            _repository = repository;
        }

        public async Task<ResultViewModel<int>> Handle(InsertProfilePictureInUserCommand request, CancellationToken cancellationToken)
        {
            var description = $"File: {request.File.FileName}, Size: {request.File.Length}";

            // TODO: Processa imagem
            // await _repository.Add...;

            return ResultViewModel<int>.Success(1);
        }
    }
}
