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
    public class ValidationTests
    {
        Validation vld = new Validation();
        [TestMethod()]
        public void CPFnCNPJTest_correctCPFWithDots()
        {

            try
            {
                vld.CPFnCNPJ("140.370.827-48");
            }
            catch (Exception)
            {
                Assert.Fail();
            }

        }
        [TestMethod()]
        public void CPFnCNPJTest_correctCPFWithoutDots()
        {
            try
            {
                vld.CPFnCNPJ("14037082748");
            }
            catch (Exception)
            {
                Assert.Fail();
            }

        }

        [TestMethod()]
        public void CPFnCNPJTest_incorrectCPF()
        {
            try
            {
                vld.CPFnCNPJ("12037082748");
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual("CPF ou CNPJ Invalido", ex.Message);
            }

        }

        [TestMethod()]
        public void CommissionTest()
        {
            try
            {
                vld.Commission("44,3");
            }
            catch (Exception)
            {

                Assert.Fail();
            }
        }

        [TestMethod()]
        public void ValidEmailTest_FalseEmail()
        {
            try
            {
                vld.ClientEmail("ddaasdas.asd");
                Assert.Fail();
            }
            catch (Exception ex)
            {

                Assert.AreEqual("Email Inválido!", ex.Message);
            }


        }


        [TestMethod()]
        public void ValidEmailTest_FalseEmail2()
        {
            try
            {
                vld.ClientEmail("ddaasasd");
                Assert.Fail();
            }
            catch (Exception ex)
            {

                Assert.AreEqual("Email Inválido!", ex.Message);
            }
        }

        [TestMethod()]
        public void ValidEmailTest_FalseEmail3()
        {
            try
            {
                vld.ClientEmail("ddaa@sa.s@.d");
                Assert.Fail();
            }
            catch (Exception ex)
            {

                Assert.AreEqual("Email Inválido!", ex.Message);
            }

        }
        [TestMethod()]
        public void ValidEmailTest_FalseEmail4()
        {
            try
            {
                vld.ClientEmail("VendedorTeste01@gmail.comwq112312");
                Assert.Fail();
            }
            catch (Exception ex)
            {

                Assert.AreEqual("Email Inválido!", ex.Message);
            }


        }
        [TestMethod()]
        public void ValidarNomeTest_2caracteres()
        {
            try
            {
                vld.Name("aa");
                Assert.Fail();
            }
            catch (Exception ex)
            {

                Assert.AreEqual("None Invalido, Digite seu nome completo", ex.Message);
            }

        }

        [TestMethod()]
        public void ValidarNomeTest_4caracteres()
        {
            try
            {
                vld.Name("aaaa");
            }
            catch (Exception)
            {
                Assert.Fail();
            }

        }

        [TestMethod()]
        public void ValidarNomeTest_4espaços()
        {
            try
            {
                vld.Name("    ");
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual("None Invalido, Digite seu nome completo", ex.Message);
            }
        }

        [TestMethod()]
        public void ValidarCPFeCNPJTest_CPFValidoComPonto()
        {
            try
            {
                vld.CPFnCNPJ("140.370.827-48");
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void ValidarCPFeCNPJTest_CPFValidoSemPonto()
        {
            try
            {
                vld.CPFnCNPJ("14037082748");
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void ValidarCPFeCNPJTest_CPFinvalidoSemPonto()
        {
            try
            {
                vld.CPFnCNPJ("14037082749");
                Assert.Fail();
            }
            catch (Exception ex)
            {

                Assert.AreEqual("CPF ou CNPJ Invalido", ex.Message);
            }

        }

        [TestMethod()]
        public void ValidarCPFeCNPJTest_CNPJValido()
        {
            try
            {
                vld.CPFnCNPJ("36240547000101");
            }
            catch (Exception)
            {

                Assert.Fail();
            }

        }

        [TestMethod()]
        public void ValidarCPFeCNPJTest_CNPJInvalido()
        {
            try
            {
                vld.CPFnCNPJ("36240547000102");
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual("CPF ou CNPJ Invalido", ex.Message);

            }
        }

        [TestMethod()]
        public void VerficarDoubleTest_String()
        {
            try
            {
                vld.Money("asadasd");
                Assert.Fail();
            }
            catch (Exception ex)
            {

                Assert.AreEqual("Valor invalido", ex.Message);
            }

        }

        [TestMethod()]
        public void VerficarDoubleTest_double()
        {
            try
            {
                vld.Money("2.1");
            }
            catch (Exception)
            {

                Assert.Fail();
            }

        }

        [TestMethod()]
        public void VerficarDoubleTest_int()
        {
            try
            {
                vld.Money("2131");
            }
            catch (Exception)
            {
                Assert.Fail();
            }

        }

        [TestMethod()]
        public void VerficarDoubleTest_int_string()
        {
            try
            {
                vld.Money("222aaaaaa222222222aaa22");
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual("Valor invalido", ex.Message);
            }

        }

        [TestMethod()]
        public void DateNowTest_correct()
        {
            try
            {
                vld.DateNotToday(DateTime.Now.AddDays(1).ToShortDateString());
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }


        [TestMethod()]
        public void DateNowTest_Now()
        {
            try
            {
                vld.DateNotToday(DateTime.Now.ToShortDateString());
                Assert.Fail();
            }
            catch (Exception ex)
            {

                Assert.AreEqual("Data inválida", ex.Message);
            }

        }

        [TestMethod()]
        public void DateNowTest_yesterday()
        {
            try
            {
                vld.DateNotToday(DateTime.Now.ToShortDateString());
                Assert.Fail();
            }
            catch (Exception ex)
            {

                Assert.AreEqual("Data inválida", ex.Message);
            }

        }

        [TestMethod()]
        public void DateNowTest_InvalidFormat()
        {
            try
            {
                vld.DateNotToday("5555/555/6666");
                Assert.Fail();
            }
            catch (Exception ex)
            {

                Assert.AreEqual("Formato de data inválido", ex.Message);
            }

        }
    }
}
