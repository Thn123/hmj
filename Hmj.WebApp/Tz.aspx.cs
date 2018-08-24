using System;

namespace Hmj.WebApp
{

    public partial class Tz : System.Web.UI.Page
    {
        public String url = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["url"] != "")
                {
                    url = Request.QueryString["url"];
                }
            }
        }
    }
}