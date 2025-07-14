using FluentValidation;
using CQRSExample.Features.GuestRegistrations.Commands;
using System;

namespace CQRSExample.Features.GuestRegistrations.Validators
{
    /// <summary>
    /// Validator for the CreateGuestRegistrationCommand.
    /// </summary>
    public class CreateGuestRegistrationCommandValidator : AbstractValidator<CreateGuestRegistrationCommand>
    {
        public CreateGuestRegistrationCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("Date of Birth is required.")
                .LessThan(DateTime.Now).WithMessage("Date of Birth must be in the past.");

            RuleFor(x => x.PassportNumber)
                .NotEmpty().WithMessage("Passport Number is required.")
                .MaximumLength(50).WithMessage("Passport Number must not exceed 50 characters.");

            RuleFor(x => x.Nationality)
                .NotEmpty().WithMessage("Nationality is required.")
                .MaximumLength(50).WithMessage("Nationality must not exceed 50 characters.");

            RuleFor(x => x.Nrc)
                .MaximumLength(50).WithMessage("NRC must not exceed 50 characters.");

            RuleFor(x => x.CurrentAddress)
                .NotEmpty().WithMessage("Current Address is required.")
                .MaximumLength(200).WithMessage("Current Address must not exceed 200 characters.");

            RuleFor(x => x.PermanentAddress)
                .MaximumLength(200).WithMessage("Permanent Address must not exceed 200 characters.");
        }
    }
}
