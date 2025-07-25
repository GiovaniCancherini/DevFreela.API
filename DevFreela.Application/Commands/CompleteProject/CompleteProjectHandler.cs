using DevFreela.Application.Models;
using DevFreela.Core.Repositories;
using MediatR;

namespace DevFreela.Application.Commands.CompleteProject
{
    public class CompleteProjectHandler : IRequestHandler<CompleteProjectCommand, ResultViewModel>
    {
        private readonly IProjectRepository _repository;
        protected internal const string PROJECT_NOT_FOUND_MESSAGE = "Project not found.";
        protected internal const string PROJECT_DELETED_MESSAGE = "Project is deleted.";

        public CompleteProjectHandler(IProjectRepository repository)
        {
            _repository = repository;
        }

        public async Task<ResultViewModel> Handle(CompleteProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _repository.GetById(request.Id);
            
            if (project is null)
            {
                return ResultViewModel.Failure(PROJECT_NOT_FOUND_MESSAGE);
            }
            if (project.IsDeleted)
            {
                return ResultViewModel.Failure(PROJECT_DELETED_MESSAGE);
            }
            
            project.Complete();
            await _repository.Update(project);

            return ResultViewModel.Success();
        }
    }
}
