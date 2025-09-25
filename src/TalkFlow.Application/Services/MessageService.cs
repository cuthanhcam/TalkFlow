using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkFlow.Application.DTOs.Common;
using TalkFlow.Application.DTOs.Message;
using TalkFlow.Application.Mappings;
using TalkFlow.Domain.Aggregates.Room;
using TalkFlow.Domain.Repositories;
using TalkFlow.Domain.ValueObjects.Room;
using TalkFlow.Domain.ValueObjects.User;

namespace TalkFlow.Application.Services
{
    public class MessageService : IMessageService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MessageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<IEnumerable<MessageDto>>> GetMessagesByRoomAsync(RoomId roomId, int skip = 0, int take = 50)
        {
            var messages = await _unitOfWork.Messages.GetMessagesByRoomAsync(roomId, skip, take);
            var dtos = messages.Select(MessageMappingProfile.ToDto);
            return Result<IEnumerable<MessageDto>>.Success(dtos);
        }

        public async Task<Result<MessageDto>> SendMessageAsync(RoomId roomId, UserId senderId, string senderDisplayName, SendMessageDto message)
        {
            var room = await _unitOfWork.Rooms.GetByIdAsync(roomId);
            if (room == null) return Result<MessageDto>.Failure("Room not found");
            if (room.IsChatBlocked) return Result<MessageDto>.Failure("Chat is blocked");

            var entity = new Message(roomId, senderId, senderDisplayName, message.Content);
            room.AddMessage(entity);
            await _unitOfWork.Messages.AddAsync(entity);
            await _unitOfWork.Rooms.UpdateAsync(room);
            await _unitOfWork.SaveChangesAsync();

            return Result<MessageDto>.Success(MessageMappingProfile.ToDto(entity));
        }

        public async Task<Result> DeleteMessageAsync(Guid messageId)
        {
            var msg = await _unitOfWork.Messages.GetByIdAsync(messageId);
            if (msg == null) return Result.Failure("Message not found");
            msg.Delete();
            await _unitOfWork.Messages.UpdateAsync(msg);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }
    }
}
