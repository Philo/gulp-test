using System.Collections.Generic;

namespace NssCorporateUmbraco.Code.GatherContent.Models
{
    public class GatherContentPages
    {
        public bool success { get; set; }
        public IList<GatherContentPageItem> pages { get; set; }
    }
}