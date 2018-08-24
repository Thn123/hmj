using Hmj.Business.ServiceImpl;
using Hmj.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hmj.WebApp.wechat.NewMember
{
    public partial class Band : WeiPage
    {
        //MySmallShopService mss = new MySmallShopService();
        public string FromUrl = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            //Session["FromUserName"] = "oQaIMwPgnsBpwQYfwLQnUBbmQKS4";
            //Session["ToUserName"] = "oQaIMwPgnsBpwQYfwLQnUBbmQKS4";
            if (!IsPostBack)
            {
                Base();
                if ((Request.QueryString["FromUserName"] != null || Session["FromUserName"] != null) && (Request.QueryString["ToUserName"] != null || Session["ToUserName"] != null))
                {
                    string user = Request.QueryString["FromUserName"] == null ? Session["FromUserName"].ToString() : Request.QueryString["FromUserName"].ToString();
                    string user2 = Request.QueryString["ToUserName"] == null ? Session["ToUserName"].ToString() : Request.QueryString["ToUserName"].ToString();
                    Session["FromUserName"] = user;
                    Session["ToUserName"] = user2;
                    CUST_INFO c = mss.GetCust(user);
                    if (c != null&&!string.IsNullOrEmpty(c.CardNo))
                    {
                        Response.Redirect("https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx32364024f2c86185&redirect_uri=http://wechat.censh.com/wechat/NewMember/ka.aspx&response_type=code&scope=snsapi_base&state=STATE&connect_redirect=1#wechat_redirect");
                    }
                }
                else
                {
                    Response.Redirect(AuthCode(Request.Url.AbsoluteUri));
                }

                //来源url
                string fromUrl = Request.QueryString["from_url"] ?? "";
                if (!string.IsNullOrEmpty(fromUrl))
                {
                    this.FromUrl = HttpUtility.UrlDecode(fromUrl);
                }
            }

        }

        
    }
}