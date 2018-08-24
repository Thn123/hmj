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
    public partial class shoubiao : WeiPage
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
                    EMPLOYEE emp = mss.GetGuanJiaName(user);
                    if (emp == null)
                    {
                        name.InnerHtml = "钟表管家 (未绑定)";
                        ljcount.InnerHtml = "0";
                        bdcount.InnerHtml = "0";
                        fkcount.InnerHtml = "0";
                    }
                    else
                    {
                        name.InnerHtml = "钟表管家 " + emp.NAME;
                        HfName.Value = emp.NAME;
                        if (emp.PORTRAIT_URL != null)
                        {
                            gjtx.InnerHtml = "<img src='" + emp.PORTRAIT_URL + "' onclick='getewm(" + emp.ID + ")'>";
                        }
                        else 
                        {
                            gjtx.InnerHtml = "<img src='" + emp.AVATAR_URL + "' onclick='getewm(" + emp.ID + ")'>";
                        }
                        bdcount.InnerHtml = mss.GetGuanJiaCount(user).ToString();
                        ljcount.InnerHtml = "62";
                        fkcount.InnerHtml = "128";
                    }
                }
                else
                {
                    Response.Redirect(AuthCode(Request.Url.AbsoluteUri));
                }
            }
        }
    }
}