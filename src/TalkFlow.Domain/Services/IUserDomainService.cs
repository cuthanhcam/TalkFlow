using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkFlow.Domain.Aggregates.User;
using TalkFlow.Domain.ValueObjects.Common;
using TalkFlow.Domain.ValueObjects.User;

namespace TalkFlow.Domain.Services
{
    public interface IUserDomainService
    {
        Task<bool> IsDisplayNameUniqueAsync(DisplayName displayName, UserId? excludeUserId = null);
        Task<bool> IsEmailUniqueAsync(Email email, UserId? excludeUserId = null);
        Task<User?> FindUserByDisplayNameAsync(DisplayName displayName);
        Task<User?> FindUserByEmailAsync(Email email);
    }
}
