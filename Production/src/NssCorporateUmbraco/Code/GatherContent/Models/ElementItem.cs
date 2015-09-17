using System.Collections.Generic;

namespace NssCorporateUmbraco.Code.GatherContent.Models
{
    public class ElementItem
    {
        public string type { get; set; }
        public string name { get; set; }
        public bool required { get; set; }
        public string label { get; set; }
        public string value { get; set; }
        public string microcopy { get; set; }
        public IList<OptionItem> options { get; set; }
        public string limit_type { get; set; }
        public string limit { get; set; }
        public bool plain_text { get; set; }
    }
}