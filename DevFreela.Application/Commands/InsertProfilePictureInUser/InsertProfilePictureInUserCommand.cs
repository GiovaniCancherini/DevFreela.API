using DevFreela.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace DevFreela.Application.Commands.InsertProfilePictureInUser
{
    public class InsertProfilePictureInUserCommand : IRequest<ResultViewModel<int>>
    {
        public InsertProfilePictureInUserCommand(int id, IFormFile file)
        {
            Id = id;
            File = file;
        }
        public int Id { get; set; }
        public IFormFile File { get; set; }
    }
}
