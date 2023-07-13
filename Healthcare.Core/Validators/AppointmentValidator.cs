using FluentValidation;
using Healthcare.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Healthcare.Core.Validators
{

    public class AppointmentValidator : AbstractValidator<Appointment>
    {
        public AppointmentValidator()
        {
            RuleFor(x => x.PatientId)
                .NotEmpty()
                .WithMessage("Patient id is required.");

                RuleFor(x => x.LocationId)
                .NotEmpty()
                .WithMessage("Location id is required.");

            RuleFor(x => x.AppointmentDate)
                .NotEmpty()
                .WithMessage("Appointment date is required.");

            RuleFor(x => x.RemindingDate)
                .LessThanOrEqualTo(x => x.AppointmentDate)
                .WithMessage("Reminding time must be lower than appointment time");
        }
    }

}
