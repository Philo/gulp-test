using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using NssCorporateUmbraco.Code.GatherContent.Models;
using Umbraco.Core;

namespace NssCorporateUmbraco.Code.GatherContent
{
    public class ImportPages
    {
        public static IList<MappedPageContent> CreatePages(string gatherContentProjectId, int startPageId)
        {
            var rawData = APIHelper.GetPagesByProjectRaw(gatherContentProjectId);
            var gatherContentPages = GetGatherContentPages(rawData);
            var mappedpageContent = GetMappedPageContent(gatherContentPages);

            return BuildPages(mappedpageContent, startPageId);
        }

        public static GatherContentPages GetGatherContentPages(string rawData)
        {
            return JsonConvert.DeserializeObject<GatherContentPages>(rawData);
        }
        
        public static IList<MappedPageContent> GetMappedPageContent(GatherContentPages gatherContentPages)
        {
            IList<MappedPageContent> mappedContent = new List<MappedPageContent>();

            foreach (var page in gatherContentPages.pages)
            {
                if (page.parent_id == "0")
                {
                    var item = new MappedPageContent();
                    item.Name = page.name;
                    item.GatherContentId = page.id;
                    item.Children = new List<MappedPageContent>();

                    mappedContent.Add(item);
                }
                else
                {
                    AddChild(page, mappedContent);
                }
            }

            return mappedContent;

        }

        private static void AddChild(GatherContentPageItem page, IEnumerable<MappedPageContent> mappedPageContent)
        {
            foreach (var mapped in mappedPageContent)
            {
                if (mapped.GatherContentId == page.parent_id)
                {
                    var item = new MappedPageContent();
                    item.Name = page.name;
                    item.GatherContentId = page.id;
                    item.Children = new List<MappedPageContent>();

                    mapped.Children.Add(item);
                    break;
                }
                else
                {
                    AddChild(page, mapped.Children);
                }
            }
        }

        public static IList<MappedPageContent> BuildPages(IList<MappedPageContent> mappedPageContent, int homeNodeId)
        {
            var contentService = ApplicationContext.Current.Services.ContentService;

            foreach (var page in mappedPageContent)
            {
                var newContent = contentService.CreateContent(page.Name, homeNodeId, "docArticle", 0);
                newContent.SetValue("gatherContentId", page.GatherContentId);
                newContent.SetValue("gatherContentImportDate", DateTime.Now.ToUniversalTime());

                contentService.SaveAndPublishWithStatus(newContent, 0, true);

                BuildPages(page.Children, newContent.Id);
            }

            return mappedPageContent;

        }
    }
}