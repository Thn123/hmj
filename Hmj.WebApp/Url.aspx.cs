using Hmj.Common;
using System;

namespace Hmj.WebApp
{
    public partial class Url : WeiPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["url"] != null && Request.QueryString["scope"] != null)
            {
                Response.Write(Request.QueryString["url"]);
                string url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + GetAppid(AppConfig.FWHOriginalID)
                    + "&redirect_uri=" + AppConfig.WebUrl + "/Url.aspx?url2=" + Request.QueryString["url"] + "&response_type=code&scope=" + Request.QueryString["scope"] + "&state=STATE#wechat_redirect";
                Response.Redirect(url);
            }
            if (Request.QueryString["url2"] != null)
            {
                if (Request.QueryString["code"] != null)
                {
                    try
                    {
                        string url = string.Format(@"https://api.weixin.qq.com/sns/oauth2/access_token?appid=" +
                            GetAppid(AppConfig.FWHOriginalID) + "&secret=" + GetSecret(AppConfig.FWHOriginalID) + "&code={0}&grant_type=authorization_code", Request.QueryString["code"].ToString());
                        string token = PostRequest(url);
                        if (token.Contains("7200"))
                        {
                            string[] b = token.Split('\"');
                            if (b.Length >= 15)
                            {
                                string u = "https://api.weixin.qq.com/sns/userinfo?access_token=" + b[3] +
                                    "&openid=" + b[13] + "&lang=zh_CN";
                                string x = PostRequest(u);
                                string[] y = x.Split('\"');
                                if (y.Length >= 30)
                                {
                                    AddWxLog("Name:" + y[7] + ",openid:" + b[13] + ",Get API," + x);
                                }

                                if (Request.QueryString["url2"].Contains("?"))
                                {
                                    Response.Redirect(Request.QueryString["url2"] + "&access_token=" + b[3] + "&openid=" + b[13]);
                                    if (Request.QueryString["url2"].Contains("&"))
                                    {

                                    }
                                }
                                else
                                {
                                    Response.Redirect(Request.QueryString["url2"] + "?access_token=" + b[3] + "&openid=" + b[13]);
                                }
                            }

                        }
                    }
                    catch (Exception)
                    {

                    }
                }

            }
        }
    }
}