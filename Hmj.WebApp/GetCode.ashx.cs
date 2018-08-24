using Hmj.Common;
using Hmj.ExtendAPI.WeiXin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using System.Web;
using System.Web.Security;
using WeChatCRM.Common.Utils;

namespace Hmj.WebApp
{
    /// <summary>
    /// GetCode 的摘要说明
    /// </summary>
    public class GetCode : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            if (GetQeuryString("appid", context) != "")
            {
                string json = string.Empty;
                string responjson = string.Empty;
                string errorMsg = "";
                string appid_s = GetQeuryString("appid", context);
                string timestamp = GetQeuryString("timestamp", context);
                string sign = GetQeuryString("sign", context);

                if (Auth(appid_s, timestamp, sign, context.Request.UserHostAddress, out errorMsg))
                {
                    HttpRequest request = context.Request;
                    Stream stream = request.InputStream;

                    if (stream.Length > 0)
                    {
                        StreamReader reader = new StreamReader(stream);
                        json = reader.ReadToEnd();
                    }
                    responjson = WXMPClientServiceApi.Create().SendTemplateMsg(new WeiPage().Token("gh_7bcc17156676"), json); 
                    context.Response.Write(responjson);
                }
                else
                {

                    context.Response.Write(" { \"errcode\":1, \"errmsg\":\"" + errorMsg + "\"}");
                }

            }
            else
            {
                context.Response.Write(" { \"errcode\":1, \"errmsg\":\"appid不能为空!\" }");
            }
        }
    
        public bool Auth(string appID, string timestamp, string sign, string ip, out string errorMsg)
        {
            bool isValidIP = IsValidIP(ip);//验证是否是白名单里的IP
            if (isValidIP == false)
            {
                if (string.IsNullOrEmpty(appID))
                {
                    errorMsg = "缺少认证参数[appid]";
                    return false;
                }
                if (string.IsNullOrEmpty(timestamp))
                {
                    errorMsg = "缺少认证参数[timestamp]";
                    return false;
                }
                if (string.IsNullOrEmpty(sign))
                {
                    errorMsg = "缺少认证参数[sign]";
                    return false;
                }
                AppUserInfo appUserInfo = AppUserManager.Create().Get(appID);
                if (appUserInfo == null)
                {
                    errorMsg = "无效的[appid]";
                    return false;
                }
                string input = string.Format("appid={0}&secretkey={1}&timestamp={2}", appID, appUserInfo.AppSecret, timestamp);

                string checksign = CryptographyManager.MD5(input);
                if (string.Compare(sign, checksign, true) != 0)
                {
                    errorMsg = "认证方式错误";
                    return false;
                }
            }

            errorMsg = "";
            return true;
        }

        private bool IsValidIP(string ip)
        {
            var ipList = new string[3]{"199.12.36.69","",""};
            var info = ipList.FirstOrDefault(m => m == ip);
            return info != null;
        }

        public string GetQeuryString(string para, HttpContext context)
        {
            if (context.Request.QueryString[para] != null) return context.Request.QueryString[para].ToString();
            else return "";
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
       
    }
}