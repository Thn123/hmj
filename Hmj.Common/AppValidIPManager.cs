using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Hmj.Common
{
    public class AppValidIPManager
    {
        private AppValidIPManager()
        {
            InitData();
        }

        private static AppValidIPManager _manager;
        private static object lockobj = new object();
        private static List<AppValidIPInfo> _dictDB;
        /// <summary>
        /// 获取单例
        /// </summary>
        /// <returns>AppUersManager 的示例</returns>
        public static AppValidIPManager Create()
        {
            if (_manager == null)
            {
                lock (lockobj)
                {
                    if (_manager == null)
                    {
                        _manager = new AppValidIPManager();
                    }
                }
            }
            return _manager;
        }

        public List<AppValidIPInfo> GetAllIP()
        {
            return _dictDB;
        }

        private void InitData()
        {
            _dictDB = new List<AppValidIPInfo>();
            string rootpath = AppDomain.CurrentDomain.BaseDirectory;
            if (rootpath.EndsWith("/") || rootpath.EndsWith("\\"))
            {
                rootpath = rootpath.Remove(rootpath.Length - 1, 1);
            }
            XElement xele = XElement.Load(string.Concat(rootpath, "\\Configs\\AppValidIP.config"));
            IEnumerable<XElement> appsxes = GetXElements(xele, "AppIP");
            foreach (XElement xe in appsxes)
            {
                AppValidIPInfo app = new AppValidIPInfo();
                app.Desc = GetValue(xe, "Desc");
                app.IPAddress = GetValue(xe, "IPAddress");
                _dictDB.Add(app);
            }
        }

        private string GetValue(XElement xele, string name)
        {
            XName xname = XName.Get(name);
            XElement x = xele.Element(xname);
            return x.Value;
        }


        private string GetAttributeValue(XElement xele, string name)
        {
            XName xname = XName.Get(name);
            XAttribute xAttribute = xele.Attribute(xname);
            if (xAttribute == null) return String.Empty;
            return xAttribute.Value;
        }

        private IEnumerable<XElement> GetXElements(XElement xele, String name)
        {
            var partNos = from item in xele.Descendants(name) select item;
            return partNos;
        }
    }

    public class AppValidIPInfo
    {
        public string Desc { get; set; }

        public string IPAddress { get; set; }
    }
}