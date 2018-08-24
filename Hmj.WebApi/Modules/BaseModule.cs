using Hmj.Business.ServiceImpl;
using Hmj.Common;
using Hmj.Common.Utils;
using Hmj.Entity;
using Hmj.Interface;
using Hmj.WebApi.Models;
using log4net;
using Nancy;
using Newtonsoft.Json;
using StructureMap.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;

namespace Hmj.WebApi.Modules
{
    public abstract class BaseModule : NancyModule
    {
        [SetterProperty]
        public ILogService LogService { get; set; }

        private static ILog _logwarn = LogManager.GetLogger("logwarn");

        private static ILog _logerror = LogManager.GetLogger("logerror");

        [SetterProperty]
        public IApiAuthService ApiAuthService { get; set; }

        public BaseModule() :
            base("")
        {
            Get["/"] = p =>
            {
                return "Hello, welcome to BeautyFarm API platform.";
            };
        }

        /// <summary>
        /// 异步执行
        /// </summary>
        /// <param name="action"></param>
        public void RunAsync(Action action)
        {
            try
            {
                Task.Factory.StartNew(action);
            }
            catch
            {
            }
        }

        public BaseModule(string modulePath) :
            base(modulePath)
        {
            Before += context =>
            {
                var authInfo = GetRequestAuthInfo(context);
                string authString = GetAuthLogString(authInfo);
                try
                {
                    string IP = HttpContext.Current.Request.UserHostAddress;
                    this.Trace("请求开始（" + IP + "）：-----------------------------------------------------------------------------------------------------------------------------");
                    this.Trace(string.Format("Request参数：{0}", authString));

                    bool isValidIP = this.IsValidIP(context);
                    //if (isValidIP == false)
                    //{
                    //    string errorMsg = string.Empty;
                    //    bool isAuth = ApiAuthService.Auth(authInfo.AppID, authInfo.Timestamp, authInfo.Sign, out errorMsg);
                    //    if (isAuth == false)
                    //    {
                    //        LogService.Info(string.Format("验证请求失败：{0}，URL：{1}", errorMsg, authString));
                    //        return ResponseJson(false, errorMsg);
                    //    }
                    //}
                }
                catch (Exception ex)
                {
                    LogService.Error(string.Format("请求时出错Before：\r\nMessage=[{0}]\r\nStackTrace=[{1}]\r\n", ex.Message, ex.StackTrace));
                    return ResponseJson(false, "请求时异常！");
                }
                return null;
            };

            After += context =>
            {
                //LogService.Trace("请求结束：---------------------------------------------------------------------------------------");
            };

            OnError += (ctx, ex) =>
            {
                var baseEx = ex.GetBaseException();
                string error = string.Format("未知异常OnError：\r\nMessage=[{0}]\r\nStackTrace=[{1}]\r\nPath=[{2}]", baseEx.Message, baseEx.StackTrace, ctx.Request.Path);
                LogService.Error(error);


                //发送提醒邮件
                //MailManager mail = new MailManager();
                //mail.Subject = "API报错提醒";
                //mail.To = "1731796086@qq.com";
                //mail.From = string.Format("{0}<likui.liu@puman.com>", "刘力魁");
                //mail.Body = string.Format("<p>HI,</p><p>&nbsp;&nbsp;&nbsp;{0}</p><p>来自：{1}</p>", error, ctx.Request.Url.HostName);
                //mail.Send();

                return ResponseJson(false, "未知异常！");
            };
        }

        /// <summary>
        /// 创建接口返回值的封装对象
        /// </summary>
        /// <param name="status"></param>
        /// <param name="msg"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private AccessResponse<T> CreateWebApiMsg<T>(bool status, string msg, T data)
        {
            return new AccessResponse<T>()
            {
                Status = status ? 1 : 0,
                Message = msg,
                Data = data
            };
        }
        private AccessResponse CreateWebApiMsg(bool isSucess, string msg)
        {
            return new AccessResponse()
            {
                Status = isSucess ? 1 : 0,
                Message = msg
            };
        }

        private bool IsValidIP(NancyContext ctx)
        {
            string ip = ctx.Request.UserHostAddress;
            var ipList = AppValidIPManager.Create().GetAllIP();
            var info = ipList.FirstOrDefault(m => m.IPAddress == ip);
            return info != null;
        }

        /// <summary>
        /// 从请求中提取验证信息对象
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        private AuthInfo GetRequestAuthInfo(NancyContext ctx)
        {
            dynamic param = ctx.Request.Query;
            return new AuthInfo()
            {
                AppID = this.GetValue<string>("appid"),
                Timestamp = this.GetValue<string>("timestamp"),
                Sign = this.GetValue<string>("sign"),
                IPAddress = ctx.Request.UserHostAddress,
                Url = string.Format("{0}", ctx.Request.Url.ToString())
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

        /// <summary>
        /// 获取日志格式化的查询字符串
        /// </summary>
        /// <returns></returns>
        private string GetQueryString()
        {
            return Request.Url.Query;
        }

        protected Response ResponseJson(bool isSucess, string msg)
        {
            return this.ResponseJson(isSucess, msg, null);
        }

        protected Response ResponseJsonError(bool isSucess, string msg, Exception ex)
        {
            StringBuilder strErr = new StringBuilder();
            strErr.Append("\r\n" + DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss"));
            strErr.Append("\r\n.客户信息：");

            string ip = "";
            if (HttpContext.Current.Request.ServerVariables.Get("HTTP_X_FORWARDED_FOR") != null)
            {
                ip = HttpContext.Current.Request.ServerVariables.Get("HTTP_X_FORWARDED_FOR").ToString().Trim();
            }
            else
            {
                ip = HttpContext.Current.Request.ServerVariables.Get("Remote_Addr").ToString().Trim();
            }
            strErr.Append("\r\n\tIp:" + ip);
            strErr.Append("\r\n\t浏览器:" + HttpContext.Current.Request.Browser.Browser.ToString());
            strErr.Append("\r\n\t浏览器版本:" + HttpContext.Current.Request.Browser.MajorVersion.ToString());
            strErr.Append("\r\n\t操作系统:" + HttpContext.Current.Request.Browser.Platform.ToString());
            strErr.Append("\r\n.错误信息：");
            strErr.Append("\r\n\t页面：" + Request.Url.ToString());
            strErr.Append("\r\n\t错误信息：" + ex.Message);
            strErr.Append("\r\n\t错误源：" + ex.Source);
            strErr.Append("\r\n\t异常方法：" + ex.TargetSite);
            strErr.Append("\r\n\t堆栈信息：" + ex.StackTrace);
            strErr.Append("\r\n--------------------------------------------------------------------------------------------------");
            //创建路径 
            string upLoadPath = HttpContext.Current.Server.MapPath("~/log/");
            if (!System.IO.Directory.Exists(upLoadPath))
            {
                System.IO.Directory.CreateDirectory(upLoadPath);
            }

            _logerror.Error(strErr.ToString());

            var result = CreateWebApiMsg(isSucess, msg);
            var json = JsonHelper.SerializeObject(result);

            var response = Response.AsText(json);
            response.ContentType = result.ContentType;
            return response;
        }

        protected Response ResponseJson(bool isSucess, string msg, object data, bool loged = true)
        {
            var result = CreateWebApiMsg(isSucess, msg, data);
            var json = JsonHelper.SerializeObject(result);

            if (loged)
            {
                this.Trace("返回结果：" + json);
            }

            var response = Response.AsText(json);
            response.ContentType = result.ContentType;
            return response;
        }

        protected IDictionary<string, object> Queries
        {
            get
            {
                DynamicDictionary query = Request.Query;
                return query;
            }
        }

        protected IDictionary<string, object> Forms
        {
            get
            {
                DynamicDictionary form = Request.Form;
                return form;
            }
        }

        protected IDictionary<string, object> Params
        {
            get
            {
                var queries = this.Queries;
                var forms = this.Forms;

                if (forms != null && forms.Count > 0)
                {
                    foreach (KeyValuePair<string, object> item in forms)
                    {
                        if (!queries.ContainsKey(item.Key))
                        {
                            queries.Add(item);
                        }
                    }
                }
                return queries;
            }
        }

        protected T BindObject<T>() where T : class
        {
            using (var body = Request.Body)
            {
                T t = default(T);
                if (body != null)
                {
                    string json = StreamHelper.Read(body);
                    //在这里可以解密
                    this.Trace("Body参数：" + json);

                    t = JsonHelper.DeserializeObject<T>(json);
                }
                return t;
            }
        }

        protected T GetValue<T>(string key)
        {
            T t = default(T);
            if (this.Params.ContainsKey(key))
            {
                var value = this.Params[key];
                if (value != null)
                {
                    object o = ConvertHelper.To<T>(value.ToString());
                    t = (T)o;
                }
            }
            return t;
        }


        protected void Trace(string log)
        {
            string msg = string.Format(" {0}", log);
            _logwarn.Warn(msg);
        }


        ORG_INFO m;
        public string Token(string ToUserName)
        {
            ISystemService sbo = new SystemService();
            m = sbo.GetMerchantsByToUserName(ToUserName);
            if (m == null)
                return "";
            string Access_token = "";
            if (m.Access_token != "")
            {
                Access_token = m.Access_token;
                string url = "https://api.weixin.qq.com/cgi-bin/user/info?access_token=" + Access_token + "&openid=" +
                    (m.OneOpenID == null ? "oQaIMwPgnsBpwQYfwLQnUBbmQKS4" : m.OneOpenID);
                string b = PostRequest(url);
                if (b.Contains("errcode"))  //返回错误信息
                {
                    Access_token = GetAccess(m);
                    m.Access_token = Access_token;
                    sbo.SaveMerchants(m);
                }
                if (m.OneOpenID == "" || m.OneOpenID == null)
                {
                    WXCUST_FANS fans = sbo.GetOneFans(m.ToUserName);
                    if (fans != null)
                    {
                        m.OneOpenID = fans.FROMUSERNAME;
                        sbo.SaveMerchants(m);
                    }
                }
            }
            else
            {
                if (m.OneOpenID == "" || m.OneOpenID == null)
                {
                    WXCUST_FANS fans = sbo.GetOneFans(m.ToUserName);
                    if (fans != null)
                    {
                        m.OneOpenID = fans.FROMUSERNAME;
                        // sbo.SaveMerchants(m);
                    }
                }
                Access_token = GetAccess(m);
                m.Access_token = Access_token;
                sbo.SaveMerchants(m);

            }
            return Access_token;
        }

        private string GetAccess(ORG_INFO m)
        {
            string Access_token = "";
            string url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + m.AppID + "&secret=" + m.Appsecret;
            try
            {
                string token = PostRequest(url);
                if (token.Contains("7200"))
                {
                    string[] b = token.Split('\"');
                    Access_token = b[3];
                }
            }
            catch (Exception)
            {
                Access_token = "";
            }
            return Access_token;
        }

        public string PostRequest(string url)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);  //定义请求对象，并设置好请求URL地址
            request.Method = "post";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();    //定义响应对象，request在调用GetResponse方法事执行请求了，而不是在HttpWebRequest.Create的时候执行。       
            Stream stream = response.GetResponseStream(); //定义一个流对象，来获取响应流
            StreamReader sr = new StreamReader(stream);  //定义一个流读取对象，读取响应流
            string responseHTML = sr.ReadToEnd();
            return responseHTML;
        }


        /// <summary>
        /// 获取最新JSAPI_TICKET凭证
        /// </summary>
        /// <param name="ToUserName"></param>
        /// <returns></returns>
        public string GetJSAPI_Ticket()
        {
            ISystemService sbo = new SystemService();
            string JSapi_ticket = "";
            ORG_INFO m = sbo.GetMerchantsByToUserName(AppConfig.FWHOriginalID);
            if (m.JSapi_Ticket != "" && m.JSapi_Ticket != null && (m.GetTicketTime == null ? DateTime.Now.AddHours(-3) : m.GetTicketTime.Value).AddHours(2) > DateTime.Now) //不为空，并且获取时间没有超过2小时
            {
                return m.JSapi_Ticket;
            }
            else
            {
                JSapi_ticket = m.JSapi_Ticket;
                string url = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token=" + Token(AppConfig.FWHOriginalID) + "&type=jsapi";
                string b = PostRequest(url);
                tickresult ticket = JsonConvert.DeserializeObject<tickresult>(b);
                if (ticket.errcode == 0)  //正确
                {
                    m.JSapi_Ticket = ticket.ticket;
                    m.GetTicketTime = DateTime.Now;
                    sbo.SaveMerchants(m);
                    return m.JSapi_Ticket;
                }
                return "";
            }
        }
        public string GetCardApi(string token)
        {
            string Cardapi_ticket = "";
            CardApiTicket at = new CardApiTicket();
            IHmjMemberService _hmjMember = new HmjMemberService();
            at = _hmjMember.GetModelCardApi();
            if (at == null)
                at = new CardApiTicket();
            if (at.Cardapi_Ticket != "" && at.Cardapi_Ticket != null && (at.GetTicketTime == null ? DateTime.Now.AddHours(-3) : at.GetTicketTime.Value).AddHours(2) > DateTime.Now) //不为空，并且获取时间没有超过2小时
            {
                return at.Cardapi_Ticket;
            }
            else
            {
                Cardapi_ticket = at.Cardapi_Ticket;
                string url = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token=" + token + "&type=wx_card";
                string b = PostRequest(url);
                CardApi ticket = JsonConvert.DeserializeObject<CardApi>(b);
                if (ticket.errcode == 0)  //正确
                {
                    at.Cardapi_Ticket = ticket.ticket;
                    at.GetTicketTime = DateTime.Now;
                    _hmjMember.AddCardApi(at);
                    return at.Cardapi_Ticket;
                }
                return "";
            }
        }
    }

    class tickresult
    {
        public int errcode { get; set; }
        public string errmsg { get; set; }
        public string ticket { get; set; }
        public int expires_in { get; set; }
    }

    class CardApi
    {
        public int errcode { get; set; }
        public string errmsg { get; set; }
        public string ticket { get; set; }
        public int expires_in { get; set; }
    }

    public class CouponAccessResponse
    {
        public int status { get; set; }
        public CouponResponse message { get; set; }
        public object data { get; set; }
    }

    public class CouponResponse
    {
        public string errcode { get; set; }
        public string errmsg { get; set; }
        public string card_id { get; set; }
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

    public class cardInfo {
        public string card_id { get; set; }
        public string code { get; set; }
        public string brand_name { get; set; }
        public string title { get; set; }
        public string color { get; set; }
    }
}