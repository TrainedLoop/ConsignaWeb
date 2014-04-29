using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ConsignaWeb.MVC.Models.Authentication;
using ConsignaWeb.MVC.Models.ChargesExtensions;
using ConsignaWeb.MVC.Models.Repository;

namespace ConsignaWeb.MVC.Controllers
{
    public class ReportsController : Controller
    {
        //
        // GET: /Charger/
        public ActionResult Chargers(DateTime? inicialDate, DateTime? finalDate, ChargesTypes? type, int? vendorId, int? productId, int? max, int? min)
        {
            Users LoggedUser = Login.GetLoggedUser();
            if (LoggedUser == null)
            {
                return View("NoPermission");
            }
            if (LoggedUser.Role == Roles.Vendedor)
            {
                return View("NoPermission");
            }

            List<Charges> charges = Charges.Queryable.Where(i => i.Client == LoggedUser && i.Value != 0).ToList();

            if (inicialDate != null)
            {
                charges = charges.Where(i => i.Data > inicialDate.Value.AddDays(-1)).ToList();

            }

            if (finalDate != null)
            {
                charges = charges.Where(i => i.Data < finalDate.Value).ToList();

            }
            if (type == ChargesTypes.Compra)
            {
                charges = charges.Where(i => i.Type == ChargesTypes.Compra).ToList();
            }
            if (type == ChargesTypes.Venda)
            {
                charges = charges.Where(i => i.Type == ChargesTypes.Venda).ToList();
            }
            if (type == ChargesTypes.Retirada_do_Estoque)
            {
                charges = charges.Where(i => i.Type == ChargesTypes.Retirada_do_Estoque).ToList();
            }
            if (vendorId != null)
            {

                charges = charges.Where(i => i.Vendor != null && i.Vendor.Id == vendorId).ToList();

            }
            if (productId != null)
            {
                charges = charges.Where(i => i.Products.Id == productId).ToList();
            }
            if (min != null)
            {
                charges = charges.Where(i => i.Value > min).ToList();
            }
            if (max != null)
            {
                charges = charges.Where(i => i.Value < max).ToList();
            }
            ViewBag.Charges = charges;
            ViewBag.Vendor = Users.Queryable.Where(i => i.UserBoss == LoggedUser).ToList();
            ViewBag.Products = Products.Queryable.Where(i => i.User == LoggedUser).ToList();
            return View();
        }

        public ActionResult AllChargesPerMount(int month = 0)
        {
            if (month == 0)
            {
                month = DateTime.Now.Month;
            }
            Users LoggedUser = Login.GetLoggedUser();
            if (LoggedUser == null)
            {
                return View("NoPermission");
            }
            if (LoggedUser.Role == Roles.Vendedor)
            {
                return View("NoPermission");
            }
            List<Charges> charges = Charges.Queryable.Where(i => i.Client == LoggedUser && i.Data.Month == month).OrderBy(i => i.Data).ToList();
            ViewBag.Charges = charges;
            return View("Chargers");
        }

        public ActionResult AllocatedProductsOnVendor(int? vendorId)
        {
            Users LoggedUser = Login.GetLoggedUser();
            if (LoggedUser == null)
            {
                return View("NoPermission");
            }
            if (LoggedUser.Role == Roles.Vendedor)
            {
                ViewBag.products = ProductsAllocated.Queryable.Where(i => i.Vendor == LoggedUser && i.Amount > 0).ToList();
                return View();
            }
            ViewBag.products = ProductsAllocated.Queryable.Where(i => i.Vendor.Id == vendorId && i.Amount > 0).ToList();
            ViewBag.vendors = Users.ListVendors(LoggedUser);

            return View();
        }

    }
}
