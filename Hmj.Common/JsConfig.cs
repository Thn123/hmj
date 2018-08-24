using System.Collections.Generic;
using System.Web;
using System.Xml;

namespace Hmj.Common
{
    public class JsConfig
    {
        const string jsxmlfile = "~/Js.xml";

        private static IDictionary<string, string> dict;
        private static void InitDist()
        {
            HttpContext context = HttpContext.Current;
            dict = new Dictionary<string, string>();
            string jsxml = context.Server.MapPath(jsxmlfile);
            XmlDocument doc = new XmlDocument();
            doc.Load(jsxml);
            var nodes = doc.DocumentElement.SelectNodes("/JsList/js");
            foreach (XmlNode node in nodes)
            {
                dict.Add(node.Attributes["key"].Value, node.Attributes["value"].Value);
            }

        }
        public static string GetJsUrl(string key)
        {
            if (dict == null)
            {
                InitDist();
            }
            if (dict.ContainsKey(key))
            {
                return dict[key];
            }
            return "";
        }
    }
    
}
