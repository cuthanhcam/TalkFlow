using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkFlow.Domain.Aggregates.Room;
using TalkFlow.Domain.Repositories;
using TalkFlow.Domain.Services;
using TalkFlow.Domain.ValueObjects.Room;
using TalkFlow.Domain.ValueObjects.User;

namespace TalkFlow.Infrastructure.Services
{
    public class RoomDomainService : IRoomDomainService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoomDomainService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> IsRoomNameUniqueAsync(RoomName roomName, RoomId? excludeRoomId = null)
        {
            var rooms = await _unitOfWork.Rooms.GetRoomsAsync(0, 1);
            var existing = rooms.FirstOrDefault(r => r.Name.Value == roomName.Value);
            if (existing == null) return true;
            return excludeRoomId != null && existing.Id == excludeRoomId;
        }

        public Task<Room?> FindRoomBySecurityCodeAsync(SecurityCode securityCode)
            => _unitOfWork.Rooms.GetBySecurityCodeAsync(securityCode);

        public async Task<bool> CanUserJoinRoomAsync(RoomId roomId, UserId userId)
        {
            var room = await _unitOfWork.Rooms.GetByIdAsync(roomId);
            if (room == null || !room.IsActive) return false;
            return !room.Connections.Any(c => c.UserId == userId);
        }

        public async Task<bool> IsUserHostOfRoomAsync(RoomId roomId, UserId userId)
        {
            var room = await _unitOfWork.Rooms.GetByIdAsync(roomId);
            return room != null && room.HostId == userId;
        }
    }
}
