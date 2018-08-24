using Hmj.Common;
using Hmj.Entity;
using Hmj.Entity.Entities;
using Hmj.Entity.Jsons;
using Hmj.Interface;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Hmj.WebApp.Controllers
{
    public class BcjStoreController : Controller
    {
        private IBcjStoreService _bcjStore;
        static ILog logger = LogManager.GetLogger(typeof(BcjStoreController));
        private IBcjBookService _book;

        public BcjStoreController(IBcjStoreService bcjStore, IBcjBookService book)
        {
            _bcjStore = bcjStore;
            _book = book;
        }

        /// <summary>
        /// 显示门店登录页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 验证门店是否存在
        /// </summary>
        /// <returns></returns>
        public ActionResult ChckStore()
        {
            JsonSMsg msg = new JsonSMsg();

            string StoreCode = Request["StoreCode"];

            int flg = _bcjStore.ChckStore(StoreCode);

            if (flg == 1)
            {
                msg.Message = "成功";
                msg.Status = 1;

                return Json(msg);
            }

            msg.Message = "失败";
            msg.Status = 0;
            return Json(msg);
        }

        /// <summary>
        /// 门店登录
        /// </summary>
        /// <returns></returns>
        public ActionResult StoreLogin()
        {
            JsonSMsg msg = new JsonSMsg();

            string mobile = Request["mobile"];
            string storecode = Request["storecode"];
            string pwd = Request["pwd"];

            if (string.IsNullOrEmpty(pwd))
            {
                msg.Message = "请输入密码";
                msg.Status = 0;

                return Json(msg);
            }
            
            long flg = _bcjStore.StoreLogin(mobile, storecode, pwd);

            if (flg > 0)
            {
                Session["StoreCode"] = storecode.ToUpper();
                Session["WhoID"] = flg;
                msg.Message = "成功";
                msg.Status = 1;

                return Json(msg);
            }

            if (flg == -2)
            {
                msg.Message = "请检查账号以及密码！！！";
                msg.Status = 0;

                return Json(msg);
            }

            msg.Message = "失败";
            msg.Status = 0;
            return Json(msg);
        }

        /// <summary>
        /// 显示预约列表
        /// </summary>
        /// <returns></returns>
        [IsLogin(true)]
        public ActionResult ShowBook()
        {
            string storeCode = Session["StoreCode"].ToString();

            BCJ_STORES_EX store = _bcjStore.GetStoreEntity(storeCode);

            return View(store);
        }

        /// <summary>
        /// 加载日历
        /// </summary>
        /// <returns></returns>
        public ActionResult GetDate()
        {
            JsonSMsg msg = new JsonSMsg();

            string store_id = Request["store_id"];
            DateTime nowData = DateTime.Now;

            string dateageo = string.Empty;
            string dateafutr = string.Empty;
            string datanow = "'" + nowData.ToString("yyyy-MM-dd") + "'";

            //未来六天
            for (int i = 0; i < 7; i++)
            {
                dateafutr += "'" + nowData.AddDays(i + 1).ToString("yyyy-MM-dd") + "',";
            }

            //过去六天
            for (int i = 6; i > 0; i--)
            {
                dateageo += "'" + nowData.AddDays(-i).ToString("yyyy-MM-dd") + "',";
            }

            List<BOOK_TIMES> agos = _book.GetStoreData(dateageo.TrimEnd(','), store_id);
            List<BOOK_TIMES> futrs = _book.GetStoreData(dateafutr.TrimEnd(','), store_id);
            List<BOOK_TIMES> nows = _book.GetStoreData(datanow, store_id);

            #region 数据类型改变
            foreach (string item in dateafutr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                string newitem = item.Trim('\'');
                BOOK_TIMES times = futrs.Where(a => a.BOOK_DATE == DateTime.Parse(newitem)).FirstOrDefault();
                if (times == null)
                {
                    futrs.Add(new BOOK_TIMES()
                    {
                        BOOK_DATE = DateTime.Parse(newitem),
                        NUMBER = "0",
                        BOOK_DATE_STR = newitem
                    });
                }
            }

            foreach (string item in dateageo.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                string newitem = item.Trim('\'');
                BOOK_TIMES times = agos.Where(a => a.BOOK_DATE == DateTime.Parse(newitem)).FirstOrDefault();
                if (times == null)
                {
                    agos.Add(new BOOK_TIMES()
                    {
                        BOOK_DATE = DateTime.Parse(newitem),
                        NUMBER = "0",
                        BOOK_DATE_STR = newitem
                    });
                }
            }
            #endregion

            List<BOOK_TIMES> agosone = agos.OrderByDescending(a => a.BOOK_DATE).ToList();
            List<BOOK_TIMES> futrsone = futrs.OrderBy(a => a.BOOK_DATE).ToList();
            BOOK_TIMES modelnow = nows.Count() <= 0 ? new BOOK_TIMES() { BOOK_DATE_STR = nowData.ToString("yyyy-MM-dd"), NUMBER = "0" } :
                nows.FirstOrDefault();

            msg.Data = new
            {
                AgoDate = agosone,
                FutureDate = futrsone,
                NowDate = modelnow
            };

            msg.Status = 1;
            msg.Message = "成功";
            return Json(msg);
        }

        /// <summary>
        /// 显示门店地图
        /// </summary>
        /// <returns></returns>
        public ActionResult ShowStoreMap()
        {
            return View();
        }

        /// <summary>
        /// 获得经纬度
        /// </summary>
        /// <returns></returns>
        public ActionResult GetLocal()
        {
            JsonSMsg msg = new JsonSMsg();

            string lo = Request["LO"];

            string ln = Request["LN"];

            string code = Request["CODE"];

            List<BCJ_CITY> citys = new List<BCJ_CITY>();


            //如果城市编码是空表示顾客自己定位刚进入页面需要加载城市等信息
            if (string.IsNullOrEmpty(code))
            {
                TenAPI model = Utility.GetLocal(lo, ln);
                string citymodel = model.result.formatted_address;

                logger.Info(citymodel + "====" + lo + "===" + ln);

                //citys.AddRange(_book.GetCity());
                citys.AddRange(_book.GetAvalibleCity()); 

                 BCJ_CITY one = citys.Where(a => citymodel.Contains(a.CITY_NAME)).ToList().FirstOrDefault();

                code = one == null ? "110100" : one.CITY_CODE;

            }

            //加载门店列表
            List<BCJ_STORES_EX> stores = _bcjStore.GetStoresByCityCode(code);


            //如果不是定位则计算距离城市中心点的位置
            foreach (BCJ_STORES_EX storeM in stores)
            {
                Poin beigin = new Poin(double.Parse(ln), double.Parse(lo));
                Poin end = new Poin(double.Parse(storeM.LONGITUDE), double.Parse(storeM.LATITUDE));
                storeM.Distance = Utility.GetDistance(beigin, end).ToString();
                storeM.NAME = storeM.NAME.Trim();
            }


            msg.Data = new
            {
                CIRY_CODE = code,
                CIRYS = citys,
                STORES = stores.OrderBy(a => a.Distance)
            };
            return Json(msg);
        }

        /// <summary>
        /// 加载预约日历
        /// </summary>
        /// <returns></returns>
        public ActionResult LoadBook()
        {
            JsonSMsg msg = new JsonSMsg();

            string store_id = Request["store_id"];
            string date = Request["date"];

            List<HMJ_BOOK_EX> book = _book.LoadBook(store_id, date);

            foreach (HMJ_BOOK_EX item in book)
            {
                item.BOOK_DATE_EX = item.BOOK_DATE.Value.ToString("yyyy-MM-dd");
            }

            msg.Status = 1;
            msg.Message = "成功";
            msg.Data = book.OrderByDescending(a => a.BOOK_DATE);
            return Json(msg);
        }

        /// <summary>
        /// 改变预约状态
        /// </summary>
        /// <returns></returns>
        public ActionResult ChageStatus()
        {
            JsonSMsg msg = new JsonSMsg();

            string book_id = Request["book_id"];
            string flg = Request["flg"];

            string whoid = Session["WhoID"].ToString();

            string msgs = _book.ChageBookStatus(book_id, flg, whoid);

            if (msgs != "1")
            {
                msg.Status = 0;
                msg.Message = "";
                return Json(msg);
            }

            msg.Status = 1;
            msg.Message = "";
            return Json(msg);
        }
        
        public ActionResult InitJW()
        {
            return View();
        }

        /// <summary>
        /// 获得经纬度
        /// </summary>
        /// <returns></returns>
        public ActionResult Init_JW()
        {
            JsonSMsg msg = new JsonSMsg();
            _bcjStore.QueryBcjStores();
            //GeocoderResponse ret = _geocoder.GetGeocoder("");
            return Json(msg);
        }

    }
}
