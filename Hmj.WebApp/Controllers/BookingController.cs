using Hmj.Entity;
using Hmj.Entity.Jsons;
using Hmj.Entity.PageSearch;
using Hmj.Entity.SearchEntity;
using Hmj.Extension;
using Hmj.Interface;
using Hmj.WebApp.POSApiHelper;
using System.Web.Mvc;

namespace WeChat.WebApp.Controllers
{
    public class BookingController : WXMyControllerBase
    {
        private IStoreService _ss;
        private IMySmallShopService _spservice;
        public BookingController(IStoreService ss, IMySmallShopService spservice)
        {
            _ss = ss;
            _spservice = spservice;
        }

        public ActionResult Index()
        {
            return View();
        }

        [MyAuthorize]
        public ActionResult StoreList()
        {
            return View(new WXStore() { ID = 0 });
        }

        public JsonResult ImportStores()
        {
            StoreApi sa = new StoreApi();
            JsonSMsg rMsg = new JsonSMsg();
            try
            {
                var result = sa.BindStore(7);//值需要获取
                rMsg.Status = 0;
                rMsg.Message = "success";
                rMsg.Data = result;
            }
            catch
            {
                rMsg.Status = -1;
                rMsg.Message = "fail";
            }
            return Json(rMsg);
        }

        [HttpPost]
        public JsonResult QueryStoreList(FormCollection form)
        {
            string colkey = form["colkey"];
            string colsinfo = form["colsinfo"];
            var stores = _ss.GetStores(CurrentMerchants.ID);
            JsonQTable fdata = JsonQTable.ConvertFromList<WXStore>(stores, colkey, colsinfo.Split(','));
            return Json(fdata);
        }

        public JsonResult GetStore(int id)
        {
            var store = _ss.GetStore(id);
            JsonSMsg rMsg = new JsonSMsg();
            if (store != null)
            {
                rMsg.Status = 0;
                rMsg.Message = "success";
                rMsg.Data = store;
            }
            else
            {
                rMsg.Status = -1;
                rMsg.Message = "fail";
            }
            return Json(rMsg);
        }

        public JsonResult SaveStore(FormCollection form, WXStore s)
        {
            if (form["IsDisplay"] == "1")
            {
                s.IsDisplay = true;
            }
            else if (form["IsDisplay"] == "0")
            {
                s.IsDisplay = false;
            }

            var id = _ss.UpdateStore(s);
            JsonSMsg rMsg = new JsonSMsg();
            if (id > 0)
            {
                rMsg.Status = 0;
                rMsg.Message = "success";
            }
            else
            {
                rMsg.Status = -1;
                rMsg.Message = "fail";
            }
            return Json(rMsg);
        }

        public ActionResult BookingList()
        {
            return View();
        }

        //列表
        [HttpPost]
        public JsonResult GetBookingList(RoleSearch search, FormCollection form)
        {
            PageView view = new PageView(form);
            if (search == null)
            {
                search = new RoleSearch();
            }
            search.ORG_ID = base.CurrentLoginUser.ID;
            string colkey = form["colkey"];
            string colsinfo = form["colsinfo"];
            PagedList<WD_BOOKING> pList = _spservice.GetBookingListByMid(CurrentMerchants.ID, view);
            JsonQTable fdata = JsonQTable.ConvertFromPagedList<WD_BOOKING>(pList, colkey, colsinfo.Split(','));
            return Json(fdata);
        }
    }
}
