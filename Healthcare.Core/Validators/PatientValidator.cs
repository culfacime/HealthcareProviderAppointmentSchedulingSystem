using FluentValidation;
using Healthcare.Core.Entities;

namespace Healthcare.Core.Validators
{
    public class PatientValidator : AbstractValidator<Patient>
    {
        public PatientValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().NotNull()
                .WithMessage("Name is required.");    
            
            RuleFor(x => x.Surname)
                .NotEmpty().NotNull()
                .WithMessage("Surname is required.");         

            RuleFor(x => x.DateOfBirth)
                .NotEmpty().NotNull()
                .WithMessage("Date of birth is required.");

            RuleFor(x => x.Email)
                .NotEmpty().NotNull()
                .WithMessage("Email is required.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().NotNull()
                .WithMessage("Phone number is required.");

        }
    }

}
