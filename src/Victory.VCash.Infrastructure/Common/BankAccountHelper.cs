using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Victory.VCash.Infrastructure.Common
{
    public static class BankAccountHelper
    {
        public static string Format(string bankAccountNumber, bool withDashes = false)
        {
            var MIN_LENGTH = 6;
            var MAX_LENGTH = 18;
            bankAccountNumber = Regex.Replace(bankAccountNumber, "[^0-9]", "");

            if (bankAccountNumber.Length < MIN_LENGTH)
                throw new Exception("Bank account is too short");

            if (bankAccountNumber.Length > MAX_LENGTH)
                throw new Exception("Bank account is too long");

            var bank = bankAccountNumber.Left(3);
            var party = bankAccountNumber.Substring(3, bankAccountNumber.Length - 5).Insert(0, "0000000000000").Right(13);
            var controlNo = bankAccountNumber.Right(2);

            if(withDashes)
                return $"{bank}-{party}-{controlNo}";

            return $"{bank}{party}{controlNo}";
        }

        public static bool TryFormat(string bankAccountNumber, out string formatted, bool withDashes = false)
        {
            try
            {
                formatted = Format(bankAccountNumber, withDashes);
                return true;
            }
            catch
            {
                formatted = null;
                return false;
            }
        }

        public static string GetBankName(string bankAccountNumber)
        {
            if (string.IsNullOrEmpty(bankAccountNumber))
                return null;

            if (bankAccountNumber.Length < 3)
                return null;

            var bankCode = bankAccountNumber.Left(3);

            var allBanks = new Dictionary<string, string>()
            {
                { "105","AIK bank" },
                { "115","Mobi bank" },
                { "150","KBM Kragujevac" },
                { "155","Halkbank" },
                { "160","Banca Intesa" },
                { "165","Addiko bank" },
                { "170","Unicredit bank" },
                { "180","Alpha bank" },
                { "190","Jubmes bank" },
                { "200","Banka Poštanka štedionica" },
                { "205","Komercijalna banka" },
                { "220","Procredit banka" },
                { "250","Eurobank" },
                { "265","Raiffeisen bank" },
                { "275","OTP bank" },
                { "285","OTP bank" },
                { "310","NLB bank" },
                { "325","Vojvođanska banka" },
                { "330","Credit Agricole bank" },
                { "340","Erste bank" },
                { "360","MTS bank" },
            };

            allBanks.TryGetValue(bankCode, out string result);
            return result;
        }
    }


}
