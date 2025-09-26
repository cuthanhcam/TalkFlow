using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using TalkFlow.Domain.Services;
using TalkFlow.Domain.ValueObjects.User;

namespace TalkFlow.Presentation.Hubs
{
    [Authorize]
    public class StrangerHub : Hub
    {
        private readonly IStrangerMatchingService _matchingService;

        public StrangerHub(IStrangerMatchingService matchingService)
        {
            _matchingService = matchingService;
        }

        public async Task StartMatching()
        {
            await Clients.Caller.SendAsync("MatchingStarted");
            var userIdStr = Context.User?.FindFirst("user_id")?.Value;
            if (Guid.TryParse(userIdStr, out var userGuid))
            {
                var result = await _matchingService.TryMatchAsync(new UserId(userGuid));
                if (result.IsSuccess && result.RoomId != null && result.PartnerId != null)
                {
                    await Clients.Caller.SendAsync("MatchFound", new { roomId = result.RoomId, partnerUserId = result.PartnerId });
                }
            }
        }

        public async Task StopMatching()
        {
            await Clients.Caller.SendAsync("MatchingStopped");
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}
