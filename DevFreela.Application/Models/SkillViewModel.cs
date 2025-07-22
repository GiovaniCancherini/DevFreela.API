using DevFreela.Core.Entities;

namespace DevFreela.Application.Models
{
    public class SkillViewModel
    {
        public SkillViewModel(int id, string description)
        {
            Id = id;
            Description = description;
        }

        public int Id { get; private set; }
        public string Description { get; private set; }

        public static SkillViewModel FromEntity(Skill skill)
            => new(skill.Id, skill.Description);
    }
}
