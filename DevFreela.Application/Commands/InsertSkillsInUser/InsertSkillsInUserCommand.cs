using DevFreela.Application.Models;
using MediatR;

namespace DevFreela.Application.Commands.InsertSkillsInUser
{
    public class InsertSkillsInUserCommand : IRequest<ResultViewModel<int>>
    {
        public InsertSkillsInUserCommand(int[] skillIds, int id)
        {
            SkillIds = skillIds;
            Id = id;
        }
        public int[] SkillIds { get; set; }
        public int Id { get; set; }
    }
}
