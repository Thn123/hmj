using Hmj.Business.ServiceImpl;
using Hmj.Entity;
using Hmj.Interface;
using System;

namespace Hmj.WebApp
{
    public partial class GetEwm : System.Web.UI.Page
    {

        ISystemService service = new SystemService();
        protected void Page_Load(object sender, EventArgs e)
        {

            int Status = 0;
            string Message = "";
            string EmpName = "";
            string EwmUrl = "";
            try
            {
                if (Request.QueryString["Mobile"] != null)
                {
                    EMPLOYEE emp = service.GetEmpByPhone(Request.QueryString["Mobile"]);
                    if (emp != null)
                    {
                        EmpName = emp.NAME;
                        EwmUrl = emp.EwmUrl;
                    }
                    else 
                    {
                        Status = -1;
                        Message = "请输入正确的员工手机号";
                    }
                }
                else
                {
                    Status = -1;
                    Message = "手机号不能为空";
                }
            }
            catch (Exception ex)
            {
                Status = -1;
                Message = ex.ToString();
            }

            Response.Write("{\"Status\":\"" + Status + "\",\"Message\":\"" + Message + "\",\"EmpName\":\"" + EmpName + "\",\"EwmUrl\":\"" + EwmUrl + "\"}");
            Response.End();


        }
    }
}