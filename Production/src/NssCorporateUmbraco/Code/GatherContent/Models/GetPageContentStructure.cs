using System.Collections.Generic;

namespace NssCorporateUmbraco.Code.GatherContent.Models
{
    public class GetPageContentStructure
    {
        public string label { get; set; }
        public string name { get; set; }
        public bool hidden { get; set; }
        public IList<ElementItem> elements { get; set; }
    }
}