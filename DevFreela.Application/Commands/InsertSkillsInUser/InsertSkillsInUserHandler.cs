using DevFreela.Application.Models;
using DevFreela.Core.Entities;
using DevFreela.Core.Repositories;
using MediatR;

namespace DevFreela.Application.Commands.InsertSkillsInUser
{
    public class InsertSkillsInUserHandler : IRequestHandler<InsertSkillsInUserCommand, ResultViewModel<int>>    
    {
        private readonly IUserRepository _repository;

        public InsertSkillsInUserHandler(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<ResultViewModel<int>> Handle(InsertSkillsInUserCommand request, CancellationToken cancellationToken)
        {
            var userSkills = request.SkillIds
                .Select(idSkill => new UserSkill(request.Id, idSkill))
                .ToList();

            await _repository.AddSkills(userSkills);

            return ResultViewModel<int>.Success(userSkills.Count);
        }
    }
}
