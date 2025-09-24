using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkFlow.Domain.Aggregates.Matching.StrangerMatching;
using TalkFlow.Domain.ValueObjects.User;

namespace TalkFlow.Domain.Repositories
{
    public interface IStrangerMatchingRepository
    {
        Task<StrangerMatching?> GetByIdAsync(StrangerMatchingId strangerMatchingId);
        Task<StrangerMatching?> GetActiveByUserIdAsync(UserId userId);
        Task<IEnumerable<StrangerMatching>> GetActiveMatchingsAsync();
        Task<IEnumerable<StrangerMatching>> GetMatchingsByUserIdAsync(UserId userId);
        Task AddAsync(StrangerMatching strangerMatching);
        Task UpdateAsync(StrangerMatching strangerMatching);
        Task DeleteAsync(StrangerMatchingId strangerMatchingId);
        Task<bool> ExistsAsync(StrangerMatchingId strangerMatchingId);
        Task<int> CountAsync();
    }
}
