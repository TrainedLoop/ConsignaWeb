using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ConsignaWeb.MVC.Models;
using ConsignaWeb.MVC.Models.Authentication;
using ConsignaWeb.MVC.Models.Bussines;
using ConsignaWeb.MVC.Models.ChargesExtensions;
using ConsignaWeb.MVC.Models.Repository;

namespace ConsignaWeb.MVC.Controllers
{
    public class ProductController : Controller
    {
        #region Register
        public ActionResult Register()
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

            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterProductForm productForm)
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
            Upload up = new Upload();

            string AmountError = null;
            try
            {
                validation.Money(productForm.Amount);
            }
            catch (Exception ex)
            {
                AmountError = ex.Message;
                error = true;
            }

            string CostError = null;
            try
            {
                validation.Money(productForm.Cost);
            }
            catch (Exception ex)
            {
                CostError = ex.Message;
                error = true;
            }
            string PriceError = null;
            try
            {
                validation.Money(productForm.Price);
            }
            catch (Exception ex)
            {
                PriceError = ex.Message;
                error = true;
            }
            string NameError = null;
            try
            {
                validation.Name(productForm.Name, "Digite o nome completo do produto");
            }
            catch (Exception ex)
            {
                NameError = ex.Message;
                error = true;
            }
            string ImageError = null;
            Products newProducts = new Products();
            if (productForm.imagefile != null)
            {
                try
                {
                    newProducts.Image = up.ProductImage(productForm.imagefile, LoggedUser.Id);
                }
                catch (Exception ex)
                {
                    ImageError = ex.Message;
                    error = true;
                }
            }
            else
                newProducts.Image = General.Images.NoImage;
            #endregion Validation


            if (!error)
            {
                Products.NewProductFromForm(productForm, LoggedUser, newProducts);
                ViewBag.SaveSuccess = "Salvo com Sucesso";
                return View("Register");
            }

            ViewBag.AmountError = AmountError;
            ViewBag.CostError = CostError;
            ViewBag.PriceError = PriceError;
            ViewBag.NameError = NameError;
            ViewBag.ImageError = ImageError;
            return View("Register");
        }


        #endregion Register


        #region Edit
        public ActionResult Edit(int id = 0)
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

            ViewBag.Products = products.OrderBy(i => i.Id).ToList();

            ViewBag.SelectedProduct = Products.TryFind(id);
            return View();
        }

        public ActionResult EditBox(int ProductId = 0)
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

            Products Product = Products.TryFind(ProductId);
            ViewBag.Product = Product;
            return View();
        }

        [HttpPost]
        public ActionResult ProductEdit(RegisterProductForm productToEdit)
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
            Upload up = new Upload();
            ViewBag.Error = true;
            Products editedProduct = Products.Find(productToEdit.Id);

            string AmountError = null;
            try
            {
                validation.Money(productToEdit.Amount);
            }
            catch (Exception ex)
            {
                error = true;
                AmountError = ex.Message;
            }
            string CostError = null;
            try
            {
                validation.Money(productToEdit.Cost);
            }
            catch (Exception ex)
            {
                error = true;
                CostError = ex.Message;
            }
            string PriceError = null;
            try
            {
                validation.Money(productToEdit.Price);
            }
            catch (Exception ex)
            {
                error = true;
                PriceError = ex.Message;
            }
            string NameError = null;
            try
            {
                validation.Name(productToEdit.Name, "Digite o nome completo do produto");
            }
            catch (Exception ex)
            {
                error = true;
                NameError = ex.Message;
            }
            string ImageError = null;
            if (productToEdit.imagefile != null)
            {
                try
                {
                    editedProduct.Image = up.ProductImage(productToEdit.imagefile, LoggedUser.Id);
                }
                catch (Exception ex)
                {

                    ImageError = ex.Message;
                    error = true;
                }
            }
            #endregion Validation
            
            if (!error)
            {
                if (string.IsNullOrEmpty(ImageError))
                {
                    Products.EditProductFromForm(productToEdit, LoggedUser, editedProduct);
                    ViewBag.SaveSuccess = "Salvo com Sucesso";
                    ViewBag.Error = false;
                }
            }


            List<Products> products = Products.ListProductsOnUser(LoggedUser);
            ViewBag.Products = products.OrderBy(i => i.Id).ToList();
            ViewBag.SelectedProduct = editedProduct;


            ViewBag.AmountError = AmountError;
            ViewBag.CostError = CostError;
            ViewBag.PriceError = PriceError;
            ViewBag.NameError = NameError;
            ViewBag.ImageError = ImageError;

            return View("Edit", new { Id = productToEdit.Id });
        }

        #endregion Edit
       
        public ActionResult ShowProducts()
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
            ViewBag.Products = products;
            return View();
        }
    }
}
