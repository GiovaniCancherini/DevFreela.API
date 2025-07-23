using DevFreela.Application.Models;
using DevFreela.Core.Repositories;
using MediatR;

namespace DevFreela.Application.Commands.CancelProject
{
    public class CancelProjectHandler : IRequestHandler<CancelProjectCommand, ResultViewModel>
    {
        private readonly IProjectRepository _repository;

        public CancelProjectHandler(IProjectRepository repository)
        {
            _repository = repository;
        }

        public async Task<ResultViewModel> Handle(CancelProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _repository.GetById(request.Id);

            if (project is null)
            {
                return ResultViewModel.Failure("Project not found.");
            }
            if (project.IsDeleted)
            {
                return ResultViewModel.Failure("Project is deleted.");
            }
            
            project.Cancel();
            await _repository.Update(project);

            return ResultViewModel.Success();
        }
    }
}
