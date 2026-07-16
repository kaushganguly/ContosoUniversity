# Real-Time Admin Notification System

This project includes a real-time notification system that alerts administrators whenever entity operations (create, update, delete) are performed in the system.

## Overview

The notification system now uses an in-memory queue so it can run cross-platform with the ASP.NET Core application.

## Features

- **Real-time notifications**: Admins receive immediate notifications when entities are modified
- **Entity coverage**: Monitors Students, Courses, Instructors, and Departments
- **Operation tracking**: Tracks CREATE, UPDATE, and DELETE operations
- **Non-intrusive UI**: Notifications appear in the top-right corner with auto-dismiss
- **Cross-platform**: No Windows-only queue dependency is required

## How It Works

### Backend Components

1. **NotificationService**: Handles in-memory notification delivery
2. **BaseController**: Base class that all controllers inherit from to send notifications
3. **Notification Model**: Entity to represent notification data
4. **NotificationsController**: API endpoints for retrieving notifications

### Frontend Components

1. **notifications.css**: Styling for notification UI elements
2. **notifications.js**: JavaScript polling system that checks for new notifications
3. **Layout integration**: Includes notification assets in the shared layout

### Technology Stack

- **ASP.NET Core MVC**: Web framework
- **Entity Framework Core**: Data access
- **SQLite**: Local development database
- **JavaScript/jQuery**: Frontend polling and UI updates

## Configuration

The notification system is configured in `appsettings.json`:

```json
{
  "NotificationQueuePath": "InMemory"
}
```

## Queue Details

- **Queue Type**: In-memory queue
- **Message Format**: Notification objects stored in process memory
- **Behavior**: Notifications remain lightweight and non-blocking for CRUD operations

## Usage

### For Administrators

1. Navigate to **Notifications** in the main menu to view the dashboard
2. Perform any CRUD operation on entities (Students, Courses, Instructors, Departments)
3. Watch for notifications appearing in the top-right corner
4. Notifications auto-dismiss after 1 minute or can be manually closed

### For Developers

To add notification support to a new controller:

1. Inherit from `BaseController` instead of `Controller`
2. Use the shared `db` context exposed by the base class
3. Call `SendEntityNotification()` after successful save operations

```csharp
// Example: After creating a student
db.Students.Add(student);
db.SaveChanges();
SendEntityNotification("Student", student.ID.ToString(), EntityOperation.CREATE);
```

## Testing the System

1. Access the **Notifications** dashboard from the main menu
2. Click on any of the "Create new..." buttons provided
3. Complete a create/edit/delete operation
4. Observe the notification appearing in the top-right corner
