using DevFreela.Application.Models;
using DevFreela.Core.Repositories;
using MediatR;

namespace DevFreela.Application.Commands.CancelProject
{
    public class CancelProjectHandler : IRequestHandler<CancelProjectCommand, ResultViewModel>
    {
        private readonly IProjectRepository _repository;
        protected internal const string PROJECT_NOT_FOUND_MESSAGE = "Project not found.";
        protected internal const string PROJECT_DELETED_MESSAGE = "Project is deleted.";

        public CancelProjectHandler(IProjectRepository repository)
        {
            _repository = repository;
        }

        public async Task<ResultViewModel> Handle(CancelProjectCommand request, CancellationToken cancellationToken)
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

            project.Cancel();
            await _repository.Update(project);

            return ResultViewModel.Success();
        }
    }
}
