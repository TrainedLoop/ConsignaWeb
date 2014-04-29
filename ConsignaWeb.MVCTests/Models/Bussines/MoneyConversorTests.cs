using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsignaWeb.MVC.Models.Bussines;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace ConsignaWeb.MVC.Models.Bussines.Tests
{
    [TestClass()]
    public class MoneyConversorTests
    {
        [TestMethod()]
        public void RoundUP_Correct_()
        {
            Assert.AreEqual(25.54, MoneyConversor.RoundUp(25.5339));
        }
        [TestMethod()]
        public void RoundUP_Correct_1()
        {
            Assert.AreEqual(25.54, MoneyConversor.RoundUp(25.5399));
        }
        [TestMethod()]
        public void RoundUP_Correct_2()
        {
            Assert.AreEqual(25.53, MoneyConversor.RoundUp(25.5300));
        }
        [TestMethod()]
        public void RoundUP_Correct_3()
        {
            Assert.AreEqual(25.54, MoneyConversor.RoundUp(25.5350));
        }
    }
}
