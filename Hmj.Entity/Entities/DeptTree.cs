using Newtonsoft.Json.Linq;
using System.Collections.Generic;
namespace Hmj.Entity.Entities
{
    //递归定义一棵树，树包括根节点和若干子树
    public class DeptTree
    {
        public DeptNode deptNode;
        public List<DeptTree> childTreeList = new List<DeptTree>();
        public DeptTree(JArray deptJArray,DeptNode rootNode)
        {
            deptNode = rootNode;
            foreach (JObject jNode in deptJArray)
            {
                if (jNode["parentid"].ToString() == deptNode.id)
                {
                    DeptNode childRootNode = new DeptNode(jNode);
                    childTreeList.Add(new DeptTree(deptJArray, childRootNode));
                }
            }
        }

        public DeptTree() { }

        //树的遍历
        public void traveralTree()
        {
        //    do();
            foreach(DeptTree childTree in childTreeList)
            {
                childTree.traveralTree();
            }
        }

        //获取树的所有节点的id
        //public List<string> getChildIds(List<string> ids)
        //{
        //    ids.Add(deptNode.id);
        //    foreach (DeptTree childTree in childTreeList)
        //    {
        //        childTree.getChildIds(ids);
        //    }
        //    return ids;
        //}
    }
}
