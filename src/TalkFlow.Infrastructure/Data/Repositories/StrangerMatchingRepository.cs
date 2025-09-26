using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkFlow.Domain.Aggregates.Matching.StrangerMatching;
using TalkFlow.Domain.Repositories;
using TalkFlow.Domain.ValueObjects.User;

namespace TalkFlow.Infrastructure.Data.Repositories
{
    public class StrangerMatchingRepository : IStrangerMatchingRepository
    {
        private readonly ApplicationDbContext _context;

        public StrangerMatchingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<StrangerMatching?> GetByIdAsync(StrangerMatchingId strangerMatchingId)
        {
            return await _context.StrangerMatchings
                .FirstOrDefaultAsync(sm => sm.Id == strangerMatchingId);
        }

        public async Task<StrangerMatching?> GetActiveByUserIdAsync(UserId userId)
        {
            return await _context.StrangerMatchings
                .FirstOrDefaultAsync(sm => sm.UserId == userId && sm.Status == StrangerMatchingStatus.Active);
        }

        public async Task<IEnumerable<StrangerMatching>> GetActiveMatchingsAsync()
        {
            return await _context.StrangerMatchings
                .Where(sm => sm.Status == StrangerMatchingStatus.Active)
                .OrderBy(sm => sm.StartedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<StrangerMatching>> GetMatchingsByUserIdAsync(UserId userId)
        {
            return await _context.StrangerMatchings
                .Where(sm => sm.UserId == userId)
                .OrderByDescending(sm => sm.StartedAt)
                .ToListAsync();
        }

        public async Task AddAsync(StrangerMatching strangerMatching)
        {
            await _context.StrangerMatchings.AddAsync(strangerMatching);
        }

        public Task UpdateAsync(StrangerMatching strangerMatching)
        {
            _context.StrangerMatchings.Update(strangerMatching);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(StrangerMatchingId strangerMatchingId)
        {
            var strangerMatching = await GetByIdAsync(strangerMatchingId);
            if (strangerMatching != null)
            {
                _context.StrangerMatchings.Remove(strangerMatching);
            }
        }

        public async Task<bool> ExistsAsync(StrangerMatchingId strangerMatchingId)
        {
            return await _context.StrangerMatchings.AnyAsync(sm => sm.Id == strangerMatchingId);
        }

        public async Task<int> CountAsync()
        {
            return await _context.StrangerMatchings.CountAsync();
        }
    }
}
