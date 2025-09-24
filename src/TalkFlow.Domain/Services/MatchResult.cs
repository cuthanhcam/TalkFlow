using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkFlow.Domain.ValueObjects.Room;
using TalkFlow.Domain.ValueObjects.User;

namespace TalkFlow.Domain.Services
{
    public class MatchResult
    {
        public bool IsSuccess { get; }
        public RoomId? RoomId { get; }
        public UserId? PartnerId { get; }
        public string? ErrorMessage { get; }

        public MatchResult(bool isSuccess, RoomId? roomId, UserId? partnerId, string? errorMessage)
        {
            IsSuccess = isSuccess;
            RoomId = roomId;
            PartnerId = partnerId;
            ErrorMessage = errorMessage;
        }

        public static MatchResult Success(RoomId roomId, UserId partnerId)
        {
            return new MatchResult(true, roomId, partnerId, null);
        }

        public static MatchResult Failure(string errorMessage)
        {
            return new MatchResult(false, null, null, errorMessage);
        }
    }
}
