using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CSharpBotAdmin.Storage;

namespace CSharpBotAdmin.Controllers
{
    public class StudentsController : Controller
    {
        /// <summary>
        /// База данных
        /// </summary>
        private CSHARPBOTEntities db = new CSHARPBOTEntities();

        /// <summary>
        /// Список студентов
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(Models.SortOrder order)
        {
            // if ((email == null) ? email : false)
            if (order != null)
            {
                ViewData["Order"] = order;
                if (order.Descent)
                {
                    return View(db.Students.OrderByDescending(a => a.EMail).ToList());
                }
                else
                {
                    return View(db.Students.OrderBy(a => a.EMail).ToList());
                }
            }
            ViewData["Order"] = new Models.SortOrder();
            return View(db.Students.OrderBy(a=> a.Name).ToList());
        }

        // GET: Students/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Students students = db.Students.Find(id);
            if (students == null)
            {
                return HttpNotFound();
            }
            return View(students);
        }

        /// <summary>
        /// Форма для добавления записи
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserID,UserName,Name,FamilyName,EMail,PhoneNumber")] Students students)
        {
            if (ModelState.IsValid)
            {
                students.ID = Guid.NewGuid();
                db.Students.Add(students);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(students);
        }

        /// <summary>
        /// Форма редактирования записи
        /// GET: Students/Edit/5
        /// </summary>
        /// <param name="id">Идентификатор записи</param>
        /// <returns></returns>
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Students students = db.Students.Find(id);
            if (students == null)
            {
                return HttpNotFound();
            }
            return View(students);
        }

        /// <summary>
        /// Выполнение редактирования записи
        /// POST: Students/Edit/5
        /// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        /// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        /// </summary>
        /// <param name="students"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserID,UserName,Name,FamilyName,EMail,ID,PhoneNumber")]Students students)
        {
            if (ModelState.IsValid)
            {
                db.Entry(students).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(students);
        }

        // GET: Students/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Students students = db.Students.Find(id);
            if (students == null)
            {
                return HttpNotFound();
            }
            return View(students);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Students students = db.Students.Find(id);
            db.Students.Remove(students);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
