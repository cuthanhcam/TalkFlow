# Changelog

All notable changes to TalkFlow project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [2.0.0] - 2025-10-11

### Added
- ✨ **Friend Room Mode**: Create private video chat rooms with room codes
- ✨ **Stranger Mode**: Random matching with people based on preferences
- 🎥 **WebRTC Integration**: P2P video/audio streaming
- 💬 **Real-time Chat**: Text messaging via SignalR
- 🖥️ **Screen Sharing**: Share screen with participants
- 🎤 **Media Controls**: Mute/unmute microphone and camera
- 🔒 **Room Security**: Password-protected rooms
- 📱 **Modern UI**: Responsive design with Bootstrap 5
- 🌐 **SignalR Hubs**: ChatHub, PresenceHub, StrangerHub
- 🔐 **JWT Authentication**: Secure token-based authentication
- 📊 **Swagger API Documentation**: Interactive API docs
- 🎯 **Stranger Matching Algorithm**: Filter by gender, age, location

### Changed
- 🔄 **Architecture**: Simplified from DDD to layered architecture (TalkFlow + TalkFlow.Web)
- 📝 **Documentation**: Complete rewrite of README.md with badges and modern format
- 🗂️ **Project Structure**: Streamlined to 2 main projects
- 💅 **UI/UX**: Complete redesign with modern glassmorphism effects

### Fixed
- 🐛 Various bug fixes in room management
- 🔧 Improved connection stability
- ⚡ Performance optimizations

### Removed
- 🗑️ Outdated DDD architecture documentation
- 🗑️ Obsolete API specifications
- 🗑️ Domain events documentation (not implemented)
- 🗑️ Cache troubleshooting guide

## [1.0.0] - 2025-09-19

### Added
- 🎉 Initial project setup
- 🔐 JWT authentication
- 📡 Basic API endpoints
- 👥 User role management
- 📊 Database schema with EF Core

### Known Issues
- Code needs refactoring
- Missing unit tests
- Logging not implemented
- Some features unstable

---

## [Unreleased]

### Planned
- 🧪 Comprehensive test suite (unit + integration tests)
- 📝 Structured logging with Serilog
- 🐳 Docker containerization
- 🚀 CI/CD pipeline with GitHub Actions
- 📊 Analytics and monitoring integration
- 🔄 Connection recovery improvements
- 🎨 Additional UI themes
- 🌍 Multi-language support
- 📱 Mobile app (React Native/Flutter)
- 🎮 Game room features
