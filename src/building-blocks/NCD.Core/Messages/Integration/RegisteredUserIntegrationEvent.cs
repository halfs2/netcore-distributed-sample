using System;

namespace NCD.Core.Messages.Integration
{
    public class RegisteredUserIntegrationEvent : IntegrationEvent
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Document { get; set; }

        public RegisteredUserIntegrationEvent(Guid id, string name, string email, string document)
        {
            Id = id;
            Name = name;
            Email = email;
            Document = document;
        }
    }
}
