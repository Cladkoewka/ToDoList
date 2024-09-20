# To-Do List API

**To-Do List API** is a pet project designed to practice skills in software development, focusing on RESTful principles. This API allows managing tasks (To-Do) with support for tags, users, and categories. The project is built using **ASP.NET Core 8** with **Entity Framework Core** for database interaction and **PostgreSQL** as the database. **Fluent Validation** is used for data validation, and **Data Transfer Objects (DTOs)** are employed to streamline communication between the client and server. Additionally, **Serilog** is implemented for logging, including support for **Seq** and **Elasticsearch** for storing and analyzing logs.

The project demonstrates key principles of **Clean Architecture**, ensuring separation of concerns, flexibility, and scalability, making it a great example for honing development skills and applying best practices in API design.

## Technologies Used

- **ASP.NET Core 8**
- **Entity Framework Core**
- **PostgreSQL**
- **Fluent Validation**
- **DTOs** for data transfer
- **Serilog** for logging (including **Seq** and **Elasticsearch**)

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

## Frontend Example

![Front](https://github.com/user-attachments/assets/e3828576-85ea-4f59-b40d-42cca9947d3c)

