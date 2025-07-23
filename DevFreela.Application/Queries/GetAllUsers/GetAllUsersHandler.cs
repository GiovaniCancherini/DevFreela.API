using DevFreela.Application.Models;
using DevFreela.Core.Repositories;
using MediatR;

namespace DevFreela.Application.Queries.GetAllUsers
{
    public class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, ResultViewModel<List<UserViewModel>>>
    {
        private readonly IUserRepository _repository;

        public GetAllUsersHandler(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<ResultViewModel<List<UserViewModel>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _repository.GetAll();

            if (users?.Count > 0)
            {
                users = (List<Core.Entities.User>)users
                    .Where(p => request.Search == "" || p.FullName.Contains(request.Search));
            }

            var model = users?
                .Select(UserViewModel.FromEntity)
                .ToList();

            return ResultViewModel<List<UserViewModel>>.Success(model);
        }
    }
}
