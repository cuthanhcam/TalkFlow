<div align="center">

# 🎥 TalkFlow

**Real-time Video Chat Application with WebRTC and SignalR**

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-8.0-512BD4?logo=dotnet)](https://asp.net/)
[![SignalR](https://img.shields.io/badge/SignalR-8.0-00ADD8?logo=signalr)](https://dotnet.microsoft.com/apps/aspnet/signalr)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

[Features](#-features) • [Tech Stack](#-tech-stack) • [Getting Started](#-getting-started) • [Documentation](#-documentation) • [Contributing](#-contributing)

</div>

---

## 📖 Overview

**TalkFlow** is a modern, real-time video chat application that enables seamless communication through video, audio, and text. Built with .NET 8.0 and WebRTC, it offers two distinct modes of interaction:

- 🤝 **Friend Room Mode**: Create private rooms and invite friends with room codes
- 🌐 **Stranger Mode**: Match randomly with people based on preferences (gender, age, location)

The application leverages **WebRTC** for peer-to-peer video/audio streaming and **SignalR** for real-time messaging and signaling, providing a smooth, low-latency communication experience.

---

## ✨ Features

### 🎯 Core Features

- **📹 Real-time Video/Audio Chat**: High-quality P2P video and audio communication
- **💬 Instant Messaging**: Real-time text chat with message history
- **🔒 Secure Rooms**: Password-protected private rooms
- **🎲 Random Matching**: Intelligent stranger matching algorithm with customizable filters
- **🖥️ Screen Sharing**: Share your screen with other participants
- **🎤 Media Controls**: Mute/unmute microphone and camera
- **👥 Multiple Participants**: Support for multi-user video conferences
- **📱 Responsive UI**: Modern, mobile-friendly interface with Bootstrap 5

### 🛠️ Advanced Features

- **JWT Authentication**: Secure authentication with JSON Web Tokens
- **Online Presence**: Real-time user presence tracking
- **Auto-matching**: Automatic pairing based on user preferences
- **Room Management**: Host controls for room settings and security
- **Connection Recovery**: Automatic reconnection on network failures

---

## 🏗️ Architecture

The application follows a **layered architecture** with clear separation of concerns:

```
┌─────────────────────────────────────────────────────┐
│              Frontend (TalkFlow.Web)                │
│    ASP.NET Core MVC + Razor + JavaScript + WebRTC   │
└───────────────────┬─────────────────────────────────┘
                    │ HTTP / WebSocket
┌───────────────────▼─────────────────────────────────┐
│              Backend API (TalkFlow)                 │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────┐   │
│  │ Controllers  │  │ SignalR Hubs │  │  Services│   │
│  └──────┬───────┘  └──────┬───────┘  └─────┬────┘   │
│         │                  │                │       │
│  ┌──────▼──────────────────▼────────────────▼────┐  │
│  │           Business Logic Layer                │  │
│  │  Repositories • DTOs • AutoMapper             │  │
│  └──────────────────┬────────────────────────────┘  │
│                     │                               │
│  ┌──────────────────▼────────────────────────────┐  │
│  │      Entity Framework Core + Identity         │  │
│  └──────────────────┬────────────────────────────┘  │
└─────────────────────┼───────────────────────────────┘
                      │
┌─────────────────────▼───────────────────────────────┐
│           SQL Server Database                       │
└─────────────────────────────────────────────────────┘
```

### Project Structure

```
TalkFlow/
├── TalkFlow/               # Backend API
│   ├── Controllers/        # API Controllers
│   ├── SignalR/           # SignalR Hubs (ChatHub, PresenceHub, StrangerHub)
│   ├── Services/          # Business logic services
│   ├── Repository/        # Data access layer
│   ├── Entities/          # Database entities
│   ├── Dtos/              # Data transfer objects
│   ├── Data/              # DbContext and migrations
│   └── Extensions/        # Service configuration extensions
│
├── TalkFlow.Web/          # Frontend Web Application
│   ├── Controllers/       # MVC Controllers
│   ├── Views/            # Razor views
│   ├── Models/           # View models
│   └── wwwroot/          # Static files (JS, CSS, images)
│
└── docs/                 # Documentation
```

---

## 🚀 Tech Stack

### Backend

| Technology            | Version | Purpose                        |
| --------------------- | ------- | ------------------------------ |
| .NET                  | 8.0     | Runtime framework              |
| ASP.NET Core          | 8.0     | Web framework                  |
| Entity Framework Core | 8.0.10  | ORM for database access        |
| SignalR               | 8.0     | Real-time communication        |
| SQL Server            | 2022    | Database                       |
| ASP.NET Core Identity | 8.0     | Authentication & authorization |
| JWT Bearer            | 8.0.10  | Token-based authentication     |
| AutoMapper            | 12.0.1  | Object-object mapping          |
| Swagger/OpenAPI       | 9.0.6   | API documentation              |

### Frontend

| Technology     | Version | Purpose                   |
| -------------- | ------- | ------------------------- |
| Razor Pages    | -       | Server-side rendering     |
| JavaScript     | ES6+    | Client-side logic         |
| WebRTC API     | -       | P2P video/audio streaming |
| Bootstrap      | 5.3     | UI framework              |
| jQuery         | 3.7.1   | DOM manipulation          |
| Font Awesome   | 6.x     | Icons                     |
| SignalR Client | -       | Real-time messaging       |

---

## 🚀 Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later
- [SQL Server](https://www.microsoft.com/sql-server) (LocalDB or Express)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)
- [Node.js](https://nodejs.org/) (for frontend dependencies, optional)

### Installation

1. **Clone the repository**

   ```bash
   git clone https://github.com/cuthanhcam/TalkFlow.git
   cd TalkFlow
   ```

2. **Configure database connection**

   Update the connection string in `TalkFlow/appsettings.json`:

   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=TalkFlowDb;Trusted_Connection=true;MultipleActiveResultSets=true"
     }
   }
   ```

3. **Run database migrations**

   ```bash
   cd TalkFlow
   dotnet ef database update
   ```

4. **Run the Backend API**

   ```bash
   dotnet run
   ```

   API will be available at `https://localhost:7198` (or the port shown in console)

5. **Run the Frontend Web App** (in a new terminal)

   ```bash
   cd TalkFlow.Web
   dotnet run
   ```

   Web app will be available at `https://localhost:5001` (or the port shown in console)

6. **Access the application**

   Open your browser and navigate to the web app URL (e.g., `https://localhost:5001`)

### Quick Start

#### Friend Room Mode

1. Enter your display name (6-20 characters)
2. Click **"Create New Room"** to create a room or **"Join Existing Room"** to join
3. Share the room URL with friends
4. Start video chatting!

#### Stranger Mode

1. Navigate to **"Meet Stranger"**
2. Fill in your information (name, gender, age, country)
3. Set your preferences for matching
4. Click **"Start Matching"** and wait for a match
5. Accept the match and start chatting!

---

## 📚 Documentation

### API Documentation

When running in development mode, access Swagger UI at: `https://localhost:7198/swagger`

### SignalR Hubs

#### ChatHub (`/hubs/chathub`)

```javascript
// Join a room
connection.invoke("JoinRoom", roomId, userId, displayName);

// Send message
connection.invoke("SendMessage", roomId, userId, displayName, message);

// Media controls
connection.invoke("MuteMicroPhone", roomId, userId, isMuted);
connection.invoke("MuteCamera", roomId, userId, isMuted);
connection.invoke("ShareScreen", roomId, userId, isSharing);

// Leave room
connection.invoke("LeaveRoom", roomId, userId);
```

#### StrangerHub (`/hubs/stranger`)

Automatically matches users upon connection based on their filter preferences.

#### PresenceHub (`/hubs/presence`)

Tracks online/offline status of users.

### Detailed Documentation

For comprehensive documentation including:

- Complete use cases and user flows
- WebRTC implementation details
- Database schema
- Matching algorithm
- Security considerations

Please refer to: **[TALKFLOW_PROJECT_DOCUMENTATION.md](./TALKFLOW_PROJECT_DOCUMENTATION.md)**

---

## 🔒 Security

- **JWT Authentication**: All API endpoints (except registration) require JWT tokens
- **Password Protection**: Rooms can be secured with passwords
- **HTTPS**: Enforced in production
- **Input Validation**: Server-side validation for all user inputs
- **CORS**: Configurable CORS policies

---

## 🛠️ Development

### Project Configuration

The solution uses `.NET 8.0` as specified in `global.json`:

```json
{
  "sdk": {
    "rollForward": "latestFeature",
    "version": "8.0.410"
  }
}
```

### Code Conventions

Follow the project's coding guidelines:

- Commit messages: See [COMMIT_CONVENTION.md](./COMMIT_CONVENTION.md)
- Git workflow: See [GIT_GUIDE.md](./GIT_GUIDE.md)

### Adding New Features

1. Create a new feature branch: `git checkout -b feature/your-feature-name`
2. Implement your changes following the existing architecture
3. Test thoroughly (API, UI, real-time features)
4. Submit a Pull Request with clear description

---

## 🚀 Deployment

### Production Checklist

- [ ] Set strong JWT secret key in configuration
- [ ] Configure production database connection string
- [ ] Set up CORS for specific production domains
- [ ] Configure STUN/TURN servers for WebRTC
- [ ] Enable HTTPS with valid SSL certificate
- [ ] Set up logging and monitoring
- [ ] Configure rate limiting for API endpoints
- [ ] Set up database backups
- [ ] Review and test security settings

### Environment Variables

Key configuration settings in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "YOUR_CONNECTION_STRING"
  },
  "TokenKey": "YOUR_STRONG_SECRET_KEY",
  "AllowedHosts": "*"
}
```

---

## 🤝 Contributing

We welcome contributions! Please follow these steps:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes following [COMMIT_CONVENTION.md](./COMMIT_CONVENTION.md)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

Please ensure:

- Code follows existing style and conventions
- All tests pass
- Documentation is updated if needed
- Commit messages are clear and descriptive

---

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

## 👥 Authors

- **CuThanhCam** - [GitHub](https://github.com/cuthanhcam)

---

## 🙏 Acknowledgments

- [WebRTC](https://webrtc.org/) - Real-time communication
- [SignalR](https://dotnet.microsoft.com/apps/aspnet/signalr) - Real-time web functionality
- [Bootstrap](https://getbootstrap.com/) - UI components
- [Font Awesome](https://fontawesome.com/) - Icons

---

## 📞 Support

If you have any questions or issues:

- 📧 Create an issue in this repository
- 📖 Check the [detailed documentation](./TALKFLOW_PROJECT_DOCUMENTATION.md)
- 💬 Review [commit conventions](./COMMIT_CONVENTION.md) and [Git guide](./GIT_GUIDE.md)

---

<div align="center">

**Made with ❤️ using .NET 8.0 and WebRTC**

</div>
