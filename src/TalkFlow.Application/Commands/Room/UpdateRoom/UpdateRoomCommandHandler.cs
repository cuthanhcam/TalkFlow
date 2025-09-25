using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkFlow.Application.DTOs.Common;
using TalkFlow.Application.DTOs.Room;
using TalkFlow.Application.Mappings;
using TalkFlow.Domain.Repositories;
using TalkFlow.Domain.Services;
using TalkFlow.Domain.ValueObjects.Room;

namespace TalkFlow.Application.Commands.Room.UpdateRoom
{
    public class UpdateRoomCommandHandler : IRequestHandler<UpdateRoomCommand, Result<RoomDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRoomDomainService _roomDomainService;

        public UpdateRoomCommandHandler(IUnitOfWork unitOfWork, IRoomDomainService roomDomainService)
        {
            _unitOfWork = unitOfWork;
            _roomDomainService = roomDomainService;
        }

        public async Task<Result<RoomDto>> Handle(UpdateRoomCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var room = await _unitOfWork.Rooms.GetByIdAsync(request.RoomId);
                if (room == null)
                {
                    return Result<RoomDto>.Failure("Room not found");
                }

                if (!string.IsNullOrEmpty(request.RoomData.RoomName))
                {
                    var roomName = new RoomName(request.RoomData.RoomName);

                    if (!await _roomDomainService.IsRoomNameUniqueAsync(roomName, request.RoomId))
                    {
                        return Result<RoomDto>.Failure("Room name already exists");
                    }

                    room.UpdateName(roomName);
                }

                if (!string.IsNullOrEmpty(request.RoomData.SecurityCode))
                {
                    var securityCode = new SecurityCode(request.RoomData.SecurityCode);
                    room.UpdateSecurityCode(securityCode);
                }

                await _unitOfWork.SaveChangesAsync();

                var roomDto = RoomMappingProfile.ToDto(room);
                return Result<RoomDto>.Success(roomDto);
            }
            catch (Exception ex)
            {
                return Result<RoomDto>.Failure($"Failed to update room: {ex.Message}");
            }
        }
    }
}
