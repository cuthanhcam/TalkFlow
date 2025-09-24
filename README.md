# TalkFlow - Video Chat Application

## Tổng quan
TalkFlow là một ứng dụng video chat realtime được xây dựng theo kiến trúc Domain-Driven Design (DDD) với .NET 8.0. Ứng dụng cho phép người dùng tạo phòng chat, tham gia phòng, gọi video trực tiếp và ghép cặp ngẫu nhiên với người lạ.

## Kiến trúc DDD

### 1. Domain Layer (`TalkFlow.Domain`)
- **Aggregates**: User, Room, Message, StrangerFilter, Match
- **Value Objects**: UserId, RoomId, DisplayName, Gender, Age, RoomName, SecurityCode, etc.
- **Domain Events**: UserCreated, RoomCreated, MessageSent, UserJoined, UserLeft, etc.
- **Domain Services**: IUserDomainService, IRoomDomainService, IStrangerMatchingService
- **Repositories**: IUserRepository, IRoomRepository, IMessageRepository, IMatchRepository
- **Specifications**: UserByGenderSpecification, UserByAgeRangeSpecification, UserByNationalitySpecification

### 2. Application Layer (`TalkFlow.Application`)
- **CQRS**: Commands và Queries với MediatR (đang phát triển)
- **DTOs**: UserDto, RoomDto, MessageDto, CreateUserDto, CreateRoomDto, etc.
- **Services**: IUserService, IRoomService, IMessageService với implementations
- **Mappings**: AutoMapper profiles cho User, Room, Message
- **Behaviors**: ValidationBehavior, LoggingBehavior, TransactionBehavior

### 3. Infrastructure Layer (`TalkFlow.Infrastructure`)
- **Data Access**: Entity Framework Core với SQL Server
- **Repositories**: Implementation đầy đủ của Domain repositories
- **External Services**: JwtTokenService, UserDomainService, RoomDomainService
- **Configuration**: EF Core configurations cho tất cả entities

### 4. Presentation Layer (`TalkFlow.Presentation`)
- **Web API**: RESTful API endpoints (User, Room, Message)
- **SignalR Hubs**: ChatHub, PresenceHub, StrangerHub, TestHub
- **Controllers**: UserController, RoomController, MessageController
- **Authentication**: JWT Bearer Token

### 5. Shared Layer (`TalkFlow.Shared`)
- **Constants**: ApiRoutes, SignalREvents
- **Extensions**: ServiceCollectionExtensions
- **Utilities**: DateTimeHelper, StringHelper

## Tính năng chính

### 1. Quản lý User
- Tạo user ẩn danh
- Cập nhật thông tin profile
- Khóa/mở khóa user
- Xóa user

### 2. Quản lý Room
- Tạo phòng chat với mã bảo mật
- Tham gia phòng bằng roomId và security code
- Cập nhật thông tin phòng
- Xóa phòng
- Chặn/bỏ chặn chat

### 3. Chat Realtime
- Gửi/nhận tin nhắn realtime qua SignalR
- Mute microphone/camera
- Chia sẻ màn hình
- WebRTC signaling cho video call

### 4. Ghép cặp ngẫu nhiên
- Tạo filter preferences (giới tính, tuổi, quốc tịch)
- Ghép cặp với người lạ phù hợp
- Tự động tạo phòng khi match

## Công nghệ sử dụng

- **.NET 8.0**
- **ASP.NET Core 8.0**
- **Entity Framework Core 8.0**
- **SignalR 8.0**
- **SQL Server**
- **JWT Authentication**
- **AutoMapper**
- **FluentValidation**
- **MediatR (CQRS)** - đang phát triển

## Cài đặt và chạy

### 1. Yêu cầu hệ thống
- .NET 8.0 SDK
- SQL Server hoặc SQL Server LocalDB
- Visual Studio 2022 hoặc VS Code

### 2. Clone repository
```bash
git clone https://github.com/cuthanhcam/TalkFlow.git
cd TalkFlow
```

### 3. Cấu hình database
Cập nhật connection string trong `src/TalkFlow.Presentation/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=TalkFlow;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

### 4. Chạy migrations
```bash
cd src/TalkFlow.Presentation
dotnet ef database update
```

### 5. Chạy ứng dụng
```bash
cd src/TalkFlow.Presentation
dotnet run
```

Ứng dụng sẽ chạy tại `https://localhost:5001`

## API Endpoints

### User Management
- `POST /api/user` - Tạo user mới
- `GET /api/user/{userId}` - Lấy thông tin user
- `GET /api/user` - Lấy danh sách users (có pagination)
- `PUT /api/user/{userId}` - Cập nhật user
- `PUT /api/user/{userId}/lock` - Khóa user
- `PUT /api/user/{userId}/unlock` - Mở khóa user
- `DELETE /api/user/{userId}` - Xóa user

### Room Management
- `POST /api/room` - Tạo phòng mới
- `POST /api/room/join` - Tham gia phòng
- `GET /api/room/{roomId}` - Lấy thông tin phòng
- `GET /api/room` - Lấy danh sách phòng (có pagination)
- `PUT /api/room/{roomId}` - Cập nhật phòng
- `DELETE /api/room/{roomId}` - Xóa phòng
- `PUT /api/room/{roomId}/block-chat` - Chặn chat
- `PUT /api/room/{roomId}/unblock-chat` - Bỏ chặn chat

## SignalR Hubs

### ChatHub (`/hubs/chathub`)
- `JoinRoom(roomId)` - Tham gia phòng
- `LeaveRoom(roomId)` - Rời phòng
- `SendMessage(roomId, message)` - Gửi tin nhắn
- `MuteMicrophone(roomId, isMuted)` - Mute mic
- `MuteCamera(roomId, isMuted)` - Mute camera
- `ShareScreen(roomId, isSharing)` - Chia sẻ màn hình
- `SendWebRTCOffer(roomId, targetUserId, offer)` - Gửi WebRTC offer
- `SendWebRTCAnswer(roomId, targetUserId, answer)` - Gửi WebRTC answer
- `SendIceCandidate(roomId, targetUserId, candidate)` - Gửi ICE candidate

### PresenceHub (`/hubs/presence`)
- Theo dõi trạng thái online/offline của users

### StrangerHub (`/hubs/stranger`)
- `StartMatching()` - Bắt đầu ghép cặp
- `StopMatching()` - Dừng ghép cặp

## Cấu trúc Database

### Tables
- `AspNetUsers` - Users (Identity)
- `AspNetRoles` - Roles (Identity)
- `AspNetUserRoles` - User-Role mapping (Identity)
- `Rooms` - Phòng chat
- `Connections` - SignalR connections
- `Messages` - Tin nhắn
- `StrangerFilters` - Filter preferences cho ghép cặp

## Authentication

Ứng dụng sử dụng JWT Bearer Token authentication:
- Tạo token khi user được tạo
- Token chứa thông tin: user_id, display_name, roles
- Tất cả API endpoints (trừ tạo user/room) đều yêu cầu authentication
- SignalR hubs yêu cầu JWT token

## WebRTC Integration

- Server chỉ làm signaling layer qua SignalR
- Client tự thiết lập P2P connection
- Hỗ trợ offer/answer/ICE candidate exchange
- Cần cấu hình STUN/TURN server cho production

## Development

### Thêm tính năng mới
1. Tạo Domain entities/aggregates trong `TalkFlow.Domain`
2. Tạo Commands/Queries trong `TalkFlow.Application`
3. Implement repositories trong `TalkFlow.Infrastructure`
4. Tạo API endpoints trong `TalkFlow.Presentation`

### Testing
- Unit tests: `TalkFlow.UnitTests`
- Integration tests: `TalkFlow.IntegrationTests`  
- Architecture tests: `TalkFlow.ArchitectureTests`

## Deployment

### Production Checklist
- [ ] Cấu hình connection string production
- [ ] Thiết lập JWT secret key mạnh
- [ ] Cấu hình CORS cho domain cụ thể
- [ ] Thiết lập HTTPS certificate
- [ ] Cấu hình STUN/TURN server cho WebRTC
- [ ] Thiết lập logging và monitoring
- [ ] Cấu hình rate limiting
- [ ] Thiết lập backup database

## Contributing

1. Fork repository
2. Tạo feature branch
3. Commit changes
4. Push to branch
5. Tạo Pull Request

## License

MIT License

## Support

Nếu có vấn đề hoặc câu hỏi, vui lòng tạo issue trong repository.