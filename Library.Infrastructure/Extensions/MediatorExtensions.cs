using System.Linq;
using System.Threading.Tasks;
using Library.Domain.SeedWork;
using MediatR;

namespace Library.Infrastructure.Extensions
{
    public static class MediatorExtensions
    {
        public static async Task DispatchDomainEventsAsync(this IMediator mediator, LibraryContext ctx)
        {
            var domainEntities = ctx.ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any())
                .ToList();

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();

            domainEntities.ToList()
                .ForEach(entity => entity.Entity.ClearDomainEvents());

            foreach (var domainEvent in domainEvents)
                await mediator.Publish(domainEvent);
        }
    }
}