using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkFlow.Application.DTOs.Common;
using TalkFlow.Application.DTOs.Room;
using TalkFlow.Application.DTOs.User;
using TalkFlow.Domain.ValueObjects.Room;
using TalkFlow.Domain.ValueObjects.User;

namespace TalkFlow.Application.Services
{
    public interface IRoomService
    {
        Task<Result<TalkFlow.Application.Commands.Room.CreateRoom.CreateRoomResponseDto>> CreateRoomAsync(CreateRoomDto roomData);
        Task<Result<JoinRoomResponseDto>> JoinRoomAsync(JoinRoomDto joinData);
        Task<Result<RoomDto>> GetRoomByIdAsync(RoomId roomId);
        Task<Result<PaginatedResult<RoomDto>>> GetRoomsAsync(int pageNumber = 1, int pageSize = 10);
        Task<Result<RoomDto>> UpdateRoomAsync(RoomId roomId, TalkFlow.Application.DTOs.Room.UpdateRoomDto roomData, UserId userId);
        Task<Result> DeleteRoomAsync(RoomId roomId, UserId userId);
        Task<Result> BlockChatAsync(RoomId roomId, UserId userId);
        Task<Result> UnblockChatAsync(RoomId roomId, UserId userId);
    }

    public record JoinRoomResponseDto
    {
        public UserDto User { get; set; } = new();
        public RoomDto Room { get; set; } = new();
    }
}
