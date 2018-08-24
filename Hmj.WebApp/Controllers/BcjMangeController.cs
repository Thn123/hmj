using Hmj.Entity.Entities;
using Hmj.Entity.Jsons;
using Hmj.Entity.PageSearch;
using Hmj.Entity.SearchEntity;
using Hmj.Extension;
using Hmj.Interface;
using System;
using System.Web.Mvc;

namespace Hmj.WebApp.Controllers
{
    public class BcjMangeController : WXMyControllerBase
    {
        private IBcjStoreService _bcjStore;
        private ICustMemberService _custMember;

        public BcjMangeController(IBcjStoreService bcjStore, ICustMemberService custMember)
        {
            _bcjStore = bcjStore;
            _custMember = custMember;
        }

        /// <summary>
        /// 显示问卷分组列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 显示添加积分页面
        /// </summary>
        /// <returns></returns>
        public ActionResult AddPoint()
        {
            return View();
        }

        /// <summary>
        /// 显示添加积分页面
        /// </summary>
        /// <returns></returns>
        public ActionResult SendToPiod()
        {
            JsonSMsg msg = new JsonSMsg();

            string mobile = Request["mobile"];
            string type = Request["type"];
            string point = Request["point"];

            if (string.IsNullOrEmpty(mobile))
            {
                msg.Message = "对不起，请输入手机号。";
                msg.Status = 0;
                return Json(msg);
            }

            if (string.IsNullOrEmpty(point))
            {
                msg.Message = "对不起，请输入积分。";
                msg.Status = 0;
                return Json(msg);
            }

            string bo = _custMember.ReduceOrAddPiod(mobile, point, type);


            msg.Status = 1;
            msg.Data = bo;
            return Json(msg);
        }

        [HttpPost]
        public JsonResult GetGroupLists(GroupSearch search, FormCollection form)
        {
            PageView view = new PageView(form);
            if (search == null)
            {
                search = new GroupSearch();
            }

            string colkey = form["colkey"];
            string colsinfo = form["colsinfo"];
            PagedList<SURREY_GROUP_EX> pList = _custMember.QueryGetGroups(search, view);
            JsonQTable fdata = JsonQTable.ConvertFromPagedList<SURREY_GROUP_EX>(pList, colkey, colsinfo.Split(','));
            return Json(fdata);
        }

        /// <summary>
        /// 佰草集报表
        /// </summary>
        /// <returns></returns>
        public ActionResult ShowBcjExcel()
        {
            return View();
        }

        /// <summary>
        /// 得到每个门店关注或者绑定的数量
        /// </summary>
        /// <param name="search"></param>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetStores(ExcelSearch search, FormCollection form)
        {
            PageView view = new PageView(form);
            try
            {
                
                if (search == null)
                {
                    search = new ExcelSearch();
                }

                string colkey = form["colkey"];
                string colsinfo = form["colsinfo"];
                
                PagedList<STORES_EXCEL> pList = _bcjStore.QueryExcels(search, view);
                JsonQTable fdata = JsonQTable.ConvertFromPagedList<STORES_EXCEL>(pList, colkey, colsinfo.Split(','));
                return Json(fdata);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

        /// <summary>
        /// 得到门店的粉丝或者绑定的详细信息
        /// </summary>
        /// <param name="search"></param>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetDetail(ExcelSearch search, FormCollection form)
        {
            PageView view = new PageView(form);
            try
            {

                if (search == null)
                {
                    search = new ExcelSearch();
                }

                string colkey = form["colkey"];
                string colsinfo = form["colsinfo"];

                PagedList<STORES_EXCEL> pList = _bcjStore.QueryExcels(search, view);
                JsonQTable fdata = JsonQTable.ConvertFromPagedList<STORES_EXCEL>(pList, colkey, colsinfo.Split(','));
                return Json(fdata);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}
