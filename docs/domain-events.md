# Domain Events

Các domain events được raise từ Domain Layer nhằm phản ánh sự kiện nghiệp vụ, phục vụ logging/audit, tích hợp hoặc side-effects thông qua handlers.

## User Events

- `UserCreatedEvent(UserId, DisplayName)`
  - Khi user ẩn danh được tạo
  - **Location**: `TalkFlow.Domain/Aggregates/User/Events/`
- `UserProfileUpdatedEvent(UserId, UserProfile)`
  - Khi cập nhật thông tin hồ sơ người dùng
  - **Location**: `TalkFlow.Domain/Aggregates/User/Events/`
- `UserLockedEvent(UserId)`
  - Khi khóa người dùng
  - **Location**: `TalkFlow.Domain/Aggregates/User/Events/`

## Room Events

- `RoomCreatedEvent(RoomId, RoomName, UserId hostId)`
  - Khi phòng được tạo
  - **Location**: `TalkFlow.Domain/Aggregates/Room/Events/`
- `UserJoinedRoomEvent(RoomId, UserId)`
  - Khi user join phòng
  - **Location**: `TalkFlow.Domain/Aggregates/Room/Events/`
- `UserLeftRoomEvent(RoomId, UserId)`
  - Khi user rời phòng
  - **Location**: `TalkFlow.Domain/Aggregates/Room/Events/`
- `MessageSentEvent(RoomId, Message)`
  - Khi có tin nhắn mới trong phòng
  - **Location**: `TalkFlow.Domain/Aggregates/Room/Events/`
- `ChatBlockedEvent(RoomId)`
  - Khi phòng bị chặn chat
  - **Location**: `TalkFlow.Domain/Aggregates/Room/Events/`
- `ChatUnblockedEvent(RoomId)`
  - Khi bỏ chặn chat
  - **Location**: `TalkFlow.Domain/Aggregates/Room/Events/`
- `RoomDeactivatedEvent(RoomId)`
  - Khi phòng bị vô hiệu hóa
  - **Location**: `TalkFlow.Domain/Aggregates/Room/Events/`

## Match Events

- `MatchCreatedEvent(MatchId, User1Id, User2Id)`
  - Khi tạo match thành công
  - **Location**: `TalkFlow.Domain/Aggregates/Matching/Match/Events/`
- `MatchCompletedEvent(MatchId)`
  - Khi hoàn thành match
  - **Location**: `TalkFlow.Domain/Aggregates/Matching/Match/Events/`
- `StrangerMatchedEvent(StrangerMatchingId, UserId, MatchedUserId)`
  - Khi ghép cặp stranger thành công
  - **Location**: `TalkFlow.Domain/Aggregates/Matching/StrangerMatching/Events/`

## Current Implementation Status

### Implemented Events

- UserCreatedEvent
- UserProfileUpdatedEvent
- UserLockedEvent
- RoomCreatedEvent
- UserJoinedRoomEvent
- UserLeftRoomEvent
- MessageSentEvent
- ChatBlockedEvent
- ChatUnblockedEvent
- RoomDeactivatedEvent
- MatchCreatedEvent
- MatchCompletedEvent
- StrangerMatchedEvent

### Event Handling

- Domain Event base infrastructure có sẵn
- Event handlers trong `TalkFlow.Domain/Events/Handlers/`
- Tích hợp với MediatR đang được phát triển

## Hướng dẫn triển khai handlers

- **Current**: Sử dụng `IDomainEventHandler<T>` trong `TalkFlow.Domain/Events/Handlers/`
- **Planned**: Migrate sang `INotificationHandler<T>` (MediatR)
- **Side-effects có thể implement**:
  - Ghi audit log
  - Thông báo qua SignalR
  - Cập nhật projections/read models
  - Gửi email/notification

## Best Practices

- Event nên immutable (record)
- Đặt tên theo nghiệp vụ, không theo kỹ thuật
- Handlers nên idempotent (xử lý lặp không gây tác dụng phụ)
- Tránh logic phức tạp trong event; chỉ chứa dữ liệu cần thiết
