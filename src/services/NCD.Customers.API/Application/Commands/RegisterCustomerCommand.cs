using FluentValidation;
using NCD.Core.Messages;
using NCD.Customers.API.Model.ValueObjects;
using System;

namespace NCD.Customers.API.Application.Commands
{
    public class RegisterCustomerCommand : Command
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Document { get; private set; }

        public RegisterCustomerCommand(Guid id, string name, string email, string document)
        {
            AggregateId = id;
            Id = id;
            Name = name;
            Email = email;
            Document = document;
        }

        public override bool IsValid()
        {
            ValidationResult = new CustomerRegisterValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class CustomerRegisterValidation : AbstractValidator<RegisterCustomerCommand>
    {
        public CustomerRegisterValidation()
        {
            RuleFor(c => c.Id)
                .NotEqual(Guid.Empty);

            RuleFor(c => c.Name)
                .NotEmpty();

            RuleFor(c => c.Document)
                .NotEmpty();

            RuleFor(c => c.Email)
                .Must(HaveValidEmail)
                .WithMessage("The email is not valid");
        }

        protected static bool HaveValidEmail(string email)
        {
            return Email.IsValid(email);
        }
    }
}
