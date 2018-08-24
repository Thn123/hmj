using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace Hmj.WebApi
{
    public class Global : System.Web.HttpApplication
    {
        private static ILog _logerror = LogManager.GetLogger("logerror");

        protected void Application_Start(object sender, EventArgs e)
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError().GetBaseException();
            StringBuilder strErr = new StringBuilder();
            strErr.Append("\r\n" + DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss"));
            strErr.Append("\r\n.客户信息：");

            string ip = "";
            if (Request.ServerVariables.Get("HTTP_X_FORWARDED_FOR") != null)
            {
                ip = Request.ServerVariables.Get("HTTP_X_FORWARDED_FOR").ToString().Trim();
            }
            else
            {
                ip = Request.ServerVariables.Get("Remote_Addr").ToString().Trim();
            }
            strErr.Append("\r\n\tIp:" + ip);
            strErr.Append("\r\n\t浏览器:" + Request.Browser.Browser.ToString());
            strErr.Append("\r\n\t浏览器版本:" + Request.Browser.MajorVersion.ToString());
            strErr.Append("\r\n\t操作系统:" + Request.Browser.Platform.ToString());
            strErr.Append("\r\n.错误信息：");
            strErr.Append("\r\n\t页面：" + Request.Url.ToString());
            
            strErr.Append("\r\n\t错误信息：" + ex.Message);
            strErr.Append("\r\n\t错误源：" + ex.Source);
            strErr.Append("\r\n\t异常方法：" + ex.TargetSite);
            strErr.Append("\r\n\t堆栈信息：" + ex.StackTrace);
            strErr.Append("\r\n--------------------------------------------------------------------------------------------------");
            //创建路径 
            string upLoadPath = Server.MapPath("~/log/");
            if (!System.IO.Directory.Exists(upLoadPath))
            {
                System.IO.Directory.CreateDirectory(upLoadPath);
            }
            //创建文件 写入错误 
            _logerror.Error(strErr.ToString());
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}