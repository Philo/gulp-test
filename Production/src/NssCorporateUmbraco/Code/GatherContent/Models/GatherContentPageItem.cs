namespace NssCorporateUmbraco.Code.GatherContent.Models
{
    public class GatherContentPageItem
    {
        public string id { get; set; }
        public string project_id { get; set; }
        public string name { get; set; }
        public string parent_id { get; set; }
        public string type { get; set; }
        public string position { get; set; }
        public string custom_state_id { get; set; }
        public string config { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public string due_date { get; set; }
        public string template_id { get; set; }
    }
}