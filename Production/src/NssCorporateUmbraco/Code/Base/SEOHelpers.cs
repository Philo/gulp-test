using Umbraco.Core.Models;
using Umbraco.Web;
using NssCorporateUmbraco.Code.Base.Constants;
using NssCorporateUmbraco.Code.Base;

namespace NssCorporateUmbraco.Code.Base
{

    public class SEOHelpers
    {

        public static string TitleTag(IPublishedContent currentPage)
        {
            if (currentPage.HasProperty(Up.Mixins.Seo.TitleTag) && currentPage.HasValue(Up.Mixins.Seo.TitleTag)) { return currentPage.GetPropertyValue<string>(Up.Mixins.Seo.TitleTag); }

            if (currentPage.Level > 1) { return currentPage.Name; }

            return Settings.DefaultTitleTag;
        }

        public static string OGTitle(IPublishedContent currentPage)
        {
            if (currentPage.HasProperty(Up.Mixins.Seo.OgTitle) && currentPage.HasValue(Up.Mixins.Seo.OgTitle)) { return currentPage.GetPropertyValue<string>(Up.Mixins.Seo.OgTitle); }

            return TitleTag(currentPage);
        }

        public static string MetaDescription(IPublishedContent currentPage)
        {
            if (currentPage.HasProperty(Up.Mixins.Seo.MetaDescription) && currentPage.HasValue(Up.Mixins.Seo.MetaDescription)) { return currentPage.GetPropertyValue<string>(Up.Mixins.Seo.MetaDescription); }

            return "";
        }

        public static string OGDescription(IPublishedContent currentPage)
        {
            if (currentPage.HasProperty(Up.Mixins.Seo.OgDescription) && currentPage.HasValue(Up.Mixins.Seo.OgDescription)) { return currentPage.GetPropertyValue<string>(Up.Mixins.Seo.OgDescription); }

            return MetaDescription(currentPage);
        }

        public static string OGImage(IPublishedContent currentPage)
        {
            if (currentPage.HasProperty(Up.Mixins.Seo.OgImage) && currentPage.HasValue(Up.Mixins.Seo.OgImage))
            {
                return @"<meta property=""og:image"" content=""" + Settings.BaseUrlWithoutSlash + new UmbracoHelper(UmbracoContext.Current).TypedMedia(currentPage.GetPropertyValue<int>(Up.Mixins.Seo.OgImage)).GetCropUrl(CropSize.OGImage_1200_x_630) + @""">";
            }

            return "";
        }
    }
}