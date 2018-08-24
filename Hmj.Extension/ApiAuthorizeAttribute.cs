using Hmj.Common;
using Hmj.Entity.Jsons;
using Hmj.Interface;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Hmj.Extension
{
    public class ApiAuthorizeAttribute : AuthorizeAttribute
    {
        private static ILogService _logService;
        private static ILogService LogService
        {
            get
            {
                if (_logService == null)
                {
                    _logService = ServiceFactory.GetInstance<ILogService>();
                }
                return _logService;
            }
        }
        private static IApiAuthService _apiAuthService;
        private static IApiAuthService ApiAuthService
        {
            get
            {
                if (_apiAuthService == null)
                {
                    _apiAuthService = ServiceFactory.GetInstance<IApiAuthService>();
                }
                return _apiAuthService;
            }
        }
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (NoAuthorize(filterContext))
            {
                return;
            }
            AuthInfo authInfo = GetRequestAuthInfo(filterContext);
            string authString = GetAuthLogString(authInfo);
            try
            {
                this.Fatal("请求开始：-----------------------------------------------------------------------------------------------------------------------------");
                this.Fatal(string.Format("Request参数：{0}", authString));

                bool isValidIP = this.IsValidIP(authInfo.IPAddress);
                if (isValidIP == false)
                {
                    string errorMsg = string.Empty;
                    bool isAuth = ApiAuthService.Auth(authInfo.AppID, authInfo.Timestamp, authInfo.Sign, out errorMsg);
                    if (isAuth == false)
                    {
                        this.Fatal(string.Format("验证请求失败：{0}，URL：{1}", errorMsg, authString));
                        filterContext.Result = new ServiceStackJsonResult() { Data = CreateJSONObject(false, errorMsg) };
                    }
                }
            }
            catch (Exception ex)
            {
                LogService.Error(string.Format("请求时出错Before：\r\nMessage=[{0}]\r\nStackTrace=[{1}]\r\n", ex.Message, ex.StackTrace));
                //return ResponseJson(false, "请求时异常！");
                filterContext.Result = new ServiceStackJsonResult() { Data = CreateJSONObject(false, "请求时异常！") };
            }

        }

        protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
        {
            return base.AuthorizeCore(httpContext);
        }

        #region 排除认证的control或action

        private bool NoAuthorize(AuthorizationContext filterContext)
        {
            ActionDescriptor actionDescriptor = filterContext.ActionDescriptor;
            if (actionDescriptor.IsDefined(typeof(NoAuthorizeAttribute), false))
            {
                return true;
            }
            if (actionDescriptor.ControllerDescriptor.IsDefined(typeof(NoAuthorizeAttribute), false))
            {
                return true;
            }

            return false;
        }

        #endregion
        private JsonSMsg CreateJSONObject(bool succeed, string msg, object data = null)
        {
            JsonSMsg rMsg = new JsonSMsg();
            rMsg.Status = succeed ? 1 : 0;
            rMsg.Message = msg;
            rMsg.Data = data;

            return rMsg;
        }
        protected void Fatal(string log)
        {
            string msg = string.Format(" {0}", log);
            LogService.Fatal(msg);
        }
        private bool IsValidIP(string ip)
        {
            //string ip = ctx.HttpContext.Request.UserHostAddress;
            var ipList = AppValidIPManager.Create().GetAllIP();
            var info = ipList.FirstOrDefault(m => m.IPAddress == ip);
            return info != null;
        }

        /// <summary>
        /// 从请求中提取验证信息对象
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        private AuthInfo GetRequestAuthInfo(AuthorizationContext ctx)
        {
            var request = ctx.HttpContext.Request;
            return new AuthInfo()
            {
                AppID = request.QueryString["appid"],
                Timestamp = request.QueryString["timestamp"],
                Sign = request.QueryString["sign"],
                IPAddress = request.UserHostAddress,
                Url = string.Format("{0}", request.Url.ToString())
            };
        }
        /// <summary>
        /// 获取日志格式化的验证信息字符串
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private string GetAuthLogString(AuthInfo info)
        {
            return string.Format("URL={0}，IP={1}", info.Url, info.IPAddress);
        }
    }


    public class AuthInfo
    {
        /// <summary>
        /// 商户ID
        /// </summary>
        public string AppID { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public string Timestamp { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        public string Sign { get; set; }

        /// <summary>
        /// 格式（JSON, XML）
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// IP
        /// </summary>
        public string IPAddress { get; set; }

        /// <summary>
        /// IP
        /// </summary>
        public string Url { get; set; }
    }
}
