using BeautyShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace BeautyShop.Controllers
{
    public class HomeController : Controller
    {
        private BeautyDataEntities db = new BeautyDataEntities();

        public ActionResult Index()
        {
                return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        //POST: Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(user user, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                var check = db.users.FirstOrDefault(s => s.user_email == user.user_email);
                if (check == null)
                {
                    user.user_password = user.user_password;//GetMD5(_user.Password);
                    user.role_id = 3;
                    if (upload != null && upload.ContentLength > 0)
                    {
                        using (var reader = new System.IO.BinaryReader(upload.InputStream))
                        {
                            user.img = reader.ReadBytes(upload.ContentLength);
                        }
                    }
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.users.Add(user);
                    db.SaveChanges();
                    //add session
                    Session["id_user"] = user.id_user;
                    Session["img"] = user.img;
                    Session["fio"] = user.fio;
                    Session["role_id"] = user.role_id;
                    Session["user_phone"] = user.user_phone;
                    Session["user_email"] = user.user_email;
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = "Такое имя пользователя уже существует";
                    return View();
                }
            }
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string user_email, string user_password)
        {
            if (ModelState.IsValid)
            {
                var f_password = user_password;//GetMD5(password);
                var data = db.users.Where(s => s.user_email.Equals(user_email) && s.user_password.Equals(f_password)).ToList();
                if (data.Count() > 0)
                {
                    //add session
                    Session["id_user"] = data.FirstOrDefault().id_user;
                    Session["img"] = data.FirstOrDefault().img;
                    Session["fio"] = data.FirstOrDefault().fio;
                    Session["role_id"] = data.FirstOrDefault().user_role.role_name;
                    Session["user_phone"] = data.FirstOrDefault().user_phone;
                    Session["user_email"] = data.FirstOrDefault().user_email;
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = "Login failed";
                    return RedirectToAction("Login");
                }
            }
            return View();
        }

        //Logout
        public ActionResult Logout()
        {
            Session.Clear();//remove session
            return RedirectToAction("Login");
        }

        //create a string MD5
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }
    }
}