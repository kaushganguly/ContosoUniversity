using Microsoft.AspNetCore.Mvc;

namespace ContosoUniversity.Controllers
{
    public class NotificationsController : BaseController
    {
        [HttpGet]
        public JsonResult GetNotifications()
        {
            try
            {
                var notifications = notificationService.GetUnreadNotifications(10);

                return Json(new
                {
                    success = true,
                    notifications,
                    count = notifications.Count
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error retrieving notifications: {ex.Message}");
                return Json(new { success = false, message = "Error retrieving notifications" });
            }
        }

        [HttpPost]
        public JsonResult MarkAsRead(int id)
        {
            try
            {
                notificationService.MarkAsRead(id);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error marking notification as read: {ex.Message}");
                return Json(new { success = false, message = "Error updating notification" });
            }
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
