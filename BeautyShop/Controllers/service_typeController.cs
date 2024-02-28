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
    public class service_typeController : Controller
    {
        private BeautyDataEntities db = new BeautyDataEntities();

        // GET: service_type
        public ActionResult Index()
        {
            return View(db.service_type.ToList());
        }

        // GET: service_type/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            service_type service_type = db.service_type.Find(id);
            if (service_type == null)
            {
                return HttpNotFound();
            }
            return View(service_type);
        }

        // GET: service_type/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: service_type/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_type,type_name")] service_type service_type)
        {
            if (ModelState.IsValid)
            {
                db.service_type.Add(service_type);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(service_type);
        }

        // GET: service_type/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            service_type service_type = db.service_type.Find(id);
            if (service_type == null)
            {
                return HttpNotFound();
            }
            return View(service_type);
        }

        // POST: service_type/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_type,type_name")] service_type service_type)
        {
            if (ModelState.IsValid)
            {
                db.Entry(service_type).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(service_type);
        }

        // GET: service_type/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            service_type service_type = db.service_type.Find(id);
            if (service_type == null)
            {
                return HttpNotFound();
            }
            return View(service_type);
        }

        // POST: service_type/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            service_type service_type = db.service_type.Find(id);
            db.service_type.Remove(service_type);
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
