using DevFreela.Application.Models;
using DevFreela.Core.Entities;
using DevFreela.Core.Repositories;
using MediatR;

namespace DevFreela.Application.Commands.InsertCommentInProject
{
    public class InsertCommentInProjectHandler : IRequestHandler<InsertCommentInProjectCommand, ResultViewModel<int>>    
    {
        private readonly IProjectRepository _repository;
        private const string PROJECT_NOT_EXIST_MESSAGE = "Project not exist.";

        public InsertCommentInProjectHandler(IProjectRepository repository)
        {
            _repository = repository;
        }

        public async Task<ResultViewModel<int>> Handle(InsertCommentInProjectCommand request, CancellationToken cancellationToken)
        {
            var exists = await _repository.Exists(request.IdProject);

            if (!exists)
            {
                return ResultViewModel<int>.Failure(PROJECT_NOT_EXIST_MESSAGE);
            }

            var comment = new ProjectComment(request.Content, request.IdProject, request.IdUser);
            await _repository.AddComment(comment);

            return ResultViewModel<int>.Success(comment.Id);
        }
    }
}
