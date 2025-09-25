using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkFlow.Application.DTOs.Common;
using TalkFlow.Application.Mappings;
using TalkFlow.Domain.Repositories;
using TalkFlow.Domain.Services;
using TalkFlow.Domain.ValueObjects.Room;
using TalkFlow.Domain.ValueObjects.User;

namespace TalkFlow.Application.Commands.Room.CreateRoom
{
    public class CreateRoomCommandHandler : IRequestHandler<CreateRoomCommand, Result<CreateRoomResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserDomainService _userDomainService;
        private readonly IRoomDomainService _roomDomainService;

        public CreateRoomCommandHandler(
            IUnitOfWork unitOfWork,
            IUserDomainService userDomainService,
            IRoomDomainService roomDomainService)
        {
            _unitOfWork = unitOfWork;
            _userDomainService = userDomainService;
            _roomDomainService = roomDomainService;
        }

        public async Task<Result<CreateRoomResponseDto>> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var displayName = new DisplayName(request.RoomData.DisplayName);
                var roomName = new RoomName(request.RoomData.RoomName);
                var securityCode = !string.IsNullOrEmpty(request.RoomData.SecurityCode)
                    ? new SecurityCode(request.RoomData.SecurityCode)
                    : SecurityCode.Empty;

                if (!await _userDomainService.IsDisplayNameUniqueAsync(displayName))
                {
                    return Result<CreateRoomResponseDto>.Failure("Display name already exists");
                }

                if (!await _roomDomainService.IsRoomNameUniqueAsync(roomName))
                {
                    return Result<CreateRoomResponseDto>.Failure("Room name already exists");
                }

                var user = TalkFlow.Domain.Aggregates.User.User.CreateAnonymous(displayName);
                await _unitOfWork.Users.AddAsync(user);

                var room = TalkFlow.Domain.Aggregates.Room.Room.Create(roomName, new UserId(user.Id), securityCode);
                await _unitOfWork.Rooms.AddAsync(room);

                await _unitOfWork.SaveChangesAsync();

                var response = new CreateRoomResponseDto
                {
                    User = UserMappingProfile.ToDto(user),
                    Room = RoomMappingProfile.ToDto(room)
                };

                return Result<CreateRoomResponseDto>.Success(response);
            }
            catch (Exception ex)
            {
                return Result<CreateRoomResponseDto>.Failure($"Failed to create room: {ex.Message}");
            }
        }
    }
}
