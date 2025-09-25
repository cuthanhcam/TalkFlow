using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkFlow.Application.DTOs.Common;
using TalkFlow.Application.DTOs.Message;
using TalkFlow.Application.Mappings;
using TalkFlow.Domain.Repositories;

namespace TalkFlow.Application.Queries.Message.GetMessages
{
    public class GetMessagesQueryHandler : IRequestHandler<GetMessagesQuery, Result<IEnumerable<MessageDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetMessagesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<IEnumerable<MessageDto>>> Handle(GetMessagesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var messages = await _unitOfWork.Messages.GetMessagesByRoomAsync(request.RoomId, request.Skip, request.Take);
                var messageDtos = messages.Select(MessageMappingProfile.ToDto);

                return Result<IEnumerable<MessageDto>>.Success(messageDtos);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<MessageDto>>.Failure($"Failed to get messages: {ex.Message}");
            }
        }
    }
}
