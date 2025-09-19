# DDD Project Structure

## Tổng quan
Cấu trúc dự án theo Domain-Driven Design (DDD) với Clean Architecture, tách biệt rõ ràng các layer và responsibility.

## Solution Structure
```
TalkFlow.sln
├── src/
│   ├── TalkFlow.Domain/                    # Domain Layer
│   ├── TalkFlow.Application/               # Application Layer
│   ├── TalkFlow.Infrastructure/            # Infrastructure Layer
│   ├── TalkFlow.Presentation/              # Presentation Layer
│   └── TalkFlow.Shared/                    # Shared Layer
├── tests/
│   ├── TalkFlow.UnitTests/                 # Unit Tests
│   ├── TalkFlow.IntegrationTests/          # Integration Tests
│   └── TalkFlow.ArchitectureTests/         # Architecture Tests
├── docs/                                   # Documentation
├── scripts/                                # Build/Deploy Scripts
└── docker/                                 # Docker Configuration
```

## Domain Layer (`TalkFlow.Domain`)

### Aggregates
```
TalkFlow.Domain/
├── Aggregates/
│   ├── User/
│   │   ├── User.cs                          # Root Entity
│   │   ├── UserId.cs                        # Value Object
│   │   ├── DisplayName.cs                   # Value Object
│   │   ├── UserProfile.cs                   # Entity
│   │   ├── StrangerFilter.cs                # Entity
│   │   └── Events/
│   │       ├── UserCreatedEvent.cs
│   │       ├── UserProfileUpdatedEvent.cs
│   │       └── UserLockedEvent.cs
│   ├── Room/
│   │   ├── Room.cs                          # Root Entity
│   │   ├── RoomId.cs                        # Value Object
│   │   ├── RoomName.cs                      # Value Object
│   │   ├── SecurityCode.cs                  # Value Object
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
│   │   ├── Email.cs
│   │   ├── PhoneNumber.cs
│   │   └── DateTimeRange.cs
│   ├── User/
│   │   ├── Gender.cs
│   │   ├── Age.cs
│   │   ├── Nationality.cs
│   │   └── PhotoUrl.cs
│   └── Room/
│       ├── RoomCapacity.cs
│       └── RoomSettings.cs
```

### Domain Services
```
TalkFlow.Domain/
├── Services/
│   ├── IUserDomainService.cs
│   ├── IRoomDomainService.cs
│   ├── IStrangerMatchingService.cs
│   └── IWebRTCSignalingService.cs
```

### Repositories (Interfaces)
```
TalkFlow.Domain/
├── Repositories/
│   ├── IUserRepository.cs
│   ├── IRoomRepository.cs
│   ├── IMessageRepository.cs
│   ├── IStrangerFilterRepository.cs
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
│   ├── User/
│   │   ├── UserByGenderSpecification.cs
│   │   ├── UserByAgeRangeSpecification.cs
│   │   └── UserByNationalitySpecification.cs
│   └── Room/
│       ├── RoomBySecurityCodeSpecification.cs
│       └── RoomByCapacitySpecification.cs
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
│   │   ├── UpdateUserProfile/
│   │   │   ├── UpdateUserProfileCommand.cs
│   │   │   ├── UpdateUserProfileCommandHandler.cs
│   │   │   └── UpdateUserProfileCommandValidator.cs
│   │   └── LockUser/
│   │       ├── LockUserCommand.cs
│   │       ├── LockUserCommandHandler.cs
│   │       └── LockUserCommandValidator.cs
│   ├── Room/
│   │   ├── CreateRoom/
│   │   ├── JoinRoom/
│   │   ├── LeaveRoom/
│   │   ├── UpdateRoom/
│   │   └── DeleteRoom/
│   ├── Message/
│   │   ├── SendMessage/
│   │   └── DeleteMessage/
│   └── Matching/
│       ├── StartStrangerMatching/
│       └── CompleteStrangerMatching/
├── Queries/
│   ├── User/
│   │   ├── GetUserById/
│   │   ├── GetUsers/
│   │   └── GetUserProfile/
│   ├── Room/
│   │   ├── GetRoomById/
│   │   ├── GetRooms/
│   │   └── GetRoomMembers/
│   └── Message/
│       ├── GetMessages/
│       └── GetMessageHistory/
└── Common/
    ├── Interfaces/
    │   ├── ICommand.cs
    │   ├── ICommandHandler.cs
    │   ├── IQuery.cs
    │   ├── IQueryHandler.cs
    │   └── IRequestHandler.cs
    ├── Behaviors/
    │   ├── ValidationBehavior.cs
    │   ├── LoggingBehavior.cs
    │   └── TransactionBehavior.cs
    └── Models/
        ├── Result.cs
        ├── PaginatedResult.cs
        └── Error.cs
```

### DTOs
```
TalkFlow.Application/
├── DTOs/
│   ├── User/
│   │   ├── UserDto.cs
│   │   ├── CreateUserDto.cs
│   │   ├── UpdateUserDto.cs
│   │   └── UserProfileDto.cs
│   ├── Room/
│   │   ├── RoomDto.cs
│   │   ├── CreateRoomDto.cs
│   │   ├── JoinRoomDto.cs
│   │   └── RoomMemberDto.cs
│   ├── Message/
│   │   ├── MessageDto.cs
│   │   ├── SendMessageDto.cs
│   │   └── MessageHistoryDto.cs
│   └── Common/
│       ├── PaginationDto.cs
│       └── FilterDto.cs
```

### Services
```
TalkFlow.Application/
├── Services/
│   ├── IUserService.cs
│   ├── IRoomService.cs
│   ├── IMessageService.cs
│   ├── IStrangerMatchingService.cs
│   ├── IWebRTCService.cs
│   └── IFileStorageService.cs
```

### Mappings
```
TalkFlow.Application/
├── Mappings/
│   ├── UserMappingProfile.cs
│   ├── RoomMappingProfile.cs
│   ├── MessageMappingProfile.cs
│   └── CommonMappingProfile.cs
```

### SignalR Hubs
```
TalkFlow.Application/
├── Hubs/
│   ├── IChatHub.cs
│   ├── IPresenceHub.cs
│   ├── IStrangerHub.cs
│   └── HubServices/
│       ├── ChatHubService.cs
│       ├── PresenceHubService.cs
│       └── StrangerHubService.cs
```

## Infrastructure Layer (`TalkFlow.Infrastructure`)

### Data Access
```
TalkFlow.Infrastructure/
├── Data/
│   ├── ApplicationDbContext.cs
│   ├── Configurations/
│   │   ├── UserConfiguration.cs
│   │   ├── RoomConfiguration.cs
│   │   ├── MessageConfiguration.cs
│   │   └── StrangerFilterConfiguration.cs
│   ├── Migrations/
│   └── Repositories/
│       ├── UserRepository.cs
│       ├── RoomRepository.cs
│       ├── MessageRepository.cs
│       ├── StrangerFilterRepository.cs
│       └── UnitOfWork.cs
```

### External Services
```
TalkFlow.Infrastructure/
├── Services/
│   ├── JwtTokenService.cs
│   ├── FileStorageService.cs
│   ├── EmailService.cs
│   ├── WebRTCSignalingService.cs
│   └── CacheService.cs
```

### Configuration
```
TalkFlow.Infrastructure/
├── Configuration/
│   ├── DatabaseConfiguration.cs
│   ├── JwtConfiguration.cs
│   ├── CorsConfiguration.cs
│   └── SignalRConfiguration.cs
```

### Identity
```
TalkFlow.Infrastructure/
├── Identity/
│   ├── ApplicationUser.cs
│   ├── ApplicationRole.cs
│   ├── IdentityDbContext.cs
│   └── IdentityServiceExtensions.cs
```

## Presentation Layer (`TalkFlow.Presentation`)

### Web API
```
TalkFlow.Presentation/
├── Controllers/
│   ├── UserController.cs
│   ├── RoomController.cs
│   ├── MessageController.cs
│   ├── StrangerController.cs
│   └── FileController.cs
├── Middleware/
│   ├── ExceptionMiddleware.cs
│   ├── LoggingMiddleware.cs
│   └── ValidationMiddleware.cs
├── Filters/
│   ├── ValidationFilter.cs
│   └── AuthorizationFilter.cs
└── Hubs/
    ├── ChatHub.cs
    ├── PresenceHub.cs
    └── StrangerHub.cs
```

### Web MVC (Optional)
```
TalkFlow.Presentation/
├── Views/
├── Controllers/
│   └── HomeController.cs
└── wwwroot/
```

## Shared Layer (`TalkFlow.Shared`)

### Constants
```
TalkFlow.Shared/
├── Constants/
│   ├── ApiRoutes.cs
│   ├── SignalREvents.cs
│   ├── WebRTCEvents.cs
│   └── ErrorCodes.cs
```

### Extensions
```
TalkFlow.Shared/
├── Extensions/
│   ├── ServiceCollectionExtensions.cs
│   ├── ApplicationBuilderExtensions.cs
│   └── StringExtensions.cs
```

### Utilities
```
TalkFlow.Shared/
├── Utilities/
│   ├── DateTimeHelper.cs
│   ├── StringHelper.cs
│   └── ValidationHelper.cs
```

## Test Projects

### Unit Tests
```
TalkFlow.UnitTests/
├── Domain/
│   ├── Aggregates/
│   ├── ValueObjects/
│   └── Services/
├── Application/
│   ├── Commands/
│   ├── Queries/
│   └── Services/
└── Infrastructure/
    ├── Repositories/
    └── Services/
```

### Integration Tests
```
TalkFlow.IntegrationTests/
├── Controllers/
├── Hubs/
├── Repositories/
└── Services/
```

### Architecture Tests
```
TalkFlow.ArchitectureTests/
├── DependencyTests.cs
├── NamingConventionTests.cs
└── LayerTests.cs
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
- **Serilog**: Structured logging
- **Polly**: Resilience patterns

### Testing
- **xUnit**: Unit testing
- **FluentAssertions**: Assertions
- **Moq**: Mocking
- **Testcontainers**: Integration testing

### Infrastructure
- **SQL Server**: Database
- **Redis**: Caching (optional)
- **Docker**: Containerization
- **GitHub Actions**: CI/CD

## Migration Strategy

### Phase 1: Domain Layer
1. Extract entities from current project
2. Create value objects and domain services
3. Implement domain events
4. Create specifications

### Phase 2: Application Layer
1. Implement CQRS with MediatR
2. Create command/query handlers
3. Implement application services
4. Add validation and mapping

### Phase 3: Infrastructure Layer
1. Implement repositories with EF Core
2. Add external services
3. Configure database and migrations
4. Implement caching and logging

### Phase 4: Presentation Layer
1. Create Web API controllers
2. Implement SignalR hubs
3. Add middleware and filters
4. Configure authentication/authorization

### Phase 5: Testing & Deployment
1. Add unit and integration tests
2. Implement architecture tests
3. Configure CI/CD pipeline
4. Deploy to production

## Benefits of DDD Structure

1. **Separation of Concerns**: Clear boundaries between layers
2. **Testability**: Easy to unit test each layer independently
3. **Maintainability**: Changes in one layer don't affect others
4. **Scalability**: Can scale different layers independently
5. **Domain Focus**: Business logic is clearly expressed
6. **Flexibility**: Easy to change infrastructure without affecting domain
