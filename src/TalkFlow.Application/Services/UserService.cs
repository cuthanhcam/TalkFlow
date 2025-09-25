using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkFlow.Application.Commands.User.CreateUser;
using TalkFlow.Application.DTOs.Common;
using TalkFlow.Application.DTOs.User;
using TalkFlow.Application.Mappings;
using TalkFlow.Domain.Repositories;
using TalkFlow.Domain.ValueObjects.User;
using MediatR;

namespace TalkFlow.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public UserService(IUnitOfWork unitOfWork, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<Result<UserDto>> CreateUserAsync(CreateUserDto userData)
        {
            var result = await _mediator.Send(new CreateUserCommand(userData));
            return result;
        }

        public async Task<Result<UserDto>> GetUserByIdAsync(UserId userId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
            {
                return Result<UserDto>.Failure("User not found");
            }

            var userDto = UserMappingProfile.ToDto(user);
            return Result<UserDto>.Success(userDto);
        }

        public async Task<Result<UserDto>> GetUserByDisplayNameAsync(DisplayName displayName)
        {
            var user = await _unitOfWork.Users.GetByDisplayNameAsync(displayName);
            if (user == null)
            {
                return Result<UserDto>.Failure("User not found");
            }

            var userDto = UserMappingProfile.ToDto(user);
            return Result<UserDto>.Success(userDto);
        }

        public async Task<Result<PaginatedResult<UserDto>>> GetUsersAsync(int pageNumber = 1, int pageSize = 10)
        {
            var skip = (pageNumber - 1) * pageSize;
            var users = await _unitOfWork.Users.GetUsersAsync(skip, pageSize);
            var totalCount = await _unitOfWork.Users.CountAsync();

            var userDtos = users.Select(UserMappingProfile.ToDto);
            var paginatedResult = new PaginatedResult<UserDto>(userDtos, pageNumber, pageSize, totalCount);

            return Result<PaginatedResult<UserDto>>.Success(paginatedResult);
        }

        public async Task<Result<UserDto>> UpdateUserAsync(UserId userId, UpdateUserDto userData)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
            {
                return Result<UserDto>.Failure("User not found");
            }

            if (userData.DisplayName != user.DisplayName.Value)
            {
                var newDisplayName = new DisplayName(userData.DisplayName);
                var existingUser = await _unitOfWork.Users.GetByDisplayNameAsync(newDisplayName);
                if (existingUser != null && existingUser.Id != userId)
                {
                    return Result<UserDto>.Failure("Display name already exists");
                }
            }

            if (!string.IsNullOrEmpty(userData.Gender))
            {
                user.UpdatePersonalInfo(
                    new Gender(userData.Gender),
                    userData.Age != null ? new Age(userData.Age.Value) : user.Age,
                    !string.IsNullOrEmpty(userData.Nationality) ? new Nationality(userData.Nationality) : user.Nationality
                );
            }

            if (!string.IsNullOrEmpty(userData.PhotoUrl))
            {
                user.UpdatePhoto(new PhotoUrl(userData.PhotoUrl));
            }

            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            var userDto = UserMappingProfile.ToDto(user);
            return Result<UserDto>.Success(userDto);
        }

        public async Task<Result> LockUserAsync(UserId userId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
            {
                return Result.Failure("User not found");
            }

            user.Lock();
            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result> UnlockUserAsync(UserId userId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
            {
                return Result.Failure("User not found");
            }

            user.Unlock();
            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result> DeleteUserAsync(UserId userId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
            {
                return Result.Failure("User not found");
            }

            await _unitOfWork.Users.DeleteAsync(userId);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }
    }
}
