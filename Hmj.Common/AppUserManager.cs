using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Hmj.Common
{
    public class AppUserManager
    {
        private AppUserManager()
        {
            InitData();
        }

        private static AppUserManager _manager;
        private static object lockobj = new object();
        private static Dictionary<string, AppUserInfo> _dictDB;
        /// <summary>
        /// 获取单例
        /// </summary>
        /// <returns>AppUersManager 的示例</returns>
        public static AppUserManager Create()
        {
            if (_manager == null)
            {
                lock (lockobj)
                {
                    if (_manager == null)
                    {
                        _manager = new AppUserManager();
                    }
                }
            }
            return _manager;
        }

        public AppUserInfo Get(string id)
        {
            if (_dictDB.ContainsKey(id))
            {
                return _dictDB[id];
            }
            return null;
        }

        private void InitData()
        {
            _dictDB = new Dictionary<string, AppUserInfo>();
            string rootpath = AppDomain.CurrentDomain.BaseDirectory;
            if (rootpath.EndsWith("/") || rootpath.EndsWith("\\"))
            {
                rootpath = rootpath.Remove(rootpath.Length - 1, 1);
            }
            XElement xele = XElement.Load(string.Concat(rootpath, "\\Configs\\AppUser.config"));
            IEnumerable<XElement> appsxes = GetXElements(xele, "AppUser");
            foreach (XElement xe in appsxes)
            {
                AppUserInfo app = new AppUserInfo();
                app.AppID = GetValue(xe, "AppID");
                app.AppSecret = GetValue(xe, "AppSecret");
                if (!_dictDB.ContainsKey(app.AppID))
                {
                    _dictDB.Add(app.AppID, app);
                }
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

    public class AppUserInfo
    {
        public string AppID { get; set; }

        public string AppSecret { get; set; }
    }
}