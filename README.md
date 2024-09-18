# To-Do List API

**To-Do List API** is a pet project designed to practice the principles of **Clean Architecture**. This API allows managing tasks (To-Do) with support for tags, users, and categories. The project is built using **ASP.NET Core 8** with **Entity Framework Core** for database interaction and **PostgreSQL** as the database. **Fluent Validation** is used for data validation.

The project showcases key principles of separation of concerns, flexibility, and scalability, making it a great example of applying clean architecture in API development.

## Technologies Used

- **ASP.NET Core 8**
- **Entity Framework Core**
- **PostgreSQL**
- **Fluent Validation**
- **Clean Architecture**

## Endpoints

### Task Endpoints

- **GET /api/tasks** — Retrieves all tasks.
- **GET /api/tasks/{id}** — Retrieves a task by its ID.
- **GET /api/tasks/by-tags?tagIds={tagIds}** — Retrieves tasks filtered by tag IDs.
- **POST /api/tasks** — Creates a new task.
- **PUT /api/tasks/{id}** — Updates an existing task.
- **DELETE /api/tasks/{id}** — Deletes a task by its ID.

### Tag Endpoints

- **GET /api/tags** — Retrieves all tags.
- **GET /api/tags/{id}** — Retrieves a tag by its ID.
- **POST /api/tags** — Creates a new tag.
- **PUT /api/tags/{id}** — Updates an existing tag.
- **DELETE /api/tags/{id}** — Deletes a tag by its ID.

### User Endpoints

- **GET /api/users** — Retrieves all users.
- **GET /api/users/{id}** — Retrieves a user by their ID.
- **GET /api/users/email/{email}** — Retrieves a user by their email.
- **POST /api/users** — Creates a new user.
- **PUT /api/users/{id}** — Updates an existing user.
- **DELETE /api/users/{id}** — Deletes a user by their ID.

## Getting Started

1. Clone the repository:
   ```bash
   git clone https://github.com/Cladkoewka/your-repo-name.git
   ```

2. Configure the database connection string in `appsettings.json`.

3. Apply database migrations:
   ```bash
   dotnet ef database update
   ```

4. Run the project:
   ```bash
   dotnet run
   ```
