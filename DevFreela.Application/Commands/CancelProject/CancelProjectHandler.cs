using DevFreela.Application.Models;
using DevFreela.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DevFreela.Application.Commands.CancelProject
{
    public class CancelProjectHandler : IRequestHandler<CancelProjectCommand, ResultViewModel>
    {
        private readonly DevFreelaDbContext _context;

        public CancelProjectHandler(DevFreelaDbContext context)
        {
            _context = context;
        }

        public async Task<ResultViewModel> Handle(CancelProjectCommand request, CancellationToken cancellationToken)
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
            
            project.Cancel();
            _context.Projects.Update(project);
            await _context.SaveChangesAsync(cancellationToken);

            return ResultViewModel.Success();
        }
    }
}
