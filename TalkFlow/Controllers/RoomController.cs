using TalkFlow.Dtos;
using TalkFlow.Entities;
using TalkFlow.Extensions;
using TalkFlow.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace TalkFlow.Controllers
{
    /// <summary>
    /// API Controller for managing chat rooms and video meetings
    /// </summary>
    [Route("api/v1/rooms")]
    [Produces("application/json")]
    [Authorize]
    public class RoomController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public RoomController(IUnitOfWork unitOfWork,
            IMapper mapper,
            ITokenService tokenService,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _tokenService = tokenService;
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }

        /// <summary>
        /// Get all available rooms
        /// </summary>
        /// <returns>List of active rooms</returns>
        /// <response code="200">Returns the list of rooms</response>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<RoomDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<RoomDto>>> GetRooms()
        {
            // Implementation for getting all rooms - you may need to add this method to repository
            return Ok(new List<RoomDto>());
        }

        /// <summary>
        /// Get specific room details by ID
        /// </summary>
        /// <param name="roomId">The unique identifier of the room</param>
        /// <returns>Room details</returns>
        /// <response code="200">Returns the room details</response>
        /// <response code="404">Room not found</response>
        [HttpGet("{roomId}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(RoomDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RoomDto>> GetRoom(Guid roomId)
        {
            var room = await _unitOfWork.RoomRepository.GetRoomDtoById(roomId);
            if (room == null) return NotFound();
            return Ok(room);
        }

        /// <summary>
        /// Create a new chat room
        /// </summary>
        /// <param name="register">Room creation details including name, security code, and host info</param>
        /// <returns>Created room with host user information</returns>
        /// <response code="200">Room created successfully</response>
        /// <response code="400">Invalid room data or creation failed</response>
        [HttpPost]
        [HttpPost("/api/Room/add-room")] // Legacy compatibility
        [AllowAnonymous]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<object>> CreateRoom(RegisterDto register)
        {
            var user = _mapper.Map<AppUser>(register);
            user.UserName = Guid.NewGuid().ToString().ToLower();

            string password = Guid.NewGuid().ToString();
            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded) return BadRequest(result.Errors);

            var roleResult = await _userManager.AddToRoleAsync(user, "Host");
            if (!roleResult.Succeeded) return BadRequest(roleResult.Errors);

            var userDto = new UserDto
            {
                UserId = user.Id,
                UserName = user.UserName,
                DisplayName = user.DisplayName,
                LastActive = user.LastActive,
                Token = await _tokenService.CreateTokenAsync(user)
            };

            var fakeLogin = await _signInManager.CheckPasswordSignInAsync(user, password, false);

            if (!fakeLogin.Succeeded) return BadRequest("Something wrong when create room");

            var room = new Room
            {
                RoomName = register.RoomName,
                SecurityCode = register.SecurityCode,
                UserId = user.Id,
                CreatedDate = DateTime.UtcNow
            };

            _unitOfWork.RoomRepository.AddRoom(room);

            if (await _unitOfWork.Complete())
            {
                return Ok(new
                {
                    User = userDto,
                    Room = await _unitOfWork.RoomRepository.GetRoomDtoById(room.RoomId)
                });
            }

            return BadRequest("Problem adding room");
        }

        /// <summary>
        /// Join an existing room
        /// </summary>
        /// <param name="roomId">The unique identifier of the room to join</param>
        /// <param name="join">User details for joining the room including display name and security code</param>
        /// <returns>User information and room details after successful join</returns>
        /// <response code="200">Successfully joined the room</response>
        /// <response code="400">Invalid join request or user creation failed</response>
        /// <response code="401">Invalid security code</response>
        /// <response code="404">Room not found</response>
        [HttpPost("{roomId}/join")]
        [HttpPost("/api/Room/join-room")] // Legacy compatibility
        [AllowAnonymous]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<object>> JoinRoom(Guid roomId, [FromBody] JoinRoomDto join)
        {
            var room = await _unitOfWork.RoomRepository.GetRoomById(roomId);
            if (room == null)
                return NotFound();

            if ((string.IsNullOrEmpty(room.SecurityCode) && string.IsNullOrEmpty(join.SecurityCode)) || room.SecurityCode == join.SecurityCode)
            {
                var user = _mapper.Map<AppUser>(join);
                user.UserName = Guid.NewGuid().ToString().ToLower();

                string password = Guid.NewGuid().ToString();
                var result = await _userManager.CreateAsync(user, password);

                if (!result.Succeeded) return BadRequest(result.Errors);

                if (room.Connections.Any())
                {
                    var roleResult = await _userManager.AddToRoleAsync(user, "Member");
                    if (!roleResult.Succeeded) return BadRequest(roleResult.Errors);
                }
                else
                {
                    var roleResult = await _userManager.AddToRoleAsync(user, "Host");
                    if (!roleResult.Succeeded) return BadRequest(roleResult.Errors);
                }

                var userDto = new UserDto
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    DisplayName = user.DisplayName,
                    LastActive = user.LastActive,
                    Token = await _tokenService.CreateTokenAsync(user)
                };

                var fakeLogin = await _signInManager.CheckPasswordSignInAsync(user, password, false);

                if (!fakeLogin.Succeeded) return BadRequest("Something wrong when create room");

                return Ok(new
                {
                    User = userDto,
                    Room = _mapper.Map<RoomDto>(room)
                });
            }
            return Unauthorized();
        }

        /// <summary>
        /// Update room settings (Host only)
        /// </summary>
        /// <param name="roomId">The unique identifier of the room to update</param>
        /// <param name="edit">Room update details</param>
        /// <returns>Updated room information</returns>
        /// <response code="200">Room updated successfully</response>
        /// <response code="204">No changes made</response>
        /// <response code="400">Update failed</response>
        /// <response code="401">Not authorized (Host only)</response>
        /// <response code="404">Room not found</response>
        [HttpPut("{roomId}")]
        [Authorize(Roles = "Admin,Host")]
        [ProducesResponseType(typeof(RoomDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RoomDto>> UpdateRoom(Guid roomId, [FromBody] EditRoomDto edit)
        {
            Room? room = await _unitOfWork.RoomRepository.GetRoomById(roomId);
            if (room == null) return NotFound();
            if (room.UserId != HttpContext.User.GetUserId())
                return Unauthorized();
            room = await _unitOfWork.RoomRepository.EditRoom(edit);
            if (room != null)
            {
                if (_unitOfWork.HasChanges())
                {
                    if (await _unitOfWork.Complete())
                        return Ok(new RoomDto
                        {
                            RoomId = room.RoomId,
                            RoomName = room.RoomName,
                            UserId = room.UserId
                        });
                    return BadRequest("Problem edit room");
                }
                else
                {
                    return NoContent();
                }
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Delete a room (Host/Admin only)
        /// </summary>
        /// <param name="roomId">The unique identifier of the room to delete</param>
        /// <returns>Deleted room information</returns>
        /// <response code="200">Room deleted successfully</response>
        /// <response code="400">Deletion failed</response>
        /// <response code="401">Not authorized (Host/Admin only)</response>
        /// <response code="404">Room not found</response>
        [HttpDelete("{roomId}")]
        [Authorize(Roles = "Admin,Host")]
        [ProducesResponseType(typeof(RoomDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RoomDto>> DeleteRoom(Guid roomId)
        {
            Room? room = await _unitOfWork.RoomRepository.GetRoomById(roomId);
            if (room == null) return NotFound();
            if (room.UserId != HttpContext.User.GetUserId())
                return Unauthorized();
            var entity = await _unitOfWork.RoomRepository.DeleteRoom(roomId);

            if (entity != null)
            {
                if (await _unitOfWork.Complete())
                    return Ok(new RoomDto
                    {
                        RoomId = entity.RoomId,
                        RoomName = entity.RoomName,
                        UserId = entity.UserId
                    });
                return BadRequest("Problem delete room");
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteAllRooms()
        {
            await _unitOfWork.RoomRepository.DeleteAllRoom();

            if (_unitOfWork.HasChanges())
            {
                if (await _unitOfWork.Complete())
                    return Ok();//xoa thanh cong
                return BadRequest("Problem delete all room");
            }
            else
            {
                return NoContent();//ko co gi de xoa
            }
        }
    }
}


