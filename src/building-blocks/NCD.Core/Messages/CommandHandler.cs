using FluentValidation.Results;
using NCD.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCD.Core.Messages
{
    public abstract class CommandHandler
    {
        protected ValidationResult ValidationResult;

        protected CommandHandler()
        {
            ValidationResult = new ValidationResult();
        }

        protected void AddError(string message)
        {
            ValidationResult.Errors.Add(new ValidationFailure(string.Empty, message));
        }

        protected async Task<ValidationResult> PersistData(IUnitOfWork uow)
        {
            if (!await uow.Commit()) AddError("An error occurred while persisting data");

            return ValidationResult;
        }
    }
}
