using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalkFlow.Application.Commands.Room.CreateRoom;
using TalkFlow.Application.Commands.Room.JoinRoom;
using TalkFlow.Application.Commands.Room.UpdateRoom;
using TalkFlow.Application.DTOs.Room;
using TalkFlow.Application.Services;
using TalkFlow.Domain.Aggregates.User;
using TalkFlow.Domain.ValueObjects.Room;

namespace TalkFlow.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;
        private readonly IMediator _mediator;

        public RoomController(IRoomService roomService, IMediator mediator)
        {
            _roomService = roomService;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoom([FromBody] CreateRoomDto roomData)
        {
            var result = await _mediator.Send(new CreateRoomCommand(roomData));

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(result.Error);
        }

        [HttpPost("join")]
        public async Task<IActionResult> JoinRoom([FromBody] JoinRoomDto joinData)
        {
            var result = await _mediator.Send(new JoinRoomCommand(joinData));

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(result.Error);
        }

        [HttpGet("{roomId}")]
        [Authorize]
        public async Task<IActionResult> GetRoom(Guid roomId)
        {
            var roomDto = new RoomId(roomId);
            var result = await _roomService.GetRoomByIdAsync(roomDto);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return NotFound(result.Error);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetRooms([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _roomService.GetRoomsAsync(pageNumber, pageSize);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(result.Error);
        }

        [HttpPut("{roomId}")]
        [Authorize]
        public async Task<IActionResult> UpdateRoom(Guid roomId, [FromBody] UpdateRoomDto roomData)
        {
            var roomDto = new RoomId(roomId);
            var result = await _mediator.Send(new UpdateRoomCommand(roomDto, roomData));

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(result.Error);
        }

        [HttpDelete("{roomId}")]
        [Authorize]
        public async Task<IActionResult> DeleteRoom(Guid roomId)
        {
            var roomDto = new RoomId(roomId);
            var userId = GetCurrentUserId();
            var result = await _roomService.DeleteRoomAsync(roomDto, userId);

            if (result.IsSuccess)
            {
                return NoContent();
            }

            return BadRequest(result.Error);
        }

        [HttpPut("{roomId}/block-chat")]
        [Authorize]
        public async Task<IActionResult> BlockChat(Guid roomId)
        {
            var roomDto = new RoomId(roomId);
            var userId = GetCurrentUserId();
            var result = await _roomService.BlockChatAsync(roomDto, userId);

            if (result.IsSuccess)
            {
                return NoContent();
            }

            return BadRequest(result.Error);
        }

        [HttpPut("{roomId}/unblock-chat")]
        [Authorize]
        public async Task<IActionResult> UnblockChat(Guid roomId)
        {
            var roomDto = new RoomId(roomId);
            var userId = GetCurrentUserId();
            var result = await _roomService.UnblockChatAsync(roomDto, userId);

            if (result.IsSuccess)
            {
                return NoContent();
            }

            return BadRequest(result.Error);
        }

        private Domain.ValueObjects.User.UserId GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst("user_id")?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                throw new UnauthorizedAccessException("Invalid user ID");
            }
            return new Domain.ValueObjects.User.UserId(userId);
        }
    }
}
