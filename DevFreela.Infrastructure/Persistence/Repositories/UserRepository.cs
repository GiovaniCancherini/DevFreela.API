using DevFreela.Core.Entities;
using DevFreela.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DevFreela.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DevFreelaDbContext _context;

        public UserRepository(DevFreelaDbContext context)
        {
            _context = context;
        }

        public async Task<int> Add(User User)
        {
            await _context.Users.AddAsync(User);
            await _context.SaveChangesAsync();

            return User.Id;
        }

        public async Task AddProfilePicture(User user)
        {
            // TODO: implement the logic to add a profile picture
            // await _context.UserProfilePicture.AddAsync(user);
            // await _context.SaveChangesAsync();
        }

        public async Task AddSkills(List<UserSkill> skills)
        {
            await _context.UserSkills.AddRangeAsync(skills);
            await _context.SaveChangesAsync();
        }
        
        public async Task<bool> Exists(int id)
        {
            return await _context.Users.AnyAsync(p => p.Id == id);
        }

        public async Task<List<User>> GetAll(string? search)
        {
            var query = _context.Users
                .Include(u => u.Skills)
                    .ThenInclude(us => us.Skill)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var loweredSearch = search.ToLower();
                query = query.Where(u => u.FullName.ToLower().Contains(loweredSearch));
            }

            return await query.ToListAsync();
        }

        public async Task<User?> GetById(int id)
        {
            var user = await _context.Users
                .SingleOrDefaultAsync(u => u.Id == id);

            return user;
        }

        public async Task<User?> GetByLogin(string email, string password)
        {
            var user = await _context.Users
                .Include(u => u.Skills)
                    .ThenInclude(us => us.Skill)
                .SingleOrDefaultAsync(u => u.Email == email && u.Password == password);

            return user;
        }

        public async Task<User?> GetDetailsById(int id)
        {
            var user = await _context.Users
                .Include(u => u.Skills)
                    .ThenInclude(us => us.Skill)
                .SingleOrDefaultAsync(u => u.Id == id);

            return user;
        }

        public async Task Login(User user)
        {
            // TODO: Implement the logic to login a user
        }

        public async Task Update(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
