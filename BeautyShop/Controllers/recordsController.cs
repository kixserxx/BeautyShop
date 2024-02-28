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
    public class recordsController : Controller
    {
        private BeautyDataEntities db = new BeautyDataEntities();

        // GET: records
        public ActionResult Index()
        {
            var records = db.records.Include(r => r.user).Include(r => r.user1).Include(r => r.sale).Include(r => r.service_work);
            return View(records.ToList());
        }

        // GET: records/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            record record = db.records.Find(id);
            if (record == null)
            {
                return HttpNotFound();
            }
            return View(record);
        }

        // GET: records/Create
        public ActionResult Create()
        {
            ViewBag.client_id = new SelectList(db.users, "id_user", "fio");
            ViewBag.master_id = new SelectList(db.users, "id_user", "fio");
            ViewBag.sale_id = new SelectList(db.sales, "id_sale", "sale_name");
            ViewBag.service_id = new SelectList(db.service_work, "id_service", "service_name");
            return View();
        }

        // POST: records/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_record,client_id,master_id,date_record,service_id,sale_id,record_price")] record record)
        {
            if (ModelState.IsValid)
            {
                db.records.Add(record);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.client_id = new SelectList(db.users, "id_user", "fio", record.client_id);
            ViewBag.master_id = new SelectList(db.users, "id_user", "fio", record.master_id);
            ViewBag.sale_id = new SelectList(db.sales, "id_sale", "sale_name", record.sale_id);
            ViewBag.service_id = new SelectList(db.service_work, "id_service", "service_name", record.service_id);
            return View(record);
        }

        // GET: records/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            record record = db.records.Find(id);
            if (record == null)
            {
                return HttpNotFound();
            }
            ViewBag.client_id = new SelectList(db.users, "id_user", "fio", record.client_id);
            ViewBag.master_id = new SelectList(db.users, "id_user", "fio", record.master_id);
            ViewBag.sale_id = new SelectList(db.sales, "id_sale", "sale_name", record.sale_id);
            ViewBag.service_id = new SelectList(db.service_work, "id_service", "service_name", record.service_id);
            return View(record);
        }

        // POST: records/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_record,client_id,master_id,date_record,service_id,sale_id,record_price")] record record)
        {
            if (ModelState.IsValid)
            {
                db.Entry(record).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.client_id = new SelectList(db.users, "id_user", "fio", record.client_id);
            ViewBag.master_id = new SelectList(db.users, "id_user", "fio", record.master_id);
            ViewBag.sale_id = new SelectList(db.sales, "id_sale", "sale_name", record.sale_id);
            ViewBag.service_id = new SelectList(db.service_work, "id_service", "service_name", record.service_id);
            return View(record);
        }

        // GET: records/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            record record = db.records.Find(id);
            if (record == null)
            {
                return HttpNotFound();
            }
            return View(record);
        }

        // POST: records/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            record record = db.records.Find(id);
            db.records.Remove(record);
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
