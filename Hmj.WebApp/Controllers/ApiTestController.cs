using HmjNew.Service;
using Hmj.Business;
using Hmj.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hmj.WebApp.Controllers
{
    public class ApiTestController : Controller
    {
        //
        // GET: /ApiTest/

        public ActionResult Index()
        {
            dt_Dyn_CreateHMJMemberShip_req req = new dt_Dyn_CreateHMJMemberShip_req();
            ZCRMT316_HMJ meber = new ZCRMT316_HMJ();
            meber.MOB_NUMBER = "18954152347";
            meber.OPENID = "sdjflajslkfd";
            meber.NAME1_TEXT = "苗玉凯";//全名
            meber.DATA_SOURCE = "0002";
            meber.ACCOUNT_ID = "18954152347";
            meber.NAME_LAST = "苗";
            meber.NAME_FIRST = "玉凯";
            meber.XSEX = "1";
            meber.REGION = "";
            meber.BIRTHDT = DateTime.Now.ToString();
            meber.NAMCOUNTRY = "CN";
            meber.WECHATNAME = "非";
            meber.WECHATFOLLOWSTATUS = "1";
            //meber.LOGINPASS2 = "111111";//兑换密码默认123456
            meber.LOYALTY_BRAND = AppConfig.LOYALTY_BRAND;//忠诚度品牌
            meber.SOURCE_SYSTEM = AppConfig.SOURCE_SYSTEM;//来源系统
            meber.VGROUP = AppConfig.VGROUP; //销售组织

            //固定死
            meber.EMPID = AppConfig.EMPID;
            meber.DEPTID = AppConfig.DEPTID;

            req.ZCRMT316 = new ZCRMT316_HMJ[] { meber };

            //创建会员
            dt_Dyn_CreateHMJMemberShip_res res = WebHmjApiHelp.CreateMemberShip(req);

            return View();
        }

    }
}
