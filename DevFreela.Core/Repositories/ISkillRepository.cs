using DevFreela.Core.Entities;

namespace DevFreela.Core.Repositories
{
    public interface ISkillRepository
    {
        Task<List<Skill>?> GetAll();
        Task<Skill?> GetDetailsById(int id);
        Task<Skill?> GetById(int id);
        Task<int> Add(Skill skill);
        Task<bool> Exists(int id);
    }
}
