using DevFreela.Application.Models;
using DevFreela.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DevFreela.Application.Commands.StartProject
{
    public class StartProjectHandler : IRequestHandler<StartProjectCommand, ResultViewModel>
    {
        private readonly DevFreelaDbContext _context;

        public StartProjectHandler(DevFreelaDbContext context)
        {
            _context = context;
        }

        public async Task<ResultViewModel> Handle(StartProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _context.Projects.SingleOrDefaultAsync(p => p.Id == request.Id);
            
            if (project is null)
            {
                return ResultViewModel.Failure("Project not found.");
            }
            if (project.IsDeleted)
            {
                return ResultViewModel.Failure("Project is deleted.");
            }
            
            project.Start();
            _context.Projects.Update(project);
            await _context.SaveChangesAsync(cancellationToken);
            
            return ResultViewModel.Success();
        }
    }
}
