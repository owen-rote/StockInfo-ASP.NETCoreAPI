# StockInfo API

## Overview
A full-stack **ASP.NET Core 9** Web API implementing **JWT authentication**, **role-based access control**, and **CRUD operations** for stocks, portfolios, and user-generated comments. It utilizes **Entity Framework Core** for **Microsoft SQL Server** persistence and **Swagger** for API documentation.

## Features
- **Authentication & Authorization**: JWT-based, **ASP.NET Identity** integration.
- **Portfolio Management**: Users can manage owned stocks.
- **Querying**: Searching, sorting with different parameters. Also Pagination options.
- **Comment System**: User-generated content.
- **RBAC (Role-Based Access Control)**: Scoped access based on roles.
- **Swagger UI**: Interactive API documentation.
- **DI (Dependency Injection)**: Repository-service pattern.

## Technologies
- **.NET 9**, **ASP.NET Core 9**, **C#**
- **Entity Framework Core** (Code-first, migrations)
- **SQL Server** (EF-backed persistence layer)
- **JWT Authentication** (Bearer tokens, `System.IdentityModel.Tokens.Jwt`)
- **ASP.NET Identity** (User and role management)
- **Swagger** (API documentation)
- **Newtonsoft.Json** (Serialization, `ReferenceLoopHandling.Ignore`)
- **Dependency Injection** (Scoped repository instances)

## Security
- **JWT Bearer Tokens** (`Microsoft.IdentityModel.Tokens`)
- **Password Hashing** (`ASP.NET Identity` `PasswordHasher<TUser>`) 
- **Role-Based Authorization** (`[Authorize(Roles="Admin")]`)
- **CSRF Protection**: Tokens passed via `Authorization` header

## API Endpoints
### Authentication
- `POST /api/account/login` - Issues JWT token
- `POST /api/account/register` - Creates user with hashed password

### Stock
- `GET /api/stock` - Retrieves all stocks (Queryable)
- `POST /api/stock` - Creates stock
- `GET /api/stock/{id}` - Fetches stock by ID
- `PUT /api/stock/{id}` - Updates stock
- `DELETE /api/stock/{id}` - Deletes stock

### Portfolio
- `GET /api/portfolio` - Retrieves user-specific portfolio (requires auth)
- `POST /api/portfolio` - Adds stock to portfolio (requires auth)
- `DELETE /api/portfolio` - Removes stock from portfolio (requires auth)

### Comment
- `GET /api/comment` - Fetches all comments
- `GET /api/comment/{id}` - Retrieves specific comment
- `POST /api/comment/{stockId}` - Adds comment to stock (tied to user's auth)
- `PUT /api/comment/{id}` - Edits comment (owner-only access)
- `DELETE /api/comment/{id}` - Deletes comment (owner/admin only)

## Setup
1. Clone repo
2. Configure `appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "your-sql-server-connection-string"
   }
   ```
3. Apply migrations:
   ```sh
   dotnet ef database update
   ```
4. Run application:
   ```sh
   dotnet run
   ```
