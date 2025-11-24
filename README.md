# Api-Web-Base

## Overview

Api-Web-Base is a clean and extensible RESTful API template built with ASP.NET Core, implementing a layered architecture (Domain, Application, Infrastructure, API) with generic base controllers and auditing middleware. This project aims to provide a solid foundation for rapid development of CRUD-based APIs with built-in request auditing.

## Features

- Layered architecture:
  - **Domain**: Entities and business models
  - **Application**: Interfaces, services, business logic
  - **Infrastructure**: Data persistence (EF Core DbContext), external services implementations
  - **ApiWebBase**: Presentation layer with Controllers and Middleware
- Generic base controllers with reusable CRUD actions (List, Insert, Update, Delete)
- Middleware for automatic auditing of POST, PUT, DELETE requests
- Audit logs saved with user info, action, entity, timestamp, IP address, and user agent
- Dependency Injection and clean separation of concerns

## Technologies

- ASP.NET Core 7+
- Entity Framework Core
- SQL Server (or any EF Core compatible database)
- Swagger / OpenAPI for API documentation and testing
- C# 11

## Getting Started

### Prerequisites

- .NET 7 SDK or later
- SQL Server (or compatible DB)
- Visual Studio 2022 / VSCode or any C# IDE

### Auditing

The API automatically logs user actions on POST, PUT, and DELETE requests through a middleware component. Audit entries include:
- User ID (from authentication claims)
- HTTP method and endpoint
- Entity name and optional entity ID
- Requestor IP address
- User agent string
- Timestamp
- Logs are saved in the AuditLogs table in the database.

### Extending

- Create entities in the Domain layer
- Define repositories and services in Application layer
- Implement database context and repositories in Infrastructure
- Create concrete controllers inheriting from BaseController<T>

## Usage and Contributions

Feel free to **use, fork, and modify** this boilerplate for your own projects and repositories.

However, this repository **does not accept pull requests or external contributions**.  
All improvements should be made in your own forks or separate projects.

Licensed under the MIT License.
