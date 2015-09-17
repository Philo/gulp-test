using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using NssCorporateUmbraco.Code.GatherContent.Models;
using umbraco.NodeFactory;
using Umbraco.Core;
using Umbraco.Core.Models;

namespace NssCorporateUmbraco.Code.GatherContent
{
    public class ImportContent
    {
        public static void StartImport(IPublishedContent startPage)
        {
            foreach (var page in startPage.Children)
            {
                ImportPageContent(page.Id);
                StartImport(page);
            }
        }

        public static void ImportPageContent(int importPageId)
        {
            Node importPage = new Node(importPageId);

            var rawData = APIHelper.GetPageRaw(importPage.GetProperty("gatherContentId").ToString());
            var structure = GetPageContentStructure(rawData);
            var pageContent = DecodeFrom64(structure.page.config);
            var pageStructure = GetPageStructure(pageContent);
            AddPageContent(pageStructure, importPageId);

        }

        public static GatherContentPage GetPageContentStructure(string rawData)
        {
            return JsonConvert.DeserializeObject<GatherContentPage>(rawData);
        }

        public static string DecodeFrom64(string encodedData)
        {
            byte[] encodedDataAsBytes = Convert.FromBase64String(encodedData);
            string returnValue = System.Text.ASCIIEncoding.ASCII.GetString(encodedDataAsBytes);
            return returnValue;
        }

        public static IList<GetPageContentStructure> GetPageStructure(string rawData)
        {
            return JsonConvert.DeserializeObject<IList<GetPageContentStructure>>(rawData);
        }

        public static void AddPageContent(IList<GetPageContentStructure> pageStructure, int importPageId)
        {
            var contentService = ApplicationContext.Current.Services.ContentService;

            foreach (var tab in pageStructure)
            {
                foreach (var element in tab.elements)
                {
                    var content = contentService.GetById(importPageId);

                    var umbracoField = MapItems(element.label);

                    if (!string.IsNullOrEmpty(umbracoField))
                    {
                        var value = element.value;
                        if (!umbracoField.Equals("bodyText"))
                            value = umbraco.library.StripHtml(element.value).Trim();
                        content.SetValue(umbracoField, value);
                    }

                    if (element.options != null)
                    {
                        foreach (var option in element.options)
                        {
                            var umbracoOptionField = MapItems(option.label);

                            if (!string.IsNullOrEmpty(umbracoOptionField))
                            {
                                content.SetValue(umbracoOptionField, option.selected);
                            }
                        }
                    }

                    contentService.SaveAndPublishWithStatus(content, 0, true);
                }
            }
        }

        private static string MapItems(string gatherContentLabel)
        {
            switch (gatherContentLabel)
            {
                case "Body Text":
                    return "bodyText";
                case "Page Summary":
                    return "pageSummary";
                case "OG Title":
                    return "ogTitle";
                case "OG Description":
                    return "ogDescription";
                case "Meta Description":
                    return "metaDescription";
                case "H1 Page Title":
                    return "h1PageTitle";
                case "Primary Navigation":
                    return "primaryNavigation";
                case "Header Navigation":
                    return "headerNavigation";
                case "Footer Navigation":
                    return "footerNavigation";
                case "Hide From Navigation":
                    return "hideFromNavigation";
                case "Hide From Search":
                    return "hideFromSearch";
                default:
                    return string.Empty;
            }
        }
    }
}