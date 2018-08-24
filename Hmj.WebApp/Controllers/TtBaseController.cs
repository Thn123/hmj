using Hmj.Common;
using Hmj.Entity;
using Hmj.Extension;
using Hmj.Interface;
using System;
using System.IO;
using System.Net;

namespace Hmj.WebApp.Controllers
{
    public class TtBaseController : BaseController
    {
        private ISystemService _sbo;
        public TtBaseController(ISystemService sbo)
        {
            _sbo = sbo;
        }
        ORG_INFO m;
        public void Base()
        {
            if (Request["code"] != null)
            {
                try
                {
                    string url = string.Format(@"https://api.weixin.qq.com/sns/oauth2/access_token?appid="
+ GetAppid(AppConfig.FWHOriginalID)
                        + "&secret=" + GetSecret(AppConfig.FWHOriginalID) +
                        "&code={0}&grant_type=authorization_code",
                        Request["code"].ToString());
                    string token = PostRequest(url);
                    if (token.Contains("7200"))
                    {
                        string[] b = token.Split('\"');
                        Session["FromUserName"] = b[13];
                        Session["ToUserName"] = AppConfig.FWHOriginalID;
                    }
                }
                catch (Exception ex)
                {

                }
            }
            else
            {
                OAuth(AppConfig.WXCorpID, Request.Url.AbsoluteUri);
            }
        }


        /// <summary>
        /// 授权 snsapi_userinfo方式  需要用户点击授权  可获取用户详细信息
        /// </summary>
        /// <param name="appid">公众号APPID</param>
        /// <param name="redirect_uri">回调地址</param>
        public void OAuth(string appid, string redirect_uri)
        {
            if (appid != "")
            {
                string url = "";
                url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_userinfo&state={2}#wechat_redirect";
                url = string.Format(url, appid, redirect_uri, Request.Params["id"] != null ? Request.Params["id"] : "0");

                Response.Redirect(url, false);
            }
        }

        public string GetSecret(string ToUserName)
        {
            m = _sbo.GetMerchantsByToUserName(ToUserName);
            return m == null ? "" : m.Appsecret;
        }

        public string PostRequest(string url)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);  //定义请求对象，并设置好请求URL地址
            request.Method = "post";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();    //定义响应对象，request在调用GetResponse方法事执行请求了，而不是在HttpWebRequest.Create的时候执行。       
            Stream stream = response.GetResponseStream(); //定义一个流对象，来获取响应流
            StreamReader sr = new StreamReader(stream);  //定义一个流读取对象，读取响应流
            string responseHTML = sr.ReadToEnd();
            return responseHTML;
        }


        public string GetAppid(string ToUserName)
        {
            m = _sbo.GetMerchantsByToUserName(ToUserName);
            return m == null ? "" : m.AppID;
        }


    }
}
