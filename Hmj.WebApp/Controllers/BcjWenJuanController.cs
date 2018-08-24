using Hmj.Entity.Jsons;
using Hmj.Entity.WxModel;
using Hmj.Interface;
using System.Web.Mvc;

namespace Hmj.WebApp.Controllers
{
    public class BcjWenJuanController : Controller
    {
        private ICustMemberService _custMember;
        private IBcjBookService _bcjBook;

        public BcjWenJuanController(ICustMemberService custMember, IBcjBookService bcjBook)
        {
            _custMember = custMember;
            _bcjBook = bcjBook;
        }


        /// <summary>
        /// 显示问卷UI
        /// </summary>
        /// <returns></returns>
        [Outh(true)]
        public ActionResult Index()
        {
            FansInfo Fans = Session["FansInfo"] as FansInfo;

            if (!_custMember.ChckBind(Fans.Openid))
            {
                //如果该粉丝已经绑定了会员，则直接进入到会员详情界面
                return Redirect("/BcjMember/Index.do");
            }

            MemberInfo meberinfo = _custMember.GetMemberInfo(Fans.Openid, "");

            ViewBag.MemberID = meberinfo.Member_Id;

            ViewBag.GropuID = "1";

            return View();
        }

        /// <summary>
        /// 增加问卷
        /// </summary>
        /// <returns></returns>
        public ActionResult SaveHis()
        {
            JsonSMsg msg = new JsonSMsg();

            string LoadData = Request["LoadData"];
            string MemberID = Request["MemberID"];
            string GropuID = Request["GropuID"];
            
            string str = _bcjBook.SaveHis(LoadData, MemberID, GropuID);

            if (str == "-1")
            {
                msg.Status = 0;
                msg.Message = "亲，您已经提交！请不要重复提交。";
                return Json(msg);
            }
            msg.Status = 1;
            msg.Message = "成功";
            return Json(msg);
        }

        /// <summary>
        /// 显示FAQ
        /// </summary>
        /// <returns></returns>
        public ActionResult FAQ()
        {
            return View();
        }
    }
}
