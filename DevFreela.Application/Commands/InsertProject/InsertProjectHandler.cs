using DevFreela.Application.Models;
using DevFreela.Infrastructure.Persistence;
using MediatR;

namespace DevFreela.Application.Commands.InsertProject
{
    public class InsertProjectHandler : IRequestHandler<InsertProjectCommand, ResultViewModel>    
    {
        private readonly DevFreelaDbContext _context;
        public InsertProjectHandler(DevFreelaDbContext context)
        {
            _context = context;
        }

        public async Task<ResultViewModel> Handle(InsertProjectCommand request, CancellationToken cancellationToken)
        {
            if (request.Title.Length > 50)
            {
                return ResultViewModel<int>.Failure("50 characters maximum for the title.");
            }
            if (request.Description.Length > 200)
            {
                return ResultViewModel<int>.Failure("200 characters maximum for description.");
            }

            var project = request.ToEntity();

            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync(cancellationToken);

            return ResultViewModel<int>.Success(project.Id);
        }
    }
}
