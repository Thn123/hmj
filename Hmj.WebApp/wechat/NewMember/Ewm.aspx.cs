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
    public partial class Ewm : System.Web.UI.Page
    {
        MySmallShopService mss = new MySmallShopService();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["Id"] != null)
                {
                    EMPLOYEE emp = mss.GetGuanjiaById(int.Parse(Request.QueryString["Id"].ToString()));
                    if (emp != null)
                    {
                        guanjia.InnerHtml = string.Format(@"<h2>钟表管家：{0}</h2>
   <p>手机号码：{1}</p>", emp.NAME, emp.MOBILE);
                        pp.InnerHtml = emp.BRAND;
                        jj.InnerHtml = emp.INTRO;
                        ewmurl.InnerHtml = string.Format(@"<img src='{0}'><p>扫描二维码，绑定专属钟表管家</p>", emp.EwmUrl);
                        if (emp.PORTRAIT_URL != null)
                        {
                            guanjiaurl.InnerHtml = string.Format(@"<img src='{0}'>", emp.PORTRAIT_URL);
                        }
                    }
                }
            }
        }
    }
}