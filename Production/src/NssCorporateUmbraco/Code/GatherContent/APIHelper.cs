using System;
using System.Net;

namespace NssCorporateUmbraco.Code.GatherContent
{
    public class APIHelper
    {
        private const string apiKey = "d6d5380884940c2d2e689b2062d3cb9777c47ebc95107ff97f7061346aa11fde";
        private const string apiPassword = "x";

        public static string GetPagesByProjectRaw(string gatherContentProjectId)
        {
            return GetRawJson("get_pages_by_project", gatherContentProjectId);
        }

        public static string GetPageRaw(string gatherContentPageId)
        {
            return GetRawJson("get_page", gatherContentPageId);
        }

        private static string GetRawJson(string functionName, string id)
        {
            string json = "";
            string URI = "https://stormid.gathercontent.com/api/0.4/" + functionName;

            var baseAddress = new Uri(string.Format("https://stormid.gathercontent.com/api/0.4/"));
            var credentialCache = new CredentialCache { { baseAddress, "Digest", new NetworkCredential(apiKey, apiPassword) } };

            using (WebClient wc = new WebClient())
            {
                wc.Credentials = credentialCache;
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";

                json = wc.UploadString(URI, "id=" + id);

                return json;
            }
        }
    }
}