using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkFlow.Application.DTOs.Common;
using TalkFlow.Application.DTOs.User;
using TalkFlow.Application.Mappings;
using TalkFlow.Domain.Repositories;
using TalkFlow.Domain.Services;
using TalkFlow.Domain.ValueObjects.User;

namespace TalkFlow.Application.Commands.User.UpdateUser
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result<UserDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserDomainService _userDomainService;

        public UpdateUserCommandHandler(IUnitOfWork unitOfWork, IUserDomainService userDomainService)
        {
            _unitOfWork = unitOfWork;
            _userDomainService = userDomainService;
        }

        public async Task<Result<UserDto>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByIdAsync(request.UserId);
                if (user == null)
                {
                    return Result<UserDto>.Failure("User not found");
                }

                if (request.UserData.DisplayName != user.DisplayName.Value)
                {
                    var newDisplayName = new DisplayName(request.UserData.DisplayName);

                    if (!await _userDomainService.IsDisplayNameUniqueAsync(newDisplayName, request.UserId))
                    {
                        return Result<UserDto>.Failure("Display name already exists");
                    }
                }

                if (!string.IsNullOrEmpty(request.UserData.Gender))
                {
                    user.UpdatePersonalInfo(
                        new Gender(request.UserData.Gender),
                        request.UserData.Age != null ? new Age(request.UserData.Age.Value) : user.Age,
                        !string.IsNullOrEmpty(request.UserData.Nationality) ? new Nationality(request.UserData.Nationality) : user.Nationality
                    );
                }

                if (!string.IsNullOrEmpty(request.UserData.PhotoUrl))
                {
                    user.UpdatePhoto(new PhotoUrl(request.UserData.PhotoUrl));
                }

                await _unitOfWork.Users.UpdateAsync(user);
                await _unitOfWork.SaveChangesAsync();

                var userDto = UserMappingProfile.ToDto(user);
                return Result<UserDto>.Success(userDto);
            }
            catch (Exception ex)
            {
                return Result<UserDto>.Failure($"Failed to update user: {ex.Message}");
            }
        }
    }
}
