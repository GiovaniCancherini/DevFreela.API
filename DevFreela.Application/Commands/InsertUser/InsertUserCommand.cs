using DevFreela.Application.Models;
using MediatR;

namespace DevFreela.Application.Commands.InsertUser
{
    public class InsertUserCommand : IRequest<ResultViewModel<int>>
    {
        public InsertUserCommand(string fullName, string email, DateTime birthDate)
        {
            FullName = fullName;
            Email = email;
            BirthDate = birthDate;
        }

        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
