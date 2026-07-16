# Core Business Workflows

The application manages a university domain where staff maintain students, courses, instructors, and departments, while receiving operational notifications on entity changes.

## Domain Entities

| Entity | Service / Bounded Context | Description | Key Relationships |
|---|---|---|---|
| Student | Academic Administration | Learner profile and enrollment ownership | Inherits Person; linked to Enrollment |
| Instructor | Academic Administration | Faculty profile responsible for courses/departments | Inherits Person; linked to CourseAssignment and OfficeAssignment |
| Course | Curriculum Management | Course catalog record with credits and department | Belongs to Department; linked to Enrollments and Instructors |
| Department | Organization Management | Academic unit with budget and administrator | Owns Courses; references Instructor admin |
| Enrollment | Academic Administration | Student-course participation and grade outcome | Joins Student and Course |
| Notification | Operational Monitoring | Event message for entity create/update/delete | Produced by CRUD workflows and consumed by notification dashboard |

## Service-to-Domain Mapping

| Service | Domain Context | Owned Entities | External Dependencies |
|---|---|---|---|
| MVC Controllers (Students/Courses/Instructors/Departments) | Academic Administration | Student, Instructor, Course, Department, Enrollment | SchoolContext / SQL Server |
| NotificationsController + NotificationService | Operational Monitoring | Notification | MSMQ + Newtonsoft.Json |

## Primary Workflows

### Workflow 1: Manage Student Record

Staff opens the student list, filters/sorts/paginates, and then creates or edits a student. Controller validation ensures required name/date constraints before persisting via DbContext. On successful create/update/delete, the base controller publishes an entity-operation notification.

### Workflow 2: Maintain Course and Teaching Material

Staff creates or edits a course including title/credits/department and optional teaching material image upload. The course is validated, saved, and linked to its department. Entity-change notifications are emitted for downstream operational visibility.

### Workflow 3: Assign Instructors and Department Administration

Staff creates/updates instructors, assigns courses, and manages department administrators. The workflow updates join relationships (`CourseAssignment`) and handles office assignment details. Department edits include row-version concurrency behavior to protect against conflicting updates.

## Cross-Service Data Flows

The app is mostly in-process, but operational notifications cross a boundary via MSMQ. Academic CRUD controllers write to SQL and then enqueue notification payloads. The notifications dashboard polls queue-backed data and returns JSON for UI display; if queue read fails, the API returns an error payload without blocking primary academic transactions.

## Business Workflow Sequence

```mermaid
sequenceDiagram
    participant Staff as "Staff User"
    participant Ctrl as "Academic Controller"
    participant Db as "SchoolContext"
    participant Base as "BaseController"
    participant Notif as "NotificationService"
    participant Queue as "MSMQ"
    participant Dash as "NotificationsController"

    Staff->>Ctrl: Submit create or update form
    Ctrl->>Ctrl: Validate business rules
    Ctrl->>Db: Save entity and relationships
    Db-->>Ctrl: Commit successful
    Ctrl->>Base: Trigger entity notification
    Base->>Notif: SendNotification(entity operation)
    Notif->>Queue: Enqueue notification
    Ctrl-->>Staff: Redirect with updated data

    Staff->>Dash: Open notifications dashboard
    Dash->>Notif: Receive pending notifications
    alt Queue available
        Notif->>Queue: Dequeue up to 10 messages
        Queue-->>Notif: Notification payloads
        Notif-->>Dash: Notification list
        Dash-->>Staff: JSON success with notifications
    else Queue unavailable
        Dash-->>Staff: JSON error message
    end
```

## Business Rules & Decision Logic

- Validation rules: course title length, credit range, required names, and date ranges for enrollment/hire dates.
- Decision logic: controllers branch on model validity and existence checks (id null/not found) before persisting or returning error views.
- State transitions: notifications progress from unread to read state conceptually; academic entities follow CRUD lifecycle changes.
- Data integrity: instructor-course assignments and department administration links are maintained through relationship updates; department edits apply row-version concurrency checks.
