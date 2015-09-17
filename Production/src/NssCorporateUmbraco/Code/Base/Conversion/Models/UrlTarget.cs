
namespace NssCorporateUmbraco.Code.Base.Conversion.Models
{
    public class UrlTarget
    {
        public string Url { get; set; }
        public string Target { get; set; }
        public string Title { get; set; }

        public string DisplayName
        {
            get { return string.IsNullOrEmpty(Title) ? Url : Title; }
        }
    }
}