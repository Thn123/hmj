using System;

namespace Hmj.WebApp
{
    public partial class WxLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string redirectUrlSM = Server.UrlEncode("http://wechat.censh.com/Token.aspx");
            string url = "https://open.weixin.qq.com/connect/qrconnect?appid=wx32364024f2c86185&redirect_uri=" + redirectUrlSM + "&response_type=code&scope=snsapi_login&state=STATE#wechat_redirect";
            Response.Redirect(url);
        }
    }
}