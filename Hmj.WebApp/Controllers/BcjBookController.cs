using Hmj.Common;
using Hmj.Entity;
using Hmj.Entity.Entities;
using Hmj.Entity.Jsons;
using Hmj.Entity.WxModel;
using Hmj.Interface;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Hmj.WebApp.Controllers
{
    public class BcjBookController : Controller
    {
        private ICustMemberService _custMember;
        static ILog logger = LogManager.GetLogger(typeof(BcjBookController));
        private IBcjBookService _book;
        private IBcjStoreService _store;

        public BcjBookController(ICustMemberService custMember, IBcjBookService book, IBcjStoreService store)
        {
            _custMember = custMember;
            _book = book;
            _store = store;
        }

        /// <summary>
        /// 显示佰草集预约界面
        /// </summary>
        /// <returns></returns>
        [Outh(true)]
        public ActionResult Index()
        {
            FansInfo Fans = Session["FansInfo"] as FansInfo;

            string vono = Request["ZCP_NUM"];

            ViewBag.No = vono;

            //该方法先获取接口里面会员信息，然后更新本地会员信息（这样后续的会员更新等就不需要连续调用会员主数据接口了）
            MemberInfo meberinfo = _custMember.GetMemberInfo(Fans.Openid);

            return View(meberinfo);
        }

        /// <summary>
        /// 获取城市信息
        /// </summary>
        /// <returns></returns>
        public ActionResult GetCity()
        {
            JsonSMsg msg = new JsonSMsg();

            string lo = Request["LO"];

            string ln = Request["LN"];

            string city_code = Request["city_code"];

            List<BCJ_CITY> citys = new List<BCJ_CITY>();
            BCJ_CITY one = null;
            if (string.IsNullOrEmpty(city_code))
            {
                citys = _book.GetCity();

                if (citys.Count() <= 0)
                {
                    msg.Status = 0;
                    return Json(msg);
                }

                TenAPI model = Utility.GetLocal(lo, ln);

                string cityname = model.result.formatted_address;
                JavaScriptSerializer js = new JavaScriptSerializer();

                logger.Info(js.Serialize(model) + lo + "====" + ln);
                one = citys.Where(a => cityname.Contains(a.CITY_NAME)).ToList().FirstOrDefault();

                city_code = one == null ? "110100" : one.CITY_CODE;
            }

            //加载门店列表
            List<BCJ_STORES_EX> stores = _store.GetStoresByCityCode(city_code);

            msg.Data = new
            {
                CITYS = citys,
                STORES = stores
            };

            msg.Status = 1;
            msg.Message = one == null ? "110100" : one.CITY_CODE;
            return Json(msg);
        }

        /// <summary>
        /// 预约开始
        /// </summary>
        /// <returns></returns>
        public ActionResult ToBook()
        {
            JsonSMsg msg = new JsonSMsg();

            //优惠券编号
            string number = Request["number"];

            //会员id
            string member_id = Request["member_id"];

            //门店id
            string store_id = Request["store_id"];

            //预约日期
            string book_date = Request["book_date"];

            //预约时间
            string book_time = Request["book_time"];

            //预约人
            string book_name = Request["book_name"];

            //预约电话
            string book_phone = Request["book_phone"];

            DateTime mydate = DateTime.Now;

            DateTime nowTime = DateTime.Parse(mydate.ToString("yyyy-MM-dd"));

            DateTime bookdate = DateTime.Parse(book_date);


            int hour = mydate.Hour;

            if (nowTime >= bookdate)
            {
                msg.Data = "";
                msg.Status = 0;
                msg.Message = "对不起，当天或者已过的时间无法预约！！";
                return Json(msg);
            }

            if (nowTime.AddDays(1) == bookdate && hour >= 16)
            {
                msg.Data = "";
                msg.Status = 0;
                msg.Message = "对不起，该时间点不能预约第二天，请于当天16点前预约，谢谢合作！！";
                return Json(msg);
            }

            string flg = _book.ToBook(number, member_id,
                store_id, book_date, book_time, book_name, book_phone);

            if (flg == "-1")
            {
                msg.Data = "";
                msg.Status = 0;
                msg.Message = "对不起，该门店该时间已经被别人预约，请选择其他时间，或者换家门店。谢谢合作！！！";
                return Json(msg);
            }

            if (flg == "-2")
            {
                msg.Data = "";
                msg.Status = 0;
                msg.Message = "对不起，预约信息插入失败！！！";
                return Json(msg);
            }

            if (flg == "-3")
            {
                msg.Data = "";
                msg.Status = 0;
                msg.Message = "对不起，该券已经使用！！！";
                return Json(msg);
            }

            msg.Data = "";
            msg.Status = 1;
            msg.Message = "成功";
            return Json(msg);
        }

        /// <summary>
        /// 显示预约页面
        /// </summary>
        /// <returns></returns>
        [Outh(true)]
        public ActionResult ShowBookHis()
        {
            FansInfo Fans = Session["FansInfo"] as FansInfo;

            MemberInfo meberinfo = _custMember.GetMemberInfo(Fans.Openid);

            List<HMJ_BOOK_EX> books = _book.GetBooks(meberinfo.Member_Id);

            foreach (HMJ_BOOK_EX item in books)
            {
                item.BOOK_DATE_EX = item.BOOK_DATE.Value.ToString("yyyy-MM-dd");
            }

            ViewBag.NewBook = books;
            return View();
        }

        /// <summary>
        /// 取消预约
        /// </summary>
        /// <returns></returns>
        public ActionResult CancelBook()
        {
            JsonSMsg msg = new JsonSMsg();

            string bookid = Request["BookID"];

            string flg = _book.ChageBookStatus(bookid, "2", "-1");
            if (flg != "1")
            {
                msg.Status = 0;
                msg.Message = "";
                return Json(msg);
            }

            msg.Status = 1;
            msg.Message = "";
            return Json(msg);
        }

        /// <summary>
        /// 修该预约
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateBook()
        {
            JsonSMsg msg = new JsonSMsg();

            string bookid = Request["book_id"];
            string BookDate = Request["BookDate"];
            string BookTime = Request["BookTime"];
            string Store_Id = Request["Store_Id"];

            DateTime mydate = DateTime.Now;

            DateTime nowTime = DateTime.Parse(mydate.ToString("yyyy-MM-dd"));

            DateTime bookdate = DateTime.Parse(BookDate);


            int hour = mydate.Hour;

            if (nowTime > bookdate)
            {
                msg.Data = "";
                msg.Status = 0;
                msg.Message = "对不起，已过的时间无法预约！！";
                return Json(msg);
            }

            if (nowTime == bookdate)
            {
                if (hour > 8)
                {
                    msg.Data = "";
                    msg.Status = 0;
                    msg.Message = "对不起，上午已到无法修该，谢谢合作！！";
                    return Json(msg);
                }

                if (hour > 12)
                {
                    msg.Data = "";
                    msg.Status = 0;
                    msg.Message = "对不起，下午已到无法修该，谢谢合作！！";
                    return Json(msg);
                }

                if (hour > 17)
                {
                    msg.Data = "";
                    msg.Status = 0;
                    msg.Message = "对不起，晚上已到无法预约，谢谢合作！！";
                    return Json(msg);
                }
            }

            string flg = _book.UpdateBook(bookid, BookDate, BookTime, Store_Id);
            if (flg == "-1")
            {
                msg.Status = 0;
                msg.Message = "对不起，该时间点已经被预约！！";
                return Json(msg);
            }

            msg.Status = 1;
            msg.Message = "";
            return Json(msg);
        }
    }
}
