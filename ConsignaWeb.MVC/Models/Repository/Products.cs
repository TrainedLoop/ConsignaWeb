using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using ConsignaWeb.MVC.Models.ChargesExtensions;

namespace ConsignaWeb.MVC.Models.Repository
{
    [ActiveRecord(Schema = "yuffiedb")]
    public class Products : ActiveRecordLinqBase<Products>
    {


        [PrimaryKey(PrimaryKeyType.Native, "Id")]
        public int Id { get; set; }

        [Property(Column = "Name")]
        public string Name { get; set; }

        [Property(Column = "Price")]
        public double Price { get; set; }

        [Property(Column = "Cost")]
        public double Cost { get; set; }

        [Property(Column = "Amount")]
        public int Amount { get; set; }

        [Property(Column = "Image")]
        public string Image { get; set; }

        [BelongsTo]
        public Users User { get; set; }

        public static List<Products> ListProductsOnUser(Users LoggedUser)
        {
            List<Products> products = new List<Products>();

            foreach (var item in Products.Queryable.Where(i => i.User == LoggedUser))
            {
                products.Add(item);
            }
            return products;
        }

        public static void NewProductFromForm(RegisterProductForm productForm, Users LoggedUser, Products newProducts)
        {
            newProducts.Name = productForm.Name;
            newProducts.Cost = Double.Parse(productForm.Cost.Replace(',', '.'), CultureInfo.InvariantCulture);
            newProducts.Amount = int.Parse(productForm.Amount);
            newProducts.Price = Double.Parse(productForm.Price.Replace(',', '.'), CultureInfo.InvariantCulture);
            newProducts.User = LoggedUser;
            newProducts.Save();
            new Charges { Client = LoggedUser, Type = ChargesTypes.Compra, Products = newProducts, Value = newProducts.Cost * newProducts.Amount, Amount = newProducts.Amount, Data = DateTime.Now }.Save();
        }
        public static void EditProductFromForm(RegisterProductForm productToEdit, Users LoggedUser, Products editedProduct)
        {
            editedProduct.Name = productToEdit.Name;

            if (editedProduct.Amount < int.Parse(productToEdit.Amount))
            {
                editedProduct.Cost = Double.Parse(productToEdit.Cost.Replace(',', '.'), CultureInfo.InvariantCulture);
                new Charges
                {
                    Client = LoggedUser,
                    Type = ChargesTypes.Compra,
                    Products = editedProduct,
                    Value = (-1 * (editedProduct.Cost * (int.Parse(productToEdit.Amount) - editedProduct.Amount))),
                    Amount = (int.Parse(productToEdit.Amount) - editedProduct.Amount),
                    Data = DateTime.Now
                }.Save();
                editedProduct.Amount = int.Parse(productToEdit.Amount);
            }
            if (editedProduct.Amount > int.Parse(productToEdit.Amount))
            {
                new Charges
                {
                    Client = LoggedUser,
                    Type = ChargesTypes.Retirada_do_Estoque,
                    Products = editedProduct,
                    Value = -1 * editedProduct.Cost * ((int.Parse(productToEdit.Amount)) - editedProduct.Amount),
                    Amount = (int.Parse(productToEdit.Amount) - editedProduct.Amount),
                    Data = DateTime.Now
                }.Save();
                editedProduct.Cost = Double.Parse(productToEdit.Cost.Replace(',', '.'), CultureInfo.InvariantCulture);
                editedProduct.Amount = int.Parse(productToEdit.Amount);
            }
            editedProduct.Price = Double.Parse(productToEdit.Price.Replace(',', '.'), CultureInfo.InvariantCulture);
            editedProduct.Save();
        }

    }




    public class RegisterProductForm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Cost { get; set; }
        public string Price { get; set; }
        public string Amount { get; set; }
        public HttpPostedFileBase imagefile { get; set; }
    }




}