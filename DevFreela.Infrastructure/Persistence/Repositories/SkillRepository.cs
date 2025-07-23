using DevFreela.Core.Entities;
using DevFreela.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DevFreela.Infrastructure.Persistence.Repositories
{
    public class SkillRepository : ISkillRepository
    {
        private readonly DevFreelaDbContext _context;

        public SkillRepository(DevFreelaDbContext context)
        {
            _context = context;
        }

        public async Task<int> Add(Skill skill)
        {
            await _context.Skills.AddAsync(skill);
            await _context.SaveChangesAsync();

            return skill.Id;
        }

        public async Task<bool> Exists(int id)
        {
            return await _context.Skills.AnyAsync(s => s.Id == id);
        }

        public async Task<List<Skill>> GetAll(string? search)
        {
            var query = _context.Skills
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var loweredSearch = search.ToLower();
                query = query.Where(s => s.Description.ToLower().Contains(loweredSearch));
            }

            return await query.ToListAsync();
        }

        public async Task<Skill?> GetById(int id)
        {
            var skill = await _context.Skills
                .SingleOrDefaultAsync(s => s.Id == id);

            return skill;
        }

        public async Task<Skill?> GetDetailsById(int id)
        {
            var skill = await _context.Skills
                .SingleOrDefaultAsync(s => s.Id == id);

            return skill;
        }
    }
}
