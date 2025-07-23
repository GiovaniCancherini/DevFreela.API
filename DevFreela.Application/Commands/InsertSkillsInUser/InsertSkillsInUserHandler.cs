using DevFreela.Application.Models;
using DevFreela.Core.Entities;
using DevFreela.Infrastructure.Persistence;
using MediatR;

namespace DevFreela.Application.Commands.InsertSkillsInUser
{
    public class InsertSkillsInUserHandler : IRequestHandler<InsertSkillsInUserCommand, ResultViewModel<int>>    
    {
        private readonly DevFreelaDbContext _context;
        public InsertSkillsInUserHandler(DevFreelaDbContext context)
        {
            _context = context;
        }

        public async Task<ResultViewModel<int>> Handle(InsertSkillsInUserCommand request, CancellationToken cancellationToken)
        {
            var userSkills = request.SkillIds
                .Select(idSkill => new UserSkill(request.Id, idSkill))
                .ToList();

            await _context.UserSkills.AddRangeAsync(userSkills);
            await _context.SaveChangesAsync(cancellationToken);

            return ResultViewModel<int>.Success(userSkills.Count);
        }
    }
}
