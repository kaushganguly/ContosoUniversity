using System;
using System.IO;
using ContosoUniversity.Data;
using ContosoUniversity.Models;
using ContosoUniversity.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace ContosoUniversity.Controllers
{
    public abstract class BaseController : Controller
    {
        protected SchoolContext db => HttpContext.RequestServices.GetRequiredService<SchoolContext>();
        protected NotificationService notificationService => HttpContext.RequestServices.GetRequiredService<NotificationService>();
        protected IWebHostEnvironment HostEnvironment => HttpContext.RequestServices.GetRequiredService<IWebHostEnvironment>();

        protected void SendEntityNotification(string entityType, string entityId, EntityOperation operation)
        {
            SendEntityNotification(entityType, entityId, null, operation);
        }

        protected void SendEntityNotification(string entityType, string entityId, string entityDisplayName, EntityOperation operation)
        {
            try
            {
                notificationService.SendNotification(entityType, entityId, entityDisplayName, operation, "System");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to send notification: {ex.Message}");
            }
        }

        protected string MapPath(string appRelativePath)
        {
            var relativePath = appRelativePath?.TrimStart('~', '/').Replace('/', Path.DirectorySeparatorChar)
                ?? string.Empty;
            return Path.Combine(HostEnvironment.WebRootPath, relativePath);
        }
    }
}
