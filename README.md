\# DVLD API — Driving \& Vehicle License Management System



A RESTful API built with ASP.NET Core for managing driving license applications, test appointments, drivers, and license-related operations. The project uses a layered architecture to separate business logic, domain entities, data access, and API endpoints.



\## Overview



DVLD API is the backend component of a Driving \& Vehicle License Management System. It exposes REST endpoints consumed by the DVLD Windows Forms desktop application and integrates with a separate authentication service.



The solution is organized into six projects, separating API responsibilities, application services, domain models, infrastructure, shared components, and automated tests.



\## Features



\- \*\*Application Management:\*\* Manage applications and application types.

\- \*\*People \& Drivers:\*\* Manage people and driver-related records.

\- \*\*Local Driving License Applications:\*\* Handle applications for local driving licenses.

\- \*\*Test Management:\*\* Manage test types, appointments, and test-related operations.

\- \*\*License Management:\*\* Support license-related workflows, including issuance, renewal, and international licenses.

\- \*\*User Management:\*\* Manage users and their roles within the DVLD system.

\- \*\*Authentication \& Authorization:\*\* Validate JWT access tokens and enforce authorization policies.

\- \*\*Database Persistence:\*\* Use Entity Framework Core with SQL Server.

\- \*\*Automated Testing:\*\* Include unit and integration tests.



\## Technologies Used



\- C# and .NET 10

\- ASP.NET Core Web API

\- Entity Framework Core

\- Microsoft SQL Server

\- JWT Bearer Authentication

\- Swagger / OpenAPI

\- Dependency Injection

\- xUnit

\- Moq

\- WebApplicationFactory



\## Architecture



The solution contains six projects:



| Project | Responsibility |

|---|---|

| `DVLD.API` | API controllers, HTTP configuration, authentication, authorization, and application startup. |

| `DVLD.Application` | Application services, feature-specific DTOs and interfaces, and application workflows. |

| `DVLD.Domain` | Domain entities, enums, and domain contracts. |

| `DVLD.Infrastructure` | Database context, EF Core configurations and migrations, repositories, HTTP client implementation, and Unit of Work. |

| `DVLD.Shared` | Shared DTOs, helpers, and response-related components. |

| `DVLD.Tests` | Unit and integration tests. |



\### Request Flow



A typical request follows this general flow:



`HTTP Request → Controller → Application Service → Repository / Unit of Work → SQL Server`



The application layer contains the business workflows, while infrastructure components handle persistence and external HTTP communication.



\## Prerequisites



Before running the project, make sure you have:



\- \[.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)

\- Microsoft SQL Server

\- A SQL Server instance accessible from your development environment

\- The separate Auth API configured and running when authentication-related operations require it

\- An IDE such as Visual Studio or a compatible .NET development environment



\## Configuration



Configure the API through `appsettings.json` or the appropriate environment-specific configuration.



\### 1. Database Connection



The default configuration uses a local SQL Server instance with Windows Authentication:



```json

{

&#x20; "ConnectionStrings": {

&#x20;   "DefaultConnection": "Server=.;Database=DVLD;Trusted\_Connection=True;TrustServerCertificate=True;"

&#x20; }

}

```



Update `DefaultConnection` to match your SQL Server instance and database configuration.



\### 2. JWT Settings



The API uses the following JWT validation settings:



```json

{

&#x20; "JwtSettings": {

&#x20;   "Issuer": "AuthService",

&#x20;   "Audience": "DVLD"

&#x20; }

}

```



These values must be consistent with the tokens issued by the authentication service. Configure any required signing secrets securely through environment variables or another appropriate secrets-management mechanism.



\### 3. Authentication API



The API communicates with a separate authentication service through an HTTP client:



```json

{

&#x20; "Auth\_API": {

&#x20;   "BaseUrl": "https://localhost:7011"

&#x20; }

}

```



Change `BaseUrl` to the address where your Auth API is running. Ensure that HTTPS and development certificates are configured correctly when using a local environment.



\## Database Setup



The project uses Entity Framework Core migrations to manage database schema changes. Migrations are applied manually rather than automatically at application startup.



\### 1. Restore Dependencies



Run the following commands from the solution directory:



```bash

dotnet restore

```



\### 2. Apply Existing Migrations



Ensure the SQL Server instance is running and the connection string is configured correctly. Then run:



```bash

dotnet ef database update --project DVLD.Infrastructure --startup-project DVLD.API

```



If the `dotnet ef` command is unavailable, install the EF Core CLI tool:



```bash

dotnet tool install --global dotnet-ef

```



The database update command applies the existing migrations to the configured database.



\### 3. Create a New Migration



When database model changes require a schema update, create a migration with:



```bash

dotnet ef migrations add MigrationName --project DVLD.Infrastructure --startup-project DVLD.API

```



Replace `MigrationName` with a descriptive name. Review the generated migration before applying it to the database.



\## Running the API



From the solution directory, run:



```bash

dotnet run --project DVLD.API

```



The API uses the ASP.NET Core `Development` environment to enable Swagger UI. When running in that environment, open the Swagger URL shown in the terminal output to explore and test the available endpoints.



Make sure the database is configured and the required authentication service is available for operations that depend on it.



\## Running Tests



The `DVLD.Tests` project contains unit tests and integration tests.



Run the test suite from the solution directory:



```bash

dotnet test DVLD.Tests

```



The test project uses:



\- \*\*xUnit\*\* for test execution.

\- \*\*Moq\*\* for mocking dependencies in unit tests.

\- \*\*WebApplicationFactory\*\* for integration testing of the ASP.NET Core application.



Integration tests are configured to use a separate test database. Ensure the test environment is configured correctly before running them.



\## Repository Structure



```text

DVLDService/

├── DVLD.API/

│   ├── Controllers/

│   ├── Extensions/

│   ├── Properties/

│   ├── appsettings.json

│   └── Program.cs

├── DVLD.Application/

│   ├── Features/

│   ├── Interfaces/

│   └── Exceptions/

├── DVLD.Domain/

│   ├── Entities/

│   ├── Enums/

│   └── Contracts/

├── DVLD.Infrastructure/

│   ├── Configurations/

│   ├── Data/

│   ├── HTTPClient/

│   ├── Migrations/

│   ├── Persistence/

│   ├── Repositories/

│   └── UnitOfWork/

├── DVLD.Shared/

│   ├── DTOs/

│   ├── Helpers/

│   └── Responses/

├── DVLD.Tests/

│   ├── IntegrationTests/

│   └── UnitTests/

└── DVLDService.slnx

```



\## Related Projects



\- \[Auth Service API](https://github.com/HaiderMohammed7/Auth-Service-API) — Separate authentication service.

\- \[DVLD System](https://github.com/HaiderMohammed7/DVLD-System) — Windows Forms desktop application.



\## Author



\*\*Haider Mohammed Hameid\*\*



\- \[GitHub](https://github.com/HaiderMohammed7)

\- \[LinkedIn](https://www.linkedin.com/in/haider-mohammed-3342381b4/)

