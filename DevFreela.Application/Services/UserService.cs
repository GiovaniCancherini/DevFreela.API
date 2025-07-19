using DevFreela.Application.Models;
using DevFreela.Core.Entities;
using DevFreela.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace DevFreela.Application.Services
{
    internal class UserService : IUserService
    {
        private readonly DevFreelaDbContext _context;

        public UserService(DevFreelaDbContext context)
        {
            _context = context;
        }

        public ResultViewModel<List<UserViewModel>> GetAll(string search = "")
        {
            var users = _context
                .Users
                .Include(u => u.Skills)
                    .ThenInclude(us => us.Skill)
                .Where(p => search == "" || p.FullName.Contains(search))
                .ToList();

            var model = users
                .Select(UserViewModel.FromEntity)
                .ToList();

            return ResultViewModel<List<UserViewModel>>.Success(model);
        }
        public ResultViewModel<UserViewModel> GetById(int id)
        {
            var user = _context.Users
                .Include(u => u.Skills)
                    .ThenInclude(us => us.Skill)
                .SingleOrDefault(u => u.Id == id);

            if (user is null)
            {
                return ResultViewModel<UserViewModel>.Failure("User not exist.");
            }

            var model = UserViewModel.FromEntity(user);

            return ResultViewModel<UserViewModel>.Success(model);
        }
        public ResultViewModel<int> Insert(CreateUserInputModel model)
        {
            var user = new User(model.FullName, model.Email, model.BirthDate);

            _context.Users.Add(user);
            _context.SaveChanges();

            return ResultViewModel<int>.Success(user.Id);
        }
        public ResultViewModel InsertSkills(int id, UserSkillInputModel model)
        {
            var userSkills = model.SkillIds
                .Select(idSkill => new UserSkill(id, idSkill))
                .ToList();

            _context.UserSkills.AddRange(userSkills);
            _context.SaveChanges();

            return ResultViewModel<int>.Success(userSkills.Count);
        }
        public ResultViewModel InsertProfilePicture(int id, IFormFile file)
        {
            var description = $"File: {file.FileName}, Size: {file.Length}";

            // TODO: Processa imagem

            return ResultViewModel.Success();
        }
        public ResultViewModel Login(int id, LoginModel model)
        {
            // TODO: Implementar o login do usuário
            return ResultViewModel.Success();
        }
    }
}
