using Hmj.Common;
using System;

namespace Hmj.WebApp
{
    public partial class Token : WeiPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int Status = 0;
            string Message = "";
            string Access_token = "";
            try
            {
                Access_token = Token(AppConfig.FWHOriginalID);
                Status = 1;
            }
            catch (Exception ex)
            {
                Status = -1;
                Message = ex.ToString();
            }

            Response.Write("{\"Status\":\"" + Status + "\",\"Message\":\"" + Message + "\",\"Access_token\":\"" + Access_token + "\"}");
            Response.End();
        }
    }
}