using DevFreela.Application.Models;
using MediatR;

namespace DevFreela.Application.Queries.GetAllUsers
{
    public class GetAllUsersQuery : IRequest<ResultViewModel<List<UserViewModel>>>
    {
        public GetAllUsersQuery(string search)
        {
            Search = search;
        }

        public string Search { get; private set; }
    }
}
