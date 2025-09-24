using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkFlow.Domain.ValueObjects.Room;
using TalkFlow.Domain.ValueObjects.User;

namespace TalkFlow.Domain.Services
{
    public interface IWebRTCSignalingService
    {
        Task SendOfferAsync(RoomId roomId, UserId fromUserId, UserId toUserId, string offer);
        Task SendAnswerAsync(RoomId roomId, UserId fromUserId, UserId toUserId, string answer);
        Task SendIceCandidateAsync(RoomId roomId, UserId fromUserId, UserId toUserId, string iceCandidate);
        Task NotifyUserJoinedAsync(RoomId roomId, UserId userId);
        Task NotifyUserLeftAsync(RoomId roomId, UserId userId);
    }
}
