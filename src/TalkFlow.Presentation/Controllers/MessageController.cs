using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalkFlow.Application.Commands.Message.SendMessage;
using TalkFlow.Application.DTOs.Message;
using TalkFlow.Application.Queries.Message.GetMessages;
using TalkFlow.Application.Services;
using TalkFlow.Domain.Aggregates.User;
using TalkFlow.Domain.ValueObjects.Room;

namespace TalkFlow.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly IMediator _mediator;

        public MessageController(IMessageService messageService, IMediator mediator)
        {
            _messageService = messageService;
            _mediator = mediator;
        }

        [HttpGet("room/{roomId}")]
        public async Task<IActionResult> GetByRoom(Guid roomId, [FromQuery] int skip = 0, [FromQuery] int take = 50)
        {
            var roomIdValue = new RoomId(roomId);
            var result = await _mediator.Send(new GetMessagesQuery(roomIdValue, skip, take));
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }

        [HttpPost("room/{roomId}")]
        public async Task<IActionResult> Send(Guid roomId, [FromBody] SendMessageDto dto)
        {
            var userId = User.FindFirst("user_id")?.Value;
            var display = User.FindFirst("display_name")?.Value ?? string.Empty;
            if (!Guid.TryParse(userId, out var uid)) return Unauthorized();

            var roomIdValue = new RoomId(roomId);
            var userIdValue = new Domain.ValueObjects.User.UserId(uid);
            var result = await _mediator.Send(new SendMessageCommand(roomIdValue, userIdValue, display, dto));
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }

        [HttpDelete("{messageId}")]
        public async Task<IActionResult> Delete(Guid messageId)
        {
            var result = await _messageService.DeleteMessageAsync(messageId);
            return result.IsSuccess ? NoContent() : BadRequest(result.Error);
        }
    }
}
