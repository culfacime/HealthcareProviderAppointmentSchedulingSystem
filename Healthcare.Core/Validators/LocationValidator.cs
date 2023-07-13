using FluentValidation;
using Healthcare.Core.Entities;

namespace Healthcare.Core.Validators
{
    public class LocationValidator : AbstractValidator<Location>
    {
        public LocationValidator()
        {
            RuleFor(x => x.LocationName)
                .NotEmpty().NotNull()
                .WithMessage("Location name is required.");

        }
    }   

}
