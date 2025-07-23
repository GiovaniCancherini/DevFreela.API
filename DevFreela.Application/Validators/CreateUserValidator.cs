using DevFreela.Application.Commands.InsertUser;
using FluentValidation;


namespace DevFreela.Application.Validators
{
    public class CreateUserValidator : AbstractValidator<InsertUserCommand>
    {
        public CreateUserValidator()
        {
            RuleFor(u => u.FullName)
                .NotEmpty()
                    .WithMessage("The user name is required.")
                .MaximumLength(50)
                    .WithMessage("The user name must be at most 50 characters long.");

            RuleFor(u => u.Email)
                .NotEmpty()
                    .WithMessage("The user email is required.")
                .EmailAddress()
                    .WithMessage("The user email must be a valid email address.")
                    .MaximumLength(100)
                    .WithMessage("The user email must be at most 100 characters long.");
            /*
            RuleFor(u => u.Password)
                .NotEmpty()
                    .WithMessage("The user password is required.")
                .MinimumLength(6)
                    .WithMessage("The user password must be at least 6 characters long.")
                .MaximumLength(20)
                    .WithMessage("The user password must be at most 20 characters long.");
            */
            RuleFor(u => u.BirthDate)
                .NotEmpty()
                    .WithMessage("The user birth date is required.")
                .LessThan(DateTime.Now)
                    .WithMessage("The user birth date must be in the past.");
            /*
            RuleFor(u => u.PhoneNumber)
                .NotEmpty()
                    .WithMessage("The user phone number is required.")
                    .Matches(@"^\+?[1-9]\d{1,14}$")
                    .WithMessage("The user phone number must be a valid international phone number format.")
                    .MaximumLength(15)
                    .WithMessage("The user phone number must be at most 15 characters long.");
            */
        }
    }
}
