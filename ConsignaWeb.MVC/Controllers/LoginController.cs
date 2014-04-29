using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using ConsignaWeb.MVC.Models.Authentication;

namespace ConsignaWeb.MVC.Controllers
{
    public class LoginController : Controller
    {
        #region Login
        public ActionResult Client()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UserLogin(string email, string password)
        {
            if (Login.LoginUser(email, Encryption.MD5(password)))
            {
                return RedirectToAction("Index","Home");
            }
            ViewBag.LoginError = "Email ou senha incorretos";
            ViewBag.Email = email;

            return View("Client");
        }
        public ActionResult Logout()
        {
            Login.Logoff();
            Thread.Sleep(500);
            return RedirectToAction("Index","Home");
        }
        #endregion Login

    }
}
