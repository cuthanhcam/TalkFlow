# Project Context - TalkFlow

## Mục tiêu dự án
- **Tên dự án**: TalkFlow
- **Mô tả**: Xây dựng một ứng dụng web trò chuyện sử dụng .NET Core Web API, hỗ trợ đăng ký/đăng nhập với OAuth2, ghép cặp trò chuyện, gọi video, và có tiềm năng phát triển thành mạng xã hội.
- **Công nghệ chính**:
  - Backend: .NET 8.0, ASP.NET Core Web API, Entity Framework Core, SignalR (chat thời gian thực), WebRTC (gọi video - chưa triển khai).
  - Frontend: Dự kiến dùng HTML, CSS, Tailwind, React (chưa bắt đầu).
  - DevOps: Dự kiến dùng Docker và cloud deployment (chưa bắt đầu).
- **Tính năng chính**:
  1. Đăng ký/đăng nhập với OAuth2 (JWT).
  2. Ghép cặp trò chuyện dựa trên sở thích.
  3. Chat thời gian thực qua SignalR.
  4. Gọi video (tương lai).
  5. Mở rộng thành mạng xã hội (tương lai).

## Trạng thái hiện tại (tính đến  Asc06, 2025)
- **Backend**:
  - Đã thiết lập cấu trúc dự án theo Clean Architecture với 5 layer: `TalkFlow.API`, `TalkFlow.Application`, `TalkFlow.Domain`, `TalkFlow.Infrastructure`, `TalkFlow.Tests`.
  - Đã cấu hình `TalkFlow.API/Program.cs` với:
    - DbContext (SQL Server, database `TalkFlowDb` với bảng `Users`).
    - SignalR (ChatHub cho chat thời gian thực - chưa test).
    - JWT Authentication (dùng JwtBearer, secret key từ `appsettings.json`).
    - CORS (AllowAll cho dev).
    - Swagger (đã hoạt động với `dotnet watch run`, fix lỗi `dotnet run` với JWT).
  - Đã tạo migration `InitialCreate` và áp dụng vào database.
- **Frontend**: Chưa bắt đầu.
- **DevOps**: Chưa bắt đầu.

## Các bước đã thực hiện
1. **Khởi tạo dự án**:
   - Tạo solution và các project con bằng lệnh `dotnet new` trong VSCode/PowerShell.
   - Cấu trúc thư mục: `src/`, `docs/`, `.gitignore`, `TalkFlow.sln`.
2. **Cài đặt package**:
   - `TalkFlow.API`: `Microsoft.EntityFrameworkCore`, `Microsoft.EntityFrameworkCore.SqlServer`, `Microsoft.EntityFrameworkCore.Design`, `Microsoft.AspNetCore.SignalR.Core`, `Microsoft.AspNetCore.Authentication.JwtBearer`.
   - `TalkFlow.Infrastructure`: `Microsoft.EntityFrameworkCore.SqlServer`, `Microsoft.EntityFrameworkCore.Design`.
3. **Cấu hình database**:
   - Entity `User` trong `TalkFlow.Domain`.
   - `TalkFlowDbContext` trong `TalkFlow.Infrastructure`.
   - Migration `InitialCreate` tạo bảng `Users`.
4. **Fix lỗi**:
   - Lỗi `AddDbContext`: Thêm package và reference.
   - Lỗi `UseSqlServer`: Thêm package `Microsoft.EntityFrameworkCore.SqlServer`.
   - Lỗi JWT `ArgumentNullException`: Thêm `JwtSettings` trong `appsettings.json` và kiểm tra null.
5. **Swagger**: Đã hoạt động với `dotnet watch run`, fix lỗi `dotnet run` bằng cách thêm secret key.

## Các bước tiếp theo
1. Triển khai API đăng ký/đăng nhập với OAuth2 (JWT).
2. Tích hợp SignalR cho chat thời gian thực.
3. Thêm tính năng ghép cặp trò chuyện.
4. Bắt đầu frontend với React/Tailwind.
5. Tìm hiểu và áp dụng DevOps (Docker, cloud).

## Ghi chú
- **Môi trường phát triển**: Windows, VSCode, PowerShell, .NET 8.
- **Database**: `(JERK\\CAMDB)\talkflow`.
- **Port**: `https://localhost:5086` (cố định trong `appsettings.json`).