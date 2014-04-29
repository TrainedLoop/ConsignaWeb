using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConsignaWeb.MVC.Models.Bussines
{
    public class MoneyConversor
    {
        public static double RoundUp(double valor)
        {
            
            return Math.Round(valor+0.004, 2, MidpointRounding.AwayFromZero);
        }

        public static double RemoveComission(double valor, double comission)
        {
            var a = valor - (valor / 100 * comission);
            return valor - (valor / 100 * comission);
        }
    }
}