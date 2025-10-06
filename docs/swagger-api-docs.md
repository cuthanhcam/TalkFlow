# TalkFlow API Documentation

## Swagger/OpenAPI Documentation

TalkFlow API sử dụng Swagger/OpenAPI để cung cấp tài liệu API interactive. Bạn có thể truy cập và test tất cả API endpoints thông qua Swagger UI.

### Truy cập Swagger UI

Khi chạy ứng dụng trong môi trường Development:

**URL:** https://localhost:5001/swagger

hoặc

**URL:** http://localhost:5000/swagger

### Các API Endpoints có sẵn

#### 🏠 **Room Management APIs**

| Method | Endpoint | Mô tả | Yêu cầu Auth |
|--------|----------|-------|--------------|
| `GET` | `/api/v1/rooms` | Lấy danh sách tất cả rooms | ❌ |
| `POST` | `/api/v1/rooms` | Tạo room mới | ❌ |
| `GET` | `/api/v1/rooms/{roomId}` | Lấy thông tin room cụ thể | ❌ |
| `POST` | `/api/v1/rooms/{roomId}/join` | Tham gia room | ❌ |
| `PUT` | `/api/v1/rooms/{roomId}` | Cập nhật room (Host only) | ✅ Host |
| `DELETE` | `/api/v1/rooms/{roomId}` | Xóa room (Host only) | ✅ Host |

#### 👥 **Stranger Matching APIs**

| Method | Endpoint | Mô tả | Yêu cầu Auth |
|--------|----------|-------|--------------|
| `POST` | `/api/v1/strangers` | Tạo phiên stranger matching | ❌ |
| `POST` | `/api/v1/strangers/{roomId}/join` | Tham gia stranger room | ✅ |

#### 👤 **Member Management APIs**

| Method | Endpoint | Mô tả | Yêu cầu Auth |
|--------|----------|-------|--------------|
| `GET` | `/api/v1/members` | Lấy danh sách members (phân trang) | ✅ |
| `GET` | `/api/v1/members/{userId}` | Lấy thông tin member cụ thể | ✅ |
| `PUT` | `/api/v1/members/{userId}/lock` | Khóa/mở khóa user (Admin/Host) | ✅ Admin/Host |

### Backward Compatibility Routes

API cũng hỗ trợ các routes cũ để tương thích ngược:

- `POST /api/Room/add-room` → `POST /api/v1/rooms`
- `POST /api/Room/join-room` → `POST /api/v1/rooms/{roomId}/join`
- `POST /api/Stranger/add-stranger` → `POST /api/v1/strangers`
- `POST /api/Stranger/join-stranger` → `POST /api/v1/strangers/{roomId}/join`

### Authentication

API sử dụng JWT (JSON Web Token) để xác thực. Trong Swagger UI:

1. Click vào nút **"Authorize"** ở góc trên bên phải
2. Nhập token theo format: `Bearer your_jwt_token_here`
3. Click **"Authorize"** để áp dụng

### Request/Response Examples

#### Tạo Room mới

**Request:**
```json
POST /api/v1/rooms
{
  "roomName": "My Video Room",
  "displayName": "John Doe", 
  "securityCode": "123456"
}
```

**Response:**
```json
{
  "user": {
    "userId": "guid",
    "userName": "auto-generated",
    "displayName": "John Doe",
    "token": "jwt_token",
    "lastActive": "2025-10-05T10:30:00Z"
  },
  "room": {
    "roomId": "guid",
    "roomName": "My Video Room",
    "userId": "guid",
    "userName": "auto-generated",
    "displayName": "John Doe"
  }
}
```

### SignalR Hubs

Ngoài REST APIs, TalkFlow cũng sử dụng SignalR cho real-time communication:

- **Presence Hub:** `/hubs/presence` - Quản lý trạng thái online/offline
- **Chat Hub:** `/hubs/chathub` - Chat real-time trong room
- **Stranger Hub:** `/hubs/stranger` - Stranger matching real-time

### Error Handling

API trả về các HTTP status codes chuẩn:

- `200 OK` - Request thành công
- `201 Created` - Tạo resource thành công  
- `204 No Content` - Thành công nhưng không có nội dung trả về
- `400 Bad Request` - Dữ liệu request không hợp lệ
- `401 Unauthorized` - Không có quyền truy cập
- `404 Not Found` - Không tìm thấy resource
- `500 Internal Server Error` - Lỗi server

### Schema Definitions

Swagger UI cung cấp định nghĩa chi tiết cho tất cả DTOs và Models được sử dụng trong API, bao gồm:

- **RegisterDto** - Tạo room
- **JoinRoomDto** - Tham gia room
- **RegisterStrangerDto** - Tạo stranger session
- **RoomDto** - Thông tin room
- **UserDto** - Thông tin user
- **MemberDto** - Thông tin member

### Development Features

Khi chạy trong môi trường Development, Swagger UI có các tính năng:

- ✅ **Try it out** - Test trực tiếp API endpoints
- ✅ **Request duration** - Hiển thị thời gian response
- ✅ **Expanded documentation** - Hiển thị tất cả endpoints mở rộng
- ✅ **JWT Authentication** - Hỗ trợ Bearer token authentication

---

**Lưu ý:** Swagger UI chỉ có sẵn trong môi trường Development. Trong Production, API documentation sẽ được phục vụ thông qua các kênh khác để đảm bảo bảo mật.