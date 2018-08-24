using Hmj.Common;
using Hmj.Entity;
using Hmj.Entity.Entities;
using Hmj.Entity.Jsons;
using Hmj.ExtendAPI.CenSH;
using Hmj.ExtendAPI.WeiXin;
using Hmj.ExtendAPI.WeiXin.Models;
using Hmj.Extension;
using Hmj.Interface;
using Hmj.WebApp.Common.Pages;
using Hmj.WebApp.Models;
using Newtonsoft.Json;
using StructureMap.Attributes;
using System;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace Hmj.WebApp.Controllers
{
    [NoAuthorize]
    public class ApiController : BaseApiController
    {
         
        [SetterProperty]
        public IStoreService StoreService { get; set; }
        [SetterProperty]
        public IEmployeeService EmployeeService { get; set; }

        [NoAuthorize]
        public ActionResult QueryDept(int deptId)
        {
            JsonSMsg rMsg = new JsonSMsg();

            BaseDeptInfo deptInfo = StoreService.GetGroupInfo(deptId);
            if (deptInfo != null)
            {
                rMsg.Status = 1;
                rMsg.Data = deptInfo;
            }
            return Json(rMsg);//, JsonRequestBehavior.AllowGet);
        }
        [NoAuthorize]
        public ActionResult QueryDeptBatch(int deptId)
        {
            JsonSMsg rMsg = new JsonSMsg();

            var list = StoreService.GetAllGroupByID(deptId);
            if (list != null)
            {
                rMsg.Status = 1;
                rMsg.Data = list;
            }

            return Json(rMsg);
        }
        [NoAuthorize]
        [HttpPost]
        public ActionResult CreateDept(DeptInfo deptInfo)
        {
            JsonSMsg rMsg = new JsonSMsg();
            string accessToken = HmjClientServiceApi.Create().GetAccessToken();
            string errMsg = "";
            int groupId = StoreService.CreateDept(deptInfo, accessToken, ref errMsg);
            if (groupId > 0)
            {
                rMsg.Status = 1;
                //deptInfo.ID = groupId;
                rMsg.Data = groupId;
            }
            rMsg.Message = errMsg;


            return Json(rMsg);
        }
        [NoAuthorize]
        [HttpPost]
        public ActionResult UpdateDept(DeptInfo deptInfo)
        {
            JsonSMsg rMsg = new JsonSMsg();

            string accessToken = HmjClientServiceApi.Create().GetAccessToken();

            string errMsg = "";
            int rows = StoreService.ModifyDept(deptInfo, accessToken, ref errMsg);
            if (rows > 0)
            {
                rMsg.Status = 1;
                //rMsg.Data = deptInfo;
            }
            rMsg.Message = errMsg;


            return Json(rMsg);
        }
        [NoAuthorize]
        public ActionResult QueryStore(int storeId)
        {
            JsonSMsg rMsg = new JsonSMsg();

            DeptInfo deptInfo = StoreService.GetDept(storeId, 3);
            if (deptInfo != null)
            {
                MDSearch store = StoreService.GetStoreInfo(deptInfo.GroupID);
                store.ID = storeId;
                store.Name = deptInfo.DeptName;
                store.Code = deptInfo.DeptCode;

                rMsg.Status = 1;
                rMsg.Data = store;
            }

            return Json(rMsg);
        }
        [NoAuthorize]
        public ActionResult QueryStoreBatch(int deptId)
        {
            JsonSMsg rMsg = new JsonSMsg();

            var list = StoreService.GetAllStore(deptId);
            if (list != null)
            {
                rMsg.Status = 1;
                rMsg.Data = list;
            }

            return Json(rMsg);
        }
        [NoAuthorize]
        [HttpPost]
        public ActionResult CreateStore(DeptInfo deptInfo)
        {
            JsonSMsg rMsg = new JsonSMsg();

            string accessToken = HmjClientServiceApi.Create().GetAccessToken();
            string errMsg = "";
            int storeId = StoreService.CreateDept(deptInfo, accessToken, ref errMsg);
            if (storeId > 0)
            {
                rMsg.Status = 1;
                //deptInfo.ID = storeId;
                rMsg.Data = storeId;
            }
            rMsg.Message = errMsg;

            return Json(rMsg);
        }
        [NoAuthorize]
        [HttpPost]
        public ActionResult UpdateStore(DeptInfo deptInfo)
        {
            JsonSMsg rMsg = new JsonSMsg();

            string accessToken = HmjClientServiceApi.Create().GetAccessToken();
            string errMsg = "";
            int rows = StoreService.ModifyDept(deptInfo, accessToken, ref errMsg);
            if (rows > 0)
            {
                rMsg.Status = 1;
                //rMsg.Data = deptInfo;
            }
            rMsg.Message = errMsg;

            return Json(rMsg);
        }
        [NoAuthorize]
        public ActionResult QueryEmp(int empId)
        {
            JsonSMsg rMsg = new JsonSMsg();

            EMPLOYEE_EX empInfo = EmployeeService.GetEmp(empId);
            if (empInfo != null)
            {
                rMsg.Status = 1;
                rMsg.Data = empInfo;
            }

            return Json(rMsg);
        }
        [NoAuthorize]
        public ActionResult QueryEmpBatch(int deptId)
        {
            JsonSMsg rMsg = new JsonSMsg();
            var list = EmployeeService.QueryEmpList(deptId);
            if (list != null)
            {
                rMsg.Status = 1;
                rMsg.Data = list;
            }
            return Json(rMsg);
        }
        [NoAuthorize]
        [HttpPost]
        public ActionResult CreateEmp(EMPLOYEE_MODEL model)
        {
            JsonSMsg rMsg = new JsonSMsg();

            string accessToken = HmjClientServiceApi.Create().GetAccessToken();

            WeiPage wp = new WeiPage();
            string mpToken = wp.Token(AppConfig.FWHOriginalID);
            int ewmId = SystemService.GetEwmId() + 1;
            model.EwmId = ewmId;
            QRCodeResponse qrCodeResponse = WXMPClientServiceApi.Create().CreateQRCode(mpToken, ewmId);
            if (qrCodeResponse != null)
            {
                model.EwmUrl = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=" + qrCodeResponse.Ticket;
            }
            DeptInfo dept = new DeptInfo();
            dept.ID = model.EmpGroupId;
            string errMsg = null;
            int rows = EmployeeService.SaveEmployee(accessToken, model, dept, ref errMsg);
            if (rows > 0)
            {
                rMsg.Status = 1;
                rMsg.Data = rows;
            }
            else
            {
                if (string.IsNullOrEmpty(errMsg))
                {
                    errMsg = "保存失败。";
                }
                rMsg.Message = errMsg;
            }

            return Json(rMsg);
        }
        [NoAuthorize]
        [HttpPost]
        public ActionResult UpdateEmp(EMPLOYEE_MODEL model)
        {
            JsonSMsg rMsg = new JsonSMsg();

            string accessToken = HmjClientServiceApi.Create().GetAccessToken();

            DeptInfo dept = new DeptInfo();
            dept.ID = model.EmpGroupId;
            string errMsg = null;
            int rows = EmployeeService.SaveEmployee(accessToken, model, dept, ref errMsg);
            if (rows > 0)
            {
                rMsg.Status = 1;
            }
            else
            {
                if (string.IsNullOrEmpty(errMsg))
                {
                    errMsg = "保存失败。";
                }
                rMsg.Message = errMsg;
            }

            return Json(rMsg);
        }


        [NoAuthorize]
        public ActionResult SyncQrCode()
        {
            WeiPage wp = new WeiPage();
            string mpToken = wp.Token(AppConfig.FWHOriginalID);

            var list = EmployeeService.QueryAllEmpWithNoQrCode(mpToken);
            if (list != null && list.Count > 0)
            {

                return Content("更新二维码成功");
            }
            else
            {
                return Content("没有数据需要同步");
            }
        }
        [NoAuthorize]
        public ActionResult InitDept()
        {
            //string accessToken = CenSHClientServiceApi.Create().GetAccessToken();

            //StoreService.InitDepartments(accessToken);
            return Content("初始化组织架构数据成功。");
        }
        [NoAuthorize]
        public ActionResult SyncDeptToRemote()
        {
            //string accessToken = CenSHClientServiceApi.Create().GetAccessToken();

            //StoreService.SyncDeptToRemote(accessToken);
            return Content("同步本地组织架构至企业号后台。");
        }
        [NoAuthorize]
        public ActionResult SyncEmp()
        {
            string accessToken = HmjClientServiceApi.Create().GetAccessToken();

            StoreService.SyncUsersToLocal(accessToken);
            return Content("同步员工数据成功。");
        }
        [NoAuthorize]
        public ActionResult GetQYToken()
        {
            string accessToken = AccessTokenGenerator.Instance.GetQYAccessToken();
            return Content(accessToken);
        }
        [NoAuthorize]
        public ActionResult GenerateKey()
        {

            string guid = Guid.NewGuid().ToString();
            string appId = CryptographyUtils.SHA1(guid);
            string appSecret = CryptographyUtils.DesEncrypt(appId);
            //string appSecret = SHA256ManagedUtils.GetHashedPassword(guid, appId);

            return Content(string.Format("AppID：{0}<br>AppSecret：{1}", appId, appSecret));
        }
        //[NoAuthorize]
        //public ActionResult UpdateDeptOrg(int deptId, int parentId)
        //{
        //    string errMsg = "";
        //    string accessToken = AccessTokenGenerator.Instance.GetQYAccessToken();
        //    int rows = StoreService.UpdateDeptOrg(accessToken, deptId, parentId, ref errMsg);
        //    if (rows > 0)
        //    {
        //        return Content("修改成功");
        //    }
        //    else
        //    {
        //        return Content(errMsg);
        //    }
        //}
        [NoAuthorize]
        public ActionResult Ewm()
        {
            WeiPage wp = new WeiPage();
            string mpToken = wp.Token(AppConfig.FWHOriginalID);
            int ewmId = SystemService.GetEwmId() + 1;

            string url = "";
            QRCodeResponse qrCodeResponse = WXMPClientServiceApi.Create().CreateQRCode(mpToken, ewmId);
            if (qrCodeResponse != null)
            {
                url = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=" + qrCodeResponse.Ticket;
            }

            return Content(string.Format("EwmId：{0}，EwmUrl：{1}", ewmId, url));
        }
        [NoAuthorize]
        public ActionResult CreateEwm(int id)
        {
            WeiPage wp = new WeiPage();
            string mpToken = wp.Token(AppConfig.FWHOriginalID);

            string url = "";
            QRCodeResponse qrCodeResponse = WXMPClientServiceApi.Create().CreateQRCode(mpToken, id);
            if (qrCodeResponse != null)
            {
                url = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=" + qrCodeResponse.Ticket;
            }

            return Content(string.Format("EwmId：{0}，EwmUrl：{1}", id, url));
        }

        /// <summary>
        /// 发送模板消息(用于对接)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult TemplateSend()
        {
            JsonSMsg rMsg = new JsonSMsg();
            string json = string.Empty;
            string responjson = string.Empty;
            HttpRequest request = HttpContext.ApplicationInstance.Context.Request;
            Stream stream = request.InputStream;
            if (stream.Length > 0)
            {
                StreamReader reader = new StreamReader(stream);
                json = reader.ReadToEnd();
            }
            responjson = WXMPClientServiceApi.Create().SendTemplateMsg(new WeiPage().Token(AppConfig.FWHOriginalID), json);
            if (responjson.Length > 0)
            {
                rMsg.Status = 1;
                rMsg.Message = "成功";
                rMsg.Data = responjson;
            }
            else
            {
                rMsg.Status = 0;
                rMsg.Message = "失败";
            }
            return Json(rMsg);
        }
        /// <summary>
        /// 获取code(用于第三方授权)弃用
        /// </summary>
        /// <returns></returns>
        [NoAuthorize]
        public ActionResult GetCode()
        {

            string json = string.Empty;
            string responjson = string.Empty;
            string errorMsg = "";
            string appid_s = Request["appid"];
            string timestamp = Request["timestamp"];
            string sign = Request["sign"];
            string url = Request["url"];
            if (ApiAuthService.Auth(appid_s, timestamp, sign, HttpContext.ApplicationInstance.Context.Request.UserHostAddress, out errorMsg))
            {
                string code = Request["code"];
                if (url.Contains("?"))
                {
                    Response.Redirect(url + "&code=" + code);
                }
                else
                {
                    Response.Redirect(url + "?code=" + code);
                }

                return Content("{ \"errcode\":0, \"errmsg\":\"ok\" }");
            }
            else
            {
                return Content("{ \"errcode\":1, \"errmsg\":\"" + errorMsg + "\" }");
            }
        }

        [NoAuthorize]
        public ActionResult GetUrl()
        {

            //string json = string.Empty;
            //string responjson = string.Empty;
            //string errorMsg = "";
            string appid_s = Request["url"];
            string timestamp = Request["scope"];
            if (Request["url"] != null && Request["scope"] != null)
            {
                string url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + GetAppid(AppConfig.FWHOriginalID) + "&redirect_uri=http://wechat.censh.com/Api/GetUrl.do?url2=" + Request["url"] + "&response_type=code&scope=" + Request["scope"] + "&state=STATE#wechat_redirect";
                Response.Redirect(url);
            }
            if (Request["url2"] != null)
            {
                if (Request["code"] != null)
                {
                    try
                    {
                        string url = string.Format(@"https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + GetAppid(AppConfig.FWHOriginalID) + "&secret=" + GetSecret(AppConfig.FWHOriginalID) + "&code={0}&grant_type=authorization_code", Request["code"].ToString());
                        string token = PostRequest(url);
                        if (token.Contains("7200"))
                        {
                            string[] b = token.Split('\"');
                            if (b.Length >= 15)
                            {
                                //try
                                //{
                                //    string u = "https://api.weixin.qq.com/sns/userinfo?access_token=" + b[3] + "&openid=" + b[13] + "&lang=zh_CN";
                                //    string x = PostRequest(u);
                                //    string[] y = x.Split('\"');
                                //    if (y.Length >= 30)
                                //    {
                                //        LogService.Info("Name:" + y[7] + ",openid:" + b[13] + ",Get API," + x);
                                //    }
                                //}
                                //catch (Exception)
                                //{

                                //}

                                if (Request["url2"].Contains("?"))
                                {
                                    Response.Redirect(Request["url2"] + "&access_token=" + b[3] + "&openid=" + b[13]);
                                    //if (Request["url2"].Contains("&"))
                                    //{

                                    //}
                                }
                                else
                                {
                                    Response.Redirect(Request["url2"] + "?access_token=" + b[3] + "&openid=" + b[13]);
                                }
                            }

                        }
                    }
                    catch (Exception)
                    {

                    }
                }

            }

            return Content("{ \"errcode\":0, \"errmsg\":\"ok\" }");

        }

        [HttpPost]
        public ActionResult SendImage()
        {
            JsonSMsg rMsg = new JsonSMsg();
            try
            {
                int ar_id = SystemService.GetMaxArid() + 1;
                string file_path = "";
                string expire = AppConfig.ExpireAr;//该二维码有效时间，以秒为单位。 最大不超过2592000（即30天），此字段如果不填，则默认有效期为30秒。
                AR_QR_FANS ar = new AR_QR_FANS();
                //获取图片流存到本地
                file_path = GetImageStreem();
                //上传本地图片到微信服务器

                string url = string.Format("http://file.api.weixin.qq.com/cgi-bin/media/upload?access_token={0}&type={1}", new WeiPage().Token(AppConfig.FWHOriginalID), "image");
                string json = Utility.HttpUploadFile(url, file_path);
                mediainfo media = JsonConvert.DeserializeObject<mediainfo>(json);
                //删除临时图片
                if (media != null)
                {
                    System.IO.File.Delete(file_path);
                }
                //获取临时二维码
                QRCodeResponse respons = WXMPClientServiceApi.Create().CreateTempQRCode(new WeiPage().Token(AppConfig.FWHOriginalID), ar_id, expire);
                string qr_url = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=" + respons.Ticket;

                //保存到数据库
                //AR_QR_FANS arinfo = SystemService.QueryArInfoByArId(ar_id);
                //if (arinfo != null)
                //{
                //    ar = arinfo;
                //}
                ar.AR_ID = ar_id;
                ar.AR_URL = qr_url;
                ar.MEDIA_ID = media.media_id;
                ar.EXPIRE_SECONDS = int.Parse(expire);
                ar.CREATE_DATE = DateTime.Now;

                int res = SystemService.SaveArInfo(ar);
                if (res > 0)
                {
                    arinfo ars = new arinfo();
                    ars.id = res;
                    ars.url = qr_url;
                    rMsg.Status = 1;
                    rMsg.Message = "成功";
                    rMsg.Data = res + "|" + qr_url;
                }
                else
                {
                    rMsg.Status = 0;
                    rMsg.Message = "失败";
                }
            }
            catch (Exception ex)
            {
                rMsg.Status = 0;
                rMsg.Message = "SendImage报错：" + ex.Message;
            }
            return Json(rMsg);
        }

        [NoAuthorize]
        public int IsExpire(int ar_id)
        {
            int return_id = 0;
            AR_QR_FANS arinfo = SystemService.QueryArInfoByArId(ar_id);
            if (arinfo != null)
            {
                DateTime t0 = arinfo.CREATE_DATE.Value;
                DateTime t1 = DateTime.Now;

                // 求时间差
                TimeSpan ts = t1 - t0;
                if (arinfo.EXPIRE_SECONDS < ts.TotalSeconds)//如果已过期才能用
                {
                    //删除过期的记录
                    int des = SystemService.DeleteAr(arinfo.ID);


                }
            }
            return return_id;

        }

        [NoAuthorize]
        public string GetImageStreem()
        {
            string filePath = string.Concat(AppConfig.UploadWXAR, DateTime.Now.ToString("yyyyMMddhhmmss") + "ar.jpg");
            Bitmap img = new Bitmap(HttpContext.Request.InputStream);
            img.Save(filePath);
            return filePath;
        }

        [HttpGet]
        public ActionResult GetOpenid(int id)
        {
            JsonSMsg rMsg = new JsonSMsg();
            string openid = "1111";
            //获取openid
            AR_QR_FANS ar = SystemService.QueryArInfo(id);
            if (ar != null)
            {
                openid = ar.OPENID;
                rMsg.Status = 1;
                rMsg.Message = "成功";
                rMsg.Data = openid;
            }
            else
            {
                rMsg.Status = 0;
                rMsg.Message = "失败";
            }
            return Json(rMsg);
        }
    }

    public class arinfo
    {
        public int id { get; set; }
        public string url { get; set; }
    }
}
