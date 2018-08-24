using Hmj.Business.ServiceImpl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hmj.WebApp.wechat.Member
{
    public partial class ApiDemo : WeiPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Session["FromUserName"] = "oQaIMwPgnsBpwQYfwLQnUBbmQKS01";
            //Session["ToUserName"] = "oQaIMwPgnsBpwQYfwLQnUBbmQKS99";
            if (!IsPostBack)
            {
                Base();
                if ((Request.QueryString["FromUserName"] != null || Session["FromUserName"] != null) && (Request.QueryString["ToUserName"] != null || Session["ToUserName"] != null))
                {
                    string user = Request.QueryString["FromUserName"] == null ? Session["FromUserName"].ToString() : Request.QueryString["FromUserName"].ToString();
                    string user2 = Request.QueryString["ToUserName"] == null ? Session["ToUserName"].ToString() : Request.QueryString["ToUserName"].ToString();
                    Session["FromUserName"] = user;
                    Session["ToUserName"] = user2;
                    unique.Value = user;
                    ticket.Value = "";
                    MySmallShopService mss = new MySmallShopService();

                    Hmj.Entity.Callback cb = mss.GetCallbackByFromUserName(user);
                    if (cb != null)
                    {
                        ticket.Value = cb.Ticket;
                    }
                }
            }
        }
    }
}