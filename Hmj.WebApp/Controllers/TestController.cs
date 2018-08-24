using Hmj.Common;
using Hmj.DTO;
using Hmj.Entity.WxModel;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;

namespace Hmj.WebApp.Controllers
{
    public class TestController : Controller
    {
        //
        // GET: /Test/

        public ActionResult Index()
        {
            // WXBookingService wrs = new WXBookingService();
            //var bookings = wrs.WXQueryBookingList("oosSLjjQJ4Wbz7wZTv9f-Yit2_mw", new DateTime(DateTime.Now.Year, 1, 1), DateTime.Now.AddDays(60), 24);
            // var a = wrs.GetFreeEmployees(37, DateTime.Now.Date.AddHours(17), DateTime.Now.Date.AddHours(18), null);


            return View();
        }

        ///// <summary>
        ///// 显示注册页面
        ///// </summary>
        ///// <returns></returns>
        //[Outh(true)]
        //public ActionResult Index()
        //{
        //    try
        //    {
        //        FansInfo Fans = Session["FansInfo"] as FansInfo;
        //        Dictionary<string, string> dic = new Dictionary<string, string>();
        //        //logger.Fatal(Fans.Openid);
        //        dic.Add("openid", Fans.Openid);

        //        JsonDTO<bool> result = RequestHelp.RequestGet<bool>("Test/LQCoupon", dic).Result;
        //        string json = JsonConvert.SerializeObject(Fans);



        //        return View();
        //    }
        //    catch (System.Exception)
        //    {
        //        throw;
        //    }

        //}


    }
}
