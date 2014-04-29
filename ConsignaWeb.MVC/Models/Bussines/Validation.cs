using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using ConsignaWeb.MVC.Models.Repository;

namespace ConsignaWeb.MVC.Models.Bussines
{
    public class Validation
    {

        #region Register Forms
        private string EmailString(string email)
        {
            if (!string.IsNullOrEmpty(email))
            {

                Regex rg = new Regex(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$");

                if (rg.IsMatch(email))
                {
                    return null;
                }
                else
                {
                    return "Email Inválido!";
                }
            }
            else
                return "Digite seu email";
        }
        private string ClientEmailInDB(string email)
        {
            var testemail = Users.Queryable.Where(i => i.Email == email).Count();

            if (testemail == 0)
            {
                return null;
            }
            return "Email Ja Registrado";
        }
        public void ClientEmail(string email)
        {
            if (EmailString(email) != null)
            {
                throw new Exception(EmailString(email));
            }

            if (ClientEmailInDB(email) != null)
            {
                throw new Exception(ClientEmailInDB(email));
            }
        }

        public void Commission(string strCommission)
        {
            decimal Commission = -1;
            strCommission = strCommission.Replace(',', '.');
            try
            {
                Commission = decimal.Parse(strCommission, CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                throw new Exception("Valor de comissão invalido");
            }
            if (Commission < 0 && Commission > 100)
            {
                throw new Exception("Valor de comissão invalido" + Commission);
            }

        }

        public void Name(string Name, string erroMessage = "None Invalido, Digite seu nome completo")
        {
            string a;
            try
            {
                a = Name.Trim(' ');
            }
            catch (Exception)
            {
                throw new Exception(erroMessage);
            }
            if (a.Length < 3)
            {
                throw new Exception(erroMessage);
            }
        }
        public void Password(string Password1, string Password2, bool empty = false)
        {
            if (!string.IsNullOrEmpty(Password1) && !string.IsNullOrEmpty(Password2))
            {
                if (Password1 != Password2)
                { throw new Exception("Senhas não conferem"); }

                if (Password1.Length < 6)
                { throw new Exception("A senha deve ter mais de 6 digitos"); }
            }
            else if (empty == false)
                throw new Exception("Digite uma senha");
        }

        public void CPFnCNPJ(string cpfcnpj)
        {
            if (!string.IsNullOrEmpty(cpfcnpj))
            {
                int[] d = new int[14];
                int[] v = new int[2];
                int j, i, soma;
                string Sequencia, SoNumero;

                SoNumero = Regex.Replace(cpfcnpj, "[^0-9]", string.Empty);

                //verificando se todos os numeros são iguais
                if (new string(SoNumero[0], SoNumero.Length) == SoNumero) throw new Exception("CPF ou CNPJ Invalido");

                // se a quantidade de dígitos numérios for igual a 11
                // iremos verificar como CPF
                if (SoNumero.Length == 11)
                {
                    for (i = 0; i <= 10; i++) d[i] = Convert.ToInt32(SoNumero.Substring(i, 1));
                    for (i = 0; i <= 1; i++)
                    {
                        soma = 0;
                        for (j = 0; j <= 8 + i; j++) soma += d[j] * (10 + i - j);

                        v[i] = (soma * 10) % 11;
                        if (v[i] == 10) v[i] = 0;
                    }
                    if (!(v[0] == d[9] & v[1] == d[10]))
                        throw new Exception("CPF ou CNPJ Invalido");
                }
                // se a quantidade de dígitos numérios for igual a 14
                // iremos verificar como CNPJ
                else if (SoNumero.Length == 14)
                {
                    Sequencia = "6543298765432";
                    for (i = 0; i <= 13; i++) d[i] = Convert.ToInt32(SoNumero.Substring(i, 1));
                    for (i = 0; i <= 1; i++)
                    {
                        soma = 0;
                        for (j = 0; j <= 11 + i; j++)
                            soma += d[j] * Convert.ToInt32(Sequencia.Substring(j + 1 - i, 1));

                        v[i] = (soma * 10) % 11;
                        if (v[i] == 10) v[i] = 0;
                    }
                    if (!(v[0] == d[12] & v[1] == d[13]))
                        throw new Exception("CPF ou CNPJ Invalido");
                }
                // CPF ou CNPJ inválido se
                // a quantidade de dígitos numérios for diferente de 11 e 14
                else throw new Exception("CPF ou CNPJ Invalido");
            }
            else
                throw new Exception("Digite seu CPF/CNPJ");
        }

        public void Money(string strMoney, double Max = double.MaxValue)
        {
            if (string.IsNullOrEmpty(strMoney))
            {
                throw new Exception("Digite um valor");
            }
            Double Money = 0;
            strMoney = strMoney.Replace(',', '.');
            try
            {
                Money = Double.Parse(strMoney, CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                throw new Exception("Valor invalido");
            }
            if (Money > Max)
                throw new Exception("O Valor não pode ultrapassar " + Max.ToString("C"));
        }
        public void Amount(string strAmount, int max = int.MaxValue)
        {
            int Amount = 0;
            Amount = int.Parse(strAmount);
            if (Amount < 0)
                throw new Exception("Quantidade invalida");
            if (Amount > max)
                throw new Exception("A quantidade não deve exceder " + max);
        }
        public void DateNotToday(string date)
        {
            DateTime outDate = DateTime.Now;
            if (DateTime.TryParse(date, out outDate))
            {
                outDate = DateTime.Parse(date);
                if (outDate < DateTime.Now)
                    throw new Exception("Data inválida");
            }
            else
                throw new Exception("Formato de data inválido");
        }
        #endregion Register Forms
    }
}