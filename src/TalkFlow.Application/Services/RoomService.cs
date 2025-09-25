using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkFlow.Application.Commands.Room.CreateRoom;
using TalkFlow.Application.DTOs.Common;
using TalkFlow.Application.DTOs.Room;
using TalkFlow.Application.DTOs.User;
using TalkFlow.Application.Mappings;
using TalkFlow.Domain.Aggregates.Room;
using TalkFlow.Domain.Aggregates.User;
using TalkFlow.Domain.Repositories;
using TalkFlow.Domain.ValueObjects.Room;
using TalkFlow.Domain.ValueObjects.User;

namespace TalkFlow.Application.Services
{
    public class RoomService : IRoomService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public RoomService(IUnitOfWork unitOfWork, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<Result<TalkFlow.Application.Commands.Room.CreateRoom.CreateRoomResponseDto>> CreateRoomAsync(CreateRoomDto roomData)
        {
            var result = await _mediator.Send(new CreateRoomCommand(roomData));
            return result;
        }

        public async Task<Result<JoinRoomResponseDto>> JoinRoomAsync(JoinRoomDto joinData)
        {
            var roomId = new RoomId(joinData.RoomId);
            var room = await _unitOfWork.Rooms.GetByIdAsync(roomId);

            if (room == null)
            {
                return Result<JoinRoomResponseDto>.Failure("Room not found");
            }

            if (!string.IsNullOrEmpty(joinData.SecurityCode))
            {
                var securityCode = new SecurityCode(joinData.SecurityCode);
                if (room.SecurityCode.Value != securityCode.Value)
                {
                    return Result<JoinRoomResponseDto>.Failure("Invalid security code");
                }
            }

            var displayName = new DisplayName(joinData.DisplayName);
            var user = TalkFlow.Domain.Aggregates.User.User.CreateAnonymous(displayName);
            await _unitOfWork.Users.AddAsync(user);

            var connection = new Connection(Guid.NewGuid().ToString(), new UserId(user.Id), room.Id);
            room.AddConnection(connection);

            await _unitOfWork.Rooms.UpdateAsync(room);
            await _unitOfWork.SaveChangesAsync();

            var response = new JoinRoomResponseDto
            {
                User = UserMappingProfile.ToDto(user),
                Room = RoomMappingProfile.ToDto(room)
            };

            return Result<JoinRoomResponseDto>.Success(response);
        }

        public async Task<Result<RoomDto>> GetRoomByIdAsync(RoomId roomId)
        {
            var room = await _unitOfWork.Rooms.GetByIdAsync(roomId);
            if (room == null)
            {
                return Result<RoomDto>.Failure("Room not found");
            }

            var roomDto = RoomMappingProfile.ToDto(room);
            return Result<RoomDto>.Success(roomDto);
        }

        public async Task<Result<PaginatedResult<RoomDto>>> GetRoomsAsync(int pageNumber = 1, int pageSize = 10)
        {
            var skip = (pageNumber - 1) * pageSize;
            var rooms = await _unitOfWork.Rooms.GetRoomsAsync(skip, pageSize);
            var totalCount = await _unitOfWork.Rooms.CountAsync();

            var roomDtos = rooms.Select(RoomMappingProfile.ToDto);
            var paginatedResult = new PaginatedResult<RoomDto>(roomDtos, pageNumber, pageSize, totalCount);

            return Result<PaginatedResult<RoomDto>>.Success(paginatedResult);
        }

        public async Task<Result<RoomDto>> UpdateRoomAsync(RoomId roomId, TalkFlow.Application.DTOs.Room.UpdateRoomDto roomData, UserId userId)
        {
            var room = await _unitOfWork.Rooms.GetByIdAsync(roomId);
            if (room == null)
            {
                return Result<RoomDto>.Failure("Room not found");
            }

            if (!room.IsHost(userId))
            {
                return Result<RoomDto>.Failure("Only room host can update room");
            }

            if (roomData.RoomName != room.Name.Value)
            {
                var newRoomName = new RoomName(roomData.RoomName);
                room.UpdateName(newRoomName);
            }

            if (roomData.SecurityCode != room.SecurityCode.Value)
            {
                var newSecurityCode = new SecurityCode(roomData.SecurityCode ?? string.Empty);
                room.UpdateSecurityCode(newSecurityCode);
            }

            await _unitOfWork.Rooms.UpdateAsync(room);
            await _unitOfWork.SaveChangesAsync();

            var roomDto = RoomMappingProfile.ToDto(room);
            return Result<RoomDto>.Success(roomDto);
        }

        public async Task<Result> DeleteRoomAsync(RoomId roomId, UserId userId)
        {
            var room = await _unitOfWork.Rooms.GetByIdAsync(roomId);
            if (room == null)
            {
                return Result.Failure("Room not found");
            }

            if (!room.IsHost(userId))
            {
                return Result.Failure("Only room host can delete room");
            }

            room.Deactivate();
            await _unitOfWork.Rooms.UpdateAsync(room);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result> BlockChatAsync(RoomId roomId, UserId userId)
        {
            var room = await _unitOfWork.Rooms.GetByIdAsync(roomId);
            if (room == null)
            {
                return Result.Failure("Room not found");
            }

            if (!room.IsHost(userId))
            {
                return Result.Failure("Only room host can block chat");
            }

            room.BlockChat();
            await _unitOfWork.Rooms.UpdateAsync(room);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result> UnblockChatAsync(RoomId roomId, UserId userId)
        {
            var room = await _unitOfWork.Rooms.GetByIdAsync(roomId);
            if (room == null)
            {
                return Result.Failure("Room not found");
            }

            if (!room.IsHost(userId))
            {
                return Result.Failure("Only room host can unblock chat");
            }

            room.UnblockChat();
            await _unitOfWork.Rooms.UpdateAsync(room);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }
    }
}
