using NssCorporateUmbraco.Code.Base.Conversion.Models;
using RJP.MultiUrlPicker.Models;

namespace NssCorporateUmbraco.Code.Base.ModelFactories
{
    public class UrlTargetFactory
    {
        public static UrlTarget Create(Link item)
        {
            return new UrlTarget
            {
                Target = item.Target,
                Title = item.Name,
                Url = item.Url
            };
        }
    }
}