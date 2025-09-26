using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalkFlow.Application.Commands.User.CreateUser;
using TalkFlow.Application.Commands.User.UpdateUser;
using TalkFlow.Application.DTOs.User;
using TalkFlow.Application.Queries.User.GetUser;
using TalkFlow.Application.Services;
using TalkFlow.Domain.ValueObjects.User;

namespace TalkFlow.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMediator _mediator;

        public UserController(IUserService userService, IMediator mediator)
        {
            _userService = userService;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto userData)
        {
            var result = await _mediator.Send(new CreateUserCommand(userData));

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(result.Error);
        }

        [HttpGet("{userId}")]
        [Authorize]
        public async Task<IActionResult> GetUser(Guid userId)
        {
            var userDto = new UserId(userId);
            var result = await _mediator.Send(new GetUserQuery(userDto));

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return NotFound(result.Error);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUsers([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _userService.GetUsersAsync(pageNumber, pageSize);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(result.Error);
        }

        [HttpPut("{userId}")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(Guid userId, [FromBody] UpdateUserDto userData)
        {
            var userDto = new UserId(userId);
            var result = await _mediator.Send(new UpdateUserCommand(userDto, userData));

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(result.Error);
        }

        [HttpPut("{userId}/lock")]
        [Authorize]
        public async Task<IActionResult> LockUser(Guid userId)
        {
            var userDto = new UserId(userId);
            var result = await _userService.LockUserAsync(userDto);

            if (result.IsSuccess)
            {
                return NoContent();
            }

            return BadRequest(result.Error);
        }

        [HttpPut("{userId}/unlock")]
        [Authorize]
        public async Task<IActionResult> UnlockUser(Guid userId)
        {
            var userDto = new UserId(userId);
            var result = await _userService.UnlockUserAsync(userDto);

            if (result.IsSuccess)
            {
                return NoContent();
            }

            return BadRequest(result.Error);
        }

        [HttpDelete("{userId}")]
        [Authorize]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            var userDto = new UserId(userId);
            var result = await _userService.DeleteUserAsync(userDto);

            if (result.IsSuccess)
            {
                return NoContent();
            }

            return BadRequest(result.Error);
        }
    }
}
