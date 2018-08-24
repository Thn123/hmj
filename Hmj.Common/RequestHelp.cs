using Hmj.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WeChatCRM.Common.Utils;

namespace Hmj.Common
{
    /// <summary>
    /// 请求帮助
    /// </summary>
    public class RequestHelp
    {
        static string localUrl = AppConfig.BeautyChinaUrl;

        /// <summary>
        /// post请求得到响应
        /// </summary>
        /// <typeparam name="T">响应的数据，如果无就传空字符串</typeparam>
        /// <typeparam name="V">请求的数据</typeparam>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static async Task<JsonDTO<T>> RequestPost<T, V>(string url, V data)
        {
            try
            {
                HttpClient client = new HttpClient();
                string str = JsonConvert.SerializeObject(data);

                //转换成键值对
                //Dictionary<string, string> dic = CommonHelp.GetDic<V>(data);
                StringContent content = new StringContent(str);
                content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                //FormUrlEncodedContent content = new FormUrlEncodedContent(dic);

                HttpResponseMessage messge = client.PostAsync(localUrl + url, content).Result;

                string result = await messge.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<JsonDTO<T>>(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// get请求得到响应数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url">/test/index</param>
        /// <param name="data">&name=12&age=1</param>
        /// <returns></returns>
        public static async Task<JsonDTO<T>> RequestGet<T>(string url, Dictionary<string, string> data = null)
        {
            HttpClient client = new HttpClient();
            StringBuilder sb = new StringBuilder();
            string result = "";
            if (data == null || data.Count == 0)
            {
                result = await client.GetStringAsync(localUrl + url);
            }
            else
            {
                foreach (string keys in data.Keys)
                {
                    string values = data[keys];
                    sb.Append(keys + "=" + values + "&");
                }
                //result = await client.GetStringAsync(localUrl + url + "?" + sb.ToString().TrimEnd('&'));

                result = NetHelper.HttpRequest(localUrl + url + "?" + sb.ToString().TrimEnd('&'),
                    "", "GET", 2000, Encoding.UTF8, "application/json");
            }
            return JsonConvert.DeserializeObject<JsonDTO<T>>(result);
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public static async Task<JsonDTO<T>> RequestFile<T>(string url, Stream file, string name, string filename,
            Dictionary<string, string> data = null)
        {
            HttpClient client = new HttpClient();
            MultipartFormDataContent content = new MultipartFormDataContent();

            if (data != null)
            {
                FormUrlEncodedContent posdata = new FormUrlEncodedContent(data);
                content.Add(posdata);
            }

            StreamContent stream = new StreamContent(file);
            content.Add(stream, name, filename);

            HttpResponseMessage messge = await client.PostAsync(localUrl + url, content);
            string result = await messge.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<JsonDTO<T>>(result);
        }
    }
}
