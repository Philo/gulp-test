using System;

namespace NssCorporateUmbraco.Code.Base
{
    public class ConversionService
    {
        public static int ConvertInt(string value, int defaultReturnValue = -1)
        {
            int result;
            var parsed = int.TryParse(value, out result);
            return parsed ? result : defaultReturnValue;
        }

        public static bool ConvertBool(string value, bool defaultReturnValue = false)
        {
            bool result;
            var parsed = bool.TryParse(value, out result);
            return parsed ? result : defaultReturnValue;
        }

        public static string BoolToYesNo(bool value)
        {
            return value ? "Yes" : "No";
        }

        public static DateTime? ConvertDateTime(string value)
        {
            DateTime result;
            var parsed = DateTime.TryParse(value, out result);
            if (parsed)
                return result;
            return null;
        }

        public static bool ConvertBoolNumeric(string value)
        {
            return value.Equals("1");
        }

        public static double ConvertDouble(string value)
        {
            double result;
            var parsed = double.TryParse(value, out result);
            return parsed ? result : -1;
        }
    }
}