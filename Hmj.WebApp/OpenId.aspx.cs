using Hmj.Common;
using System;

namespace Hmj.WebApp
{
    public partial class OpenId : WeiPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int Status = 0;
            string Message = "";
            string OpenId = "";
            if (Request.QueryString["code"] != null)
            {
                try
                {
                    string url = string.Format(@"https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + GetAppid(AppConfig.FWHOriginalID) + "&secret=" + GetSecret(AppConfig.FWHOriginalID) + "&code={0}&grant_type=authorization_code", Request.QueryString["code"].ToString());
                    string token = PostRequest(url);
                    if (token.Contains("7200"))
                    {
                        string[] b = token.Split('\"');
                        OpenId = b[13];
                        Status = 1;
                    }
                }
                catch (Exception ex)
                {
                    Status = -1;
                    Message = ex.ToString();
                }
            }
            Response.Write("{\"Status\":\"" + Status + "\",\"Message\":\"" + Message + "\",\"OpenId\":\"" + OpenId + "\"}");
            Response.End();
        }
    }
}