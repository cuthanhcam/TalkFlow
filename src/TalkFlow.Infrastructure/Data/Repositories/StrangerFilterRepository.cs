using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkFlow.Domain.Aggregates.User;
using TalkFlow.Domain.Repositories;
using TalkFlow.Domain.ValueObjects.User;

namespace TalkFlow.Infrastructure.Data.Repositories
{
    public class StrangerFilterRepository : IStrangerFilterRepository
    {
        private readonly ApplicationDbContext _context;

        public StrangerFilterRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<StrangerFilter?> GetByUserIdAsync(UserId userId)
        {
            var user = await _context.Users
                .Include(u => u.StrangerFilter)
                .FirstOrDefaultAsync(u => u.Id == userId.Value);
            return user?.StrangerFilter;
        }

        public async Task<IEnumerable<StrangerFilter>> GetActiveFiltersAsync()
        {
            return await _context.StrangerFilters
                .Where(sf => sf.CurrentRoomId == null)
                .ToListAsync();
        }

        public async Task AddAsync(StrangerFilter filter)
        {
            await _context.StrangerFilters.AddAsync(filter);
        }

        public Task UpdateAsync(StrangerFilter filter)
        {
            _context.StrangerFilters.Update(filter);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(Guid filterId)
        {
            var filter = await _context.StrangerFilters.FindAsync(filterId);
            if (filter != null)
            {
                _context.StrangerFilters.Remove(filter);
            }
        }
    }
}
