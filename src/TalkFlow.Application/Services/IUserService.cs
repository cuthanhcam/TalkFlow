using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkFlow.Application.DTOs.Common;
using TalkFlow.Application.DTOs.User;
using TalkFlow.Domain.ValueObjects.User;

namespace TalkFlow.Application.Services
{
    public interface IUserService
    {
        Task<Result<UserDto>> CreateUserAsync(CreateUserDto userData);
        Task<Result<UserDto>> GetUserByIdAsync(UserId userId);
        Task<Result<UserDto>> GetUserByDisplayNameAsync(DisplayName displayName);
        Task<Result<PaginatedResult<UserDto>>> GetUsersAsync(int pageNumber = 1, int pageSize = 10);
        Task<Result<UserDto>> UpdateUserAsync(UserId userId, UpdateUserDto userData);
        Task<Result> LockUserAsync(UserId userId);
        Task<Result> UnlockUserAsync(UserId userId);
        Task<Result> DeleteUserAsync(UserId userId);
    }
}
