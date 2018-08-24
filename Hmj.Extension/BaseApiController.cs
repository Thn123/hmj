using Hmj.Entity;
using Hmj.Entity.Jsons;
using Hmj.Interface;
using StructureMap.Attributes;
using System.IO;
using System.Net;
using System.Web.Mvc;

namespace Hmj.Extension
{
    [ApiAuthorize]
    public class BaseApiController : BaseController
    {
        [SetterProperty]
        public ISystemService SystemService { get; set; }
        [SetterProperty]
        public ILogService LogService { get; set; }
        [SetterProperty]
        public  IApiAuthService ApiAuthService { get; set; }
       
        ORG_INFO m;
        public string GetAppid(string ToUserName)
        {

            ORG_INFO m = SystemService.GetMerchantsByToUserName(ToUserName);
            return m == null ? "" : m.AppID;
        }
        public string GetSecret(string ToUserName)
        {
            m = SystemService.GetMerchantsByToUserName(ToUserName);
            return m == null ? "" : m.Appsecret;
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

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //AuthInfo authInfo = GetRequestAuthInfo(filterContext);
            //string errorMsg = string.Empty;
            //bool isAuth = ApiAuthService.Auth(authInfo.AppID, authInfo.Timestamp, authInfo.Sign, out errorMsg);
            //if (isAuth == true)
            //{
            //    filterContext.Result = new ServiceStackJsonResult() { Data = CreateJSONObject(false, errorMsg) };
            //}
            base.OnActionExecuting(filterContext);
        }
       



        /// <summary>
        /// 从请求中提取验证信息对象
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        private AuthInfo GetRequestAuthInfo(ActionExecutingContext ctx)
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
        private JsonSMsg CreateJSONObject(bool succeed, string msg, object data = null)
        {
            JsonSMsg rMsg = new JsonSMsg();
            rMsg.Status = succeed ? 1 : 0;
            rMsg.Message = msg;
            rMsg.Data = data;

            return rMsg;
        }
    }
}
