# Project Structure - TalkFlow

```
TalkFlow/
├── src/
│   ├── TalkFlow.API/                 # Dự án Web API (Presentation Layer)
│   │   ├── Controllers/             # Các API endpoints
│   │   ├── Dtos/                    # Data Transfer Objects (đối tượng truyền dữ liệu)
│   │   ├── Middleware/              # Middleware tùy chỉnh (nếu cần)
│   │   ├── Program.cs               # Cấu hình ứng dụng
│   │   └── appsettings.json         # Cấu hình (connection string, secrets)
│   │
│   ├── TalkFlow.Application/        # Application Layer (Business Logic)
│   │   ├── Interfaces/              # Interface cho các service
│   │   ├── Services/                # Implement các service (chat, auth, matching)
│   │   └── Models/                  # View Models hoặc Application Models
│   │
│   ├── TalkFlow.Domain/             # Domain Layer (Core Business Logic)
│   │   ├── Entities/                # Các entity (User, ChatRoom, Message)
│   │   ├── Interfaces/              # Interface cho repository
│   │   └── ValueObjects/            # Các đối tượng giá trị (nếu cần)
│   │
│   ├── TalkFlow.Infrastructure/     # Infrastructure Layer (Data Access, External Services)
│   │   ├── Data/                    # DbContext, migrations
│   │   ├── Repositories/            # Implement repository
│   │   └── Services/                # Các dịch vụ bên ngoài (SignalR, WebRTC)
│   │
│   └── TalkFlow.Tests/              # Unit Tests (nếu bạn muốn viết test)
│       ├── UnitTests/               # Test cho Application/Domain
│       └── IntegrationTests/        # Test tích hợp
│
├── docs/                            # Tài liệu dự án
│   ├── database-schema.md           # Mô tả schema cơ sở dữ liệu
│   ├── project-workflows.md         # Quy trình làm việc
│   └── api-endpoints.md             # Danh sách API
│
├── .gitignore                       # File cấu hình Git
├── Dockerfile                       # File Docker (sẽ thêm sau)
├── docker-compose.yml               # Cấu hình Docker Compose (sẽ thêm sau)
└── TalkFlow.sln                     # Solution file
```