using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkFlow.Domain.Aggregates.Matching.Match;
using TalkFlow.Domain.ValueObjects.User;

namespace TalkFlow.Domain.Repositories
{
    public interface IMatchRepository
    {
        Task<Match?> GetByIdAsync(MatchId matchId);
        Task<IEnumerable<Match>> GetMatchesByUserIdAsync(UserId userId);
        Task<IEnumerable<Match>> GetActiveMatchesAsync();
        Task AddAsync(Match match);
        Task UpdateAsync(Match match);
        Task DeleteAsync(MatchId matchId);
        Task<bool> ExistsAsync(MatchId matchId);
        Task<int> CountAsync();
    }
}
