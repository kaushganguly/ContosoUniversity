using System;
using System.Diagnostics;
using System.Linq;
using ContosoUniversity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Controllers
{
    public class StudentsController : BaseController
    {
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var students = from s in db.Students
                           select s;

            if (!string.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.LastName.Contains(searchString)
                                       || s.FirstMidName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    students = students.OrderByDescending(s => s.LastName);
                    break;
                case "Date":
                    students = students.OrderBy(s => s.EnrollmentDate);
                    break;
                case "date_desc":
                    students = students.OrderByDescending(s => s.EnrollmentDate);
                    break;
                default:
                    students = students.OrderBy(s => s.LastName);
                    break;
            }

            const int pageSize = 10;
            var pageNumber = page ?? 1;
            return View(PaginatedList<Student>.Create(students, pageNumber, pageSize));
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var student = db.Students
                .Include(s => s.Enrollments)
                    .ThenInclude(e => e.Course)
                .SingleOrDefault(s => s.ID == id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        public ActionResult Create()
        {
            return View(new Student
            {
                EnrollmentDate = DateTime.Today
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("LastName,FirstMidName,EnrollmentDate")] Student student)
        {
            try
            {
                if (student.EnrollmentDate == DateTime.MinValue || student.EnrollmentDate == default(DateTime))
                {
                    ModelState.AddModelError("EnrollmentDate", "Please enter a valid enrollment date.");
                }

                if (student.EnrollmentDate < new DateTime(1753, 1, 1) || student.EnrollmentDate > new DateTime(9999, 12, 31))
                {
                    ModelState.AddModelError("EnrollmentDate", "Enrollment date must be between 1753 and 9999.");
                }

                if (ModelState.IsValid)
                {
                    db.Students.Add(student);
                    db.SaveChanges();

                    var studentName = $"{student.FirstMidName} {student.LastName}";
                    SendEntityNotification("Student", student.ID.ToString(), studentName, EntityOperation.CREATE);

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError($"Error creating student: {ex.Message} | Student: {student?.FirstMidName} {student?.LastName} | EnrollmentDate: {student?.EnrollmentDate} | Stack: {ex.StackTrace}");
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            return View(student);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var student = db.Students.Find(id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind("ID,LastName,FirstMidName,EnrollmentDate")] Student student)
        {
            try
            {
                if (student.EnrollmentDate == DateTime.MinValue || student.EnrollmentDate == default(DateTime))
                {
                    ModelState.AddModelError("EnrollmentDate", "Please enter a valid enrollment date.");
                }

                if (student.EnrollmentDate < new DateTime(1753, 1, 1) || student.EnrollmentDate > new DateTime(9999, 12, 31))
                {
                    ModelState.AddModelError("EnrollmentDate", "Enrollment date must be between 1753 and 9999.");
                }

                if (ModelState.IsValid)
                {
                    db.Entry(student).State = EntityState.Modified;
                    db.SaveChanges();

                    var studentName = $"{student.FirstMidName} {student.LastName}";
                    SendEntityNotification("Student", student.ID.ToString(), studentName, EntityOperation.UPDATE);

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError($"Error editing student: {ex.Message} | Student ID: {student?.ID} | Student: {student?.FirstMidName} {student?.LastName} | EnrollmentDate: {student?.EnrollmentDate} | Stack: {ex.StackTrace}");
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            return View(student);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var student = db.Students.Find(id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var student = db.Students.Find(id);
                var studentName = $"{student.FirstMidName} {student.LastName}";
                db.Students.Remove(student);
                db.SaveChanges();

                SendEntityNotification("Student", id.ToString(), studentName, EntityOperation.DELETE);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Trace.TraceError($"Error deleting student: {ex.Message} | Student ID: {id} | Stack: {ex.StackTrace}");
                TempData["ErrorMessage"] = "Unable to delete the student. Try again, and if the problem persists see your system administrator.";
                return RedirectToAction("Index");
            }
        }
    }
}
