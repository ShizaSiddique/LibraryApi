# Library API

A .NET 10 Web API for a library system: browse books, manage the catalog, and borrow and return books.
Built with Clean Architecture, CQRS, Entity Framework Core, SQL Server stored procedures, and ASP.NET Core Identity.

## Tech stack

- .NET 10 / ASP.NET Core Web API (controllers)
- Entity Framework Core 10 with SQL Server
- MediatR 12.5 for CQRS (the last Apache 2.0-licensed version; v13+ requires a commercial license)
- ASP.NET Core Identity with bearer token authentication
- Two SQL Server stored procedures, deployed through EF Core migrations
- Scalar for interactive API documentation

## Architecture

The solution follows Clean Architecture, with dependencies pointing inward:

```
src/
├── Library.Domain           Entities (Author, Book, Loan) and domain rules. No dependencies.
├── Library.Application      CQRS commands, queries and handlers; DTOs; interfaces
│                            (ILibraryDbContext, ILoanProcedures); business exceptions.
├── Library.Infrastructure   EF Core DbContext, entity configurations, migrations,
│                            stored procedure calls, Identity setup.
└── Library.Api              Controllers, global exception handling, DI composition root.
```

- **Domain** references nothing.
- **Application** references Domain only, and defines interfaces for what it needs.
- **Infrastructure** implements those interfaces (Dependency Inversion).
- **Api** wires everything together and only talks to Application through MediatR.

## CQRS

Every use case is a separate command (writes) or query (reads), each with its own handler,
organized by feature:

```
Application/
├── Books/
│   ├── Commands/  CreateBook, UpdateBook, DeleteBook
│   └── Queries/   GetBooks, GetBookById
└── Loans/
    ├── Commands/  BorrowBook, ReturnBook
    └── Queries/   GetMyLoans, GetOverdueLoans
```

Controllers contain no business logic; they send requests through MediatR's `ISender`.
Commands return minimal data (e.g. the new ID), and queries return DTOs.

## Stored procedures

Both are created by the `AddStoredProcedures` migration, so no manual SQL setup is needed.

| Procedure | Purpose |
|---|---|
| `sp_BorrowBook` | Decrements available copies and creates the loan in a single transaction. The availability check and decrement happen in one `UPDATE ... WHERE AvailableCopies > 0`, which prevents two users from borrowing the last copy at the same time. |
| `sp_GetOverdueLoans` | Reporting query that joins loans, books and Identity users, and calculates days overdue. |

Procedures are called through `ILoanProcedures`, so the Application layer does not know they are stored procedures.
Calls are parameterized (EF Core `SqlQuery` with interpolated parameters), which protects against SQL injection.

## Getting started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)

### 1. Start SQL Server

```bash
docker run --platform linux/amd64 -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Library@12345" -p 1433:1433 --name library-sql -d mcr.microsoft.com/mssql/server:2022-latest
```

Wait about 30 seconds for SQL Server to start.
(`--platform linux/amd64` is only needed on Apple Silicon Macs.)

### 2. Create the database

```bash
dotnet tool install --global dotnet-ef
dotnet ef database update --project src/Library.Infrastructure --startup-project src/Library.Api
```

This creates all tables, seed data (2 authors, 2 books), the Identity tables and both stored procedures.

### 3. Run the API

```bash
dotnet run --project src/Library.Api
```

Open **http://localhost:5099/scalar** to explore and test the API.

## Authentication

1. Register: `POST /api/auth/register`
```json
   { "email": "user@test.com", "password": "Test@12345" }
```
2. Log in: `POST /api/auth/login` with the same body. Copy `accessToken` from the response.
3. Send it on protected requests as a header:
   `Authorization: Bearer <accessToken>`

Passwords must contain an uppercase letter, a lowercase letter, a digit and a symbol.

## Endpoints

| Method | Route | Auth | Description |
|---|---|---|---|
| POST | `/api/auth/register` | Public | Create an account |
| POST | `/api/auth/login` | Public | Get an access token |
| GET | `/api/books` | Public | List all books |
| GET | `/api/books/{id}` | Public | Get a book |
| POST | `/api/books` | Required | Add a book |
| PUT | `/api/books/{id}` | Required | Update a book |
| DELETE | `/api/books/{id}` | Required | Delete a book (only if it has no loan history) |
| POST | `/api/loans` | Required | Borrow a book (`sp_BorrowBook`) |
| PUT | `/api/loans/{id}/return` | Required | Return your own loan |
| GET | `/api/loans/me` | Required | Your loans |
| GET | `/api/loans/overdue` | Required | Overdue loans report (`sp_GetOverdueLoans`) |


