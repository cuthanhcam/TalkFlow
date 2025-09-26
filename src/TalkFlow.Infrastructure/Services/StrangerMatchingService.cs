using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkFlow.Domain.Aggregates.Room;
using TalkFlow.Domain.Aggregates.User;
using TalkFlow.Domain.Repositories;
using TalkFlow.Domain.Services;
using TalkFlow.Domain.ValueObjects.Room;
using TalkFlow.Domain.ValueObjects.User;

namespace TalkFlow.Infrastructure.Services
{
    public class StrangerMatchingService : IStrangerMatchingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public StrangerMatchingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<User>> FindMatchingUsersAsync(StrangerFilter filter, UserId excludeUserId)
        {
            return await _unitOfWork.Users.GetUsersByFilterAsync(filter, excludeUserId);
        }

        public async Task<bool> CanMatchUsersAsync(UserId user1Id, UserId user2Id)
        {
            var user1 = await _unitOfWork.Users.GetByIdAsync(user1Id);
            var user2 = await _unitOfWork.Users.GetByIdAsync(user2Id);
            if (user1 == null || user2 == null) return false;
            if (user1.StrangerFilter == null || user2.StrangerFilter == null) return false;
            return user1.StrangerFilter.Matches(user2) && user2.StrangerFilter.Matches(user1);
        }

        public async Task<MatchResult> TryMatchAsync(UserId requesterId)
        {
            var requester = await _unitOfWork.Users.GetByIdAsync(requesterId);
            if (requester == null) return new MatchResult(false, null, null, "Requester not found");
            if (requester.StrangerFilter == null) return new MatchResult(false, null, null, "Requester has no filter");

            var candidates = await FindMatchingUsersAsync(requester.StrangerFilter, requesterId);
            var partner = candidates.FirstOrDefault();
            if (partner == null) return new MatchResult(false, null, null, "No candidates");

            var room = Room.Create(new RoomName($"Stranger-{Guid.NewGuid():N}"), requesterId, SecurityCode.Empty);
            await _unitOfWork.Rooms.AddAsync(room);

            if (requester.StrangerFilter != null)
            {
                requester.StrangerFilter.SetCurrentRoom(room.Id);
            }
            if (partner.StrangerFilter != null)
            {
                partner.StrangerFilter.SetCurrentRoom(room.Id);
            }
            await _unitOfWork.Users.UpdateAsync(requester);
            await _unitOfWork.Users.UpdateAsync(partner);

            await _unitOfWork.SaveChangesAsync();

            return new MatchResult(true, new RoomId(room.Id), new UserId(partner.Id), null);
        }
    }
}
