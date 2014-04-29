using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConsignaWeb.MVC.Models.Repository
{
    [ActiveRecord(Schema = "yuffiedb")]
    public class ProductsAllocated : ActiveRecordLinqBase<ProductsAllocated>
    {
        [PrimaryKey(PrimaryKeyType.Native, "Id")]
        public int Id { get; set; }

        [BelongsTo]
        public Products Product { get; set; }

        [BelongsTo]
        public Users Vendor { get; set; }

        [Property(Column = "Amount")]
        public int Amount { get; set; }

        [Property(Column = "Price")]
        public double Price { get; set; }

        [Property(Column = "Cost")]
        public double Cost { get; set; }

        [Property(Column = "Commision")]
        public double Commision { get; set; }

        [Property(Column = "DateAccountability")]
        public DateTime DateAccountability { get; set; }

        public static List<ProductsAllocated> AlocatedProductsOnVendor(Users vendor)
        {
            List<ProductsAllocated> alocatedProducts = ProductsAllocated.Queryable.Where(i => i.Vendor == vendor && i.Amount > 0).ToList();
            return alocatedProducts;
        }


    }
    public class AlocatteProductForm
    {
        public int ProductId { get; set; }
        public Products Product { get; set; }
        public int Amount { get; set; }
        public string Comission { get; set; }
        public DateTime DateAccountability { get; set; }
        public int BoxNumber { get; set; }
    }
}