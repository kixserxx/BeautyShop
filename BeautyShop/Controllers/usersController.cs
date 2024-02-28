using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using BeautyShop.Models;

namespace BeautyShop.Controllers
{
    public class usersController : Controller
    {
        private BeautyDataEntities db = new BeautyDataEntities();

        // GET: users
        public ActionResult Index(string sortOrder, string sortColumn, string searchString)
        {
            if (Session["id_user"] != null)
            {
                var users = db.users.Include(u => u.user_role);
                bool ascending = sortOrder == "asc";
                if (!String.IsNullOrEmpty(searchString))
                {
                    users = users.Where(s => s.fio.Contains(searchString)
                                          || s.user_role.role_name.Contains(searchString)
                                          || s.user_email.Contains(searchString));
                }
                switch (sortColumn)
                {
                    case "fio":
                        users = ascending ? users.OrderBy(b => b.fio) : users.OrderByDescending(b => b.fio);
                        break;
                    case "user_role":
                        users = ascending ? users.OrderBy(b => b.user_role.role_name) : users.OrderByDescending(b => b.user_role.role_name);
                        break;
                    case "user_email":
                        users = ascending ? users.OrderBy(b => b.user_email) : users.OrderByDescending(b => b.user_email);
                        break;
                    default:
                        users = users.OrderBy(b => b.fio);
                        break;
                }
                ViewData["SortOrder"] = sortOrder;
                ViewData["SortColumn"] = sortColumn;
                return View(users.ToList());
            }
            else
            {
                return RedirectToAction("../Home/Login");
            }
        }

        // GET: users/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["id_user"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                user user = db.users.Find(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                return View(user);
            }
            else
            {
                return RedirectToAction("../Home/Login");
            }
        }

        // GET: users/Create
        public ActionResult Create()
        {
            if (Session["id_user"] != null)
            {
                ViewBag.role_id = new SelectList(db.user_role, "id_role", "role_name");
                return View();
            }
            else
            {
                return RedirectToAction("../Home/Login");
            }
        }

        // POST: users/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_user,img,fio,role_id,user_phone,user_email,user_password")] user user, HttpPostedFileBase upload)
        {
            if (Session["id_user"] != null)
            {
                if (ModelState.IsValid)
                {
                    if (upload != null && upload.ContentLength > 0)
                    {
                        using (var reader = new System.IO.BinaryReader(upload.InputStream))
                        {
                            user.img = reader.ReadBytes(upload.ContentLength);
                        }
                    }
                    db.users.Add(user);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.role_id = new SelectList(db.user_role, "id_role", "role_name", user.role_id);
                return View(user);
            }
            else
            {
                return RedirectToAction("../Home/Login");
            }
        }

        // GET: users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["id_user"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                user user = db.users.Find(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                ViewBag.role_id = new SelectList(db.user_role, "id_role", "role_name", user.role_id);
                return View(user);
            }
            else
            {
                return RedirectToAction("../Home/Login");
            }
        }

        // POST: users/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_user,fio,role_id,user_phone,user_email,user_password")] user user, HttpPostedFileBase upload)
        {
            if (Session["id_user"] != null)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        db.Entry(user).State = EntityState.Modified;
                        if (upload != null && upload.ContentLength > 0)
                        {
                            using (var reader = new System.IO.BinaryReader(upload.InputStream))
                            {
                                user.img = reader.ReadBytes(upload.ContentLength);
                            }
                            db.SaveChanges();
                        }
                        else
                        {
                            db.Entry(user).Property(m => m.img).IsModified = false;
                            db.SaveChanges();
                        }
                        return RedirectToAction("Index");
                    }
                    ViewBag.role_id = new SelectList(db.user_role, "id_role", "role_name", user.role_id);
                    return View(user);
                }
                catch (Exception e) { }
                ViewBag.role_id = new SelectList(db.user_role, "id_role", "role_name", user.role_id);
                return View(user);
            }
            else
            {
                return RedirectToAction("../Home/Login");
            }
        }

        // GET: users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["id_user"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                user user = db.users.Find(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                return View(user);
            }
            else
            {
                return RedirectToAction("../Home/Login");
            }
        }

        // POST: users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["id_user"] != null)
            {
                user user = db.users.Find(id);
                db.users.Remove(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("../Home/Login");
            }
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
