# TALKFLOW

## Ứng dụng Video Chat Realtime với WebRTC và SignalR

---

## 📋 MỤC LỤC

1. [Tổng quan hệ thống](#1-tổng-quan-hệ-thống)
2. [Công nghệ sử dụng](#2-công-nghệ-sử-dụng)
3. [Kiến trúc hệ thống](#3-kiến-trúc-hệ-thống)
4. [Use Cases](#4-use-cases)
5. [API Endpoints](#5-api-endpoints)
6. [SignalR Hubs](#6-signalr-hubs)
7. [Database Schema](#7-database-schema)
8. [Chức năng chi tiết](#8-chức-năng-chi-tiết)
9. [Luồng hoạt động](#9-luồng-hoạt-động)
10. [WebRTC Implementation](#10-webrtc-implementation)
11. [Security và Authentication](#11-security-và-authentication)
12. [Testing và Deployment](#12-testing-và-deployment)

---

## 1. TỔNG QUAN HỆ THỐNG

### 1.1. Giới thiệu

**TalkFlow** là ứng dụng video chat realtime cho phép người dùng:

- Tạo phòng chat video riêng tư với bạn bè
- Tham gia phòng bằng Room ID và mật khẩu (nếu có)
- Ghép cặp ngẫu nhiên với người lạ dựa trên sở thích
- Giao tiếp bằng video, audio và text chat realtime

### 1.2. Mục tiêu

- Cung cấp nền tảng giao tiếp video chất lượng cao
- Bảo mật thông tin người dùng
- Trải nghiệm người dùng mượt mà, giao diện hiện đại
- Hỗ trợ nhiều người dùng cùng lúc (scalable)

### 1.3. Đặc điểm nổi bật

- ✅ **Realtime Communication**: Video/audio/text chat với độ trễ thấp
- ✅ **P2P WebRTC**: Kết nối peer-to-peer giảm tải server
- ✅ **SignalR Integration**: Signaling và messaging realtime
- ✅ **Stranger Matching**: Thuật toán ghép cặp thông minh
- ✅ **Security**: Mã hóa kết nối, bảo vệ phòng bằng password
- ✅ **Modern UI**: Giao diện responsive, hiện đại với Bootstrap 5

---

## 2. CÔNG NGHỆ SỬ DỤNG

### 2.1. Backend

| Công nghệ             | Phiên bản | Mục đích               |
| --------------------- | --------- | ---------------------- |
| .NET                  | 8.0       | Framework chính        |
| ASP.NET Core MVC      | 8.0       | Web framework          |
| Entity Framework Core | 8.0       | ORM - Database access  |
| SignalR               | 8.0       | Realtime communication |
| SQL Server            | 2022      | Database               |
| Identity Framework    | 8.0       | Authentication         |

### 2.2. Frontend

| Công nghệ            | Phiên bản | Mục đích              |
| -------------------- | --------- | --------------------- |
| Razor Pages          | -         | Server-side rendering |
| JavaScript (Vanilla) | ES6+      | Client logic          |
| jQuery               | 3.7.1     | DOM manipulation      |
| Bootstrap            | 5.3       | UI framework          |
| Font Awesome         | 6.x       | Icons                 |
| WebRTC API           | -         | Video/audio P2P       |
| PeerJS               | 1.5.4     | WebRTC wrapper        |

### 2.3. Tools & Libraries

- **AutoMapper**: Object mapping
- **jQuery Validation**: Form validation
- **Choices.js**: Custom select dropdowns
- **Git**: Version control
- **Visual Studio 2022**: IDE

---

## 3. KIẾN TRÚC HỆ THỐNG

### 3.1. Kiến trúc tổng quan

```
┌──────────────────────────────────────────────────────────┐
│                    CLIENT BROWSER                        │
│  ┌─────────────┐ ┌─────────────┐ ┌─────────────────────┐ │
│  │    Razor    │ │ JavaScript  │ │     WebRTC P2P      │ │
│  │    Pages    │ │    Logic    │ │     (PeerJS)        │ │
│  └──────┬──────┘ └──────┬──────┘ └──────────┬──────────┘ │
└─────────┼────────────────┼───────────────────┼───────────┘
          │                │                   │
          │ HTTP           │ SignalR           │ STUN/TURN
          │                │ WebSocket         │
┌─────────▼────────────────▼───────────────────▼───────────┐
│                ASP.NET CORE SERVER                       │
│  ┌─────────────┐ ┌─────────────┐ ┌─────────────────────┐ │
│  │     MVC     │ │   SignalR   │ │   Authentication    │ │
│  │ Controllers │ │    Hubs     │ │    (Identity)       │ │
│  └──────┬──────┘ └──────┬──────┘ └──────────┬──────────┘ │
│         │               │                   │            │
│  ┌──────▼───────────────▼───────────────────▼──────────┐ │
│  │              Business Logic Layer                   │ │
│  │      - Services  - Repositories  - Domain           │ │
│  └─────────────────────────┬───────────────────────────┘ │
│                            │                             │
│  ┌─────────────────────────▼───────────────────────────┐ │
│  │         Entity Framework Core (ORM)                 │ │
│  └─────────────────────────┬───────────────────────────┘ │
└────────────────────────────┼─────────────────────────────┘
                             │
┌────────────────────────────▼─────────────────────────────┐
│               SQL SERVER DATABASE                        │
│  Tables: Users, Rooms, Connections,                      │
│          StrangerFilters, Messages                       │
└──────────────────────────────────────────────────────────┘
```

### 3.2. Mô hình MVC

#### Controllers

- **HomeController**: Trang chủ, tạo/join phòng
- **RoomController**: Quản lý phòng, cấu hình phòng
- **StrangerController**: Ghép cặp người lạ

#### Models/Entities

- **AppUser**: Thông tin người dùng
- **Room**: Phòng chat
- **Connection**: SignalR connections
- **StrangerFilter**: Bộ lọc ghép cặp

#### Views

- **Home/Index**: Landing page
- **Home/FriendHub**: Tạo/join phòng với bạn bè
- **Home/Privacy**: Chính sách bảo mật
- **Room/Meeting**: Phòng video chat chính
- **Stranger/Index**: Form đăng ký stranger
- **Stranger/FindOut**: Chọn preferences
- **Stranger/Success**: Xác nhận match
- **Stranger/Meeting2Peer**: Phòng stranger chat

---

## 4. USE CASES

### 4.1. Use Case Diagram

```
                    ┌─────────────┐
                    │    User     │
                    └──────┬──────┘
                           │
        ┌──────────────────┼──────────────────┐
        │                  │                  │
┌───────▼────────┐ ┌───────▼────────┐  ┌──────▼──────┐
│ Create Room    │ │  Join Room     │  │Meet Stranger│
│ with Friends   │ │  by Room ID    │  │  (Random)   │
└───────┬────────┘ └───────┬────────┘  └──────┬──────┘
        │                  │                  │
        │                  │                  │
┌───────▼──────────────────▼──────────────────▼──────┐
│              Video Chat Room                       │
│  ┌──────────┐ ┌──────────┐ ┌────────────────────┐  │
│  │ Video    │ │  Audio   │ │  Text Chat         │  │
│  │ Call     │ │  Call    │ │  (Realtime)        │  │
│  └──────────┘ └──────────┘ └────────────────────┘  │
│  ┌──────────┐ ┌──────────┐ ┌────────────────────┐  │
│  │ Share    │ │ Mute     │ │  Room Settings     │  │
│  │ Screen   │ │ Mic/Cam  │ │  (Security)        │  │
│  └──────────┘ └──────────┘ └────────────────────┘  │
└────────────────────────────────────────────────────┘
```

### 4.2. Chi tiết Use Cases

#### UC1: Tạo phòng chat với bạn bè

**Actors**: User (Host)

**Preconditions**: Không có

**Main Flow**:

1. User truy cập trang chủ
2. User nhập Display Name (6-20 ký tự)
3. User click "Create New Room"
4. System tạo Room với RoomID unique
5. System redirect user vào Room/Meeting
6. System hiển thị Welcome modal với Room URL
7. User copy URL chia sẻ cho bạn bè

**Postconditions**:

- Room được tạo trong database
- User trở thành Host của room
- Room URL có thể chia sẻ

**Alternative Flows**:

- 3a. Display Name không hợp lệ → Hiển thị validation error

---

#### UC2: Tham gia phòng bằng Room ID

**Actors**: User (Guest)

**Preconditions**: Có Room ID từ Host

**Main Flow**:

1. User click "Join Existing Room"
2. User nhập Display Name (6-20 ký tự)
3. User nhập Room ID
4. User nhập Password (nếu phòng có bảo vệ)
5. System validate thông tin
6. System cho phép user join room
7. System redirect vào Room/Meeting
8. System thông báo cho Host về guest mới

**Postconditions**:

- User tham gia room thành công
- Tất cả members nhận được notification
- Video/audio connection được thiết lập

**Alternative Flows**:

- 5a. Room ID không tồn tại → Error "Room not found"
- 5b. Password sai → Error "Invalid password"
- 5c. Room đã đầy → Error "Room is full"

---

#### UC3: Ghép cặp ngẫu nhiên (Stranger)

**Actors**: User

**Preconditions**: Không có

**Main Flow**:

1. User chọn "Meet Stranger"
2. User nhập thông tin cá nhân:
   - Display Name (6-20 ký tự)
   - Gender (Male/Female/Others)
   - Age (13-100)
   - Country
3. User chọn preferences để tìm:
   - Gender preference (Male/Female/Others/All)
   - Age range (15-18, 18-24, 24-30, 30+)
   - Countries (optional, multiple)
4. System lưu StrangerFilter
5. System chạy matching algorithm
6. System tìm user khác có filter phù hợp
7. System tạo room tự động
8. System thông báo match success cho cả 2 users
9. System redirect cả 2 vào Meeting2Peer

**Postconditions**:

- 2 users được ghép cặp
- Room được tạo tự động
- Cả 2 users vào cùng room

**Alternative Flows**:

- 6a. Không tìm thấy match → Hiển thị "Waiting" screen
- 6b. Match timeout → Cho phép retry

---

#### UC4: Video Call trong phòng

**Actors**: User (Host hoặc Guest)

**Preconditions**: User đã vào room

**Main Flow**:

1. System tự động bật camera và microphone
2. System thiết lập WebRTC connection với peers
3. User nhìn thấy video của chính mình (local)
4. User nhìn thấy video của người khác (remote)
5. User có thể:
   - Mute/unmute microphone
   - Turn on/off camera
   - Share screen
   - Send text messages
   - Leave room

**Postconditions**:

- P2P connection được thiết lập
- Video/audio streaming hoạt động
- Chat realtime hoạt động

**Alternative Flows**:

- 2a. User từ chối camera permission → Hiển thị avatar
- 2b. Connection failed → Hiển thị error, retry

---

#### UC5: Cấu hình phòng (Room Settings)

**Actors**: Host only

**Preconditions**: User là Host của room

**Main Flow**:

1. Host click nút Settings
2. System hiển thị Security Configuration modal
3. Host có thể:
   - Set/change room password
   - Toggle "Accept attendees automatically"
4. Host click "Save Changes"
5. System đóng modal
6. System hiển thị toast "Settings saved successfully"
7. System áp dụng settings mới

**Postconditions**:

- Room settings được cập nhật
- Password được lưu (nếu có)
- Guest mới phải nhập password

**Alternative Flows**:

- 4a. Host để password trống → Xóa password protection
- 5a. Save failed → Toast "Failed to save"

---

#### UC6: Gửi text message realtime

**Actors**: User (trong room)

**Preconditions**: User đã vào room

**Main Flow**:

1. User mở chat panel
2. User gõ message vào input box
3. User nhấn Enter hoặc click Send
4. System gửi message qua SignalR
5. System broadcast message đến tất cả members
6. Message hiển thị trong chat panel của mọi người
7. Message có timestamp và sender name

**Postconditions**:

- Message được hiển thị cho tất cả users
- Chat history được cập nhật

**Alternative Flows**:

- 3a. Message trống → Không gửi
- 4a. Connection lost → Queue message, gửi khi reconnect

---

## 5. API ENDPOINTS

### 5.1. Home Controller

#### POST /Home/CreateRoom

Tạo phòng chat mới

**Request Body**:

```json
{
  "CreateRoom": {
    "RoomName": "My Room",
    "DisplayName": "John Doe",
    "SecurityCode": "123456"
  }
}
```

**Response**: Redirect to `/Room/Meeting/{roomId}`

**Status Codes**:

- `302 Found`: Success, redirect
- `400 Bad Request`: Validation error

---

#### POST /Home/JoinRoom

Tham gia phòng đã có

**Request Body**:

```json
{
  "JoinRoom": {
    "RoomId": "632e5d13-0c36-44c2-3551-08de089ade39",
    "DisplayName": "Jane Smith",
    "SecurityCode": "123456"
  }
}
```

**Response**: Redirect to `/Room/Meeting/{roomId}`

**Status Codes**:

- `302 Found`: Success
- `400 Bad Request`: Invalid room ID
- `401 Unauthorized`: Wrong password
- `404 Not Found`: Room not exists

---

### 5.2. Room Controller

#### GET /Room/Meeting/{id}

Vào phòng chat

**Parameters**:

- `id` (Guid): Room ID

**Response**: View với room data

**Status Codes**:

- `200 OK`: Success
- `404 Not Found`: Room not exists
- `401 Unauthorized`: Need password

---

#### POST /Room/ChangeRoomSercurityCode

Thay đổi security code của phòng

**Request Body**:

```json
{
  "RoomId": "632e5d13-0c36-44c2-3551-08de089ade39",
  "RoomName": "My Room",
  "SecurityCode": "new_password"
}
```

**Response**:

```json
{
  "success": true,
  "message": "Security settings updated"
}
```

**Status Codes**:

- `200 OK`: Success
- `400 Bad Request`: Invalid data
- `403 Forbidden`: Not host
- `404 Not Found`: Room not exists

---

### 5.3. Stranger Controller

#### GET /Stranger/Index

Trang đăng ký stranger

**Response**: View form đăng ký

---

#### POST /Stranger/FindOut

Chọn preferences để tìm stranger

**Request Body**:

```json
{
  "DisplayName": "Anonymous",
  "Gender": "Male",
  "Age": 25,
  "Nationality": "VN"
}
```

**Response**: Redirect to `/Stranger/CallAddStranger`

---

#### POST /Stranger/CallAddStranger

Tạo stranger profile và bắt đầu matching

**Request Body**:

```json
{
  "DisplayName": "Anonymous",
  "Gender": "Male",
  "Age": 25,
  "Nationality": "VN",
  "StrangerFilter": {
    "FindGender": ["Female", "Others"],
    "MinAge": 18,
    "MaxAge": 30,
    "FindRegion": ["VN", "US"]
  }
}
```

**Response**: Redirect to `/Stranger/Waiting`

**Status Codes**:

- `302 Found`: Success
- `400 Bad Request`: Validation error

---

#### GET /Stranger/Waiting

Trang chờ matching

**Response**: View với loading animation

---

#### POST /Stranger/Matching

Xử lý khi tìm thấy match

**Request Body**:

```json
{
  "roomId": "632e5d13-0c36-44c2-3551-08de089ade39"
}
```

**Response**: Redirect to `/Stranger/Success`

---

#### GET /Stranger/Success

Trang xác nhận match

**Response**: View với thông tin match

---

#### GET /Stranger/Meeting2Peer

Phòng video chat stranger

**Response**: View room chat

---

## 6. SIGNALR HUBS

### 6.1. ChatHub (`/chathub`)

#### Methods (Client → Server)

##### 1. JoinRoom

Join vào một room

```javascript
connection.invoke("JoinRoom", roomId, userId, displayName);
```

**Parameters**:

- `roomId` (Guid): ID của room
- `userId` (Guid): ID của user
- `displayName` (string): Tên hiển thị

**Server Actions**:

- Add user vào SignalR group (roomId)
- Lưu connection vào database
- Broadcast "UserJoined" event

---

##### 2. SendMessage

Gửi text message

```javascript
connection.invoke("SendMessage", roomId, userId, displayName, message);
```

**Parameters**:

- `roomId` (Guid): ID của room
- `userId` (Guid): ID của sender
- `displayName` (string): Tên sender
- `message` (string): Nội dung tin nhắn

**Server Actions**:

- Validate message
- Broadcast "ReceiveMessage" đến tất cả members
- Lưu message vào database (optional)

---

##### 3. MuteMicroPhone

Thông báo trạng thái microphone

```javascript
connection.invoke("MuteMicroPhone", roomId, userId, isMuted);
```

**Parameters**:

- `roomId` (Guid): ID của room
- `userId` (Guid): ID của user
- `isMuted` (bool): true = muted, false = unmuted

**Server Actions**:

- Broadcast "UserMutedMicrophone" event

---

##### 4. MuteCamera

Thông báo trạng thái camera

```javascript
connection.invoke("MuteCamera", roomId, userId, isMuted);
```

**Parameters**:

- `roomId` (Guid): ID của room
- `userId` (Guid): ID của user
- `isMuted` (bool): true = off, false = on

**Server Actions**:

- Broadcast "UserMutedCamera" event

---

##### 5. ShareScreen

Thông báo trạng thái screen share

```javascript
connection.invoke("ShareScreen", roomId, userId, isSharing);
```

**Parameters**:

- `roomId` (Guid): ID của room
- `userId` (Guid): ID của user
- `isSharing` (bool): true = sharing, false = stopped

**Server Actions**:

- Broadcast "UserSharedScreen" event

---

##### 6. LeaveRoom

Rời khỏi room

```javascript
connection.invoke("LeaveRoom", roomId, userId);
```

**Parameters**:

- `roomId` (Guid): ID của room
- `userId` (Guid): ID của user

**Server Actions**:

- Remove user khỏi SignalR group
- Xóa connection khỏi database
- Broadcast "UserLeft" event
- Check nếu room trống → Delete room

---

#### Events (Server → Client)

##### 1. ReceiveMessage

Nhận tin nhắn mới

```javascript
connection.on("ReceiveMessage", (userId, displayName, message, timestamp) => {
  // Display message in chat UI
});
```

---

##### 2. UserJoined

User mới join room

```javascript
connection.on("UserJoined", (userId, displayName) => {
  // Show notification: "John joined"
  // Initialize WebRTC connection with new peer
});
```

---

##### 3. UserLeft

User rời room

```javascript
connection.on("UserLeft", (userId, displayName) => {
  // Show notification: "John left"
  // Close WebRTC connection with peer
});
```

---

##### 4. UserMutedMicrophone

User mute/unmute micro

```javascript
connection.on("UserMutedMicrophone", (userId, isMuted) => {
  // Update UI indicator
});
```

---

##### 5. UserMutedCamera

User on/off camera

```javascript
connection.on("UserMutedCamera", (userId, isMuted) => {
  // Update UI indicator
});
```

---

##### 6. UserSharedScreen

User share screen

```javascript
connection.on("UserSharedScreen", (userId, isSharing) => {
  // Update UI, handle screen stream
});
```

---

### 6.2. StrangerHub (`/strangerhub`)

#### Methods

##### 1. OnConnectedAsync (Auto)

Tự động gọi khi user connect

**Server Actions**:

- Get userId từ token
- Get user's StrangerFilter
- Chạy matching algorithm:
  ```csharp
  var matches = await StrangerFindMatch();
  foreach (var group in matches) {
      // Tạo room
      // Thông báo cho cả 2 users
      await Clients.User(userId).SendAsync("JoinStrangerRoom", new { roomId });
  }
  ```

---

#### Matching Algorithm

**Logic**:

```csharp
// Tìm các cặp users có filter phù hợp nhau
var matches = users.Where(user1 =>
    users.Any(user2 =>
        user1.StrangerFilter.FindGender.Contains(user2.Gender) &&
        user2.StrangerFilter.FindGender.Contains(user1.Gender) &&
        user1.StrangerFilter.MinAge <= user2.Age &&
        user1.StrangerFilter.MaxAge >= user2.Age &&
        user2.StrangerFilter.MinAge <= user1.Age &&
        user2.StrangerFilter.MaxAge >= user1.Age &&
        (user1.StrangerFilter.FindRegion.Count == 0 ||
         user1.StrangerFilter.FindRegion.Contains(user2.Nationality)) &&
        (user2.StrangerFilter.FindRegion.Count == 0 ||
         user2.StrangerFilter.FindRegion.Contains(user1.Nationality))
    )
);
```

**Matching Criteria**:

1. ✅ Gender matching (2 chiều)
2. ✅ Age trong khoảng cho phép (2 chiều)
3. ✅ Country matching (nếu có specify)
4. ✅ Cả 2 users chưa có CurrentRoom

---

### 6.3. PresenceHub (`/presencehub`)

#### Methods

##### 1. OnConnectedAsync

User online

**Server Actions**:

- Track user online status
- Broadcast user online đến friends

##### 2. OnDisconnectedAsync

User offline

**Server Actions**:

- Track user offline status
- Broadcast user offline đến friends

---

## 7. DATABASE SCHEMA

### 7.1. Entity Relationship Diagram

```
┌────────────────────────────────────┐
│          AspNetUsers               │
├────────────────────────────────────┤
│ Id (PK, Guid)                      │
│ UserName (unique)                  │
│ DisplayName                        │
│ Gender (nullable)                  │
│ Age (nullable)                     │
│ Nationality (nullable)             │
│ PhotoUrl (nullable)                │
│ LastActive (DateTime)              │
│ Locked (bool)                      │
│ StrangerFilterFilterID (FK)       │
└──────────┬─────────────────────────┘
           │ 1:1
           │
           ▼
┌──────────────────────────────────┐
│      StrangerFilters             │
├──────────────────────────────────┤
│ FilterID (PK, Guid)              │
│ _FindGender (string)             │ → Split to List
│ MinAge (int)                     │
│ MaxAge (int)                     │
│ _FindRegion (string)             │ → Split to List
│ CurrentRoomRoomId (FK)           │
└────────┬─────────────────────────┘
         │ N:1
         │
         ▼
┌────────────────────────────────────┐
│             Rooms                  │
├────────────────────────────────────┤
│ RoomId (PK, Guid)                  │
│ RoomName (nullable)                │
│ SecurityCode (nullable)            │
│ CountMember (int)                  │
│ UserId (FK)                        │
│ CreatedDate (DateTime)             │
│ BlockedChat (bool)                 │
└──────────┬─────────────────────────┘
           │ 1:N
           │
           ▼
┌──────────────────────────────────┐
│        Connections               │
├──────────────────────────────────┤
│ ConnectionId (PK, string)        │
│ UserID (Guid)                    │
│ RoomId (FK, Guid)                │
└──────────────────────────────────┘
```

### 7.2. Bảng chi tiết

#### AspNetUsers

Lưu thông tin người dùng (ASP.NET Identity)

| Column                 | Type          | Constraints          | Description             |
| ---------------------- | ------------- | -------------------- | ----------------------- |
| Id                     | Guid          | PK, NOT NULL         | User ID unique          |
| UserName               | nvarchar(256) | Unique               | Username để login       |
| DisplayName            | nvarchar(MAX) | NOT NULL             | Tên hiển thị            |
| Gender                 | nvarchar(MAX) | NULL                 | Male/Female/Others      |
| Age                    | int           | NULL                 | Tuổi (13-100)           |
| Nationality            | nvarchar(MAX) | NULL                 | Mã quốc gia (VN, US...) |
| PhotoUrl               | nvarchar(MAX) | NULL                 | Avatar URL              |
| LastActive             | datetime2     | NOT NULL             | Lần active cuối         |
| Locked                 | bit           | NOT NULL, Default(0) | Bị khóa hay không       |
| StrangerFilterFilterID | Guid          | FK, NULL             | Link to StrangerFilters |

---

#### Rooms

Lưu thông tin phòng chat

| Column       | Type          | Constraints          | Description           |
| ------------ | ------------- | -------------------- | --------------------- |
| RoomId       | Guid          | PK, NOT NULL         | Room ID unique        |
| RoomName     | nvarchar(MAX) | NULL                 | Tên phòng             |
| SecurityCode | nvarchar(MAX) | NULL                 | Password bảo vệ phòng |
| CountMember  | int           | NOT NULL             | Số lượng members      |
| UserId       | Guid          | FK, NOT NULL         | ID của Host           |
| CreatedDate  | datetime2     | NOT NULL             | Ngày tạo              |
| BlockedChat  | bit           | NOT NULL, Default(0) | Chat bị chặn          |

**Relationships**:

- N:1 với AspNetUsers (UserId → Users.Id)
- 1:N với Connections

---

#### StrangerFilters

Lưu preferences để ghép cặp

| Column            | Type          | Constraints            | Description               |
| ----------------- | ------------- | ---------------------- | ------------------------- |
| FilterID          | Guid          | PK, NOT NULL           | Filter ID unique          |
| \_FindGender      | nvarchar(MAX) | NOT NULL               | CSV: "Male,Female,Others" |
| MinAge            | int           | NOT NULL, Default(0)   | Tuổi tối thiểu            |
| MaxAge            | int           | NOT NULL, Default(100) | Tuổi tối đa               |
| \_FindRegion      | nvarchar(MAX) | NOT NULL               | CSV: "VN,US,JP"           |
| CurrentRoomRoomId | Guid          | FK, NULL               | Room đang ở               |

**Relationships**:

- 1:1 với AspNetUsers (FilterID ← Users.StrangerFilterFilterID)
- N:1 với Rooms (CurrentRoomRoomId → Rooms.RoomId)

**Note**: `_FindGender` và `_FindRegion` lưu dạng CSV, được parse thành List trong code:

```csharp
[NotMapped]
public ICollection<string> FindGender {
    get => _FindGender.Split(',');
    set => _FindGender = string.Join(',', value);
}
```

---

#### Connections

Lưu SignalR connections

| Column       | Type          | Constraints  | Description           |
| ------------ | ------------- | ------------ | --------------------- |
| ConnectionId | nvarchar(450) | PK, NOT NULL | SignalR connection ID |
| UserID       | Guid          | NOT NULL     | User ID               |
| RoomId       | Guid          | FK, NULL     | Room ID (nếu có)      |

**Relationships**:

- N:1 với Rooms

**Purpose**: Track các SignalR connections để:

- Gửi message đến đúng users
- Cleanup khi disconnect
- Biết user nào đang ở room nào

---

### 7.3. Sample Data

#### Users

```sql
INSERT INTO AspNetUsers VALUES
('1a2b3c...', 'user1', 'John Doe', 'Male', 25, 'US', NULL, '2025-01-10', 0, NULL),
('4d5e6f...', 'user2', 'Jane Smith', 'Female', 22, 'VN', NULL, '2025-01-10', 0, 'abc123...');
```

#### Rooms

```sql
INSERT INTO Rooms VALUES
('room123...', 'My Room', '123456', 2, '1a2b3c...', '2025-01-10', 0);
```

#### StrangerFilters

```sql
INSERT INTO StrangerFilters VALUES
('abc123...', 'Female,Others', 18, 30, 'VN,US', NULL);
```

#### Connections

```sql
INSERT INTO Connections VALUES
('conn_xyz...', '1a2b3c...', 'room123...');
```

---

## 8. CHỨC NĂNG CHI TIẾT

### 8.1. Friend Room Mode

#### 8.1.1. Tạo phòng

**URL**: `/Home/FriendHub`

**UI Elements**:

- Input: Display Name (6-20 ký tự, required)
- Button: "Create New Room" (primary)
- Button: "Join Existing Room" (outline)
- Validation: Real-time với form-validation.js

**Workflow**:

```
User nhập tên → Click Create
  ↓
Validate frontend (minlength=6)
  ↓
POST /Home/CreateRoom
  ↓
Backend validate [StringLength(20, MinimumLength = 6)]
  ↓
Tạo Room entity:
  - RoomId = Guid.NewGuid()
  - UserId = current user
  - RoomName = DisplayName
  - SecurityCode = null (optional)
  - CreatedDate = Now
  - CountMember = 1
  ↓
Lưu vào database
  ↓
Redirect → /Room/Meeting/{roomId}
```

**Frontend Code**:

```html
<input
  id="input_display_name"
  required
  minlength="6"
  placeholder="Minimum 6 characters"
  class="form-control form-control-modern"
/>
<div class="invalid-feedback"></div>

<button
  class="btn btn-modern-primary"
  onclick="homeFormSubmit('homepage_form')"
>
  <i class="fas fa-plus-circle me-2"></i>Create New Room
</button>
```

---

#### 8.1.2. Join phòng

**URL**: Modal popup từ `/Home/FriendHub`

**UI Elements**:

- Input: Display Name (6-20 ký tự)
- Input: Room ID (Guid format)
- Input: Password (optional)
- Button: "Join Room"

**Workflow**:

```
User nhập thông tin → Click Join
  ↓
POST /Home/JoinRoom
  ↓
Validate:
  - Room ID exists?
  - Password correct (if required)?
  - Room not full?
  ↓
Success:
  - Tăng CountMember
  - Tạo Connection record
  - Redirect → /Room/Meeting/{roomId}
  ↓
Fail:
  - 404: Room not found
  - 401: Invalid password
  - 400: Room full
```

**Frontend Code**:

```html
<form id="form_join_room" data-validate="true">
  <input name="JoinRoom.DisplayName" required minlength="6" />
  <input name="JoinRoom.RoomId" required />
  <input name="JoinRoom.SecurityCode" type="password" />
  <button onclick="homeFormSubmit('form_join_room')">Join Room</button>
</form>
```

---

#### 8.1.3. Video Chat Room

**URL**: `/Room/Meeting/{roomId}`

**UI Layout**:

```
┌─────────────────────────────────────────────────────┐
│  Header: Time | Room Code | Controls                │
├─────────────────────────────────────────────────────┤
│                                                      │
│            Video Grid (Main Area)                   │
│   ┌──────────┐  ┌──────────┐  ┌──────────┐        │
│   │  Local   │  │ Remote 1 │  │ Remote 2 │        │
│   │  Video   │  │  Video   │  │  Video   │        │
│   └──────────┘  └──────────┘  └──────────┘        │
│                                                      │
├─────────────────────────────────────────────────────┤
│  Footer: [Mic] [Cam] [Share] [Chat] [People] [Leave]│
└─────────────────────────────────────────────────────┘

│                                    │
│  Chat Panel (Right)                │
│  or Participants List              │
│                                    │
└────────────────────────────────────┘
```

**Controls**:

| Button       | Icon                           | Function                | States                 |
| ------------ | ------------------------------ | ----------------------- | ---------------------- |
| Microphone   | mic/mic_off                    | Mute/unmute             | On (white) / Off (red) |
| Camera       | videocam/videocam_off          | On/off video            | On (white) / Off (red) |
| Screen Share | screen_share/stop_screen_share | Share screen            | Inactive / Active      |
| Chat         | forum                          | Open chat panel         | -                      |
| Participants | people_alt                     | Show members list       | -                      |
| Settings     | settings                       | Room config (Host only) | -                      |
| Leave        | call_end                       | Exit room               | Red                    |

**Features**:

1. **Video Grid**:

   - Responsive layout (1-4 videos)
   - Auto resize dựa trên số lượng
   - Local video có mirror effect
   - Remote videos có display name overlay

2. **Chat Panel**:

   - Real-time messaging via SignalR
   - Timestamp cho mỗi message
   - Scroll to bottom auto
   - Message có sender name và time

3. **Participants Panel**:

   - List tất cả members
   - Host có icon crown
   - Trạng thái mic/camera của từng người

4. **Welcome Modal** (Host only):

   - Hiển thị khi vừa tạo room
   - Room URL với copy button
   - Copy → Toast "Link copied!"

5. **Security Config Modal** (Host only):
   - Set/change password
   - Password visibility toggle (eye icon)
   - Toggle "Accept attendees automatically"
   - Save → Close modal + Toast success

---

#### 8.1.4. WebRTC Connection Flow

```
User A joins room
  ↓
SignalR: JoinRoom(roomId, userA)
  ↓
Server broadcasts: UserJoined(userA)
  ↓
User B (already in room) receives UserJoined
  ↓
User B creates WebRTC offer:
  - peerConnection = new RTCPeerConnection(config)
  - addStream(localStream)
  - createOffer()
  ↓
User B sends offer via SignalR
  ↓
User A receives offer
  ↓
User A creates answer:
  - setRemoteDescription(offer)
  - createAnswer()
  ↓
User A sends answer via SignalR
  ↓
User B receives answer
  ↓
Both exchange ICE candidates via SignalR
  ↓
P2P connection established
  ↓
Video/audio streaming directly (no server relay)
```

**JavaScript Code Flow**:

```javascript
// 1. Initialize WebRTC
const peerConnection = new RTCPeerConnection({
  iceServers: [{ urls: "stun:stun.l.google.com:19302" }],
});

// 2. Add local stream
navigator.mediaDevices
  .getUserMedia({ video: true, audio: true })
  .then((stream) => {
    localVideo.srcObject = stream;
    stream.getTracks().forEach((track) => {
      peerConnection.addTrack(track, stream);
    });
  });

// 3. Handle remote stream
peerConnection.ontrack = (event) => {
  remoteVideo.srcObject = event.streams[0];
};

// 4. Handle ICE candidates
peerConnection.onicecandidate = (event) => {
  if (event.candidate) {
    signalR.invoke("SendIceCandidate", roomId, event.candidate);
  }
};

// 5. Create offer
peerConnection
  .createOffer()
  .then((offer) => peerConnection.setLocalDescription(offer))
  .then(() =>
    signalR.invoke("SendOffer", roomId, peerConnection.localDescription)
  );
```

---

### 8.2. Stranger Mode

#### 8.2.1. Registration Flow

**URLs**: `/Stranger/Index` → `/Stranger/FindOut` → `/Stranger/CallAddStranger`

**Step 1: Personal Info** (`/Stranger/Index`)

- Display Name (6-50 ký tự)
- Gender (Male/Female/Others)
- Age (13-100)
- Country (dropdown với ~200 quốc gia)

**Step 2: Preferences** (`/Stranger/FindOut`)

- I want to find: Gender (Male/Female/Others/All)
- Age range:
  - 15-18
  - 18-24
  - 24-30
  - 30+
- Countries: Multiple select (optional)

**Step 3: Start Matching** (`/Stranger/CallAddStranger`)

- Lưu user info vào database
- Tạo StrangerFilter entity
- Redirect → `/Stranger/Waiting`

---

#### 8.2.2. Matching Algorithm

**Input**: User với StrangerFilter

**Process**:

```csharp
// 1. Lấy tất cả users đang chờ (không có CurrentRoom)
var waitingUsers = await context.Users
    .Include(u => u.StrangerFilter)
    .Where(u => !u.Locked && u.StrangerFilter != null && u.StrangerFilter.CurrentRoom == null)
    .ToListAsync();

// 2. Tìm các cặp match (bidirectional)
var matches = new List<List<AppUser>>();

foreach (var user1 in waitingUsers) {
    foreach (var user2 in waitingUsers) {
        if (user1.Id == user2.Id) continue;

        // Check if they match each other
        bool match =
            // User1 accepts User2's gender
            user1.StrangerFilter.FindGender.Contains(user2.Gender ?? "") &&
            // User2 accepts User1's gender
            user2.StrangerFilter.FindGender.Contains(user1.Gender ?? "") &&
            // User1's age in User2's range
            user2.StrangerFilter.MinAge <= user1.Age &&
            user2.StrangerFilter.MaxAge >= user1.Age &&
            // User2's age in User1's range
            user1.StrangerFilter.MinAge <= user2.Age &&
            user1.StrangerFilter.MaxAge >= user2.Age &&
            // Country matching (if specified)
            (user1.StrangerFilter.FindRegion.Count == 0 ||
             user1.StrangerFilter.FindRegion.Contains(user2.Nationality ?? "")) &&
            (user2.StrangerFilter.FindRegion.Count == 0 ||
             user2.StrangerFilter.FindRegion.Contains(user1.Nationality ?? ""));

        if (match) {
            matches.Add(new List<AppUser> { user1, user2 });
            break; // Found match for user1, move to next
        }
    }
}

// 3. Create rooms và notify users
foreach (var pair in matches) {
    var room = new Room {
        RoomId = Guid.NewGuid(),
        RoomName = "Stranger Chat",
        UserId = pair[0].Id,
        CreatedDate = DateTime.Now
    };
    await context.Rooms.AddAsync(room);

    // Update filters
    pair[0].StrangerFilter.CurrentRoom = room;
    pair[1].StrangerFilter.CurrentRoom = room;

    await context.SaveChangesAsync();

    // Notify both users via SignalR
    await Clients.User(pair[0].Id.ToString())
        .SendAsync("JoinStrangerRoom", new { roomId = room.RoomId });
    await Clients.User(pair[1].Id.ToString())
        .SendAsync("JoinStrangerRoom", new { roomId = room.RoomId });
}
```

**Output**:

- Cặp users được match
- Room được tạo tự động
- Cả 2 users nhận notification qua SignalR

---

#### 8.2.3. Waiting Screen

**URL**: `/Stranger/Waiting`

**UI**:

- Loading spinner animation
- Text: "Finding a perfect match for you..."
- Cancel button (optional)

**Logic**:

```javascript
// Connect to StrangerHub
const connection = new signalR.HubConnectionBuilder()
  .withUrl("/strangerhub")
  .build();

// Listen for match found
connection.on("JoinStrangerRoom", (data) => {
  // Redirect to success page
  window.location.href = `/Stranger/Matching?roomId=${data.roomId}`;
});

connection
  .start()
  .then(() => console.log("Connected to StrangerHub"))
  .catch((err) => console.error(err));
```

**Auto Matching**:

- Server tự động chạy matching khi user connect to StrangerHub
- Check matching mỗi khi có user mới join
- Timeout sau 60s nếu không tìm thấy → Retry hoặc cancel

---

#### 8.2.4. Match Success

**URL**: `/Stranger/Success`

**UI**:

- Animated eyes looking around
- Text: "We found someone for you!"
- User info (nếu có)
- Button "Accept" (primary)
- Button "Decline" (danger)

**Actions**:

- **Accept**: POST `/Stranger/Matching` → Redirect `/Stranger/Meeting2Peer`
- **Decline**: Xóa match, quay lại `/Stranger/Waiting`

---

#### 8.2.5. Stranger Chat Room

**URL**: `/Stranger/Meeting2Peer`

**Giống Friend Room nhưng khác:**

- Không có Settings button (không phải host)
- Không hiển thị Room URL
- Có "Next" button để skip và tìm người khác
- Timeout sau 10 phút → Auto leave

---

### 8.3. Core Features

#### 8.3.1. Video Controls

##### Mute/Unmute Microphone

**Frontend**:

```javascript
function changeMicState() {
  isMuted = !isMuted;

  // Update UI
  if (isMuted) {
    btnMic.classList.add("btn-danger");
    iconMic.innerHTML = "mic_off";
  } else {
    btnMic.classList.remove("btn-danger");
    iconMic.innerHTML = "mic";
  }

  // Disable audio track
  if (stream) {
    stream.getAudioTracks()[0].enabled = !isMuted;
  }

  // Notify others via SignalR
  connection.invoke("MuteMicroPhone", roomId, userId, isMuted);
}
```

**Backend** (SignalR):

```csharp
public async Task MuteMicroPhone(Guid roomId, Guid userId, bool isMuted)
{
    await Clients.OthersInGroup(roomId.ToString())
        .SendAsync("UserMutedMicrophone", userId, isMuted);
}
```

---

##### Turn On/Off Camera

**Frontend**:

```javascript
function changeCamState() {
  isStreamCam = !isStreamCam;

  // Update UI
  if (isStreamCam) {
    btnCam.classList.remove("btn-danger");
    iconCam.innerHTML = "videocam";
    divUserCard.style.display = "none"; // Hide avatar
    userVideo.style.display = "block"; // Show video
  } else {
    btnCam.classList.add("btn-danger");
    iconCam.innerHTML = "videocam_off";
    divUserCard.style.display = "flex"; // Show avatar
    userVideo.style.display = "none"; // Hide video
  }

  // Actually disable video track
  if (stream && stream.getVideoTracks().length > 0) {
    stream.getVideoTracks()[0].enabled = isStreamCam;
  }

  // Notify others
  connection.invoke("MuteCamera", roomId, userId, !isStreamCam);
}
```

**Effect**: Camera light thực sự tắt (không chỉ ẩn video)

---

##### Screen Share

**Frontend**:

```javascript
async function shareScreen() {
  try {
    const screenStream = await navigator.mediaDevices.getDisplayMedia({
      video: true,
    });

    // Replace video track in peer connection
    const videoTrack = screenStream.getVideoTracks()[0];
    const sender = peerConnection
      .getSenders()
      .find((s) => s.track.kind === "video");
    sender.replaceTrack(videoTrack);

    // Update UI
    btnShare.classList.add("active");
    iconShare.innerHTML = "stop_screen_share";

    // Notify others
    connection.invoke("ShareScreen", roomId, userId, true);

    // Handle stream ended
    videoTrack.onended = () => {
      stopScreenShare();
    };
  } catch (err) {
    console.error("Screen share error:", err);
  }
}
```

---

#### 8.3.2. Chat System

**Frontend**:

```javascript
// Send message
function sendMessage() {
  const input = document.getElementById("input_message");
  const message = input.value.trim();

  if (message) {
    connection.invoke("SendMessage", roomId, userId, displayName, message);
    input.value = "";
  }
}

// Receive message
connection.on("ReceiveMessage", (senderId, senderName, message, timestamp) => {
  const chatDisplay = document.getElementById("chat_display");

  const msgDiv = document.createElement("div");
  msgDiv.className = senderId === userId ? "message-self" : "message-other";
  msgDiv.innerHTML = `
        <div class="message-header">${senderName}</div>
        <div class="message-content">${escapeHtml(message)}</div>
        <div class="message-time">${formatTime(timestamp)}</div>
    `;

  chatDisplay.appendChild(msgDiv);
  chatDisplay.scrollTop = chatDisplay.scrollHeight;
});
```

**Backend**:

```csharp
public async Task SendMessage(Guid roomId, Guid userId, string displayName, string message)
{
    // Validate
    if (string.IsNullOrWhiteSpace(message)) return;

    // Check if chat is blocked
    var room = await _context.Rooms.FindAsync(roomId);
    if (room?.BlockedChat == true) {
        await Clients.Caller.SendAsync("Error", "Chat is blocked in this room");
        return;
    }

    // Broadcast to all members
    await Clients.Group(roomId.ToString()).SendAsync(
        "ReceiveMessage",
        userId,
        displayName,
        message,
        DateTime.Now
    );
}
```

---

#### 8.3.3. Room Settings (Host Only)

**Security Configuration**:

```javascript
function changeRoomSecurityCode() {
  const input = document.getElementById("input_pass_config");
  const password = input.value;

  const postData = {
    RoomId: roomId,
    RoomName: roomName,
    SecurityCode: password,
  };

  // Close modal immediately
  const modal = bootstrap.Modal.getInstance(
    document.getElementById("ModalSecurityConfig")
  );
  modal.hide();

  // Save to server
  fetch("/Room/ChangeRoomSercurityCode", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(postData),
  })
    .then((response) => {
      if (!response.ok) throw new Error("Save failed");
      return response.json();
    })
    .then((data) => {
      CallToast("Security settings saved successfully!", "success");
    })
    .catch((error) => {
      CallToast("Failed to save settings. Please try again.", "error");
    });
}
```

**Backend**:

```csharp
[HttpPost]
public async Task<IActionResult> ChangeRoomSercurityCode([FromBody] EditRoomModel model)
{
    var room = await _context.Rooms.FindAsync(model.RoomId);
    if (room == null) return NotFound();

    // Verify user is host
    var userId = User.GetUserId();
    if (room.UserId != userId) return Forbid();

    // Update security code
    room.SecurityCode = string.IsNullOrWhiteSpace(model.SecurityCode)
        ? null
        : model.SecurityCode;

    await _context.SaveChangesAsync();

    return Ok(new { success = true });
}
```

---

#### 8.3.4. Leave Room

**Frontend**:

```javascript
function leaveRoom() {
  // Show confirmation modal
  $("#exampleModal").modal("show");
}

// On confirm
function confirmLeave() {
  // Close peer connections
  if (peerConnection) {
    peerConnection.close();
  }

  // Stop local stream
  if (stream) {
    stream.getTracks().forEach((track) => track.stop());
  }

  // Notify server
  connection.invoke("LeaveRoom", roomId, userId);

  // Disconnect SignalR
  connection.stop();

  // Redirect to home
  window.location.href = "/Home/Index";
}
```

**Backend**:

```csharp
public async Task LeaveRoom(Guid roomId, Guid userId)
{
    // Get connections
    var connections = await _context.Connections
        .Where(c => c.UserID == userId)
        .ToListAsync();

    // Remove from group
    await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId.ToString());

    // Delete connections
    _context.Connections.RemoveRange(connections);

    // Update room count
    var room = await _context.Rooms
        .Include(r => r.Connections)
        .FirstOrDefaultAsync(r => r.RoomId == roomId);

    if (room != null) {
        room.CountMember = room.Connections.Count;

        // Delete room if empty
        if (room.CountMember == 0) {
            _context.Rooms.Remove(room);
        }
    }

    await _context.SaveChangesAsync();

    // Notify others
    await Clients.OthersInGroup(roomId.ToString())
        .SendAsync("UserLeft", userId);
}
```

---

## 9. LUỒNG HOẠT ĐỘNG

### 9.1. Friend Room - Complete Flow

```
┌─────────────────────────────────────────────────────────────┐
│ 1. USER CREATES ROOM                                        │
└─────────────────────────────────────────────────────────────┘
   User visits /Home/FriendHub
        ↓
   Enter Display Name: "John Doe"
        ↓
   Click "Create New Room"
        ↓
   Frontend validation (minlength=6)
        ↓
   POST /Home/CreateRoom
        ↓
   Backend validation [StringLength(20, MinimumLength=6)]
        ↓
   Create Room entity:
   - RoomId = Guid.NewGuid()
   - RoomName = "John Doe"
   - UserId = current user
   - SecurityCode = null
   - CreatedDate = Now
   - CountMember = 1
        ↓
   Save to database
        ↓
   Redirect → /Room/Meeting/{roomId}

┌─────────────────────────────────────────────────────────────┐
│ 2. JOHN ENTERS ROOM                                         │
└─────────────────────────────────────────────────────────────┘
   Load /Room/Meeting/{roomId}
        ↓
   Show Welcome Modal (Host only):
   - Room URL: https://localhost:7198/Room/Meeting/{roomId}
   - Copy button
        ↓
   Initialize media devices:
   - Request camera permission
   - Request microphone permission
        ↓
   Get local stream:
   navigator.mediaDevices.getUserMedia({ video: true, audio: true })
        ↓
   Display local video (mirrored)
        ↓
   Connect to SignalR ChatHub:
   const connection = new signalR.HubConnectionBuilder()
       .withUrl("/chathub")
       .build();
        ↓
   connection.start()
        ↓
   Invoke: JoinRoom(roomId, userId, displayName)
        ↓
   Server:
   - Add to SignalR group (roomId)
   - Create Connection record in database
        ↓
   John is now in room, waiting for others

┌─────────────────────────────────────────────────────────────┐
│ 3. JANE JOINS ROOM                                          │
└─────────────────────────────────────────────────────────────┘
   John copies room URL and shares to Jane
        ↓
   Jane clicks URL or manually joins:
   - Click "Join Existing Room" on /Home/FriendHub
   - Enter Display Name: "Jane Smith"
   - Enter Room ID: {roomId}
   - Enter Password: (if required)
        ↓
   POST /Home/JoinRoom
        ↓
   Backend validates:
   - Room exists? ✓
   - Password correct? ✓
   - Room not full? ✓
        ↓
   Update room:
   - CountMember += 1
        ↓
   Redirect → /Room/Meeting/{roomId}
        ↓
   Jane gets local stream
        ↓
   Jane connects to SignalR ChatHub
        ↓
   Jane invokes: JoinRoom(roomId, janeUserId, "Jane Smith")
        ↓
   Server broadcasts to group:
   UserJoined(janeUserId, "Jane Smith")

┌─────────────────────────────────────────────────────────────┐
│ 4. WEBRTC CONNECTION ESTABLISHMENT                          │
└─────────────────────────────────────────────────────────────┘
   John receives "UserJoined" event (Jane joined)
        ↓
   John creates PeerConnection for Jane:
   const pc = new RTCPeerConnection(config);
        ↓
   John adds local stream to PeerConnection:
   localStream.getTracks().forEach(track => pc.addTrack(track, localStream));
        ↓
   John creates Offer:
   const offer = await pc.createOffer();
   await pc.setLocalDescription(offer);
        ↓
   John sends Offer to Jane via SignalR:
   connection.invoke("SendWebRTCOffer", roomId, janeUserId, offer);
        ↓
   Jane receives Offer
        ↓
   Jane creates PeerConnection
        ↓
   Jane adds local stream
        ↓
   Jane sets remote description:
   await pc.setRemoteDescription(offer);
        ↓
   Jane creates Answer:
   const answer = await pc.createAnswer();
   await pc.setLocalDescription(answer);
        ↓
   Jane sends Answer to John via SignalR:
   connection.invoke("SendWebRTCAnswer", roomId, johnUserId, answer);
        ↓
   John receives Answer
        ↓
   John sets remote description:
   await pc.setRemoteDescription(answer);
        ↓
   Both sides exchange ICE candidates:
   pc.onicecandidate = (event) => {
       if (event.candidate) {
           connection.invoke("SendIceCandidate", roomId, remoteUserId, event.candidate);
       }
   };
        ↓
   P2P connection established!
        ↓
   John sees Jane's video (remote)
   Jane sees John's video (remote)

┌─────────────────────────────────────────────────────────────┐
│ 5. COMMUNICATION                                            │
└─────────────────────────────────────────────────────────────┘
   Video/Audio: Streaming P2P (no server relay)
        ↓
   Text Chat: Via SignalR
   John types message → Enter
        ↓
   connection.invoke("SendMessage", roomId, johnUserId, "John", "Hello Jane!");
        ↓
   Server broadcasts to group:
   ReceiveMessage(johnUserId, "John", "Hello Jane!", timestamp)
        ↓
   Jane's client receives message
        ↓
   Jane sees: "John: Hello Jane! (10:30 AM)"

┌─────────────────────────────────────────────────────────────┐
│ 6. CONTROLS                                                 │
└─────────────────────────────────────────────────────────────┘
   John mutes microphone:
   Click mic button
        ↓
   stream.getAudioTracks()[0].enabled = false;
        ↓
   connection.invoke("MuteMicroPhone", roomId, johnUserId, true);
        ↓
   Server → Clients: UserMutedMicrophone(johnUserId, true)
        ↓
   Jane sees mic icon change to "mic_off" for John

┌─────────────────────────────────────────────────────────────┐
│ 7. LEAVE ROOM                                               │
└─────────────────────────────────────────────────────────────┘
   Jane clicks Leave button
        ↓
   Show confirmation modal
        ↓
   Jane confirms "Just leave"
        ↓
   Stop local stream:
   stream.getTracks().forEach(track => track.stop());
        ↓
   Close peer connection:
   peerConnection.close();
        ↓
   Invoke: LeaveRoom(roomId, janeUserId)
        ↓
   Server:
   - Remove from SignalR group
   - Delete Connection record
   - CountMember -= 1
   - If CountMember == 0 → Delete Room
        ↓
   Server broadcasts:
   UserLeft(janeUserId, "Jane Smith")
        ↓
   John receives UserLeft event
        ↓
   John's client:
   - Remove Jane's video element
   - Close PeerConnection with Jane
   - Show notification: "Jane left the room"
        ↓
   Jane disconnects SignalR
        ↓
   Redirect Jane → /Home/Index
```

---

### 9.2. Stranger Mode - Complete Flow

```
┌─────────────────────────────────────────────────────────────┐
│ 1. REGISTRATION                                             │
└─────────────────────────────────────────────────────────────┘
   User visits /Stranger/Index
        ↓
   Enter personal info:
   - Display Name: "Anonymous"
   - Gender: "Male"
   - Age: 25
   - Country: "Vietnam"
        ↓
   Click "Next"
        ↓
   POST /Stranger/FindOut
        ↓
   Redirect → /Stranger/FindOut (preferences page)
        ↓
   Enter preferences:
   - I want to find: ["Female", "Others"]
   - Age range: "18-24"
   - Countries: ["VN", "US"] (optional)
        ↓
   Click "Start Matching"
        ↓
   POST /Stranger/CallAddStranger
        ↓
   Backend creates:
   - User entity (if new)
   - StrangerFilter entity:
     {
       FindGender: "Female,Others",
       MinAge: 18,
       MaxAge: 24,
       FindRegion: "VN,US",
       CurrentRoom: null
     }
        ↓
   Save to database
        ↓
   Redirect → /Stranger/Waiting

┌─────────────────────────────────────────────────────────────┐
│ 2. WAITING & MATCHING                                       │
└─────────────────────────────────────────────────────────────┘
   Load /Stranger/Waiting
        ↓
   Show loading animation
        ↓
   Connect to SignalR StrangerHub:
   const connection = new signalR.HubConnectionBuilder()
       .withUrl("/strangerhub")
       .build();
        ↓
   connection.start()
        ↓
   Server OnConnectedAsync() triggered:
   - Get userId from token
   - Get user's StrangerFilter from database
   - Run matching algorithm
        ↓
   Matching Algorithm executes:
   foreach (user1 in waitingUsers) {
       foreach (user2 in waitingUsers) {
           if (Match(user1, user2)) {
               // Create room
               // Link both users to room
               // Notify both users
           }
       }
   }
        ↓
   Match found!
   User1 (Male, 25, VN) ↔ User2 (Female, 22, VN)
        ↓
   Server creates Room:
   - RoomId = Guid.NewGuid()
   - RoomName = "Stranger Chat"
   - UserId = user1.Id (host)
   - CreatedDate = Now
        ↓
   Server updates both StrangerFilters:
   - user1.StrangerFilter.CurrentRoom = room
   - user2.StrangerFilter.CurrentRoom = room
        ↓
   Server notifies both users via SignalR:
   Clients.User(user1.Id).SendAsync("JoinStrangerRoom", { roomId })
   Clients.User(user2.Id).SendAsync("JoinStrangerRoom", { roomId })

┌─────────────────────────────────────────────────────────────┐
│ 3. MATCH CONFIRMATION                                       │
└─────────────────────────────────────────────────────────────┘
   Both users receive "JoinStrangerRoom" event
        ↓
   Client redirects:
   window.location.href = `/Stranger/Matching?roomId=${roomId}`;
        ↓
   POST /Stranger/Matching
   Body: { roomId }
        ↓
   Backend validates match
        ↓
   Redirect → /Stranger/Success
        ↓
   Show success page:
   - Animated eyes
   - "We found someone for you!"
   - Button "Accept" (primary)
   - Button "Decline" (danger)
        ↓
   User clicks "Accept"
        ↓
   Redirect → /Stranger/Meeting2Peer

┌─────────────────────────────────────────────────────────────┐
│ 4. STRANGER CHAT                                            │
└─────────────────────────────────────────────────────────────┘
   Same as Friend Room:
   - Load /Stranger/Meeting2Peer
   - Get local stream
   - Connect SignalR ChatHub
   - Join room
   - Establish WebRTC P2P
   - Video/audio/text chat
        ↓
   Differences:
   - No Settings button (not host)
   - No room URL shown
   - Can click "Next" to find new stranger
        ↓
   When leave:
   - Stop streams
   - Leave room
   - Clear StrangerFilter.CurrentRoom
   - Redirect → /Stranger/Index (or /Stranger/Waiting for next)
```

---

## 10. WEBRTC IMPLEMENTATION

### 10.1. Architecture

```
┌──────────────┐                              ┌──────────────┐
│   Client A   │                              │   Client B   │
│              │                              │              │
│  ┌────────┐  │                              │  ┌────────┐  │
│  │WebRTC  │  │                              │  │WebRTC  │  │
│  │Peer    │  │◄────────P2P Connection──────►│  │Peer    │  │
│  │        │  │      (Video/Audio)           │  │        │  │
│  └────┬───┘  │                              │  └───┬────┘  │
│       │      │                              │      │       │
└───────┼──────┘                              └──────┼───────┘
        │                                             │
        │            Signaling (SignalR)             │
        │                  ↓                         │
        └──────────────►┌──────────┐◄───────────────┘
                        │  Server  │
                        │          │
                        │ SignalR  │
                        │  Hubs    │
                        └──────────┘
```

### 10.2. Configuration

**STUN/TURN Servers**:

```javascript
const configuration = {
  iceServers: [
    {
      urls: "stun:stun.l.google.com:19302",
    },
    {
      urls: "stun:stun1.l.google.com:19302",
    },
    // TURN server for fallback (if needed)
    // {
    //     urls: 'turn:your-turn-server.com:3478',
    //     username: 'user',
    //     credential: 'pass'
    // }
  ],
};

const peerConnection = new RTCPeerConnection(configuration);
```

**Media Constraints**:

```javascript
const mediaConstraints = {
  video: {
    width: { ideal: 1280 },
    height: { ideal: 720 },
    frameRate: { ideal: 30 },
  },
  audio: {
    echoCancellation: true,
    noiseSuppression: true,
    autoGainControl: true,
  },
};

navigator.mediaDevices.getUserMedia(mediaConstraints).then((stream) => {
  localVideo.srcObject = stream;
});
```

---

### 10.3. Signaling Flow (Chi tiết)

#### Step 1: User A creates Offer

```javascript
// A: Initialize
const peerConnection = new RTCPeerConnection(configuration);

// A: Add local stream
localStream.getTracks().forEach((track) => {
  peerConnection.addTrack(track, localStream);
});

// A: Handle ICE candidates
peerConnection.onicecandidate = (event) => {
  if (event.candidate) {
    // Send to B via SignalR
    signalRConnection.invoke("SendIceCandidate", {
      roomId: roomId,
      targetUserId: userB.id,
      candidate: event.candidate,
    });
  }
};

// A: Handle incoming remote stream
peerConnection.ontrack = (event) => {
  remoteVideo.srcObject = event.streams[0];
};

// A: Create offer
const offer = await peerConnection.createOffer();
await peerConnection.setLocalDescription(offer);

// A: Send offer to B via SignalR
signalRConnection.invoke("SendWebRTCOffer", {
  roomId: roomId,
  targetUserId: userB.id,
  offer: peerConnection.localDescription,
});
```

---

#### Step 2: User B receives Offer and creates Answer

```javascript
// B: Receive offer from A
signalRConnection.on("ReceiveWebRTCOffer", async (data) => {
  // B: Initialize
  const peerConnection = new RTCPeerConnection(configuration);

  // B: Add local stream
  localStream.getTracks().forEach((track) => {
    peerConnection.addTrack(track, localStream);
  });

  // B: Handle ICE candidates
  peerConnection.onicecandidate = (event) => {
    if (event.candidate) {
      signalRConnection.invoke("SendIceCandidate", {
        roomId: roomId,
        targetUserId: userA.id,
        candidate: event.candidate,
      });
    }
  };

  // B: Handle incoming remote stream
  peerConnection.ontrack = (event) => {
    remoteVideo.srcObject = event.streams[0];
  };

  // B: Set remote description (offer from A)
  await peerConnection.setRemoteDescription(
    new RTCSessionDescription(data.offer)
  );

  // B: Create answer
  const answer = await peerConnection.createAnswer();
  await peerConnection.setLocalDescription(answer);

  // B: Send answer to A via SignalR
  signalRConnection.invoke("SendWebRTCAnswer", {
    roomId: roomId,
    targetUserId: userA.id,
    answer: peerConnection.localDescription,
  });
});
```

---

#### Step 3: User A receives Answer

```javascript
// A: Receive answer from B
signalRConnection.on("ReceiveWebRTCAnswer", async (data) => {
  await peerConnection.setRemoteDescription(
    new RTCSessionDescription(data.answer)
  );

  // Now offer/answer exchange is complete
  // ICE candidates will start flowing
});
```

---

#### Step 4: Both exchange ICE Candidates

```javascript
// Both A and B: Receive ICE candidate
signalRConnection.on("ReceiveIceCandidate", async (data) => {
  try {
    await peerConnection.addIceCandidate(new RTCIceCandidate(data.candidate));
  } catch (error) {
    console.error("Error adding ICE candidate:", error);
  }
});
```

---

#### Step 5: Connection State Monitoring

```javascript
peerConnection.onconnectionstatechange = () => {
  console.log("Connection state:", peerConnection.connectionState);

  switch (peerConnection.connectionState) {
    case "connected":
      console.log("P2P connection established!");
      break;
    case "disconnected":
      console.log("Peer disconnected, attempting reconnect...");
      break;
    case "failed":
      console.log("Connection failed, need to restart");
      restartConnection();
      break;
    case "closed":
      console.log("Connection closed");
      break;
  }
};

peerConnection.oniceconnectionstatechange = () => {
  console.log("ICE state:", peerConnection.iceConnectionState);
};
```

---

### 10.4. Screen Sharing Implementation

```javascript
async function startScreenShare() {
  try {
    // Get screen stream
    const screenStream = await navigator.mediaDevices.getDisplayMedia({
      video: {
        cursor: "always",
        displaySurface: "monitor", // or "window", "application"
      },
      audio: false,
    });

    // Get video track from screen stream
    const screenTrack = screenStream.getVideoTracks()[0];

    // Find video sender in peer connection
    const sender = peerConnection
      .getSenders()
      .find((s) => s.track && s.track.kind === "video");

    if (sender) {
      // Replace camera video with screen video
      await sender.replaceTrack(screenTrack);
    }

    // Update UI
    btnScreenShare.classList.add("active");
    iconScreenShare.innerHTML = "stop_screen_share";

    // Notify others via SignalR
    signalRConnection.invoke("ShareScreen", roomId, userId, true);

    // Store original video track for later restore
    window.originalVideoTrack = localStream.getVideoTracks()[0];

    // Handle when user stops sharing (click browser's "Stop sharing" button)
    screenTrack.onended = () => {
      stopScreenShare();
    };
  } catch (error) {
    console.error("Screen share error:", error);
    alert("Could not share screen. Permission denied or not supported.");
  }
}

async function stopScreenShare() {
  // Find video sender
  const sender = peerConnection
    .getSenders()
    .find((s) => s.track && s.track.kind === "video");

  if (sender && window.originalVideoTrack) {
    // Restore camera video
    await sender.replaceTrack(window.originalVideoTrack);
  }

  // Update UI
  btnScreenShare.classList.remove("active");
  iconScreenShare.innerHTML = "screen_share";

  // Notify others
  signalRConnection.invoke("ShareScreen", roomId, userId, false);
}
```

---

### 10.5. Handling Multiple Peers

**Mesh Topology** (Current implementation):

```
     A ────── B
     │    ╱   │
     │   ╱    │
     │  ╱     │
     │ ╱      │
     C ────── D

Each peer connects to all other peers directly.
For 4 users: 6 connections (n*(n-1)/2)
```

**Code**:

```javascript
const peerConnections = new Map(); // Map<userId, RTCPeerConnection>

// When new user joins
signalRConnection.on("UserJoined", async (newUserId, displayName) => {
  // Create new peer connection for this user
  const pc = createPeerConnection(newUserId);
  peerConnections.set(newUserId, pc);

  // Create offer and send to new user
  const offer = await pc.createOffer();
  await pc.setLocalDescription(offer);
  signalRConnection.invoke("SendWebRTCOffer", roomId, newUserId, offer);
});

// When user leaves
signalRConnection.on("UserLeft", (userId) => {
  const pc = peerConnections.get(userId);
  if (pc) {
    pc.close();
    peerConnections.delete(userId);
  }
  removeVideoElement(userId);
});

function createPeerConnection(userId) {
  const pc = new RTCPeerConnection(configuration);

  // Add local stream
  localStream.getTracks().forEach((track) => {
    pc.addTrack(track, localStream);
  });

  // Handle remote stream
  pc.ontrack = (event) => {
    addRemoteVideo(userId, event.streams[0]);
  };

  // Handle ICE candidates
  pc.onicecandidate = (event) => {
    if (event.candidate) {
      signalRConnection.invoke(
        "SendIceCandidate",
        roomId,
        userId,
        event.candidate
      );
    }
  };

  return pc;
}
```

**Limitations of Mesh**:

- Max ~4-6 users before bandwidth issues
- Each user must upload stream to all others
- CPU/bandwidth intensive

**Alternative: SFU (Selective Forwarding Unit)**:

- Server forwards streams (not implemented)
- Scales to more users
- Requires media server (e.g., Janus, Mediasoup)

---

## 11. SECURITY VÀ AUTHENTICATION

### 11.1. Password Protection

**Set Password**:

```csharp
[HttpPost]
public async Task<IActionResult> ChangeRoomSercurityCode([FromBody] EditRoomModel model)
{
    var room = await _context.Rooms.FindAsync(model.RoomId);
    if (room == null) return NotFound();

    // Only host can change password
    var userId = User.GetUserId();
    if (room.UserId != userId)
        return Forbid("Only host can change room settings");

    // Set or clear password
    room.SecurityCode = string.IsNullOrWhiteSpace(model.SecurityCode)
        ? null
        : model.SecurityCode;

    await _context.SaveChangesAsync();
    return Ok();
}
```

**Verify Password**:

```csharp
[HttpPost]
public async Task<IActionResult> JoinRoom(RoomViewModel obj)
{
    var room = await _context.Rooms.FindAsync(obj.JoinRoom.RoomId);
    if (room == null)
        return NotFound("Room not found");

    // Check password if room is protected
    if (!string.IsNullOrEmpty(room.SecurityCode)) {
        if (room.SecurityCode != obj.JoinRoom.SecurityCode) {
            ModelState.AddModelError("", "Invalid password");
            return View("FriendHub", obj);
        }
    }

    // Allow join
    // ...
}
```

---

### 11.2. Data Validation

**Frontend Validation**:

```javascript
// form-validation.js
function validateField(field) {
  // Required check
  if (field.hasAttribute("required") && !field.value.trim()) {
    showError(field, "This field is required");
    return false;
  }

  // Min length check
  const minLength = field.getAttribute("minlength");
  if (minLength && field.value.trim().length < minLength) {
    showError(field, `Minimum ${minLength} characters required`);
    return false;
  }

  // Max length check
  const maxLength = field.getAttribute("maxlength");
  if (maxLength && field.value.trim().length > maxLength) {
    showError(field, `Maximum ${maxLength} characters allowed`);
    return false;
  }

  // Age range check
  if (field.type === "number") {
    const min = field.getAttribute("min");
    const max = field.getAttribute("max");
    const value = parseInt(field.value);

    if (min && value < min) {
      showError(field, `Value must be at least ${min}`);
      return false;
    }
    if (max && value > max) {
      showError(field, `Value must be at most ${max}`);
      return false;
    }
  }

  hideError(field);
  return true;
}
```

**Backend Validation**:

```csharp
// Data Annotations
public class RegisterDto
{
    [Required]
    [StringLength(20, MinimumLength = 6)]
    public string RoomName { get; set; }

    [Required]
    [StringLength(20, MinimumLength = 6)]
    public string DisplayName { get; set; }

    [StringLength(50)]
    public string? SecurityCode { get; set; }
}

public class JoinRoomDto
{
    [Required]
    public Guid RoomId { get; set; }

    [Required]
    [StringLength(20, MinimumLength = 6)]
    public string DisplayName { get; set; }

    public string? SecurityCode { get; set; }
}

public class StrangerModel
{
    [Required]
    [StringLength(50, MinimumLength = 6)]
    public string DisplayName { get; set; }

    [Required]
    public string Gender { get; set; }

    [Required]
    [Range(13, 100)]
    public int Age { get; set; }

    [Required]
    public string Nationality { get; set; }

    public StrangerFilterDto StrangerFilter { get; set; }
}
```

---

### 11.3. HTTPS và Secure Connections

**Enforce HTTPS**:

```csharp
// Program.cs
if (!app.Environment.IsDevelopment())
{
    app.UseHsts(); // HTTP Strict Transport Security
}
app.UseHttpsRedirection();
```

**Development Certificate**:

```bash
dotnet dev-certs https --trust
```

**WebRTC Security**:

- All WebRTC connections use DTLS-SRTP encryption
- ICE candidates prevent direct IP exposure in some cases
- STUN server helps with NAT traversal
- TURN server provides encrypted relay (if configured)

---

### 11.4. XSS Prevention

**Output Encoding**:

```javascript
// Escape HTML in chat messages
function escapeHtml(text) {
  const map = {
    "&": "&amp;",
    "<": "&lt;",
    ">": "&gt;",
    '"': "&quot;",
    "'": "&#039;",
  };
  return text.replace(/[&<>"']/g, (m) => map[m]);
}

// Usage
connection.on("ReceiveMessage", (userId, name, message, time) => {
  const safeMessage = escapeHtml(message);
  const safeName = escapeHtml(name);
  chatDisplay.innerHTML += `<div>${safeName}: ${safeMessage}</div>`;
});
```

**Razor Encoding**:

```razor
@* Automatic encoding *@
<div>@Model.DisplayName</div>

@* Raw HTML (use with caution) *@
<div>@Html.Raw(Model.RichContent)</div>
```

---

### 11.5. CORS Configuration

```csharp
// Program.cs
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowWebApp", policy =>
    {
        policy.WithOrigins("https://localhost:7198", "https://yourdomain.com")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials(); // For SignalR
    });
});

app.UseCors("AllowWebApp");
```

---

### 11.6. Rate Limiting

**Concept** (Not fully implemented):

```csharp
// Limit room creation
[RateLimit(PerMinute = 5)]
public async Task<IActionResult> CreateRoom(RoomViewModel obj)
{
    // ...
}

// Limit messages
[RateLimit(PerMinute = 60)]
public async Task SendMessage(Guid roomId, string message)
{
    // ...
}
```

---

## 12. TESTING VÀ DEPLOYMENT

### 12.1. Testing Strategy

#### Unit Tests (Concept)

```csharp
public class RoomServiceTests
{
    [Fact]
    public async Task CreateRoom_ValidData_ReturnsRoom()
    {
        // Arrange
        var mockRepo = new Mock<IRoomRepository>();
        var service = new RoomService(mockRepo.Object);
        var dto = new CreateRoomDto {
            RoomName = "Test Room",
            DisplayName = "John Doe"
        };

        // Act
        var result = await service.CreateRoomAsync(dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test Room", result.RoomName);
    }

    [Fact]
    public async Task CreateRoom_InvalidName_ThrowsException()
    {
        // Arrange
        var service = new RoomService(Mock.Of<IRoomRepository>());
        var dto = new CreateRoomDto {
            RoomName = "Test", // Too short
            DisplayName = "John"
        };

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(
            () => service.CreateRoomAsync(dto)
        );
    }
}
```

---

#### Integration Tests (Concept)

```csharp
public class RoomControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public RoomControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateRoom_ReturnsRedirect()
    {
        // Arrange
        var formData = new Dictionary<string, string>
        {
            ["CreateRoom.RoomName"] = "Test Room",
            ["CreateRoom.DisplayName"] = "John Doe"
        };
        var content = new FormUrlEncodedContent(formData);

        // Act
        var response = await _client.PostAsync("/Home/CreateRoom", content);

        // Assert
        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        Assert.Contains("/Room/Meeting/", response.Headers.Location.ToString());
    }
}
```

---

#### Manual Testing Checklist

**Friend Room**:

- [ ] Create room với display name hợp lệ
- [ ] Create room với display name quá ngắn (< 6)
- [ ] Join room với room ID đúng
- [ ] Join room với room ID sai
- [ ] Join room có password
- [ ] Join room với password sai
- [ ] Video call giữa 2 users
- [ ] Video call giữa 3-4 users
- [ ] Mute/unmute microphone
- [ ] Turn on/off camera
- [ ] Share screen
- [ ] Send text messages
- [ ] Copy room URL
- [ ] Change room password (host)
- [ ] Leave room
- [ ] Room deleted khi tất cả users leave

**Stranger**:

- [ ] Register với thông tin hợp lệ
- [ ] Set preferences
- [ ] Matching thành công
- [ ] Matching timeout
- [ ] Accept match
- [ ] Decline match
- [ ] Video call với stranger
- [ ] Skip stranger (Next button)

**Cross-browser**:

- [ ] Chrome
- [ ] Firefox
- [ ] Edge
- [ ] Safari (macOS/iOS)

**Mobile**:

- [ ] Android Chrome
- [ ] iOS Safari

---

### 12.2. Deployment

#### 12.2.1. Database Migration

**Local → Production**:

```bash
# Generate SQL script
dotnet ef migrations script -o migration.sql

# Apply on production SQL Server
sqlcmd -S production-server -d TalkFlow -i migration.sql
```

---

#### 12.2.2. IIS Deployment

**Step 1: Publish**:

```bash
dotnet publish -c Release -o ./publish
```

**Step 2: IIS Setup**:

1. Cài đặt .NET 8.0 Hosting Bundle
2. Tạo Application Pool:
   - Name: TalkFlow
   - .NET CLR Version: No Managed Code
   - Managed Pipeline Mode: Integrated
3. Tạo Website:
   - Site name: TalkFlow
   - Physical path: C:\inetpub\wwwroot\TalkFlow
   - Binding: https, port 443, SSL certificate
4. Copy files từ ./publish vào physical path
5. Set permissions: IIS_IUSRS read/execute

**Step 3: web.config**:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <system.webServer>
    <handlers>
      <add name="aspNetCore" path="*" verb="*"
           modules="AspNetCoreModuleV2"
           resourceType="Unspecified" />
    </handlers>
    <aspNetCore processPath="dotnet"
                arguments=".\TalkFlow.Web.dll"
                stdoutLogEnabled="false"
                stdoutLogFile=".\logs\stdout"
                hostingModel="inprocess" />
  </system.webServer>
</configuration>
```

---

#### 12.2.3. Azure App Service

**Step 1: Publish via VS**:

1. Right-click project → Publish
2. Target: Azure
3. Specific target: Azure App Service (Windows)
4. Create new App Service
5. Publish

**Step 2: Configuration**:

- Application Settings → Add connection string
- SSL Settings → Enforce HTTPS
- Scale Up → Choose pricing tier (B1 minimum for SignalR)

**Step 3: SignalR Service** (Optional):

- Tạo Azure SignalR Service resource
- Copy connection string
- Add to appsettings.json:

```json
{
  "Azure": {
    "SignalR": {
      "ConnectionString": "..."
    }
  }
}
```

- Update Program.cs:

```csharp
builder.Services.AddSignalR().AddAzureSignalR();
```

---

#### 12.2.4. Docker Deployment

**Dockerfile**:

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["TalkFlow.Web/TalkFlow.Web.csproj", "TalkFlow.Web/"]
COPY ["TalkFlow/TalkFlow.csproj", "TalkFlow/"]
RUN dotnet restore "TalkFlow.Web/TalkFlow.Web.csproj"
COPY . .
WORKDIR "/src/TalkFlow.Web"
RUN dotnet build "TalkFlow.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TalkFlow.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TalkFlow.Web.dll"]
```

**docker-compose.yml**:

```yaml
version: "3.8"
services:
  web:
    build: .
    ports:
      - "5000:80"
      - "5001:443"
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - ConnectionStrings__DefaultConnection=Server=db;Database=TalkFlow;User=sa;Password=Your_password123
    depends_on:
      - db

  db:
    image: mcr.microsoft.com/mssql/server:2022-latest
    environment:
      - ACCEPT_EULA=Y
      - SA_PASSWORD=Your_password123
    ports:
      - "1433:1433"
    volumes:
      - sqldata:/var/opt/mssql

volumes:
  sqldata:
```

**Commands**:

```bash
# Build
docker-compose build

# Run
docker-compose up -d

# View logs
docker-compose logs -f

# Stop
docker-compose down
```

---

#### 12.2.5. Production Checklist

**Configuration**:

- [ ] Update connection string
- [ ] Set strong JWT secret key
- [ ] Configure CORS for specific domains
- [ ] Enable HTTPS with valid SSL certificate
- [ ] Set up TURN server for WebRTC
- [ ] Configure logging (Serilog, Application Insights)
- [ ] Set up health checks
- [ ] Enable response compression
- [ ] Configure rate limiting
- [ ] Set up database backups

**appsettings.Production.json**:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Warning",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=prod-server;Database=TalkFlow;User Id=talkflow_user;Password=***"
  },
  "JwtSettings": {
    "SecretKey": "your-super-secure-secret-key-min-32-chars",
    "Issuer": "https://talkflow.com",
    "Audience": "https://talkflow.com",
    "ExpiryMinutes": 60
  },
  "AllowedHosts": "talkflow.com,www.talkflow.com",
  "WebRTCConfig": {
    "IceServers": [
      { "urls": "stun:stun.l.google.com:19302" },
      {
        "urls": "turn:your-turn.server.com:3478",
        "username": "user",
        "credential": "pass"
      }
    ]
  }
}
```

---

### 12.3. Monitoring

**Logging**:

```csharp
// Program.cs
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.AddEventLog();

// In controllers
private readonly ILogger<HomeController> _logger;

public HomeController(ILogger<HomeController> logger)
{
    _logger = logger;
}

public IActionResult CreateRoom(RoomViewModel obj)
{
    _logger.LogInformation("User {UserId} creating room {RoomName}",
        User.GetUserId(), obj.CreateRoom.RoomName);
    // ...
}
```

**Health Checks**:

```csharp
// Program.cs
builder.Services.AddHealthChecks()
    .AddSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
    .AddSignalRHub("/chathub", name: "signalr-chathub");

app.MapHealthChecks("/health");
```

**Application Insights** (Azure):

```csharp
builder.Services.AddApplicationInsightsTelemetry();
```

---

### 12.4. Performance Optimization

**Database Indexing**:

```csharp
// In DataContext.OnModelCreating
modelBuilder.Entity<Room>()
    .HasIndex(r => r.RoomId);

modelBuilder.Entity<Connection>()
    .HasIndex(c => c.UserID);

modelBuilder.Entity<AppUser>()
    .HasIndex(u => new { u.Gender, u.Age, u.Nationality });
```

**Caching**:

```csharp
builder.Services.AddMemoryCache();

// In controller
private readonly IMemoryCache _cache;

public async Task<IActionResult> GetRoom(Guid id)
{
    if (!_cache.TryGetValue($"room_{id}", out Room room))
    {
        room = await _context.Rooms.FindAsync(id);
        _cache.Set($"room_{id}", room, TimeSpan.FromMinutes(10));
    }
    return Ok(room);
}
```

**SignalR Scale-Out** (Redis):

```csharp
builder.Services.AddSignalR()
    .AddStackExchangeRedis("localhost:6379");
```

**Static File Caching**:

```csharp
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers.Append(
            "Cache-Control", "public,max-age=31536000");
    }
});
```

---

## 🎓 KẾT LUẬN

### Điểm mạnh của dự án:

1. ✅ **Real-time communication** với SignalR và WebRTC
2. ✅ **P2P video call** giảm tải server
3. ✅ **Stranger matching** với thuật toán thông minh
4. ✅ **Modern UI/UX** responsive, đẹp mắt
5. ✅ **Security** với password protection, HTTPS
6. ✅ **Scalable architecture** dễ mở rộng

### Hạn chế:

1. ⚠️ Mesh topology giới hạn ~4-6 users/room
2. ⚠️ Chưa có persistence cho messages
3. ⚠️ Stranger matching chưa optimize (O(n²))
4. ⚠️ Chưa có user authentication (anonymous only)

### Hướng phát triển:

1. 📈 SFU media server cho scale lớn hơn
2. 💬 Lưu chat history vào database
3. 👤 User accounts với profile, friends list
4. 📊 Analytics và monitoring
5. 📱 Mobile apps (React Native)
6. 🎥 Recording meetings
7. 🔔 Notifications (email, push)
8. 🌍 i18n (đa ngôn ngữ)

---

## 📚 TÀI LIỆU THAM KHẢO

1. **ASP.NET Core Documentation**: https://docs.microsoft.com/en-us/aspnet/core/
2. **SignalR Documentation**: https://docs.microsoft.com/en-us/aspnet/core/signalr/
3. **WebRTC Documentation**: https://webrtc.org/
4. **Entity Framework Core**: https://docs.microsoft.com/en-us/ef/core/
5. **Bootstrap 5**: https://getbootstrap.com/docs/5.3/
6. **PeerJS**: https://peerjs.com/docs.html
7. **MDN Web Docs - WebRTC**: https://developer.mozilla.org/en-US/docs/Web/API/WebRTC_API

---

© 2025 TalkFlow - All Rights Reserved
