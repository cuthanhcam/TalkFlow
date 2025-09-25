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

namespace TalkFlow.Application.Commands.Message.SendMessage
{
    public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand, Result<MessageDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public SendMessageCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<MessageDto>> Handle(SendMessageCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var room = await _unitOfWork.Rooms.GetByIdAsync(request.RoomId);
                if (room == null)
                {
                    return Result<MessageDto>.Failure("Room not found");
                }

                if (room.IsChatBlocked)
                {
                    return Result<MessageDto>.Failure("Chat is blocked in this room");
                }

                var message = new TalkFlow.Domain.Aggregates.Room.Message(request.RoomId, request.SenderId, request.SenderDisplayName, request.Message.Content);
                room.AddMessage(message);

                await _unitOfWork.Messages.AddAsync(message);
                await _unitOfWork.Rooms.UpdateAsync(room);
                await _unitOfWork.SaveChangesAsync();

                var messageDto = MessageMappingProfile.ToDto(message);
                return Result<MessageDto>.Success(messageDto);
            }
            catch (Exception ex)
            {
                return Result<MessageDto>.Failure($"Failed to send message: {ex.Message}");
            }
        }
    }
}
