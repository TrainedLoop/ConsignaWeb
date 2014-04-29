using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ConsignaWeb.MVC.Models.Authentication;
using ConsignaWeb.MVC.Models.Bussines;
using ConsignaWeb.MVC.Models.Repository;

namespace ConsignaWeb.MVC.Controllers
{
    public class VendorController : Controller
    {


        #region Register
        public ActionResult Register()
        {
            Users LoggedUser = Login.GetLoggedUser();
            if (LoggedUser == null)
            {
                return View("404");
            }
            if (LoggedUser.Role == Roles.Vendedor)
            {
                return View("NoPermission");
            }

            return View();
        }
        [HttpPost]
        public ActionResult Register(Users.RegisterVendorFormData vendor)
        {
            ViewBag.Saved = "";
            Users LoggedUser = Login.GetLoggedUser();
            if (LoggedUser == null)
            {
                return Redirect("404");
            }
            if (LoggedUser.Role == Roles.Vendedor)
            {
                return Redirect("NoPermission");
            }
            #region Validations

            Validation validation = new Validation();
            bool error = false;
            string NameError = null;
            try
            {
                validation.Name(vendor.Name);
            }
            catch (Exception ex)
            {
                NameError = ex.Message;
                error = true;
            }
            string SurNameError = null;
            try
            {
                validation.Name(vendor.SurName);
            }
            catch (Exception ex)
            {
                NameError = ex.Message;
                error = true;
            }
            string PasswordError = null;
            try
            {
                validation.Password(vendor.Password1, vendor.Password2);
            }
            catch (Exception ex)
            {

                PasswordError = ex.Message;
                error = true;
            }
            string EmailError = null;
            try
            {
                validation.ClientEmail(vendor.Email);
            }
            catch (Exception ex)
            {
                EmailError = ex.Message;
                error = true;
            }
            string CPFCNPJError = null;
            try
            {
                validation.CPFnCNPJ(vendor.CPFCNPJ);
            }
            catch (Exception ex)
            {
                CPFCNPJError = ex.Message;
                error = true;
            }
            #endregion Validations


            if (!error)
            {
                Users.NewVendorFromForm(vendor, LoggedUser);
                ViewBag.SaveSuccess = "Salvo com Sucesso";
                return View("Register");
            }

            ViewBag.Name = vendor.Name;
            ViewBag.SurName = vendor.SurName;
            ViewBag.Email = vendor.Email;
            ViewBag.CPFCNPJ = vendor.CPFCNPJ;

            ViewBag.NameError = NameError;
            ViewBag.SurNameError = SurNameError;
            ViewBag.PasswordError = PasswordError;
            ViewBag.EmailError = EmailError;
            ViewBag.CPFCNPJError = CPFCNPJError;
            return View("Register");
        }


        #endregion Register

        #region Edit
        public ActionResult Edit(int vendorid = 0)
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

            List<Users> Vendors = Users.ListVendors(LoggedUser);
            ViewBag.Vendors = Vendors.OrderBy(i => i.Id).ToList();
            ViewBag.SelectedVendor = Vendors.Where(i => i.Id == vendorid).FirstOrDefault();
            return View();
        }

        public ActionResult EditVendorBox(int VendorId)
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


            List<Users> Vendors = Users.ListVendors(LoggedUser);
            ViewBag.Vendor = Vendors.Where(i => i.Id == VendorId).FirstOrDefault();
            return View();
        }
        [HttpPost]
        public ActionResult EditVendor(Users.RegisterVendorFormData EditVendor)
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
            List<Users> VendorsList = Users.ListVendors(LoggedUser).ToList();
            ViewBag.Error = true;
            #region Validation
            Validation validation = new Validation();
            bool error = false;
            string PasswordError = null;
            try
            {
                validation.Password(EditVendor.Password1, EditVendor.Password2, true);
            }
            catch (Exception ex)
            {
                PasswordError = ex.Message;
                error = true;
            }
            string EmailError = null;
            try
            {
                validation.ClientEmail(EditVendor.Email);
            }
            catch (Exception ex)
            {
                EmailError = ex.Message;
                error = true;
            }

            #endregion Validation

            Users vendor = VendorsList.Find(i => i.Id == EditVendor.Id);
            if (!error)
            {
                if (!string.IsNullOrEmpty(EditVendor.Password1)) { vendor.Password = Encryption.MD5(EditVendor.Password1); }
                if (!string.IsNullOrEmpty(EditVendor.Email)) { vendor.Email = EditVendor.Email; }
                vendor.Update();
                ViewBag.SaveSuccess = "Salvo com Sucesso";
                ViewBag.Error = false;
            }

            ViewBag.Vendors = VendorsList.OrderBy(i => i.Id).ToList();
            ViewBag.SelectedVendor = VendorsList.Find(i => i.Id == EditVendor.Id);
            ViewBag.PasswordError = PasswordError;
            ViewBag.EmailError = (vendor.Email == EditVendor.Email) ? null : EmailError;
            return View("Edit", new { Id = EditVendor.Id });
        }


        public ActionResult DeleteVendor(int vendorId)
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

            List<Users> VendorsList = Users.ListVendors(LoggedUser).ToList(); ;
            Users vendor = VendorsList.Find(i => i.Id == vendorId);
            List<ProductsAllocated> alocatedProducts = ProductsAllocated.AlocatedProductsOnVendor(vendor);
            if (alocatedProducts.Count > 0)
            {
                ViewBag.Vendors = VendorsList.OrderBy(i => i.Id).ToList();
                ViewBag.SelectedVendor = VendorsList.Find(i => i.Id == vendorId);
                ViewBag.Error = true;
                ViewBag.ProductAlocatedError = "Vendedor ainda tem contas a prestar";
                return View("Edit");
            }
            vendor.UserBoss = null;
            vendor.Save();
            return Redirect("/Vendor/Edit");
        }

        #endregion Edit

        public ActionResult ShowVendors()
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

            List<Users> vendors = new List<Users>();

            foreach (var item in Users.Queryable.Where(i => i.UserBoss == LoggedUser))
            {
                vendors.Add(item);
            }

            ViewBag.vendors = vendors;
            return View();
        }

        #region Allocate
        public ActionResult AllocateProduct()
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

            List<Products> products = Products.ListProductsOnUser(LoggedUser);
            List<Users> vendors = Users.ListVendors(LoggedUser);

            ViewBag.Products = products;
            ViewBag.Vendors = vendors;

            return View();
        }
        [HttpPost]
        public ActionResult AllocateProducts(string vendorid, List<AlocatteProductForm> alocateForm)
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


            Validation validattion = new Validation();
            List<string> productError = new List<string>();
            string vendorError = null;
            List<string> AmountError = new List<string>();
            List<string> ComissionError = new List<string>();
            List<string> DataError = new List<string>();
            List<string> formError = new List<string>();
            List<int> boxErrorNumber = new List<int>();
            for (int i = 0; i < alocateForm.Count; i++)
            {
                productError.Add("");
                AmountError.Add("");
                ComissionError.Add("");
                DataError.Add("");
                formError.Add("");

            }


            alocateForm.RemoveAll(i => i.DateAccountability <= DateTime.MinValue || i.Amount == 0 || i.ProductId == 0 || i.Comission == "");

            foreach (var item in alocateForm)
            {
                if (Products.Queryable.Where(i => i.User == LoggedUser && i.Id == item.ProductId).FirstOrDefault() == null)
                {
                    productError[item.BoxNumber] = "Produto não encontrado";
                    boxErrorNumber.Add(item.BoxNumber);
                }

                try
                {
                    validattion.Commission(item.Comission.ToString(CultureInfo.InvariantCulture).Replace(',', '.'));
                }
                catch (Exception ex)
                {
                    ComissionError[item.BoxNumber] = ex.Message;
                }

                if (!string.IsNullOrEmpty(ComissionError[item.BoxNumber]))
                    boxErrorNumber.Add(item.BoxNumber);
                else
                {
                    item.Comission = item.Comission.ToString(CultureInfo.InvariantCulture).Replace(',', '.');
                }
                try
                {
                    validattion.DateNotToday(item.DateAccountability.ToString());
                }
                catch (Exception ex)
                {

                    DataError[item.BoxNumber] = ex.Message;
                }
                if (!string.IsNullOrEmpty(DataError[item.BoxNumber]))
                    boxErrorNumber.Add(item.BoxNumber);

                item.Product = Products.Queryable.Where(i => i.User == LoggedUser && i.Id == item.ProductId).FirstOrDefault();

            }

            Users vendor = new Users();
            if (string.IsNullOrEmpty(vendorid))
                vendorError = "Vendedor Invalido";



            else
            {
                vendor = Users.Queryable.Where(i => i.UserBoss == LoggedUser && i.Id == int.Parse(vendorid)).FirstOrDefault();
                if (vendor == null)
                {
                    vendorError = "Vendedor Invalido";
                }
            }

            foreach (var productItem in alocateForm)
            {

                if (productItem.Product.Amount < productItem.Amount || productItem.Amount <= 0)
                {
                    AmountError[productItem.BoxNumber] = "Quantidade indisponivel: " + productItem.Product.Name + " Estq: " + productItem.Product.Amount;
                    boxErrorNumber.Add(productItem.BoxNumber);
                }
                if (string.IsNullOrEmpty(productError[productItem.BoxNumber]) && string.IsNullOrEmpty(vendorError) && string.IsNullOrEmpty(AmountError[productItem.BoxNumber]) && string.IsNullOrEmpty(formError[productItem.BoxNumber]) && string.IsNullOrEmpty(DataError[productItem.BoxNumber]))
                {
                    ProductsAllocated existingAlocate = ProductsAllocated.Queryable.Where(i => i.Product == productItem.Product && i.Vendor == vendor &&
                        i.Price == productItem.Product.Price && i.Cost == productItem.Product.Cost && i.DateAccountability == productItem.DateAccountability && i.Commision == double.Parse(productItem.Comission, CultureInfo.InvariantCulture)).FirstOrDefault();
                    productItem.Product.Amount -= productItem.Amount;
                    if (existingAlocate == null)
                    {
                        ProductsAllocated productAlocate = new ProductsAllocated { Product = productItem.Product, Vendor = vendor, Amount = productItem.Amount, Cost = productItem.Product.Cost, Price = productItem.Product.Price, DateAccountability = productItem.DateAccountability, Commision = double.Parse(productItem.Comission, CultureInfo.InvariantCulture) };
                        productAlocate.Save();
                    }
                    else
                    {
                        existingAlocate.Amount += productItem.Amount;
                        existingAlocate.Update();

                    }
                    ViewBag.SaveSuccess = "Produto alocado com sucesso";
                    productItem.Product.Save();

                }
            }
            List<Products> listproducts = Products.Queryable.Where(i => i.User == LoggedUser).ToList();
            List<Users> vendors = Users.Queryable.Where(i => i.UserBoss == LoggedUser).ToList();
            ViewBag.Products = listproducts;
            ViewBag.Vendors = vendors;

            List<AlocatteProductForm> boxWithError = new List<AlocatteProductForm>();

            //errors
            foreach (var item in boxErrorNumber.Distinct())
            {
                boxWithError.Add(alocateForm.Where(i => i.BoxNumber == item).FirstOrDefault());
            }
            ViewBag.boxWithError = boxWithError;
            ViewBag.productError = productError;
            ViewBag.vendorError = vendorError;
            ViewBag.AmountError = AmountError;
            ViewBag.FormError = formError;
            ViewBag.DataError = DataError;
            ViewBag.ComissionError = ComissionError;
            if (boxWithError.Count() > 0)
            {
                ViewBag.Vendor = vendor;
            }
            return View("AllocateProduct");
        }


        public ActionResult EditAllocatedProducts(int vendorId = 0) //vendor selector
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

            ViewBag.vendors = Users.ListVendors(LoggedUser);
            ViewBag.vendorId = vendorId;
            return View();
        }
        public ActionResult AllocatedProductsBox(int? vendorId, string Save)
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
            ViewBag.vendorId = vendorId;
            ViewBag.SaveSuccess = Save;
            ViewBag.ProductsAllocated = ProductsAllocated.AlocatedProductsOnVendor(Users.TryFind(vendorId));
            return View();
        }
        [HttpPost]
        public ActionResult AllocatedProductsBoxEdit(int? productId, string DateAccountability, string amount, int vendorId = 0)
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
            #region Validation
            Validation validation = new Validation();
            bool error = false;
            string DataError = null;
            try
            {
                validation.DateNotToday(DateAccountability);
            }
            catch (Exception ex)
            {
                DataError = ex.Message;
                error = true;
            }
            string AmountError = null;
            try
            {
                validation.Amount(amount);
            }
            catch (Exception ex)
            {
                AmountError = ex.Message;
                error = true;
            }
            string InventoryError = null;
            ProductsAllocated productToEdite = ProductsAllocated.TryFind(productId);
            if (productToEdite.Product.Amount - (int.Parse(amount) - productToEdite.Amount) < 0)
            {
                InventoryError = "Item indisponivel no estoque";
                error = true;
            }

            #endregion Validation

            if (!error)
            {
                productToEdite.Product.Amount = productToEdite.Product.Amount - (int.Parse(amount) - productToEdite.Amount);
                productToEdite.DateAccountability = DateTime.Parse(DateAccountability);
                productToEdite.Product.Save();
                productToEdite.Amount = int.Parse(amount);
                productToEdite.Save();
                ViewBag.SaveSuccess = "Salvo com Sucesso";
            }

            ViewBag.DataError = DataError;
            ViewBag.AmountError = AmountError;
            ViewBag.InventoryError = InventoryError;
            ViewBag.vendors = Users.Queryable.Where(i => i.UserBoss == LoggedUser).ToList();
            ViewBag.vendorId = vendorId;

            return View("EditAllocatedProducts", new { vendorId = vendorId });
        }
        #endregion Allocate


        #region Accontability

        public ActionResult AccountabilityPerDate()
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
            DateTime date = DateTime.Now;
            List<Users> vendors = ProductsAllocated.Queryable.Where(i => i.DateAccountability.Date == date.Date).Select(i => i.Vendor).Distinct().ToList();
            ViewBag.date = date.ToShortDateString();
            ViewBag.vendors = vendors;
            return View();
        }
        [HttpPost]
        public ActionResult AccountabilityPerDate(DateTime? date, int? vendorId, int? countProductId)
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

            if (date == DateTime.MinValue || date == null)
                date = DateTime.Now;
            List<Users> vendors = ProductsAllocated.Queryable.Where(i => i.DateAccountability.Date == date.Value.Date && i.Amount > 0 && i.Vendor.UserBoss == LoggedUser).Select(i => i.Vendor).Distinct().ToList();
            ViewBag.date = date.Value.ToShortDateString();
            ViewBag.vendors = vendors;
            ViewBag.vendorId = vendorId;
            ViewBag.productId = countProductId;
            return View();
        }
        [HttpPost]
        public ActionResult PendingVendorsOnDay(DateTime? date, int? vendorId)
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
            if (date == DateTime.MinValue || date == null)
                date = DateTime.Now;
            List<ProductsAllocated> products = ProductsAllocated.Queryable.Where(i => i.Vendor.Id == vendorId && i.Vendor.UserBoss == LoggedUser && i.DateAccountability.Date == date.Value.Date && i.Amount > 0).ToList();
            if(products.Count == 0)
            {
                return null;
            }
            ViewBag.products = products;
            ViewBag.date = date.Value.ToShortDateString();
            ViewBag.vendorId = vendorId;
            return View();
        }
        [HttpPost]
        public ActionResult CountAllocattedProductPerDate(int idProductAllocated, int vendorId, DateTime? date)
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
            ProductsAllocated product = ProductsAllocated.Queryable.Where(i => i.Id == idProductAllocated && i.Vendor.UserBoss == LoggedUser).FirstOrDefault();
            ViewBag.product = product;
            if (date != null)
                ViewBag.date = date.Value.ToShortDateString();
            else
                ViewBag.date = DateTime.Now.Date;
            ViewBag.vendorId = vendorId;

            return View();

        }
        [HttpPost]
        public ActionResult DateAccountability(int AllocatedId, int VendorId, string QuantityReturned = "0", string TotalPaid = "0")
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

            Users vendor = Users.ListVendors(LoggedUser).Find(i => i.Id == VendorId);
            ProductsAllocated product = ProductsAllocated.AlocatedProductsOnVendor(vendor).Find(i => i.Id == AllocatedId);
            #region Validation
            bool error = false;
            Validation vld = new Validation();
            vld.Amount(QuantityReturned, product.Amount);
            try
            {
                vld.Amount(QuantityReturned);

            }
            catch (Exception ex)
            {

                ViewBag.QuantityError = ex.Message;
                error = true;
            }
            try
            {
                vld.Amount(TotalPaid);

            }
            catch (Exception ex)
            {

                ViewBag.TotalPaidError = ex.Message;
                error = true;
            }
            try
            {
                vld.Amount(TotalPaid, product.Amount);
            }
            catch (Exception ex)
            {

                ViewBag.TotalPaidError = ex.Message;
                error = true;
            }
            #endregion Validation

            if (product.Amount - (Convert.ToInt32(QuantityReturned) + Convert.ToInt32(TotalPaid)) >= 0 && !error)
            {
                product.Amount -= (Convert.ToInt32(TotalPaid) + (Convert.ToInt32(QuantityReturned)));
                product.Product.Amount += Convert.ToInt32(QuantityReturned);
                product.Update();
                Charges charge = new Charges()
                {
                    Amount = Convert.ToInt32(TotalPaid),
                    Client = LoggedUser,
                    Data = DateTime.Now,
                    Products = product.Product,
                    Type = Models.ChargesExtensions.ChargesTypes.Venda,
                    Value = MoneyConversor.RoundUp((MoneyConversor.RemoveComission(product.Price * Convert.ToInt32(TotalPaid), product.Commision))),
                    Vendor = vendor
                };
                charge.Save();
                ViewBag.VendorId = VendorId;
                ViewBag.date = product.DateAccountability.ToShortDateString();
                ViewBag.Save = "Salvo com sucesso";
                return View("AccountabilityPerDate");
            }
            ViewBag.error = error;
            ViewBag.vendorId = VendorId;
            ViewBag.productId = AllocatedId;
            return View("AccountabilityPerDate");

        }

        public ActionResult AccountabilityPerVendor()
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
            List<Users> Vendors = Users.Queryable.Where(i => i.UserBoss == LoggedUser).ToList();
            ViewBag.Vendors = Vendors.OrderBy(i => i.Id).ToList();
            return View();
        }
        [HttpPost]
        public ActionResult AccountabilityPerVendor(int vendorId, int countProductId = 0)
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

            List<Users> Vendors = Users.Queryable.Where(i => i.UserBoss == LoggedUser).ToList();
            ViewBag.Vendors = Vendors.OrderBy(i => i.Id).ToList();
            ViewBag.SelectedVendor = Vendors.Where(i => i.Id == vendorId).FirstOrDefault();
            if (countProductId != 0 && countProductId != 0)
                ViewBag.Product = ProductsAllocated.Queryable.Where(i => i.Id == countProductId && i.Vendor.Id == vendorId).FirstOrDefault();
            return View();
        }
        [HttpPost]
        public ActionResult ProductsToAccountabilityOnVendor(int VendorId = 0)
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
            List<ProductsAllocated> products = ProductsAllocated.Queryable.Where(i => i.Vendor.Id == VendorId && i.Vendor.UserBoss == LoggedUser && i.Amount > 0).ToList();
            ViewBag.products = products;
            return View();
        }
        [HttpPost]
        public ActionResult CountAllocatedProductsPerVendor(int VendorId, ProductsAllocated Product)
        {
            if (Product.Vendor.Id == VendorId)
            {
                ViewBag.Product = Product;
            }
            return View();
        }

        public ActionResult VendorAccountability(int AllocatedId, int VendorId, string QuantityReturned = "0", string TotalPaid = "0")
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

            Users vendor = Users.ListVendors(LoggedUser).Find(i => i.Id == VendorId);
            ProductsAllocated product = ProductsAllocated.AlocatedProductsOnVendor(vendor).Find(i => i.Id == AllocatedId);
            #region Validation
            bool error = false;
            Validation vld = new Validation();
            vld.Amount(QuantityReturned, product.Amount);
            try
            {
                vld.Amount(QuantityReturned);

            }
            catch (Exception ex)
            {

                ViewBag.QuantityError = ex.Message;
                error = true;
            }
            try
            {
                vld.Amount(TotalPaid);

            }
            catch (Exception ex)
            {

                ViewBag.TotalPaidError = ex.Message;
                error = true;
            }
            try
            {
                vld.Amount(TotalPaid, product.Amount);
            }
            catch (Exception ex)
            {

                ViewBag.TotalPaidError = ex.Message;
                error = true;
            }
            #endregion Validation

            ViewBag.Vendors = Users.ListVendors(LoggedUser);
            ViewBag.SelectedVendor = vendor;
            if (product.Amount - (Convert.ToInt32(QuantityReturned) + Convert.ToInt32(TotalPaid)) >= 0 && !error)
            {
                product.Amount -= (Convert.ToInt32(TotalPaid) + (Convert.ToInt32(QuantityReturned)));
                product.Product.Amount += Convert.ToInt32(QuantityReturned);
                product.Update();
                Charges charge = new Charges()
                {
                    Amount = Convert.ToInt32(TotalPaid),
                    Client = LoggedUser,
                    Data = DateTime.Now,
                    Products = product.Product,
                    Type = Models.ChargesExtensions.ChargesTypes.Venda,
                    Value = MoneyConversor.RoundUp((MoneyConversor.RemoveComission(product.Price * Convert.ToInt32(TotalPaid), product.Commision))),
                    Vendor = vendor
                };
                charge.Save();
                ViewBag.Save = "Salvo com sucesso";
                return View("AccountabilityPerVendor");
            }
            ViewBag.vendorId = VendorId;
            ViewBag.Product = product;
            ViewBag.error = error;
            return View("AccountabilityPerVendor");

        }
        #endregion Accontability
    }
}