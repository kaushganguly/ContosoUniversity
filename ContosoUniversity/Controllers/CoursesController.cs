using System;
using System.IO;
using System.Linq;
using ContosoUniversity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Controllers
{
    public class CoursesController : BaseController
    {
        public ActionResult Index()
        {
            var courses = db.Courses.Include(c => c.Department);
            return View(courses.ToList());
        }

        public ActionResult Details(int? id)
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

        public ActionResult Create()
        {
            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "Name");
            return View(new Course());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("CourseID,Title,Credits,DepartmentID,TeachingMaterialImagePath")] Course course, IFormFile teachingMaterialImage)
        {
            if (ModelState.IsValid)
            {
                if (teachingMaterialImage != null && teachingMaterialImage.Length > 0)
                {
                    if (!TrySaveTeachingMaterial(course, teachingMaterialImage))
                    {
                        ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "Name", course.DepartmentID);
                        return View(course);
                    }
                }

                db.Courses.Add(course);
                db.SaveChanges();

                SendEntityNotification("Course", course.CourseID.ToString(), course.Title, EntityOperation.CREATE);

                return RedirectToAction("Index");
            }

            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "Name", course.DepartmentID);
            return View(course);
        }

        public ActionResult Edit(int? id)
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
        public ActionResult Edit([Bind("CourseID,Title,Credits,DepartmentID,TeachingMaterialImagePath")] Course course, IFormFile teachingMaterialImage)
        {
            if (ModelState.IsValid)
            {
                var existingCourse = db.Courses.AsNoTracking().SingleOrDefault(c => c.CourseID == course.CourseID);
                if (existingCourse == null)
                {
                    return NotFound();
                }

                course.TeachingMaterialImagePath = existingCourse.TeachingMaterialImagePath;

                if (teachingMaterialImage != null && teachingMaterialImage.Length > 0)
                {
                    if (!TrySaveTeachingMaterial(course, teachingMaterialImage, existingCourse.TeachingMaterialImagePath))
                    {
                        ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "Name", course.DepartmentID);
                        return View(course);
                    }
                }

                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();

                SendEntityNotification("Course", course.CourseID.ToString(), course.Title, EntityOperation.UPDATE);

                return RedirectToAction("Index");
            }

            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "Name", course.DepartmentID);
            return View(course);
        }

        public ActionResult Delete(int? id)
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
        public ActionResult DeleteConfirmed(int id)
        {
            var course = db.Courses.Find(id);
            var courseTitle = course.Title;

            DeleteTeachingMaterial(course.TeachingMaterialImagePath);

            db.Courses.Remove(course);
            db.SaveChanges();

            SendEntityNotification("Course", id.ToString(), courseTitle, EntityOperation.DELETE);

            return RedirectToAction("Index");
        }

        private bool TrySaveTeachingMaterial(Course course, IFormFile teachingMaterialImage, string existingPath = null)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
            var fileExtension = Path.GetExtension(teachingMaterialImage.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(fileExtension))
            {
                ModelState.AddModelError("teachingMaterialImage", "Please upload a valid image file (jpg, jpeg, png, gif, bmp).");
                return false;
            }

            if (teachingMaterialImage.Length > 5 * 1024 * 1024)
            {
                ModelState.AddModelError("teachingMaterialImage", "File size must be less than 5MB.");
                return false;
            }

            try
            {
                var uploadsPath = MapPath("~/Uploads/TeachingMaterials/");
                Directory.CreateDirectory(uploadsPath);

                var fileName = $"course_{course.CourseID}_{Guid.NewGuid()}{fileExtension}";
                var filePath = Path.Combine(uploadsPath, fileName);

                DeleteTeachingMaterial(existingPath);

                using (var fileStream = System.IO.File.Create(filePath))
                {
                    teachingMaterialImage.CopyTo(fileStream);
                }

                course.TeachingMaterialImagePath = $"~/Uploads/TeachingMaterials/{fileName}";
                return true;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("teachingMaterialImage", "Error uploading file: " + ex.Message);
                return false;
            }
        }

        private void DeleteTeachingMaterial(string teachingMaterialImagePath)
        {
            if (string.IsNullOrEmpty(teachingMaterialImagePath))
            {
                return;
            }

            var filePath = MapPath(teachingMaterialImagePath);
            if (!System.IO.File.Exists(filePath))
            {
                return;
            }

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
}
