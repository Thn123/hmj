using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace CyRong.Common
{
    public class HttpRequestHelper
    {
        /// <summary>
        /// post request
        /// </summary>
        /// <param name="postUrl"></param>
        /// <param name="postXml"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string HttpXmlPostRequest(string postUrl, string postXml, Encoding encoding)
        {
            if (string.IsNullOrEmpty(postUrl))
            {
                throw new ArgumentNullException("HttpXmlPost ArgumentNullException :  postUrl IsNullOrEmpty");
            }

            if (string.IsNullOrEmpty(postXml))
            {
                throw new ArgumentNullException("HttpXmlPost ArgumentNullException : postXml IsNullOrEmpty");
            }

            var request = (HttpWebRequest)WebRequest.Create(postUrl);
            byte[] byteArray = encoding.GetBytes(postXml);
            request.ContentLength = byteArray.Length;
            request.Method = "post";
            request.ContentType = "text/xml";

            using (var requestStream = request.GetRequestStream())
            {
                requestStream.Write(byteArray, 0, byteArray.Length);
            }

            using (var responseStream = request.GetResponse().GetResponseStream())
            {
                return new StreamReader(responseStream, encoding).ReadToEnd();
            }
        }

        /// <summary>
        /// 这个方法是两个URL第一个url是微信的，第二个是本地图片路径
        /// </summary>
        /// <param name="url"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string HttpUploadFile(string url, string path)
        {
            // 设置参数
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            CookieContainer cookieContainer = new CookieContainer();
            request.CookieContainer = cookieContainer;
            request.AllowAutoRedirect = true;
            request.Method = "POST";
            string boundary = DateTime.Now.Ticks.ToString("X"); // 随机分隔线
            request.ContentType = "multipart/form-data;charset=utf-8;boundary=" + boundary;
            byte[] itemBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "\r\n");
            byte[] endBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");

            int pos = path.LastIndexOf("\\");
            string fileName = path.Substring(pos + 1);

            //请求头部信息
            StringBuilder sbHeader = new StringBuilder(string.Format("Content-Disposition:form-data;name=\"file\";filename=\"{0}\"\r\nContent-Type:application/octet-stream\r\n\r\n", fileName));
            byte[] postHeaderBytes = Encoding.UTF8.GetBytes(sbHeader.ToString());

            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            byte[] bArr = new byte[fs.Length];
            fs.Read(bArr, 0, bArr.Length);
            fs.Close();

            Stream postStream = request.GetRequestStream();
            postStream.Write(itemBoundaryBytes, 0, itemBoundaryBytes.Length);
            postStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);
            postStream.Write(bArr, 0, bArr.Length);
            postStream.Write(endBoundaryBytes, 0, endBoundaryBytes.Length);
            postStream.Close();

            //发送请求并获取相应回应数据
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            //直到request.GetResponse()程序才开始向目标网页发送Post请求
            Stream instream = response.GetResponseStream();
            StreamReader sr = new StreamReader(instream, Encoding.UTF8);
            //返回结果网页（html）代码
            string content = sr.ReadToEnd();
            return content;
        }


        /// <summary>
        /// get request
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string HttpGetRequest(string url)
        {
            string responseHTML = "";
            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);  //定义请求对象，并设置好请求URL地址
                HttpWebResponse response;

                response = (HttpWebResponse)request.GetResponse();

                // HttpWebResponse response = (HttpWebResponse)request.GetResponse();    //定义响应对象，request在调用GetResponse方法事执行请求了，而不是在HttpWebRequest.Create的时候执行。       
                Stream stream = response.GetResponseStream(); //定义一个流对象，来获取响应流
                StreamReader sr = new StreamReader(stream);  //定义一个流读取对象，读取响应流
                responseHTML = sr.ReadToEnd();
            }
            catch (Exception)
            {
                //response = (HttpWebResponse)ex.Response;
            }


            return responseHTML;
        }

        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static Dictionary<String, Object> GetParameter(HttpContext context)
        {
            StreamReader reader = new StreamReader(context.Request.InputStream);
            String strJson = HttpUtility.UrlDecode(reader.ReadToEnd());
            JavaScriptSerializer jss = new JavaScriptSerializer();
            //将json字符串反序列化成一个Dictionary对象
            Dictionary<String, Object> dicParameter = jss.Deserialize<Dictionary<String, Object>>(strJson);
            return dicParameter;
        }

    }
}
