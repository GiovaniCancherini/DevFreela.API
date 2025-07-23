using DevFreela.Application.Commands.InsertProject;
using FluentValidation;


namespace DevFreela.Application.Validators
{
    public class InsertProjectValidator : AbstractValidator<InsertProjectCommand>
    {
        public InsertProjectValidator()
        {
            RuleFor(p => p.Title)
                .NotEmpty()
                    .WithMessage("The project title is required.")
                .MaximumLength(50)
                    .WithMessage("The project title must be at most 50 characters long.");

            RuleFor(p => p.Description)
                .NotEmpty()
                    .WithMessage("The project description is required.")
                .MaximumLength(500)
                    .WithMessage("The project description must be at most 500 characters long.");

            RuleFor(p => p.TotalCost)
                .GreaterThan(0)
                    .WithMessage("The total cost of the project must be greater than zero.");

            RuleFor(p => p.IdClient)
                .GreaterThan(0)
                    .WithMessage("The client ID is required and must be greater than zero.");

            RuleFor(p => p.IdFreeLancer)
                .GreaterThan(0)
                    .WithMessage("The freelancer ID is required and must be greater than zero.");

        }
    }
}
