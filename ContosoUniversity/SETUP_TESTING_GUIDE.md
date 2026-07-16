# Setup and Testing Guide for Notification System

## Prerequisites

1. Install the .NET 10 SDK.
2. Ensure the repository has been restored with `dotnet restore`.

## Building the Project

1. Open the solution in your preferred IDE or terminal.
2. Build the solution with `dotnet build`.

## Testing the Notification System

### Step 1: Run the Application
1. Start the app with `dotnet run`.
2. Open the displayed local URL in your browser.

### Step 2: Access Notification Dashboard
1. Click **Notifications** in the main navigation menu.
2. This page explains the notification system and provides test links.

### Step 3: Test Notifications
1. **Create a Student**:
   - Click **Students** → **Create New**
   - Fill in the form and submit
   - Watch for a green notification in the top-right corner

2. **Edit a Student**:
   - Go to the Students list and click **Edit** on any student
   - Make changes and save
   - Watch for a blue notification

3. **Delete a Student**:
   - Go to the Students list and click **Delete** on any student
   - Confirm deletion
   - Watch for an orange notification

4. **Test Other Entities**:
   - Repeat the same process for Courses, Instructors, and Departments
   - Each operation should trigger appropriate notifications

## Troubleshooting

### No Notifications Appearing
1. **Check Browser Console**: Press F12 and look for JavaScript errors
2. **Check Network Tab**: Verify calls to `/Notifications/GetNotifications` are happening
3. **Check App Logs**: Review the application output for any runtime exceptions

### JavaScript Not Loading
1. **File Paths**: Verify `notifications.js` and `notifications.css` files exist
2. **Browser Cache**: Clear cache and refresh

## Configuration Notes

- **Queue Path**: Configured in `appsettings.json` as `InMemory`
- **Polling Interval**: JavaScript checks for new notifications every 5 seconds
- **Auto-dismiss**: Notifications automatically disappear after 1 minute (60 seconds)
- **Max Notifications**: Maximum of 5 notifications shown simultaneously

## Production Considerations

1. Replace the in-memory queue with a durable shared notification mechanism if cross-instance delivery is required
2. Add persistence if notifications must survive app restarts
3. Use a shared backing service when running multiple application instances

## Development Tips

- Notifications are designed to be non-blocking - in-memory queue issues won't break main operations
- Debug output shows notification send/receive operations
- Use the notification dashboard to understand system behavior
