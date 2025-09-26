using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkFlow.Domain.Aggregates.User;
using TalkFlow.Domain.Repositories;
using TalkFlow.Domain.Services;
using TalkFlow.Domain.ValueObjects.Common;
using TalkFlow.Domain.ValueObjects.User;

namespace TalkFlow.Infrastructure.Services
{
    public class UserDomainService : IUserDomainService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserDomainService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> IsDisplayNameUniqueAsync(DisplayName displayName, UserId? excludeUserId = null)
        {
            var user = await _unitOfWork.Users.GetByDisplayNameAsync(displayName);
            if (user == null) return true;
            return excludeUserId != null && new UserId(user.Id) == excludeUserId;
        }

        public async Task<bool> IsEmailUniqueAsync(Email email, UserId? excludeUserId = null)
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(email);
            if (user == null) return true;
            return excludeUserId != null && new UserId(user.Id) == excludeUserId;
        }

        public Task<User?> FindUserByDisplayNameAsync(DisplayName displayName)
            => _unitOfWork.Users.GetByDisplayNameAsync(displayName);

        public Task<User?> FindUserByEmailAsync(Email email)
            => _unitOfWork.Users.GetByEmailAsync(email);
    }
}
