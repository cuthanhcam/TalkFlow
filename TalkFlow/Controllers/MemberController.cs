using TalkFlow.Dtos;
using TalkFlow.Extensions;
using TalkFlow.Helpers;
using TalkFlow.Interfaces;
using TalkFlow.SignalR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace TalkFlow.Controllers
{
    /// <summary>
    /// API Controller for managing room members and user operations
    /// </summary>
    [Route("api/v1/members")]
    [Produces("application/json")]
    [Authorize]
    public class MemberController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<PresenceHub> _presenceHub;
        private readonly PresenceTracker _presenceTracker;

        public MemberController(IUnitOfWork unitOfWork, IHubContext<PresenceHub> presenceHub, PresenceTracker presenceTracker)
        {
            _unitOfWork = unitOfWork;
            _presenceHub = presenceHub;
            _presenceTracker = presenceTracker;
        }

        /// <summary>
        /// Get all members with pagination
        /// </summary>
        /// <param name="userParams">Pagination and filtering parameters</param>
        /// <returns>Paginated list of members</returns>
        /// <response code="200">Returns paginated member list</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<MemberDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetAllMembers([FromQuery] UserParams userParams)
        {
            userParams.CurrentDisplayName = User.GetDisplayName();
            var comments = await _unitOfWork.UserRepository.GetMembersAsync(userParams);
            Response.AddPaginationHeader(comments.CurrentPage, comments.PageSize, comments.TotalCount, comments.TotalPages);

            return Ok(comments);
        }

        /// <summary>
        /// Get specific member by ID
        /// </summary>
        /// <param name="userId">The unique identifier of the member</param>
        /// <returns>Member details</returns>
        /// <response code="200">Returns member details</response>
        /// <response code="404">Member not found</response>
        [HttpGet("{userId}")]
        [ProducesResponseType(typeof(MemberDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MemberDto>> GetMember(Guid userId)
        {
            return Ok(await _unitOfWork.UserRepository.GetMemberAsync(userId));
        }

        /// <summary>
        /// Lock/unlock a user (Admin/Host only)
        /// </summary>
        /// <param name="userId">The unique identifier of the user to lock</param>
        /// <returns>No content on success</returns>
        /// <response code="204">User lock status updated successfully</response>
        /// <response code="400">User not found</response>
        /// <response code="401">Not authorized (Admin/Host only)</response>
        [HttpPut("{userId}/lock")]
        [Authorize(Roles = "Admin,Host")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> LockUser(Guid userId)
        {
            var u = await _unitOfWork.UserRepository.UpdateLocked(userId);
            if (u != null)
            {
                var connections = await _presenceTracker.GetConnectionsForUserID(userId);
                await _presenceHub.Clients.Clients(connections).SendAsync("OnLockedUser", true);
                return NoContent();
            }
            else
            {
                return BadRequest("Can not find given username");
            }
        }
    }
}


