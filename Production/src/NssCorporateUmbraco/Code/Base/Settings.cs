using System.Configuration;

namespace NssCorporateUmbraco.Code.Base
{
    public class Settings
    {
        public static string BaseUrl
        {
            get
            {
                string baseUrl = GetAppString("BaseUrl");

                if (baseUrl.EndsWith("/")) { return baseUrl; }

                return baseUrl + "/";
            }
        }

        public static string BaseUrlWithoutSlash
        {
            get
            {
                string baseUrl = GetAppString("BaseUrl");

                if (baseUrl.EndsWith("/")) { return baseUrl.TrimEnd('/'); }

                return baseUrl;
            }
        }

        public static string DefaultTitleTag
        {
            get { return GetAppString("DefaultTitleTag"); }
        }

        public static string DefaultFeatures
        {
            get { return GetAppString("Nodes:DefaultFeatures"); }
        }

        private static string GetAppString(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        public static string DefaultEmailTemplate
        {
            get { return GetAppString("DefaultEmailTemplate"); }
        }
    }
}