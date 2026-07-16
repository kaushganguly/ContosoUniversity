using ContosoUniversity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Controllers
{
    public class CoursesController : BaseController
    {
        public IActionResult Index()
        {
            var courses = db.Courses.Include(c => c.Department);
            return View(courses.ToList());
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var course = db.Courses.Include(c => c.Department).SingleOrDefault(c => c.CourseID == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        public IActionResult Create()
        {
            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "Name");
            return View(new Course());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("CourseID,Title,Credits,DepartmentID,TeachingMaterialImagePath")] Course course, IFormFile teachingMaterialImage)
        {
            if (ModelState.IsValid)
            {
                if (!TrySaveTeachingMaterial(course, teachingMaterialImage))
                {
                    ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "Name", course.DepartmentID);
                    return View(course);
                }

                db.Courses.Add(course);
                db.SaveChanges();
                SendEntityNotification("Course", course.CourseID.ToString(), course.Title, EntityOperation.CREATE);
                return RedirectToAction("Index");
            }

            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "Name", course.DepartmentID);
            return View(course);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var course = db.Courses.Find(id);
            if (course == null)
            {
                return NotFound();
            }

            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "Name", course.DepartmentID);
            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([Bind("CourseID,Title,Credits,DepartmentID,TeachingMaterialImagePath")] Course course, IFormFile teachingMaterialImage)
        {
            if (ModelState.IsValid)
            {
                if (!TrySaveTeachingMaterial(course, teachingMaterialImage, deleteOld: true))
                {
                    ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "Name", course.DepartmentID);
                    return View(course);
                }

                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();
                SendEntityNotification("Course", course.CourseID.ToString(), course.Title, EntityOperation.UPDATE);
                return RedirectToAction("Index");
            }

            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "Name", course.DepartmentID);
            return View(course);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var course = db.Courses.Include(c => c.Department).SingleOrDefault(c => c.CourseID == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var course = db.Courses.Find(id);
            if (course == null)
            {
                return NotFound();
            }

            var courseTitle = course.Title;

            if (!string.IsNullOrEmpty(course.TeachingMaterialImagePath))
            {
                var filePath = MapContentPath(course.TeachingMaterialImagePath);
                if (System.IO.File.Exists(filePath))
                {
                    try
                    {
                        System.IO.File.Delete(filePath);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error deleting file: {ex.Message}");
                    }
                }
            }

            db.Courses.Remove(course);
            db.SaveChanges();
            SendEntityNotification("Course", id.ToString(), courseTitle, EntityOperation.DELETE);

            return RedirectToAction("Index");
        }

        private bool TrySaveTeachingMaterial(Course course, IFormFile teachingMaterialImage, bool deleteOld = false)
        {
            if (teachingMaterialImage == null || teachingMaterialImage.Length == 0)
            {
                return true;
            }

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
            var allowedContentTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "image/jpeg",
                "image/png",
                "image/gif",
                "image/bmp",
                "image/pjpeg"
            };
            var fileExtension = Path.GetExtension(teachingMaterialImage.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(fileExtension))
            {
                ModelState.AddModelError("teachingMaterialImage", "Please upload a valid image file (jpg, jpeg, png, gif, bmp).");
                return false;
            }

            if (!string.IsNullOrWhiteSpace(teachingMaterialImage.ContentType) && !allowedContentTypes.Contains(teachingMaterialImage.ContentType))
            {
                ModelState.AddModelError("teachingMaterialImage", "Uploaded file content type is not supported.");
                return false;
            }

            if (teachingMaterialImage.Length > 5 * 1024 * 1024)
            {
                ModelState.AddModelError("teachingMaterialImage", "File size must be less than 5MB.");
                return false;
            }

            try
            {
                var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "TeachingMaterials");
                Directory.CreateDirectory(uploadsPath);

                var fileName = $"course_{course.CourseID}_{Guid.NewGuid()}{fileExtension}";
                var filePath = Path.Combine(uploadsPath, fileName);

                if (deleteOld && !string.IsNullOrEmpty(course.TeachingMaterialImagePath))
                {
                    var oldFilePath = MapContentPath(course.TeachingMaterialImagePath);
                    if (!string.IsNullOrEmpty(oldFilePath) && System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                using var stream = new FileStream(filePath, FileMode.Create);
                teachingMaterialImage.CopyTo(stream);
                course.TeachingMaterialImagePath = $"/Uploads/TeachingMaterials/{fileName}";

                return true;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("teachingMaterialImage", "Error uploading file: " + ex.Message);
                return false;
            }
        }

        private static string MapContentPath(string appRelativePath)
        {
            var fileName = Path.GetFileName(appRelativePath);
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return string.Empty;
            }

            var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "TeachingMaterials");
            return Path.Combine(uploadsPath, fileName);
        }
    }
}
