using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkFlow.Domain.Aggregates.User;
using TalkFlow.Domain.ValueObjects.Common;
using TalkFlow.Domain.ValueObjects.User;

namespace TalkFlow.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(UserId userId);
        Task<User?> GetByDisplayNameAsync(DisplayName displayName);
        Task<User?> GetByEmailAsync(Email email);
        Task<IEnumerable<User>> GetUsersAsync(int skip = 0, int take = 10);
        Task<IEnumerable<User>> GetUsersByFilterAsync(StrangerFilter filter, UserId excludeUserId);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(UserId userId);
        Task<bool> ExistsAsync(UserId userId);
        Task<int> CountAsync();
    }
}
