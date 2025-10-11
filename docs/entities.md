# Database Entities

## Overview

- **Database**: SQL Server / LocalDB
- **ORM**: Entity Framework Core 8.0.10
- **Authentication**: ASP.NET Core Identity
- **Migration Strategy**: Code First
- **Connection**: Configured in `TalkFlow/appsettings.json`

## Current Project Structure

**TalkFlow** has a simplified architecture with 2 main projects:
- `TalkFlow/` - Backend API (Controllers, Services, Repositories, Entities, SignalR Hubs)
- `TalkFlow.Web/` - Frontend Web App (MVC Controllers, Views, wwwroot)

## Entity Relationship Diagram

```
┌──────────────────────────┐
│      AspNetUsers         │
├──────────────────────────┤
│ Id (PK, Guid)            │
│ UserName                 │
│ DisplayName              │
│ Gender                   │
│ Age                      │
│ Nationality              │
│ PhotoUrl                 │
│ LastActive               │
│ Locked                   │
└───────┬──────────────────┘
        │ 1:1
        │
┌───────▼──────────────────┐
│    StrangerFilters       │
├──────────────────────────┤
│ FilterID (PK)            │
│ _FindGender              │
│ MinAge                   │
│ MaxAge                   │
│ _FindRegion              │
│ CurrentRoomRoomId (FK)   │
└──────────────────────────┘

┌──────────────────────────┐
│         Rooms            │
├──────────────────────────┤
│ RoomId (PK)              │
│ RoomName                 │
│ SecurityCode             │
│ CountMember              │
│ UserId (FK)              │
│ CreatedDate              │
│ BlockedChat              │
└───────┬──────────────────┘
        │ 1:N
        │
┌───────▼──────────────────┐
│      Connections         │
├──────────────────────────┤
│ ConnectionId (PK)        │
│ UserID                   │
│ RoomId (FK)              │
└──────────────────────────┘
```

## Entities (EF Core Implementation)

### AppUser (IdentityUser<Guid>)

**Table**: `AspNetUsers`

**Location**: `TalkFlow/Entities/AppUser.cs`

| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| Id | Guid | PK, NOT NULL | User unique identifier |
| UserName | nvarchar(256) | Unique | Username (auto-generated GUID) |
| Email | nvarchar(256) | Nullable | Email address |
| DisplayName | nvarchar(MAX) | NOT NULL | Display name (6-50 characters) |
| LastActive | datetime2 | NOT NULL | Last active timestamp |
| Locked | bit | Default(0) | Account lock status |
| PhotoUrl | nvarchar(MAX) | Nullable | Avatar URL |
| Gender | nvarchar(MAX) | Nullable | Male/Female/Others |
| Age | int | Nullable | User age (13-100) |
| Nationality | nvarchar(MAX) | Nullable | Country code (e.g., VN, US) |

**Relationships**:
- 1:1 with `StrangerFilters` (optional)
- 1:N with `Rooms` (as Host)
- 1:N with `Connections`

**Indexes**: LastActive

---

### AppRole (IdentityRole<Guid>)

**Table**: `AspNetRoles`

**Location**: `TalkFlow/Entities/AppRole.cs`

| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| Id | Guid | PK, NOT NULL | Role unique identifier |
| Name | nvarchar(256) | Unique | Role name |
| NormalizedName | nvarchar(256) | Unique | Normalized role name |

**Default Roles**: Admin, Member (configured in seed data)

---

### Room

**Table**: `Rooms`

**Location**: `TalkFlow/Entities/Room.cs`

| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| RoomId | Guid | PK, NOT NULL | Room unique identifier |
| RoomName | nvarchar(MAX) | Nullable | Room name |
| SecurityCode | nvarchar(MAX) | Nullable | Password for room (4-20 chars) |
| CountMember | int | NOT NULL, Default(0) | Current member count |
| UserId | Guid | FK, NOT NULL | Host user ID |
| CreatedDate | datetime2 | NOT NULL | Room creation timestamp |
| BlockedChat | bit | NOT NULL, Default(0) | Chat blocked status |

**Relationships**:
- N:1 with `AppUser` (Host)
- 1:N with `Connections`

**Navigation Properties**:
- `AppUser User` - Host user
- `ICollection<Connection> Connections` - Active connections

**Business Rules**:
- Room can be password-protected
- Only host can modify room settings
- CountMember is updated when users join/leave

---

### Connection

**Table**: `Connections`

**Location**: `TalkFlow/Entities/Connection.cs`

| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| ConnectionId | nvarchar(450) | PK, NOT NULL | SignalR connection ID |
| UserID | Guid | NOT NULL | User ID |
| RoomId | Guid | FK, Nullable | Room ID (if in a room) |

**Relationships**:
- N:1 with `Rooms` (optional)

**Navigation Properties**:
- `Room? Room` - Associated room

**Purpose**:
- Track SignalR connections
- Map connections to rooms
- Used for broadcasting messages

---

### Message (Planned/Not Currently Implemented)

**Table**: `Messages` (not in current schema)

Note: Message persistence is not currently implemented. Messages are sent real-time via SignalR but not stored in database.

If implemented, the schema would be:

| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| Id | Guid | PK, NOT NULL | Message ID |
| RoomId | Guid | FK, NOT NULL | Room ID |
| SenderId | Guid | FK, NOT NULL | Sender user ID |
| Content | nvarchar(1000) | NOT NULL | Message content |
| SentAt | datetime2 | NOT NULL | Send timestamp |

**Future Enhancement**: Add message history feature

---

### StrangerFilter

**Table**: `StrangerFilters`

**Location**: `TalkFlow/Entities/StrangerFilter.cs`

| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| FilterID | Guid | PK, NOT NULL | Filter unique identifier |
| _FindGender | nvarchar(MAX) | NOT NULL | CSV: "Male,Female,Others" |
| MinAge | int | NOT NULL, Default(0) | Minimum age preference |
| MaxAge | int | NOT NULL, Default(100) | Maximum age preference |
| _FindRegion | nvarchar(MAX) | NOT NULL | CSV: "VN,US,JP" |
| CurrentRoomRoomId | Guid | FK, Nullable | Current matched room |

**Relationships**:
- 1:1 with `AppUser` (back reference via AppUser.StrangerFilterFilterID)
- N:1 with `Rooms` (optional)

**Navigation Properties**:
- `Room? CurrentRoom` - Current matched room

**Special Properties** (NotMapped):
```csharp
[NotMapped]
public ICollection<string> FindGender { 
    get => _FindGender?.Split(',').ToList() ?? new List<string>(); 
    set => _FindGender = string.Join(',', value); 
}

[NotMapped]
public ICollection<string> FindRegion { 
    get => _FindRegion?.Split(',').ToList() ?? new List<string>(); 
    set => _FindRegion = string.Join(',', value); 
}
```

**Usage**: Stores user preferences for stranger matching algorithm

---

### Match & StrangerMatching (Not Implemented)

Note: These entities are **not currently implemented** in the database. Stranger matching is handled in-memory via SignalR StrangerHub.

---

## Entity Relationships Summary

```
AppUser (1) ──── (N) Room [as Host]
AppUser (1) ──── (1) StrangerFilter [optional]
Room (1) ──── (N) Connection
StrangerFilter (N) ──── (1) Room [optional CurrentRoom]
```

**Key Points**:
- One user can host multiple rooms
- Each user can have one stranger filter
- Rooms contain multiple connections (SignalR)
- Stranger filters track current matched room

---

## Seed Data

**Location**: `TalkFlow/Data/Seed.cs`

### Default Roles
- **Admin**: Full system access
- **Member**: Regular user access

### Seed Process
```csharp
public static async Task SeedUsers(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
{
    // Creates default roles if they don't exist
    // Can be extended to create default admin user
}
```

**Execution**: Runs automatically on application startup in `Program.cs`

---

## Performance Optimization

### Current Indexes
- `IX_AspNetUsers_LastActive` on AspNetUsers(LastActive)
- Primary keys on all tables

### Recommended Future Indexes
```sql
CREATE INDEX IX_Connections_RoomId ON Connections(RoomId);
CREATE INDEX IX_Connections_UserID ON Connections(UserID);
CREATE INDEX IX_Rooms_UserId_CreatedDate ON Rooms(UserId, CreatedDate DESC);
CREATE INDEX IX_StrangerFilters_CurrentRoomRoomId ON StrangerFilters(CurrentRoomRoomId) WHERE CurrentRoomRoomId IS NOT NULL;
```

---

## EF Core Configuration

**Location**: `TalkFlow/Data/DataContext.cs`

### DbContext
```csharp
public class DataContext : IdentityDbContext<AppUser, AppRole, Guid>
{
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Connection> Connections { get; set; }
    public DbSet<StrangerFilter> StrangerFilters { get; set; }
}
```

### Entity Configurations
Configurations are defined using Fluent API in `OnModelCreating`:
- Relationships and foreign keys
- Index definitions
- Column constraints
- Default values

---

## Data Mapping Notes

### String Properties
- All string properties use `nvarchar(MAX)` for flexibility
- No explicit length constraints in database (validated in application layer)

### CSV Storage
- `FindGender` and `FindRegion` stored as CSV strings
- Converted to `ICollection<string>` via `[NotMapped]` properties
- Example: "Male,Female,Others" → List<string> { "Male", "Female", "Others" }

### Boolean Properties
- Stored as `bit` in SQL Server
- Default values set in migrations

---

## Implementation Status

### ✅ Completed
- Core entities: AppUser, AppRole, Room, Connection, StrangerFilter
- ASP.NET Core Identity integration
- EF Core migrations
- Seed data system
- SignalR connection tracking

### ❌ Not Implemented
- Message persistence (messages are real-time only)
- Match and StrangerMatching entities
- Message history feature
- User profiles (separate table)

### Database Information
- **Name**: TalkFlow (or TalkFlowDb)
- **Provider**: SQL Server / LocalDB
- **Migration Strategy**: Code First
- **Connection String**: `TalkFlow/appsettings.json`
- **Current Migration**: Initial migration with all core tables

---

## Future Enhancements

### Planned Features
1. **Message History**
   - Add Messages table
   - Store chat history
   - Pagination support

2. **Soft Delete**
   - Add `IsDeleted` flag to Room and AppUser
   - Implement soft delete pattern

3. **Audit Trail**
   - Add `CreatedBy`, `CreatedAt`, `ModifiedBy`, `ModifiedAt` to entities
   - Track all changes

4. **Performance**
   - Implement caching for frequently accessed data
   - Add database indexes for common queries
   - Consider NoSQL for message history (MongoDB/Redis)

5. **Scalability**
   - Partition messages by RoomId if volume is high
   - Implement database sharding
   - Use read replicas for queries

---

**Last Updated**: January 2025  
**EF Core Version**: 8.0.10  
**Database Provider**: Microsoft.EntityFrameworkCore.SqlServer
