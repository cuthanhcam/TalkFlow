using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkFlow.Domain.Aggregates.User.Events;
using TalkFlow.Domain.Events;

namespace TalkFlow.Domain.Events.Handlers
{
    public class UserCreatedEventHandler : IDomainEventHandler<UserCreatedEvent>
    {
        public Task Handle(UserCreatedEvent domainEvent)
        {
            Console.WriteLine($"User created: {domainEvent.UserId} with display name: {domainEvent.DisplayName}");

            return Task.CompletedTask;
        }
    }
}
