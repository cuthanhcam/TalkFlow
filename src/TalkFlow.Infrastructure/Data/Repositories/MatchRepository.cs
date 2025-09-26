using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkFlow.Domain.Aggregates.Matching.Match;
using TalkFlow.Domain.Repositories;
using TalkFlow.Domain.ValueObjects.User;

namespace TalkFlow.Infrastructure.Data.Repositories
{
    public class MatchRepository : IMatchRepository
    {
        private readonly ApplicationDbContext _context;

        public MatchRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Match?> GetByIdAsync(MatchId matchId)
        {
            return await _context.Matches
                .FirstOrDefaultAsync(m => m.Id == matchId);
        }

        public async Task<IEnumerable<Match>> GetMatchesByUserIdAsync(UserId userId)
        {
            return await _context.Matches
                .Where(m => m.User1Id == userId || m.User2Id == userId)
                .OrderByDescending(m => m.MatchedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Match>> GetActiveMatchesAsync()
        {
            return await _context.Matches
                .Where(m => m.Status == MatchStatus.Pending)
                .OrderBy(m => m.MatchedAt)
                .ToListAsync();
        }

        public async Task AddAsync(Match match)
        {
            await _context.Matches.AddAsync(match);
        }

        public Task UpdateAsync(Match match)
        {
            _context.Matches.Update(match);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(MatchId matchId)
        {
            var match = await GetByIdAsync(matchId);
            if (match != null)
            {
                _context.Matches.Remove(match);
            }
        }

        public async Task<bool> ExistsAsync(MatchId matchId)
        {
            return await _context.Matches.AnyAsync(m => m.Id == matchId);
        }

        public async Task<int> CountAsync()
        {
            return await _context.Matches.CountAsync();
        }
    }
}
