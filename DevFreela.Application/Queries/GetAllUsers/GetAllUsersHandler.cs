using DevFreela.Application.Models;
using DevFreela.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DevFreela.Application.Queries.GetAllUsers
{
    public class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, ResultViewModel<List<UserViewModel>>>
    {
        private readonly DevFreelaDbContext _context;
        public GetAllUsersHandler(DevFreelaDbContext context)
        {
            _context = context;
        }

        public async Task<ResultViewModel<List<UserViewModel>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _context
                .Users
                .Include(u => u.Skills)
                    .ThenInclude(us => us.Skill)
                .Where(p => request.Search == "" || p.FullName.Contains(request.Search))
                .ToListAsync();

            var model = users
                .Select(UserViewModel.FromEntity)
                .ToList();

            return ResultViewModel<List<UserViewModel>>.Success(model);
        }
    }
}
