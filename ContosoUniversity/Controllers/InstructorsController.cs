using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContosoUniversity.Models;
using ContosoUniversity.Models.SchoolViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Controllers
{
    public class InstructorsController : BaseController
    {
        public ActionResult Index(int? id, int? courseID)
        {
            var viewModel = new InstructorIndexData();
            viewModel.Instructors = db.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.CourseAssignments)
                    .ThenInclude(c => c.Course)
                        .ThenInclude(d => d.Department)
                .OrderBy(i => i.LastName);

            if (id != null)
            {
                ViewBag.InstructorID = id.Value;
                viewModel.Courses = viewModel.Instructors.Where(
                    i => i.ID == id.Value).Single().CourseAssignments.Select(s => s.Course);
            }

            if (courseID != null)
            {
                ViewBag.CourseID = courseID.Value;
                viewModel.Enrollments = viewModel.Courses.Where(
                    x => x.CourseID == courseID).Single().Enrollments;
            }

            return View(viewModel);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var instructor = db.Instructors.Find(id);
            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        public ActionResult Create()
        {
            var instructor = new Instructor
            {
                CourseAssignments = new List<CourseAssignment>()
            };
            PopulateAssignedCourseData(instructor);
            return View(instructor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("LastName,FirstMidName,HireDate,OfficeAssignment")] Instructor instructor, string[] selectedCourses)
        {
            if (selectedCourses != null)
            {
                instructor.CourseAssignments = new List<CourseAssignment>();
                foreach (var course in selectedCourses)
                {
                    instructor.CourseAssignments.Add(new CourseAssignment { InstructorID = instructor.ID, CourseID = int.Parse(course) });
                }
            }

            if (ModelState.IsValid)
            {
                db.Instructors.Add(instructor);
                db.SaveChanges();

                SendEntityNotification("Instructor", instructor.ID.ToString(), EntityOperation.CREATE);

                return RedirectToAction("Index");
            }

            PopulateAssignedCourseData(instructor);
            return View(instructor);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var instructor = db.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.CourseAssignments)
                    .ThenInclude(c => c.Course)
                .SingleOrDefault(i => i.ID == id);

            if (instructor == null)
            {
                return NotFound();
            }

            PopulateAssignedCourseData(instructor);
            return View(instructor);
        }

        private void PopulateAssignedCourseData(Instructor instructor)
        {
            var allCourses = db.Courses;
            var instructorCourses = new HashSet<int>(instructor.CourseAssignments.Select(c => c.CourseID));
            var viewModel = new List<AssignedCourseData>();
            foreach (var course in allCourses)
            {
                viewModel.Add(new AssignedCourseData
                {
                    CourseID = course.CourseID,
                    Title = course.Title,
                    Assigned = instructorCourses.Contains(course.CourseID)
                });
            }
            ViewBag.Courses = viewModel;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int? id, string[] selectedCourses)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var instructorToUpdate = db.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.CourseAssignments)
                    .ThenInclude(c => c.Course)
                .SingleOrDefault(i => i.ID == id);

            if (instructorToUpdate == null)
            {
                return NotFound();
            }

            instructorToUpdate.OfficeAssignment ??= new OfficeAssignment();

            if (await TryUpdateModelAsync(instructorToUpdate, "", i => i.LastName, i => i.FirstMidName, i => i.HireDate, i => i.OfficeAssignment))
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(instructorToUpdate.OfficeAssignment?.Location))
                    {
                        instructorToUpdate.OfficeAssignment = null;
                    }

                    UpdateInstructorCourses(selectedCourses, instructorToUpdate);

                    db.SaveChanges();

                    SendEntityNotification("Instructor", instructorToUpdate.ID.ToString(), EntityOperation.UPDATE);

                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }

            PopulateAssignedCourseData(instructorToUpdate);
            return View(instructorToUpdate);
        }

        private void UpdateInstructorCourses(string[] selectedCourses, Instructor instructorToUpdate)
        {
            if (selectedCourses == null)
            {
                instructorToUpdate.CourseAssignments = new List<CourseAssignment>();
                return;
            }

            var selectedCoursesHS = new HashSet<string>(selectedCourses);
            var instructorCourses = new HashSet<int>(instructorToUpdate.CourseAssignments.Select(c => c.Course.CourseID));
            foreach (var course in db.Courses)
            {
                if (selectedCoursesHS.Contains(course.CourseID.ToString()))
                {
                    if (!instructorCourses.Contains(course.CourseID))
                    {
                        instructorToUpdate.CourseAssignments.Add(new CourseAssignment { InstructorID = instructorToUpdate.ID, CourseID = course.CourseID });
                    }
                }
                else if (instructorCourses.Contains(course.CourseID))
                {
                    var courseToRemove = instructorToUpdate.CourseAssignments.SingleOrDefault(i => i.CourseID == course.CourseID);
                    db.Entry(courseToRemove).State = EntityState.Deleted;
                }
            }
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var instructor = db.Instructors.Find(id);
            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var instructor = db.Instructors
                .Include(i => i.OfficeAssignment)
                .Single(i => i.ID == id);

            db.Instructors.Remove(instructor);

            var department = db.Departments.SingleOrDefault(d => d.InstructorID == id);
            if (department != null)
            {
                department.InstructorID = null;
            }

            db.SaveChanges();

            SendEntityNotification("Instructor", id.ToString(), EntityOperation.DELETE);

            return RedirectToAction("Index");
        }
    }
}
