# Recchx - Cold Email Outreach Platform

A comprehensive cold email outreach platform built with .NET 8, designed to help businesses manage prospects, create campaigns, and track email engagement with enterprise-grade security features.

## Table of Contents

- [Overview](#overview)
- [Architecture](#architecture)
- [Features](#features)
- [Technology Stack](#technology-stack)
- [Getting Started](#getting-started)
- [Project Structure](#project-structure)
- [Security Features](#security-features)
- [API Documentation](#api-documentation)
- [Background Jobs](#background-jobs)
- [Database](#database)
- [Configuration](#configuration)
- [Development](#development)
- [License](#license)

## Overview

Recchx is a modular monolith application following Clean Architecture principles, designed to streamline cold email outreach campaigns. The platform provides robust user management, OAuth-based mailbox integration, prospect management, campaign automation, and comprehensive analytics.

## Architecture

The application follows Clean Architecture with a Modular Monolith approach, organized into the following modules:

- **Users Module**: Authentication, authorization, and user profile management
- **Mailbox Module**: OAuth integration with Gmail and Outlook
- **Prospects Module**: Contact and company management
- **Campaigns Module**: Email campaign creation and management
- **Applications Module**: Applicant tracking for recruitment campaigns
- **Shared Kernel**: Common domain entities, value objects, and utilities

### Design Patterns

- CQRS (Command Query Responsibility Segregation) with MediatR
- Repository Pattern for data access
- Result Pattern for error handling
- Dependency Injection
- Middleware Pipeline for cross-cutting concerns

## Features

### Phase 1: Authentication & Profile Management (Completed)

- User registration with email validation
- Secure login with JWT tokens
- Password hashing using BCrypt
- Profile management (view and update)
- Account deletion
- Token-based authentication

### Enhanced Security Features (Completed)

1. **Token Blacklisting**: Revoked tokens are stored in the database and validated on each request
2. **Single Session Per User**: New logins automatically invalidate all previous sessions
3. **Refresh Token Pattern**: 
   - Short-lived access tokens (1 hour)
   - Long-lived refresh tokens (7 days)
   - Automatic token rotation on refresh
4. **Device Fingerprinting**: SHA256 hash of IP address and User-Agent for device tracking
5. **Enhanced Token Claims**: JWT includes session ID, device fingerprint, and standard claims

### Background Job Processing

- Automatic cleanup of expired tokens (30+ days old)
- Automatic cleanup of revoked sessions (7+ days old)
- Daily scheduled maintenance at 2:00 AM
- Hangfire dashboard for job monitoring

## Technology Stack

### Backend

- **.NET 8**: Latest LTS version of .NET
- **ASP.NET Core Web API**: RESTful API framework
- **Entity Framework Core**: ORM for database operations
- **PostgreSQL**: Primary database (AWS RDS)
- **MediatR**: CQRS implementation
- **FluentValidation**: Input validation
- **BCrypt.Net**: Password hashing
- **JWT Bearer**: Token-based authentication
- **Hangfire**: Background job processing
- **Serilog**: Structured logging

### Development Tools

- **Swagger/OpenAPI**: API documentation and testing
- **Rider/Visual Studio**: IDE support

## Getting Started

### Prerequisites

- .NET 8 SDK or later
- PostgreSQL database
- Visual Studio 2022 / JetBrains Rider / VS Code

### Installation

1. Clone the repository:
```bash
git clone https://github.com/Gbohunmifrancis/Recchx.git
cd Recchx
```

2. Configure the database connection:
   - Create `appsettings.json` in the `Recchx.WebAPI` project
   - Add your PostgreSQL connection string:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=your-host;Database=your-db;Username=your-user;Password=your-password;SSL Mode=Require;"
  },
  "Jwt": {
    "SecretKey": "your-secret-key-minimum-32-characters",
    "Issuer": "Recchx",
    "Audience": "RecchxUsers",
    "ExpiryMinutes": 60
  }
}
```

3. Apply database migrations:
```bash
cd Recchx.WebAPI
dotnet ef database update
```

4. Run the application:
```bash
dotnet run
```

5. Access the API:
   - API: `http://localhost:5259`
   - Swagger UI: `http://localhost:5259/swagger`
   - Hangfire Dashboard: `http://localhost:5259/hangfire`

## Project Structure

```
Recchx/
├── Recchx.WebAPI/              # API entry point and controllers
│   ├── Controllers/            # API endpoints
│   ├── Middleware/             # Custom middleware
│   ├── Infrastructure/         # Infrastructure configuration
│   └── Program.cs              # Application startup
├── Recchx.Users/               # Users module
│   ├── Application/            # Commands, queries, and DTOs
│   ├── Domain/                 # Domain entities and interfaces
│   └── Infrastructure/         # Repositories and services
├── Recchx.Mailbox/             # Mailbox OAuth module
│   └── Domain/                 # Mailbox entities
├── Recchx.Prospects/           # Prospects management module
│   └── Domain/                 # Contact and company entities
├── Recchx.Campaigns/           # Campaign management module
│   └── Domain/                 # Campaign entities
├── Recchx.Applications/        # Applicant tracking module
│   └── Domain/                 # Application entities
└── Recchx.SharedKernel/        # Shared domain logic
    ├── Entities/               # Base entity classes
    ├── ValueObjects/           # Value objects
    ├── Results/                # Result pattern implementation
    ├── Exceptions/             # Custom exceptions
    └── Events/                 # Domain events
```

## Security Features

### Authentication Flow

1. **Registration**:
   - User submits email, name, and password
   - Server extracts IP address and User-Agent from HTTP context
   - Password is hashed using BCrypt
   - Device fingerprint is generated
   - All previous sessions are revoked (single session mode)
   - User record, access token, refresh token, and session are created
   - Returns JWT token, refresh token, and session ID

2. **Login**:
   - User submits email and password
   - Server validates credentials
   - Server extracts device information
   - All previous sessions are revoked
   - New tokens and session are created
   - Returns JWT token, refresh token, and session ID

3. **Token Refresh**:
   - Client submits refresh token
   - Server validates refresh token and device fingerprint
   - Old tokens are revoked
   - New tokens and session are created
   - Returns new JWT token and refresh token

4. **Request Validation**:
   - Every authenticated request passes through TokenValidationMiddleware
   - Middleware extracts JWT ID from token claims
   - Validates if session exists and is active in database
   - Updates last activity timestamp
   - Returns 401 Unauthorized if session is invalid

### Token Management

- **Access Tokens**: Valid for 1 hour, contains user claims, session ID, and device fingerprint
- **Refresh Tokens**: Valid for 7 days, cryptographically secure random tokens
- **Session Tracking**: Each login creates a unique session with device information
- **Automatic Cleanup**: Background job removes old expired/revoked tokens and sessions

## API Documentation

### Authentication Endpoints

#### Register User
```http
POST /api/auth/signup
Content-Type: application/json

{
  "email": "user@example.com",
  "firstName": "John",
  "lastName": "Doe",
  "password": "SecurePassword123!"
}
```

#### Login
```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "SecurePassword123!"
}
```

#### Refresh Token
```http
POST /api/auth/refresh
Content-Type: application/json

{
  "refreshToken": "your-refresh-token"
}
```

#### Logout (Current Session)
```http
POST /api/auth/logout
Authorization: Bearer {access-token}
```

#### Logout (All Sessions)
```http
POST /api/auth/logout-all
Authorization: Bearer {access-token}
```

### User Profile Endpoints

#### Get User Profile
```http
GET /api/users/profile
Authorization: Bearer {access-token}
```

#### Update User Profile
```http
PUT /api/users/profile
Authorization: Bearer {access-token}
Content-Type: application/json

{
  "firstName": "John",
  "lastName": "Doe",
  "email": "newemail@example.com"
}
```

#### Delete Account
```http
DELETE /api/users/profile
Authorization: Bearer {access-token}
```

## Background Jobs

The application uses Hangfire for background job processing with PostgreSQL storage.

### Configured Jobs

1. **Token Cleanup Job**:
   - Schedule: Daily at 2:00 AM
   - Purpose: Remove old expired and revoked tokens
   - Retention: 30 days for tokens, 7 days for sessions
   - Automatic retry: 3 attempts on failure

### Monitoring

Access the Hangfire dashboard at `/hangfire` (development only) to:
- View scheduled jobs
- Monitor job execution history
- Manually trigger jobs
- View job failures and retries

## Database

### Schema Overview

- **Users**: User accounts with authentication data
- **RefreshTokens**: Refresh tokens with device information
- **UserSessions**: Active JWT sessions for validation
- **MailboxConnections**: OAuth tokens for email providers
- **Contacts**: Prospect contact information
- **Companies**: Company/organization data
- **Campaigns**: Email campaign configurations
- **EmailTemplates**: Reusable email templates
- **Applications**: Job applicant tracking

### Migrations

Create a new migration:
```bash
dotnet ef migrations add MigrationName --project Recchx.Users --startup-project Recchx.WebAPI
```

Apply migrations:
```bash
dotnet ef database update --project Recchx.WebAPI
```

Remove last migration:
```bash
dotnet ef migrations remove --project Recchx.Users --startup-project Recchx.WebAPI
```

## Configuration

### Required Settings

1. **Database Connection**: PostgreSQL connection string
2. **JWT Configuration**: Secret key, issuer, audience, expiry
3. **Logging**: Serilog configuration for file and console output
4. **Hangfire**: Background job processing configuration

### Environment Variables

For production deployment, use environment variables instead of appsettings.json:

```bash
ConnectionStrings__DefaultConnection="Host=..."
Jwt__SecretKey="your-secret-key"
Jwt__Issuer="Recchx"
Jwt__Audience="RecchxUsers"
```

## Development

### Coding Standards

- Follow Clean Architecture principles
- Use CQRS pattern for application logic
- Implement Result pattern for error handling
- Write async/await methods for I/O operations
- Use dependency injection for all services
- Validate all inputs using FluentValidation
- Log important operations and errors using Serilog

### Testing

- Unit tests for domain logic
- Integration tests for repositories
- API tests for endpoints
- Use in-memory database for testing

### Git Workflow

1. Create feature branch from `main`
2. Implement feature with meaningful commits
3. Test thoroughly
4. Create pull request
5. Code review
6. Merge to main

## License

This project is proprietary software. All rights reserved.

Copyright (c) 2025 Francis Gbohunmi

---

For questions, issues, or contributions, please contact the development team.
