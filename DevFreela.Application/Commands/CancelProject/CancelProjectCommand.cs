using DevFreela.Application.Models;
using MediatR;

namespace DevFreela.Application.Commands.CancelProject
{
    public class CancelProjectCommand : IRequest<ResultViewModel>
    {
        public CancelProjectCommand(int id)
        {
            Id = id;
        }

        public int Id { get; set; }
    }
}
