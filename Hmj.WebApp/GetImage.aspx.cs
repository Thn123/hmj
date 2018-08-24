using Hmj.Common;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace Hmj.WebApp
{
    public partial class GetImage : WeiPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            string url = "https://api.weixin.qq.com/cgi-bin/material/batchget_material?access_token=" + Token(AppConfig.FWHOriginalID);
            string j = @"{
""type"":""image"",
""offset"":""0"",
""count"":""20"",
}";
            var resMessage = HttpJsonPostRequest(url, j, Encoding.UTF8);
            Response.Write(resMessage);
        }
        public static string HttpJsonPostRequest(string postUrl, string postJson, Encoding encoding)
        {
            if (string.IsNullOrEmpty(postUrl))
            {
                throw new ArgumentNullException("HttpJsonPost ArgumentNullException :  postUrl IsNullOrEmpty");
            }

            if (string.IsNullOrEmpty(postJson))
            {
                throw new ArgumentNullException("HttpJsonPost ArgumentNullException : postJson IsNullOrEmpty");
            }
            var request = (HttpWebRequest)WebRequest.Create(postUrl);
            byte[] byteArray = encoding.GetBytes(postJson);
            request.ContentLength = byteArray.Length;
            request.Method = "post";
            request.ContentType = "application/json;charset=utf-8";

            using (var requestStream = request.GetRequestStream())
            {
                requestStream.Write(byteArray, 0, byteArray.Length);
            }

            using (var responseStream = request.GetResponse().GetResponseStream())
            {
                return new StreamReader(responseStream, encoding).ReadToEnd();
            }
        }
    }
}