using System.Linq;
using System.Text.RegularExpressions;

namespace TransactionBankControl.Domain.ValueObjects
{
    public static class DocumentoValido
    {
        public static bool Validar(string documento)
        {
            documento = Regex.Replace(documento ?? "", "[^0-9]", "");

            if (documento.Length == 11)
                return ValidarCpf(documento);

            if (documento.Length == 14)
                return ValidarCnpj(documento);

            return false;
        }

        private static bool ValidarCpf(string cpf)
        {
            if (cpf.Length != 11 || cpf.Distinct().Count() == 1)
                return false;

            var soma1 = 0;
            for (int i = 0; i < 9; i++)
                soma1 += (cpf[i] - '0') * (10 - i);

            var digito1 = soma1 % 11;
            digito1 = digito1 < 2 ? 0 : 11 - digito1;

            var soma2 = 0;
            for (int i = 0; i < 10; i++)
                soma2 += (cpf[i] - '0') * (11 - i);

            var digito2 = soma2 % 11;
            digito2 = digito2 < 2 ? 0 : 11 - digito2;

            return cpf[9] - '0' == digito1 && cpf[10] - '0' == digito2;
        }

        private static bool ValidarCnpj(string cnpj)
        {
            if (cnpj.Length != 14 || cnpj.Distinct().Count() == 1)
                return false;

            var multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            var multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            var tempCnpj = cnpj.Substring(0, 12);
            var soma = 0;

            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

            var resto = (soma % 11);
            resto = resto < 2 ? 0 : 11 - resto;
            var digito1 = resto;

            tempCnpj += digito1;
            soma = 0;
            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];

            resto = (soma % 11);
            resto = resto < 2 ? 0 : 11 - resto;
            var digito2 = resto;

            return cnpj.EndsWith(digito1.ToString() + digito2.ToString());
        }
    }
}