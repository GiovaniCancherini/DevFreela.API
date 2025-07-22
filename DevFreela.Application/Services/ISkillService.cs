using DevFreela.Application.Models;
using DevFreela.Core.Entities;

namespace DevFreela.Application.Services
{
    public interface ISkillService
    {
        ResultViewModel<List<Skill>> GetAll();
        ResultViewModel<Skill> GetById(int id);
        ResultViewModel<int> Insert(CreateSkillInputModel skillInputModel);
    }
}
