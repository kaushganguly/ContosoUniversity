# Architecture Diagram

ContosoUniversity is a single ASP.NET MVC web application that serves Razor views for school administration workflows and uses Entity Framework Core for persistence. The application is organized around MVC controllers, EF Core domain models, and a queue-backed notification service.

## Application Architecture

```mermaid
flowchart TD
    subgraph Client["Client Layer"]
        Browser["Web Browser"]
    end
    subgraph Web["Presentation Layer - ASP.NET MVC 5"]
        Razor["Razor Views"]
        Controllers["MVC Controllers"]
        Filters["Global Error Filter"]
        Bundles["Script and CSS Bundles"]
    end
    subgraph App["Application Services"]
        BaseCtrl["BaseController"]
        NotifySvc["NotificationService"]
        Initializer["DbInitializer"]
    end
    subgraph Data["Data Layer"]
        Context["SchoolContext EF Core 3.1"]
        Entities["School Domain Entities"]
        SqlDb[("SQL Server LocalDB")]
    end
    subgraph External["External Integrations"]
        Msmq["Microsoft Message Queuing"]
        Uploads["Teaching Material File Store"]
    end

    Browser -->|"HTTP requests"| Controllers
    Controllers -->|"render"| Razor
    Controllers -->|"shared data access and notifications"| BaseCtrl
    BaseCtrl -->|"CRUD operations"| Context
    Context -->|"maps"| Entities
    Context -->|"SQL queries"| SqlDb
    Controllers -->|"course image upload"| Uploads
    BaseCtrl -->|"entity change events"| NotifySvc
    NotifySvc -->|"queued JSON messages"| Msmq
    Initializer -->|"seed on startup"| Context
    Filters -.->|"handles MVC errors"| Controllers
    Bundles -.->|"serves client assets"| Razor
```

### Technology Stack Summary

| Layer | Technology | Version | Purpose |
|---|---:|---:|---|
| Presentation | ASP.NET MVC | 5.2.9 | Server-rendered MVC controllers and Razor views |
| Runtime | .NET Framework | 4.8 | Legacy ASP.NET web application runtime |
| Data Access | Entity Framework Core | 3.1.32 | ORM for SQL Server persistence |
| Database | SQL Server LocalDB | MSSQLLocalDB | Local relational database configured by `DefaultConnection` |
| Messaging | System.Messaging / MSMQ | .NET Framework built-in | Queue notifications for entity create, update, and delete events |
| Serialization | Newtonsoft.Json | 13.0.3 | Serializes notification messages |
| Client Assets | Bootstrap, jQuery, MVC bundles | Bootstrap 5.3.3, jQuery 3.7.1 | UI styling, validation, and script bundling |

### Data Storage & External Services

The application stores school data in a SQL Server LocalDB database named `ContosoUniversityNoAuthEFCore`. It also writes uploaded course teaching material images to the web application's `Uploads/TeachingMaterials` directory and publishes entity-change notifications to a private MSMQ queue configured by `NotificationQueuePath`.

### Key Architectural Decisions

- Uses a classic ASP.NET MVC 5 monolith: controllers directly coordinate EF Core queries, view models, Razor views, and persistence.
- Centralizes context and notification access in `BaseController`, so CRUD controllers inherit the same `SchoolContext` and `NotificationService` lifecycle.
- Seeds demo data at application startup with `DbInitializer.Initialize`, using `Database.EnsureCreated` instead of versioned migrations.

## Component Relationships

```mermaid
flowchart LR
    subgraph Presentation["Presentation"]
        HomeCtrl["HomeController"]
        StudentCtrl["StudentsController"]
        CourseCtrl["CoursesController"]
        InstructorCtrl["InstructorsController"]
        DepartmentCtrl["DepartmentsController"]
        NotificationCtrl["NotificationsController"]
        Views["Razor Views"]
    end
    subgraph Shared["Shared MVC Infrastructure"]
        BaseController["BaseController"]
        RouteConfig["RouteConfig"]
        FilterConfig["FilterConfig"]
        BundleConfig["BundleConfig"]
    end
    subgraph Business["Business Logic"]
        NotifyService["NotificationService"]
        Paging["PaginatedList"]
        InstructorCourses["Instructor Course Assignment Logic"]
        UploadLogic["Course Teaching Material Upload Logic"]
    end
    subgraph DataAccess["Data Access"]
        SchoolContext["SchoolContext"]
        DbInit["DbInitializer"]
        DomainModels["Domain Models"]
    end
    subgraph Infra["Infrastructure"]
        SqlServer["SQL Server LocalDB"]
        Queue["MSMQ Private Queue"]
        FileStore["Uploads Folder"]
    end

    HomeCtrl -->|"inherits"| BaseController
    StudentCtrl -->|"inherits"| BaseController
    CourseCtrl -->|"inherits"| BaseController
    InstructorCtrl -->|"inherits"| BaseController
    DepartmentCtrl -->|"inherits"| BaseController
    NotificationCtrl -->|"inherits"| BaseController
    RouteConfig -.->|"routes requests"| Presentation
    FilterConfig -.->|"error handling"| Presentation
    BundleConfig -.->|"assets"| Views
    Presentation -->|"renders"| Views
    StudentCtrl -->|"pages lists"| Paging
    CourseCtrl -->|"validates and stores files"| UploadLogic
    InstructorCtrl -->|"maintains selections"| InstructorCourses
    BaseController -->|"uses"| SchoolContext
    BaseController -->|"uses"| NotifyService
    NotificationCtrl -->|"polls and updates"| NotifyService
    SchoolContext -->|"maps"| DomainModels
    DbInit -->|"seeds"| SchoolContext
    SchoolContext -->|"persists"| SqlServer
    NotifyService -->|"send and receive"| Queue
    UploadLogic -->|"writes"| FileStore
```

### Component Inventory

| Component | Layer | Type | Responsibility |
|---|---|---|---|
| HomeController | Presentation | MVC Controller | Serves landing, about, contact, error, and unauthorized views |
| StudentsController | Presentation | MVC Controller | Manages student list, search, pagination, details, create, edit, and delete workflows |
| CoursesController | Presentation | MVC Controller | Manages courses, department selection, and teaching material image uploads |
| InstructorsController | Presentation | MVC Controller | Manages instructor profiles, office assignments, course assignments, and detail drill-downs |
| DepartmentsController | Presentation | MVC Controller | Manages departments, administrators, and optimistic concurrency handling |
| NotificationsController | Presentation | MVC Controller | Exposes JSON endpoints and dashboard for queued entity notifications |
| BaseController | Shared Infrastructure | MVC Base Class | Creates and disposes `SchoolContext` and `NotificationService`, and sends entity notifications |
| SchoolContext | Data Access | EF Core DbContext | Defines entity sets, table names, inheritance, relationships, and composite keys |
| DbInitializer | Data Access | Startup Seeder | Creates and seeds the database with sample university data |
| NotificationService | Business Logic | Service | Serializes notifications and exchanges messages with MSMQ |
| PaginatedList | Business Logic | Utility | Creates paged result sets for list screens |
| RouteConfig | Shared Infrastructure | MVC Configuration | Defines default MVC route `{controller}/{action}/{id}` |
| FilterConfig | Shared Infrastructure | MVC Configuration | Registers global MVC error handling |
| BundleConfig | Shared Infrastructure | Asset Configuration | Defines JavaScript and CSS bundles |
