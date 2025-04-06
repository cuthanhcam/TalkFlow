# Database Schema - TalkFlow

## Users
- Id (Guid, PK)
- Username (nvarchar(50), unique)
- Email (nvarchar(100), unique)
- PasswordHash (nvarchar(256))
- CreatedAt (datetime)
- Preferences (nvarchar(max), JSON)

## ChatRooms
- Id (Guid, PK)
- Name (nvarchar(100))
- IsPrivate (bit)
- CreatedAt (datetime)

## ChatRoomUsers
- Id (Guid, PK)
- ChatRoomId (Guid, FK -> ChatRooms)
- UserId (Guid, FK -> Users)
- JoinedAt (datetime)

## Messages
- Id (Guid, PK)
- ChatRoomId (Guid, FK -> ChatRooms)
- UserId (Guid, FK -> Users)
- Content (nvarchar(max))
- SentAt (datetime)