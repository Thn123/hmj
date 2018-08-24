using Hmj.Common;
using Hmj.Entity.WxModel;
using log4net;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Hmj.WebApp
{
    /// <summary>
    /// 注意：如果加上FilterAttribute特性就不仅仅是
    /// </summary>
    public class OuthFilter : IAuthorizationFilter
    {
        //验证（注意：这样写的全局验证需要再global的Application_Start中添加）
        //GlobalFilters.Filters.Add(new YanZheng());

        //IActionFilter:每个action执行的之前和结束后使用（分为两个方法OnActionExecuted和OnActionExecuting）
        //IAuthorizationFilter:一般判断当前用户是否有action的执行权限，在每个action被执行前OnAuthorization
        //IExceptionFilter:当action中发生未处理异常的时候执行OnException
        //IResultFilter:在每个actionresult的前后执行
        private static ILog logger = LogManager.GetLogger("logfatal");



        public void OnAuthorization(AuthorizationContext filterContext)
        {
            //要执行action的名字
            string action_name = filterContext.ActionDescriptor.ActionName;

            var attributes = filterContext.ActionDescriptor.GetCustomAttributes(false);

            string path = filterContext.HttpContext.Request.Url.AbsoluteUri;
            //string goods = filterContext.HttpContext.Request["names"];
            //filterContext.HttpContext.Items.Add("names", "goods");

            bool power = false;

            bool logins = false;

            //获取在每个control上添加的特性。
            foreach (object item in attributes)
            {
                if (item is OuthAttribute)
                {
                    OuthAttribute mes = item as OuthAttribute;

                    power = mes.IS_OUTH;
                }

                if (item is IsLoginAttribute)
                {
                    IsLoginAttribute islogin = item as IsLoginAttribute;

                    logins = islogin.IS_LOGIN;
                }
            }

            if (logins)
            {
                if (filterContext.HttpContext.Session["StoreCode"] == null || filterContext.HttpContext.Session["WhoID"] == null)
                {
                    //直接阻止下面的响应
                    filterContext.Result = new RedirectResult("/BcjStore/Index.do");
                }
            }


            if (power && filterContext.HttpContext.Session["FansInfo"] == null)
            {
                //FansInfo model = new FansInfo()
                //{
                //    Headimgurl = "http://thirdwx.qlogo.cn/mmopen/2fibrz5JaYCaUibbNJM0dJPjmOmxDKe2TqOcqnFRySWUswlRm7ouk8suStkP9icnuRWap5R2mgUAatwXzHoFwGCiaNkYNCYdxaVw/132",
                //    Nickname = "陈艳容",
                //    //Openid = "oDRuD1DSi1yyDx9x4_Ttpf_0haB0",
                //    Openid = "oDRuD1A65qVf-QjFFpdnQccRo7HA"
                //};
                //filterContext.HttpContext.Session["FansInfo"] = model;


                //HttpCookie cook = new HttpCookie("mykey", JsonConvert.SerializeObject(model));
                //filterContext.HttpContext.Response.Cookies.Add(cook);

                #region yes
                if (filterContext.HttpContext.Request["code"] != null)
                {
                    try
                    {
                        logger.Fatal("进入二次授权");
                        string url = string.Format(@"https://api.weixin.qq.com/sns/oauth2/access_token?appid="
    + AppConfig.WXCorpID
                            + "&secret=" + AppConfig.WXCorpSecret +
                            "&code={0}&grant_type=authorization_code",
                            filterContext.HttpContext.Request["code"].ToString());
                        string token = PostRequest(url);
                        logger.Fatal("token" + token);
                        if (token.Contains("7200"))
                        {
                            string[] b = token.Split('\"');
                            filterContext.HttpContext.Session["FromUserName"] = b[13];
                            filterContext.HttpContext.Session["ToUserName"] = AppConfig.FWHOriginalID;

                            string urlinfo = "https://api.weixin.qq.com/sns/userinfo?access_token=" + b[3] +
                                "&openid=" + b[13] + "&lang=zh_CN";

                            string str = PostRequest(urlinfo);
                            logger.Fatal(str);
                            FansInfo infos = JsonConvert.DeserializeObject<FansInfo>(str);
                            byte[] bt = System.Text.Encoding.Default.GetBytes(infos.Nickname);
                            infos.Nickname = Convert.ToBase64String(bt);
                            filterContext.HttpContext.Session["FansInfo"] = infos;
                            HttpCookie cook = new HttpCookie("FansInfo", str);
                            filterContext.HttpContext.Response.Cookies.Add(cook);

                            logger.Fatal("进入二次授权结束" + str);
                        }

                    }
                    catch (Exception ex)
                    {
                        logger.Fatal("二次授权错误" + ex.ToString());
                    }
                }
                else
                {
                    //800是主站使用的（80被nginx使用了）
                    string urlok = filterContext.HttpContext.Request.Url.AbsoluteUri.Replace(":800", "");
                    logger.Fatal(urlok);
                    string url = OAuth(AppConfig.WXCorpID,
                        urlok, filterContext.HttpContext);

                    //直接阻止下面的响应
                    filterContext.Result = new RedirectResult(url);
                }
                #endregion


            }
        }

        /// <summary>
        /// 授权 snsapi_userinfo方式  需要用户点击授权  可获取用户详细信息
        /// </summary>
        /// <param name="appid">公众号APPID</param>
        /// <param name="redirect_uri">回调地址</param>
        public string OAuth(string appid, string redirect_uri, HttpContextBase context)
        {
            if (appid != "")
            {
                string url = "";
                url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_userinfo&state={2}#wechat_redirect";
                url = string.Format(url, appid, redirect_uri,
                   context.Request.Params["id"] != null ? context.Request.Params["id"] : "0");

                return url;
            }

            return "";
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

    }
}