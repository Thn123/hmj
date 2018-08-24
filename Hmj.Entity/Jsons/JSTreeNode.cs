using System.Collections.Generic;

namespace Hmj.Entity.Jsons
{
    public class JSTreeNode
    {
        public string id { get; set; }

        public string text { get; set; }

        public bool hasChildren { get; set; }

        //public List<JSTreeNode> children { get; set; }
        private List<JSTreeNode> _children;
        public List<JSTreeNode> children
        {
            get
            {
                if (_children == null)
                {
                    _children = new List<JSTreeNode>();
                }
                return _children;
            }
        }

        public string type { get; set; }

        public string icon { get; set; }

        public Dictionary<string, string> data { get; set; }
    }
}
