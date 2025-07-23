using DevFreela.Application.Models;
using DevFreela.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DevFreela.Application.Queries.GetAllProjects
{
    public class GetAllProjectsHandler : IRequestHandler<GetAllProjectsQuery, ResultViewModel<List<ProjectItemViewModel>>>
    {
        private readonly DevFreelaDbContext _context;
        public GetAllProjectsHandler(DevFreelaDbContext context)
        {
            _context = context;
        }

        public async Task<ResultViewModel<List<ProjectItemViewModel>>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
        {
            var projects = await _context.Projects
                .Include(p => p.Client)
                .Include(p => p.Freelancer)
                .Include(p => p.Comments)
                .Where(p => !p.IsDeleted && (request.Search== "" || p.Title.Contains(request.Search)))
                .Skip(request.Page * request.Size)
                .Take(request.Size)
                .ToListAsync();

            var model = projects
                .Select(ProjectItemViewModel.FromEntity)
                .ToList();

            return ResultViewModel<List<ProjectItemViewModel>>.Success(model);
        }
    }
}
