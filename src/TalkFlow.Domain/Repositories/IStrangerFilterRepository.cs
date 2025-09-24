using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkFlow.Domain.Aggregates.User;
using TalkFlow.Domain.ValueObjects.User;

namespace TalkFlow.Domain.Repositories
{
    public interface IStrangerFilterRepository
    {
        Task<StrangerFilter?> GetByUserIdAsync(UserId userId);
        Task<IEnumerable<StrangerFilter>> GetActiveFiltersAsync();
        Task AddAsync(StrangerFilter filter);
        Task UpdateAsync(StrangerFilter filter);
        Task DeleteAsync(Guid filterId);
    }
}
