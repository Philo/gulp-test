using System;
using System.Collections.Generic;
using System.Linq;
using NssCorporateUmbraco.Code.Base.Constants;
using NssCorporateUmbraco.Code.Base.Conversion.Models;
using NssCorporateUmbraco.Code.Base.ModelFactories;
using RJP.MultiUrlPicker.Models;
using Umbraco.Core.Models;
using Umbraco.Web;
using UmbracoExamine.DataServices;

namespace NssCorporateUmbraco.Code.Base.Extensions
{
    public static class PublishedContentExtensions
    {
        public static bool IsNullOrEmpty(this IPublishedContent node)
        {
            if (node == null)
                return true;
            return node.Id == -1;
        }

        public static string Get(this IPublishedContent content, string alias)
        {
            if (content.IsNullOrEmpty())
                return string.Empty;
            var prop = content.GetPropertyValue(alias);
            return prop == null ? string.Empty : prop.ToString();
        }


        public static IPublishedContent RootNode(this IPublishedContent node)
        {
            return node.AncestorOrSelf(1);
        }

        public static IPublishedContent SecondLevelNode(this IPublishedContent node)
        {
            return node.AncestorOrSelf(2);
        }

        public static string FinalUrl(this IPublishedContent content)
        {
            var property = content.ConvertUrlSingle(Up.Mixins.PageInfo.UrlOverride);
            if (property == null)
                return string.Empty;
            var bounceUrl = property.Url;
            return bounceUrl.HasValue() ? bounceUrl : content.Url;
        }

        public static UrlTarget ConvertUrlSingle(this IPublishedContent content, string propertyName)
        {
            if (content.IsNullOrEmpty())
                return null;
            var picker = content.GetPropertyValue<MultiUrls>(propertyName);
            if (picker == null || !picker.Any())
                return null;
            var result = picker.FirstOrDefault();
            if (result == null)
                return null;
            return UrlTargetFactory.Create(result);
        }

        public static List<IPublishedContent> PathAsList(this IPublishedContent content)
        {
            var pathList = new List<IPublishedContent>();
            var umbracoHelper = new UmbracoHelper(UmbracoContext.Current);

            foreach (var pageId in content.Path.Split(','))
            {
                var result = ConversionService.ConvertInt(pageId);
                if (result>0 && result!=content.Id)
                {
                    pathList.Add(umbracoHelper.TypedContent(pageId));
                }
            }

            return pathList;
        }

        public static bool IsValid(this IPublishedContent content)
        {
            return content.HasValue() && content.Id>0;
        }
    }
}