using DevFreela.Application.Models;
using DevFreela.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DevFreela.Application.Queries.GetSkillById
{
    public class GetSkillByIdHandler : IRequestHandler<GetSkillByIdQuery, ResultViewModel<SkillViewModel>>
    {
        private readonly DevFreelaDbContext _context;
        public GetSkillByIdHandler(DevFreelaDbContext context)
        {
            _context = context;
        }

        public async Task<ResultViewModel<SkillViewModel>> Handle(GetSkillByIdQuery request, CancellationToken cancellationToken)
        {
            var skill = await _context
                .Skills
                .SingleOrDefaultAsync(s => s.Id == request.Id);

            if (skill is null)
            {
                return ResultViewModel<SkillViewModel>.Failure("Skill not exist.");
            }

            var model = SkillViewModel.FromEntity(skill);

            return ResultViewModel<SkillViewModel>.Success(model);
        }
    }
}
