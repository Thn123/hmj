using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Hmj.Entity;
using Hmj.Business.ServiceImpl;

namespace Hmj.WebApp.wechat.Test
{
    public partial class Detail : WeiPage
    {
        SystemService ss = new SystemService();
        public TT_Detail t = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["id"] != null)
            {
                t = ss.GetTT_Detail(int.Parse(Request.QueryString["id"]));
            }
        }
    }
}