using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkFlow.Application.DTOs.Common;
using TalkFlow.Application.DTOs.Room;
using TalkFlow.Application.Mappings;
using TalkFlow.Domain.Aggregates.Room;
using TalkFlow.Domain.Repositories;
using TalkFlow.Domain.Services;
using TalkFlow.Domain.ValueObjects.Room;
using TalkFlow.Domain.ValueObjects.User;

namespace TalkFlow.Application.Commands.Room.JoinRoom
{
    public class JoinRoomCommandHandler : IRequestHandler<JoinRoomCommand, Result<RoomDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRoomDomainService _roomDomainService;

        public JoinRoomCommandHandler(IUnitOfWork unitOfWork, IRoomDomainService roomDomainService)
        {
            _unitOfWork = unitOfWork;
            _roomDomainService = roomDomainService;
        }

        public async Task<Result<RoomDto>> Handle(JoinRoomCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var roomId = new RoomId(request.RoomData.RoomId);
                var displayName = new DisplayName(request.RoomData.DisplayName);

                var room = await _unitOfWork.Rooms.GetByIdAsync(roomId);
                if (room == null)
                {
                    return Result<RoomDto>.Failure("Room not found");
                }

                if (!room.IsActive)
                {
                    return Result<RoomDto>.Failure("Room is not active");
                }

                if (!string.IsNullOrEmpty(request.RoomData.SecurityCode))
                {
                    var securityCode = new SecurityCode(request.RoomData.SecurityCode);
                    if (room.SecurityCode.Value != securityCode.Value)
                    {
                        return Result<RoomDto>.Failure("Invalid security code");
                    }
                }

                var user = TalkFlow.Domain.Aggregates.User.User.CreateAnonymous(displayName);
                await _unitOfWork.Users.AddAsync(user);
                await _unitOfWork.SaveChangesAsync();

                var connection = new Connection(
                    Guid.NewGuid().ToString(),
                    new UserId(user.Id),
                    roomId
                );
                room.AddConnection(connection);

                await _unitOfWork.SaveChangesAsync();

                var roomDto = RoomMappingProfile.ToDto(room);
                return Result<RoomDto>.Success(roomDto);
            }
            catch (Exception ex)
            {
                return Result<RoomDto>.Failure($"Failed to join room: {ex.Message}");
            }
        }
    }
}
