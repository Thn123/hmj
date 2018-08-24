using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Hmj.ScheduleService.Code
{
    public abstract class ServiceApiBase
    {
        protected abstract string ApiUrl { get; }

        protected string DoJSONRequest(string path, Dictionary<string, object> data, string method = "POST")
        {
            string strdata = JsonHelper.SerializeObject(data);


            string url = ApiUrl + path;
            return Net.HttpRequest(url, strdata, method, 20000, Encoding.UTF8, ContentTypes.JSON);
        }

        protected string DoFormRequest(string path, Dictionary<string, object> data, string method = "POST")
        {
            string strdata = DictToStr(data, "&", true);

            string url = ApiUrl + path;
            return Net.HttpRequest(url, strdata, method, 60000, Encoding.UTF8, ContentTypes.FORM);
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
        protected string MD5(string input)
        {
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);
            md5.Clear();
            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2").PadLeft(2, '0'));
            }
            return sb.ToString();
        }
    }
}
