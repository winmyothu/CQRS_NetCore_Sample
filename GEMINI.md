# GEMINI.md

## 🧠 Project Overview

This is an example project demonstrating the CQRS (Command Query Responsibility Segregation) pattern in a .NET application.

**Stack:**
- Backend: .NET 8 with CQRS
- Database: SQL Server
- API: RESTful, built with ASP.NET Core Web API

The project manages a simple system for:
- Customers
- Products

---

## ✍️ Coding Guidelines

### Backend (.NET 8)
- Use **PascalCase** for class, method, and enum names.
- Use **camelCase** for variables and parameters.
- Use **Records** for immutable DTOs and commands/queries where appropriate.
- Commands and queries are handled by **MediatR**.
- Input is validated using **FluentValidation**.
- Controllers are thin and delegate business logic to MediatR handlers.

---

## ⚙️ Architecture Rules

- The project follows a CQRS pattern, separating read (Query) and write (Command) operations.
- **Features Folder**: Contains all the core logic, organized by business feature (e.g., Customers, Products).
  - **Commands**: Define the intent to change state.
  - **Queries**: Define the intent to read state.
  - **Handlers**: Contain the business logic to process commands and queries.
  - **Validators**: Contain validation logic for commands.
- **Data Folder**: Contains the Entity Framework `DbContext` for database interaction.
- **Models Folder**: Contains the core domain entities.
- **Controllers Folder**: Contains the API endpoints that send commands and queries to MediatR.

---

## ✅ Quality & Standards

- All public methods should have XML comments.
- No hardcoded strings (use constants or resource files).
- Always handle nulls defensively.
- Use async programming where appropriate.
- Break down methods > 40 lines.
- Use Unit Tests for Handlers and Service logic.

---

## 🔍 Gemini Review Instructions

When helping or reviewing code:
- Propose improvements but explain *why* (readability, performance, etc.).
- Follow the CQRS separation strictly.
- Suggest helper methods or refactors if the logic repeats.
- Don’t introduce breaking changes without justification.
- Prioritize **clarity and maintainability** over cleverness.

---

## 📁 Folder Organization

```
/
├── Controllers/
│   ├── CustomerController.cs
│   └── ProductController.cs
├── Data/
│   └── AppDbContext.cs
├── Features/
│   ├── Customers/
│   │   ├── Commands/
│   │   ├── Handlers/
│   │   ├── Queries/
│   │   └── Validators/
│   └── Products/
│       ├── Commands/
���       ├── Handlers/
│       └── Queries/
├── Migrations/
├── Models/
│   ├── Customer.cs
│   └── Product.cs
├── Properties/
└── CQRSExample.Tests/
```

### Frontend (Angular)

```
CQRSExample.Frontend/src/app/
├── app.component.css
├── app.component.html
├── app.component.spec.ts
├── app.component.ts
├── app.config.ts
├── app.module.ts
├── app.routes.ts
├── core/
│   └── api/
│       ├── guest-registration.service.spec.ts
│       └── guest-registration.service.ts
└── features/
    └── guest-registration-form/
        ├── guest-registration-form.component.css
        ├── guest-registration-form.component.html
        ├── guest-registration-form.component.spec.ts
        └── guest-registration-form.component.ts
```