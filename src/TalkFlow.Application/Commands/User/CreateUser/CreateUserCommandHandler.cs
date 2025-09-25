using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkFlow.Application.DTOs.Common;
using TalkFlow.Application.DTOs.User;
using TalkFlow.Application.Mappings;
using TalkFlow.Domain.Aggregates.User;
using TalkFlow.Domain.Repositories;
using TalkFlow.Domain.Services;
using TalkFlow.Domain.ValueObjects.Common;
using TalkFlow.Domain.ValueObjects.User;

namespace TalkFlow.Application.Commands.User.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<UserDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserDomainService _userDomainService;

        public CreateUserCommandHandler(IUnitOfWork unitOfWork, IUserDomainService userDomainService)
        {
            _unitOfWork = unitOfWork;
            _userDomainService = userDomainService;
        }

        public async Task<Result<UserDto>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var displayName = new DisplayName(request.UserData.DisplayName);

                if (!await _userDomainService.IsDisplayNameUniqueAsync(displayName))
                {
                    return Result<UserDto>.Failure("Display name already exists");
                }

                if (!string.IsNullOrEmpty(request.UserData.Email))
                {
                    var email = new Email(request.UserData.Email);
                    if (!await _userDomainService.IsEmailUniqueAsync(email))
                    {
                        return Result<UserDto>.Failure("Email already exists");
                    }
                }

                var user = TalkFlow.Domain.Aggregates.User.User.CreateAnonymous(displayName);

                if (!string.IsNullOrEmpty(request.UserData.Email))
                {
                    user.Email = request.UserData.Email;
                }

                if (!string.IsNullOrEmpty(request.UserData.Gender))
                {
                    user.UpdatePersonalInfo(
                        new Gender(request.UserData.Gender),
                        request.UserData.Age != null ? new Age(request.UserData.Age.Value) : null,
                        !string.IsNullOrEmpty(request.UserData.Nationality) ? new Nationality(request.UserData.Nationality) : null
                    );
                }

                if (!string.IsNullOrEmpty(request.UserData.PhotoUrl))
                {
                    user.UpdatePhoto(new PhotoUrl(request.UserData.PhotoUrl));
                }

                if (request.UserData.StrangerFilter != null)
                {
                    var strangerFilter = new StrangerFilter(
                        request.UserData.StrangerFilter.FindGender,
                        request.UserData.StrangerFilter.MinAge,
                        request.UserData.StrangerFilter.MaxAge,
                        request.UserData.StrangerFilter.FindRegion
                    );
                    user.UpdateStrangerFilter(strangerFilter);
                }

                await _unitOfWork.Users.AddAsync(user);
                await _unitOfWork.SaveChangesAsync();

                var userDto = UserMappingProfile.ToDto(user);
                return Result<UserDto>.Success(userDto);
            }
            catch (Exception ex)
            {
                return Result<UserDto>.Failure($"Failed to create user: {ex.Message}");
            }
        }
    }
}
