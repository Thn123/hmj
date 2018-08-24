using Hmj.Entity.Entities;
using Hmj.Entity.Jsons;
using Hmj.Entity.PageSearch;
using Hmj.Entity.SearchEntity;
using Hmj.Extension;
using Hmj.Interface;
using System.Web.Mvc;

namespace Hmj.WebApp.Controllers
{
    public class MessageRecordController : WXMyControllerBase
    {
        //
        // GET: /Message/
        private IWXMessageRecordService _mrs;
        private ISystemService _ss;
        public MessageRecordController(IWXMessageRecordService mrs, ISystemService ss)
        {
            _mrs = mrs;
            _ss = ss;
        }

        public ActionResult Index()
        {
            return View(new CUST_MSG_RECORD_EX());
        }

        [HttpPost]
        public JsonResult QueryFansMessages(FormCollection form)
        {
            PageView view = new PageView(form);
            string colkey = form["colkey"];
            string colsinfo = form["colsinfo"];

            FansMessageSearch fms = new FansMessageSearch();
            fms.ToUserName = WXMyContext.CurrentMerchants.ToUserName;
            if (!string.IsNullOrEmpty(form["SEARCHTYPE"]))
            {
                switch (form["SEARCHTYPE"])
                {
                    case "0":
                        break;
                    case "1":
                        fms.IsReturn = false;
                        break;
                    case "2":
                        fms.IsReturn = true;
                        break;
                    case "3":
                        fms.IsStar = true;
                        break;
                }
            }

            if (!string.IsNullOrEmpty(form["SearchText"]))
            {
                fms.SearchText = form["SearchText"];
            }

            PagedList<CUST_MSG_RECORD_EX> pList = _mrs.QueryFansMessages(view, fms);
            JsonQTable fdata = JsonQTable.ConvertFromPagedList<CUST_MSG_RECORD_EX>(pList, colkey, colsinfo.Split(','));
            return Json(fdata);
        }

        //[HttpPost]
        //public JsonResult GetFansMessage(int id)
        //{
        //    JsonSMsg rMsg = new JsonSMsg();
        //    var message = _CustMsgHisService.GetFansMessage(id);
        //    if (message != null)
        //    {
        //        rMsg.Status = 0;
        //        rMsg.Message = "success";
        //        rMsg.Data = message;
        //    }
        //    else
        //    {
        //        rMsg.Status = -1;
        //        rMsg.Message = "fail";
        //    }
        //    return Json(rMsg);
        //}

        //[HttpPost]
        //public JsonResult UpdateFanMessages(int id, CUST_MSG_HIS_EX fanMessage)
        //{
        //    JsonSMsg rMsg = new JsonSMsg();
        //    try
        //    {
        //        if (id <= 0)
        //        {
        //            rMsg.Status = -1;
        //            rMsg.Message = "参数错误！";
        //            return Json(rMsg);
        //        }
        //        int ret = _CustMsgHisService.UpdateFansMessageIsStar(fanMessage);
        //        if (ret > 0)
        //        {
        //            rMsg.Status = 1;
        //            rMsg.Data = ret;
        //        }
        //        else
        //        {
        //            rMsg.Status = -1;
        //            rMsg.Message = "意外错误，请稍后重试";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        rMsg.Status = -1;
        //        rMsg.Message = ex.Message;
        //    }
        //    return Json(rMsg);
        //}

        //[HttpPost]
        //public JsonResult ReplyMessage(CUST_MSG_HIS_EX fanMessage)
        //{
        //    JsonSMsg rMsg = new JsonSMsg();
        //    SendMessageApi sma = new SendMessageApi();
        //    var result = sma.SendMessage(fanMessage.FROMUSERNAME, fanMessage.Return_Con);
        //    rMsg.Status = result.errcode;
        //    rMsg.Message = result.errmsg;

        //    if (result.errcode == 0)
        //    {
        //        _CustMsgHisService.ReplyMessage(fanMessage);
        //    }
        //    return Json(rMsg);
        //}
    }
}
