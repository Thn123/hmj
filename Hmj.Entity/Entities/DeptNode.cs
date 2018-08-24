using Newtonsoft.Json.Linq;

namespace Hmj.Entity.Entities
{
    public class DeptNode
    {
        public string id {get;set;}
        public string name { get; set; }
        public string parentId { get; set; }
        public string order { get; set; }

        public DeptNode(JObject jObject)
        {
            this.id = jObject["id"].ToString();
            this.name = jObject["name"].ToString();
            this.parentId = jObject["parentid"].ToString();
            this.order = jObject["order"].ToString();
        }
        public DeptNode() { }
    }
}
