using System.Collections.Generic;

namespace NssCorporateUmbraco.Code.GatherContent.Models
{
    public class MappedPageContent
    {
        public string GatherContentId { get; set; }
        public IList<MappedPageContent> Children { get; set; }
        public string Name { get; set; }
    }
}