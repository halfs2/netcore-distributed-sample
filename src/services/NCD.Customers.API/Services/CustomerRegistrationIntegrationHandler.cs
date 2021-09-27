using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NCD.Core.Mediator;
using NCD.Core.Messages.Integration;
using NCD.Customers.API.Application.Commands;
using NCD.MessageBus;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NCD.Customers.API.Services
{
    public class CustomerRegistrationIntegrationHandler : BackgroundService
    {
        private readonly IMessageBus _messageBus;
        private readonly IServiceProvider _serviceProvider;

        public CustomerRegistrationIntegrationHandler(IServiceProvider serviceProvider, IMessageBus messageBus) 
        {
            _serviceProvider = serviceProvider;
            _messageBus = messageBus;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _messageBus.RespondAsync<RegisteredUserIntegrationEvent, ResponseMessage>(async request => 
                await RegisterCustomer(request));

            _messageBus.SubscribeAsync<UserDeletedIntegrationEvent>("UserDeleted", async request =>
                await DeleteCustomer(request));

            return Task.CompletedTask;
        }

        private async Task DeleteCustomer(UserDeletedIntegrationEvent request)
        {
            var customerDeleteCommand = new DeleteCustomerCommand(request.Id);

            using (var scope = _serviceProvider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediatorHandler>();
                await mediator.SendCommand(customerDeleteCommand);
            }
        }

        private async Task<ResponseMessage> RegisterCustomer(RegisteredUserIntegrationEvent message)
        {
            var customerCommand = new RegisterCustomerCommand(id: message.Id, name: message.Name, email: message.Email, document: message.Document);
            ValidationResult success;
            using (var scope = _serviceProvider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediatorHandler>();
                success = await mediator.SendCommand(customerCommand);
            }

            return new ResponseMessage(success);
        }
    }


}
