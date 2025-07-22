using DevFreela.Application.Models;
using MediatR;

namespace DevFreela.Application.Commands.InsertCommentInProject
{
    public class InsertCommentInProjectCommand : IRequest<ResultViewModel<int>>
    {
        public InsertCommentInProjectCommand(string content, int idProject, int idUser)
        {
            Content = content;
            IdProject = idProject;
            IdUser = idUser;
        }

        public string Content { get; set; }
        public int IdProject { get; set; }
        public int IdUser { get; set; }
    }
}
