using DevFreela.Application.Models;
using DevFreela.Infrastructure.Persistence;
using MediatR;

namespace DevFreela.Application.Commands.InsertProject
{
    public class ValidateInsertProjectCommandoBehavior : IPipelineBehavior<InsertProjectCommand, ResultViewModel<int>>
    {
        private readonly DevFreelaDbContext _context;
        public ValidateInsertProjectCommandoBehavior(DevFreelaDbContext context)
        {
            _context = context;
        }

        public async Task<ResultViewModel<int>> Handle(InsertProjectCommand request, RequestHandlerDelegate<ResultViewModel<int>> next, CancellationToken cancellationToken)
        {
            if (request.Title.Length > 50)
            {
                return ResultViewModel<int>.Failure("50 characters maximum for the title.");
            }
            if (request.Description.Length > 200)
            {
                return ResultViewModel<int>.Failure("200 characters maximum for description.");
            }
            if (request.TotalCost <= 0)
            {
                return ResultViewModel<int>.Failure("Total cost must be greater than zero.");
            }

            var clientExists = _context.Users.Any(u => u.Id == request.IdClient);
            var freeLancerExists = _context.Users.Any(u => u.Id == request.IdFreeLancer);
            if (!clientExists || !freeLancerExists)
            {
                return ResultViewModel<int>.Failure("Client or FreeLancer does not exist.");
            }

            return await next();
        }
    }
}
