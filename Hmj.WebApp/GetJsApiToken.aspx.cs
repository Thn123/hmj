using System;

namespace Hmj.WebApp
{
    public partial class GetJsApiToken : WeiPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int Status = 0;
            string Message = "";
            string JsApi = "";
            try
            {
                JsApi = GetJSAPI_Ticket();
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