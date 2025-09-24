using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkFlow.Domain.Aggregates.User;
using TalkFlow.Domain.ValueObjects.User;

namespace TalkFlow.Domain.Services
{
    public interface IStrangerMatchingService
    {
        Task<IEnumerable<User>> FindMatchingUsersAsync(StrangerFilter filter, UserId excludeUserId);
        Task<bool> CanMatchUsersAsync(UserId user1Id, UserId user2Id);
        Task<MatchResult> TryMatchAsync(UserId requesterId);
    }
}
