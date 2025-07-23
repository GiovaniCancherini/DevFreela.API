using DevFreela.Application.Models;
using DevFreela.Core.Repositories;
using MediatR;

namespace DevFreela.Application.Queries.GetAllSkills
{
    public class GetAllSkillsHandler : IRequestHandler<GetAllSkillsQuery, ResultViewModel<List<SkillViewModel>>>
    {
        private readonly ISkillRepository _repository;

        public GetAllSkillsHandler(ISkillRepository repository)
        {
            _repository = repository;
        }

        public async Task<ResultViewModel<List<SkillViewModel>>> Handle(GetAllSkillsQuery request, CancellationToken cancellationToken)
        {
            var skills = await _repository.GetAll();
            if (skills?.Count > 0)
            {
                skills = (List<Core.Entities.Skill>)skills
                    .Where(s => request.Search == "" || s.Description.Contains(request.Search));
            }

            var model = skills?
                .Select(SkillViewModel.FromEntity)
                .ToList();

            return ResultViewModel<List<SkillViewModel>>.Success(model);
        }
    }
}
