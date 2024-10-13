# To-Do List API

**To-Do List API** is a pet project designed to practice skills in software development, focusing on RESTful principles. This API allows managing tasks (To-Do) with support for tags, users, and categories. The project is built using **ASP.NET Core 8** with **Entity Framework Core** for database interaction and **PostgreSQL** as the database. **Fluent Validation** is used for data validation, and **Data Transfer Objects (DTOs)** are employed to streamline communication between the client and server. Additionally, **Serilog** is implemented for logging, including support for **Seq** and **Elasticsearch** for storing and analyzing logs.

The project now also utilizes **xUnit** for unit testing, **Moq** for mocking dependencies, **FluentAssertions** for more readable assertions in tests, and **AutoFixture** for automated test data generation. Docker is used to run the API in a container alongside PostgreSQL, providing an efficient development and deployment environment. The image for the API is available on Docker Hub as `cladkoewka/to-do-api-cladkoewka:latest`.

The API and database are deployed using **Render**, while the frontend is hosted on **GitHub Pages**. The project demonstrates key principles of **Clean Architecture**, ensuring separation of concerns, flexibility, and scalability, making it a great example for honing development skills and applying best practices in API design.

You can access the frontend at: [To-Do List Frontend](https://github.com/Cladkoewka/ToDoListFrontend)

You can try it at: [To-Do List GitHub Pages](https://cladkoewka.github.io/ToDoListFrontend/)

## Technologies Used

- **ASP.NET Core 8**
- **Entity Framework Core**
- **PostgreSQL**
- **Fluent Validation**
- **DTOs** for data transfer
- **Serilog** for logging (including **Seq** and **Elasticsearch**)
- **xUnit** for unit testing (with **Moq**, **FluentAssertions**, and **AutoFixture**)
- **Docker** for containerization

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

## Deployment Instructions

### Prerequisites
- [Docker](https://www.docker.com/get-started)

### Deployment Using Docker

1. **Pull the API Image from Docker Hub**

   Run the following command to download the API image:

   ```bash
   docker pull cladkoewka/to-do-api-cladkoewka:latest
   ```

2. **Create the `docker-compose.yml`**

   You can copy the `docker-compose.yml` file from the current repository to your local machine. Make sure to adjust any necessary configuration settings in the file.

3. **Start the Containers**

   In the directory where your `docker-compose.yml` is located, run the following command to start the containers:

   ```bash
   docker-compose up
   ```

   This command will start the containers for the API and PostgreSQL.

4. **Check the API**

   Once the containers are running, the API will be accessible at:

   ```
   http://localhost:8080/api
   ```

   You can test the API endpoints using tools like [Postman](https://www.postman.com/).

## Contribution

Feel free to fork the repository, make changes, and create a pull request. Contributions are welcome!

## License

This project is licensed under the MIT License.
