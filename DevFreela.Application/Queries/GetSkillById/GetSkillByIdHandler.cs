using DevFreela.Application.Models;
using DevFreela.Core.Repositories;
using MediatR;

namespace DevFreela.Application.Queries.GetSkillById
{
    public class GetSkillByIdHandler : IRequestHandler<GetSkillByIdQuery, ResultViewModel<SkillViewModel>>
    {
        private readonly ISkillRepository _repository;
        private const string SKILL_NOT_EXIST_MESSAGE = "Skill not exist.";

        public GetSkillByIdHandler(ISkillRepository repository)
        {
            _repository = repository;
        }

        public async Task<ResultViewModel<SkillViewModel>> Handle(GetSkillByIdQuery request, CancellationToken cancellationToken)
        {
            var skill = await _repository.GetDetailsById(request.Id);

            if (skill is null)
            {
                return ResultViewModel<SkillViewModel>.Failure(SKILL_NOT_EXIST_MESSAGE);
            }

            var model = SkillViewModel.FromEntity(skill);

            return ResultViewModel<SkillViewModel>.Success(model);
        }
    }
}
