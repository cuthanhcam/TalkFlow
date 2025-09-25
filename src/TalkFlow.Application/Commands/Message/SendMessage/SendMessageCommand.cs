using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkFlow.Application.Common.Interfaces;
using TalkFlow.Application.DTOs.Common;
using TalkFlow.Application.DTOs.Message;
using TalkFlow.Domain.ValueObjects.Room;
using TalkFlow.Domain.ValueObjects.User;

namespace TalkFlow.Application.Commands.Message.SendMessage
{
    public record SendMessageCommand(RoomId RoomId, UserId SenderId, string SenderDisplayName, SendMessageDto Message) : ICommand<Result<MessageDto>>;
}
