# Domain Events

Các domain events được raise từ Domain Layer nhằm phản ánh sự kiện nghiệp vụ, phục vụ logging/audit, tích hợp hoặc side-effects thông qua handlers.

## User Events
- `UserCreatedEvent(UserId, DisplayName)`
  - Khi user ẩn danh được tạo
- `UserProfileUpdatedEvent(UserId, UserProfile)`
  - Khi cập nhật thông tin hồ sơ người dùng
- `UserLockedEvent(UserId)`
  - Khi khóa người dùng

## Room Events
- `RoomCreatedEvent(RoomId, RoomName, UserId hostId)`
  - Khi phòng được tạo
- `UserJoinedRoomEvent(RoomId, UserId)`
  - Khi user join phòng
- `UserLeftRoomEvent(RoomId, UserId)`
  - Khi user rời phòng
- `MessageSentEvent(RoomId, Message)`
  - Khi có tin nhắn mới trong phòng
- `ChatBlockedEvent(RoomId)`
  - Khi phòng bị chặn chat
- `ChatUnblockedEvent(RoomId)`
  - Khi bỏ chặn chat
- `RoomDeactivatedEvent(RoomId)`
  - Khi phòng bị vô hiệu hóa

## Hướng dẫn triển khai handlers (gợi ý)
- Tạo thư mục `Domain/Events/Handlers`
- Implement các `INotificationHandler<T>` (nếu đẩy qua MediatR) hoặc registry tùy ý
- Ví dụ side-effects:
  - Ghi audit log
  - Thông báo qua SignalR
  - Cập nhật projections/read models
  - Gửi email/notification

## Best Practices
- Event nên immutable (record)
- Đặt tên theo nghiệp vụ, không theo kỹ thuật
- Handlers nên idempotent (xử lý lặp không gây tác dụng phụ)
- Tránh logic phức tạp trong event; chỉ chứa dữ liệu cần thiết
