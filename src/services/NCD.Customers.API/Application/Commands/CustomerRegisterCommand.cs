using FluentValidation;
using NCD.Core.Messages;
using NCD.Customers.API.Model.ValueObjects;
using System;

namespace NCD.Customers.API.Application.Commands
{
    public class CustomerRegisterCommand : Command
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Document { get; private set; }

        public CustomerRegisterCommand(Guid id, string name, string email, string document)
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

    public class CustomerRegisterValidation : AbstractValidator<CustomerRegisterCommand>
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
                .Must(TerEmailValido)
                .WithMessage("The email is not valid");
        }

        protected static bool TerEmailValido(string email)
        {
            return Email.IsValid(email);
        }
    }
}
