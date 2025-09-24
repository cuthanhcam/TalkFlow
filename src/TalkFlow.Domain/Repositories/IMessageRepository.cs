using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkFlow.Domain.Aggregates.Room;
using TalkFlow.Domain.ValueObjects.Room;

namespace TalkFlow.Domain.Repositories
{
    public interface IMessageRepository
    {
        Task<IEnumerable<Message>> GetMessagesByRoomAsync(RoomId roomId, int skip = 0, int take = 50);
        Task<Message?> GetByIdAsync(Guid messageId);
        Task AddAsync(Message message);
        Task UpdateAsync(Message message);
        Task DeleteAsync(Guid messageId);
        Task<int> CountByRoomAsync(RoomId roomId);
    }
}
