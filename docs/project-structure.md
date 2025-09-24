# TalkFlow DDD Project Structure

## Tổng quan

Cấu trúc dự án TalkFlow theo Domain-Driven Design (DDD) với Clean Architecture, tách biệt rõ ràng các layer và responsibility.

## Solution Structure

```
TalkFlow.sln
├── src/
│   ├── TalkFlow.Domain/                   # Domain Layer
│   ├── TalkFlow.Application/              # Application Layer
│   ├── TalkFlow.Infrastructure/           # Infrastructure Layer
│   ├── TalkFlow.Presentation/             # Presentation Layer
│   └── TalkFlow.Shared/                   # Shared Layer
├── tests/
│   ├── TalkFlow.UnitTests/                 # Unit Tests
│   ├── TalkFlow.IntegrationTests/          # Integration Tests
│   └── TalkFlow.ArchitectureTests/         # Architecture Tests
├── scripts/                                # Build/Deploy Scripts
├── docs/                                   # Documentation (MD files)
├── *.md                                    # Project documentation
├── *.html                                  # Test files
├── *.json                                  # Postman collections & configs
└── *.sh                                    # Shell scripts
```

## Domain Layer (`TalkFlow.Domain`)

### Aggregates

```
TalkFlow.Domain/
├── Aggregates/
│   ├── User/
│   │   ├── User.cs                          # Root Entity
│   │   ├── UserProfile.cs                   # Entity
│   │   ├── StrangerFilter.cs                # Entity
│   │   ├── AppRole.cs                       # Entity
│   │   └── Events/
│   │       ├── UserCreatedEvent.cs
│   │       ├── UserProfileUpdatedEvent.cs
│   │       └── UserLockedEvent.cs
│   ├── Room/
│   │   ├── Room.cs                          # Root Entity
│   │   ├── Connection.cs                    # Entity
│   │   ├── Message.cs                       # Entity
│   │   └── Events/
│   │       ├── RoomCreatedEvent.cs
│   │       ├── UserJoinedRoomEvent.cs
│   │       ├── UserLeftRoomEvent.cs
│   │       ├── MessageSentEvent.cs
│   │       ├── ChatBlockedEvent.cs
│   │       ├── ChatUnblockedEvent.cs
│   │       └── RoomDeactivatedEvent.cs
│   └── Matching/
│       ├── Match/
│       │   ├── Match.cs                     # Root Entity
│       │   ├── MatchId.cs                   # Value Object
│       │   └── Events/
│       │       ├── MatchCreatedEvent.cs
│       │       └── MatchCompletedEvent.cs
│       └── StrangerMatching/
│           ├── StrangerMatching.cs          # Root Entity
│           └── Events/
│               └── StrangerMatchedEvent.cs
```

### Value Objects

```
TalkFlow.Domain/
├── ValueObjects/
│   ├── Common/
│   │   ├── EntityId.cs
│   │   ├── Email.cs
│   │   ├── PhoneNumber.cs
│   │   └── DateTimeRange.cs
│   ├── User/
│   │   ├── UserId.cs
│   │   ├── DisplayName.cs
│   │   ├── Gender.cs
│   │   ├── Age.cs
│   │   ├── Nationality.cs
│   │   └── PhotoUrl.cs
│   └── Room/
│       ├── RoomId.cs
│       ├── RoomName.cs
│       ├── SecurityCode.cs
│       └── RoomCapacity.cs
```

### Domain Services

```
TalkFlow.Domain/
├── Services/
│   ├── IUserDomainService.cs
│   ├── IRoomDomainService.cs
│   ├── IStrangerMatchingService.cs
│   ├── IWebRTCSignalingService.cs
│   └── MatchResult.cs
```

### Repositories (Interfaces)

```
TalkFlow.Domain/
├── Repositories/
│   ├── IUserRepository.cs
│   ├── IRoomRepository.cs
│   ├── IMessageRepository.cs
│   ├── IStrangerFilterRepository.cs
│   ├── IMatchRepository.cs
│   ├── IStrangerMatchingRepository.cs
│   └── IUnitOfWork.cs
```

### Domain Events

```
TalkFlow.Domain/
├── Events/
│   ├── IDomainEvent.cs
│   ├── DomainEventBase.cs
│   └── Handlers/
│       ├── IDomainEventHandler.cs
│       └── UserCreatedEventHandler.cs
```

### Specifications

```
TalkFlow.Domain/
├── Specifications/
│   ├── ISpecification.cs
│   ├── BaseSpecification.cs
│   └── User/
│       ├── UserByGenderSpecification.cs
│       ├── UserByAgeRangeSpecification.cs
│       └── UserByNationalitySpecification.cs
```

## Application Layer (`TalkFlow.Application`)

### Commands & Queries (CQRS)

```
TalkFlow.Application/
├── Commands/
│   ├── User/
│   │   ├── CreateUser/
│   │   │   ├── CreateUserCommand.cs
│   │   │   ├── CreateUserCommandHandler.cs
│   │   │   └── CreateUserCommandValidator.cs
│   │   └── UpdateUser/
│   │       ├── UpdateUserCommand.cs
│   │       ├── UpdateUserCommandHandler.cs
│   │       └── UpdateUserCommandValidator.cs
│   ├── Room/
│   │   ├── CreateRoom/
│   │   │   ├── CreateRoomCommand.cs
│   │   │   ├── CreateRoomCommandHandler.cs
│   │   │   └── CreateRoomCommandValidator.cs
│   │   ├── JoinRoom/
│   │   │   ├── JoinRoomCommand.cs
│   │   │   ├── JoinRoomCommandHandler.cs
│   │   │   └── JoinRoomCommandValidator.cs
│   │   └── UpdateRoom/
│   │       ├── UpdateRoomCommand.cs
│   │       ├── UpdateRoomCommandHandler.cs
│   │       └── UpdateRoomCommandValidator.cs
│   └── Message/
│       └── SendMessage/
│           ├── SendMessageCommand.cs
│           ├── SendMessageCommandHandler.cs
│           └── SendMessageCommandValidator.cs
├── Queries/
│   └── User/
│       └── GetUser/
│           ├── GetUserQuery.cs
│           ├── GetUserQueryHandler.cs
│           └── GetUserQueryValidator.cs
└── Common/
    ├── Interfaces/
    │   ├── ICommand.cs
    │   ├── ICommandHandler.cs
    │   ├── IQuery.cs
    │   ├── IQueryHandler.cs
    │   └── IRequestHandler.cs
    └── Behaviors/
        ├── ValidationBehavior.cs
        ├── LoggingBehavior.cs
        └── TransactionBehavior.cs
```

### DTOs

```
TalkFlow.Application/
├── DTOs/
│   ├── User/
│   │   ├── UserDto.cs
│   │   ├── CreateUserDto.cs
│   │   └── UpdateUserDto.cs
│   ├── Room/
│   │   ├── RoomDto.cs
│   │   ├── CreateRoomDto.cs
│   │   ├── JoinRoomDto.cs
│   │   └── UpdateRoomDto.cs
│   ├── Message/
│   │   ├── MessageDto.cs
│   │   └── SendMessageDto.cs
│   └── Common/
│       ├── PaginatedListDto.cs
│       └── ResponseDto.cs
```

### Services

```
TalkFlow.Application/
├── Services/
│   ├── IUserService.cs
│   ├── IRoomService.cs
│   ├── IMessageService.cs
│   ├── MessageService.cs
│   ├── RoomService.cs
│   └── UserService.cs
```

### Mappings

```
TalkFlow.Application/
├── Mappings/
│   ├── UserMappingProfile.cs
│   ├── RoomMappingProfile.cs
│   └── MessageMappingProfile.cs
```

## Infrastructure Layer (`TalkFlow.Infrastructure`)

### Data Access

```
TalkFlow.Infrastructure/
├── Data/
│   ├── ApplicationDbContext.cs
│   ├── Configurations/
│   │   ├── UserConfiguration.cs
│   │   ├── UserProfileConfiguration.cs
│   │   ├── RoomConfiguration.cs
│   │   ├── ConnectionConfiguration.cs
│   │   ├── MessageConfiguration.cs
│   │   ├── StrangerFilterConfiguration.cs
│   │   ├── MatchConfiguration.cs
│   │   └── StrangerMatchingConfiguration.cs
│   ├── Migrations/                         # EF Core migrations
│   └── Repositories/
│       ├── UserRepository.cs
│       ├── RoomRepository.cs
│       ├── MessageRepository.cs
│       ├── StrangerFilterRepository.cs
│       ├── MatchRepository.cs
│       ├── StrangerMatchingRepository.cs
│       └── UnitOfWork.cs
```

### External Services

```
TalkFlow.Infrastructure/
├── Services/
│   ├── JwtTokenService.cs
│   ├── UserDomainService.cs
│   ├── RoomDomainService.cs
│   └── StrangerMatchingService.cs
```

## Presentation Layer (`TalkFlow.Presentation`)

### Web API

```
TalkFlow.Presentation/
├── Controllers/
│   ├── UserController.cs
│   ├── RoomController.cs
│   └── MessageController.cs
├── Middleware/
│   └── (Middleware files)
├── Hubs/
│   ├── ChatHub.cs
│   ├── PresenceHub.cs
│   ├── StrangerHub.cs
│   └── TestHub.cs
├── Program.cs
├── appsettings.json
└── appsettings.Development.json
```

## Shared Layer (`TalkFlow.Shared`)

### Constants

```
TalkFlow.Shared/
├── Constants/
│   ├── ApiRoutes.cs
│   └── SignalREvents.cs
```

### Extensions

```
TalkFlow.Shared/
├── Extensions/
│   └── ServiceCollectionExtensions.cs
```

### Utilities

```
TalkFlow.Shared/
├── Utilities/
│   ├── DateTimeHelper.cs
│   └── StringHelper.cs
```

## Test Projects

### Unit Tests

```
TalkFlow.UnitTests/
├── Domain/
│   ├── Aggregates/
│   │   └── RoomAggregateTests.cs
│   ├── ValueObjects/
│   └── Services/
├── Application/
│   └── Services/
│       └── MessageServiceTests.cs
└── Infrastructure/
    ├── Repositories/
    └── Services/
```

### Integration Tests

```
TalkFlow.IntegrationTests/
├── Controllers/
└── RoomApiTests.cs
```

### Architecture Tests

```
TalkFlow.ArchitectureTests/
└── LayerRulesTests.cs
```

## Key DDD Patterns

### 1. Aggregates

- **User Aggregate**: User + UserProfile + StrangerFilter
- **Room Aggregate**: Room + Connection + Message
- **Match Aggregate**: Match + MatchingCriteria

### 2. Value Objects

- Immutable objects: UserId, RoomId, DisplayName, Gender, Age
- Validation logic encapsulated within value objects

### 3. Domain Events

- UserCreated, RoomCreated, MessageSent, UserJoinedRoom
- Event-driven architecture for loose coupling

### 4. Specifications

- Business rules encapsulated in specification classes
- Reusable query logic

### 5. Repository Pattern

- Abstract data access
- Unit of Work for transaction management

### 6. CQRS

- Separate read and write models
- Command/Query handlers with validation

## Technology Stack

### Core

- **.NET 8.0**
- **ASP.NET Core 8.0**
- **Entity Framework Core 8.0**
- **SignalR 8.0**

### Libraries

- **MediatR**: CQRS implementation
- **FluentValidation**: Command/Query validation
- **AutoMapper**: Object mapping
- **Serilog**: Structured logging (planned)

### Testing

- **xUnit**: Unit testing
- **FluentAssertions**: Assertions (planned)
- **Moq**: Mocking (planned)

### Infrastructure

- **SQL Server**: Database
- **JWT**: Authentication
- **SignalR**: Real-time communication

## Current Implementation Status

### Completed

- Domain Layer structure with Aggregates, Value Objects, Events
- Application Layer with CQRS pattern (partial)
- Infrastructure Layer with EF Core repositories
- Presentation Layer with API controllers and SignalR hubs
- Basic authentication with JWT

### In Progress

- Complete CQRS implementation
- Domain event handling
- Comprehensive validation
- Unit and integration tests

### Planned

- Architecture tests
- Logging and monitoring
- Caching layer
- CI/CD pipeline
- Docker containerization

## Project Structure Notes

1. **Missing components**: Some planned components are not yet implemented
2. **Domain Services**: Implemented in Infrastructure layer instead of Domain layer
3. **CQRS**: Partial implementation, not all commands/queries are complete
