using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ath_p4_proj1
{
    internal class Validator
    {
        public static bool NumericId(string? value, int maxLength = Int32.MaxValue)
        {
            var regex = new Regex("^[0-9]+$");

            if (String.IsNullOrEmpty(value)) return false;
            if (!regex.IsMatch(value)) return false;
            if (value.Length > maxLength) return false;
            return true;
        }

        public static bool PhoneNumber(string? value, int length = 9)
        {
            var regex = new Regex("^[0-9]+$");

            if (String.IsNullOrEmpty(value)) return false;
            if (!regex.IsMatch(value)) return false;
            if(value.Length != length) return false;
            return true;
        }

        public static bool Email(string? value)
        {
            var regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");

            if (String.IsNullOrEmpty(value)) return false;
            if (!regex.IsMatch(value)) return false;
            return true;
        }

        public static bool Name(string? value)
        {
            var regex = new Regex("^[A-Za-z][A-Za-z0-9_]{1,50}$");

            if (String.IsNullOrEmpty(value)) return false;
            if (!regex.IsMatch(value)) return false;
            return true;
        }
    }
}
