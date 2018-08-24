using Hmj.Entity.Jsons;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Xml;

namespace Hmj.Extension
{
    public static class ControllerExtension
    {
        public static List<JsonTreeNode> GetXmlMenu(this Controller controller)
        {
            string menuxml = controller.Server.MapPath("~/MenuData.xml");
            XmlDocument doc = new XmlDocument();
            doc.Load(menuxml);
            var nodes = doc.DocumentElement.SelectNodes("/MenuData/Menu");
            return ConvertXmlNodeToJsonNode(nodes);
        }

        private static List<JsonTreeNode> ConvertXmlNodeToJsonNode(XmlNodeList nodes)
        {
            List<JsonTreeNode> treenodelist = new List<JsonTreeNode>();
            foreach (XmlNode node in nodes)
            {
                JsonTreeNode treenode = new JsonTreeNode();
                treenode.id = node.Attributes["id"].Value;
                treenode.text = node.Attributes["text"].Value;
                treenode.value = node.Attributes["url"].Value;
                treenode.isexpand = node.Attributes["isexpand"].Value == "true";
                treenode.showcheck = false;
                treenode.complete = true;
                treenode.hasChildren = node.ChildNodes != null && node.ChildNodes.Count > 0;

                if (treenode.hasChildren)
                {
                    treenode.ChildNodes.AddRange(ConvertXmlNodeToJsonNode(node.ChildNodes));
                }
                treenodelist.Add(treenode);
            }
            return treenodelist;
        }
    }
}
