using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NCD.Core.Utils;
using NCD.MessageBus;

namespace NCD.Identity.API.Configuration
{
    public static class MessageBusConfig
    {
        public static void AddMessageBusConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMessageBus(configuration.GetMessageQueueConnection("MessageBus"));
        }
    }
}
