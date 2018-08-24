using Hmj.Common.Utils;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using WeChatCRM.Common.Utils;

namespace Hmj.ExtendAPI
{
    public abstract class ServiceApiBase
    {
        protected abstract string ApiUrl { get; }

        protected string DoJSONRequest(string path, Dictionary<string, object> data, string method = "POST")
        {
            string strdata = JsonHelper.SerializeObject(data);
            return this.DoJSONRequest(path, strdata);
        }

        protected string DoJSONRequest(string path, string data, string method = "POST")
        {
            string url = ApiUrl + path;
            String guid = IdUtils.NewGuid();
            Trace("请求URL：" + url + "------" + guid);
            string result = NetHelper.HttpRequest(url, data, method, 30000, Encoding.UTF8, ContentTypes.JSON);
            Trace("响应结果：" + result + "------" + guid);
            return result;
        }

        protected string DoFormRequest(string path, Dictionary<string, object> data, string method = "POST")
        {
            string strdata = DictToStr(data, "&", true);

            string url = ApiUrl + path;
            String guid = IdUtils.NewGuid();
            Trace("请求URL：" + url + "------" + guid);
            string result = NetHelper.HttpRequest(url, strdata, method, 60000, Encoding.UTF8, ContentTypes.FORM);
            Trace("响应结果：" + result + "------" + guid);
            return result;
        }

        /// <summary>
        /// 字典转字符串
        /// </summary>
        /// <param name="dict">字典类型数据</param>
        /// <param name="str_join">连接字符串</param>
        /// <param name="isUrlEncode">是否需要url编码</param>
        /// <returns>字典数据的key和value组成的字符串</returns>
        private string DictToStr(Dictionary<string, object> dict, string str_join, bool isUrlEncode)
        {
            //连接字符串
            str_join = str_join == null ? "&" : str_join;
            StringBuilder result = new StringBuilder();
            string value = string.Empty;
            int i = 0;
            foreach (KeyValuePair<string, object> kv in dict)
            {
                string v = kv.Value != null ? kv.Value.ToString() : "";
                value = isUrlEncode == true ? HttpUtility.UrlEncode(v, Encoding.UTF8) : v;
                result.AppendFormat("{0}{1}={2}", i > 0 ? str_join : "", kv.Key, value);
                i++;
            }

            return result.ToString();
        }

        protected void Trace(string log)
        {
            LogManager.GetLogger("Hmj.Log").Trace(log);
        }
    }
}
