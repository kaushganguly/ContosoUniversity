# Contoso University - .NET 10

This project is an ASP.NET Core MVC application targeting .NET 10.

## Project Overview

### Framework
- ASP.NET Core MVC (.NET 10)

### Database Access
- Entity Framework Core 10 with SQLite

### Project Structure
```
ContosoUniversity/
├── Controllers/            # MVC controllers
├── Data/                   # Entity Framework context and initializer
├── Models/                 # Data models and view models
├── Views/                  # Razor views
├── Content/                # CSS and other content
├── Scripts/                # JavaScript files
├── Uploads/                # Uploaded teaching materials
├── Program.cs              # ASP.NET Core application startup
└── appsettings.json        # Application configuration
```

## Database Configuration

The application uses SQLite with the following connection string in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=ContosoUniversity.db"
  }
}
```

## Running the Application

1. **Prerequisites**:
   - .NET 10 SDK

2. **Setup**:
   - Restore NuGet packages with `dotnet restore`
   - Build the solution with `dotnet build`
   - Run the application with `dotnet run`

## Features

- **Student Management**: CRUD operations for students with pagination and search
- **Course Management**: Manage courses and their assignments to departments
- **Instructor Management**: Handle instructor assignments and office locations
- **Department Management**: Manage departments and their administrators
- **Statistics**: View enrollment statistics by date

## Database Initialization

The application uses Entity Framework Core code-first initialization that:
- Creates the database if it doesn't exist
- Seeds sample data including students, instructors, courses, and departments
- Reuses the generated SQLite database file for local development
