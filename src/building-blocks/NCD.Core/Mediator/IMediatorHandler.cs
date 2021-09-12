using FluentValidation.Results;
using NCD.Core.Messages;
using System.Threading.Tasks;

namespace NCD.Core.Mediator
{
    public interface IMediatorHandler
    {
        Task PublishEvent<T>(T eventItem) where T : Event;

        Task<ValidationResult> SendCommand<T>(T command) where T : Command;
           
    }
}
