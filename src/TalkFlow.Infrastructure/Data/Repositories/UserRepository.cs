using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkFlow.Domain.Aggregates.User;
using TalkFlow.Domain.Repositories;
using TalkFlow.Domain.ValueObjects.Common;
using TalkFlow.Domain.ValueObjects.User;

namespace TalkFlow.Infrastructure.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByIdAsync(UserId userId)
        {
            return await _context.Users
                .Include(u => u.Profile)
                .Include(u => u.StrangerFilter)
                .FirstOrDefaultAsync(u => u.Id == userId.Value);
        }

        public async Task<User?> GetByDisplayNameAsync(DisplayName displayName)
        {
            return await _context.Users
                .Include(u => u.Profile)
                .Include(u => u.StrangerFilter)
                .FirstOrDefaultAsync(u => u.DisplayName.Value == displayName.Value);
        }

        public async Task<User?> GetByEmailAsync(Email email)
        {
            return await _context.Users
                .Include(u => u.Profile)
                .Include(u => u.StrangerFilter)
                .FirstOrDefaultAsync(u => u.Email == email.Value);
        }

        public async Task<IEnumerable<User>> GetUsersAsync(int skip = 0, int take = 10)
        {
            return await _context.Users
                .Include(u => u.Profile)
                .Include(u => u.StrangerFilter)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetUsersByFilterAsync(StrangerFilter filter, UserId excludeUserId)
        {
            var query = _context.Users
                .Include(u => u.Profile)
                .Include(u => u.StrangerFilter)
                .Where(u => u.Id != excludeUserId.Value);

            if (filter.FindGender.Any())
            {
                query = query.Where(u => u.Gender != null &&
                    filter.FindGender.Contains(u.Gender.Value, StringComparer.OrdinalIgnoreCase));
            }

            if (filter.MinAge > 0 || filter.MaxAge < 100)
            {
                query = query.Where(u => u.Age != null &&
                    u.Age.Value >= filter.MinAge && u.Age.Value <= filter.MaxAge);
            }

            if (filter.FindRegion.Any())
            {
                query = query.Where(u => u.Nationality != null &&
                    filter.FindRegion.Contains(u.Nationality.Value, StringComparer.OrdinalIgnoreCase));
            }

            return await query.ToListAsync();
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(UserId userId)
        {
            var user = await GetByIdAsync(userId);
            if (user != null)
            {
                _context.Users.Remove(user);
            }
        }

        public async Task<bool> ExistsAsync(UserId userId)
        {
            return await _context.Users.AnyAsync(u => u.Id == userId.Value);
        }

        public async Task<int> CountAsync()
        {
            return await _context.Users.CountAsync();
        }
    }
}
