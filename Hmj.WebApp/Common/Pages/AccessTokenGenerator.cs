using Hmj.ExtendAPI.WeiXin;
using Hmj.ExtendAPI.WeiXin.Models;
using System;
using System.IO;
using System.Text;
using System.Web;
using System.Xml;

namespace Hmj.WebApp.Common.Pages
{
    public class AccessTokenGenerator
    {
        private static AccessTokenGenerator _instance;
        private static object _lockobject = new object();

        private AccessTokenGenerator()
        {

        }

        /// <summary>
        /// 单例对象
        /// </summary>
        /// <returns></returns>
        public static AccessTokenGenerator Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lockobject)
                    {
                        if (_instance == null)
                        {
                            _instance = new AccessTokenGenerator();
                        }
                    }
                }
                return _instance;
            }
        }

        public string GetQYAccessToken()
        {
            // 读取XML文件中的数据
            string filepath = HttpContext.Current.Server.MapPath("~/Configs/AccessTokenData.config");
            StreamReader str = new StreamReader(filepath, Encoding.UTF8);
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(str);
            str.Close();
            str.Dispose();

            var rootNode = xmlDocument.SelectSingleNode("AccessTokenInfo");
            string token = rootNode.SelectSingleNode("Token").InnerText;
            DateTime AccessExpires = Convert.ToDateTime(rootNode.SelectSingleNode("Expires").InnerText);

            if (DateTime.Now >= AccessExpires)
            {
                AccessTokenResponse response = WXQYClientServiceApi.Create().GetAccessToken();
                if (response != null)
                {

                    rootNode.SelectSingleNode("Token").InnerText = response.AccessToken;
                    DateTime _accessExpires = DateTime.Now.AddSeconds(response.ExpiresIn);
                    rootNode.SelectSingleNode("Expires").InnerText = _accessExpires.ToString();
                    xmlDocument.Save(filepath);
                    token = response.AccessToken;
                }
                else
                {
                    token = "";
                    throw new Exception("获取AccessToken异常。");
                }
            }
            return token;
        }
    }
}