using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalkFlow.Domain.Events
{
    public abstract record DomainEventBase : IDomainEvent
    {
        public Guid Id { get; } = Guid.NewGuid();

        public DateTime OccurredOn { get; } = DateTime.UtcNow;
    }
}
