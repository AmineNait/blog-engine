
# Blog Engine

## Description
This project is an administrative interface for creating blog posts and a REST API for querying them, developed in C# and .NET Core.

## Architecture
The project follows a microservices architecture with separate backend and frontend services. The backend provides a RESTful API for managing blog posts and categories, while the frontend offers a user-friendly interface for content management.

## Tools
- **Backend:** .NET Core, Entity Framework Core, SQL Server
- **Frontend:** ASP.NET Core MVC, Bootstrap


| Tech          | Description                          |
|---------------|--------------------------------------|
| Docker        | Containers Deployment (Docker-Compose) |
| Visual Studio | Development IDE                      |
| SQL Server    | Database                             |

## Backend

### Tools and Frameworks

| Tool/Framework | Description                           |
|----------------|---------------------------------------|
| C#             | Programming Language                  |
| .NET 8.0       | Framework                             |
| Web API        | Server API                            |
| EF Core        | ORM (Object-Relational Mapping)       |
| SQL Server     | Database                              |
| Swagger        | Endpoints Documentation               |
| XUnit          | Testing Framework                     |

### Folder Structure
```plaintext
<root>/
├── BlogApi/
│   ├── Controllers/
│   ├── Migrations/
│   ├── Models/
├── BlogApi.Tests/
```

| Project/Folder       | Description                                                  |
|----------------------|--------------------------------------------------------------|
| BlogApi/             | Main project containing the API implementation               |
| Controllers/         | Contains the API controllers to handle HTTP requests         |
| Migrations/          | Contains EF Core migration files                             |
| Models/              | Contains the domain models and data context                  |
| BlogApi.Tests/       | Contains the test project for the BlogApi                    |

## Frontend
### Tools and Frameworks

| Tool/Framework | Description                           |
|----------------|---------------------------------------|
| ASP.NET MVC    | Web Framework                         |
| Bootstrap      | Frontend Styling                      |
| XUnit          | Testing Framework                     |


### Folder Structure
```plaintext
<root>/
├── BlogAdmin/
│   ├── Controllers/
│   ├── Migrations/
│   ├── Models/
│   ├── Views/
│   │   ├── Category/
│   │   ├── Post/
│   │   ├── Home/
│   │   └── Shared/
├── BlogAdmin.Tests/

```

| Project/Folder       | Description                                                  |
|----------------------|--------------------------------------------------------------|
| BlogAdmin/           | Main project containing the MVC implementation               |
| Controllers/         | Contains the MVC controllers to handle HTTP requests         |
| Migrations/          | Contains EF Core migration files                             |
| Models/              | Contains the domain models and data context                  |
| Views/               | Contains the Razor views for the application                 |
| /Category/           | Views related to category management                         |
| /Post/               | Views related to post management                             |
| /Home/               | Views related to the home page                               |
| /Shared/             | Contains shared views and layout files                       |
| BlogAdmin.Tests/     | Contains the test project for the BlogAdmin                  |


## Getting Started

### Prerequisites
- .NET Core SDK
- Docker
- SQL Server

### Local Setup

#### 1. Backend
1. Navigate to the `BlogApi` directory:
    ```bash
    cd blog-engine/BlogApi
    ```
2. Restore NuGet packages:
    ```bash
    dotnet restore
    ```
3. Update the database connection string in `appsettings.json`.

4. Run the application:
    ```bash
    dotnet run
    ```

#### 2. Frontend
1. Navigate to the `BlogAdmin` directory:
    ```bash
    cd blog-engine/BlogAdmin
    ```
2. Restore NuGet packages:
    ```bash
    dotnet restore
    ```
3. Run the application:
    ```bash
    dotnet run
    ```
#### Acces to the applications :

Blog API : `https://localhost:7139/swagger`

Blog Admin : `https://localhost:7275`

### Docker Setup

To start the application using Docker.
Navigate to the `docker` directory:
    ```bash
    cd docker
    ```
Run 
    ```bash
    docker-compose up --build
    ```
#### Acces to the applications :

Blog API: `http://localhost:5000/swagger`

Blog Admin:`http://localhost:5001`

### Testing
To run the tests for both backend and frontend projects, navigate to their respective test project directories and execute:
```bash
dotnet test
```

For example, to test the backend:
```bash
cd blog-engine/BlogApi.Tests
dotnet test
```

And to test the frontend:
```bash
cd blog-engine/BlogAdmin.Tests
dotnet test
```

## Contribution
Thank you for your interest! However, this project is part of a job application test and is not open for contributions at this time.

## License
This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

