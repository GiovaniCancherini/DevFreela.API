using DevFreela.Application.Models;
using DevFreela.Core.Entities;
using DevFreela.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DevFreela.Application.Commands.InsertCommentInProject
{
    public class InsertCommentInProjectHandler : IRequestHandler<InsertCommentInProjectCommand, ResultViewModel<int>>    
    {
        private readonly DevFreelaDbContext _context;
        public InsertCommentInProjectHandler(DevFreelaDbContext context)
        {
            _context = context;
        }

        public async Task<ResultViewModel<int>> Handle(InsertCommentInProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _context.Projects.SingleOrDefaultAsync(p => p.Id == request.IdProject);

            if (project is null)
            {
                return ResultViewModel<int>.Failure("Project not found.");
            }
            if (project.IsDeleted)
            {
                return ResultViewModel<int>.Failure("Project is deleted.");
            }

            var comment = new ProjectComment(request.Content, request.IdProject, request.IdUser);

            await _context.ProjectComments.AddAsync(comment);
            await _context.SaveChangesAsync(cancellationToken);

            return ResultViewModel<int>.Success(comment.Id);

        }
    }
}
