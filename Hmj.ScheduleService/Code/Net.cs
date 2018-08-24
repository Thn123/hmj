using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;

namespace Hmj.ScheduleService.Code
{
    public static class HttpVerbs
    {

        public const string POST = "POST";
        public const string GET = "GET";
        public const string PUT = "PUT";
        public const string DELETE = "DELETE";
        public const string HEAD = "HEAD";

        //public const string POST = "POST";
    }

    public static class ContentTypes
    {
        public const string FORM = "application/x-www-form-urlencoded; charset=utf-8";
        public const string JSON = "application/json; charset=utf-8";
        public const string Multipart = "multipart/form-data";
    }
    /**/
    /// <summary>
    /// Net : 提供静态方法，对常用的网络操作进行封装
    /// </summary>
    public sealed class Net
    {

        /// <summary>
        /// 返回URL内容,带POST数据提交
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="data">数据</param>
        /// <param name="method">GET/POST(默认)</param>
        /// <param name="timeout">超时时间（以毫秒为单位）</param>
        /// <param name="encoding">编码格式</param>
        /// <returns></returns>
        public static string HttpRequest(string url, string data, string method, int timeout, Encoding encoding, string contentType)
        {
            string res = string.Empty;

            //请求
            WebRequest webRequest = null;
            Stream postStream = null;

            //响应
            WebResponse webResponse = null;
            StreamReader streamReader = null;

            method = string.IsNullOrEmpty(method) ? HttpVerbs.POST : method;

            if (method == HttpVerbs.GET)
            {
                if (!string.IsNullOrEmpty(data))
                {
                    if (url.IndexOf("?") > 0)
                    {
                        url = url + '&' + data;
                    }
                    else
                    {
                        url = url + '?' + data;
                    }
                }
            }
            try
            {
                //请求
                webRequest = WebRequest.Create(url);
                webRequest.Method = method;
                webRequest.Timeout = timeout;
                webRequest.ContentType = contentType;

                if (method == HttpVerbs.POST)
                {
                    byte[] postData = encoding.GetBytes(data);
                    webRequest.ContentLength = postData.Length;
                    postStream = webRequest.GetRequestStream();
                    postStream.Write(postData, 0, postData.Length);
                }
                //响应
                webResponse = webRequest.GetResponse();
                streamReader = new StreamReader(webResponse.GetResponseStream(), encoding);
                res = streamReader.ReadToEnd();
            }
            catch (WebException ex)
            {
                using (HttpWebResponse response = (HttpWebResponse)ex.Response)
                {
                    if (response != null)
                    {
                        using (Stream responseStream = response.GetResponseStream())
                        {
                            res = new StreamReader(responseStream).ReadToEnd();
                            res = ":(" + res;
                        }
                    }
                    else
                    {
                        res = ":(我猜是连接超时";
                    }
                }
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            finally
            {
                if (postStream != null)
                {
                    postStream.Close();
                }
                if (streamReader != null)
                {
                    streamReader.Close();
                }
                if (webResponse != null)
                {
                    webResponse.Close();
                }
            }

            return res;
        }


        private static string FormDataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";

        private static string FileHeaderTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";

        public static string HttpUploadFile(string url, string fileKey, string contentType, NameValueCollection nvc, List<string> fileList)
        {
            string result = string.Empty;
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.ContentType = "multipart/form-data; boundary=" + boundary;
            webRequest.Method = "POST";
            webRequest.KeepAlive = true;
            webRequest.Credentials = System.Net.CredentialCache.DefaultCredentials;

            Stream postStream = webRequest.GetRequestStream();

            //string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
            if (nvc != null)
            {
                foreach (string key in nvc.Keys)
                {
                    //分隔符
                    postStream.Write(boundarybytes, 0, boundarybytes.Length);

                    string formitem = string.Format(FormDataTemplate, key, nvc[key]);
                    byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                    postStream.Write(formitembytes, 0, formitembytes.Length);
                }

            }


            foreach (var file in fileList)
            {
                //分隔符
                postStream.Write(boundarybytes, 0, boundarybytes.Length);

                try
                {
                    //读取文件流
                    FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
                    //文件头部
                    string fileName = Path.GetFileName(file);
                    string header = string.Format(FileHeaderTemplate, fileKey, fileName, contentType);
                    byte[] headerbytes = Encoding.UTF8.GetBytes(header);
                    postStream.Write(headerbytes, 0, headerbytes.Length);

                    byte[] buffer = new byte[4096];
                    int bytesRead = 0;
                    while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        postStream.Write(buffer, 0, bytesRead);
                    }
                    fileStream.Close();

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            //字节尾部
            byte[] trailer = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");
            postStream.Write(trailer, 0, trailer.Length);

            WebResponse webResponse = null;
            StreamReader streamReader = null;
            try
            {
                webResponse = webRequest.GetResponse();
                streamReader = new StreamReader(webResponse.GetResponseStream());

                result = streamReader.ReadToEnd();
            }
            catch (WebException ex)
            {
                using (HttpWebResponse response = (HttpWebResponse)ex.Response)
                {
                    if (response != null)
                    {
                        using (Stream responseStream = response.GetResponseStream())
                        {
                            result = new StreamReader(responseStream).ReadToEnd();
                            result = ":(" + result;
                        }
                    }
                    else
                    {
                        result = ":(我猜是连接超时";
                    }
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            finally
            {
                webRequest = null;
                if (postStream != null)
                {
                    postStream.Close();
                }
                if (webResponse != null)
                {
                    webResponse.Close();
                }
                if (streamReader != null)
                {
                    streamReader.Close();
                }
            }

            return result;
        }

    }
}