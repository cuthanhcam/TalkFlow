using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;
using TalkFlow.Application.DTOs.Message;

namespace TalkFlow.Presentation.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        public async Task JoinRoom(string roomId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
            await Clients.Group(roomId).SendAsync("UserJoined", Context.User?.Identity?.Name);
        }

        public async Task LeaveRoom(string roomId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
            await Clients.Group(roomId).SendAsync("UserLeft", Context.User?.Identity?.Name);
        }

        public async Task SendMessage(string roomId, SendMessageDto message)
        {
            await Clients.Group(roomId).SendAsync("ReceiveMessage", new
            {
                SenderId = Context.User?.FindFirst("user_id")?.Value,
                SenderDisplayName = Context.User?.FindFirst("display_name")?.Value,
                Content = message.Content,
                SentAt = DateTime.UtcNow
            });
        }

        public async Task MuteMicrophone(string roomId, bool isMuted)
        {
            await Clients.Group(roomId).SendAsync("MicrophoneMuted", new
            {
                UserId = Context.User?.FindFirst("user_id")?.Value,
                IsMuted = isMuted
            });
        }

        public async Task MuteCamera(string roomId, bool isMuted)
        {
            await Clients.Group(roomId).SendAsync("CameraMuted", new
            {
                UserId = Context.User?.FindFirst("user_id")?.Value,
                IsMuted = isMuted
            });
        }

        public async Task ShareScreen(string roomId, bool isSharing)
        {
            await Clients.Group(roomId).SendAsync("ScreenSharing", new
            {
                UserId = Context.User?.FindFirst("user_id")?.Value,
                UserDisplayName = Context.User?.FindFirst("display_name")?.Value,
                IsSharing = isSharing
            });
        }

        public async Task SendWebRTCOffer(string roomId, string targetUserId, string offer)
        {
            await Clients.User(targetUserId).SendAsync("ReceiveWebRTCOffer", new
            {
                FromUserId = Context.User?.FindFirst("user_id")?.Value,
                Offer = offer,
                RoomId = roomId
            });
        }

        public async Task SendWebRTCAnswer(string roomId, string targetUserId, string answer)
        {
            await Clients.User(targetUserId).SendAsync("ReceiveWebRTCAnswer", new
            {
                FromUserId = Context.User?.FindFirst("user_id")?.Value,
                Answer = answer,
                RoomId = roomId
            });
        }

        public async Task SendIceCandidate(string roomId, string targetUserId, string candidate)
        {
            await Clients.User(targetUserId).SendAsync("ReceiveIceCandidate", new
            {
                FromUserId = Context.User?.FindFirst("user_id")?.Value,
                Candidate = candidate,
                RoomId = roomId
            });
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
