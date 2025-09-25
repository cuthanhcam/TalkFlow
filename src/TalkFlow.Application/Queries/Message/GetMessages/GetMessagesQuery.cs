using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkFlow.Application.Common.Interfaces;
using TalkFlow.Application.DTOs.Common;
using TalkFlow.Application.DTOs.Message;
using TalkFlow.Domain.ValueObjects.Room;

namespace TalkFlow.Application.Queries.Message.GetMessages
{
    public record GetMessagesQuery(RoomId RoomId, int Skip = 0, int Take = 50) : IQuery<Result<IEnumerable<MessageDto>>>;
}
