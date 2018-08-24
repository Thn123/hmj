using Hmj.Entity;
using Hmj.Entity.Entities;
using Hmj.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace Hmj.WebApp.Controllers
{
    public class SendMilkController : TtBaseController
    {
        ISendMilkService _milk;
        private ISystemService _service;
        private ICommonService _conmmon;
        public SendMilkController(ISystemService sbo, ISendMilkService milk,
            ISystemService ServiceStack, ICommonService conmmon)
            : base(sbo)
        {
            _service = ServiceStack;
            _milk = milk;
            _conmmon = conmmon;
        }

        //
        // GET: /SendMilk/
        [Outh(true)]
        public ActionResult Index()
        {
            //Base();
            //Session["FromUserName"] = "oAWd50qK6bOaprTwyLAn4hvLWDdM";
            //Session["ToUserName"] = "gh_248a60bf062e";

            string openid = Session["FromUserName"] == null ? "" : Session["FromUserName"].ToString();
            if (!string.IsNullOrEmpty(openid))
            {
                WXCUST_FANS cfan = _service.GetFansByFromUserName(openid);
                if (cfan != null)
                {
                    openid = cfan.ID.ToString();
                }
            }

            List<FILE_EX> fils = _service.GetFiilesImage();

            ViewBag.Files = fils;
            ViewBag.OpenId = openid;
            return View();
        }

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <returns></returns>
        public ActionResult Upload()
        {
            try
            {
                HttpPostedFileBase file = Request.Files["filename"];
                string bytes = Request["filename"];


                byte[] bytess = Convert.FromBase64String(bytes.Split(',')[1]);

                string fansId = Request["openid"];

                string bendword = Request["bendword"];
                string bagword = Request["bagword"];
                string smllword = Request["smllword"];

                Random ran = new Random();
                string Extension = ran.Next(100, 999) + ".jpeg";//Path.GetExtension(file.FileName).ToLower();

                Stream st = new MemoryStream(bytess);

                byte[] data = bytess;//new byte[file.InputStream.Length];
                //file.InputStream.Read(data, 0, data.Length);
                string url = Url.Action("../home/ViewImage");

                FILES fileEntity = _conmmon.UploadFile(Extension, "image/jpg",
                    data, url, bendword, bagword, smllword, fansId);

                return Json("");
            }
            catch (Exception ex)
            {

                return Json("");
            }

        }

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <returns></returns>
        public ActionResult Good()
        {
            try
            {
                string FansID = Request["FansID"];
                string FileID = Request["FileID"];

                string str = _conmmon.InsertGood(FansID, FileID);

                return Json("");
            }
            catch (Exception ex)
            {

                return Json("");
            }

        }
    }
}
