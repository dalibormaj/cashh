using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Victory.VCash.Infrastructure.Common
{
    public static class StringExtensions
    {
        public static string MaskRight(this string value, string mask)
        {
            var result = value;
            var valueLength = value?.Length ?? 0;

            if (string.IsNullOrEmpty(result))
                return string.Empty;

            if (string.IsNullOrEmpty(mask))
                return result;

            if (result.Length <= mask.Length)
                return mask.Substring(0, result.Length);

            result = result.Insert(result.Length - mask.Length, mask).Substring(0, valueLength);
            return result;
        }

        public static string Left(this string value, int length)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            var valueLength = value.Length;
            if(valueLength <= length)
                return value;

            return value.Substring(0, length);
        }

        public static string Right(this string value, int length)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            var valueLength = value.Length;
            if (valueLength <= length)
                return value;

            return value.Substring(valueLength - length, length);
        }

        public static bool IsNumeric(this string number)
        {
            return long.TryParse(number, out long value);
        }
    }
}
