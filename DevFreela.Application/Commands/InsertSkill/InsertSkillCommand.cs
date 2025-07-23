using DevFreela.Application.Models;
using MediatR;

namespace DevFreela.Application.Commands.InsertSkill
{
    public class InsertSkillCommand : IRequest<ResultViewModel<int>>
    {
        public InsertSkillCommand(string decription)
        {
            Decription = decription;
        }
        public string Decription { get; set; }
    }
}
