using FluentValidation.Results;
using MediatR;
using NCD.Core.Messages;
using System;
using System.Threading.Tasks;

namespace NCD.Core.Mediator
{
    public class MediatorHandler : IMediatorHandler
    {
        private readonly IMediator _mediator;

        public MediatorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task PublishEvent<T>(T eventItem) where T : Event
        {
            await _mediator.Publish(eventItem);
        }

        public async Task<ValidationResult> SendCommand<T>(T command) where T : Command
        {
            return await _mediator.Send(command);
        }
    }
}
