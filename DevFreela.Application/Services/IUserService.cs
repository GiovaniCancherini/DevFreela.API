using DevFreela.Application.Models;
using Microsoft.AspNetCore.Http;

namespace DevFreela.Application.Services
{
    public interface IUserService
    {
        ResultViewModel<List<UserViewModel>> GetAll(string search = "");
        ResultViewModel<UserViewModel> GetById(int id);
        ResultViewModel<int> Insert(CreateUserInputModel model);
        ResultViewModel InsertSkills(int id, UserSkillInputModel model);
        ResultViewModel InsertProfilePicture(int id, IFormFile file);
        ResultViewModel Login(int id, LoginModel model);
    }
}
