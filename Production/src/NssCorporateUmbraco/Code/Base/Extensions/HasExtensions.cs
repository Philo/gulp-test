namespace NssCorporateUmbraco.Code.Base.Extensions
{
    public static class HasExtensions
    {
        public static bool HasValue(this object obj)
        {
            var s = obj as string;
            if (s != null)
                return !string.IsNullOrEmpty(s) && !string.IsNullOrWhiteSpace(s);

            return obj != null;
        }
    }
}