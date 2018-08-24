using Hmj.Business.ServiceImpl;
using Hmj.Common;
using Hmj.Entity;
using Hmj.Entity.Jsons;
using Hmj.Entity.PageSearch;
using Hmj.Extension;
using Hmj.Interface;
using Newtonsoft.Json;
using StructureMap.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using WeChatCRM.Common.Utils;

namespace Hmj.WebApp.Controllers
{
    public class TemplateController : BaseController
    {
        //
        // GET: /Template/
        [SetterProperty]
        public IStoreService StoreService { get; set; }
        [SetterProperty]
        public IEmployeeService EmployeeService { get; set; }
        MySmallShopService mss = new MySmallShopService();
        private ISystemService _sys;
        public TemplateController(ISystemService sys)
        {
            this._sys = sys;
        }

        [WXMyAuthorize]
        public ActionResult TempInfo()
        {
            return View();
        }

        [WXMyAuthorize]
        public ActionResult TempList()
        {
            return View();
        }

        //获取粉丝列表
        [HttpPost]
        public ActionResult QueryFansList(FormCollection form)
        {
             string name = form["EMP_NAME"];
            string sGroupId = form["SelectGroupId"];
            if (string.IsNullOrEmpty(sGroupId))
            {
                sGroupId = "";
            }
         
            PageView view = new PageView(form);
            string colkey = form["colkey"];
            string colsinfo = form["colsinfo"];
            PagedList<WXCUST_FANS> pList = _sys.QueryTemplateFansListT(sGroupId, name, view);
            JsonQTable fdata = JsonQTable.ConvertFromPagedList<WXCUST_FANS>(pList, colkey, colsinfo.Split(','));
            return Json(fdata);
        }

        //获取模板消息列表
        //[HttpPost]
        //public ActionResult QueryTemplateInfoList(FormCollection form)
        //{
        //    string name = form["EMP_NAME1"];
        //    string sGroupId = form["SelectGroupId1"];
        //    if (string.IsNullOrEmpty(sGroupId))
        //    {
        //        sGroupId = "";
        //    }

        //    PageView view = new PageView(form);
        //    string colkey = form["colkey"];
        //    string colsinfo = form["colsinfo"];
        //    PagedList<TEMPLATE_INFO> pList = _sys.QueryTemplateInfoList( view);
        //    JsonQTable fdata = JsonQTable.ConvertFromPagedList<TEMPLATE_INFO>(pList, colkey, colsinfo.Split(','));
        //    return Json(fdata);
        //}

        //获取模板消息
        //[HttpPost]
        //public JsonResult QueryTemplateInfo()
        //{
        //      string id = Request["id"];
          
        //    var  list = _sys.QueryTemplateInfo(int.Parse(id));
          
        //    string fdata = JsonConvert.SerializeObject(list);
        //    return Json(fdata);
        //}

        //获取模板消息日志列表
        //[HttpPost]
        //public ActionResult QueryTemplateLogList(FormCollection form)
        //{
        //    RoleSearch role = new RoleSearch();
        //    string name = form["EMP_NAME"];
        //    string sGroupId = form["SelectGroupId"];
        //    if (string.IsNullOrEmpty(sGroupId))
        //    {
        //        sGroupId = "";
        //    }
        //    role.CName = name;
        //    PageView view = new PageView(form);
        //    string colkey = form["colkey"];
        //    string colsinfo = form["colsinfo"];
        //    PagedList<TEMPLATE_LOG> pList = _sys.QueryTemplateLogList(role,view);
        //    JsonQTable fdata = JsonQTable.ConvertFromPagedList<TEMPLATE_LOG>(pList, colkey, colsinfo.Split(','));
        //    return Json(fdata);
        //}
        
        //获取选项列表
         [HttpPost]
        public JsonResult SelStatusChange()
        {
            string groupid = Request["groupid"];
          
            var  list = StoreService.GetAllGroup();
            var query2 = from n in list
                         where Where(n,groupid)
                         select n;
            string fdata = JsonConvert.SerializeObject(query2);
            return Json(fdata); 
        }
         private bool Where(GROUP_INFO n, string groupid)
         {
             bool iswhere = false;
             foreach (var item in groupid.Split(','))
             {
                 if (string.IsNullOrEmpty(item))
                 {
                     continue;
                 }
                 if (n.PARENT_ID == int.Parse(item))
                 {
                     iswhere = true;
                     continue;
                 }
             }

             return iswhere;
         }

        //发送模板消息
        private JsonSMsg TemplateSend(Template1 text)
        {
            JsonSMsg rMsg = new JsonSMsg();
            OAauth_Log oa = mss.GetOA(text.Openid);
            WeiPage w = new WeiPage();
            string token = w.Token(AppConfig.FWHOriginalID);
            rMsg.Data += "," + token;
            var temp = new
            {
                first = new { value = text.first, color = "#173177" },
                ADA_number = new { value = text.ADA_number, color = "#173177" },
                ADA_name = new { value = text.ADA_name, color = "#173177" },
                ADA_date = new { value = text.ADA_date, color = "#173177" },
                remark = new { value = "\n" + text.remark, color = "#000000" }
            };

  
            string message = SendTemplateMessage(token, text.Openid, text.TemplateID, "#FF0000", temp, "http://www.censh.com", text.first);
            result data = JsonConvert.DeserializeObject<result>(message);
            if (data != null)
            {
                rMsg.Status = data.errcode;
                rMsg.Message = data.errmsg;
            }
            else
            {
                rMsg.Status = -1;
                rMsg.Message = "出现异常";
            }
            return rMsg;
        }

        //push发送
        //public JsonResult PushTemlpate()
        //{
        //    JsonSMsg rMsg = new JsonSMsg();
        //    string groupid = Request["groupid"];
        //    string id = Request["id"];
        //    TEMPLATE_INFO info = _sys.QueryTemplateInfo(int.Parse(id));
        //    List<WXCUST_FANS> fanslist = _sys.QueryTemplateFansList(groupid, "A Running Man");
        //    int s_num = 0;
        //    int f_num = 0;
        //    foreach (var item in fanslist)
        //    {
        //        Template1 text = new Template1();
        //        text.TemplateID = info.TEMPLATE_ID;
        //        text.Openid = item.FROMUSERNAME;
        //        text.first = info.TEMPLATE_NAME;
        //        text.ADA_number = "ku01234685";
        //        text.ADA_name = item.NAME;
        //        text.ADA_date = "2016年12月6号";
        //        text.remark = "可立即购货，自申请成功之日起计60天内，一次性购货满600元，将获得70元折扣优惠";
        //        JsonSMsg msg = TemplateSend(text);
        //        if (msg.Status == 0)
        //        {
        //            s_num++;
        //        }
        //        else
        //        {
        //            f_num++;
        //        }
        //    }
        //    TEMPLATE_LOG templatelog = new TEMPLATE_LOG();
        //    templatelog.NAME = info.TEMPLATE_NAME;
        //    templatelog.TEMPLATE_ID = info.TEMPLATE_ID;
        //    templatelog.URL = "http://www.censh.com";
        //    templatelog.TEMP_TYPE = 1;
        //    templatelog.TAG = "";
        //    templatelog.STATUS = 1;
        //    templatelog.SEND_SUCCESS_NUM = s_num;
        //    templatelog.SEND_FAILED_NUM = f_num;
        //    templatelog.KEWORD = "";
        //    templatelog.REMARK = "";
        //    templatelog.CREATE_TIME = DateTime.Now;
        //    _sys.SaveTemplateLog(templatelog);
        //    rMsg.Status = 0;
        //    rMsg.Message = "发送成功";
        //    return Json(rMsg);
        //}

        //发送模板消息
        public string SendTemplateMessage(string accessToken, string wxOpenID, string tempID, string topColor, object data, string url, string first)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("touser", wxOpenID);
            dict.Add("template_id", tempID.Trim());
            dict.Add("url", url);
            dict.Add("topcolor", "#FF0000");
            dict.Add("data", data);
            return this.DoJSONRequest("cgi-bin/message/template/send?access_token=" + accessToken, dict, first, "POST");
        }

        //序列化json
        private string DoJSONRequest(string path, Dictionary<string, object> data, string first, string method = "POST")
        {
            string strdata = JsonConvert.SerializeObject(data);

            if (!path.Contains("?"))
            {
                path += "?";
            }
            string url = "https://api.weixin.qq.com/" + path;
            string result = NetHelper.HttpRequest(url, strdata, method, 60000, Encoding.UTF8, ContentTypes.JSON);
            try  //保存日志
            {
                WD_TemplateMessageLog log = new WD_TemplateMessageLog();
                log.CreateDate = DateTime.Now;
                log.FromUserName = data["touser"].ToString();
                log.keyword = strdata;
                log.Result = result;
                log.url = url;
                log.First = first;
                mss.SaveTemplateMessagelog(log);
            }
            catch (Exception)
            {

            }
            return result;
        }

    }
    class result
    {
        public int errcode { get; set; }
        public string errmsg { get; set; }
    }

     class Template1
    {
        public string TemplateID { get; set; }
        public string first { set; get; }
        public string keyword1 { get; set; }
        public string keyword2 { get; set; }
        public string ADA_number { get; set; }
        public string ADA_name { get; set; }
        public string ADA_date { get; set; }
        public string remark { get; set; }
        public string result { get; set; }
        public string Openid { get; set; }
    }
}
