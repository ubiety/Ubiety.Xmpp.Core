# WARP.md

This file provides guidance to WARP (warp.dev) when working with code in this repository.

## Project Overview

Ubiety XMPP Core is a .NET library for XMPP (Jabber) client and server communication. It implements the XMPP protocol using modern .NET patterns including async/await, state machines, and dependency injection. The library is designed to support .NET 9.0 and follows XMPP RFCs for messaging, authentication, and stream management.

## Common Development Commands

### Building
```powershell
# Standard build
dotnet build

# Build with specific configuration
dotnet build --configuration Release

# Using NUKE build system (recommended)
.\build.ps1
```

### Testing
```powershell
# Run all tests
dotnet test

# Run tests with coverage
dotnet test /p:CollectCoverage=true /p:CoverletOutput=artifacts/coverage /p:CoverletOutputFormat=opencover

# Run specific test project
dotnet test test/Ubiety.Xmpp.Test/Ubiety.Xmpp.Test.csproj

# Using NUKE
.\build.ps1 Test
```

### Package Management
```powershell
# Restore packages
dotnet restore

# Pack for NuGet
dotnet pack --configuration Release --output artifacts

# Using NUKE for complete build pipeline
.\build.ps1 Pack
```

## Architecture Overview

### Core Components

**State Machine Architecture**: The library uses a state-based architecture where XMPP connection lifecycle is managed through discrete states:
- `ConnectingState`: Initial connection establishment
- `ConnectedState`: Socket connected, initiating XMPP stream
- `StreamFeaturesState`: Processing server capabilities
- `StartTlsState`: TLS/SSL negotiation
- `SaslState`: Authentication processing
- `BindingState`: Resource binding
- `DisconnectedState`: Clean disconnection

**Key Base Classes**:
- `XmppBase`: Abstract base class providing core XMPP functionality, connection management, and state coordination
- `IClient`: Interface defining client operations for JID management, authentication, and connection control

**Registries**:
- `TagRegistry`: Manages XMPP XML element creation and parsing
- `SaslRegistry`: Handles SASL authentication mechanism registration and selection

**Network Layer**:
- `AsyncClientSocket`: Manages TCP connections with async I/O operations
- `Address`: DNS resolution and server discovery
- `Parser`: XML stream parsing for XMPP stanzas

**SASL Authentication**:
- `SaslProcessor`: Base class for authentication mechanisms
- `PlainProcessor`, `Md5Processor`, `ScramProcessor`: Specific auth implementations
- Supports PLAIN, DIGEST-MD5, and SCRAM authentication methods

### Project Structure
- `src/Ubiety.Xmpp.Core/`: Main library implementation
- `src/Ubiety.Xmpp.App/`: Sample application demonstrating usage
- `test/Ubiety.Xmpp.Test/`: XUnit test suite with FluentAssertions
- `build/`: NUKE build system implementation

### Dependencies
- **Ubiety.Dns.Core**: DNS resolution for XMPP server discovery
- **Ubiety.Scram.Core**: SCRAM authentication implementation
- **Ubiety.Stringprep.Core**: String preparation for XMPP identifiers
- **StyleCop.Analyzers**: Code style enforcement
- **XUnit + FluentAssertions**: Testing framework

## Development Guidelines

### Code Style
The project enforces StyleCop rules and uses .editorconfig for consistent formatting. All code should:
- Follow standard C# naming conventions
- Include comprehensive XML documentation
- Use modern C# patterns (records, pattern matching, nullable reference types)
- Implement proper async/await patterns for network operations

### State Machine Extension
When adding new XMPP features:
1. Create new state classes implementing `IState`
2. Add state transitions in appropriate existing states
3. Update `XmppBase.Parser_Tag` if new error conditions need handling
4. Register new XML elements in `TagRegistry` if required

### Testing
- Use XUnit for unit tests with FluentAssertions for readable assertions
- Mock network dependencies using interfaces (`ISocket`, `IState`)
- Test state transitions independently
- Verify XMPP protocol compliance with RFC examples

### SASL Extension
To add new authentication mechanisms:
1. Implement `SaslProcessor` abstract class
2. Register in `SaslRegistry` during initialization
3. Add mechanism to `MechanismTypes` enumeration
4. Include appropriate unit tests for the mechanism

## NUKE Build System

The project uses NUKE for build automation. Key targets:
- `Clean`: Remove build artifacts
- `Restore`: Restore NuGet packages
- `Compile`: Build solution
- `Test`: Run tests with optional coverage
- `Pack`: Create NuGet packages
- `Publish`: Push to NuGet (requires API key)
- `SonarBegin`/`SonarEnd`: SonarCloud analysis integration

Build parameters:
- `--configuration`: Debug/Release
- `--cover`: Enable/disable code coverage
- `--nuget-key`: NuGet API key for publishing
- `--sonar-key`: SonarCloud authentication
