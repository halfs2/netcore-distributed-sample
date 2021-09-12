using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NCD.Customers.API.Application.Events
{
    public class CustomerEventHandler : INotificationHandler<RegisteredCustomerEvent>
    {
        public Task Handle(RegisteredCustomerEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
