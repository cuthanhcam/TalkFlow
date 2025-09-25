using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkFlow.Application.DTOs.Common;
using TalkFlow.Application.DTOs.Message;
using TalkFlow.Domain.ValueObjects.Room;
using TalkFlow.Domain.ValueObjects.User;

namespace TalkFlow.Application.Services
{
    public interface IMessageService
    {
        Task<Result<IEnumerable<MessageDto>>> GetMessagesByRoomAsync(RoomId roomId, int skip = 0, int take = 50);
        Task<Result<MessageDto>> SendMessageAsync(RoomId roomId, UserId senderId, string senderDisplayName, SendMessageDto message);
        Task<Result> DeleteMessageAsync(Guid messageId);
    }
}
