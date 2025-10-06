using TalkFlow.Dtos;
using TalkFlow.Entities;
using TalkFlow.Extensions;
using TalkFlow.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace TalkFlow.Controllers
{
    /// <summary>
    /// API Controller for stranger matching and random video calls
    /// </summary>
    [Route("api/v1/strangers")]
    [Produces("application/json")]
    [Authorize]
    public class StrangerController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public StrangerController(IUnitOfWork unitOfWork,
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
        /// Create a new stranger matching session
        /// </summary>
        /// <param name="register">Stranger session details including filters for age, gender, nationality</param>
        /// <returns>Created stranger session with user and room information</returns>
        /// <response code="200">Stranger session created successfully</response>
        /// <response code="400">Invalid session data or creation failed</response>
        [HttpPost]
        [HttpPost("/api/Stranger/add-stranger")] // Legacy compatibility
        [AllowAnonymous]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<object>> CreateStrangerSession(RegisterStrangerDto register)
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
                StrangerFilter = register.StrangerFilter,
                Gender = register.Gender,
                Age = register.Age,
                Nationality = register.Nationality,
                //Type = register.Type,
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
        /// Join a stranger room for random video call
        /// </summary>
        /// <param name="roomId">The unique identifier of the stranger room to join</param>
        /// <param name="join">Join request with security code</param>
        /// <returns>User information and room details after successful join</returns>
        /// <response code="200">Successfully joined stranger room</response>
        /// <response code="400">User doesn't have valid stranger filter</response>
        /// <response code="401">Invalid security code</response>
        /// <response code="404">Stranger room not found</response>
        [HttpPost("{roomId}/join")]
        [HttpPost("/api/Stranger/join-stranger")] // Legacy compatibility  
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<object>> JoinStrangerRoom(Guid roomId, [FromBody] JoinStrangerRoomDto join)
        {
            var room = await _unitOfWork.RoomRepository.GetRoomById(roomId);
            if (room == null)
                return NotFound();

            if ((string.IsNullOrEmpty(room.SecurityCode) && string.IsNullOrEmpty(join.SecurityCode)) || room.SecurityCode == join.SecurityCode)
            {
                var user = await _unitOfWork.UserRepository.GetUserByIdAsync(HttpContext.User.GetUserId());
                if (user?.StrangerFilter == null)
                    return BadRequest("User doesn't have filter");

                user.StrangerFilter.CurrentRoom = room;
                await _unitOfWork.Complete();

                var userDto = new UserDto
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    DisplayName = user.DisplayName,
                    LastActive = user.LastActive,
                    StrangerFilter = _mapper.Map<StrangerFilterDto>(user.StrangerFilter),
                    Gender = user.Gender,
                    Age = user.Age,
                    Nationality = user.Nationality,
                    //Type = user.Type,
                    Token = await HttpContext.GetTokenAsync(JwtBearerDefaults.AuthenticationScheme, "access_token") ?? string.Empty
                };

                return Ok(new
                {
                    User = userDto,
                    Room = _mapper.Map<RoomDto>(room)
                });
            }
            return Unauthorized();
        }
    }
}


