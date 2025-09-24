using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkFlow.Domain.Aggregates.Room;
using TalkFlow.Domain.ValueObjects.Room;
using TalkFlow.Domain.ValueObjects.User;

namespace TalkFlow.Domain.Services
{
    public interface IRoomDomainService
    {
        Task<bool> IsRoomNameUniqueAsync(RoomName roomName, RoomId? excludeRoomId = null);
        Task<Room?> FindRoomBySecurityCodeAsync(SecurityCode securityCode);
        Task<bool> CanUserJoinRoomAsync(RoomId roomId, UserId userId);
        Task<bool> IsUserHostOfRoomAsync(RoomId roomId, UserId userId);
    }
}
