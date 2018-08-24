using Hmj.Business.ServiceImpl;
using Hmj.Entity;
using Hmj.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hmj.WebApp.wechat.Member
{
    public partial class Callback : System.Web.UI.Page
    {
        ILogService log = new LogService();
        protected void Page_Load(object sender, EventArgs e)
        {

            string state = Request.QueryString["state"];
            string ticket = Request.QueryString["ticket"];
            string jump = Request.QueryString["jump"];
            string openid = Request.QueryString["openid"];
            try
            {
                Hmj.Entity.Callback cb = new Entity.Callback();
                cb.FromUserName = openid;
                if (!string.IsNullOrWhiteSpace(state))
                {
                    cb.State = int.Parse(state);
                }
                else {
                    cb.State = 0;
                }
                cb.Jump = jump;
                cb.Ticket = ticket;
                cb.CreateTime = DateTime.Now;
                MySmallShopService mss = new MySmallShopService();
                mss.SaveCallback(cb);
            }
            catch (Exception ex)
            {
                log.Error("盛时登录回传:"+ex.Message);
            }
         
            Response.Redirect("www.censh.com");
        }
    }
}