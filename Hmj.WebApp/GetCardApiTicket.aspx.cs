using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hmj.WebApp
{
    public partial class GetCardApiTicket : WeiPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int Status = 0;
            string Message = "";
            string JsApi = "";
            try
            {
                JsApi = GetCardApi();
                Status = 1;
            }
            catch (Exception ex)
            {
                Status = -1;
                Message = ex.ToString();
            }

            Response.Write("{\"Status\":\"" + Status + "\",\"Message\":\"" + Message + "\",\"JsApi\":\"" + JsApi + "\"}");
            Response.End();
        }
    }
}