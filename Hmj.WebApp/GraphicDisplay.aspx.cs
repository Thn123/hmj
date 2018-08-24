using Hmj.Business.ServiceImpl;
using Hmj.Entity;
using Hmj.Interface;
using System;

namespace Hmj.WebApp
{
    public partial class GraphicDisplay : System.Web.UI.Page
    {
        ISystemService sbo = new SystemService();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["id"] != null)
                {
                    WXGraphicDetail d = sbo.GetGraphicDetail(int.Parse(Request.QueryString["id"].ToString()));
                    if (d != null)
                    {
                        Title.Text = d.Title;
                        divbody.InnerHtml = d.Body;
                    }
                }
            }
        }
    }
}