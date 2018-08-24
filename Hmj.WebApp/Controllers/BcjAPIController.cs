using Hmj.Common;
using Hmj.Entity.Entities;
using Hmj.Entity.Jsons;
using Hmj.Interface;
using System;
using System.Web.Mvc;

namespace Hmj.WebApp.Controllers
{
    public class BcjAPIController : Controller
    {
        private IBcjStoreService _bcjStore;

        public BcjAPIController(IBcjStoreService bcjStore)
        {
            _bcjStore = bcjStore;
        }

        /// <summary>
        /// 发送短信模板
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult SendTmp(BCJ_TMP_DETAIL request)
        {
            JsonSMsg msg = new JsonSMsg();
            try
            {
                if (string.IsNullOrEmpty(request.Template_Code))
                {
                    msg.Status = 0;
                    msg.Message = "模板ID不能是空";
                    return Json(msg);
                }

                BasePage bpage = new BasePage();
                string access_token = bpage.MyToken(AppConfig.FWHOriginalID);

                int str = _bcjStore.SendTmp(request, access_token);

                if (str == -1)
                {
                    msg.Status = 0;
                    msg.Message = "没有改模板，请查看模板ID";
                    return Json(msg);
                }

                msg.Status = 1;
                msg.Message = "OK";
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Status = 0;
                msg.Message = ex.Message;
                return Json(msg);
            }
        }
    }
}
