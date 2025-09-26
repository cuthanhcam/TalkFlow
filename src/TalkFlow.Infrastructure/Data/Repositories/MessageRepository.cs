using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkFlow.Domain.Aggregates.Room;
using TalkFlow.Domain.Repositories;
using TalkFlow.Domain.ValueObjects.Room;

namespace TalkFlow.Infrastructure.Data.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ApplicationDbContext _context;

        public MessageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Message>> GetMessagesByRoomAsync(RoomId roomId, int skip = 0, int take = 50)
        {
            return await _context.Messages
                .Where(m => m.RoomId == roomId && !m.IsDeleted)
                .OrderByDescending(m => m.SentAt)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        public async Task<Message?> GetByIdAsync(Guid messageId)
        {
            return await _context.Messages
                .FirstOrDefaultAsync(m => m.Id == messageId);
        }

        public async Task AddAsync(Message message)
        {
            await _context.Messages.AddAsync(message);
        }

        public Task UpdateAsync(Message message)
        {
            _context.Messages.Update(message);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(Guid messageId)
        {
            var message = await GetByIdAsync(messageId);
            if (message != null)
            {
                _context.Messages.Remove(message);
            }
        }

        public async Task<int> CountByRoomAsync(RoomId roomId)
        {
            return await _context.Messages
                .CountAsync(m => m.RoomId == roomId && !m.IsDeleted);
        }
    }
}
