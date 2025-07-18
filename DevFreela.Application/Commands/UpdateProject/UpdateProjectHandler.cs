using DevFreela.Application.Models;
using DevFreela.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DevFreela.Application.Commands.UpdateProject
{
    public class UpdateProjectHandler : IRequestHandler<UpdateProjectCommand, ResultViewModel>    
    {
        private readonly DevFreelaDbContext _context;
        public UpdateProjectHandler(DevFreelaDbContext context)
        {
            _context = context;
        }

        public async Task<ResultViewModel> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
        {
            if (request.Title.Length > 50)
            {
                return ResultViewModel<int>.Failure("50 characters maximum for the title.");
            }
            if (request.Description.Length > 200)
            {
                return ResultViewModel<int>.Failure("200 characters maximum for description.");
            }

            var project = await _context.Projects.SingleOrDefaultAsync(p => p.Id == request.IdProject);

            if (project is null)
            {
                return ResultViewModel<int>.Failure("Project not exist.");
            }
            if (project.IsDeleted)
            {
                return ResultViewModel.Failure("Project is deleted.");
            }

            project.Update(request.Title, request.Description, request.TotalCost);

            _context.Projects.Update(project);
            await _context.SaveChangesAsync(cancellationToken);

            return ResultViewModel<int>.Success(project.Id);
        }
    }
}
