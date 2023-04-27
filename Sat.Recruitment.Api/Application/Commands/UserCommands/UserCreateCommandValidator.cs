using FluentValidation;
using Sat.Recruitment.Api.Domain.Enums;
using System;

namespace Sat.Recruitment.Api.Application.Commands.UserCommands
{
    public class UserCreateCommandValidator: AbstractValidator<UserCreateCommand>
    {
        public UserCreateCommandValidator()
        {
            RuleFor((u) => u.Name)
                .NotEmpty().WithMessage("The name is required");
            RuleFor((u) => u.Email)
                .NotEmpty().WithMessage("The email is required")
                .EmailAddress().WithMessage("Invalid email");
            RuleFor((u)=> u.Address)
                .NotEmpty().WithMessage("The address is required");
            RuleFor((u) => u.Phone)
                .NotEmpty().WithMessage("The phone is required");
            RuleFor((u) => u.Money)
                .NotEmpty()
                .GreaterThan(0)
                .ScalePrecision(2,6)
                .WithMessage("Money must have at least 2 decimal places and at most 6 decimal places.");
            RuleFor(u => u.UserType.ToString())
            .IsEnumName(typeof(UserType), caseSensitive: false) // validate that the enum value is a valid name, ignoring case
            .WithMessage("Invalid enum value");
        }
    }
}
