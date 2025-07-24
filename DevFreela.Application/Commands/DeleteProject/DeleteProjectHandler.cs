using DevFreela.Application.Models;
using DevFreela.Core.Repositories;
using MediatR;

namespace DevFreela.Application.Commands.DeleteProject
{
    public class DeleteProjectHandler : IRequestHandler<DeleteProjectCommand, ResultViewModel>
    {
        private readonly IProjectRepository _repository;
        private const string PROJECT_NOT_FOUND_MESSAGE = "Project not found.";
        private const string PROJECT_DELETED_MESSAGE = "Project is deleted.";

        public DeleteProjectHandler(IProjectRepository repository)
        {
            _repository = repository;
        }

        public async Task<ResultViewModel> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
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

            project.SetAsDeleted();
            await _repository.Update(project);

            return ResultViewModel.Success();
        }
    }
}
