using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NCD.Core.Mediator;
using NCD.Customers.API.Application.Commands;
using NCD.Customers.API.Application.Events;
using NCD.Customers.API.Data;
using NCD.Customers.API.Data.Repository;
using NCD.Customers.API.Model;
using NCD.Customers.API.Services;

namespace NCD.Customers.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IMediatorHandler, MediatorHandler>();
            services.AddScoped<IRequestHandler<RegisterCustomerCommand, ValidationResult>, CustomerCommandHandler>();

            services.AddScoped<INotificationHandler<RegisteredCustomerEvent>, CustomerEventHandler>();

            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<CustomerDbContext>();
        }
    }
}
