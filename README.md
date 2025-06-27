# 🚀 Ambev Developer Evaluation Web API

Welcome to the **Ambev Developer Evaluation Web API** project!  
This repository contains a robust API developed in C# 12 and .NET 8, designed for technical assessments, system integration, and process automation.  
Here you will find a modern architecture, best practices, and integration with multiple technologies.

---

## 🛠️ Technologies Used

- **.NET 8** &nbsp;|&nbsp; C# 12
- **Entity Framework Core** (PostgreSQL)
- **MongoDB** (with advanced serialization support)
- **RabbitMQ** (messaging)
- **MediatR** (CQRS and pipeline behaviors)
- **AutoMapper** (object mapping)
- **Swagger/OpenAPI** (interactive documentation)
- **JWT Authentication** (security)
- **Docker** (containerization)
- **Angular** (suggested front-end for integration)

---

## 🏗️ Patterns and Best Practices

- **Dependency Injection** via IoC
- **Centralized Validation** with MediatR pipeline
- **Custom Middleware** for exception handling
- **Environment-based configuration** via `appsettings.json`
- **CORS** configured for integration with Angular applications
- **Automatic documentation** with Swagger

---

## 🚦 How to Run Locally

1. **Prerequisites**  
   - [.NET 8 SDK](https://dotnet.microsoft.com/download)
   - [Docker](https://www.docker.com/get-started)

2. **Clone the repository**

3. **Start the containers**

4. **Access the API documentation**  
   - [https://localhost:5001/swagger](https://localhost:5001/swagger)

5. **Authentication**  
   - Use the `/api/auth` endpoint to obtain a JWT token and access protected routes.

---

## 🧪 Tests and Database

- A **database backup file** will be available at the root of the project (`src/backup-database.sql`) to facilitate restoration and local testing.
- To restore the database, use the appropriate command for your DBMS (PostgreSQL).

---

## 💡 Quick Tips

- CORS is enabled for `http://localhost:4200` (Angular default).
- Connection settings are in `src/Ambev.DeveloperEvaluation.WebApi/appsettings.json`.
- The project follows the **Clean Architecture** pattern for maximum maintainability.

---

## 🤝 Contribution

Contributions are welcome!  

---

Made with 💙 by Lucas Holanda