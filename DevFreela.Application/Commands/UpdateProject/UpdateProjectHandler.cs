using DevFreela.Application.Models;
using DevFreela.Core.Repositories;
using DevFreela.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DevFreela.Application.Commands.UpdateProject
{
    public class UpdateProjectHandler : IRequestHandler<UpdateProjectCommand, ResultViewModel<int>>    
    {
        private readonly IProjectRepository _repository;

        public UpdateProjectHandler(IProjectRepository repository)
        {
            _repository = repository;
        }

        public async Task<ResultViewModel<int>> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
        {
            if (request.Title.Length > 50)
            {
                return ResultViewModel<int>.Failure("50 characters maximum for the title.");
            }
            if (request.Description.Length > 200)
            {
                return ResultViewModel<int>.Failure("200 characters maximum for description.");
            }

            var project = await _repository.GetById(request.IdProject);

            if (project is null)
            {
                return ResultViewModel<int>.Failure("Project not exist.");
            }
            if (project.IsDeleted)
            {
                return ResultViewModel<int>.Failure("Project is deleted.");
            }

            project.Update(request.Title, request.Description, request.TotalCost);

            await _repository.Update(project);

            return ResultViewModel<int>.Success(project.Id);
        }
    }
}
