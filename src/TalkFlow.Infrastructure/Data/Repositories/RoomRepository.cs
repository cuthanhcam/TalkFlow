using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkFlow.Domain.Aggregates.Room;
using TalkFlow.Domain.Repositories;
using TalkFlow.Domain.ValueObjects.Room;
using TalkFlow.Domain.ValueObjects.User;

namespace TalkFlow.Infrastructure.Data.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly ApplicationDbContext _context;

        public RoomRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Room?> GetByIdAsync(RoomId roomId)
        {
            return await _context.Rooms
                .Include(r => r.Connections)
                .Include(r => r.Messages)
                .FirstOrDefaultAsync(r => r.Id == roomId);
        }

        public async Task<Room?> GetBySecurityCodeAsync(SecurityCode securityCode)
        {
            return await _context.Rooms
                .Include(r => r.Connections)
                .Include(r => r.Messages)
                .FirstOrDefaultAsync(r => r.SecurityCode.Value == securityCode.Value);
        }

        public async Task<IEnumerable<Room>> GetRoomsAsync(int skip = 0, int take = 10)
        {
            return await _context.Rooms
                .Include(r => r.Connections)
                .Include(r => r.Messages)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        public async Task<IEnumerable<Room>> GetRoomsByHostAsync(UserId hostId)
        {
            return await _context.Rooms
                .Include(r => r.Connections)
                .Include(r => r.Messages)
                .Where(r => r.HostId == hostId)
                .ToListAsync();
        }

        public async Task<Room?> GetRoomByConnectionIdAsync(string connectionId)
        {
            return await _context.Rooms
                .Include(r => r.Connections)
                .Include(r => r.Messages)
                .FirstOrDefaultAsync(r => r.Connections.Any(c => c.ConnectionId == connectionId));
        }

        public async Task AddAsync(Room room)
        {
            await _context.Rooms.AddAsync(room);
        }

        public Task UpdateAsync(Room room)
        {
            _context.Rooms.Update(room);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(RoomId roomId)
        {
            var room = await GetByIdAsync(roomId);
            if (room != null)
            {
                _context.Rooms.Remove(room);
            }
        }

        public async Task<bool> ExistsAsync(RoomId roomId)
        {
            return await _context.Rooms.AnyAsync(r => r.Id == roomId);
        }

        public async Task<int> CountAsync()
        {
            return await _context.Rooms.CountAsync();
        }
    }
}
