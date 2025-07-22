using DevFreela.Application.Models;
using DevFreela.Core.Entities;
using DevFreela.Infrastructure.Persistence;

namespace DevFreela.Application.Services
{
    public class SkillService : ISkillService
    {
        private readonly DevFreelaDbContext _context;

        public SkillService(DevFreelaDbContext context)
        {
            _context = context;
        }

        public ResultViewModel<List<Skill>> GetAll()
        {
            var skills = _context.Skills.ToList();

            return ResultViewModel<List<Skill>>.Success(skills);
        }

        public ResultViewModel<Skill> GetById(int id)
        {
            var skill = _context.Skills
                .SingleOrDefault(p => p.Id == id);

            if (skill is null)
            {
                return ResultViewModel<Skill>.Failure("Skill not exist.");
            }

            return ResultViewModel<Skill>.Success(skill);
        }

        public ResultViewModel<int> Insert(CreateSkillInputModel model)
        {
            var skill = new Skill(model.Decription);

            _context.Skills.Add(skill);
            _context.SaveChanges();

            return ResultViewModel<int>.Success(skill.Id);
        }
    }
}
