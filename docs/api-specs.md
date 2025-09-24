# API Specifications

## Tổng quan

- **Base URL**: `https://localhost:5001`
- **Auth**: JWT Bearer Token
- **Content-Type**: `application/json`
- **Stack**: ASP.NET Core 8 Web API + SignalR + EF Core 8 + MediatR (CQRS - đang phát triển)

## Authentication Flow (ẩn danh)

1. Tạo user/phòng bằng API public (không cần đăng nhập ban đầu)
2. Backend khởi tạo user ẩn danh và trả về thông tin user + token (nếu áp dụng)
3. Client dùng JWT để call API bảo vệ và connect SignalR hubs

## REST API (Presentation Layer)

### User

- POST `api/user`

  - Tạo user ẩn danh
  - Body: CreateUserDto

  ```json
  {
    "displayName": "string(3-50)",
    "email": "string?",
    "gender": "Male|Female|Other|PreferNotToSay?",
    "age": 13,
    "nationality": "string?",
    "photoUrl": "url?",
    "strangerFilter": {
      "findGender": ["Male"],
      "minAge": 0,
      "maxAge": 100,
      "findRegion": ["VN"]
    }
  }
  ```

  - 200: UserDto

- GET `api/user/{userId}` (Auth)

  - Trả về UserDto

- GET `api/user?pageNumber=1&pageSize=10` (Auth)

  - Trả về PaginatedResult<UserDto>

- PUT `api/user/{userId}` (Auth)

  - Cập nhật thông tin user: UpdateUserDto

- PUT `api/user/{userId}/lock` (Auth)
- PUT `api/user/{userId}/unlock` (Auth)
- DELETE `api/user/{userId}` (Auth)

### Room

- POST `api/room`

  - Tạo phòng mới và user host ẩn danh tương tự logic cũ
  - Body: CreateRoomDto

  ```json
  {
    "roomName": "string(3-100)",
    "securityCode": "string(4-20)?",
    "displayName": "string(3-50)"
  }
  ```

  - 200: { user: UserDto, room: RoomDto }

- POST `api/room/join`

  - Tham gia phòng bằng RoomId (+ optional securityCode)
  - Body: JoinRoomDto

  ```json
  {
    "roomId": "guid",
    "displayName": "string(3-50)",
    "securityCode": "string?"
  }
  ```

  - 200: { user: UserDto, room: RoomDto }

- GET `api/room/{roomId}` (Auth)
- GET `api/room?pageNumber=1&pageSize=10` (Auth)
- PUT `api/room/{roomId}` (Auth)
  - Body: UpdateRoomDto { roomName, securityCode }
- DELETE `api/room/{roomId}` (Auth)
- PUT `api/room/{roomId}/block-chat` (Auth)
- PUT `api/room/{roomId}/unblock-chat` (Auth)

Ghi chú quyền: Các thao tác update/delete/block thường yêu cầu user hiện tại là Host của phòng.

## SignalR Hubs

### ChatHub `/hubs/chathub` (Authorize)

- Methods client->server:
  - JoinRoom(roomId: string)
  - LeaveRoom(roomId: string)
  - SendMessage(roomId: string, { content: string(1-1000) })
  - MuteMicrophone(roomId: string, isMuted: bool)
  - MuteCamera(roomId: string, isMuted: bool)
  - ShareScreen(roomId: string, isSharing: bool)
  - SendWebRTCOffer(roomId: string, targetUserId: string, offer: string)
  - SendWebRTCAnswer(roomId: string, targetUserId: string, answer: string)
  - SendIceCandidate(roomId: string, targetUserId: string, candidate: string)
- Events server->client:
  - UserJoined, UserLeft, ReceiveMessage, MicrophoneMuted, CameraMuted, ScreenSharing,
    ReceiveWebRTCOffer, ReceiveWebRTCAnswer, ReceiveIceCandidate

### PresenceHub `/hubs/presence` (Authorize)

- Theo dõi user connect/disconnect (mở rộng sau dựa trên nhu cầu)

### StrangerHub `/hubs/stranger` (Authorize)

- Methods: StartMatching(), StopMatching()
- Events: MatchingStarted, MatchingStopped, MatchFound (mở rộng khi thêm matching engine)

## DTOs chính

- **UserDto**: { userId, userName, displayName, email, photoUrl, lastActive, isLocked, gender?, age?, nationality?, strangerFilter?, token }
- **RoomDto**: { roomId, roomName, hostId, hostDisplayName, memberCount, createdAt, isChatBlocked, isActive, securityCode? }
- **MessageDto**: { id, senderId, senderDisplayName, content, sentAt, isDeleted }
- **CreateUserDto**: { displayName, email?, gender?, age?, nationality?, photoUrl?, strangerFilter? }
- **CreateRoomDto**: { roomName, securityCode?, displayName }
- **JoinRoomDto**: { roomId, displayName, securityCode? }
- **UpdateUserDto**: { displayName?, email?, gender?, age?, nationality?, photoUrl? }
- **UpdateRoomDto**: { roomName?, securityCode? }
- **SendMessageDto**: { content }

## Lỗi chuẩn

- 400, 401, 403, 404, 500 như thông lệ REST

## CORS

- Cho phép mọi origin ở dev; prod cấu hình domain cụ thể

## WebRTC

- Server làm signaling qua ChatHub; client chịu trách nhiệm P2P, cần STUN/TURN khi triển khai Internet
