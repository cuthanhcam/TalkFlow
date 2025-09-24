# Database Entities

## Tổng quan

- **DB**: SQL Server
- **ORM**: EF Core 8
- **Auth**: ASP.NET Core Identity (User/Role)
- **Migrations**: Code First

## Entities (Domain -> EF mapping)

### User (IdentityUser<Guid> mở rộng trong Domain)

Table: `AspNetUsers`

- Id: Guid (PK)
- UserName: string (auto GUID)
- Email: string?
- DisplayName: string (ValueObject => string, max 50)
- LastActive: DateTime (UTC)
- IsLocked: bool (default false)
- PhotoUrl: string? (ValueObject => string, max 500)
- Gender: string? (Male/Female/Other/PreferNotToSay)
- Age: int? (13..120)
- Nationality: string? (max 100)
- Profile: UserProfile? (1:1, optional)
- StrangerFilter: StrangerFilter? (1:1, optional)
- Rooms: ICollection<Room> (Host)

Indexes: DisplayName(unique), Email, LastActive

### AppRole (Identity)

Table: `AspNetRoles`

- Id: Guid (PK)
- Name: string (unique)
- NormalizedName: string (unique)

### Room (Aggregate Root)

Table: `Rooms`

- Id: Guid (PK) — RoomId (ValueObject)
- Name: string (RoomName VO, max 100, required, unique index)
- SecurityCode: string? (SecurityCode VO, 4..20)
- Capacity: int (RoomCapacity VO, derived from Connections.Count)
- HostId: Guid (FK → AspNetUsers.Id)
- CreatedAt: DateTime (UTC)
- IsChatBlocked: bool (default false)
- IsActive: bool (default true)

Navigation:

- Connections (1:N) → `Connections`
- Messages (1:N) → `Messages`

Indexes: Name(unique), SecurityCode, HostId, CreatedAt, IsActive

### Connection (Entity)

Table: `Connections`

- ConnectionId: string (PK, max 256)
- UserId: Guid (FK → AspNetUsers.Id)
- RoomId: Guid (FK → Rooms.Id)
- ConnectedAt: DateTime (UTC)

Indexes: UserId, ConnectedAt

### Message (Entity)

Table: `Messages`

- Id: Guid (PK)
- RoomId: Guid (FK → Rooms.Id)
- SenderId: Guid (FK → AspNetUsers.Id)
- SenderDisplayName: string (max 50)
- Content: string (max 1000)
- SentAt: DateTime (UTC)
- IsDeleted: bool (default false)

Indexes: SenderId, SentAt, IsDeleted

### StrangerFilter (Entity)

Table: `StrangerFilters`

- Id: Guid (PK)
- UserId: Guid (FK → AspNetUsers.Id)
- FindGender: string (CSV persisted, mapped to ICollection<string>)
- MinAge: int (default 0)
- MaxAge: int (default 100)
- FindRegion: string (CSV persisted, mapped to ICollection<string>)
- CurrentRoomId: Guid? (nullable, tham chiếu Room đang ghép)
- CreatedAt: DateTime (UTC)
- UpdatedAt: DateTime? (UTC)

Indexes: MinAge, MaxAge, CreatedAt

### Match (Aggregate Root) - **New**

Table: `Matches`

- Id: Guid (PK)
- User1Id: Guid (FK → AspNetUsers.Id)
- User2Id: Guid (FK → AspNetUsers.Id)
- CreatedAt: DateTime (UTC)
- IsCompleted: bool (default false)
- CompletedAt: DateTime? (UTC)

### StrangerMatching (Aggregate Root) - **New**

Table: `StrangerMatchings`

- Id: Guid (PK)
- UserId: Guid (FK → AspNetUsers.Id)
- Status: string (Waiting/Matched/Cancelled)
- CreatedAt: DateTime (UTC)
- MatchedAt: DateTime? (UTC)
- MatchedUserId: Guid? (FK → AspNetUsers.Id)

## Quan hệ chính

- User 1..N Room (Host)
- Room 1..N Connection, 1..N Message
- User 1..1 StrangerFilter
- Connection N..1 Room, N..1 User
- Message N..1 Room, N..1 User
- User 1..N Match (User1 hoặc User2)
- User 1..N StrangerMatching

## Gợi ý seed & roles

- Roles: Admin, Host, Member (tùy nhu cầu)
- Seed admin user (optional)

## Gợi ý index bổ sung (tối ưu truy vấn realtime)

```sql
CREATE INDEX IX_Messages_RoomId_SentAt ON Messages(RoomId, SentAt DESC);
CREATE INDEX IX_Connections_RoomId ON Connections(RoomId);
CREATE INDEX IX_Rooms_IsActive_CreatedAt ON Rooms(IsActive, CreatedAt DESC);
```

## EF Core Configurations

### Existing Configurations
- **UserConfiguration.cs**: User entity mapping
- **UserProfileConfiguration.cs**: UserProfile entity mapping  
- **RoomConfiguration.cs**: Room entity mapping
- **ConnectionConfiguration.cs**: Connection entity mapping
- **MessageConfiguration.cs**: Message entity mapping
- **StrangerFilterConfiguration.cs**: StrangerFilter entity mapping
- **MatchConfiguration.cs**: Match entity mapping
- **StrangerMatchingConfiguration.cs**: StrangerMatching entity mapping

## Lưu ý mapping ValueObjects → Columns

- DisplayName, RoomName, SecurityCode, PhotoUrl, Gender, Nationality: map sang string
- Age, Capacity: map sang int  
- Collection strings lưu CSV (FindGender, FindRegion) theo cấu hình HasConversion

## Implementation Status

### Completed
- Tất cả entities được implement đầy đủ
- EF Core configurations hoàn chỉnh
- Migrations được setup
- Value Objects mapping hoạt động

### Current Database
- **Name**: TalkFlow
- **Provider**: SQL Server / LocalDB
- **Migrations**: Code First approach
- **Connection**: Configured trong `TalkFlow.Presentation/appsettings.json`

## Khả năng mở rộng

- Soft delete: thêm IsDeleted cho Room/User nếu cần
- Audit: thêm CreatedBy, ModifiedBy, ModifiedAt  
- Partition Messages theo RoomId nếu lưu lượng lớn
- Indexing optimization cho performance
