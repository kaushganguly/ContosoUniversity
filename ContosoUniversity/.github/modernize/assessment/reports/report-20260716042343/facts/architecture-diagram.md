# Architecture Diagram

ContosoUniversity is a single ASP.NET MVC web application that combines presentation, business, and persistence concerns in one deployable unit. The codebase uses MVC controllers and Razor views over Entity Framework Core with SQL Server LocalDB, and publishes change notifications through MSMQ.

## Application Architecture

```mermaid
flowchart TD
    subgraph Client["Client Layer"]
        Browser["Web Browser"]
        Ajax["Notification Polling Script"]
    end
    subgraph App["Application Layer - ASP.NET MVC 5 on .NET Framework 4.8"]
        Controllers["MVC Controllers"]
        Views["Razor Views"]
        BaseCtrl["BaseController"]
        NotifySvc["NotificationService"]
    end
    subgraph Data["Data Layer"]
        EF["Entity Framework Core 3.1"]
        Init["DbInitializer"]
        Uploads["Teaching Material Files"]
        DB[("SQL Server LocalDB")]
    end
    subgraph External["External Services"]
        MSMQ["MSMQ Private Queue"]
    end

    Browser -->|"HTTP GET/POST"| Controllers
    Controllers -->|"render responses"| Views
    Ajax -->|"JSON polling"| Controllers
    Controllers -->|"shared notification flow"| BaseCtrl
    Controllers -->|"CRUD and queries"| EF
    Controllers -->|"save uploaded images"| Uploads
    BaseCtrl -->|"dispatch entity events"| NotifySvc
    NotifySvc -->|"enqueue messages"| MSMQ
    EF -->|"SQL queries"| DB
    Init -->|"seed sample data"| DB
```

### Technology Stack Summary

| Layer | Technology | Version | Purpose |
|---|---|---:|---|
| Presentation | ASP.NET MVC | 5.2.9 | Server-side web framework and routing |
| Presentation | Razor | 3.2.9 | View templating |
| Presentation | jQuery + Bootstrap | 3.7.1 / 5.3.3 | Client-side behaviors and styling |
| Application | .NET Framework | 4.8 | Runtime for the web application |
| Application | Newtonsoft.Json | 13.0.3 | JSON serialization for notifications |
| Data | Entity Framework Core | 3.1.32 | ORM and LINQ-based persistence |
| Data | Microsoft.Data.SqlClient | 2.1.4 | SQL Server connectivity |
| Integration | MSMQ | OS feature | Asynchronous entity change notifications |
| Storage | SQL Server LocalDB | MSSQLLocalDB | Primary relational database |
| Storage | File system uploads | N/A | Teaching material image storage |

### Data Storage & External Services

The application stores operational data in a single SQL Server LocalDB database named `ContosoUniversityNoAuthEFCore`. Uploaded teaching material images are written to the local `Uploads/TeachingMaterials` directory, and entity create/update/delete events are emitted to a local MSMQ private queue for notification scenarios.

### Key Architectural Decisions

- Uses a monolithic MVC architecture where controllers directly coordinate EF Core queries and updates instead of going through a separate repository or application service layer.
- Centralizes notification dispatch in `BaseController`, allowing CRUD controllers to reuse the same MSMQ integration path for entity change events.
- Mixes traditional ASP.NET MVC 5 hosting with newer EF Core and Microsoft.Extensions libraries, which is functional but increases modernization and dependency-compatibility complexity.

## Component Relationships

```mermaid
flowchart LR
    subgraph Presentation
        HomeCtrl["HomeController"]
        StudentCtrl["StudentsController"]
        CourseCtrl["CoursesController"]
        InstructorCtrl["InstructorsController"]
        DepartmentCtrl["DepartmentsController"]
        NotificationCtrl["NotificationsController"]
    end
    subgraph Business["Business Logic"]
        BaseCtrl2["BaseController"]
        NotifySvc2["NotificationService"]
        Pager["PaginatedList"]
    end
    subgraph DataAccess["Data Access"]
        SchoolCtx["SchoolContext"]
        Factory["SchoolContextFactory"]
        Seed["DbInitializer"]
        ViewModels["School ViewModels"]
    end
    subgraph Infra["Infrastructure"]
        AntiForgery["ValidateAntiForgeryToken"]
        Queue["MSMQ Queue"]
        Sql[("LocalDB")]
        Files["Upload Storage"]
    end

    HomeCtrl -->|"statistics queries"| SchoolCtx
    StudentCtrl -->|"paged CRUD"| Pager
    StudentCtrl -->|"queries and updates"| SchoolCtx
    CourseCtrl -->|"queries and updates"| SchoolCtx
    CourseCtrl -->|"stores images"| Files
    InstructorCtrl -->|"loads view model graphs"| ViewModels
    InstructorCtrl -->|"queries and updates"| SchoolCtx
    DepartmentCtrl -->|"queries and updates"| SchoolCtx
    NotificationCtrl -->|"reads notification data"| NotifySvc2
    HomeCtrl -.->|"inherits"| BaseCtrl2
    StudentCtrl -.->|"inherits"| BaseCtrl2
    CourseCtrl -.->|"inherits"| BaseCtrl2
    InstructorCtrl -.->|"inherits"| BaseCtrl2
    DepartmentCtrl -.->|"inherits"| BaseCtrl2
    NotificationCtrl -.->|"inherits"| BaseCtrl2
    BaseCtrl2 -->|"creates contexts"| Factory
    BaseCtrl2 -->|"sends entity events"| NotifySvc2
    NotifySvc2 -->|"enqueue"| Queue
    SchoolCtx -->|"SQL operations"| Sql
    Seed -->|"initialize and seed"| Sql
    AntiForgery -.->|"protects POST actions"| Presentation
```

### Component Inventory

| Component | Layer | Type | Responsibility |
|---|---|---|---|
| HomeController | Presentation | MVC Controller | Landing pages and enrollment statistics |
| StudentsController | Presentation | MVC Controller | Student search, paging, and CRUD |
| CoursesController | Presentation | MVC Controller | Course CRUD and teaching material uploads |
| InstructorsController | Presentation | MVC Controller | Instructor CRUD plus course assignment orchestration |
| DepartmentsController | Presentation | MVC Controller | Department CRUD with concurrency handling |
| NotificationsController | Presentation | MVC Controller | Notification polling and mark-as-read endpoints |
| BaseController | Business Logic | Base controller | Shared context creation and notification dispatch |
| NotificationService | Business Logic | Service | MSMQ message creation and queue operations |
| PaginatedList | Business Logic | Utility | Pagination helper for list screens |
| SchoolContext | Data Access | DbContext | Entity mapping and database interaction |
| SchoolContextFactory | Data Access | Factory | Creates configured DbContext instances |
| DbInitializer | Data Access | Initializer | Database bootstrap and seed data |
| AssignedCourseData / InstructorIndexData / EnrollmentDateGroup | Data Access | View models | Shapes data for complex UI views |
