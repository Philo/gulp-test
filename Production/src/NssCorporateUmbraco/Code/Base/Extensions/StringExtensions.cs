using System;
using System.Collections.Generic;
using System.Linq;

namespace NssCorporateUmbraco.Code.Base.Extensions
{

    public static class EnumerableExtensions
    {
        public static bool IsEmpty<T>(this IEnumerable<T> items)
        {
            if (items != null && items.Any())
                return false;
            return true;
        }

        public static bool IsNotEmpty<T>(this IEnumerable<T> items)
        {
            return !items.IsEmpty();
        }
    }


    public static class StringExtensions
    {
        public static string ToCssClassName(this string input)
        {
            var split = input.AsArray(' ');

            var result = split.Select(item => item.Substring(0, 1)).ToList();

            return string.Join(string.Empty, result);
        }

        public static string[] AsArray(this string input, char splitChar = ',')
        {
            return input.Split(new[] { splitChar}, StringSplitOptions.RemoveEmptyEntries);
        }

    }
}