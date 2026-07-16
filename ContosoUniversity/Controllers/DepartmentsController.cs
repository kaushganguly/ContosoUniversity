using System;
using System.Linq;
using ContosoUniversity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Controllers
{
    public class DepartmentsController : BaseController
    {
        public ActionResult Index()
        {
            var departments = db.Departments.Include(d => d.Administrator);
            return View(departments.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var department = db.Departments.Find(id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        public ActionResult Create()
        {
            ViewBag.InstructorID = new SelectList(db.Instructors, "ID", "FullName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Name,Budget,StartDate,InstructorID")] Department department)
        {
            if (ModelState.IsValid)
            {
                db.Departments.Add(department);
                db.SaveChanges();

                SendEntityNotification("Department", department.DepartmentID.ToString(), department.Name, EntityOperation.CREATE);

                return RedirectToAction("Index");
            }

            ViewBag.InstructorID = new SelectList(db.Instructors, "ID", "FullName", department.InstructorID);
            return View(department);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var department = db.Departments.Find(id);
            if (department == null)
            {
                return NotFound();
            }

            ViewBag.InstructorID = new SelectList(db.Instructors, "ID", "FullName", department.InstructorID);
            return View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind("DepartmentID,Name,Budget,StartDate,InstructorID,RowVersion")] Department department)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(department).State = EntityState.Modified;
                    db.SaveChanges();

                    SendEntityNotification("Department", department.DepartmentID.ToString(), department.Name, EntityOperation.UPDATE);

                    return RedirectToAction("Index");
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var entry = ex.Entries.Single();
                var clientValues = (Department)entry.Entity;
                var databaseEntry = entry.GetDatabaseValues();

                if (databaseEntry == null)
                {
                    ModelState.AddModelError(string.Empty, "Unable to save changes. The department was deleted by another user.");
                }
                else
                {
                    var databaseValues = (Department)databaseEntry.ToObject();

                    if (databaseValues.Name != clientValues.Name)
                        ModelState.AddModelError("Name", $"Current value: {databaseValues.Name}");
                    if (databaseValues.Budget != clientValues.Budget)
                        ModelState.AddModelError("Budget", $"Current value: {databaseValues.Budget:c}");
                    if (databaseValues.StartDate != clientValues.StartDate)
                        ModelState.AddModelError("StartDate", $"Current value: {databaseValues.StartDate:d}");
                    if (databaseValues.InstructorID != clientValues.InstructorID)
                    {
                        var instructor = db.Instructors.Find(databaseValues.InstructorID);
                        ModelState.AddModelError("InstructorID", $"Current value: {instructor?.FullName}");
                    }

                    ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                        + "was modified by another user after you got the original value. The "
                        + "edit operation was canceled and the current values in the database "
                        + "have been displayed. If you still want to edit this record, click "
                        + "the Save button again. Otherwise click the Back to List hyperlink.");

                    department.RowVersion = databaseValues.RowVersion;
                }
            }

            ViewBag.InstructorID = new SelectList(db.Instructors, "ID", "FullName", department.InstructorID);
            return View(department);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var department = db.Departments.Find(id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var department = db.Departments.Find(id);
            var departmentName = department.Name;
            db.Departments.Remove(department);
            db.SaveChanges();

            SendEntityNotification("Department", id.ToString(), departmentName, EntityOperation.DELETE);

            return RedirectToAction("Index");
        }
    }
}
