# LiquidLabsAssessment

Libraries & Frameworks Used

ASP.NET Core Web API
Used to build the REST API. It provides built-in dependency injection, middleware support, and high performance for scalable backend development.

MediatR
Used to implement CQRS pattern. It keeps controllers thin and moves business logic into handlers, improving separation of concerns and maintainability.

ADO.NET (Microsoft.Data.SqlClient)
Used for database access instead of ORM as per requirement. It allows direct SQL queries, better performance control, and full flexibility over database operations.

Newtonsoft.Json
Used to handle dynamic JSON from the external API, especially for parsing flexible data objects into key-value structures.

Serilog
Used for structured logging (console + file logging). Helps in debugging, monitoring, and tracking application errors in production.

Swagger (Swashbuckle)
Used for API documentation and testing endpoints easily during development.

Architecture Decisions
Clean Architecture
Project is structured into API, Application, Domain, and Infrastructure layers to ensure separation of concerns and scalability.

CQRS Pattern
Implemented using MediatR to separate read and write operations and keep code clean and maintainable.

Caching Strategy
Implements database-first cache aside pattern:

Check SQL Server first
If data not found → call external API
Save result to DB
Return response

This reduces external API calls and improves performance.


Repository & Project Setup

This project is maintained in a single GitHub repository containing both the ASP.NET Core Web API and SQL Server database scripts.

The repository is created at the beginning of development, and changes are committed incrementally (feature by feature) instead of uploading everything at once.

Clone repository

git clone https://github.com/subkrishk1989-max/LiquidLabsAssessment.git
cd \LiquidLabsAssessment\LiquidLabsAssessment

2. Setup database

Run SQL scripts in /Database folder using SQL Server or Docker.

3. Update connection string

Edit appsettings.json.

4. Run API
dotnet restore
dotnet build
dotnet run --project LiquidLabsAssessment.Api