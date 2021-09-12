using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using NCD.Core.Data;
using NCD.Core.DomainObjects;
using NCD.Core.Mediator;
using NCD.Core.Messages;
using NCD.Customers.API.Model;
using System.Linq;
using System.Threading.Tasks;

namespace NCD.Customers.API.Data
{
    public class CustomerDbContext : DbContext, IUnitOfWork
    {
        private readonly IMediatorHandler _mediatorHandler;
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Address> Address { get; set; }
        
        public CustomerDbContext(DbContextOptions<CustomerDbContext> options, IMediatorHandler mediatorHandler) : base(options) 
        {
            _mediatorHandler = mediatorHandler;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<Event>();
            modelBuilder.Ignore<ValidationResult>();

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CustomerDbContext).Assembly);
        }

        public async Task<bool> Commit()
        {
            var sucesso = await base.SaveChangesAsync() > 0;

            if (sucesso) await _mediatorHandler.PublishEvents(this);

            return sucesso;
        }
    }

    public static class MediatorExtension
    {
        public static async Task PublishEvents<T>(this IMediatorHandler mediator, T ctx) where T : DbContext
        {
            var domainEntities = ctx.ChangeTracker
                                    .Entries<Entity>()
                                    .Where(x => x.Entity.Events != null && x.Entity.Events.Any());

            var domainEvents = domainEntities
                                .SelectMany(x => x.Entity.Events)
                                .ToList();

            domainEntities.ToList().ForEach(entity => entity.Entity.ClearEvents());

            var tasks = domainEvents.Select(async (domainEvent) =>
            {
                await mediator.PublishEvent(domainEvent);
            });

            await Task.WhenAll(tasks);
        }
    }
}
