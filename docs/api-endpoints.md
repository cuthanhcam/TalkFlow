# API Endpoints - TalkFlow

## Authentication (chưa triển khai)
- POST /api/auth/register - Đăng ký người dùng
- POST /api/auth/login - Đăng nhập (trả về JWT)
- GET /api/auth/user - Lấy thông tin người dùng (yêu cầu token)

## Chat (chưa triển khai)
- GET /api/chat/rooms - Lấy danh sách phòng chat
- POST /api/chat/rooms - Tạo phòng chat
- GET /api/chat/messages/{roomId} - Lấy tin nhắn trong phòng

## SignalR Hub
- /chatHub - Endpoint cho chat thời gian thực