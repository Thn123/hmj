using Hmj.Common;
using Hmj.Common.Utils;
using Hmj.DTO;
using Hmj.Entity.WxModel;
using Hmj.Interface;
using log4net;
using Newtonsoft.Json;
using StructureMap.Attributes;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Hmj.WebApp.Controllers
{
    public class HmjMemberController : Controller
    {

        private static ILog logger = LogManager.GetLogger("logfatal");
        [SetterProperty]
        public IHmjMemberService _hmjMember { get; set; }

        /// <summary>
        /// 显示页面
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Index()
        {
            JsonDTO<MemberResDTO> result = await RequestHelp.RequestPost<MemberResDTO, MemberUpdateReqDTO>("Test/PostTest",
                new MemberUpdateReqDTO() { NAME_LAST = "你好" });
            //return Json(result);
            return View();
        }

        /// <summary>
        /// 显示注册页面
        /// </summary>
        /// <returns></returns>
        [Outh(true)]
        public ActionResult ToZhuce()
        {
            try
            {
                FansInfo Fans = Session["FansInfo"] as FansInfo;
                //Dictionary<string, string> dic = new Dictionary<string, string>();
                logger.Fatal("Fans.Openid:" + Fans.Openid + "<br/>Fans.Nickname:" + Fans.Nickname);
                //dic.Add("openid", Fans.Openid);

                //将验证是否绑定的方法改为同步，因为如果发起请求会因为接口超时的问题而进入异常，则页面始终报错
                bool isok = _hmjMember.ChckBind(Fans.Openid);

                //JsonDTO<bool> result = RequestHelp.RequestGet<bool>("Member/ChckBind", dic).Result;
                string json = JsonConvert.SerializeObject(Fans);

                //JsonDTO<LogResponse> log = RequestHelp.RequestPost<LogResponse, LogRequest>("User/AddLog",
                //  new LogRequest() { Title = "ToZhuCe", MsgType = "Message", MsgContent = json.Replace('\"', '\'') }).Result;

                //if (result.Data)
                if (isok)
                {
                    return Redirect(ConfigurationManager.AppSettings["WebUrl"] + "/assets/hmjweixin/html/user.html?obj=" +
                      Server.UrlEncode(json));
                }

                ViewBag.FansInfo = json;

                return View();
            }
            catch (System.Exception)
            {
                ViewBag.ErrorInfo = "系统繁忙，请刷新后重试";
                return View();
            }
        }

        /// <summary>
        /// 获取微信信息
        /// </summary>
        /// <returns></returns>
        public ActionResult GetWxToke()
        {
            FansInfo infos = Session["FansInfo"] as FansInfo;

            if (infos == null)
            {
                //return Json("");
                infos = new FansInfo();
            }

            return Json(infos);
        }

        /// <summary>
        /// 是否注册
        /// </summary>
        /// <returns></returns>
        public ActionResult IsRegister()
        {
            string str = Request["openid"];

            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("openid", str);
            JsonDTO<bool> result = RequestHelp.RequestGet<bool>("Member/ChckBind", dic).Result;

            return Json(result.Data);
        }


        /// <summary>
        /// 领取卡券
        /// </summary>
        /// <returns></returns>
        [Outh(true)]
        public ActionResult LQCoupon()
        {
            try
            {
                FansInfo Fans = Session["FansInfo"] as FansInfo;
                Dictionary<string, string> dic = new Dictionary<string, string>();
                logger.Fatal(Fans.Openid);
                dic.Add("openid", Fans.Openid);

                ViewBag.OpenId = Fans.Openid;

                return View();
            }
            catch (System.Exception)
            {
                return View();
            }

        }


        /// <summary>
        /// 专家问答
        /// </summary>
        /// <returns></returns>
        [Outh(true)]
        public ActionResult Zhuanjia()
        {
            try
            {
                FansInfo Fans = Session["FansInfo"] as FansInfo;
                Dictionary<string, string> dic = new Dictionary<string, string>();
                logger.Fatal(Fans.Openid);
                dic.Add("openid", Fans.Openid);

                JsonDTO<bool> result = RequestHelp.RequestGet<bool>("Member/ChckBind", dic).Result;
                string json = JsonConvert.SerializeObject(Fans);

                ViewBag.FansInfo = false;

                //如果是注册用户
                if (result.Data)
                {
                    ViewBag.FansInfo = true;
                }
                return View();


                //return Redirect(ConfigurationManager.AppSettings["WebUrl"] + "/hmjmember/tozhuce.do");
            }
            catch (System.Exception)
            {
                return View();
            }

        }

        [Outh(true)]
        public ActionResult WMall()
        {
            try
            {
                FansInfo Fans = Session["FansInfo"] as FansInfo;
                //Fans.Openid = "oDRuD1A65qVf-QjFFpdnQccRo7HA";
                Entity.Entities.HmjMemberDetail result = _hmjMember.GetMemberDetailByOpenId(Fans.Openid);

                //如果是未注册用户
                if (result == null)
                {
                    return Redirect(ConfigurationManager.AppSettings["WebUrl"] + "/hmjmember/tozhuce.do");
                }
                #region 获取用户信息并进行封装后跳转
                //拼接参数
                var code = ConfigurationManager.AppSettings["WMallCode"];
                var queryStr = $"userId={result.MEMBERNO}&code={code}&phone={result.MOBILE}&username={result.NAME}";
                // 私钥加密 并进行urlencode
                var sign = HttpUtility.UrlEncode(RSAUtils.RSAEncryptByPrivateKey("", queryStr));
                #endregion

                return Redirect(ConfigurationManager.AppSettings["WMallUrl"] + "?r=" + sign);
            }
            catch (System.Exception ex)
            {
                ViewBag.MyMessage = ex.ToString();
                return View();
            }

        }
    }
}
