using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ConsignaWeb.MVC.Models.Authentication;
using ConsignaWeb.MVC.Models.Bussines;
using ConsignaWeb.MVC.Models.Repository;

namespace ConsignaWeb.MVC.Controllers
{
    public class ClientController : Controller
    {

        #region Register
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegisterAction(Users.RegisterClientFormData client)
        {
            Validation validation = new Validation();
            Encryption encrypt = new Encryption();

            bool error = false;
            string NameError = null;
            try
            {
                validation.Name(client.Name);
            }
            catch (Exception ex)
            {
                error = true;
                NameError = ex.Message;
            }
            string SurNameError = null;
            try
            {
                validation.Name(client.SurName);
            }
            catch (Exception ex)
            {
                SurNameError = ex.Message;
                error = true; ;
            }
            string PasswordError = null;
            try
            {
                validation.Password(client.Password1, client.Password2);
            }
            catch (Exception ex)
            {
                error = true;
                PasswordError = ex.Message;
            }
            string EmailError = null;
            try
            {
                validation.ClientEmail(client.Email);
            }
            catch (Exception ex)
            {                
                EmailError = ex.Message;
                error = true;
            }
            string CPFCNPJError = null;
            try
            {
                validation.CPFnCNPJ(client.CPFCNPJ);
            }
            catch (Exception ex)
            {
                CPFCNPJError = ex.Message;
                error = true;
            }

            if (!error)
            {
                Users newuser = new Users();
                newuser.Name = client.Name;
                newuser.SurName = client.SurName;
                newuser.Password = Encryption.MD5(client.Password1);
                newuser.Role = Roles.Usuario;
                newuser.CPF = client.CPFCNPJ;
                newuser.Credits = 0;
                newuser.Email = client.Email;
                newuser.Save();
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Name = client.Name;
            ViewBag.SurName = client.SurName;
            ViewBag.Email = client.Email;
            ViewBag.CPFCNPJ = client.CPFCNPJ;

            ViewBag.NameError = NameError;
            ViewBag.SurNameError = SurNameError;
            ViewBag.PasswordError = PasswordError;
            ViewBag.EmailError = EmailError;
            ViewBag.CPFCNPJError = CPFCNPJError;
            return View("Register");
        }
        #endregion Register

        #region Edit
        public ActionResult EditPassword()
        {
            Users LoggedUser = Login.GetLoggedUser();
            if (LoggedUser == null)
            {
                return View("NoPermission");
            }
            ViewBag.User = LoggedUser;
            return View();
        }

       [HttpPost]
        public ActionResult EditPasswordAction(Users.RegisterClientFormData editusers, string pass)
        {
            ViewBag.Saved = "";
            Users LoggedUser = Login.GetLoggedUser();
            if (LoggedUser == null)
            {
                return View("NoPermission");
            }

            ViewBag.User = LoggedUser;
            Validation validation = new Validation();
            bool error = false;
            string PassworldError = null;
            try
            {
                validation.Password(editusers.Password1, editusers.Password2);
            }
            catch (Exception ex)
            {
                PassworldError = ex.Message;
                error = true;
            }
            if (!error)
            {
                string teste = Encryption.MD5(editusers.Password1);
                if (LoggedUser.Password == Encryption.MD5(pass))
                {
                    LoggedUser.Password = Encryption.MD5(editusers.Password1);
                    LoggedUser.Update();
                    ViewBag.Saved = "Alterado com Sucesso";
                }
                else
                {
                    ViewBag.NoSamePassword = "Senha incorreta.";
                }
            }
            ViewBag.PasswordError = PassworldError;
            return View("EditPassword");
        }
        #endregion Edit
    }
}
