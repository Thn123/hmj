using Hmj.Business.ServiceImpl;
using Hmj.Entity;
using System;

namespace Hmj.WebApp.wechat.Area
{
    public partial class Html : System.Web.UI.Page
    {
        MySmallShopService mss = new MySmallShopService();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["Id"] != null)
                {
                    AreaManage a = new AreaManage();
                    a = mss.GetArea(int.Parse(Request.QueryString["Id"].ToString()));
                    con.InnerHtml = a.Body;
                }
            }
        }
    }
}