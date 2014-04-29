using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ConsignaWeb.MVC.Models.Authentication;

namespace ConsignaWeb.MVC.Models.Repository
{
    [ActiveRecord(Schema = "yuffiedb")]
    public class Users : ActiveRecordLinqBase<Users>
    {
        [PrimaryKey(PrimaryKeyType.Native, "Id")]
        public int Id { get; set; }

        [Property(Column = "Email")]
        public string Email { get; set; }

        [Property(Column = "Password")]
        public string Password { get; set; }

        [Property(Column = "Name")]
        public string Name { get; set; }

        [Property(Column = "SurName")]
        public string SurName { get; set; }

        [Property(Column = "CPF")]
        public string CPF { get; set; }

        [Property]
        public Roles Role { get; set; }

        [Property(Column = "Credits")]
        public double Credits { get; set; }

        [BelongsTo]
        public Users UserBoss { get; set; }


        public static void NewVendorFromForm(RegisterVendorFormData vendor, Users LoggedUser)
        {
            Users nvendor = new Users();
            nvendor.UserBoss = LoggedUser;
            nvendor.Name = vendor.Name;
            nvendor.SurName = vendor.SurName;
            nvendor.Password = Encryption.MD5(vendor.Password1);
            nvendor.Role = Roles.Vendedor;
            nvendor.CPF = vendor.CPFCNPJ;
            nvendor.Email = vendor.Email;
            nvendor.Save();
        }

        public static List<Users> ListVendors(Users LoggedUser)
        {
            List<Users> Vendors = Users.Queryable.Where(i => i.UserBoss == LoggedUser).ToList();
            return Vendors;
        }

        public class RegisterClientFormData
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string SurName { get; set; }
            public string Email { get; set; }
            public string Password1 { get; set; }
            public string Password2 { get; set; }
            public string CPFCNPJ { get; set; }
        }
        public class RegisterVendorFormData : RegisterClientFormData
        {
            public Users UserBoss { get; set; }
        }



    }
    public enum Roles
    {
        Usuario = 1,
        Vendedor = 2
    }
}