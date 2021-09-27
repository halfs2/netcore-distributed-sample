using System;

namespace NCD.Core.Messages.Integration
{
    public class UserDeletedIntegrationEvent : IntegrationEvent
    {
        public Guid Id { get; private set; }
        
        public UserDeletedIntegrationEvent(Guid id)
        {
            Id = id;
            AggregateId = id;
        }
    }
}
