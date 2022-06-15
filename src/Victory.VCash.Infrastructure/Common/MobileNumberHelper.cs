using System;

namespace Victory.VCash.Infrastructure.Common
{
	public static class MobileNumberHelper
	{
		public static string Format(string phoneNumber, CountryPhoneCode code = CountryPhoneCode.Serbia, FormatAction action = FormatAction.ADD_COUNTRY_CODE)
		{
			if (string.IsNullOrEmpty(phoneNumber))
				throw new Exception("Phone number cannot be empty");

			phoneNumber = phoneNumber.Replace("-", "").Replace("/", "").Replace("\\", "").Replace("+","");

			if (!(phoneNumber.IsNumeric() && phoneNumber.Length > 6))
				throw new Exception("Invalid phone number");

			//remove leading 0
			phoneNumber = (phoneNumber.Substring(0, 1) == "0") ? phoneNumber.Substring(1, phoneNumber.Length - 1) : phoneNumber;

			//format
			if (action == FormatAction.ADD_COUNTRY_CODE)
			{
				var countryCodeLength = ((int)code).ToString().Length;
				if (phoneNumber.Substring(0, countryCodeLength) == ((int)code).ToString())
					phoneNumber = $"+{phoneNumber.Trim()}";
				if (!phoneNumber.Substring(0, 1).Equals("+"))
					phoneNumber = $"+{(int)code}{phoneNumber.Trim()}";
			}

			if (action == FormatAction.REMOVE_COUNTRY_CODE)
			{
				var countryCodeLength = ((int)code).ToString().Length;
				if (phoneNumber.Substring(0, countryCodeLength) == ((int)code).ToString())
					phoneNumber = $"0{phoneNumber.Remove(0, countryCodeLength)}";
				if (phoneNumber.Substring(0, countryCodeLength+1).Equals($"+{(int)code}"))
					phoneNumber = $"0{phoneNumber.Remove(0, countryCodeLength+1)}";
				if (!phoneNumber.Substring(0, 1).Equals("0"))
					phoneNumber = $"0{phoneNumber}";
			}

			return phoneNumber;
		}

		public static bool TryFormat(string phoneNumber, out string formatedPhoneNumber, CountryPhoneCode code = CountryPhoneCode.Serbia, FormatAction action = FormatAction.ADD_COUNTRY_CODE)
		{
			try
			{
				formatedPhoneNumber = Format(phoneNumber, code, action);
				return true;
			}
			catch
			{
				formatedPhoneNumber = null;
				return false;
			}
		}
	}

	public enum CountryPhoneCode : int
	{
		Serbia = 381
	}

	public enum FormatAction
    {
		ADD_COUNTRY_CODE,
		REMOVE_COUNTRY_CODE
    }
}
