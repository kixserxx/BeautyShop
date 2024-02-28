using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BeautyShop.Models;

namespace BeautyShop.Controllers
{
    public class user_roleController : Controller
    {
        private BeautyDataEntities db = new BeautyDataEntities();

        // GET: user_role
        public ActionResult Index()
        {
            return View(db.user_role.ToList());
        }

        // GET: user_role/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            user_role user_role = db.user_role.Find(id);
            if (user_role == null)
            {
                return HttpNotFound();
            }
            return View(user_role);
        }

        // GET: user_role/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: user_role/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_role,role_name")] user_role user_role)
        {
            if (ModelState.IsValid)
            {
                db.user_role.Add(user_role);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user_role);
        }

        // GET: user_role/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            user_role user_role = db.user_role.Find(id);
            if (user_role == null)
            {
                return HttpNotFound();
            }
            return View(user_role);
        }

        // POST: user_role/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_role,role_name")] user_role user_role)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user_role).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user_role);
        }

        // GET: user_role/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            user_role user_role = db.user_role.Find(id);
            if (user_role == null)
            {
                return HttpNotFound();
            }
            return View(user_role);
        }

        // POST: user_role/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            user_role user_role = db.user_role.Find(id);
            db.user_role.Remove(user_role);
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
