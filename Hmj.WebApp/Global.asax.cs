using FluentScheduler;
using Hmj.Business;
using Hmj.Business.Job;
using Hmj.Common;
using Hmj.Extension;
using Hmj.Interface;
using log4net;
using RedisSessionProvider.Config;
using StackExchange.Redis;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Vulcan.Framework.DBConnectionManager;

namespace Hmj.WebApp
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        private static ILog _logerror = LogManager.GetLogger("logerror");
        //private static ConfigurationOptions redisConfigOpts;

        protected void Application_Start()
        {
            var config = GlobalConfiguration.Configuration;
            var jqueryFormatter = config.Formatters.FirstOrDefault(x => x.GetType() == typeof(System.Web.Http.ModelBinding.JQueryMvcFormUrlEncodedFormatter));

            config.Formatters.Remove(config.Formatters.JsonFormatter);
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            config.Formatters.Remove(config.Formatters.FormUrlEncodedFormatter);
            config.Formatters.Remove(jqueryFormatter);

            

            config.Formatters.Insert(0, new ServiceStackTextFormatter(ServiceStack.Text.JsonDateHandler.TimestampOffset));
            //AreaRegistration.RegisterAllAreas(); 
            //WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //默认的数据库管理
            DbConnectionFactory.Default = new SqlConnectionFactory();

            //依赖注入
            Bootstrapper.Restart();

            ControllerBuilder.Current.SetControllerFactory(new StructureMapControllerFactory());
            log4net.Config.XmlConfigurator.Configure();

            //添加全局的过滤器（在这里增加全局的筛选器可以添加多个）所有的action都要执行
            GlobalFilters.Filters.Add(new OuthFilter());

            
            if (AppConfig.IsOpenTimeTask == 1)
            {
                //注册华美家后但又未进行绑定转换，第二天早上10天推送消息提醒绑定
                //JobManager.Initialize(new MySendTmpList());

                //发送crm提交的定时微信模板消息
                JobManager.Initialize(new SendTmpMessageJob());

                //激活会员(在执行正常程序时，有可能会员的激活操作失败。所以需要定时任务，每半小时轮询处理失败记录)
                JobManager.Initialize(new MemberActivateJob());
            }
        }


        protected void Application_Error(object sender, EventArgs e)
        {
            // 在出现未处理的错误时运行的代码 
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
            //System.IO.File.AppendAllText(upLoadPath + DateTime.Now.ToString("yyyy.MM.dd") + ".log", strErr.ToString(), System.Text.Encoding.UTF8);
            //处理完及时清理异常 
            Server.ClearError();
            //跳转至出错页面 
            //Response.Redirect("http://www.holidays5.com");
        }
    }
}