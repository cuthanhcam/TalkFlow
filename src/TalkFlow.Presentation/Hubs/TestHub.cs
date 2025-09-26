using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;

namespace TalkFlow.Presentation.Hubs
{
    public class TestHub : Hub
    {
        public async Task JoinRoom(string roomId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
            await Clients.Group(roomId).SendAsync("UserJoined", new { displayName = "TestUser", connectionId = Context.ConnectionId });
        }

        public async Task LeaveRoom(string roomId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
            await Clients.Group(roomId).SendAsync("UserLeft", new { displayName = "TestUser", connectionId = Context.ConnectionId });
        }

        public async Task SendMessage(string roomId, string content)
        {
            await Clients.Group(roomId).SendAsync("MessageReceived", new
            {
                senderId = Context.ConnectionId,
                senderDisplayName = "TestUser",
                content = content,
                sentAt = DateTime.UtcNow
            });
        }

        public async Task UpdatePresence(string status)
        {
            await Clients.All.SendAsync("UserOnline", new { displayName = "TestUser", status = status });
        }

        public async Task StartMatching()
        {
            await Clients.Caller.SendAsync("MatchFound", new { roomId = "test-room-123", partnerUserId = "test-partner" });
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync("Connected", new { message = "Connected to Test Hub", connectionId = Context.ConnectionId });
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await Clients.All.SendAsync("UserOffline", new { displayName = "TestUser", connectionId = Context.ConnectionId });
            await base.OnDisconnectedAsync(exception);
        }
    }
}
