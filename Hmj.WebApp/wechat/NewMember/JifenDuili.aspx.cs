using Hmj.Entity;
using Hmj.WebApp.Common.Pages;
using Hmj.WebApp.SelectMemberNew;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hmj.WebApp.wechat.NewMember
{
    public partial class JifenDuili : WeiPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Base();
                if ((Request.QueryString["FromUserName"] != null || Session["FromUserName"] != null) && (Request.QueryString["ToUserName"] != null || Session["ToUserName"] != null))
                {
                    string user = Request.QueryString["FromUserName"] == null ? Session["FromUserName"].ToString() : Request.QueryString["FromUserName"].ToString();
                    string user2 = Request.QueryString["ToUserName"] == null ? Session["ToUserName"].ToString() : Request.QueryString["ToUserName"].ToString();
                    Session["FromUserName"] = user;
                    Session["ToUserName"] = user2;
                    Bind(user);
                }
                else
                {
                    string host = Request.Url.Host;
                    string path = Request.Path;
                    string redirect_uri = Request.Url.AbsoluteUri;

                    Response.Redirect(AuthCode(redirect_uri));
                }
            }
        }


        public void Bind(string FromUserName)
        {
            CUST_INFO cust = mss.GetCust(FromUserName);
            if (cust != null && !string.IsNullOrEmpty(cust.CardNo))
            {
                Z_LOY_BP_GETDETAILResponse zloy = SelectMember(cust.MOBILE);
                if (zloy.T_CENTRAL != null && zloy.T_CENTRAL.Length > 0)
                {
                    string DesRet = CenshEncryption.Encrypt3DES(cust.MOBILE);
                    string mobile = Server.UrlEncode(DesRet);
                    string redirectUrlWX = Server.UrlEncode("http://www.censh.com/jifen/");
                    string WX = "http://www.censh.com/crm/weiXin/index?mobile=" + mobile + "&redirectUrl=" + redirectUrlWX;

                    Response.Redirect(WX);
                }
            }
            else
            {
                Response.Redirect("https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx32364024f2c86185&redirect_uri=http://wechat.censh.com/wechat/NewMember/Reg.aspx&response_type=code&scope=snsapi_base&state=STATE&connect_redirect=1#wechat_redirect");
            }
        }
    }
}