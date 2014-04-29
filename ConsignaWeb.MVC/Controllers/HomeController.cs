using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ConsignaWeb.MVC.Models.Authentication;
using ConsignaWeb.MVC.Models.Repository;

namespace ConsignaWeb.MVC.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            Users LoggedUser = Login.GetLoggedUser();
            ViewBag.user = LoggedUser;

            if (LoggedUser != null)
            {
                if (LoggedUser.Role == Roles.Usuario)
                {
                    var products = Products.ListProductsOnUser(LoggedUser).Where(i => i.Amount > 0);
                    var Allocates = ProductsAllocated.Queryable.Where(i => i.Vendor.UserBoss == LoggedUser && i.Amount > 0);

                    double stockValue = 0;
                    foreach (var item in products)
                    {
                        stockValue = stockValue + item.Cost * item.Amount;
                    }
                    var stockAmount = products.Select(i => i.Amount).Sum();

                    var allAllocatedAmount = Allocates.ToList().Sum(i => i.Amount);
                    double allAllocatedCost = 0;
                    foreach (var item in Allocates)
                    {
                        allAllocatedCost = allAllocatedCost + (item.Cost * item.Amount);
                    }
                    double allocatedToReceive = 0;
                    foreach (var item in Allocates)
                    {
                        allocatedToReceive = allocatedToReceive + (item.Amount * (item.Price - ((item.Price / 100) * item.Commision)));
                    }

                    var AllBuy = Charges.Queryable.Where(i => i.Amount > 0 && i.Client == LoggedUser && i.Type == Models.ChargesExtensions.ChargesTypes.Compra).Select(i => i.Value).ToList().Sum();
                    var AllSell = Charges.Queryable.Where(i => i.Amount > 0 && i.Client == LoggedUser && i.Type == Models.ChargesExtensions.ChargesTypes.Venda).Select(i => i.Value).ToList().Sum();

                    var totalVendor = Users.ListVendors(LoggedUser).Count;
                    var vendorWithProductAllocated = Allocates.Select(i => i.Vendor).Distinct().Count();

                    ViewBag.statistic = new HomeStatistics
                    {
                        AllAllocatedAmount = allAllocatedAmount,
                        AllAllocatedCost = allAllocatedCost,
                        AllAllocatedToReceive = allocatedToReceive,
                        AllProductsAmount = stockAmount,
                        AllProductsValue = stockValue,
                        TotalVendors = totalVendor,
                        VendorsWithAllocatedProducts = vendorWithProductAllocated,
                        TotalGain = AllSell,
                        TotalPayd = AllBuy

                    };
                }
                if (LoggedUser.Role == Roles.Vendedor)
                {
                    VendorPage vendorPage = new VendorPage();
                    List<ProductsAllocated> products = ProductsAllocated.Queryable.Where(i => i.Vendor == LoggedUser && i.Amount > 0).ToList();

                    vendorPage.Name = LoggedUser.Name + " " + LoggedUser.SurName;
                    foreach (var item in products)
                    {
                        vendorPage.TotalAmountAllocatedProducts += item.Amount;
                        vendorPage.TotalValueInProducts += Convert.ToDecimal(item.Price);
                        vendorPage.TotalValueToEarnInComission += Convert.ToDecimal(item.Price - ((item.Price / 100) * item.Commision));
                    }
                    vendorPage.AccontabilityDelayed = products.Where(i => i.DateAccountability <= DateTime.Now.Date).ToList();
                    vendorPage.NextProductToAccontability = products.Where(i => i.DateAccountability >= DateTime.Now.Date).OrderBy(i => i.DateAccountability).Take(6).ToList();
                    
                    ViewBag.vendorPage = vendorPage;
                    return View();
                }
                
            }

            return View();
        }

        public class HomeStatistics
        {
            public double AllProductsValue { get; set; }
            public int AllProductsAmount { get; set; }

            public double AllAllocatedCost { get; set; }
            public double AllAllocatedToReceive { get; set; }
            public double AllAllocatedAmount { get; set; }
            public int TotalVendors { get; set; }
            public int VendorsWithAllocatedProducts { get; set; }

            public double TotalPayd { get; set; }
            public double TotalGain { get; set; }
        }

        public class VendorPage
        {
            public string Name { get; set; }
            public decimal TotalAmountAllocatedProducts { get; set; }
            public decimal TotalValueInProducts { get; set; }
            public decimal TotalValueToEarnInComission { get; set; }
            public List<ProductsAllocated> NextProductToAccontability { get; set; }
            public List<ProductsAllocated> AccontabilityDelayed { get; set; }
        }

    }
}
