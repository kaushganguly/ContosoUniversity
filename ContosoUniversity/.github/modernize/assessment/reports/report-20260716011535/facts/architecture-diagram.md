# Architecture Diagram

ContosoUniversity is a legacy ASP.NET MVC 5 web application targeting .NET Framework 4.8, implementing a university management system with server-side rendered views and a SQL Server database accessed via Entity Framework Core.

## Application Architecture

```mermaid
flowchart TD
    subgraph Client["Client Layer"]
        Browser["Web Browser"]
    end
    subgraph App["Application Layer - ASP.NET MVC 5 / .NET Framework 4.8"]
        MVC["ASP.NET MVC 5 Controllers + Razor Views"]
        Bundle["Script/CSS Bundling (WebOptimization)"]
        GlobalAsax["Global.asax Application Lifecycle"]
    end
    subgraph Services["Service Layer"]
        NotifSvc["NotificationService (MSMQ)"]
        LogSvc["LoggingService"]
    end
    subgraph Data["Data Layer"]
        EFCore["Entity Framework Core 3.1 DbContext"]
        DB[("SQL Server LocalDB")]
        MSMQ[("MSMQ Private Queue")]
    end
    subgraph External["External / Infrastructure"]
        IIS["IIS Express Host"]
        FileSystem["Local File System (Uploads)"]
    end

    Browser -->|"HTTP requests"| IIS
    IIS -->|"routes"| MVC
    MVC -->|"creates/queries"| EFCore
    MVC -->|"triggers"| NotifSvc
    EFCore -->|"SQL queries"| DB
    NotifSvc -->|"enqueues messages"| MSMQ
    MVC -->|"serves static assets"| Bundle
    GlobalAsax -->|"seeds on startup"| EFCore
    MVC -->|"file upload path"| FileSystem
```

### Technology Stack Summary

| Layer | Technology | Version | Purpose |
|-------|-----------|---------|---------|
| Presentation | ASP.NET MVC | 5.2.9 | Server-side MVC web framework |
| Presentation | Razor Views | 3.2.9 | HTML templating engine |
| Presentation | Bootstrap | 5.3.3 | CSS/UI framework |
| Presentation | jQuery | 3.7.1 | Client-side scripting |
| Business Logic | C# / .NET Framework | 4.8 | Application runtime |
| Data Access | Entity Framework Core | 3.1.32 | ORM for SQL Server |
| Data Access | Microsoft.Data.SqlClient | 2.1.4 | SQL Server driver |
| Messaging | System.Messaging (MSMQ) | Windows built-in | Notification queue |
| Serialization | Newtonsoft.Json | 13.0.3 | JSON serialization |
| Security | Microsoft.Identity.Client | 4.21.1 | Identity library (unused in app) |
| Bundling | Microsoft.AspNet.Web.Optimization | 1.1.3 | CSS/JS bundling and minification |
| Host | IIS / IIS Express | — | Web server |

### Data Storage & External Services

The application uses a single SQL Server LocalDB instance (`ContosoUniversityNoAuthEFCore`) for all persistent data, accessed through Entity Framework Core 3.1 via the `SchoolContext` DbContext. File uploads (teaching material images for courses) are stored on the local server file system under `~/Uploads/TeachingMaterials/`. Notifications are dispatched asynchronously via a Windows MSMQ private queue (`.\Private$\ContosoUniversityNotifications`) and are also persisted to the `Notification` table in the database for the notifications dashboard.

### Key Architectural Decisions

- **Table-per-Hierarchy (TPH) inheritance**: `Student` and `Instructor` both extend the abstract `Person` class and are stored in a single `Person` table with a `Discriminator` column.
- **Hybrid ORM**: Entity Framework Core 3.1 is used as the ORM within a legacy ASP.NET MVC 5 / .NET Framework 4.8 project — a non-standard combination that limits cross-platform portability.
- **MSMQ for notifications**: Application-level change notifications (CREATE/UPDATE/DELETE) are published to an MSMQ queue, making the notification subsystem Windows-only and incompatible with cloud hosting without replacement.

## Component Relationships

```mermaid
flowchart LR
    subgraph Presentation["Presentation"]
        HomeCtrl["HomeController"]
        StudentCtrl["StudentsController"]
        CourseCtrl["CoursesController"]
        DeptCtrl["DepartmentsController"]
        InstrCtrl["InstructorsController"]
        NotifCtrl["NotificationsController"]
        BaseCtrl["BaseController (abstract)"]
    end
    subgraph Business["Business / Services"]
        NotifSvc["NotificationService"]
        LogSvc["LoggingService"]
        DbInit["DbInitializer"]
    end
    subgraph DataAccess["Data Access"]
        SchoolCtx["SchoolContext (DbContext)"]
        CtxFactory["SchoolContextFactory"]
    end
    subgraph Models["Domain Models"]
        Person["Person (abstract)"]
        Student["Student"]
        Instructor["Instructor"]
        Course["Course"]
        Department["Department"]
        Enrollment["Enrollment"]
        OfficeAssign["OfficeAssignment"]
        CourseAssign["CourseAssignment"]
        Notification["Notification"]
    end

    StudentCtrl -->|"inherits"| BaseCtrl
    CourseCtrl -->|"inherits"| BaseCtrl
    DeptCtrl -->|"inherits"| BaseCtrl
    InstrCtrl -->|"inherits"| BaseCtrl
    NotifCtrl -->|"inherits"| BaseCtrl
    HomeCtrl -->|"inherits"| BaseCtrl
    BaseCtrl -->|"creates via factory"| CtxFactory
    CtxFactory -->|"instantiates"| SchoolCtx
    BaseCtrl -->|"holds"| NotifSvc
    NotifCtrl -->|"reads queue"| NotifSvc
    StudentCtrl -->|"queries"| SchoolCtx
    CourseCtrl -->|"queries"| SchoolCtx
    DeptCtrl -->|"queries"| SchoolCtx
    InstrCtrl -->|"queries"| SchoolCtx
    SchoolCtx -.->|"owns"| Person
    SchoolCtx -.->|"owns"| Course
    SchoolCtx -.->|"owns"| Department
    SchoolCtx -.->|"owns"| Enrollment
    SchoolCtx -.->|"owns"| Notification
    Student -.->|"extends"| Person
    Instructor -.->|"extends"| Person
```

### Component Inventory

| Component | Layer | Type | Responsibility |
|-----------|-------|------|----------------|
| BaseController | Presentation | Abstract MVC Controller | Provides shared `SchoolContext`, `NotificationService`, and `SendEntityNotification` helper |
| StudentsController | Presentation | MVC Controller | CRUD for students with pagination and search |
| CoursesController | Presentation | MVC Controller | CRUD for courses including file upload for teaching materials |
| DepartmentsController | Presentation | MVC Controller | CRUD for departments with instructor assignment |
| InstructorsController | Presentation | MVC Controller | CRUD for instructors with office assignment and course management |
| NotificationsController | Presentation | MVC Controller | JSON API for reading/managing notifications from MSMQ queue |
| HomeController | Presentation | MVC Controller | Dashboard showing enrollment statistics by date |
| SchoolContext | Data Access | EF Core DbContext | Defines all DbSets, model configuration (TPH, composite keys, relationships) |
| SchoolContextFactory | Data Access | Static Factory | Reads connection string from ConfigurationManager and creates SchoolContext |
| DbInitializer | Data Access | Static Utility | Seeds initial data (students, instructors, departments, courses) |
| NotificationService | Services | Service Class | Sends/receives notifications via MSMQ; serializes to JSON |
| LoggingService | Services | Service Class | Application-level logging wrapper |
| Person | Domain Model | Abstract Entity | Base class for Student and Instructor (TPH) |
| Student | Domain Model | Entity | Represents a student with enrollment date and enrollments |
| Instructor | Domain Model | Entity | Represents an instructor with hire date, office, and course assignments |
| Course | Domain Model | Entity | Academic course with title, credits, department, and teaching material |
| Department | Domain Model | Entity | Academic department with budget, start date, and administrator |
| Enrollment | Domain Model | Entity | Many-to-many join between Student and Course with optional grade |
| CourseAssignment | Domain Model | Entity | Many-to-many join between Course and Instructor |
| OfficeAssignment | Domain Model | Entity | One-to-one relationship with Instructor for office location |
| Notification | Domain Model | Entity | Persisted notification record with entity type, operation, and read status |
