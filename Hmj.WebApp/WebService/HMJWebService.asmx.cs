using Hmj.Business.ServiceImpl;
using Hmj.Common;
using Hmj.Common.Utils;
using Hmj.Entity;
using Hmj.Entity.Entities;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.Services;
using WeChatCRM.Common.Utils;

namespace Hmj.WebApp.WebService
{
    /// <summary>
    /// HMJWebService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://locahost.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class HMJWebService : System.Web.Services.WebService
    {

        #region
        [WebMethod]
        public AccessResponse SendTmps(BCJ_TMP_DETAIL_EX request)
        {
            ILog _loginfo = LogManager.GetLogger("loginfo");
            string ip = String.Empty;
            //服务端获取IP地址
            ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (null == ip || ip == String.Empty)
            {
                ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            if (null == ip || ip == String.Empty)
            {
                ip = HttpContext.Current.Request.UserHostAddress;
            }
            //ILog _logerror = LogManager.GetLogger("logerror");
            //_loginfo.Info("IP:"+ip);
            //string ip = this.Context.Request.UserHostAddress;
            //HttpRequest.UserHostAddress.ToString()
            //BCJ_TMP_DETAIL tmp_detail = JsonConvert.DeserializeObject<BCJ_TMP_DETAIL>(request);
            AccessResponse response = new AccessResponse();
            string json = "";
            string key = FormsAuthentication.HashPasswordForStoringInConfigFile(ip, "SHA1");
            if (System.Configuration.ConfigurationManager.AppSettings.Get("AuthSecret") == null)
            {
                _loginfo.Info("发送模板消息接口 IP:" + ip + " 验证不通过：服务没有配置白名单");
                response.Wv_Return = "N";
                response.Message = "服务没有配置白名单";
                response.Member_Group = "0";
                return response;
            }
            if (!System.Configuration.ConfigurationManager.AppSettings.Get("AuthSecret").Split(',').Contains<string>(key))
            {
                _loginfo.Info("发送模板消息接口 IP:" + ip + " 验证不通过：当前ip" + ip + "不在没权限访问");
                response.Wv_Return = "N";
                response.Message = "当前ip" + ip + "不在没权限访问";
                response.Member_Group = "0";
                return response;
            }
            int successCnt = 0;
            try
            {
                if (string.IsNullOrWhiteSpace(request.IsRealTime))
                {
                    _loginfo.Info("发送模板消息接口 IP:" + ip + " 验证不通过：请提供是否实时参数信息");
                    response.Wv_Return = "N";
                    response.Message = "请提供是否实时参数信息";
                    response.Member_Group = "0";
                    return response;
                }

                if (string.IsNullOrWhiteSpace(request.Invoke_Time))
                {
                    _loginfo.Info("发送模板消息接口 IP:" + ip + " 验证不通过：调用时间参数异常");
                    response.Wv_Return = "N";
                    response.Message = "调用时间参数异常";
                    response.Member_Group = "0";
                    return response;
                }

                try
                {
                    DateTime Invoke_Time = DateTime.Parse(request.Invoke_Time);
                }
                catch
                {
                    _loginfo.Info("发送模板消息接口 IP:" + ip + " 验证不通过：调用时间格式不正确");
                    response.Wv_Return = "N";
                    response.Message = "调用时间格式不正确";
                    response.Member_Group = "0";
                    return response;
                }

                if (request.Templates.Count <= 0)
                {
                    _loginfo.Info("发送模板消息接口 IP:" + ip + " 验证不通过：请至少提供一个消息模版信息");
                    response.Wv_Return = "N";
                    response.Message = "请至少提供一个消息模版信息";
                    response.Member_Group = "0";
                    return response;
                }

                if (request.Template_Params.Count <= 0)
                {
                    _loginfo.Info("发送模板消息接口 IP:" + ip + " 验证不通过：请至少提供一个会员目标组信息");
                    response.Wv_Return = "N";
                    response.Message = "请至少提供一个会员目标组信息";
                    response.Member_Group = "0";
                    return response;
                }

                string resmsg = NetHelper.HttpRequest(AppConfig.BeautyChinaWebApp, "", "GET", 2000,
                    Encoding.UTF8, "application/json");

                TokeRes toke = JsonHelper.DeserializeObject<TokeRes>(resmsg);
                HmjMemberService _hmjMember = new HmjMemberService();
                string errorMessage = "";
                string requestStr = JsonHelper.SerializeObject(request);
                //添加日志： ip+请求参数
                _loginfo.Info("发送模板消息接口 IP:" + ip + " 请求参数：" + requestStr);
                int str = _hmjMember.SendTmpEx(request, toke.Access_Token, ref successCnt, ref errorMessage);

                if (str == -1)
                {
                    _loginfo.Info("发送模板消息接口 IP:" + ip + " 返回结果失败：" + "[成功数]" + successCnt + " [错误信息]" + errorMessage);
                    response.Wv_Return = "N";
                    response.Message = errorMessage;
                    response.Member_Group = successCnt.ToString();
                    return response;
                }

                _loginfo.Info("发送模板消息接口 IP:" + ip + " 返回结果成功：[成功数]" + successCnt);
                response.Wv_Return = "Y";
                response.Message = "OK";
                response.Member_Group = successCnt.ToString();
                return response;
            }
            catch (Exception ex)
            {
                _loginfo.Info("发送模板消息接口 IP:" + ip + " 返回结果异常：[成功数]" + successCnt);
                WriteLog("发送模板消息", ex);
                //return ResponseJsonError(false, error_message, ex);
                response.Wv_Return = "N";
                response.Message = ex.Message;
                response.Member_Group = successCnt.ToString();
                return response;
            }
        }
        #endregion

        private void WriteLog(string funcName, Exception ex)
        {
            ILog _logerror = LogManager.GetLogger("logerror");
            _logerror.Error(funcName + "报错\r\n时间" + DateTime.Now.ToString() + "\r\n" + ex.Message + "\r\n" + ex.ToString());
        }
    }
}
