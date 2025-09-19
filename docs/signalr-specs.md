# SignalR & WebRTC Specifications

## Tổng quan
- Hubs chạy trong `TalkFlow.Presentation`
- Auth: JWT bắt buộc cho tất cả hubs
- Client phải truyền access token khi khởi tạo connection

## ChatHub (`/hubs/chathub`)

### Client → Server Methods
- `JoinRoom(roomId: string)`
- `LeaveRoom(roomId: string)`
- `SendMessage(roomId: string, message: { content: string(1-1000) })`
- `MuteMicrophone(roomId: string, isMuted: boolean)`
- `MuteCamera(roomId: string, isMuted: boolean)`
- `ShareScreen(roomId: string, isSharing: boolean)`
- `SendWebRTCOffer(roomId: string, targetUserId: string, offer: string)`
- `SendWebRTCAnswer(roomId: string, targetUserId: string, answer: string)`
- `SendIceCandidate(roomId: string, targetUserId: string, candidate: string)`

### Server → Client Events
- `UserJoined` { userDisplayName }
- `UserLeft` { userDisplayName }
- `ReceiveMessage` { senderId, senderDisplayName, content, sentAt }
- `MicrophoneMuted` { userId, isMuted }
- `CameraMuted` { userId, isMuted }
- `ScreenSharing` { userId, userDisplayName, isSharing }
- `ReceiveWebRTCOffer` { fromUserId, offer, roomId }
- `ReceiveWebRTCAnswer` { fromUserId, answer, roomId }
- `ReceiveIceCandidate` { fromUserId, candidate, roomId }

## PresenceHub (`/hubs/presence`)
- Theo dõi kết nối người dùng; có thể mở rộng thêm events: `UserOnline`, `UserOffline`.

## StrangerHub (`/hubs/stranger`)

### Client → Server
- `StartMatching()`
- `StopMatching()`

### Server → Client
- `MatchingStarted`
- `MatchingStopped`
- `MatchFound` { roomId, partnerUserId } (dự kiến khi bổ sung matching engine)

## Kết nối từ client (ví dụ TypeScript)
```ts
import * as signalR from '@microsoft/signalr';

const token = '<jwt>'; // nhận từ API
const chatConnection = new signalR.HubConnectionBuilder()
  .withUrl('/hubs/chathub', { accessTokenFactory: () => token })
  .withAutomaticReconnect()
  .build();

await chatConnection.start();
await chatConnection.invoke('JoinRoom', roomId);

chatConnection.on('ReceiveMessage', (msg) => console.log(msg));
```

## WebRTC Notes
- Server chỉ chuyển tiếp offer/answer/ICE qua SignalR.
- Client tạo RTCPeerConnection, gọi `setLocalDescription`, gửi offer qua hub, nhận answer và ICE của peer.
- Local dev đủ chạy STUN miễn phí; production cần TURN (coturn) để vượt NAT.
