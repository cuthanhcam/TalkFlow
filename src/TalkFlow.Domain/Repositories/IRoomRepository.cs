using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkFlow.Domain.Aggregates.Room;
using TalkFlow.Domain.ValueObjects.Room;
using TalkFlow.Domain.ValueObjects.User;

namespace TalkFlow.Domain.Repositories
{
    public interface IRoomRepository
    {
        Task<Room?> GetByIdAsync(RoomId roomId);
        Task<Room?> GetBySecurityCodeAsync(SecurityCode securityCode);
        Task<IEnumerable<Room>> GetRoomsAsync(int skip = 0, int take = 10);
        Task<IEnumerable<Room>> GetRoomsByHostAsync(UserId hostId);
        Task<Room?> GetRoomByConnectionIdAsync(string connectionId);
        Task AddAsync(Room room);
        Task UpdateAsync(Room room);
        Task DeleteAsync(RoomId roomId);
        Task<bool> ExistsAsync(RoomId roomId);
        Task<int> CountAsync();
    }
}
