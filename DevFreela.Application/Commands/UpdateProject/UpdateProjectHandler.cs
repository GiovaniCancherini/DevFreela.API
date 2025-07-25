using DevFreela.Application.Models;
using DevFreela.Core.Repositories;
using MediatR;

namespace DevFreela.Application.Commands.UpdateProject
{
    public class UpdateProjectHandler : IRequestHandler<UpdateProjectCommand, ResultViewModel>    
    {
        private readonly IProjectRepository _repository;
        protected internal const string PROJECT_NOT_FOUND_MESSAGE = "Project not found.";
        protected internal const string PROJECT_DELETED_MESSAGE = "Project is deleted.";
        protected internal const string PROJECT_TITLE_MAX_50_MESSAGE = "50 characters maximum for the title.";
        protected internal const string PROJECT_DESCRIPTION_MAX_200_MESSAGE = "200 characters maximum for description.";

        public UpdateProjectHandler(IProjectRepository repository)
        {
            _repository = repository;
        }

        public async Task<ResultViewModel> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
        {
            if (request.Title.Length > 50)
            {
                return ResultViewModel.Failure(PROJECT_TITLE_MAX_50_MESSAGE);
            }
            if (request.Description.Length > 200)
            {
                return ResultViewModel.Failure(PROJECT_DESCRIPTION_MAX_200_MESSAGE);
            }

            var project = await _repository.GetById(request.IdProject);

            if (project is null)
            {
                return ResultViewModel.Failure(PROJECT_NOT_FOUND_MESSAGE);
            }
            if (project.IsDeleted)
            {
                return ResultViewModel.Failure(PROJECT_DELETED_MESSAGE);
            }

            project.Update(request.Title, request.Description, request.TotalCost);

            await _repository.Update(project);

            return ResultViewModel.Success();
        }
    }
}
